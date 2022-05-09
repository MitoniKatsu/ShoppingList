import { Selector } from '@ngxs/store';
import { ShoppingListStateModel } from './sl.model';
import { ShoppingListState } from './sl.state';

export class ShoppingListStateSelector {
  @Selector([ShoppingListState])
  static getShoppingListItems(state: ShoppingListStateModel) {
    return state.shoppingList.items;
  }
}
