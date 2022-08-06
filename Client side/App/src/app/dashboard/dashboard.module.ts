import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { ProfileComponent } from './profile/profile.component';
import { SearchComponent } from './search/search.component';
import { MonthlyGeneratorComponent } from './monthly-generator/monthly-generator.component';
import { CalculatorComponent } from './calculator/calculator.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '../material/material.module';
import { TanulmanyiComponent } from './monthly-generator/tanulmanyi/tanulmanyi.component';
import { ElnokiHatComponent } from './monthly-generator/elnoki-hat/elnoki-hat.component';
import { OcsiMutatoComponent } from './monthly-generator/ocsi-mutato/ocsi-mutato.component';


@NgModule({
  declarations: [
    SearchComponent,
    MonthlyGeneratorComponent,
    ProfileComponent,
    CalculatorComponent,
    DashboardComponent,
    ElnokiHatComponent,
    TanulmanyiComponent,
    OcsiMutatoComponent,
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    FormsModule,
    MaterialModule,
  ]
})
export class DashboardModule { }
