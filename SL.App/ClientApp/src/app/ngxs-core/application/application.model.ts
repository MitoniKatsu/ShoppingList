import { ListItemDto } from "src/app/models/dto/list-item-dto.model";

export class ApplicationStateModel {
  loggedIn: boolean;
  userId: string;
  userName: string;
  userEmail: string;
  returning: boolean;
  loading: boolean;
  listItems: ListItemDto[];
}
