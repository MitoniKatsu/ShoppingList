import { Action, State, StateContext, Store } from '@ngxs/store';
import { throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { LoginService } from 'src/app/services/login.service';
import { ApplicationActions } from './application.action';
import { ApplicationStateModel } from './application.model';

@State<ApplicationStateModel>({
  name: 'app',
  defaults: {
    loggedIn: false,
    userId: null,
    userName: null,
    userEmail: null,
    returning: false,
    loading: true,
    listItems: []
  }
})

export class ApplicationState {
  constructor(
    private store: Store,
    private userSvc: LoginService
  ) {}

  @Action(ApplicationActions.LoginWithUserId)
  loginWithUserId({patchState}: StateContext<ApplicationStateModel>, {payload}) {
    patchState({
      loading: true
    });
    const userId = payload;
    return this.userSvc.getUserAccountByCookie(userId)
    .pipe(
      catchError(err => {
        patchState({
          loading: false
        });
        return throwError(err);
      }),
      tap(response => {
        if (response !== null && response.status === 200) {
          patchState({
            userId: response.body.id,
            userName: response.body.name,
            userEmail: response.body.email,
            returning: response.body.returning,
            loading: false
          });
        }
    }));
  }

  @Action(ApplicationActions.LoginWithEmail)
  loginWithEmail({patchState}: StateContext<ApplicationStateModel>, {payload}) {
    patchState({
      loading: true
    });
    const user: {userName: string, userEmail: string} = payload;
    return this.userSvc.getUserAccountByLogin(user.userName, user.userEmail)
    .pipe(
      catchError(err => {
        patchState({
          loading: false
        });
        return throwError(err);
      }),
      tap(response => {
        if (response !== null && response.status === 200) {
          patchState({
            userId: response.body.id,
            userName: response.body.name,
            userEmail: response.body.email,
            returning: response.body.returning,
            loading: false,
            loggedIn: true
          });
        }
    }));
  }

}
