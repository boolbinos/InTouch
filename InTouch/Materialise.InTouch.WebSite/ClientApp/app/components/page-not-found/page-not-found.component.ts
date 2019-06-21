import { Component } from '@angular/core';

@Component({
    selector: 'page-not-found',
    templateUrl: 'page-not-found.component.html',
    styleUrls: ['page-not-found.component.css']
})
export class PageNotFoundComponent {
    isCompatible(): boolean {

        let ua = window.navigator.userAgent;

        let isMsie = ua.indexOf('MSIE ') !== -1;
        let isTrident = ua.indexOf('Trident/') !== -1;

        let isCompatible = !isMsie && !isTrident;

        return isCompatible;
    }
}