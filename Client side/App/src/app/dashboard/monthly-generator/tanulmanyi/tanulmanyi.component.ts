import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { GeneratorService } from 'src/app/_services/generator.service';
import { jwthelper } from 'src/app/_helpers/jwt.helper';
import { GenerateDto } from 'src/app/_models/Other/GenerateDto';
import { FilterStudentDto } from 'src/app/_models/Other/FilterStudentDto';
import { saveAs } from 'file-saver';
import { FacultyDto } from 'src/app/_models/Other/FacultyDto';
import { CalculatorService } from 'src/app/_services/calculator.service';
import alertify from 'alertifyjs';
import { TanulmanyiDto } from 'src/app/_models/Other/TanulmanyiDto';

@Component({
  selector: 'app-tanulmanyi',
  templateUrl: './tanulmanyi.component.html',
  styleUrls: ['./tanulmanyi.component.css']
})
export class TanulmanyiComponent implements OnInit {

  faculties: FacultyDto[] = [];
  userDto = new FilterStudentDto(0, 0);
  generateDto = new GenerateDto("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", null, "", "");
  tanulmanyiDto = new TanulmanyiDto("", "", "", "", "")
  disabled: any;
  checked: boolean = false;
  file: File;
  fileName: string;
  formData: FormData = new FormData();
  valid: boolean = false;

  constructor(private jwt: jwthelper, private calculator: CalculatorService, private titleService: Title, private generator: GeneratorService) {
    this.titleService.setTitle('Tanulmányi Dokumentumok');
    this.userDto.roleid = this.jwt.LoggedInRoleId();
    this.userDto.uniid = this.jwt.LoggedInUniD();
    this.calculator.getAllowedFaculty(this.userDto).subscribe(data => {
      this.makeFacultyList(data);
    });
  }

  ngOnInit() {
    if (localStorage.getItem('tanulmanyi') != null) {
      this.tanulmanyiDto = JSON.parse(localStorage.getItem('tanulmanyi'));
      this.generateDto.FunkcioTerulet = this.tanulmanyiDto.FunkcioTerulet;
      this.generateDto.PresidentName = this.tanulmanyiDto.PresidentName;
      this.generateDto.faculty = this.tanulmanyiDto.faculty;
      this.generateDto.Tan_Temanumber = this.tanulmanyiDto.Tan_Temanumber;
      this.generateDto.Koz_Temanumber = this.tanulmanyiDto.Koz_Temanumber;
    }

  }

  performClick(elemId) {
    this.file = null;
    if (this.checked == false) {
      var elem = document.getElementById(elemId);
      if (elem && document.createEvent) {
        var evt = document.createEvent("MouseEvents");
        evt.initEvent("click", true, false);
        elem.dispatchEvent(evt);
        //console.log(evt);
      }
    }
    else {
      this.fileName = null;
      this.file = null;
    }
  }

  selectFile(event): any {
    //console.log(this.checked);
    if (event == undefined) {
      alertify.set('notifier', 'position', 'top-right');
      alertify.error('Nem választottál ki fájlt!', 3);
      this.checked = false;
    } else {
      this.file = event[0];
      this.fileName = event[0].name;
      this.generateDto.file = this.file;
      //console.log(this.file);
    }
  }
  generateCheck() {
    if (this.generateDto.faculty == "" || this.generateDto.SemesterType == "" || this.generateDto.Month == "" || this.generateDto.Tan_BizonylatIkatoszam == "" || this.generateDto.Tan_Temanumber == "" || this.generateDto.FunkcioTerulet == "" || this.generateDto.PresidentName == "" || this.generateDto.Tan_summaryIktatoSzam == "") {
      alertify.set('notifier', 'position', 'top-right');
      alertify.error('Nem töltöted ki a mezőket!', 3);
    } else if (this.checked == true) {
      if (this.generateDto.Kozeleti_BizonylatIktatoszam == "" || this.generateDto.Koz_Temanumber == "" || this.generateDto.Kozeleti_SummaryIktatoszam == "") {
        alertify.set('notifier', 'position', 'top-right');
        alertify.error('Nem töltöted ki a közéleti generáláshoz a mezőket!', 3);
      } else if (this.file == undefined) {
        alertify.set('notifier', 'position', 'top-right');
        alertify.error('Nem választottál ki fájlt!', 3);
        this.checked = false;
      } else {
        this.valid = true;
        this.generate();
      }
    } else {
      this.valid = true;
      this.generate();
    }
  }
  generate() {
    if (this.valid) {
      this.generateDto.RoleId = this.userDto.roleid.toString();
      this.generateDto.Uniid = this.userDto.uniid.toString();
      this.tanulmanyiDto.FunkcioTerulet = this.generateDto.FunkcioTerulet;
      this.tanulmanyiDto.PresidentName = this.generateDto.PresidentName;
      this.tanulmanyiDto.faculty = this.generateDto.faculty;
      this.tanulmanyiDto.Koz_Temanumber = this.generateDto.Koz_Temanumber;
      this.tanulmanyiDto.Tan_Temanumber = this.generateDto.Tan_Temanumber;
      //console.log(this.generateDto);
      this.CreateFormData(this.generateDto);
      this.formData.forEach(element => {
        //console.log(element);
      });
      this.generator.DL_MonthlyStipend(this.formData).subscribe((data) => {
        localStorage.setItem('tanulmanyi', JSON.stringify(this.tanulmanyiDto));
        this.downLoadFile(data, "application/zip", this.GetFacultyName(this.generateDto.faculty) + this.generateDto.Month + "tanulmanyiUtalas.zip");
      },
        err => {
          if (err.status == 400) {
            alertify.set('notifier', 'position', 'top-right');
            alertify.error('Töltsd ki a mezőket!', 3);
          }
        })
    }
  }
  //FORMDATÁVÁ convertálja a generateDto-t
  CreateFormData(dto: GenerateDto) {
    for (var key in dto) {
      this.formData.append(key, dto[key]);
    }
  }
  downLoadFile(data: any, type: string, filename: string) {
    var blob = new Blob([data], { type: type.toString() });
    var url = window.URL.createObjectURL(blob);
    saveAs(blob, filename);
    //window.open(url);
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
}
