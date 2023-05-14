import {Injectable} from '@angular/core';
import {Client} from "../models/client.model";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Router} from "@angular/router";
import {SessionsService} from "./sessions.service";
import {catchError, Observable} from "rxjs";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})

export class ClientsService {

  constructor(private http: HttpClient,
              private router: Router,
              private sessions: SessionsService,) {
  }

  public get options(): { headers: HttpHeaders } {
    return {
      headers: new HttpHeaders({'Authorization': 'Bearer ' + this.sessions.token})
    };
  }

  public findAllClients(): Observable<Client[]> {
    return this.http.get<Client[]>(environment.api + '/adminPage/allClients', this.options).pipe(
      catchError(err => {
        this.unauthenticated(err);
        throw new Error((err));
      })
    )
  }

  public removeClient(idClient: number): Observable<any> {
    return this.http.delete<any>(environment.api + '/adminPage/removeClient?idClient=' + idClient, this.options).pipe(
      catchError(err => {
        this.unauthenticated(err);
        throw new Error((err));
      })
    )
  }

  public changeClients(change: any): Observable<any> {
    return this.http.put<any>(environment.api + '/AdminPage/activeChange', change, this.options).pipe(
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
