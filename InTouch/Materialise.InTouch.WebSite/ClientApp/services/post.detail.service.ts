import { Injectable, Inject, EventEmitter } from '@angular/core';
import { Headers, Http } from '@angular/http';
import { Router } from '@angular/router';

import { PostDetailView } from '../app/components/post/shared/postDetailView';
import { FileView } from '../app/components/post/shared/fileView';
import { CommentView } from '../app/components/comment/shared/commentView';
import { UserView } from '../app/components/user/shared/userView'
import { UserService } from '../services/user.service';
import { ToasterService } from '../services/toaster.service';
import { AccessService } from '../services/Access.service';

import { HttpInterceptorService } from './http-interceptor-service';

import 'rxjs/add/operator/toPromise';
import { UserLikeView } from "../app/components/user/shared/userLikeView";


@Injectable()
export class PostDetailService {

    private headers = new Headers({ 'Content-Type': 'application/json' });
    private postsUrl = 'api/Posts';
    private likesUrl = 'api/Likes';
    private filesUrl = 'api/Files';
    private commentsUrl = 'api/Comment';
    //
    private importUrl: string = 'api/posts/Import';
    //
    public facebookImport: EventEmitter<number> = new EventEmitter();
    public currentUser: UserView;
    public userService: UserService;

    constructor(private http: HttpInterceptorService,
        @Inject('BASE_URL') private baseUrl: string,
        private router: Router, userService: UserService,
        private toasterService: ToasterService,
        private access: AccessService) {

        userService.getCurrentUser().then(u => this.currentUser = u);

    }

    getPost(id: number): Promise<PostDetailView> {
        return new Promise(resolve => {
            this.http.get(this.baseUrl + this.postsUrl + "/" + id).subscribe(result => {
                resolve(result.json() as PostDetailView);
            },
                error => {
                    this.access.CheckUnAuthorizeUser(error);
                    console.log(error);
                });
        });
    }

    getPostsLikes(id: number): Promise<Array<UserLikeView>> {
        return new Promise(resolve => {
            this.http.get(this.baseUrl + this.postsUrl + "/" + id+'/likes').subscribe(result => {
                resolve(result.json() as Array<UserLikeView>);
            },
                error => {
                    this.access.CheckUnAuthorizeUser(error);
                    console.log(error);
                });
        });
    }


    getPostsFiles(postId: number): Promise<Array<FileView>> {
        return new Promise(resolve => {
            this.http.get(this.baseUrl + this.postsUrl +'/'+ postId+'/files').subscribe(result => {
                resolve(result.json() as Array<FileView>);
            },
                error => {
                    this.access.CheckUnAuthorizeUser(error);
                    console.log(error);
                });
        });
    }

    getPostsComments(postId: number): Promise<Array<CommentView>> {
        return new Promise(resolve => {
            this.http.get(this.baseUrl + this.postsUrl+'/'+ postId+'/comments').subscribe(result => {
                resolve(result.json() as Array<CommentView>);
            },
                error => {
                    this.access.CheckUnAuthorizeUser(error);
                    console.log(error);
                });
        });
    }
}