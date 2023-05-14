import {Component, OnInit} from '@angular/core';
import {DashboardService} from "../../../../services/dasboard.service";
import * as moment from "moment/moment";

@Component({
  selector: 'app-stats1day',
  templateUrl: './stats1day.component.html',
  styleUrls: ['./../dashboard.component.scss']
})
export class Stats1dayComponent implements OnInit {
  success: number = 0;
  noRun: number = 0;
  error: number = 0;

  constructor(public service: DashboardService) {
    this.service.TodayTasks().subscribe(x => {
      x.forEach(value => {
        if (moment(value.date) < moment(Date.now())) {
          if (value.state == 'Success')
            this.success++;
          else if (value.state == "NoRun")
            this.noRun++;
          else
            this.error++;
        }
      })
    })
  }

  ngOnInit(): void {
  }

}
