import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UniDto } from '../_models/UniDto';
import { retry } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UniService {

  //apiUrl = 'http://localhost:4000/api/uni/';
  apiUrl = environment.apiUrl+"uni/";

  constructor(private http: HttpClient) { }

  GetAllUni() {
    return this.http.get(this.apiUrl).pipe(
      retry(2)
    );
  }

  AddUni(university: UniDto) {
    return this.http.post<any>(this.apiUrl, university);
  }
  DeleteUni(uniId: number) {
    return this.http.delete(this.apiUrl + uniId);
  }
}
