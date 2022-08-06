import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from 'src/app/_services/user.service';
import { jwthelper } from 'src/app/_helpers/jwt.helper';

@Component({
  selector: 'app-calculator-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  isAdmin = false;
  loggedUser: string;
  databases: any;
  menu = false;

  constructor(private route: ActivatedRoute, private router: Router, private userService: UserService, private jwtHelp: jwthelper) {}

  ngOnInit() {
    if (this.jwtHelp.LoggedInRoleId() == 1) {
      this.isAdmin = true;
    } else {
      this.isAdmin = false;
    }
    this.loggedUser = this.jwtHelp.LoggedInUserName();
  }

  overmouse() {
    //console.log('asd');
    this.menu = true;
  }

  exit() {
    this.router.navigate([''], { relativeTo: this.route });
    this.userService.logout();
  }

}
