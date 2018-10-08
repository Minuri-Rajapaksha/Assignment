
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
      localStorage.setItem('role', user.profile.Role);
      localStorage.setItem('username', user.profile.name);
    });
  }

  getClientSettings(): UserManagerSettings {
    const host = window.location.origin;
    return {
      authority: 'https://authminuri.azurewebsites.net',
      client_id: 'grantTypeImplicit',
      redirect_uri: `${host}/signin-oidc`,
      // silent_redirect_uri should go to seperate html
      automaticSilentRenew: true,
      silent_redirect_uri: `${host}/signin-oidc`,
      // Check what is purpose of checkSessionInterval property
      checkSessionInterval: 1800,
      post_logout_redirect_uri: `${host}/signout-oidc`,
      response_type: 'id_token token',
      scope: 'openid profile webapi.full_access',
      filterProtocolClaims: true,
      loadUserInfo: true
    };
  }

  isLoggedIn(): boolean {
    return this.user != null && !this.user.expired;
  }

  isAdmin(): boolean {
    return this.user != null && !this.user.expired && this.user.profile.Role === '1';
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
    sessionStorage.setItem('urlBeforeRedirectToLogin', window.location.href.split('/')[3]);
    return this.manager.signinRedirect();
  }

  completeAuthentication(): Promise<void> {
    return this.manager.signinRedirectCallback().then(user => {
      this.routeService.navigateByUrl(sessionStorage.getItem('urlBeforeRedirectToLogin'));
      this.user = user;
    });
  }
}
