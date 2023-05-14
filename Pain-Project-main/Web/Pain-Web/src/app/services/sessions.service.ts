import {Injectable} from '@angular/core';
import {catchError, map, Observable, of, tap} from "rxjs";
import {environment} from "../../environments/environment";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {JwtHelperService} from "@auth0/angular-jwt";

@Injectable({
  providedIn: 'root'
})
export class SessionsService {

  public token: string | null = null;

  constructor(private http: HttpClient,
              private jwt: JwtHelperService,) {
    this.token = this.loadToken();
  }

  public get options(): { headers: HttpHeaders } {
    return {
      headers: new HttpHeaders({'Authorization': 'Bearer ' + this.token})
    };
  }

  public login(credentials: any): Observable<boolean> {
    return this.http.post<string>(environment.api + '/adminPage/login', credentials).pipe(
      tap(token => this.token = token),
      tap(token => this.saveToken(token)),
      map(token => true),
      catchError(() => of(false))
    )
  }

  public logout(): void {
    this.token = null;
    sessionStorage.removeItem('token');
  }

  public get authenticated(): boolean {
    return !!this.token && !this.jwt.isTokenExpired(this.token);
  }

  public reLog(id: number): Observable<any> {
    return this.http.get<string>(environment.api + '/adminPage/relog?id=' + id, this.options).pipe(
      tap(token => this.token = token),
      tap(token => this.saveToken(token))
    )
  }


  private saveToken(token: string): void {
    sessionStorage.setItem('token', token);
  }

  private loadToken(): string | null {
    return sessionStorage.getItem('token');
  }
}
