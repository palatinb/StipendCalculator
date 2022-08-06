import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class jwthelper {
    
    helper = new JwtHelperService();

    constructor() { }

    LoggedInUserID()
    {
        var userOBject = localStorage.getItem("currentUser");
        var user = JSON.parse(userOBject);
        var token = this.helper.decodeToken(user.token);
        return token.id;
    }
    LoggedInUser()
    {
        var userOBject = localStorage.getItem("currentUser");
        var user = JSON.parse(userOBject);
        var token = this.helper.decodeToken(user.token);
        return token;
    }
    LoggedInRoleId()
    {
        var userOBject = localStorage.getItem("currentUser");
        var user = JSON.parse(userOBject);
        var token = this.helper.decodeToken(user.token);
        return token.roleid;
    }
    LoggedInUniD()
    {
        var userOBject = localStorage.getItem("currentUser");
        var user = JSON.parse(userOBject);
        var token = this.helper.decodeToken(user.token);
        return token.uniid;
    }
    LoggedInUserName()
    {
        var userObject = localStorage.getItem("currentUser");
        var user = JSON.parse(userObject);
        var token = this.helper.decodeToken(user.token);
        return token.name;
    }
    isExpired()
    {
        var userOBject = localStorage.getItem("currentUser");
        var user = JSON.parse(userOBject);
        var token = this.helper.decodeToken(user.token);
        var expired = this.helper.isTokenExpired(token);
        return expired;
    }
}