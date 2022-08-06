import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin/admin.component';
import { UniaddComponent } from './uniadd/uniadd.component';
import { DbaseComponent } from './dbase/dbase.component';
import { UserComponent } from './user/user.component';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '../material/material.module';

@NgModule({
  declarations: [
    AdminComponent,
    UniaddComponent,
    DbaseComponent,
    UserComponent,
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    FormsModule,
    MaterialModule,
  ]
})
export class AdminModule { }
