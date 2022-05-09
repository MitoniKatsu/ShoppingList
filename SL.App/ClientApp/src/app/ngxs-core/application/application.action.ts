export namespace ApplicationActions {
  export class LoginWithUserId {
    static readonly type = '[APPLICATION] Login with UserId';
    constructor(public payload: string) {}
  }

  export class LoginWithEmail {
    static readonly type = '[APPLICATION] Login with Email';
    constructor(public payload: {userName: string, userEmail: string}) {}
  }
}
