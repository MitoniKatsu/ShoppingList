import { Action, State, StateContext, Store } from '@ngxs/store';
import { throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { ListItemDto } from 'src/app/models/dto/list-item-dto.model';
import { ShoppingListService } from 'src/app/services/shopping-list.service';
import { ApplicationActions } from '../application/application.action';
import { ApplicationStateSelector } from '../application/application.selector';
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
    );
  }

  @Action(ShoppingListActions.AddToShoppingList)
  addToShoppingList({getState, patchState}: StateContext<ShoppingListStateModel>, {payload}) {
    this.store.dispatch(new ApplicationActions.UpdateLoading(true))
    .subscribe();
    const itemList = this.store.selectSnapshot(ApplicationStateSelector.getListItems);
    const shoppingListId = getState().shoppingList.id;
    const itemName: string = payload;
    // initialize with name only
    let itemExists = false;
    const itemToAdd: ListItemDto = {
      id: null,
      name: itemName
    };
    // check for matching item in itemList
    const index = itemList.findIndex(f => f.name.toLowerCase() === itemName.toLowerCase());
    if (index !== -1) {
      itemToAdd.id = itemList[index].id;
      itemExists = true;
    }
    // add item to shoppingList
    return this.shoppingSvc.addItemToShoppingList(shoppingListId, itemToAdd).pipe(
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
          if (!itemExists) {
            // reload itemList to add newly-added item to autocomplete
            this.store.dispatch([
              new ApplicationActions.LoadListItems(),
              new ApplicationActions.UpdateLoading(false)
            ])
            .subscribe();
          } else {
            this.store.dispatch(new ApplicationActions.UpdateLoading(false))
            .subscribe();
          }
        }
      })
    );
  }
}

