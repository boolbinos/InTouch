import { Component, Input } from '@angular/core';
import { PostInfo } from "./shared/postInfo";

@Component({
    selector: 'post-info',
    templateUrl: './postinfo.component.html',
    styleUrls: ['./postinfo.component.css' ]
})

export class PostInfoComponent {
    @Input() post: PostInfo;

    constructor() {

    }

    hasAdditionContent(): boolean {
        return this.post.priority == 'High'
            || this.post.videoUrl != null
            || this.post.postType == 'Facebook'
            || this.post.postType == 'Sharepoint';
    }
}