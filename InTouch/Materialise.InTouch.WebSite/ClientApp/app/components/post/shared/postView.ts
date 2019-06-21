import { FileView } from './fileView';
import { CommentView } from '../../comment/shared/commentView';
import { UserLikeView } from "../../user/shared/userLikeView";

export class PostView {
    id: number;
    title: string;
    content: string;
    createdDate: Date;
    isPublic: boolean;
    userName: string;
    userId: number;
    files: Array<FileView>;
    usersLikes: Array<UserLikeView>;
    durationInSeconds: number;
    videoUrl: string;
    postType: string;
    startDate: Date;
    endDate: Date;
    priority: string;
    commentsAmount: number;
    comments: Array<CommentView>;
    avatar: string

  constructor()
  {
      this.files = new Array<FileView>();
      this.usersLikes = new Array<UserLikeView>();
      this.comments = new Array<CommentView>();
      this.id=0;
      this.title="";
      this.content="";
      this.durationInSeconds=0;
      this.videoUrl="";
      this.createdDate=new Date();
      this.startDate=new Date();
      this.endDate=new Date();
      this.priority="Normal";
      this.isPublic=false;
      this.userId=0;
      this.userName="";
      this.postType = "None";
      this.commentsAmount = 0;
      this.avatar = "";
  }
}