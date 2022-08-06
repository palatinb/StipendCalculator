import { Component, OnInit } from '@angular/core';
import { UserLoginDto } from '../../_models/User/UserLoginDto';
import { UserService } from '../../_services/user.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthGuard } from '../../_helpers/auth.guard';
import { jwthelper } from '../../_helpers/jwt.helper';
import alertify from 'alertifyjs';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css']
})
export class SigninComponent implements OnInit {

  userModel = new UserLoginDto('', '');
  errorMsg = '';
  onSubmitted = false;
  showpass:any;

  constructor(private userService: UserService,
              private router: Router,
              private route: ActivatedRoute,
              private authGuard: AuthGuard,
              private jwtHelp: jwthelper,
              private titleService: Title) {
    this.titleService.setTitle('Bejelentkezés');
    var userOBject = localStorage.getItem("currentUser");
    var user = JSON.parse(userOBject);
    if (user != null) {
      if (this.userService.isExpired()) {
        this.router.navigate(['/login']);
      } else {
        const roleid = this.jwtHelp.LoggedInRoleId();
        if (roleid === '1') { this.router.navigate(['/admin']); } else { this.router.navigate(['/dashboard']); }
      }
    } else {
    }
  }

  onSubmit() {
    this.onSubmitted = true;
    this.userService.login(this.userModel)
      .subscribe(
        data => {
          const roleid = this.jwtHelp.LoggedInRoleId();
          if (roleid === '1') { this.router.navigate(['/admin']); } else { this.router.navigate(['/dashboard']); }
          /* this.router.navigate(['/admin'], { relativeTo: this.route } )*/
          alertify.set('notifier', 'position', 'top-right');
          alertify.success('Sikeresen bejelentkeztél!', 3);
          this.onSubmitted = false;
        },
        error => {
          this.onSubmitted = false;
          
            alertify.set('notifier', 'position', 'top-right');
            alertify.error('Sikertelen bejelentkezés!');
          
        });
  }

  ngOnInit() {

  }

}
