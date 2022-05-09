import { Action, NgxsOnInit, State, StateContext, Store } from '@ngxs/store';
import { throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { LoginService } from 'src/app/services/login.service';
import { ShoppingListService } from 'src/app/services/shopping-list.service';
import { ShoppingListActions } from '../sl/sl.action';
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

export class ApplicationState implements NgxsOnInit {
  constructor(
    private store: Store,
    private userSvc: LoginService,
    private shoppingSvc: ShoppingListService
  ) {}
  ngxsOnInit(ctx?: StateContext<any>) {
    this.store.dispatch(new ApplicationActions.LoadListItems())
    .subscribe();
  }

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
            loggedIn: true
          });
          this.store.dispatch(new ShoppingListActions.LoadShoppingList(response.body.id))
          .subscribe();
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
            loggedIn: true
          });
          this.store.dispatch(new ShoppingListActions.LoadShoppingList(response.body.id))
          .subscribe();
        }
    }));
  }

  @Action(ApplicationActions.LoadListItems)
  loadListItems({patchState}: StateContext<ApplicationStateModel>) {
    return this.shoppingSvc.getListItems()
    .pipe(
      catchError(err => {
        return throwError(err);
      }),
      tap(response => {
        if (response !== null && response.status === 200) {
          patchState({
            listItems: response.body
          });
        }
    }));
  }

  @Action(ApplicationActions.UpdateLoading)
  updateLoadingFlag({patchState}: StateContext<ApplicationStateModel>, {payload}) {
    return patchState({
      loading: payload
    });
  }
}
