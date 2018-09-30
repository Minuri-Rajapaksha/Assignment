
import { Component, OnInit } from '@angular/core';
import { AuthService } from './../../../services/auth-service';

@Component({
  selector: 'app-signin-oidc-component',
  templateUrl: './signin-oidc.component.html'
})
export class SigninOidcComponent implements OnInit {

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.authService.completeAuthentication();
  }

}
