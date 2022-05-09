import { Action, State, StateContext, Store } from '@ngxs/store';
import { throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { ShoppingListService } from 'src/app/services/shopping-list.service';
import { ApplicationActions } from '../application/application.action';
import { ShoppingListActions } from './sl.action';
import { ShoppingListStateModel } from './sl.model';

@State<ShoppingListStateModel>({
  name: 'sl',
  defaults: {
    shoppingList: null
  }
})

export class ShoppingListState {
  constructor(
    private store: Store,
    private shoppingSvc: ShoppingListService
  ) {}

  @Action(ShoppingListActions.LoadShoppingList)
  loadShoppingList({patchState}: StateContext<ShoppingListStateModel>, {payload}) {
    const userId: string = payload;
    return this.shoppingSvc.getShoppingList(userId).pipe(
      catchError(err => {
        this.store.dispatch(new ApplicationActions.UpdateLoading(false))
        .subscribe();
        return throwError(err);
      }),
      tap(response => {
        if (response !== null && response.status === 200) {
          patchState({
            shoppingList: response.body
          });
          this.store.dispatch(new ApplicationActions.UpdateLoading(false))
          .subscribe();
        }
      })
    )
  }
}

