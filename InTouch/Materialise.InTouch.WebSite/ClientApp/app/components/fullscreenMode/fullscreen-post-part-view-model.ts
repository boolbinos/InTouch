import { FileView } from '../post/shared/fileView';

export class FullScreenPostPartViewModel {
    public id: number;
    public title: string;
    public createdDate: Date;
    public contentType: number;
    public file: FileView;
    public videoUrl: string;
    public durationInSeconds: number;
    public postType: string;
    public priority: string;
}