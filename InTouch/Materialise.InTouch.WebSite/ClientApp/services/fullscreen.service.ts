import { HttpInterceptorService } from './http-interceptor-service';
import { Injectable, Inject } from '@angular/core';

import { FullScreenPostPartViewModel } from '../app/components/fullscreenMode/fullscreen-post-part-view-model';

@Injectable()
export class FullscreenService {
    private fullsecreenUrl: string = "api/Fullscreen";

    constructor( @Inject('BASE_URL') private baseUrl: string, private http: HttpInterceptorService) {
    }

    public getFullscreenModels(lastBatchDate: Date, lastPostCreateDate: Date): Promise<FullScreenPostPartViewModel[]> {
        return new Promise((resolve, reject) => {
            this.http.get(this.baseUrl + this.fullsecreenUrl + "/" + lastBatchDate.toISOString() + "/" + lastPostCreateDate.toISOString())
                .subscribe(result => {
                    resolve(result.json() as FullScreenPostPartViewModel[]);
            }, error => {
                reject();
            });
        });
    }
}