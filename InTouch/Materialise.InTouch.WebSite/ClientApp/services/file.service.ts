
import { Injectable, Inject } from '@angular/core';

import { HttpInterceptorService } from './http-interceptor-service';
import { FileUploader } from 'ng2-file-upload';

import { FileView } from '../app/components/post/shared/fileView';


@Injectable()
export class FileService {

    private fileUrl: string = 'api/Files';

    public imageUploader: FileUploader;

    public search: string = undefined;

    constructor(
        @Inject('BASE_URL') private baseUrl: string) {

        var options = {
            url: this.fileUrl,
            autoUpload: true,
            allowedMimeType: ['image/jpeg', 'image/jpg', 'image/png'],
            maxFileSize: 10485760
        };

        this.imageUploader = new FileUploader(options);
    }

    public getUploadedFile(fileId: string): string {
        return this.baseUrl + this.fileUrl + "/" + fileId;
    }

    public getTempFile(file: FileView): string{
        return this.baseUrl + this.fileUrl + "/" + file.id + '/' + file.name;
    }
}