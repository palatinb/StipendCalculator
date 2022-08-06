import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { UserLoginDto } from '../_models/User/UserLoginDto';
import { throwError } from 'rxjs';
import { UserDto } from '../_models/User/UserDto';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, catchError, retry } from 'rxjs/operators';
import { UserRegisterDto } from '../_models/User/UserRegisterDto';
import { jwthelper } from '../_helpers/jwt.helper';
import { UserModifiedDto } from '../_models/User/UserModifiedDto';
import { CheckOldPwDto } from '../_models/Other/CheckOldPwDto';
import { UserNotAdminDto } from '../_models/User/UserNotAdminDto';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  //apiUrl = 'http://localhost:4000/api/user/';
  apiUrl = environment.apiUrl+"user/";

  private currentUserSubject: BehaviorSubject<UserDto>;
  public currentUser: Observable<UserDto>;
  helper = new JwtHelperService();

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<UserDto>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }


  /* login(user: LoginUserDto) {
    return this.http.post<any>(this.apiUrl + 'login', user)
      .pipe(catchError(this.errorHandler));
  } */

  errorHandler(error: HttpErrorResponse) {
    return throwError(error);
  }

  public get currentUserValue(): UserDto {
    return this.currentUserSubject.value;
  }
  public isAuthenticated(): boolean {
    //console.log(this.currentUserValue);
    if (this.currentUserValue) { return true; } else { return false; }
  }
  public isExpired():boolean {
     var decodeToken = this.helper.decodeToken(this.currentUserValue.token);
      if (decodeToken.exp == undefined) return false;

      const date = new Date(0);
      date.setUTCSeconds(decodeToken.exp);
      if (date == undefined) {return false;} else
      {
      var expired =  !(date.valueOf() > new Date().valueOf())

      return expired;
      }
  }
  public isExpired2() : boolean {
    var decodeToken = this.helper.decodeToken(this.currentUserValue.token);
    const date = new Date(0);
      date.setUTCSeconds(decodeToken.exp);
      var expired = this.helper.isTokenExpired(this.currentUserValue.token);
      return expired;
  }
  // Regisztráció
  Register(user: UserRegisterDto) {
    return this.http.post<any>(this.apiUrl + 'register', user);
  }

  // Bejelentkezés
  login(user: UserLoginDto) {
    return this.http.post<any>(this.apiUrl + 'login', user)
      .pipe(map(user => {
        // store user details and jwt token in local storage to keep user logged in between page refreshes
        localStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user);
        return user;
      }));
  }
  // Kijelentkezés
  logout() {
    // remove user from local storage and set current user to null
    localStorage.removeItem('currentUser');
    sessionStorage.removeItem("userbase");
    this.currentUserSubject.next(null);
  }
  ModifybyA(user: UserModifiedDto) {
    return this.http.put(this.apiUrl + "bya", user);
  }
  Modify(user: UserNotAdminDto) {
    return this.http.put(this.apiUrl, user);
  }
  // visszaadja az összes Usert
  getMembers() {
    return this.http.get(this.apiUrl).pipe(
      retry(3),
    );
  }
  // Visszaad egy bizonyos IDu Usert
  getUser(userId: number) {
    return this.http.get(this.apiUrl + userId);
  }
  deleteUser(userId: number) {
    return this.http.delete(this.apiUrl + userId);
  }

  checkOldPw(dto: CheckOldPwDto) {
    return this.http.post(this.apiUrl + "check", dto)
  }
  checkUsername(user: UserRegisterDto) {
    return this.http.post(this.apiUrl + "checkuser", user);
  }
  checkUsername2(user: UserModifiedDto) {
    return this.http.post(this.apiUrl + "checkuser", user);
  }
}
