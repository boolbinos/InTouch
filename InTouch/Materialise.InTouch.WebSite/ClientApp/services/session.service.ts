import { Injectable, Inject } from '@angular/core';
import { Headers, Http } from '@angular/http';
import { Router } from '@angular/router';

import 'rxjs/add/operator/toPromise';

@Injectable()
export class SessionService {
    private keepSessionAliveUrl: string = 'Home/KeepSessionAlive';

    private readonly ticksInMinute = 1000 * 60;
    private requestPeriod: number = 15;  // default session time = 20


    constructor( @Inject('BASE_URL') private baseUrl: string,
        private http: Http,
        private router: Router) { }


    public start(): void {
        setInterval(() => this.keepSessionAlive(), this.ticksInMinute * this.requestPeriod);
    }

    keepSessionAlive(): void {
        this.http
            .get(this.baseUrl + this.keepSessionAliveUrl)
            .toPromise()
            .catch(error => { });
    }

}