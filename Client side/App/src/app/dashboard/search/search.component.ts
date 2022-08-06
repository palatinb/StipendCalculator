import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { StudentService } from 'src/app/_services/student.service';
import { FilterStudentDto } from 'src/app/_models/Other/FilterStudentDto';
import { jwthelper } from 'src/app/_helpers/jwt.helper';
import { StudentDto } from 'src/app/_models/Student/StudentDto';
import { BehaviorSubject, from } from 'rxjs';
import { StudentEditDto } from '../../_models/Student/StudentEditDto';
import { MatPaginator, MatTableDataSource } from '@angular/material';
import { LZStringService } from 'ng-lz-string';
import alertify from 'alertifyjs';
import { Title } from '@angular/platform-browser';
import { trigger, state, transition, style, animate } from '@angular/animations';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('1000ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class SearchComponent implements OnInit {
  displayedColumns: string[] = [
    'neptunCode',
    'modulCode',
    'modulName',
    /*     'telephelyName',
        'year',
        'studentGrop', */
    'activeSemester',
    /* 'finishedSemester', */
    'passiveSemester',
    /*     'allSemesters',
        'accceptedCredit',*/
    'earnedCredit',
    'exceptedCredit',
    'creditIndex',
    /*     'stipendIndex',
        'groupIndex',
        'stipendAmmount', */
    'financialState',
    /*  'yearOfEnrollment' */
  ];

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  userDto = new FilterStudentDto(0, 0);
  EditModel = new StudentDto(null, '', '', '',null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,null);
  public dataSource: MatTableDataSource<StudentDto>;
  public Students: StudentDto[];
  public astudents: string;
  IsLoadingFinished: boolean = false;
  ResultLength = 0;
  noData = true;
  noResult = true;
  //neptuncodes:StudentStorageDto[];
  private currentStudentBaseSubject: BehaviorSubject<StudentDto>;
  noNetwork = true;
  expandedElement: StudentDto | null;

  constructor(private studentservice: StudentService, private jwt: jwthelper, private lz: LZStringService, private titleService: Title) {
    this.titleService.setTitle('Keresés');
  }

  EditMember(member) {
    this.EditModel.id = member.id;
    this.EditModel.neptuncode = member.neptunCode;
    this.EditModel.modulcode = member.modulCode;
    this.EditModel.modulname = member.modulName;
    this.EditModel.telephely = member.telephelyName;
    this.EditModel.year = member.year;
    this.EditModel.studentgroup = member.studentGrop;
    this.EditModel.activesemester = member.activeSemester;
    this.EditModel.passivesemester = member.passiveSemester;
    this.EditModel.earnedcredit = member.earnedCredit;
    this.EditModel.creditindex = member.creditIndex;
    this.EditModel.earnedcredit = member.earnedCredit;
    this.EditModel.financialstate = member.financialState;
    this.EditModel.acceptedcredit = member.accceptedCredit;
    this.EditModel.allsemester = member.allSemesters;
    this.EditModel.stipendindex = member.stipendIndex;
    this.EditModel.groupindex = member.groupIndex;
    this.EditModel.exceptedcredit = member.exceptedCredit;
    this.EditModel.YearOfEnrollment = member.yearOfEnrollment;
    this.EditModel.finishedsemester = member.finishedSemester;
    this.EditModel.stipendamount = member.stipendAmmount;
  }

  EditStudent() {
    if(this.EditModel.earnedcredit < 0) {
      alertify.set('notifier', 'position', 'top-right');
      alertify.error('A megszerzett kreditek száma nem lehet mínusz!', 3);
    } else {
      this.studentservice.EditStudent(this.EditModel).subscribe(data =>{
        //console.log(data);
        sessionStorage.removeItem('userbase');
        this.loadData();
        alertify.set('notifier', 'position', 'top-right');
        alertify.success('Sikeresen módosítottad!', 3);
      },
      error => {
        alertify.set('notifier', 'position', 'top-right');
        alertify.error('Nem sikerült módosítani!', 3);
      });
    }
  }

  async ngOnInit() {
    await this.loadData();
  }
  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
    if(this.dataSource.filteredData.length==0){
      this.noResult = false;
      this.noData = true;
    }else{
      this.noResult = true;
      this.noData = true;
   
    }
    this.IsLoadingFinished = false;
  }
  async loadData() {
    this.currentStudentBaseSubject = new BehaviorSubject<StudentDto>(JSON.parse(sessionStorage.getItem("usrbase")));
    this.astudents = sessionStorage.getItem("userbase");

    if (this.astudents == null)
    {
      this.userDto.roleid = this.jwt.LoggedInRoleId();
      this.userDto.uniid = this.jwt.LoggedInUniD();
      this.IsLoadingFinished = true;
      this.noData = false;
      if(!navigator.onLine) {
        this.noNetwork = false;
        this.noData = true;
      }

      await this.studentservice.Filter(this.userDto).subscribe(data =>
        {
        //this.neptuncodes = data as StudentStorageDto[];
        this.Students = data as StudentDto[];
        this.dataSource = new MatTableDataSource(this.Students);
        var compressed = this.lz.compress(JSON.stringify(this.Students));
        sessionStorage.setItem("userbase", compressed);
        this.IsLoadingFinished = false;
        this.ResultLength = this.Students.length;
        this.addPagination();
        });
    }
    else {
      this.Students = JSON.parse(this.lz.decompress(sessionStorage.getItem("userbase")));
      //console.log(this.Students);
      this.dataSource = new MatTableDataSource(this.Students);
      this.addPagination();
    }
  }
  addPagination() {
    this.dataSource.paginator = this.paginator;
  }
}

