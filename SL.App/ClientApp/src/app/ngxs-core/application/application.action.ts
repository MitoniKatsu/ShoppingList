export namespace ApplicationActions {
  export class LoginWithUserId {
    static readonly type = '[APPLICATION] Login with UserId';
    constructor(public payload: string) {}
  }

  export class LoginWithEmail {
    static readonly type = '[APPLICATION] Login with Email';
    constructor(public payload: {userName: string, userEmail: string}) {}
  }

  export class LoadListItems {
    static readonly type = '[APPLICATION] Load ListItems';
    constructor() {}
  }

  export class UpdateLoading {
    static readonly type = '[APPLICATION] Update Loading flag';
    constructor(public payload: boolean) {}
  }

  export class LogoutApplication {
    static readonly type = '[APPLICATION] Logout Application';
    constructor() {}
  }
}
