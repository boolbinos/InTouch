import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { Location } from '@angular/common';
import { DomSanitizer, SafeUrl, Title } from '@angular/platform-browser';
import { FileUploader } from 'ng2-file-upload';
import { FileItem, ParsedResponseHeaders, FilterFunction, FileLikeObject, FileUploaderOptions } from 'ng2-file-upload';

import { PostCreate } from './shared/postCreate';
import { PostService } from '../../../services/post.service';
import { FileService } from '../../../services/file.service';
import { FileView } from '../post/shared/fileView';
import { UserView } from '../user/shared/userView';
import { UserService } from '../../../services/user.service';
import { ToasterService } from '../../../services/toaster.service';
import { CalendarModule } from 'primeng/primeng';
import { PreloaderComponent } from '../preloader/preloader.component';

@Component({
    selector: 'create-post',
    templateUrl: './createpost.component.html',
    styleUrls: ['./createpost.component.css']
})
export class CreatePostComponent implements OnInit{
    model: PostCreate = new PostCreate();
    submitted: boolean = false;
    currentUser: UserView;
    postCreating: boolean = false;
    error: string;
    isImageUploaded: Array<boolean> = new Array<boolean>();

    public constructor(private titleService: Title,
        private userService: UserService,
        public postService: PostService,
        private router: Router,
        private sanitizer: DomSanitizer,
        private location: Location,
        private toasterService: ToasterService,
        private fileService: FileService) {

        this.titleService.setTitle("Create new post");
        this.setupImageUploader();
    }

    ngOnInit(): void {
        window.scrollTo({ top: 0 });

        this.userService.getCurrentUser().then(user => {
            this.currentUser = user as UserView;
        });

        this.postService
            .getDefaultPostDuration()
            .then(result => {
                this.model.durationInSeconds = result as number;
            });

        this.postService
            .getDefaultPostEndDate()
            .then(result => {
                var daysToAdd = result as number;
                this.model.endDate = new Date(this.model.startDate.getTime() + (60 * 60 * 24 * 1000 * daysToAdd));
            });

    }

    public get startDate(): Date {
        return this.model.startDate;
    }

    public set startDate(date: Date) {
        this.model.startDate = new Date(date);
        if (this.model.startDate > this.model.endDate) {
            this.model.endDate = new Date(this.model.startDate);
            this.toasterService.showSnackbar("Start date can't be more then end date");
        }
    }

    public get endDate(): Date {
        return this.model.endDate;
    }

    public set endDate(date: Date) {
        this.model.endDate = new Date(date);
        if (this.model.endDate < this.model.startDate) {
            this.model.endDate = new Date(this.model.startDate);
            this.toasterService.showSnackbar("End date can't be less then start date");

        }
    }
    
    public get imageUploader(): FileUploader {
        return this.fileService.imageUploader;
    }

    public get uploadedImages(): Array<FileView> {
        return this.model.files;
    }

    public get queuedImages(): Array<FileItem> {
        return this.imageUploader.queue;
    }

    setupImageUploader() {
        this.imageUploader.clearQueue();

        this.imageUploader.onWhenAddingFileFailed = (item: FileLikeObject, filter: any, options: any) => {

            switch (filter.name) {
                case "fileSize":
                    this.error = 'Image size should be < ' + options.maxFileSize / 1024 / 1024 + 'mb';
                    break;
                case "mimeType":
                    this.error = 'Incorrect file type';
                    break;
                default:
                    break;
            }
        };
        this.imageUploader.onSuccessItem = (item: FileItem, response: string, status: number, headers: ParsedResponseHeaders) => {
            this.model.files.push(JSON.parse(response) as FileView);
            this.error = '';
        };

    }

    private setImageUploaded(index: number) {
        this.isImageUploaded[index] = true;
    }

    private isImageLoaded(index: number) {
        return this.isImageUploaded[index];
    }
    
    public deleteItem(item: FileItem): void {
        this.model.files.splice(this.model.files.map(obj => obj.name).indexOf(item.file.name), 1);
        item.remove();
    }

    public getImageUrl(image: FileView): string {
        return this.fileService.getTempFile(image);
    }

    public goBack(): void {
        this.location.back();
    }

    public isModerator(user: UserView): boolean {
        return this.userService.isModerator(user);
    }

    public onSubmit() {
        this.submitted = true;
        this.model.CheckUrl();
        this.postCreating = true;
        this.postService.createPost(this.model).then(x => {
            this.postCreating = false;
            this.router.navigate(['posts']);
            this.toasterService.showSnackbar("Created");
        }, m => {
        });
    }
}