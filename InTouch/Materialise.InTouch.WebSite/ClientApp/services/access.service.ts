import { Injectable } from '@angular/core';
import { Headers } from '@angular/http';
import { Router } from '@angular/router';



@Injectable()
export class AccessService {
    constructor(private router: Router) {}

    public CheckUnAuthorizeUser(errors: any): void
    {
        let getStatusCode = new Headers(errors).get("status") as string;
        if (getStatusCode=="403")
        {
            this.router.navigate(['Access']);
        }
        return;
    }
}