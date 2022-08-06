import { Component, OnInit } from '@angular/core';
import { UserNotAdminDto } from 'src/app/_models/User/UserNotAdminDto';
import { jwthelper } from 'src/app/_helpers/jwt.helper';
import { UserService } from 'src/app/_services/user.service';
import { CheckOldPwDto } from 'src/app/_models/Other/CheckOldPwDto';
import alertify from 'alertifyjs';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  constructor(private jwt: jwthelper, private userServices: UserService, private titleService: Title) {
    this.titleService.setTitle('Profil');
  }

  edit = false;
  showpassOld = false;
  showpassNew = false;
  showpassNew2 = false;
  showModel = new UserNotAdminDto(0, '', '', '', '', '');
  editModel = new UserNotAdminDto(0, '', '', '', '', '');
  oldPass: any;

  ngOnInit() {
    this.showModel = this.jwt.LoggedInUser() as UserNotAdminDto;
    this.editModel.name = this.showModel.name;
    this.editModel.username = this.showModel.username;
    this.editModel.id = this.showModel.id;
  }
  editUser() {
    this.edit = true;
    this.editModel.name = this.showModel.name;
    this.editModel.username = this.showModel.username;
    this.editModel.id = this.showModel.id;
  }

  checkOldPassword() {
    this.userServices.checkOldPw(new CheckOldPwDto(this.editModel.id, this.editModel.password)).subscribe(data => {
      this.oldPass = data;
    });
  }

  onSubmit() {
    if (this.editModel.password === '' || this.editModel.passwordHash2 === '' || this.editModel.passwordHash === '') {
      alertify.set('notifier', 'position', 'top-right');
      alertify.error('Töltsd ki a mezőket!', 3);
    } else if (this.editModel.passwordHash !== this.editModel.passwordHash2) {
      alertify.set('notifier', 'position', 'top-right');
      alertify.error('Nem egyezeik a kettő jelszó!', 3);
    } else if (this.editModel.password === this.editModel.passwordHash) {
      alertify.set('notifier', 'position', 'top-right');
      alertify.error('Nem lehet a régi jelszavad!', 3);
    } else if (this.oldPass == false) {
      alertify.set('notifier', 'position', 'top-right');
      alertify.error('Helytelen régi jelszó!', 3);
    } else {
      this.userServices.Modify(this.editModel).subscribe(data => {
        alertify.set('notifier', 'position', 'top-right');
        alertify.success('Sikeresen módosítotad a jelszavadat!', 3);
        this.editModel.password = '';
        this.editModel.passwordHash = '';
        this.editModel.passwordHash2 = '';
      });
    }
  }

}
