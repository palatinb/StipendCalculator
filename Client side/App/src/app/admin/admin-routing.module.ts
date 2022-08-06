import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminComponent } from './admin/admin.component';
import { UserComponent } from './user/user.component';
import { UniaddComponent } from './uniadd/uniadd.component';
import { DbaseComponent } from './dbase/dbase.component';
import { RoleGuard } from '../_helpers/role.guard';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    data: { title: 'Admin', exceptedRole: 1 },
    canActivate: [RoleGuard],
    children: [
      {
        path: '',
        redirectTo: '/admin/users',
        pathMatch: 'full'
      },
      {
        path: 'users',
        component: UserComponent,
        data: { title: 'Felhasználó hzzáadása' }
      },
      {
        path: 'uniadd',
        component: UniaddComponent,
        data: { title: 'Egyetem hozzáadása' }
      },
      {
        path: 'dbase',
        component: DbaseComponent,
        data: { title: 'Adatbázi hozzáadása' }
      },
      {
        path: '**',
        redirectTo: '/admin/users',
      },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
