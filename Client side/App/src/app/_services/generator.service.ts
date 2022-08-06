import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { GenerateDto } from '../_models/Other/GenerateDto';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GeneratorService {
  
  //apiUrl = 'http://localhost:4000/api/generator/';
  apiUrl = environment.apiUrl+"generator/";
  
  constructor(private http: HttpClient) { }

  DL_ElnökiHatarozat(generateDto : GenerateDto){
    return this.http.post(this.apiUrl+'elnok',generateDto,{responseType: 'blob' });
  }

  DL_ÖCSI(generateDto : GenerateDto){
    return this.http.post(this.apiUrl+'ocsi',generateDto,{responseType: 'blob' });
  }

  DL_Mutatók(generateDto : GenerateDto){
    return this.http.post(this.apiUrl+'mutato',generateDto,{responseType: 'blob' });
  }

  DL_MonthlyStipend(formData : FormData){
    return this.http.post(this.apiUrl+'tanulmanyi',formData,{responseType: 'blob' });
  }
}
