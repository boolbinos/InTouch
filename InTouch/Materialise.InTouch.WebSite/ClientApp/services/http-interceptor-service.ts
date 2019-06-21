import { Http, Request, RequestOptions, RequestOptionsArgs, Response, XHRBackend } from "@angular/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Rx";
import { Router, NavigationEnd } from "@angular/router";
import { Message } from 'primeng/components/common/api';
import { MessageService } from 'primeng/components/common/messageservice';
import { Event } from "@angular/router";

// operators
import "rxjs/add/operator/catch"
import "rxjs/add/observable/throw"
import "rxjs/add/operator/map"

@Injectable()
export class HttpInterceptorService extends Http {
    msgs: Message[] = [];
    autoClose: boolean = true;

    constructor(
        private messageService: MessageService,
        backend: XHRBackend,
        options: RequestOptions,
        public http: Http,
        private router: Router) {
        super(backend, options);
    }

    public request(url: string | Request, options?: RequestOptionsArgs): Observable<Response> {
        return super.request(url, options)
            .catch(error => this.handleError(error));
    }

    public handleError(error: Response) {
       
  

        switch (error.status) {
            case 404: {
                let notFound = "notfound";
                window.history.pushState({}, 'previous', "");
                this.router.navigate([notFound]);
                break;
            }
            case 0: {
                location.reload();
                break;
            }
            default: {
                window.history.pushState({}, 'previous', "");
                this.router.navigate([document.referrer])
                    .then(x => {
                        let errorMessage = error.text();
                        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: errorMessage });
                    });
            }
        }


        return Observable.throw(error);
    }
}
