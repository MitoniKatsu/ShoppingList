export namespace ShoppingListActions {
  export class LoadShoppingList {
    static readonly type = '[SHOPPINGLIST] Load shopping list';
    constructor(public payload: string) {}
  }
}
