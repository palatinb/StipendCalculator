import { NgModule } from '@angular/core';
import { MatTableModule } from '@angular/material';
import {MatDialogModule} from '@angular/material/dialog';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatPaginatorModule} from '@angular/material/paginator';
import {MatSortModule} from '@angular/material/sort';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatSelectModule} from '@angular/material/select';
import {MatIconModule} from '@angular/material/icon';
import {MatTooltipModule} from '@angular/material/tooltip'; 
import {MatStepperModule} from '@angular/material/stepper'; 
import {MatMenuModule} from '@angular/material/menu'; 
import {MatCheckboxModule} from '@angular/material/checkbox'; 

const MaterialComponents = [
  MatTableModule,
  MatSelectModule,
  MatDialogModule,
  MatProgressSpinnerModule,
  MatPaginatorModule,
  MatSortModule,
  MatFormFieldModule,
  MatInputModule,
  MatIconModule,
  MatTooltipModule,
  MatStepperModule,
  MatMenuModule,
  MatCheckboxModule,
];

@NgModule({
  imports: [MaterialComponents],
  exports: [MaterialComponents]
})
export class MaterialModule { }

