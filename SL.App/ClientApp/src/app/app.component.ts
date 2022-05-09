import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { Select, Store } from '@ngxs/store';
import { CookieService } from 'ngx-cookie-service';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { LoginDialogComponent } from './login-dialog/login-dialog.component';
import { UserAccountDto } from './models/dto/user-account-dto.model';
import { ApplicationActions } from './ngxs-core/application/application.action';
import { ApplicationStateSelector } from './ngxs-core/application/application.selector';

export const COOKIE_KEY = 'SL-User';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

  @Select(ApplicationStateSelector.getLoggedInStatus) $loggedIn: Observable<boolean>;
  @Select(ApplicationStateSelector.getUserState) $userAccount: Observable<UserAccountDto>;

  userName = new BehaviorSubject<string>('');
  returning = new BehaviorSubject<boolean>(false);

  constructor(
    private store: Store,
    private cookieService: CookieService,
    private dialog: MatDialog
    ) {}

  ngOnInit() {
    this.$userAccount.pipe(tap(val => {
      if (val && val !== null) {
        this.userName.next(val.name == null ? '' : val.name);
        this.returning.next(val.returning);
        if (val.id !== null) {
          this.cookieService.set(COOKIE_KEY, val.id, 30, null, null, true, 'None');
        }
      } else {
        this.userName.next('');
        this.returning.next(false);
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
}
