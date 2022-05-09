import { ListItem } from 'src/app/models/shopping-list.model';

export class ApplicationStateModel {
  loggedIn: boolean;
  userId: string;
  userName: string;
  userEmail: string;
  returning: boolean;
  loading: boolean;
  listItems: ListItem[];
}
