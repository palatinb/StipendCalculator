import { Component, OnInit, ViewChild } from '@angular/core';
import { CalculateDto } from 'src/app/_models/Other/CalculateDto';
import { StudentCalculationDto } from 'src/app/_models/Student/StudentCalculationDto';
import { jwthelper } from 'src/app/_helpers/jwt.helper';
import { CalculatorService } from 'src/app/_services/calculator.service';
import { FilterStudentDto } from 'src/app/_models/Other/FilterStudentDto';
import { GroupDto } from '../../_models/Other/GroupDto';
import { FacultyDto } from 'src/app/_models/Other/FacultyDto';
import { StudentService } from 'src/app/_services/student.service';
import { StaticDto } from 'src/app/_models/Other/StaticDto';
import { Title } from '@angular/platform-browser';
import alertify from 'alertifyjs';
import { KeyValue } from '@angular/common';
import { inputDto } from 'src/app/_models/Other/inputDto';

@Component({
  selector: 'app-calculator',
  templateUrl: './calculator.component.html',
  styleUrls: ['./calculator.component.css']
})

export class CalculatorComponent implements OnInit {
  calculateModel = new CalculateDto(null, null, null, null, null); // ezt küldjök a számoláshoz a szervernek
  userDto = new FilterStudentDto(0, 0); // ezzel kérjük le hogy mihez férnek hozzá.
  dataSource: StudentCalculationDto[] = []; // táblázat datasource
  displayedColumns: string[] = ['index', 'neptuncode', 'stipendindex', 'groupIndex', 'amount']; // megjelenített oszlopok
  faculties: FacultyDto[] = []; // megjelenített egyetemek ahová számolhat a felhasználó ösztöndíjat
  groupModel: GroupDto = new GroupDto('0', '0'); // ezzel kapjuk meg a csoportosított hallgatókat
  studentInGroup: any; // dictionary a csoportosított hallgatókkal
  GroupAcceptedStates: KeyValue<string, boolean>[] = []; // az Elfogadot csoportok eldőntéséhez használt változó
  selectedGroup: any; // a kiválasztott csoport
  missingSemesterType: boolean = true; // letíltja a Kar mezőt
  missingInputFields: boolean = true; // letíltja a Csoportok mezőt
  saved: boolean = true; // az Elfogadot csoportok eldőntéséhez használt változó
  staticModel = new StaticDto(null, 0, 0, 0, 0, 0); // az Input mezők
  calculated: boolean = false; // figyeli hogy kiszámolta-e már az ösztöndíjat
  disableInput: boolean = false; // letíltja az input mezőket ha már kivan/volt számolva a csoport
  loading: boolean = false; // a töltő képernyőhöz szükséges
  loadingText: string = ""; // a töltő képernyőre kiiratott szöveg  
  inputModel = new inputDto(null, null, null); // az Input mezők mentéséhez szükséges
  selectedGroupKey: string;

  constructor(private calculator: CalculatorService, private jwt: jwthelper, private studentserv: StudentService, private titleService: Title) {
    this.titleService.setTitle('Számolás');
    this.userDto.roleid = this.jwt.LoggedInRoleId(); // lekéri a bejelnetkezett felhasználó id-ját
    this.userDto.uniid = this.jwt.LoggedInUniD(); // lekárdezi a bejelentkezett felhasználó egyetemi id-ját

  }

  ngOnInit() {
    this.calculator.getAllowedFaculty(this.userDto).subscribe(data => { //lekéri a karokat és meghívja a makeFacultyList eljárást
      this.makeFacultyList(data);
    });
    if (localStorage.getItem("fivemonthPrice") != null) { // megpróbálja lekérni a localStorageból a Keret 5 Hónaprát
      this.staticModel.fivemonth = JSON.parse(localStorage.getItem("fivemonthPrice"));
    }
  }
  // Elmenti a kiszámolt csoport ösztöndíját
  accept() {
    this.loadingText = "mentése";
    if (this.staticModel.fivemonth == null) { // ha nem töltötte ki a mezőket
      alertify.set('notifier', 'position', 'top-right');
      alertify.error('Tölts ki a mezőket!', 3);
    } else if (this.calculated == false) { // hamég nem számolta ki az ösztöndíjat
      alertify.set('notifier', 'position', 'top-right');
      alertify.error('Először számold ki az ösztöndíjat!', 3);
    } else {
      this.loading = !this.loading;
      this.calculator.postStudents(this.dataSource).subscribe(
        res => {
          localStorage.setItem("fivemonthPrice", JSON.stringify(this.staticModel.fivemonth)); // elmenti a Keret 5 Hónaprát
          alertify.set('notifier', 'position', 'top-right');
          alertify.success('Sikeresen elmenteted!', 3);
          //átálitja az adott csoporthoz a saved változót
          this.loading = !this.loading;
          this.GroupAcceptedStates.forEach(element => {
            if (element.key == this.selectedGroup.key) { element.value = true }
          });
          this.saved = this.selectedGroup.value;
          //console.log(this.selectedGroup);
        },
        err => {
          this.loading = !this.loading;
          alertify.set('notifier', 'position', 'top-right');
          alertify.error('Nem sikerült a mentés!', 3);
        },
      );
      this.CalculateStatistic(); // újra kiszámolja a statisztikákat
    }
  }

  // kitörli az adott csoporthoz tartozó ösztöndíjakat
  reset() {
    this.loadingText = "törlése";
    this.loading = !this.loading;
    this.GroupAcceptedStates.forEach(element => {
      if (element.key == this.selectedGroup.key) { element.value = false }
    });
    this.selectedGroup.value.forEach(element => {
      element.stipendAmmount = 0;
      element.groupIndex = 0;
    });
    this.calculator.postStudents(this.selectedGroup.value).subscribe(res => {
      alertify.set('notifier', 'position', 'top-right');
      alertify.success('Sikeresen törölted!', 3);
      this.loading = !this.loading;
      this.saved = !this.saved;
      this.dataSource = this.selectedGroup.value;
      this.CalculateStatistic();
      this.CalculateAllStudGroup(); // kiszámolja az ösztöndíjra jogosult személyek számát
    },
      err => {
        alertify.set('notifier', 'position', 'top-right');
        alertify.error('Nem sikerült törölni!', 3);
        this.loading = !this.loading;
      })

  }

  trackBy(index, item) {
    return index;
  }

  // a csoport kiválasztásánál megnézi el volt-e mentve ha nem akkor megpróbálja lekérni a localstorageból az elmentett input mezőket
  changeGroup() {
    this.selectedGroupKey = this.selectedGroup.key;
    //console.log(this.selectedGroup.key);
    //console.log(this.selectedGroup.value);
    if (this.CheckAcceptedStates(this.selectedGroup.value)) { // megnézi el volt-e mentve
      this.saved = true; // ha igen akkor letítja a saved változóval a gombokat és a mezőket
    } else {
      this.saved = false;
    }
    this.calculateModel.maxPrice = null; // lenulláza a beviteli mezőt hogy ne keveredjen össze a localstorageből lekértel
    this.calculateModel.minPrice = null; // lenulláza a beviteli mezőt hogy ne keveredjen össze a localstorageből lekértel
    this.calculateModel.minStipendIndex = null; // lenulláza a beviteli mezőt hogy ne keveredjen össze a localstorageből lekértel
    if (localStorage.getItem(this.selectedGroup.key) != null) { // megnézi a el volt-e mentve a localStorageba
      this.calculateModel = JSON.parse(localStorage.getItem(this.selectedGroup.key)); //ha igen akkor lekéri az adatokat és elmenti a calculateModelbe
    }
    this.calculateModel.students = this.selectedGroup.value; // a kiválasztott csoport-nak a halgatóit elmenti a calculateModel.students változóba 
    this.dataSource = this.calculateModel.students; // megjeleníti a táblázatban
  }

  changeSemester() {
    this.missingSemesterType = false; // a Ker mezőt engedi kiválasztani
    this.selectedGroup = null; // a Csoportok mezőt kiválasztatlanra álítja
    this.groupModel.faculty = null; // a Kar mezőt kiválasztatlanra álítja
    this.missingInputFields = true; // letíltja a csoportok mezőt
  }

  // ösztöndíjat számol és frissíti a labeleket
  calculate() {
    if (this.calculateModel.input == null || this.calculateModel.maxPrice == null || this.calculateModel.minPrice == null
      || this.calculateModel.minStipendIndex == null || this.calculateModel.students == null) { // leelenőrzi kivan-e töltve
      alertify.set('notifier', 'position', 'top-right');
      alertify.error('Töltsd ki a mezőket!', 3);
    } else {
      this.inputModel.maxPrice = this.calculateModel.maxPrice;
      this.inputModel.minPrice = this.calculateModel.minPrice;
      this.inputModel.minStipendIndex = this.calculateModel.minStipendIndex;
      localStorage.setItem(this.selectedGroup.key, JSON.stringify(this.inputModel)); // elmenti az input mezőket
      // kiszámolja az ösztöndíjat
      this.calculator.getCalculated(this.calculateModel).subscribe(data => {
        this.dataSource = data as StudentCalculationDto[];
        this.selectedGroup.value = data;
        this.calculated = true;
      });
    }
  }

  // szervertől visszakapott Karokból csinál olvasható lemeket
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
  getGrouppedStudents() { // bekéri a csoportosított halgatókat és eldönti el volt-e mentve
    this.studentserv.getStudentsInGroup(this.groupModel).subscribe(data => { // bekéri a csoportosított halgatókat
      this.studentInGroup = data; // a studentInGroup ba elmenti a csoportokat
      this.GroupAcceptedStates = [];
      for (let index = 0; index < Object.keys(this.studentInGroup).length; index++) { // eldönti el volt-e mentve már a csoport és add nekik egy saved értéket
        this.GroupAcceptedStates.push({
          key: Object.keys(this.studentInGroup)[index],
          value: this.CheckAcceptedStates(Object.values(this.studentInGroup)[index])
        });
      }
      this.missingInputFields = false; // feloldja a tiltást a Csoportok mezőröl
      //console.log("hallgatok csoportban: (studentInGroup)", this.studentInGroup);
      //console.log("elfogadott státusz: (GroupAcceptedStates)", this.GroupAcceptedStates);
      this.CalculateAllStudGroup(); // kiszámolja az ösztöndíjra jogosult személyek számát
    });
  }

  CalculateStatistic() { // összeadja az ösztöndíjakat a statisztikák megjelenítéséhez
    //console.log(this.staticModel.fivemonth)
    this.staticModel.onemonth = this.staticModel.fivemonth / 5;
    this.dataSource.forEach(element => {
      if (!(element.stipendAmmount === 0)) {
        this.staticModel.alleligble = this.staticModel.alleligble + 1;
        this.staticModel.onetransfer = this.staticModel.onetransfer + element.stipendAmmount;
      }

      this.staticModel.remnant = this.staticModel.fivemonth - (this.staticModel.onetransfer * 5);
      this.staticModel.remnantpercent = ((this.staticModel.remnant / this.staticModel.fivemonth) * 100).toPrecision(3);
    });
  }
  CalculateAllStudGroup() { // kiszámolja az ösztöndíjra jogosult személyek számát
    var num = 0;
    this.staticModel.alleligble = 0; this.staticModel.onemonth = 0; this.staticModel.onetransfer = 0; this.staticModel.remnant = 0; this.staticModel.remnantpercent = 0; // lenulláza a statisztikai mezőket
    this.staticModel.onemonth = this.staticModel.fivemonth / 5;
    if(this.studentInGroup != undefined) {
      for (let i = 0; i < Object.keys(this.studentInGroup).length; i++) { // megnézi hány elemból áll az adott kar és addig fut
        for (let y = 0; y < this.studentInGroup[Object.keys(this.studentInGroup)[i]].length; y++) { // végig megy az előzőleg kiválasztott csoport tagjain
          num ++;
          var student = this.studentInGroup[Object.keys(this.studentInGroup)[i]][y]; // elmenti a students változóba az éppen kiválasztott személyt
          if (!(student.stipendAmmount === 0)) { // leelenőrzi kaphat-e ösztöndíjat, ha igen akkor a student.stipendAmmount nem egyenlő nullával
            this.staticModel.alleligble = this.staticModel.alleligble + 1;
            this.staticModel.onetransfer = this.staticModel.onetransfer + student.stipendAmmount;
          }
        }
        this.staticModel.remnant = this.staticModel.fivemonth - (this.staticModel.onetransfer * 5);
        this.staticModel.remnantpercent = ((this.staticModel.remnant / this.staticModel.fivemonth) * 100).toPrecision(3);
      }
    }
  }
  // leelenőrzi hogy el volt-e mentve a csoport
  CheckAcceptedStates(Students: any): boolean {
    var StipSum: number = 0;
    Students.forEach(element => {
      if (element.stipendAmmount > 0) { StipSum += element.stipendAmmount }
    });
    if (StipSum > 0) { return true; } { return false; }
  }
}
