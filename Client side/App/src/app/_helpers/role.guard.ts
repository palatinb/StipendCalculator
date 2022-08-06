import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { UserService } from '../_services/user.service';
import { AuthGuard } from './auth.guard';
import { jwthelper } from './jwt.helper';

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {

    constructor(public auth: AuthGuard, public router: Router, private userService: UserService, private jwtHelp: jwthelper ) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
      // this will be passed from the route config
      // on the data property
      const exceptedRole = route.data.exceptedRole;

        if (this.userService.isAuthenticated() && this.jwtHelp.LoggedInRoleId() != exceptedRole) {
            this.router.navigate(['dashboard']);
            return false;
        }
        else if (!this.userService.isAuthenticated() || this.jwtHelp.LoggedInRoleId() != exceptedRole) {
            this.router.navigate(['/login'], { queryParams: { returnUrl: state.url }});
            return false;
        }
        return true;
    }
}