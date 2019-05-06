import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { 
  MatFormField, 
  MatFormFieldModule, 
  MatInputModule, 
  MatDialogModule, 
  MatCheckboxModule,
  MatCardModule,
  MatExpansionModule
} from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { JwtInterceptor } from 'src/app/interceptors/jwt.interceptor';
import { ErrorInterceptor } from 'src/app/interceptors/error.interceptor';
import { AppComponent } from './app.component';
import { HomePageComponent } from './components-page/home-page/home-page.component';
import { ApplicationPageComponent } from './components-page/application-page/application-page.component';
import { HeaderComponent } from './components-header/header/header.component';
import { NavComponent } from './components-header/nav/nav.component';
import { FooterComponent } from './components-footer/footer/footer.component';
import { LoginPageComponent } from './components-page/login-page/login-page.component';

@NgModule({
  declarations: [
    AppComponent,
    HomePageComponent,
    ApplicationPageComponent,
    HeaderComponent,
    NavComponent,
    FooterComponent,
    LoginPageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MatFormFieldModule,
    MatInputModule,
    MatDialogModule,
    MatCheckboxModule,
    MatCardModule,
    MatExpansionModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule
  ],
  entryComponents: [
    //DialogOverviewExampleDialog
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
