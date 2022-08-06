import { Component, OnInit } from '@angular/core';
import { jwthelper } from 'src/app/_helpers/jwt.helper';
import { FacultyDto } from 'src/app/_models/Other/FacultyDto';
import { FilterStudentDto } from 'src/app/_models/Other/FilterStudentDto';
import { CalculatorService } from 'src/app/_services/calculator.service';
import { Title } from '@angular/platform-browser';
import { GenerateDto } from 'src/app/_models/Other/GenerateDto';
import { GeneratorService } from 'src/app/_services/generator.service';
import { saveAs } from 'file-saver';
import alertify from 'alertifyjs';

@Component({
  selector: 'app-ocsi-mutato',
  templateUrl: './ocsi-mutato.component.html',
  styleUrls: ['./ocsi-mutato.component.css']
})
export class OcsiMutatoComponent implements OnInit {

  faculties: FacultyDto[] = [];
  userDto = new FilterStudentDto(0, 0);
  generateDto = new GenerateDto("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", null, "", "");
  disabled: any;

  constructor(private jwt: jwthelper, private calculator: CalculatorService, private titleService: Title, private generator: GeneratorService) {
    this.titleService.setTitle('Ösztöndíj mutató & ÖCSI');
    this.userDto.roleid = this.jwt.LoggedInRoleId();
    this.userDto.uniid = this.jwt.LoggedInUniD();
    this.calculator.getAllowedFaculty(this.userDto).subscribe(data => {
      //console.log(data);
      this.makeFacultyList(data);
    });
  }

  ngOnInit() {
  }

  generateOCSI() {
    if (this.generateDto.SemesterType == "" || this.generateDto.faculty == "") {
      alertify.set('notifier', 'position', 'top-right');
      alertify.error('Töltsd ki a mezőket!', 3);
    } else {
    this.generateDto.RoleId = this.userDto.roleid.toString();
    this.generateDto.Uniid = this.userDto.uniid.toString();
    //console.log(this.generateDto);
    this.generator.DL_ÖCSI(this.generateDto).subscribe((data) => {
      this.downLoadFile(data, "application/vnd.ms-word", this.GetFacultyName(this.generateDto.faculty) + " ÖCSI.docx");
    })
  }
  }
  generateMutato() {
    if (this.generateDto.SemesterType == "" || this.generateDto.faculty == "") {
      alertify.set('notifier', 'position', 'top-right');
      alertify.error('Töltsd ki a mezőket!', 3);
    } else {
      this.generateDto.RoleId = this.userDto.roleid.toString();
      this.generateDto.Uniid = this.userDto.uniid.toString();
      this.generator.DL_Mutatók(this.generateDto).subscribe((data) => {
        this.downLoadFile(data, "application/zip", this.GetFacultyName(this.generateDto.faculty) + " Ösztöndíj mutatók " + this.GetSemester() + ".zip");
      })
    }
  }

  GetSemester() {
    //$"{DateTime.Now.Year}/{DateTime.Now.AddYears(1).Year.ToString().Remove(0,2)}/1" : $"{DateTime.Now.AddYears(-1).Year}/{DateTime.Now.Year.ToString().Remove(0,2)}/2";
    return this.generateDto.SemesterType == "1" ? new Date().getFullYear() + "/" + (new Date().getFullYear() + 1).toString().slice(2, 4) + "/1" : new Date().getFullYear() - 1 + "/" + new Date().getFullYear().toString().slice(2, 4) + "/2"
  }

  GetFacultyName(facultyCode: string): string {
    return this.faculties.find(q => q.modulCodeStart == facultyCode).name
  }

  makeFacultyList(facultyList: any) {
    facultyList.forEach(element => {
      switch (element) {
        case 'A': { this.faculties.push(new FacultyDto(element, 'OE AMK')); break; }
        case 'B': { this.faculties.push(new FacultyDto(element, 'OE BGK')); break; }
        case 'G': { this.faculties.push(new FacultyDto(element, 'OE KGK')); break; }
        case 'K': { this.faculties.push(new FacultyDto(element, 'OE KVK')); break; }
        case 'N': { this.faculties.push(new FacultyDto(element, 'OE NIK')); break; }
        case 'R': { this.faculties.push(new FacultyDto(element, 'OE RKK')); break; }
      }
    });
  }

  downLoadFile(data: any, type: string, filename: string) {
    var blob = new Blob([data], { type: type.toString() });
    var url = window.URL.createObjectURL(blob);
    saveAs(blob, filename);
    //window.open(url);
  }
}