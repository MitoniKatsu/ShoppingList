import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { Select, Store } from '@ngxs/store';
import { CookieService } from 'ngx-cookie-service';
import { BehaviorSubject, Observable } from 'rxjs';
import { finalize, take, tap } from 'rxjs/operators';
import { LoginDialogComponent } from '../login-dialog/login-dialog.component';
import { UserAccountDto } from '../models/dto/user-account-dto.model';
import { ApplicationActions } from '../ngxs-core/application/application.action';
import { ApplicationStateSelector } from '../ngxs-core/application/application.selector';
import { ShoppingListActions } from '../ngxs-core/sl/sl.action';

export const COOKIE_KEY = 'SL-User';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  userName = '';
  returning = false;

  @Select(ApplicationStateSelector.getUserState) $userAccount: Observable<UserAccountDto>;
  @Select(ApplicationStateSelector.getLoggedInStatus) $loggedIn: Observable<boolean>;

  isExpanded = false;

  constructor(
    private store: Store,
    private cookieService: CookieService,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.$userAccount.pipe(tap(val => {
      if (val && val !== null) {
        this.userName = val.name == null ? '' : val.name;
        this.returning = val.returning;
        if (val.id !== null) {
          this.cookieService.set(COOKIE_KEY, val.id, 30, null, null, true, 'None');
        }
      } else {
        this.userName = '';
        this.returning = false;
      }
    })).subscribe();
    this.checkCookie();
  }

  checkCookie() {
    if (this.cookieService.check(COOKIE_KEY)) {
      const cookieVal = this.cookieService.get(COOKIE_KEY);
      if (cookieVal !== 'null') {
        this.store.dispatch(new ApplicationActions.LoginWithUserId(cookieVal))
          .subscribe();
        return;
      }
    }

    LoginDialogComponent.openLoginDialog(this.dialog).subscribe(s => {
      if (s && s !== null) {
        this.store.dispatch(new ApplicationActions.LoginWithEmail(s))
        .subscribe();
      }
    });
  }

  clearCookie() {
    this.cookieService.delete(COOKIE_KEY);
  }

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

  logout() {
    this.store.dispatch([
      new ShoppingListActions.LogoutShoppingList(),
      new ApplicationActions.LogoutApplication()
    ]).subscribe(() => this.clearCookie());
  }
}
