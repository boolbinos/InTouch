export class PostInfo {
    createdDate: Date;
    userName: string;
    videoUrl: string;
    postType: string;
    priority: string;
    avatar: string

  constructor()
  {
      this.videoUrl="";
      this.createdDate=new Date();
      this.priority="Normal";
      this.userName="";
      this.postType = "None";
      this.avatar = "";
  }
}