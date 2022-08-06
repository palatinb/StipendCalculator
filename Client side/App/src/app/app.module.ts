import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { JwtInterceptor } from './_helpers/jwt.interceptor';
import { jwthelper } from './_helpers/jwt.helper';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material/material.module';
import { MatPaginatorModule } from '@angular/material';
import { LZStringModule, LZStringService } from 'ng-lz-string';



@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [  
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MaterialModule,
    MatPaginatorModule,
    LZStringModule,
  ],
  providers: [
    LZStringService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    },
    {
      provide: jwthelper,
      useClass: jwthelper
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
