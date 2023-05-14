import {Injectable} from '@angular/core';
import {User} from "../models/user.model";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Router} from "@angular/router";
import {SessionsService} from "./sessions.service";
import {catchError, Observable} from "rxjs";
import {environment} from "../../environments/environment";
import {LoginService} from "./login.service";


@Injectable({
  providedIn: 'root'
})

export class UsersService {
  constructor(private http: HttpClient,
              private router: Router,
              private sessions: SessionsService,
              private logins: LoginService) {
  }

  public get options(): { headers: HttpHeaders } {
    return {
      headers: new HttpHeaders({'Authorization': 'Bearer ' + this.sessions.token})
    };
  }

  public findAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(environment.api + '/adminPage/allUsers', this.options).pipe(
      catchError(err => {
        this.unauthenticated(err);
        throw new Error((err));
      })
    )
  }

  public removeUser(idUser: number): Observable<any> {
    return this.http.delete<any>(environment.api + '/adminPage/removeUser?idUser=' + idUser, this.options).pipe(
      catchError(err => {
        this.unauthenticated(err);
        throw new Error((err));
      })
    )
  }

  public addUser(user: User): Observable<any> {
    return this.http.post<any>(environment.api + '/adminPage/addUser', user, this.options).pipe(
      catchError(err => {
        this.unauthenticated(err);
        throw new Error((err));
      })
    )
  }

  public darkmodeChange(change: boolean): Observable<any> {
    let idUser = this.logins.GetLogin().Id;
    return this.http.put<any>(environment.api + '/adminPage/darkmodeChange?idUser=' + idUser + '&change=' + change, null, this.options).pipe(
      catchError(err => {
        this.unauthenticated(err);
        throw new Error((err));
      })
    )
  }

  private unauthenticated(err: any): void {
    if (err.status === 401) {
      this.sessions.logout();
    }
  }

}
