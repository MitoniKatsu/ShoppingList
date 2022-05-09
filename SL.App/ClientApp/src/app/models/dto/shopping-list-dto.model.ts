import { ShoppingListItemDto } from './shopping-list-item-dto.model';
import { UserAccountDto } from './user-account-dto.model';

export class ShoppingListDto {
  id: string;
  userId: string;
  items: ShoppingListItemDto[];
}
