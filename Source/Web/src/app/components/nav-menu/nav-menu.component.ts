import { Component } from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logOut() {
    if (window.location.hostname === 'localhost') {
      window.open(`https://localhost:44310/api/account/logout`, '_self');
    } else {
      window.open(`https://authminuri.azurewebsites.net/account/logout`, '_self');
    }
  }
}
