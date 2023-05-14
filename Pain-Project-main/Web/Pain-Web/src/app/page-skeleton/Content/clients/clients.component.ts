import {Component, OnInit} from '@angular/core';

import {Client} from "../../../models/client.model";
import {ClientsService} from "../../../services/clients.service";
import {MatDialog} from "@angular/material/dialog";
import {RemoveDialogComponent} from "../../../components/dialogs/remove-dialog/remove-dialog.component";
import {InterfaceClientsCanDeactivate} from "../../../Guards/interface-clients-can-deactivate";
import {ConfigsService} from "../../../services/configs.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.scss']
})
export class ClientsComponent implements OnInit, InterfaceClientsCanDeactivate {
  sum = 15;
  isDirty: boolean = false;
  searchedClient: string = '';
  filterValue: string = 'none';
  clients: Client[] = [];

  changesClients: { [Key: number]: boolean } = {};

  constructor(private service: ClientsService,
              public dialog: MatDialog,
              private configService: ConfigsService,
              private router: Router,) {
  }

  activeChange(idClient: number): void {
    // @ts-ignore
    this.changesClients[idClient] = !this.clients.find(x => x.id == idClient).active;
  }

  saveActives(): void {
    this.isDirty = false;
    this.service.changeClients(this.changesClients).subscribe(() => this.Reload());
    this.changesClients = {};
  }

  ngOnInit(): void {
    this.service.findAllClients().subscribe(x => this.clients = x);
  }

  onClick(event: any): void {
    event.stopPropagation();
  }

  canDeactivate(): boolean {
    return !this.isDirty;
  }

  onScrollDown(ev: any) {
    this.sum += 15;
  }

  editConfig(id: number) {
    this.configService.findConfigByID(id).subscribe(x => this.router.navigateByUrl('/ui/edit-config', {state: x}))
  }

  openDialog(type: any, idClient: number, idConfig: number): void {
    if (type == 'client') {
      const dialogRef = this.dialog.open(RemoveDialogComponent, {
        panelClass: 'custom-dialog-container',
        width: '500px',
      })
      dialogRef.componentInstance.type = 'client';
      dialogRef.afterClosed().subscribe(result => {
        if (result == true)
          this.service.removeClient(idClient).subscribe(() => this.Reload())
      })
    } else if (type == 'configFromClient') {
      const dialogRef = this.dialog.open(RemoveDialogComponent, {
        panelClass: 'custom-dialog-container',
        width: '500px',
      })
      dialogRef.componentInstance.type = 'configFromClient';
      dialogRef.afterClosed().subscribe(result => {
        if (result == true)
          this.configService.removeClientFromConfig(idConfig, idClient).subscribe(() => this.Reload())
      })
    }
  }

  private Reload(): void {
    let currentUrl = this.router.url;
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.router.onSameUrlNavigation = 'reload';
    this.router.navigate([currentUrl]).then();
  }
}
