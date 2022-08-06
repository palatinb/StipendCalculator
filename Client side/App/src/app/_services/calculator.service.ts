import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FilterStudentDto } from '../_models/Other/FilterStudentDto';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CalculatorService {
  //apiUrl = 'http://localhost:4000/api/calculator/';
  apiUrl = environment.apiUrl+"calculator/";
  
  constructor(private http: HttpClient) { }

  getAllowedFaculty(userDto: FilterStudentDto)
  {
    return this.http.post(this.apiUrl+"faculty", userDto)
  }
  getCalculated(data: any) {
    return this.http.post(this.apiUrl + "calculate", data);
  }
  postStudents(students: any) {
    return this.http.put(this.apiUrl + "save", students);
  }
}
