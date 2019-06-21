import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PostListComponent } from './components/post/postList.component';
import { AccessPageComponent } from './components/access-page/access-page.component';
import { PostDetailComponent } from './components/post/post-detail.component';
import { CreatePostComponent } from './components/createpost/createpost.component';
import { SettingsComponent } from "./components/settings/settings.component";
import { EditPostComponent } from './components/editpost/editpost.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { FullScreenModecomponent } from './components/fullscreenMode/fullscreen-mode.component';
import { AboutComponent } from './components/about/about.component';

const routes: Routes = [

    { path: '', redirectTo: '/posts', pathMatch: 'full'},
    { path: 'posts/detail/:id', component: PostDetailComponent },
    { path: 'posts', component: PostListComponent },
    { path: 'createpost', component: CreatePostComponent },
    { path: 'posts/edit/:id', component: EditPostComponent },
    { path: 'fullscreenmode', component: FullScreenModecomponent },
    { path: 'createpost', component: CreatePostComponent },
    { path: 'settings', component: SettingsComponent },
    { path: 'Access', component: AccessPageComponent },
    { path: 'notfound', component: PageNotFoundComponent },
    { path: 'about', component: AboutComponent },
    { path: 'Home/SignedOut', redirectTo: '' },
    { path: '**', redirectTo: 'notfound' }

];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
