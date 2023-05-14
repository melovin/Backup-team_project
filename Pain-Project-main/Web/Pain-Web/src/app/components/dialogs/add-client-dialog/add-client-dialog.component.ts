import {Component, Inject, OnInit} from '@angular/core';
import {ClientsService} from "../../../services/clients.service";
import {ConfigsService} from "../../../services/configs.service";
import {MAT_DIALOG_DATA} from "@angular/material/dialog";

@Component({
  selector: 'app-add-client-dialog',
  templateUrl: './add-client-dialog.component.html',
  styleUrls: ['./add-client-dialog.component.scss']
})
export class AddClientDialogComponent implements OnInit {

  cssClassIcon = 'NotActiveSearch';
  searchActive: boolean = false;
  searchedClient: string = '';
  clients: clientAdd[] = [];

  constructor(private service: ClientsService,
              private configService: ConfigsService,
              @Inject(MAT_DIALOG_DATA) public data: number
  ) {
  }

  ngOnInit(): void {
    this.configService.clientsByConfig(this.data).subscribe(x => this.clients = x);
  }

  ConfigCheck(isChecked: boolean, checkedClient: clientAdd): void {
    checkedClient.active = !checkedClient.active;
  }

  ActiveChange(): void {
    if (this.searchActive)
      this.searchedClient = '';
    this.searchActive = !this.searchActive;
    this.cssClassIcon = this.searchActive ? 'ActiveSearch' : 'NotActiveSearch';
  }
}

export interface clientAdd {
  id: number;
  name: string;
  active: boolean;
}
