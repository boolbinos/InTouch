import 'rxjs/add/operator/switchMap';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { FileItem, ParsedResponseHeaders, FilterFunction, FileLikeObject, FileUploaderOptions } from 'ng2-file-upload';
import { DomSanitizer, SafeUrl, Title } from '@angular/platform-browser';
import { CalendarModule } from 'primeng/primeng';
import { PostInfoComponent } from '../post/postinfo/postinfo.component';
import { ToasterService } from '../../../services/toaster.service';


import { PostView } from '../post/shared/postView';
import { FileView } from '../post/shared/fileView';
import { PostEdit } from './shared/postEdit';
import { PostService } from '../../../services/post.service';

import { UserView } from '../user/shared/userView';
import { UserService } from '../../../services/user.service';

@Component({
    selector: 'edit-post',
    templateUrl: './editpost.component.html',
    styleUrls: ['./editpost.component.css']
})
export class EditPostComponent implements OnInit {
    post: PostEdit = new PostEdit(0, '', '', 0, '', [], new Date(), new Date(), '', 'undefined', new Date());
    private error: string;
    private FileInfo: Array<FileView>;
    submitted: boolean = false;
    public currentUser: UserView;


    constructor(private userService: UserService,
        private postService: PostService,
        private route: ActivatedRoute,
        private location: Location,
        private router: Router,
        private titleService: Title,
        private sanitizer: DomSanitizer,
        private toasterService: ToasterService) {
        this.FileInfo = new Array<FileView>();
        this.postService.uploader.clearQueue();

        this.postService.uploader.onWhenAddingFileFailed = (item: FileLikeObject, filter: any, options: any) => {

            switch (filter.name) {
                case "fileSize":
                    this.setError('Image size should be < ' + options.maxFileSize / 1024 / 1024 + 'mb');
                    break;
                case "mimeType":
                    this.setError('Incorrect file type');
                    break;
                default:
                    break;
            }
        };

        this.postService.uploader.onAfterAddingFile = (item: FileItem) => {
            let fileInfoById = this.FileInfo.find(file => file.id === item.file.name);

            if (fileInfoById != undefined && fileInfoById.isAttached == false) {
                item.isUploaded = true;
            }
        };

        this.postService.uploader.onSuccessItem = (item: FileItem, response: string, status: number, headers: ParsedResponseHeaders) => {

            let responseFileView = JSON.parse(response) as FileView;
            responseFileView.isAttached = true;
            this.FileInfo.push(responseFileView);

            let uploadedFileInQueueIdx = this.postService.uploader.queue.findIndex((element: FileItem, index: number, array: FileItem[]) => element.file.size === item.file.size);
            this.postService.uploader.queue[uploadedFileInQueueIdx].file.name = responseFileView.id;

            this.clearError();
        };

        this.titleService.setTitle("Edit post");
    }

    public get startDate(): Date {
        return this.post.startDate;
    }
    public set startDate(date: Date) {
        this.post.startDate = new Date(date);
        if (this.post.startDate > this.post.endDate) {
            this.post.endDate = new Date(this.post.startDate);
            this.toasterService.showSnackbar("Start date can't bee more then end date");
        }
    }

    public get endDate(): Date {
        return this.post.endDate;
    }
    public set endDate(date: Date) {
        this.post.endDate = new Date(date);
        if (this.post.endDate < this.post.startDate) {
            this.post.endDate = new Date(this.post.startDate);
            this.toasterService.showSnackbar("end date can't bee less then start date");

        }
    }

    public getFileUploader(): any {
        return this.postService.uploader;
    }
    public getFileUploaderQueue(): Array<FileItem> {
        return this.getFileUploader().queue;
    }

    ngOnInit(): void {
        this.userService.getCurrentUser().then(user => {
            this.currentUser = user as UserView;
        });

        this.route.paramMap
            .switchMap((params: ParamMap) => this.postService.getEditPostModel(+params.get('id')!))
            .subscribe(post => {
                this.post = post;
                this.post.startDate = new Date(post.startDate);
                this.post.endDate = new Date(post.endDate);
                this.post.createdDate = new Date(post.createdDate);

                this.FileInfo = post.files;
                this.loadImagesToFileUploader();
            });
    }

    private loadImagesToFileUploader() {
        let files = new Array<File>();

        this.FileInfo.forEach(file =>
            files.push(new File([], file.id, { type: file.contentType })));

        this.postService.uploader.addToQueue(files);

    }

    public clearError(): void {
        this.error = '';
    }

    public getError(): string {
        return this.error;
    }

    public setError(error: string) {
        this.error = error;
    }

    public onSubmit() {
        this.submitted = true;
        let editPost = new PostEdit(this.post.id, this.post.title, this.post.content, this.post.durationInSeconds, this.post.videoUrl, this.FileInfo, this.post.startDate, this.post.endDate, this.post.priority, this.post.userName, this.post.createdDate);
        editPost.CheckUrl();
        this.postService.editPost(this.post.id, editPost).then(x => {
            this.location.back();
        }, m => {
            //TODO: error handling
            //debugger;
        });
    }

    public getDeleteItem(item: FileItem): void {
        this.FileInfo.splice(this.FileInfo.map(obj => obj.id).indexOf(item.file.name), 1);
        item.remove();
    }

    public showItem(item: FileItem): SafeUrl {
        let fileInfo = this.FileInfo.find(file => file.id === item.file.name);


        if (fileInfo !== undefined && fileInfo.isAttached !== true) {
            return this.postService.GetImages(fileInfo.id);
        }
        return this.sanitizer.bypassSecurityTrustUrl(window.URL.createObjectURL(item._file));
    }



    goBack(): void {
        this.location.back();
    }

    public isModerator(user: UserView): boolean {
        return this.userService.isModerator(user);
    }
}