import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import {CommentView} from './../shared/commentView'
import { UserView } from '../../user/shared/userView';

import { UserService } from '../../../../services/user.service';
import { ConfirmationService } from 'primeng/primeng';
import { CommentService } from '../../../../services/comment.service';


@Component({
    selector: 'comment',
    templateUrl: './comment.component.html',
    styleUrls: ['./comment.component.css']
})

export class CommentComponent implements OnInit{
    @Input() public comment: CommentView;
    @Input() public comments: CommentView[];
    @Output() public outComments: EventEmitter<CommentView[]> = new EventEmitter<CommentView[]>();
    public inComment: CommentView;
    public inComments: CommentView[];
    public currentUser: UserView;

    constructor(
        private userService: UserService,
        private confirmationService: ConfirmationService,
        private commentService: CommentService
    ) {

    }

    getCurrentUser() {
        this.userService.getCurrentUser()
            .then(user => {
                this.currentUser = user as UserView;
            });
    }

    deleteComment(id: number): void {
        this.commentService.deleteComment(id);
        let deletedCommentIndex = this.inComments.findIndex(comment => comment.id === id);
        this.inComments.splice(deletedCommentIndex, 1);
        this.outComments.emit(this.inComments);
    }

    confirmCommentDelete(comment: CommentView) {
        this.confirmationService.confirm({
            message: 'Are you sure that you want to delete this comment?',
            accept: () => {
                this.deleteComment(comment.id);
            }
        });
    }

    ngOnInit() {
        this.inComment = this.comment;
        this.inComments = this.comments;
        this.getCurrentUser();
    }
 
}