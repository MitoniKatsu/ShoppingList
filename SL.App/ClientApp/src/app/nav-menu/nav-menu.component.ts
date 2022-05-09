import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  @Input() userName = '';
  @Input() returning = false;

  isExpanded = false;

  getHeader(): string {
    if (this.userName.length > 0) {
      return this.returning ?
      `Welcome back to your shopping list, ${this.userName}` :
      `Welcome ${this.userName}, to your new shopping list`;
    } else {
      return '';
    }
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
