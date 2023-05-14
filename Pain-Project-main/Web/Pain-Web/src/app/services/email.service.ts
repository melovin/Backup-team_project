import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Router} from "@angular/router";
import {SessionsService} from "./sessions.service";
import {catchError, Observable} from "rxjs";
import {environment} from "../../environments/environment";
import {EmailSettingsModel} from '../models/emailSettings.model';

@Injectable({
  providedIn: 'root'
})

export class EmailService {

  constructor(private http: HttpClient,
              private router: Router,
              private sessions: SessionsService,) {
  }

  public get options(): { headers: HttpHeaders } {
    return {
      headers: new HttpHeaders({'Authorization': 'Bearer ' + this.sessions.token})
    };
  }

  public GetEmailSettings(): Observable<EmailSettingsModel> {
    return this.http.get<EmailSettingsModel>(environment.api + '/adminPage/getEmailSettings', this.options).pipe(
      catchError(err => {
        this.unauthenticated(err);
        throw new Error((err));
      })
    )
  }

  public changeEmailSettings(change: any): Observable<any> {
    return this.http.put<any>(environment.api + '/AdminPage/changeEmailSettings', change, this.options).pipe(
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
