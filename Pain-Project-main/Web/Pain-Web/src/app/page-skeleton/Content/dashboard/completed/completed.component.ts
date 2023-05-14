import {Component, OnInit} from '@angular/core';
import * as moment from "moment/moment";
import {DashboardService, dashboardTask} from "../../../../services/dasboard.service";

@Component({
  selector: 'app-completed',
  templateUrl: './completed.component.html',
  styleUrls: ['./../dashboard.component.scss']
})
export class CompletedComponent implements OnInit {
  completed: dashboardTask[] = [];

  constructor(public service: DashboardService) {
    this.service.TodayTasks().subscribe(x => this.completed = x.filter(x => moment(x.date) <= moment(Date.now())))
  }


  format_time(s: string) {
    let now = moment(s);
    return (now.format("HH:mm"));
  }

  ngOnInit(): void {
  }

}
