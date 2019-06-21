import { NgModule, ErrorHandler } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { FileUploadModule } from "ng2-file-upload";
import { ImageUploadModule } from 'angular2-image-upload';
import { CarouselModule } from 'ngx-bootstrap';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './components/app/app.component';
import { AccessPageComponent } from './components/access-page/access-page.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { PostListComponent } from './components/post/postList.component';
import { PostDetailComponent } from './components/post/post-detail.component';
import { CreatePostComponent } from './components/createpost/createpost.component';
import { EditPostComponent } from './components/editpost/editpost.component';
import { PreloaderComponent } from './components/preloader/preloader.component';
import { FullScreenModecomponent } from './components/fullscreenMode/fullscreen-mode.component';
import { UsersListComponent } from "./components/user/users-list.component";
import { SettingsComponent } from "./components/settings/settings.component";
import { PostInfoComponent } from "./components/post/postinfo/postinfo.component";
import { CommentComponent } from "./components/comment/comment/comment.component";
import { LatestCommentsComponent } from './components/comment/latest-comments/latest-comments.component';
import { CommentListComponent } from "./components/comment/comment-list.component";
import { AboutComponent } from './components/about/about.component';

import { TruncatePipe } from '../app/helpers/text-truncate';

import { AppConfigModule } from './app-config.module';
import { AppRoutingModule } from './app-routing.module';
import { NgxGalleryModule } from 'ngx-gallery';
import { TooltipModule } from 'ngx-bootstrap/tooltip';

import { SessionService } from '../services/session.service';
import { PostService } from '../services/post.service';
import { FileService } from '../services/file.service';
import { PostDetailService } from '../services/post.detail.service';
import { UserService } from '../services/user.service';
import { ToasterService } from '../services/toaster.service';
import { AccessService } from '../services/Access.service';
import { CommentService } from '../services/comment.service';
import { HttpInterceptorService } from '../services/http-interceptor-service';
import { SharedModule, ConfirmDialogModule, ConfirmationService, GrowlModule, EditorModule, CalendarModule } from 'primeng/primeng';
import { MessageService } from 'primeng/components/common/messageservice';
import { FullscreenService } from '../services/fullscreen.service';

import { InfiniteScrollModule } from 'angular2-infinite-scroll';
@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        CreatePostComponent,
        PostListComponent,
        PostDetailComponent,
        PreloaderComponent,
        FullScreenModecomponent,
        EditPostComponent,
        UsersListComponent,
        AccessPageComponent,
        SettingsComponent,
        PageNotFoundComponent,
        PostInfoComponent,
        CommentComponent,
        LatestCommentsComponent,
        TruncatePipe,
        CommentListComponent,
        AboutComponent
    ],
    imports: [
        NgxGalleryModule,
        CarouselModule.forRoot(),
        TooltipModule.forRoot(),
        BrowserAnimationsModule,
        CommonModule,
        HttpModule,
        FormsModule,
        FileUploadModule,
        ImageUploadModule.forRoot(),
        AppConfigModule,
        AppRoutingModule,
        EditorModule,
        SharedModule,
        ConfirmDialogModule,
        GrowlModule,
        InfiniteScrollModule,
        CalendarModule
    ],
    providers: [
        SessionService,
        HttpInterceptorService,
        PostService,
        FileService,
        PostDetailService,
        UserService,
        ToasterService,
        AccessService,
        ConfirmationService,
        MessageService,
        CommentService,
        FullscreenService
    ]
})
export class AppModuleShared {
}
