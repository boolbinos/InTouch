import { Component } from '@angular/core';
import { ToasterService } from '../../../services/toaster.service';
import { PostService } from '../../../services/post.service';
import { Title } from '@angular/platform-browser';
@Component({
    selector: 'settings',
    templateUrl: './settings.component.html'
})
export class SettingsComponent {

    public count: string;

    constructor(
        private titleService: Title,
        private postService: PostService,
        private toasterService: ToasterService) {
        this.titleService.setTitle("Settings");
    }

    importPosts(provider: string): void {
        this.toasterService.showSnackbar("Import Started");
        this.postService.importPosts(provider).then(result => {
            this.count = result as string;
            if (parseInt(this.count) === 0) {
                this.toasterService.showSnackbar("Everything is already up-to-date");
            }
            else {
                this.toasterService.showSnackbar(provider+" posts have been successfully uploaded");
            }
        });
    }
}