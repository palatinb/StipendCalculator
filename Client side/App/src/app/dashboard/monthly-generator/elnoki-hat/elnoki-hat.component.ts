import { Component, OnInit } from '@angular/core';
import { FacultyDto } from 'src/app/_models/Other/FacultyDto';
import { FilterStudentDto } from 'src/app/_models/Other/FilterStudentDto';
import { CalculatorService } from 'src/app/_services/calculator.service';
import { StudentService } from 'src/app/_services/student.service';
import { jwthelper } from 'src/app/_helpers/jwt.helper';
import { GenerateDto } from 'src/app/_models/Other/GenerateDto';
import { GeneratorService } from 'src/app/_services/generator.service';
import { saveAs } from 'file-saver';
import { Title } from '@angular/platform-browser';
import alertify from 'alertifyjs';
import { SavePresHatDto } from 'src/app/_models/Other/SavePresHatDto';

@Component({
  selector: 'app-elnoki-hat',
  templateUrl: './elnoki-hat.component.html',
  styleUrls: ['./elnoki-hat.component.css']
})
export class ElnokiHatComponent implements OnInit {
  isKvk = false;
  faculties: FacultyDto[] = [];
  userDto = new FilterStudentDto(0, 0);
  generateDto = new GenerateDto("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", null, "", "");
  saveDto = new SavePresHatDto("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");

  constructor(private calculator: CalculatorService, private jwt: jwthelper, private studentserv: StudentService, private generator: GeneratorService, private titleService: Title) {
    this.titleService.setTitle('Elnöki határozat');
    this.userDto.roleid = this.jwt.LoggedInRoleId();
    this.userDto.uniid = this.jwt.LoggedInUniD();
    if (this.userDto.roleid == 5) { this.isKvk = true };
  }

  ngOnInit() {
    this.calculator.getAllowedFaculty(this.userDto).subscribe(data => {
      this.makeFacultyList(data);
      if (localStorage.getItem("presdto") != null) {
        this.generateDto = JSON.parse(localStorage.getItem("presdto"))
      }
    });
  }
  generateCheck() {
    if (this.generateDto.faculty == "" || this.generateDto.Month == "" || this.generateDto.ETPay == "" || this.generateDto.ElnokiIktato == "" ||
         this.generateDto.PresidentName == "" || this.generateDto.PresidentNeptun == "" ||
          this.generateDto.VicePresidentName == "" || this.generateDto.VicePresidentNeptun == "" || this.generateDto.VicePresidentPercent == null || 
          this.generateDto.GazdName == "" || this.generateDto.GazdNeptun == "" || this.generateDto.GazdPercent == null || 
          this.generateDto.EfName == "" || this.generateDto.EfNeptun == "" || this.generateDto.EfPercent == null ||
           this.generateDto.PrName == "" || this.generateDto.PrNeptun == ""|| this.generateDto.PrPercent == null ) {
       alertify.set('notifier', 'position', 'top-right');
       alertify.error('Töltsd ki a mezőket!', 3);
     }else if (this.isKvk){
      if (this.generateDto.Ef2_Name == "" || this.generateDto.Ef2_Neptun == "" || this.generateDto.Ef2Percent == null) {
      alertify.set('notifier', 'position', 'top-right');
      alertify.error('Töltsd ki a mezőket!', 3);
      }
    } else {
      this.generate();
    }
    
  }
  generate() {
    this.generateDto.RoleId = this.userDto.roleid.toString();
    this.generateDto.Uniid = this.userDto.uniid.toString();
    this.SavePresHatDto();
    //console.log(this.generateDto);
    //var months = this.generateDto.Month.split("-");
    /*     if (months.length > 1) {
          this.generator.DL_ElnökiHatarozat(this.generateDto).subscribe((data) => {
            this.downLoadFile(data, "application/zip", "elnökiHatarozat" + this.generateDto.Month + ".zip");
          })
        } else */
    {
      this.generator.DL_ElnökiHatarozat(this.generateDto).subscribe((data) => {
        this.downLoadFile(data, "application/vnd.ms-word", this.generateDto.ElnokiIktato + " Elnöki határozat " + this.generateDto.Month + ".docx");
      },
        err => {
          if (err.status == 400) {
            alertify.set('notifier', 'position', 'top-right');
            alertify.error('Töltsd ki a mezőket!', 3);
          }
        })
    }
  }

  downLoadFile(data: any, type: string, filename: string) {
    var blob = new Blob([data], { type: type.toString() });
    var url = window.URL.createObjectURL(blob);
    saveAs(blob, filename);
    //window.open(url);
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
  SavePresHatDto() {
    this.saveDto.ETPay = this.generateDto.ETPay;
    this.saveDto.PresidentName = this.generateDto.PresidentName;
    this.saveDto.PresidentNeptun = this.generateDto.PresidentNeptun;
    this.saveDto.VicePresidentName = this.generateDto.VicePresidentName;
    this.saveDto.VicePresidentNeptun = this.generateDto.VicePresidentNeptun;
    this.saveDto.VicePresidentPercent = this.generateDto.VicePresidentPercent;
    this.saveDto.PrName = this.generateDto.PrName;
    this.saveDto.PrNeptun = this.generateDto.PrNeptun;
    this.saveDto.PrPercent = this.generateDto.PrPercent;
    this.saveDto.GazdName = this.generateDto.GazdName;
    this.saveDto.GazdNeptun = this.generateDto.GazdNeptun;
    this.saveDto.GazdPercent = this.generateDto.GazdPercent;
    this.saveDto.EfName = this.generateDto.EfName;
    this.saveDto.EfNeptun = this.generateDto.EfNeptun;
    this.saveDto.EfPercent = this.generateDto.EfPercent;
    this.saveDto.Ef2_Name = this.generateDto.Ef2_Name;
    this.saveDto.Ef2_Neptun = this.generateDto.Ef2_Neptun;
    this.saveDto.Ef2Percent = this.generateDto.Ef2Percent;
    this.saveDto.faculty = this.generateDto.faculty;


    localStorage.setItem('presdto', JSON.stringify(this.saveDto));
  }
}
