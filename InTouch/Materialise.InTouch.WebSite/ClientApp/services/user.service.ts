import { Injectable, Inject, OnInit } from '@angular/core';
import { Headers, Http } from '@angular/http';
import { Router } from '@angular/router';

import { UserView } from '../app/components/user/shared/userView'
import { AccessService } from './Access.service';
import { HttpInterceptorService } from './http-interceptor-service';

@Injectable()
export class UserService implements OnInit {
    private headers = new Headers({ 'Content-Type': 'application/json' });
    private userUrl = 'api/User';
    private accountSignOutUrl = 'Account/SignOut';
    private assignUserRoleUrl = 'api/User/assignRole';
    private getCurrentUserUrl = 'api/User/GetCurrentUser';

    public currentUser: UserView;

    constructor(private http: HttpInterceptorService,
        @Inject('BASE_URL') private baseUrl: string,
        private router: Router, private access: AccessService) {
    }

    public getUser(id: number): Promise<UserView> {
        return new Promise(resolve => {
            this.http.get(this.baseUrl + this.userUrl + "/" + id).subscribe(result => {
                resolve(result.json() as UserView);
            }, error => {
                this.access.CheckUnAuthorizeUser(error);
            }
            )
        });
    }

    public getUsers(): Promise<UserView[]> {
        return new Promise(resolve => {
            this.http.get(this.baseUrl + this.userUrl).subscribe(result => {
                resolve(result.json() as UserView[]);
            }, error => {
                this.access.CheckUnAuthorizeUser(error);
                console.log(error);
            })
        });
    }

    isModerator(user: UserView): boolean {
        return user.roleName === 'Moderator';
    }

    public assignUserRole(user: UserView): Promise<UserView> {

        let roleName: string = (user.roleName === 'Moderator') ? 'User' : 'Moderator';

        return new Promise((resolve, reject) => {
            this.http.post(this.baseUrl + this.assignUserRoleUrl +
                "/?userId=" + user.id +
                "&roleName=" + roleName,
                { headers: this.headers })
                .toPromise()
                .then(res => {
                    user.roleName = roleName
                },
                msg => {
                });
        });
    }

    public getCurrentUser(): Promise<UserView> {
        return new Promise<UserView>(resolve => {
            if (!this.currentUser) {
                this.http.get(this.baseUrl + this.getCurrentUserUrl).subscribe(result => {
                    this.currentUser = result.json() as UserView;                    
                    resolve(this.currentUser);
                }, error => {
                   this.access.CheckUnAuthorizeUser(error);
                    console.log(error);
                }
                );
            } else {
                resolve(this.currentUser);
            }
        });
    }

    ngOnInit() {

    }


}