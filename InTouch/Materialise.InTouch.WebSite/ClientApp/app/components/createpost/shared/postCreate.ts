import {FileView} from '../../post/shared/fileView';

export class PostCreate {
    title: string;
    content: string;
    durationInSeconds: number;
    files: Array<FileView>;
    videoUrl: string;
    startDate: Date;
    endDate: Date;
    priority: string;
    constructor()
    {
        this.title="";
        this.content="";
        this.durationInSeconds=0;
        this.files=new Array<FileView>();
        this.videoUrl="";
        this.startDate=new Date();
        this.endDate=new Date();
        this.priority="Normal";
    }

    CheckUrl():void
    {
       if (/youtu.be/i.test(this.videoUrl) || /www.youtube.com/i.test(this.videoUrl))
       {
           this.videoUrl = /youtu.be/i.test(this.videoUrl) ? this.videoUrl.replace('youtu.be', 'www.youtube.com/embed')
               : this.videoUrl.replace('www.youtube.com/watch?v=', 'www.youtube.com/embed/');
       }
       if(/materialisenv.sharepoint.com/i.test(this.videoUrl))
       {
          var getposition = this.videoUrl.search('chid=');
          if(getposition!=-1)
          {
          var getString= this.videoUrl.substring(getposition,this.videoUrl.length);
          this.videoUrl = "https://materialisenv.sharepoint.com/portals/hub/_layouts/15/VideoEmbedHost.aspx?"+getString;
         }
       }
    }
}