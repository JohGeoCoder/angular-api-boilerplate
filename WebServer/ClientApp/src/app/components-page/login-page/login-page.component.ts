import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';

import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { Login } from 'src/app/models/login';
import { Registration } from 'src/app/models/registration';


@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent implements OnInit {
  
  private error: string = '';
  private step: number = 0;

  private loginModel: Login = new Login();
  private registrationModel: Registration = new Registration();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService
  ) { }

  ngOnInit() {
  }

  private onLogin = () => {
    this.authenticationService.login(this.loginModel)
      .pipe(first())
      .subscribe(
        data => {
          this.router.navigate(['/']);
        }, 
        error => {
          this.error = error;
        }
      );
  }

  private onRegister = () => {
    this.authenticationService.register(this.registrationModel)
      .pipe(first())
      .subscribe(
        data => {
          this.router.navigate(['/']);
        }, 
        error => {
          this.error = error;
        }
      );
  }

}
