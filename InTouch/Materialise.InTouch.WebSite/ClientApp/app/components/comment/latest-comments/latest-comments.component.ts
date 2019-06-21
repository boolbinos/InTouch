import { Component, HostListener, Inject, Input, OnChanges } from '@angular/core';

import { CommentService } from '../../../../services/comment.service';
import { LastCommentView } from '../shared/lastCommentView';
import { PreloaderComponent } from '../../preloader/preloader.component';
import { DOCUMENT } from "@angular/platform-browser";

import { Observable } from 'rxjs/Rx';

@Component({
    selector: 'latest-comments',
    templateUrl: './latest-comments.component.html',
    styleUrls: ['./latest-comments.component.css']
})

export class LatestCommentsComponent {
    previousOffset: number = 0;
    lastComments: Observable<LastCommentView[]>;
    timer = Observable.timer(0, 60000);
    @Input() public isPostsLoaded: boolean;
    public inIsPostsLoaded: boolean = false;


    constructor(private commentService: CommentService, @Inject(DOCUMENT) private document: Document) {
        this.lastComments = this.commentService.getLatestCommentsWithTimer(this.timer);
    }

    ngOnChanges() {
        this.inIsPostsLoaded = this.isPostsLoaded;
    }

    @HostListener("window:scroll", [])
    onWindowScroll() {
        let widget = this.document.getElementById("widgetSide");
        let windowHeight = window.innerHeight;
        let widgetHeight = widget.offsetHeight;
        let currentOffset = window.pageYOffset;
        let currentMargin = parseInt(widget.style.marginTop);

        let heightDiff = widgetHeight - windowHeight;
        let bias = 220;
        let newMargin = 0;

        let postListStnHeight = this.document.getElementById('post-list-section').offsetHeight;
        
        let widgetOldMargin = parseInt(widget.style.marginTop);

        if (widgetOldMargin < postListStnHeight - widgetHeight - bias || this.previousOffset > currentOffset) {
            if (heightDiff < 0) {
                bias = 0;
                heightDiff = 0;
            }
            if (currentOffset < 25)
                newMargin = 0;

            if (currentOffset > heightDiff + bias)
                newMargin = currentOffset - 25 - heightDiff - bias;
            widget.style.marginTop = newMargin.toString() + "px";
        }
        this.previousOffset = currentOffset;
    }
}