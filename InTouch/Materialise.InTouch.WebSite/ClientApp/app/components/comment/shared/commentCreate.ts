export class CommentCreate {
    postId: number;
    content: string;

    constructor() {
        this.postId = 0;
        this.content = "";
    }
}