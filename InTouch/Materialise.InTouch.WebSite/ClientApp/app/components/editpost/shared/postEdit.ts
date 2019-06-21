import { FileView } from '../../post/shared/fileView';
export class PostEdit {
    id: number;
    title: string;
    content: string;
    userName: string;
    createdDate: Date;
    durationInSeconds: number;
    videoUrl: string;
    files: FileView[];
    startDate: Date;
    endDate: Date;
    priority: string;

    constructor(id:number, title: string, content: string, durationInSeconds: number, videoUrl: string, files: FileView[], startDate: Date, endDate: Date, priority: string, userName:string, createdDate:Date) {
        this.title = title;
        this.content = content;
        this.durationInSeconds = durationInSeconds;
        this.videoUrl=videoUrl;
        this.files = files;
        this.startDate = startDate;
        this.endDate = endDate;
        this.priority = priority;
        this.id = id;
        this.createdDate=createdDate;
        this.userName=userName;
    }

    CheckUrl(): void {
        if (/youtu.be/i.test(this.videoUrl) || /www.youtube.com/i.test(this.videoUrl)) {
            this.videoUrl = /youtu.be/i.test(this.videoUrl) ? this.videoUrl.replace('youtu.be', 'www.youtube.com/embed')
                : this.videoUrl.replace('www.youtube.com/watch?v=', 'www.youtube.com/embed/');
        }
        if (/materialisenv.sharepoint.com/i.test(this.videoUrl)) {
            var getposition = this.videoUrl.search('chid=');
            if (getposition != -1) {
                var getString = this.videoUrl.substring(getposition, this.videoUrl.length);
                this.videoUrl = "https://materialisenv.sharepoint.com/portals/hub/_layouts/15/VideoEmbedHost.aspx?" + getString;
            }
        }
    }
}