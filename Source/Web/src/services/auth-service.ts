
import { UserManager, UserManagerSettings, User } from 'oidc-client';
import { HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { Component, Inject } from '@angular/core';

@Inject(Router)
export class AuthService {

  private user: User = null;
  private manager = new UserManager(this.getClientSettings());

  constructor(private routeService: Router) {
    this.manager.getUser().then(user => {
      this.user = user;
    });
  }

  getClientSettings(): UserManagerSettings {
    return {
      authority: 'https://localhost:44357',
      client_id: 'grantTypeImplicit',
      redirect_uri: 'http://localhost:5896/signin-oidc',
      // silent_redirect_uri should go to seperate html
      automaticSilentRenew: true,
      silent_redirect_uri: 'http://localhost:5896/signin-silent-oidc.html',
      // Check what is purpose of checkSessionInterval property
      checkSessionInterval: 3600,
      post_logout_redirect_uri: 'http://localhost:5896/signout-oidc',
      response_type: 'id_token token',
      scope: 'openid profile webapi.full_access',
      filterProtocolClaims: true,
      loadUserInfo: true
    };
  }

  isLoggedIn(): boolean {
    return this.user != null && !this.user.expired;
  }

  getClaims(): any {
    return this.user.profile;
  }

  getAuthorizationHeaderValue(): any {
    return {
      headers: new HttpHeaders({ 'Authorization': `${this.user.token_type} ${this.user.access_token}` })
    };
  }

  startAuthentication(): Promise<void> {
    sessionStorage.setItem("urlBeforeRedirectToLogin", window.location.href.split('/')[3]);
    return this.manager.signinRedirect();
  }

  completeAuthentication(): Promise<void> {
    return this.manager.signinRedirectCallback().then(user => {
      this.routeService.navigateByUrl(sessionStorage.getItem("urlBeforeRedirectToLogin"));
      this.user = user;
    });
  }
}
