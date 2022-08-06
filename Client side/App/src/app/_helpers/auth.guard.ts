import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { UserService } from '../_services/user.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private userService: UserService
    ) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) : boolean {
        if (this.userService.isAuthenticated() && !this.userService.isExpired2() ) {
            // authorised so return true 
            //this.router.navigate(['/dashboard'], { queryParams: {returnUrl: state.url }});
            return true;
        } 

        // not logged in so reserdirect to login page with the return url
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url }});
        return false;
    }
}