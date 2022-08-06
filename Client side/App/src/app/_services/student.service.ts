import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FilterStudentDto } from '../_models/Other/FilterStudentDto';
import { GroupDto } from '../_models/Other/GroupDto';
import { environment } from 'src/environments/environment';
import { UploadDto } from '../_models/Other/UploadDto';

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  //apiUrl = 'http://localhost:4000/api/student/';
  apiUrl = environment.apiUrl+"student/";

  constructor(private http: HttpClient) { }

  Upload(fileInput: FormData) {
    //console.log(fileInput);
    return this.http.post(this.apiUrl, fileInput);
  }

  Filter(studentDto: FilterStudentDto) {
    return this.http.post(this.apiUrl + 'filter', studentDto);
  }

  EditStudent(student: object) {
    return this.http.put(this.apiUrl + 'update', student);
  }

  getStudentsInGroup(groupdto: GroupDto) {
    return this.http.post(this.apiUrl + 'group', groupdto);
  }
}
