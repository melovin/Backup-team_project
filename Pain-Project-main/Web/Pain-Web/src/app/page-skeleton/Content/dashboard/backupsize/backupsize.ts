import {Component, OnInit} from '@angular/core';
import {DashboardService} from "../../../../services/dasboard.service";

@Component({
  selector: 'app-backupsize',
  templateUrl: './backupsize.component.html',
  styleUrls: ['./../dashboard.component.scss']
})
export class Backupsize implements OnInit {
  size: number = 0;

  constructor(public service: DashboardService) {
    this.service.GetSize().subscribe(x => this.size = x);
  }

  ngOnInit(): void {
  }

}
