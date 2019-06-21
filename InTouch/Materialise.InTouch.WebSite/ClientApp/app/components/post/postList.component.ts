import {
    Component,
    Inject,
    OnInit,
    AfterViewInit,
    TemplateRef,
    ChangeDetectorRef,
    HostListener,
    Output,
    OnDestroy
} from '@angular/core';
import { Http } from '@angular/http';
import { Router, ActivatedRoute } from '@angular/router';
import { DomSanitizer, SafeUrl, Title } from '@angular/platform-browser';
import { Location } from '@angular/common';
import { BehaviorSubject, Subscription } from 'rxjs';

import { PostView } from './shared/postView';
import { UserView } from '../user/shared/userView';

import { FileService } from '../../../services/file.service';
import { PostService } from '../../../services/post.service';
import { UserService } from '../../../services/user.service';
import { ConfirmationService } from 'primeng/primeng';

import { LatestCommentsComponent } from '../comment/latest-comments/latest-comments.component';

@Component({
    selector: 'post-list',
    templateUrl: './postList.component.html',
    styleUrls: ['./post-list.component.css']
})

export class PostListComponent implements OnInit, AfterViewInit, OnDestroy {

    public search: string;
    public postListLoaded = false;
    public searchSubscription: Subscription;
    public myPostsSubscription: Subscription;
    public pageNum: number;
    public pageSize: number;
    public totalPageCount: number;    

    public currentUser: UserView;
    public posts: PostView[];
    public selectedPost: PostView;
    public isDataLoaded: BehaviorSubject<boolean>;    
    
    public isPageScrolled: boolean;
    public subscriptionFacebook: any;

    private invisibleEmptyList: boolean = true;
    public isMyPosts: boolean;
    private isLoadReady: boolean = true;


    constructor(
        @Inject('SHORT_CONTENT_LENGTH') private maxLength: number,
        private postService: PostService,
        private fileService: FileService,
        private titleService: Title,
        private router: Router,
        private userService: UserService,        
        private sanitizer: DomSanitizer,
        private confirmationService: ConfirmationService,
        private location: Location,
        private changeDetector: ChangeDetectorRef,
        private route: ActivatedRoute) {

        this.invisibleEmptyList = true;
        this.posts = new Array<PostView>();
        this.pageNum = 1;
        this.pageSize = 10;
        this.totalPageCount = 0;
        this.isDataLoaded = new BehaviorSubject(false);

        this.titleService.setTitle("InTouch posts");

        this.searchSubscription = this.postService.setSearchStr$.subscribe(
            () => {
                this.pageNum = 1;
                this.getPosts(this.pageNum, this.pageSize, this.postService.getIsMyPosts(), this.postService.getSearchStr());
            }
        )

        this.myPostsSubscription = this.postService.setIsMyPosts$.subscribe(
            () => {
                this.pageNum = 1;
                this.getPosts(this.pageNum, this.pageSize, this.postService.getIsMyPosts(), this.postService.getSearchStr());
            }
        )
    }

    @HostListener("window:scroll", [])
    onWindowScroll() {
        this.isPageScrolled = window.pageYOffset > 0;
        let position = window.scrollY;
        let height = document.documentElement.offsetHeight - document.documentElement.clientHeight;

        let eps = 100;

        if (Math.abs(height - position) < eps && this.isLoadReady) {
            this.isLoadReady = false;
            this.loadMore();
        }

    }

    getIsDataLoadedValue(): boolean {
        return this.isDataLoaded.getValue();
    }

    jumpToTop() {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    }

    anyPagesLeftToLoad() {
        return this.pageNum < this.totalPageCount;
    }

    confirm(post: PostView) {
        this.confirmationService.confirm({
            message: 'Are you sure that you want to delete this post?',
            accept: () => {
                this.deletePost(post);
            }
        });
    }

    getPosts(page: number, pageSize: number, isMyPosts: boolean, search: string) {
        return new Promise(resolve => {
            this.isDataLoaded.next(false);
           
            this.postService
                .getPosts(page, pageSize, isMyPosts, this.postService.getSearchStr())
                .then(pagedResult => {
                    this.postListLoaded = true;
                    if (this.pageNum == 1 ) {
                        this.posts = pagedResult.data;
                        window.scrollTo({ top: 0 });
                    }
                    else {
                        this.posts = this.posts.concat(pagedResult.data);
                    }
                    this.totalPageCount = pagedResult.paging.pageCount;
                    if (this.totalPageCount == 0) {
                        this.invisibleEmptyList = false;
                    } else {
                        this.invisibleEmptyList = true;
                    }
                    this.isDataLoaded.next(true);
                    resolve();
                });
        });
    }

    ngOnDestroy() {
        this.searchSubscription.unsubscribe();
        this.myPostsSubscription.unsubscribe();
        this.postService.setSearchStr(undefined);
        this.invisibleEmptyList = true;
    }

    getImagesPosts(item: string): string {
        return this.fileService.getUploadedFile(item);
    }

    persistPostNavigatingTo(): void {
        sessionStorage.setItem('lastOnScreenPostId', this.selectedPost.id.toString());
    }

    loadMore(): Promise<boolean> {
        return new Promise((resolve, reject) => {
            let successfullyLoaded = false;
            if (this.pageNum >= this.totalPageCount) {
                resolve(successfullyLoaded);
            } else {
                this.pageNum = this.pageNum + 1;
                this.getPosts(this.pageNum, this.pageSize, this.isMyPosts, this.postService.getSearchStr()).then(() => {
                    this.isLoadReady = true;
                    successfullyLoaded = true;
                    resolve(successfullyLoaded);
                });
            }
        });
    }

    ngOnInit(): void {
        this.userService.getCurrentUser().then(user => {
            this.currentUser = user as UserView;
        });
        this.getPosts(this.pageNum, this.pageSize, this.isMyPosts, this.search);
        
        this.subscriptionFacebook = this.postService.facebookImport
            .subscribe(() => {
                this.posts = [];
                this.getPosts(this.pageNum, this.pageSize, this.isMyPosts, this.search);
            });
    }


    ngAfterViewInit(): void {
        try {
            let lastPostId = sessionStorage.getItem('lastOnScreenPostId');

            if (lastPostId != null) {
                this.postService
                    .getPost(+lastPostId)
                    .then(post => {
                        if (post !== null) {
                            this.navigateToPost(post.id);
                        }
                    });
            }
        }
        finally {
            sessionStorage.removeItem('lastOnScreenPostId');
        }
    }

    navigateToPost(postId: number): void {
        this.findElementOnPages(postId).then(postElement => {
            postElement.scrollIntoView({ behavior: 'smooth' });
        }).catch(e => {
            console.log(e);
        });
    }

    findElementOnPages(postId: number): Promise<HTMLElement> {
        let dataLoadedSubscription: Subscription;
        return new Promise<HTMLElement>((resolve, reject) => {
            dataLoadedSubscription = this.isDataLoaded.subscribe((isLoaded) => {
                if (isLoaded) {
                    let post = this.posts.find(post => post.id === postId);

                    if (post === undefined) {

                        this.loadMore().then((succesfully) => {
                            let failed = !succesfully;

                            if (failed) {
                                reject('post not found');
                                dataLoadedSubscription.unsubscribe();
                            }
                        });
                    }
                    else {
                        this.changeDetector.detectChanges();

                        let lastPostElement = document.getElementById(postId.toString());
                        resolve(lastPostElement);
                        dataLoadedSubscription.unsubscribe();
                    }
                }
            });
        });
    }

    getLikersName(post: PostView): string {
        return this.postService.getLikersName(post.usersLikes);
    }

    editPost(post: PostView): void {
        this.selectedPost = post;
        this.router.navigate(['posts/edit', this.selectedPost.id]);
    }

    deletePost(post: PostView): void {
        this.selectedPost = post;
        let id = this.selectedPost.id;
        let index: number = this.posts.findIndex(postFind => postFind.id === id);
        this.postService
            .deletePost(id)
            .then(redirectPage => {
                this.posts.splice(index, 1);
            });
    }

    doPublic(post: PostView): void {
        this.postService.doPublic(post.id).then(result => {
            if (result) {
                post.isPublic = true;
            }
        });
    }

    doPrivate(post: PostView): void {
        this.postService.doPrivate(post.id).then(result => {
            if (result) {
                post.isPublic = false;
            }
        });
    }

    getStatus(post: PostView): boolean {
        return this.postService.getStatus(post);
    }

    getLikesCount(post: PostView): number {
        return this.postService.getLikesCount(post);
    }

    isPostLiked(post: PostView): boolean {
        return this.postService.isPostLiked(post.usersLikes);
    }

    likeClick(post: PostView): void {
        this.postService.like(post.usersLikes, post.id);
    }

    checkAccess(post: PostView): boolean {
        return this.postService.checkAccess(post.userId);
    }

    showStatus(post: PostView): boolean {
        return this.postService.showStatus(post.userId, post.isPublic);
    }

    getCurrentLocation(): string {
        return this.location.path();
    }
}
