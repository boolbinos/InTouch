import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { CommentCreate } from './shared/commentCreate';
import { PostDetailView } from '../post/shared/postDetailView';
import { CommentView } from './shared/commentView';

import { CommentService } from '../../../services/comment.service';

@Component({
    selector: 'comment-list',
    templateUrl: './comment-list.component.html'
})

export class CommentListComponent implements OnInit {
    public createCommentModel: CommentCreate = new CommentCreate();
    @Input() public comments: CommentView[];
    @Input() public post: PostDetailView;
    public inComments: CommentView[];
    public inPost: PostDetailView; 


    constructor (private commentService: CommentService) {

    }

    ngOnInit() {
        this.inComments = this.comments;
        this.inPost = this.post;
    }

    createComment(post: PostDetailView): void {
        if (this.createCommentModel.content.trim() !== "") {
            this.createCommentModel.postId = post.id;
            this.commentService.createComment(this.createCommentModel)
                .then(comment => this.inComments.unshift(comment));
        }
        this.createCommentModel.content = "";
    }

}