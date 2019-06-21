import 'rxjs/add/operator/switchMap';
import { Component, OnInit, ViewChild, AfterViewChecked, ElementRef, HostListener } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Location } from '@angular/common';
import { Router, NavigationStart} from '@angular/router';
import { PostView } from './shared/postView';
import { PostDetailView } from './shared/postDetailView';
import { CommentView } from '../comment/shared/commentView';
import { PostService } from '../../../services/post.service';
import { FileService } from '../../../services/file.service';
import { UserView } from '../user/shared/userView';
import { UserLikeView } from '../user/shared/userLikeView';
import { UserService } from '../../../services/user.service';
import { PostDetailService } from '../../../services/post.detail.service';

import { DomSanitizer, SafeResourceUrl, SafeUrl, Title } from '@angular/platform-browser';
import { ConfirmationService } from 'primeng/primeng';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';
import { PostInfoComponent } from './postinfo/postinfo.component';
import { FileView } from "./shared/fileView";

@Component({
    selector: 'post-detail',
    templateUrl: './post-detail.component.html',
    styleUrls: ['./post-detail.component.css']
})
export class PostDetailComponent implements OnInit, AfterViewChecked {
    url: SafeUrl;
    post: PostDetailView;
    currentUser: UserView;
    comments: Array<CommentView>;
    likes: Array<UserLikeView> = [];
    files: Array<FileView> = [];

    galleryOptions: NgxGalleryOptions[];
    galleryImages: NgxGalleryImage[] = [];

    hasAutoScrolled: boolean;
    fragment: string;

    constructor(
        private postService: PostService,
        private fileService: FileService,
        private userService: UserService,
        private route: ActivatedRoute,
        private location: Location,
        private router: Router,
        private sanitizer: DomSanitizer,
        private titleService: Title,
        private confirmationService: ConfirmationService,
        private postDetailService: PostDetailService
    ) {
        router.events.subscribe(event => {
            if (event instanceof NavigationStart) {
                if (!event.url.includes('posts')) {
                    this.removeNavigationAnchorFromSession();
                }
            }
        });     
    }

    goOnPrevious() {
        this.location.back();
    }

    confirm(post: PostDetailView) {
        this.confirmationService.confirm({
            message: 'Are you sure that you want to delete this post?',
            accept: () => {
                this.deletePost();
            }
        });
    }

    @HostListener('window:onclose', ['$event'])
    unloadHandler(event: any) {
        this.removeNavigationAnchorFromSession();
    }

    removeNavigationAnchorFromSession(): void {
        sessionStorage.removeItem('lastOnScreenPostId');
    }

    ngOnInit(): void {
        this.getPostOnly();
        this.getPostsLikes();
        this.getCurrentUser();
        this.getPostsFiles();
        this.getPostComments();
        this.initGalleryOptions();
        this.titleService.setTitle("Post details");
    }

    getPostOnly() {
        this.route.paramMap
            .switchMap((params: ParamMap) => this.postDetailService.getPost(+params.get('id')!))
            .subscribe(post => {

                this.post = post;
                this.url = this.sanitizer.bypassSecurityTrustResourceUrl(post.videoUrl);
            });
    }
    getPostsLikes() {
        this.route.paramMap
            .switchMap((params: ParamMap) => this.postDetailService.getPostsLikes(+params.get('id')!))
            .subscribe(usersLikers => {
                this.likes = usersLikers;
            });
    }
    getPostsFiles() {
        this.route.paramMap
            .switchMap((params: ParamMap) => this.postDetailService.getPostsFiles(+params.get('id')!))
            .subscribe(files => {

                this.files = files;
                files.forEach(f => {
                    this.galleryImages.push({
                        small: 'api/files/' + f.id,
                        medium: 'api/files/' + f.id,
                        big: 'api/files/' + f.id,
                    })
                });

                if (this.files.length == 1) {
                    this.removeIconsFromGallery()
                }
            });
    }
    getPostComments() {
        this.route.paramMap
            .switchMap((params: ParamMap) => this.postDetailService.getPostsComments(+params.get('id')!))
            .subscribe(comments => {
                this.comments = comments;
            });
    }

    getCurrentUser() {
        this.userService.getCurrentUser()
            .then(user => {
                this.currentUser = user as UserView;
            });
    }

    initGalleryOptions() {
        this.galleryOptions = [
            {
                width: '100%',
                height: '500px',
                thumbnailsColumns: 4,
                thumbnailsPercent: 25,
                imageAnimation: NgxGalleryAnimation.Fade,
                previewFullscreen: false
            },
            // max-width 800
            {
                breakpoint: 800,
                width: '100%',
                height: '400px',
                imagePercent: 80,
                thumbnailsPercent: 20,
                thumbnailsMargin: 20,
                thumbnailMargin: 20
            },
            // max-width 400
            {
                breakpoint: 400,
                preview: false
            }
        ];

        this.titleService.setTitle("Post details");
        this.route.fragment.subscribe(fragment => { this.fragment = fragment; });
    }

    ngAfterViewChecked() {

        if (this.fragment != undefined && !this.hasAutoScrolled) {
            setTimeout(() => {
                try {
                    this.hasAutoScrolled = true;
                    document.querySelector('#' + this.fragment).scrollIntoView({ behavior: 'smooth' });
                } catch (e) {
                    this.hasAutoScrolled = false;
                }
            }, 0);
        }
    }

    goBack(): void {
        this.location.back();
    }

    getImagesPosts(item: string): string {
        return this.fileService.getUploadedFile(item);
    }

    deletePost(): void {
        var id = +this.route.snapshot.params['id'];
        this.postService
            .deletePost(id)
            .then(redirectPage => {
                this.router.navigate([redirectPage]);
            });
    }

    editPost(): void {
        this.router.navigate(['posts/edit', this.post.id]);
    }

    doPublic(post: PostDetailView): void {
            this.postService.doPublic(post.id).then(result => {
                if (result) {
                    post.isPublic = true;
                }
            });
    }

    doPrivate(post: PostDetailView): void {
        this.postService.doPrivate(post.id).then(result => {
                if (result) {
                    post.isPublic = false;
                }
            });
    }

    getStatus(post: PostDetailView): boolean {
        return post.isPublic;
    }

    getLikesCount(likes: Array<UserLikeView>): number {
        return likes.length;
    }

    isPostLiked(likes: Array<UserLikeView>): boolean {
        return this.postService.isPostLiked(likes);
    }


    likeClick(likes: Array<UserLikeView>): void {
        this.postService.like(likes, this.post.id);
    }

    getLikersName(likes: Array<UserLikeView>): string {
        return this.postService.getLikersName(likes);
    }

    checkAccess(post: PostDetailView): boolean {
        return this.postService.checkAccess(post.userId);
    }

    showStatus(post: PostDetailView): boolean {
        return this.postService.showStatus(post.userId, post.isPublic);
    }

    removeIconsFromGallery() {
        this.galleryOptions.forEach(o => {
            o.arrowNextIcon = 'fa fa-arrow-circle-right hidden-lg-up hidden-md-up hidden-sm-up hidden-xl-up hidden-xs-up';
            o.arrowPrevIcon = 'fa fa-arrow-circle-left hidden-lg-up hidden-md-up hidden-sm-up hidden-xl-up hidden-xs-up';
        });
    }

    getCurrentLocation(): string {
        return this.location.path();
    }
}