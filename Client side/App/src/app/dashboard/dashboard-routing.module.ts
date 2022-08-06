import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SearchComponent } from './search/search.component';
import { MonthlyGeneratorComponent } from './monthly-generator/monthly-generator.component';
import { ProfileComponent } from './profile/profile.component';
import { CalculatorComponent } from './calculator/calculator.component';
import { AuthGuard } from '../_helpers/auth.guard';
import { ElnokiHatComponent } from './monthly-generator/elnoki-hat/elnoki-hat.component';
import { TanulmanyiComponent } from './monthly-generator/tanulmanyi/tanulmanyi.component';
import { OcsiMutatoComponent } from './monthly-generator/ocsi-mutato/ocsi-mutato.component';

const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    data: { title: '' },
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        redirectTo: '/dashboard/search',
        pathMatch: 'full'
      },
      {
        path: 'search',
        component: SearchComponent,
        data: { title: '' }
      },
      {
        path: 'monthly',
        component: MonthlyGeneratorComponent,
        data: { title: '' },
        children: [
          {
            path: 'ocsi-mutato',
            component: OcsiMutatoComponent,
            data: { title: '' }
          },
          {
            path: 'elnoki',
            component: ElnokiHatComponent,
            data: { title: '' }
          },
          {
            path: 'tanulmanyi',
            component: TanulmanyiComponent,
            data: { title: '' }
          }
        ]
      },
      {
        path: 'profile',
        component: ProfileComponent,
        data: { title: '' }
      },
      {
        path: 'calculator',
        component: CalculatorComponent,
        data: { title: '' }
      },
      {
        path: '**',
        redirectTo: '/dashboard/search',
      },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
