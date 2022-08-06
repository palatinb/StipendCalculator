import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  //apiUrl = 'http://localhost:4000/api/role';
  apiUrl = environment.apiUrl+"role/";  
  
  constructor(private http: HttpClient) { }


  GetAllRole() {
    return this.http.get(this.apiUrl);
  }
} 
