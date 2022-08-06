export class StudentEditDto {
  constructor(
      public Id: number,
      public modulcode: string,
      public neptuncode: string,
      public modulname: string,
      public telephely: string,
      //évfolyam
      public year: number,
      //tankör
      public studentgroup: string,
      public activesemester: number,
      public passivesemester: number,
      //ösztöndíj index
      public creditindex: number,
      //megszerzett kreditek
      public earnedcredit: number,
      public financialstate: string,
      //elismert kreditek
      public acceptedcredit:number,
      public allsemester: number,
      //ösztöndíjmutató
      public stipendindex: number,
      //öcsi
      public groupindex: number,
      //elvárt kredit
      public exceptedcredit: number,
      //beiratkozás
      public yearofenroll: string,
      public finishedsemester: number,
      //ösztöndíjösszeg
      public stipendamount: string,
  )
  {}
}
