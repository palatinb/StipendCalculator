import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/_services/user.service';
import { RoleService } from 'src/app/_services/role.service';
import { UniService } from 'src/app/_services/uni.service';
import { UserRegisterDto } from 'src/app/_models/User/UserRegisterDto';
import { UniDto } from 'src/app/_models/UniDto';
import { RoleDto } from 'src/app/_models/RoleDto';
import { UserShowDto } from 'src/app/_models/User/UserShowDto';
import { UserModifiedDto } from 'src/app/_models/User/UserModifiedDto';
import alertify from 'alertifyjs';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {

  constructor(private userService: UserService,
    private roleSerivce: RoleService,
    private uniService: UniService,
    private titleService: Title) {
      this.titleService.setTitle('Felhasználók');
  }

  Model = new UserRegisterDto('', '', '', 0, 0);
  ModifiedModel = new UserModifiedDto(0, '', '', '', 0, 0);
  selectedUser: string;
  universities: UniDto[];
  permissions: RoleDto[];
  permHasError = true;
  uniHasError = true;
  selected = true;
  popup = false;
  noData = false;
  members: UserShowDto[];
  displayedColumns: string[] = ['name', 'username', 'roleName', 'lastLogin', 'Edit', 'Delete'];
  noNetwork = true;

  EditMember(member) {
    //console.log(member);
    this.selected = false;
    this.userService.getUser(member.id).subscribe(dat => {
      this.ModifiedModel = dat as UserModifiedDto;

    });

  }

  DeleteMember(member) {
    this.userService.deleteUser(member.id).subscribe((result) => {
      alertify.set('notifier', 'position', 'top-right');
      alertify.success('Sikeresen törölted a ' + member.name + ' nevü felhasználót!', 3);
      this.userService.getMembers().subscribe(data => this.members = data as UserShowDto[]);
    });
  }

  showAdd() {
    this.selected = true;
    this.Model = new UserRegisterDto('', '', '', 0, 0);
  }

  validateUsername(username) {
    if (this.userService.checkUsername(username)) {
      // ha fogalalt a username
      return false;
    }
    // ha szabad a username
    return true;
  }

  validatePerm(value) {
    if (value === 0) {
      this.permHasError = true;
    } else {
      this.permHasError = false;
    }
  }
  validateUni(value) {
    if (value === 0) {
      this.uniHasError = true;
    } else {
      this.uniHasError = false;
    }
  }


  trackBy(index, item) {
    return index;
  }

  ngOnInit() {
    this.uniService.GetAllUni().subscribe(data => this.universities = data as UniDto[]);
    this.roleSerivce.GetAllRole().subscribe(data => this.permissions = data as RoleDto[]);
    if(!navigator.onLine) {
      this.noNetwork = false;
      this.noData = true;
    }else if(navigator.onLine) {
      this.noData = false;
      this.userService.getMembers().subscribe(data => {
        this.members = data as UserShowDto[]
        this.noData = true;   
      });
    }
  }
  onSubmitAdd() {
    if (this.Model.Name === '' || this.Model.PasswordHash === '' || this.Model.Username === '' ||
      this.Model.RoleId === 0 || this.Model.UiD === 0) {
      //console.log(this.Model);
      alertify.set('notifier', 'position', 'top-right');
      alertify.error('Töltsd ki a mezőket!', 'custom', 3);
    } else {
      this.userService.checkUsername(this.Model)
        .subscribe(data => {
          //console.log(data);
          if (data === true) {
            alertify.set('notifier', 'position', 'top-right');
            alertify.error('A felhasználónév foglalt.');
          } else {
            this.userService.Register(this.Model)
              .subscribe((result) => {
                //console.log('Add success');
                this.userService.getMembers().subscribe(data => this.members = data as UserShowDto[]);
                alertify.set('notifier', 'position', 'top-right');
                alertify.success('Sikeresen hozzáadtad a felhasználót!', 3);
              },
                error => {
                  alertify.set('notifier', 'position', 'top-right');
                  alertify.error('A hozzáadás nem sikerült!', 3);
                });
          }
        },
        error => {
          //console.log(error);
        })
    }
  }

  onSubmitEdit() {
    this.userService.ModifybyA(this.ModifiedModel)
      .subscribe(
        (result) => {
          alertify.set('notifier', 'position', 'top-right');
          alertify.success('Sikeresen módosítottad a ' + this.ModifiedModel.name + ' nevü felhasználót!', 3);
          this.userService.getMembers().subscribe(data => this.members = data as UserShowDto[]);
        },
        error => {
          alertify.set('notifier', 'position', 'top-right');
          alertify.error('A módosítás nem sikerült', 3);
        });

  }


  // ebben lehet módosítani a felasználónevet csak a serveren kell megírni hozzá a checkusert
  /* onSubmitEdit() {
    if (this.ModifiedModel.name === '' || this.ModifiedModel.username === '' || this.ModifiedModel.roleId === 0 || this.ModifiedModel.uiD === 0) {
      alertify.set('notifier', 'position', 'top-right');
      alertify.error('Töltsd ki a mezőket!', 'custom', 3);
    } else {
      this.userService.checkUsername2(this.ModifiedModel)
        .subscribe(data => {
          //console.log(data);
          if (data === true) {
            alertify.set('notifier', 'position', 'top-right');
            alertify.error('A felhasználónév foglalt.');
          } else {
            this.userService.ModifybyA(this.ModifiedModel)
              .subscribe(
                (result) => {
                  alertify.set('notifier', 'position', 'top-right');
                  alertify.success('Sikeresen módosítottad a ' + this.ModifiedModel.name + ' nevü felhasználót!', 3);
                  this.userService.getMembers().subscribe(data => this.members = data as UserShowDto[]);
                },
                error => {
                  alertify.set('notifier', 'position', 'top-right');
                  alertify.error('A módosítás nem sikerült', 3);
                });
          };
        },
          error => {
            //console.log(error);
          })
    }

  } */
}
