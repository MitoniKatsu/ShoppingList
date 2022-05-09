import { State, Store } from '@ngxs/store';
import { ShoppingListStateModel } from './sl.model';

@State<ShoppingListStateModel>({
  name: 'sl',
  defaults: {
    shoppingList: null
  }
})

export class ShoppingListState {
  constructor(
    private store: Store
  ) {}
}
