export class PostDetailView {
    id: number;
    title: string;
    content: string;
    createdDate: Date;
    isPublic: boolean;
    userName: string;
    userId: number;
    videoUrl: string;
    postType: string;
    priority: string;
    avatar: string;

    constructor() {
        this.id = 0;
        this.title = "";
        this.content = "";
        this.videoUrl = "";
        this.createdDate = new Date();
        this.priority = "Normal";
        this.isPublic = false;
        this.userId = 0;
        this.userName = "";
        this.postType = "None";
        this.avatar = "";
    }
}