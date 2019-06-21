export class CommentView {
    id: number;
    userId: number;
    postId: number;
    content: string;
    createdDate: Date;
    userFirstName: string;
    userLastName: string;
    avatar: string;

    constructor() {
        this.id = 0;
        this.userId = 0;
        this.postId = 0;
        this.content = "";
        this.createdDate = new Date();
        this.userFirstName = "";
        this.userLastName = "";
        this.avatar = "";
    }
}