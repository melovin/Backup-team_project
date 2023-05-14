import {Injectable} from '@angular/core';
import {Log} from "../models/log.model";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Router} from "@angular/router";
import {SessionsService} from "./sessions.service";
import {catchError, Observable} from "rxjs";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})

export class LogsService {
  constructor(private http: HttpClient,
              private router: Router,
              private sessions: SessionsService,) {
  }

  public get options(): { headers: HttpHeaders } {
    return {
      headers: new HttpHeaders({'Authorization': 'Bearer ' + this.sessions.token})
    };
  }

  public findAllLogs(): Observable<Log[]> {
    return this.http.get<Log[]>(environment.api + '/adminPage/allLogs', this.options).pipe(
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
