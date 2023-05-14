import {Injectable} from '@angular/core';
import {Config} from "../models/config.model";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Router} from "@angular/router";
import {SessionsService} from "./sessions.service";
import {catchError, Observable} from "rxjs";
import {environment} from "../../environments/environment";
import {clientAdd} from "../components/dialogs/add-client-dialog/add-client-dialog.component";


@Injectable({
  providedIn: 'root'
})

export class ConfigsService {
  constructor(private http: HttpClient,
              private router: Router,
              private sessions: SessionsService,) {
  }

  public get options(): { headers: HttpHeaders } {
    return {
      headers: new HttpHeaders({'Authorization': 'Bearer ' + this.sessions.token})
    };
  }

  public sendConfig(config: Config): Observable<any> {
    return this.http.post<any>(environment.api + '/adminPage/addConfig', config, this.options);
  }

  public updateConfig(config: Config): Observable<any> {
    return this.http.put<any>(environment.api + '/adminPage/updateConfig?id=' + config.id, config, this.options);
  }

  public findAllConfigs(): Observable<Config[]> {
    return this.http.get<Config[]>(environment.api + '/adminPage/allConfigs', this.options).pipe(
      catchError(err => {
        this.unauthenticated(err);
        throw new Error((err));
      })
    )
  }

  public findConfigByID(id: number): Observable<Config> {
    return this.http.get<Config>(environment.api + '/AdminPage/getConfigByID?id=' + id, this.options);
  }

  public removeConfig(idConfig: number): Observable<any> {
    return this.http.delete<any>(environment.api + '/adminPage/removeConfig?idConfig=' + idConfig, this.options);
  }

  public clientsByConfig(idConfig: number): Observable<any> {
    return this.http.get<any>(environment.api + '/adminPage/clientsByConfig?idConfig=' + idConfig, this.options);
  }


  public changeClients(idConfig: number, clients: clientAdd[]) {
    const dicClients: { [key: number]: boolean; } = {};

    clients.forEach(function (value) {
      dicClients[value.id] = value.active;
    });
    return this.http.put<any>(environment.api + '/adminPage/changeClientsOnConfig?idConfig=' + idConfig, dicClients, this.options);
  }

  public removeClientFromConfig(idConfig: number, idClient: number) {
    return this.http.delete<any>(environment.api + '/adminPage/removeClientFromConfig?idConfig=' + idConfig + '&idClient=' + idClient, this.options);
  }

  private unauthenticated(err: any): void {
    if (err.status === 401) {
      this.sessions.logout();
    }
  }
}
