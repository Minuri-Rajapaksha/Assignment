import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth-service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
  providers: [AuthService]
})
export class NavMenuComponent {
  isExpanded = false;
  isAdmin = false;
  username = '';

  constructor(private authService: AuthService) {
    authService.getUser().subscribe(user => {
      this.isAdmin = this.authService.isAdmin();
      this.username = user.profile.name;
    });
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logOut() {
    sessionStorage.clear();
    window.open(`https://authminuri.azurewebsites.net/account/logout`, '_self');
  }
}
