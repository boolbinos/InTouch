import { Injectable, Inject, EventEmitter } from '@angular/core';
import { Headers, Http } from '@angular/http';
import { Router } from '@angular/router';
import { HttpInterceptorService } from './http-interceptor-service';
import 'rxjs/add/operator/toPromise';

import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { FileUploader } from 'ng2-file-upload';

import { PostView } from '../app/components/post/shared/postView';
import { FileView } from '../app/components/post/shared/fileView';
import { UserView } from '../app/components/user/shared/userView';
import { CommentView } from '../app/components/comment/shared/commentView';
import { UserLikeView } from "../app/components/user/shared/userLikeView";
import { PagedResult } from '../app/components/post/shared/pagedResult';
import { PostCreate } from '../app/components/createpost/shared/postCreate';
import { PostEdit } from '../app/components/editpost/shared/postEdit';

import { UserService } from '../services/user.service';
import { ToasterService } from '../services/toaster.service';
import { AccessService } from '../services/Access.service';


@Injectable()
export class PostService {
    public setSearchStr$: Observable<string>;
    public setSearchStrSubject = new Subject<string>();

    public setIsMyPosts$: Observable<boolean>;
    public setIsMyPostsSubject = new Subject<boolean>();



    private headers = new Headers({ 'Content-Type': 'application/json' });
    private postsUrl = 'api/Posts';
    private likesUrl = 'api/Likes';
    private durationFromConfig = 'getDefaultPostDuration';
    private postEndDateFromConfig = 'getDefaultPostEndDate';

    private importUrl: string = 'api/posts/Import';

    private fileUploadUrl: string = 'api/Files';
    private ImagesUploadUrl: string = 'api/Files';

    public uploader: FileUploader;
    public facebookImport: EventEmitter<number> = new EventEmitter();
    public currentUser: UserView;
    public userService: UserService;

    public search: string = undefined;
    public isMyPosts: boolean = false;

    constructor(
        @Inject('BASE_URL') private baseUrl: string,
        private http: HttpInterceptorService,
        private router: Router, userService: UserService,
        private toasterService: ToasterService,
        private access: AccessService) {

        var options = {
            url: this.fileUploadUrl,
            autoUpload: true,
            allowedMimeType: ['image/jpeg', 'image/jpg', 'image/png'],
            maxFileSize: 10485760
        };

        userService.getCurrentUser().then(u => this.currentUser = u);
        this.uploader = new FileUploader(options);

        this.setSearchStr$ = this.setSearchStrSubject.asObservable();
        this.setSearchStr$.subscribe(
            (search) => this.search = search
        );

        this.setIsMyPosts$ = this.setIsMyPostsSubject.asObservable();
        this.setIsMyPosts$.subscribe(
            
            (isMyPosts) => { this.isMyPosts = isMyPosts; }
        );
    }

    setSearchStr(search: string) {
        if (search === '') {
            search = undefined;
        }
        this.setSearchStrSubject.next(search);
    }

    getSearchStr() {
        return this.search;
    }

    getIsMyPosts() {
        return this.isMyPosts;
    }

    setIsMyPosts(isMyPosts: boolean) {
        this.setIsMyPostsSubject.next(isMyPosts);
    }

    setIsMyPostsWithoutObservable(isMyPosts: boolean) {
        this.isMyPosts = isMyPosts;
    }

    getStatus(post: PostView): boolean {
        return post.isPublic;
    }

    getPost(id: number): Promise<PostView> {
        return new Promise(resolve => {
            this.http.get(this.baseUrl + this.postsUrl + "/" + id).subscribe(result => {
                resolve(result.json() as PostView);
            },
                error => {
                });
        });
    }

    getEditPostModel(id: number): Promise<PostEdit> {
        return new Promise(resolve => {
            this.http.get(this.baseUrl + this.postsUrl + "/GetPostEdit/" + id).subscribe(result => {
                resolve(result.json() as PostEdit);
            },
                error => {
                });
        });
    }

    getLikersName(likes: Array<UserLikeView>): string {
        let names = '';
        let end = '';
        if (likes.length > 10) {
            likes.length = 10;
            end = '...';
        }
        likes.forEach(u => {
            if (u.id !== this.currentUser.id) {
                names += ' ' + u.fullName + ',';
            }
        });
        if (names !== '' && names.length > 0) {
            names = names.substring(0, names.length - 1) + end+' liked it';
            return names;
        }
        names = '...';
        return names;
    }

    public getPosts(page: number, pageSize: number, isMyPosts: boolean, SearchStr: string): Promise<PagedResult<PostView>> {
        if (SearchStr != undefined) {
            return new Promise(resolve => {
                this.http.get(this.baseUrl + this.postsUrl + "?pageNum=" + page + "&pageSize=" + pageSize + "&JustMyPosts=" + this.isMyPosts + "&SearchStr=" + SearchStr).subscribe(result => {
                    resolve(result.json() as PagedResult<PostView>);
                },
                    error => console.error(error));
            });
        }
        else {
            return new Promise(resolve => {
                this.http.get(this.baseUrl + this.postsUrl + "?pageNum=" + page + "&pageSize=" + pageSize + "&JustMyPosts=" + this.isMyPosts).subscribe(result => {
                    resolve(result.json() as PagedResult<PostView>);
                },
                    error => console.error(error));
            });
        }

    }

    public GetImages(id: string): string {
        return this.baseUrl + this.ImagesUploadUrl + "/" + id;
    }

    createPost(createPost: PostCreate) {
        return new Promise((resolve, reject) => {

            this.http.post(this.baseUrl + this.postsUrl, createPost, { headers: this.headers })
                .toPromise()
                .then(res => { // Success
                    var result = res.json() as PostView;
                    resolve(result);
                },
                msg => { // Error
                    reject(msg);
                });
        });
    }

    editPost(id: number, editPost: PostEdit) {
        return new Promise((resolve, reject) => {
            this.http.put(this.baseUrl + this.postsUrl + '/' + id, editPost)
                .toPromise()
                .then(res => { // Success
                    var result = res.json() as PostView;
                    resolve(result);
                },
                msg => { // Error
                    reject(msg);
                });
        });
    }

    deletePost(id: number) {
        return new Promise((resolve, reject) => {
            this.http.delete(this.baseUrl + this.postsUrl + "/" + id,
                { headers: this.headers })
                .toPromise()
                .then(res => { // Success
                    resolve("/posts");
                },
                msg => { // Error
                    reject(msg);
                });
        });
    }

    doPublic(postId: number) {
        return new Promise<boolean>((resolve, reject) => {
            if (this.currentUser.roleName === "Moderator") {
                this.http.put(this.baseUrl + this.postsUrl + '/' + postId + "/public",
                    { headers: this.headers })
                    .toPromise()
                    .then(res => { // Success
                        var result = res.json() as boolean;
                        resolve(result);
                    },
                    msg => { // Error
                        reject(msg);
                    });
            }
        });
    }

    doPrivate(postId: number) {
        return new Promise<boolean>((resolve, reject) => {
            if (this.currentUser.roleName === "Moderator") {
                this.http.put(this.baseUrl + this.postsUrl + '/' + postId + "/private",
                    { headers: this.headers })
                    .toPromise()
                    .then(res => { // Success
                        var result = res.json() as boolean;
                        resolve(result);
                    },
                    msg => { // Error
                        reject(msg);
                    });
            }
        });
    }

    like(likes: Array<UserLikeView>, postId: number) {
        return new Promise<boolean>((resolve, reject) => {
            this.http.post(this.baseUrl + this.likesUrl,
                postId,
                { headers: this.headers })
                .toPromise()
                .then(res => { // Success
                    var result = res.json() as boolean;
                    if (result) {
                        if (!this.isPostLiked(likes)) {
                            likes.push(new UserLikeView(this.currentUser.id, this.currentUser.firstName + ' ' + this.currentUser.lastName));
                        } else {
                            var userIndex = likes.findIndex(u => u.id == this.currentUser.id);
                            likes.splice(userIndex, 1);
                        }
                    }
                    resolve(result);
                },
                msg => { // Error
                    reject(msg);
                });
        });
    }

    isPostLiked(likes: Array<UserLikeView>): boolean {
        var idn = likes.findIndex(u => u.id === this.currentUser.id);
        if (idn != -1) {
            return true;
        } else {
            return false;
        }
    }

    getLikesCount(post: PostView): number {
        var count = post.usersLikes.length;
        return count;
    }

    importPosts(providerName: string) {
        return new Promise((resolve, reject) => {

            this.http.post(this.baseUrl + this.importUrl + "/" + providerName,
                { headers: this.headers })
                .toPromise()
                .then(res => { // Success
                    var result = res.json() as number;
                    this.facebookImport.emit(result);
                    resolve(result);
                },
                msg => { // Error
                    reject(msg);
                });
        });
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
    }

    checkAccess(postOwnerUserId: number): boolean {
        var idn = this.currentUser.roleName == 'Moderator' || this.currentUser.id == postOwnerUserId;
        return idn;
    }

    getDefaultPostDuration() {
        return new Promise((resolve, reject) => {
            this.http.get(this.baseUrl + '/' + this.durationFromConfig)
                .toPromise()
                .then(res => { // Success
                    var result = new Headers(res.headers).get("duration") as string;
                    resolve(result);
                },
                msg => { // Error
                    reject(msg);
                });
        });
    }

    getDefaultPostEndDate() {
        return new Promise((resolve, reject) => {
            this.http.get(this.baseUrl + '/' + this.postEndDateFromConfig)
                .toPromise()
                .then(res => { // Success
                    var result = "";
                    if (this.currentUser.roleName == 'Moderator') {
                        result = new Headers(res.headers).get("enddateforAdmins") as string;
                    }
                    else {
                        result = new Headers(res.headers).get("enddateforUsers") as string;
                    }
                    resolve(result);
                },
                msg => { // Error
                    reject(msg);
                });
        });
    }

    showStatus(postOwnerUserId: number, isPublic: boolean): boolean {
        if (this.currentUser.roleName === "Moderator") {
            return true;
        }
        return false;

    }
}