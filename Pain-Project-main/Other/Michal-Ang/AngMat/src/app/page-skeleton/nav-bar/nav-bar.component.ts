import { Component, OnInit } from '@angular/core';
import { LoginPageComponent} from "../../login-page/login-page.component";
import {LogsComponent} from "../Content/logs/logs.component";

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {
  constructor() { }

  ngOnInit(): void {

  }
  onClickLogout(): void {
  }
}
