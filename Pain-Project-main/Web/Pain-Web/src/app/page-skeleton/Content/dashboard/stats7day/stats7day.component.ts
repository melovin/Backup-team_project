import {Component, OnInit} from '@angular/core';
import {DashboardService} from "../../../../services/dasboard.service";

@Component({
  selector: 'app-stats7day',
  templateUrl: './stats7day.component.html',
  styleUrls: ['./../dashboard.component.scss']
})
export class Stats7dayComponent implements OnInit {
  success: number = 0;
  noRun: number = 0;
  error: number = 0;

  constructor(public service: DashboardService) {
    this.service.SevenDays().subscribe(x => {
      x.forEach(value => {
        if (value.state == 'Success')
          this.success++;
        else if (value.state == "NoRun")
          this.noRun++;
        else
          this.error++;
      })
    })
  }

  ngOnInit(): void {
  }

}
