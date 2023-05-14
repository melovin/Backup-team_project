import { Component, OnInit } from '@angular/core';
import { Config } from "../../TestingFiles/ConfigInterface";
import { CONFIGS } from "../../TestingFiles/Configs";

@Component({
  selector: 'app-configurations',
  templateUrl: './configurations.component.html',
  styleUrls: ['./configurations.component.scss']
})
export class ConfigurationsComponent implements OnInit {

  configs = CONFIGS;
  constructor() { }

  ngOnInit(): void {
  }

}
