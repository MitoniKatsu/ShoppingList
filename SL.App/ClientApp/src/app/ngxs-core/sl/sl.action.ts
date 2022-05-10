export namespace ShoppingListActions {
  export class LoadShoppingList {
    static readonly type = '[SHOPPINGLIST] Load shopping list';
    constructor(public payload: string) {}
  }

  export class AddToShoppingList {
    static readonly type = '[SHOPPINGLIST] Add item to list';
    constructor(public payload: string) {}
  }

  export class DeleteFromShoppingList {
    static readonly type = '[SHOPPINGLIST] Delete item from list';
    constructor(public payload: string) {}
  }

  export class ToggleItemComplete {
    static readonly type = '[SHOPPINGLIST] Toggle item complete status';
    constructor(public payload: string) {}
  }

  export class LogoutShoppingList {
    static readonly type = '[SHOPPINGLIST] Logout Shopping List';
    constructor() {}
  }
}
