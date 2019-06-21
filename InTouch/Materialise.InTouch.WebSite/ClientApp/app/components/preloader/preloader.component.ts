import { Component } from '@angular/core';

@Component({
    selector: 'preloader',
    templateUrl: './preloader.component.html',
    styleUrls: ['./preloader.component.css']
})
export class PreloaderComponent {

    isCompatible(): boolean {

        let ua = window.navigator.userAgent;

        let isMsie = ua.indexOf('MSIE ') !== -1;
        let isTrident = ua.indexOf('Trident/') !== -1;
  
        let isCompatible = !isMsie && !isTrident;    
        
        return isCompatible;
    }
}