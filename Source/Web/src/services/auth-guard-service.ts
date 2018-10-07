
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot } from '@angular/router';

import { AuthService } from './auth-service';

@Injectable()
export class AuthGuardService implements CanActivate {

  constructor(private authService: AuthService) { }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    if (route.url.length > 0 && (route.url[0].path === 'report-view' ||
      route.url[0].path === 'upload-balance') && !this.authService.isAdmin()) {
      return false;
    }

    if (this.authService.isLoggedIn()) {
      return true;
    }

    this.authService.startAuthentication();
    return false;
  }
}
