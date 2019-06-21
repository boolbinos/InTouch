import { Component, Inject, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';

import { UserView } from './shared/userView';
import { UserService } from '../../../services/user.service';

import { ToasterService } from '../../../services/toaster.service';
import { PostService } from '../../../services/post.service';


@Component({
    selector: 'users-list',
    templateUrl: './users-list.component.html'
})

export class UsersListComponent implements OnInit {
    public users: UserView[];
    public currentUser: UserView;
    public count: string;


    constructor(private userService: UserService,
        private router: Router,
        private postService: PostService,
        private toasterService: ToasterService) {
    }

    getUsers(): void {
        this.userService
            .getUsers()
            .then(users => this.users = users);
    }

    assignRole(user: UserView) {
        this.userService.
            assignUserRole(user)
            .then(user => {
                let index: number = this.users.findIndex(userFind => userFind.id === user.id);
                this.users[index] = user;
            });
    }


    ngOnInit(): void {
        this.getUsers();
        this.userService.getCurrentUser().then(user => { this.currentUser = user });
    }
}