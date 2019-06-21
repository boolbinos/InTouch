import { Injectable, Inject, EventEmitter } from '@angular/core';
import { Headers, Http } from '@angular/http';
import { Router } from '@angular/router';

import { CommentView } from '../app/components/comment/shared/commentView';
import { LastCommentView } from '../app/components/comment/shared/lastCommentView';
import { CommentCreate} from '../app/components/comment/shared/commentCreate';

import 'rxjs/add/operator/toPromise';
import { Observable } from 'rxjs/Rx';

@Injectable()
export class CommentService {
    private headers = new Headers({ 'Content-Type': 'application/json' });
    private commentsUrl = 'api/Comment';

    constructor(private http: Http,
        @Inject('BASE_URL') private baseUrl: string,
        private router: Router)
    {
        
    }

    createComment(createComment: CommentCreate): Promise<CommentView> {
        return new Promise((resolve, reject) => {
            this.http.post(this.baseUrl + this.commentsUrl,
                    createComment,
                    { headers: this.headers })
                .toPromise()
                .then(res => {
                        var result = res.json() as CommentView;
                        resolve(result);
                    },
                    msg => {
                        reject(msg);
                    });
        });
    }

    deleteComment(id: number) {
        return new Promise((resolve, reject) => {
            this.http.delete(this.baseUrl + this.commentsUrl + "/" + id,
                    { headers: this.headers })
                .toPromise()
                .then(res => { // Success
                        resolve(res);
                    },
                    msg => { // Error
                        reject(msg);
                    });
        });
    }

    getCommentsForPost(postId: number): Promise<CommentView> {
        return new Promise((resolve, reject) => {
            this.http.get(this.baseUrl + this.commentsUrl + '/getForPost/' +
                postId,
                { headers: this.headers })
                .toPromise()
                .then(res => {
                    var result = res.json() as CommentView;
                    resolve(result);
                },
                msg => {
                    reject(msg);
                });
        });
    }

    getLatestCommentsWithTimer(timer: Observable<number>): Observable<LastCommentView[]> {
        return timer
            .flatMap(() => this.http.get(this.baseUrl + this.commentsUrl + '/LatestComments')
                .map(res => res.json() as LastCommentView[])
                .catch((error: any) => Observable.throw(error.json().error || 'Server error')));
    }
}