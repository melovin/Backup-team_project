import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Router} from "@angular/router";
import {SessionsService} from "./sessions.service";
import {catchError, Observable} from "rxjs";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  constructor(private http: HttpClient,
              private router: Router,
              private sessions: SessionsService,) {
  }

  public get options(): { headers: HttpHeaders } {
    return {
      headers: new HttpHeaders({'Authorization': 'Bearer ' + this.sessions.token})
    };
  }

  public GetSize(): Observable<number> {
    return this.http.get<number>(environment.api + '/adminPage/getSize', this.options).pipe(
      catchError(err => {
        this.unauthenticated(err);
        throw new Error((err));
      })
    )
  }

  public TodayTasks(): Observable<dashboardTask[]> {
    return this.http.get<dashboardTask[]>(environment.api + '/adminPage/todayTasks', this.options);
  }

  public SevenDays(): Observable<dashboardTask[]> {
    return this.http.get<dashboardTask[]>(environment.api + '/adminPage/sevenDays', this.options);
  }

  public GetPercent(): Observable<number> {
    return this.http.get<number>(environment.api + '/adminPage/getPercent', this.options);
  }


  private unauthenticated(err: any): void {
    if (err.status === 401) {
      this.sessions.logout();
    }
  }
}

export interface dashboardTask {
  taskId: number;
  configName: string;
  clientName: string;
  state: string;
  date: string;
}
