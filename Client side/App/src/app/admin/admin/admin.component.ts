import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../_services/user.service';
import { jwthelper } from 'src/app/_helpers/jwt.helper';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  loggedUser: string;

  constructor(private route: ActivatedRoute, private router: Router, private userService: UserService, private jwtHelp: jwthelper, private titleService: Title) {
    
  }


  ngOnInit() {
    
  }
  exit() {
    this.router.navigate([''], { relativeTo: this.route });
    this.userService.logout();
  }

}
