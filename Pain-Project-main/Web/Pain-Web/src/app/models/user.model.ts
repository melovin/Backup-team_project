interface Log {
  loginTime : string;
  ipAddress : string;
}

export class User {
  public id : number;
  public name : string = '';
  public surname : string = '';
  public login : string = '';
  public createDate : string = '';
  public email : string = '';
  public password : string = '';
  public logs : Log[] = [];
}
