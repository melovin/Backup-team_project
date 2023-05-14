import {Component, OnInit} from '@angular/core';
import {DashboardService, dashboardTask} from "../../../../services/dasboard.service";
import * as moment from "moment/moment";

@Component({
  selector: 'app-awaits',
  templateUrl: './awaits.component.html',
  styleUrls: ['./../dashboard.component.scss']
})
export class AwaitsComponent implements OnInit {
  awaits: dashboardTask[] = [];

  constructor(public service: DashboardService) {
    this.service.TodayTasks().subscribe(x => this.awaits = x.filter(x => moment(x.date) > moment(Date.now())))
  }

  format_time(s: string) {
    let now = moment(s);
    return (now.format("HH:mm"));
  }

  ngOnInit(): void {
  }

}
