import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { Subscription } from 'rxjs';

import * as $ from 'jquery';

import { PostView } from '../post/shared/postView';
import { UserView } from '../user/shared/userView';

import { PostService } from '../../../services/post.service';
import { UserService } from '../../../services/user.service';
import { ToasterService } from '../../../services/toaster.service';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent implements OnInit, OnDestroy {

    private searchSubscription: Subscription;
    public count: string;
    public currentUser: UserView;
    public searchInp: string;

    public constructor(
        @Inject('BASE_URL') private baseUrl: string,
        private postService: PostService,
        private router: Router,
        private userService: UserService,
        private toasterService: ToasterService) {
        this.searchSubscription = this.postService.setSearchStr$.subscribe(
            (search) => this.searchInp = search
        );
    }

    public setupNavBar() {
        $("#navbarcollapse").on("show.bs.collapse", function () {
            if (!$('.navbar-toggler').hasClass('active')) {
                $('.navbar-toggler').toggleClass('active');
            }
        })
            .on("hide.bs.collapse", function () {
                if ($('.navbar-toggler').hasClass('active')) {
                    $('.navbar-toggler').toggleClass('active');
                }
            });
    };
    public togglerClick() {
        let togglerIsDisplayed = $('.navbar-toggler').css('display') != 'none';
        let navbarIsActive = $('.navbar-toggler').hasClass('active');
        if (togglerIsDisplayed && navbarIsActive) {
            $('.navbar-toggler').click();
        }
    }

    public showUserName(user: UserView): string {
        if (user) {
            return user.firstName + " " + user.lastName;
        } else {
            return "Loading...";
        }
    }

    ngOnInit() {
        this.setupNavBar();
        this.userService.getCurrentUser().then(user => {
            this.currentUser = user;
        }
        );
    }

    ngOnDestroy(): void {
        this.searchSubscription.unsubscribe();
    }

    onSearchSubmit(formObject: any) {
        let searchStr: string = formObject['search'];
        this.postService.setSearchStr(searchStr);
        this.router.navigate(['/posts']);
    }

    redirect() {
        this.postService.setIsMyPostsWithoutObservable(false);
        this.postService.setSearchStr(undefined);
        this.router.navigate(['/posts']);
        this.searchInp = undefined;        
    }

    redirectToMyPosts() {
        this.postService.setIsMyPosts(true);
        this.router.navigate(['/posts']);
    }
}
