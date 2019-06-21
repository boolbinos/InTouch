import { Component, OnInit, Inject, ViewChild, ElementRef, HostListener, OnDestroy } from '@angular/core';
import { Http } from '@angular/http';
import { DomSanitizer, SafeResourceUrl, SafeUrl, Title } from '@angular/platform-browser';
import { CarouselComponent } from 'ngx-bootstrap';
import { Router, NavigationStart } from '@angular/router';
import { PostView } from '../post/shared/postView';
import { PostService } from '../../../services/post.service';
import { FileService } from '../../../services/file.service';
import { FullscreenService } from '../../../services/fullscreen.service';
import { FullScreenPostPartViewModel } from './fullscreen-post-part-view-model';

@Component({
    selector: 'fullscreen-mode',
    templateUrl: './fullscreen-mode.component.html',
    styleUrls: ['./fullscreen-mode.component.css']
})
export class FullScreenModecomponent implements OnInit, OnDestroy {


    public batch: FullScreenPostPartViewModel[] = [];

    private isRefresh: boolean;
    private videoBlob: SafeResourceUrl;

    public carousel: CarouselComponent;
    public noWrapSlides: boolean = false;
    public activeSlideIndex: number = 0;
    public styleView: number;

    @ViewChild(CarouselComponent)
    private carouselComponent: CarouselComponent;

    constructor(private postService: PostService,
        private fileService: FileService,
        private titleService: Title,
        private sanitizer: DomSanitizer,
        private fsService: FullscreenService,
        private router: Router) {
        this.titleService.setTitle("Fullscreen mode");
        this.router.events.subscribe(event => {
            if (event instanceof NavigationStart) {
                let isRefresh = event.url.includes(this.router.url);
                if (isRefresh) {
                    this.isRefresh = true;
                }
            }
        });
    }

    @HostListener('window:unload', ['$event'])
    unloadHandler(event: Event) {
        this.removeSessionData();
    }

    ngOnDestroy() {

    }

    removeSessionData(): void {
        sessionStorage.removeItem('lastBatchDate');
        sessionStorage.removeItem('lastPostCreateDate');
    }

    getPostImages(id: string) {
        return this.fileService.getUploadedFile(id);
    }

    runCarousel(isFirstSlide: boolean = true) {
        if (isFirstSlide) {
            this.carouselComponent.selectSlide(0);
        }

        let activeSlide = this.carouselComponent.activeSlide;

        let post = this.batch[activeSlide];

        let postDuration = post.durationInSeconds * 1000;

        if (post.videoUrl !== null) {
            this.videoBlob = this.createVideoBlob(post.videoUrl);
            postDuration += 2000;
        }

        setTimeout(() => {
            if (post.videoUrl !== null) {
                document.querySelector('#video').remove();
                this.videoBlob = null;
            }

            this.carouselComponent.nextSlide();

            document.getElementsByClassName('item')[0].remove;

            let isLastSlide = activeSlide == this.batch.length - 1;
            if (isLastSlide) {
                this.getBatch().then(() => {
                    this.runCarousel(true);
                });
            }
            else {
                this.runCarousel(false);
            }

        }, postDuration);
    }

    ngOnInit(): void {
        this.carouselComponent.interval = 0;

        this.getBatch().then(() => {
            setTimeout(() => {
                this.runCarousel(true)
            }, 0)
        });
    }

    createVideoBlob(url: string): SafeResourceUrl {
        return this.videoBlob = this.sanitizer.bypassSecurityTrustResourceUrl(url);
    }

    getBatch(): Promise<void> {
        return new Promise<void>(resolve => {

            let lastBatchDate = new Date(sessionStorage.getItem('lastBatchDate'));
            let lastPostCreateDate = new Date(sessionStorage.getItem('lastPostCreateDate'));

            this.fsService.getFullscreenModels(lastBatchDate, lastPostCreateDate)
                .then(newBatch => {
                    sessionStorage.setItem('lastBatchDate', new Date().toUTCString());
                    sessionStorage.setItem('lastPostCreateDate', new Date(newBatch[newBatch.length - 1].createdDate).toUTCString());

                    newBatch = newBatch.map((post) => {
                        if (post.videoUrl != undefined && post.videoUrl != null) {

                            let playbackParameter: string;

                            if (/www.youtube.com/i.test(post.videoUrl)) {
                                playbackParameter = "?autoplay=1"
                            }
                            else {
                                playbackParameter = "&autoPlay=true";
                            }
                            post.videoUrl += playbackParameter;
                        }
                        return post;
                    });

                    this.batch = newBatch;
                }).then(() => resolve()).catch((error) =>
                { });
        });
    }
}