import { NgModule, InjectionToken } from '@angular/core';

export let APP_CONFIG = new InjectionToken<AppConfig>('app.config');

export class AppConfig {
    shortContentLength: number;
    intervalForScrolling: number;
}

export const APP_DI_CONFIG: AppConfig = {
    shortContentLength: 300,
    intervalForScrolling: 1000
};

@NgModule({
    providers: [
        { provide: APP_CONFIG, useValue: APP_DI_CONFIG },
        { provide: 'SHORT_CONTENT_LENGTH', useValue: APP_DI_CONFIG.shortContentLength },
        { provide: 'IntervalForScrolling', useValue: APP_DI_CONFIG.intervalForScrolling }
    ]
})
export class AppConfigModule { }