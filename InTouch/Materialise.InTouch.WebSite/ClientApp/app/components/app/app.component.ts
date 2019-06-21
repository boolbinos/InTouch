import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Title } from '@angular/platform-browser';
import { SessionService } from '../../../services/session.service';

import * as $ from 'jquery';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

    public hideNav: number;
    public url: string;
    public route: string;

    constructor(location: Location,
        private titleService: Title,
        private sessionService: SessionService) {
        location.path() == '/fullscreenmode' ? this.hideNav = 1 : this.hideNav = 2;
        this.titleService.setTitle("InTouch news");
    }

    ngOnInit(): void {
        this.sessionService.start();
    }
}
