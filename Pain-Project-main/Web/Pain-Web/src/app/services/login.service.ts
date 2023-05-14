import {Injectable} from '@angular/core';
import {SessionsService} from "./sessions.service";
import {JwtHelperService} from "@auth0/angular-jwt";

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private session: session;
  private user: loginUser;

  constructor(private sessionService: SessionsService,
              private jwt: JwtHelperService,
  ) {
  }

  GetLogin(): loginUser {
    return this.user;
  }

  SetLogin(): void {
    this.session = this.jwt.decodeToken(this.sessionService.token || '');
    this.user = this.session.user;
  }

}

export interface loginUser {
  Id: number;
  Name: string;
  Surname: string;
  Email: string;
  Darkmode: boolean;
  Create: string;
}

export interface session {
  exp: number;
  user: loginUser;
}
