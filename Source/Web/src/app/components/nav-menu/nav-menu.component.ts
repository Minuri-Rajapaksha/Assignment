import { Component } from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  role = localStorage.getItem('role');
  username = localStorage.getItem('username');

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
