import { Selector } from '@ngxs/store';
import { UserAccountDto } from 'src/app/models/dto/user-account-dto.model';
import { ApplicationStateModel } from './application.model';
import { ApplicationState } from './application.state';

export class ApplicationStateSelector {
  @Selector([ApplicationState])
  static getLoggedInStatus(state: ApplicationStateModel) {
    return state.loggedIn;
  }

  @Selector([ApplicationState])
  static getLoadingStatus(state: ApplicationStateModel) {
    return state.loading;
  }

  @Selector([ApplicationState])
  static getUserState(state: ApplicationStateModel) {
    const user: UserAccountDto = {
      id: state.userId,
      name: state.userName,
      email: state.userEmail,
      returning: state.returning
    };

    return user;
  }
}
