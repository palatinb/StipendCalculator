import { Component, OnInit } from '@angular/core';
import { FilterStudentDto } from 'src/app/_models/Other/FilterStudentDto';
import { StudentService } from 'src/app/_services/student.service';
import { jwthelper } from 'src/app/_helpers/jwt.helper';
import { GeneratorService } from 'src/app/_services/generator.service';
import { Title } from '@angular/platform-browser';


@Component({
  selector: 'app-monthly-generator',
  templateUrl: './monthly-generator.component.html',
  styleUrls: ['./monthly-generator.component.css']
})
export class MonthlyGeneratorComponent implements OnInit {

  constructor(private titleService: Title) {
    this.titleService.setTitle('Ösztöndíj kalkulátor / Dashbboard / Elnöki határozat');
  }

  ngOnInit() {
    
  }

 

 

  mutato() {

  }

  tanulmanyi() {

  }

  

}
