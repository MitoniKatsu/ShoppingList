export namespace ShoppingListActions {
  export class LoadShoppingList {
    static readonly type = '[SHOPPINGLIST] Load shopping list';
    constructor(public payload: string) {}
  }

  export class AddToShoppingList {
    static readonly type = '[SHOPPINGLIST] Add item to list';
    constructor(public payload: string) {}
  }
}
