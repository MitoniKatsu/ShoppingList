import { ListItemDto } from './list-item-dto.model';
import { ShoppingListDto } from './shopping-list-dto.model';

export class ShoppingListItemDto {
  id: string;
  shoppingListId: string;
  complete: boolean;
  listItem: ListItemDto;
}
