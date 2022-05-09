export class ShoppingList {
  id: string;
  items: ShoppingListItem[];
}

export class ShoppingListItem {
  id: string;
  item: ListItem;
  complete: boolean;
}

export class ListItem {
  id: string;
  name: string;
}
