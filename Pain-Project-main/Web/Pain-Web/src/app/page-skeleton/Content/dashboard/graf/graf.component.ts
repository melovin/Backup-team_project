import {Component, OnInit} from '@angular/core';
import {DashboardService} from "../../../../services/dasboard.service";

@Component({
  selector: 'app-graf',
  templateUrl: './graf.component.html',
  styleUrls: ['./../dashboard.component.scss']
})
export class GrafComponent implements OnInit {
  percent: string = '50';

  constructor(public service: DashboardService) {
    this.service.GetPercent().subscribe(x => this.percent = x.toFixed(0));
  }

  ngOnInit(): void {
  }

}
