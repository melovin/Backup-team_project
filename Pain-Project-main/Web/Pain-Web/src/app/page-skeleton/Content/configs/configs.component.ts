import {Component, OnInit} from '@angular/core';

import {MatDialog} from "@angular/material/dialog";
import {RemoveDialogComponent} from "../../../components/dialogs/remove-dialog/remove-dialog.component";
import {ConfigsService} from "../../../services/configs.service";
import {Config} from "../../../models/config.model";
import {AddClientDialogComponent} from "../../../components/dialogs/add-client-dialog/add-client-dialog.component";
import {Router} from "@angular/router";

@Component({
  selector: 'app-configs',
  templateUrl: './configs.component.html',
  styleUrls: ['./configs.component.scss']
})
export class ConfigsComponent implements OnInit {
  sum = 15;
  searchedClient: string = '';
  configs: Config[] = [];

  constructor(public dialog: MatDialog,
              private configService: ConfigsService,
              private router: Router) {
  }


  ngOnInit(): void {
    this.configService.findAllConfigs().subscribe(configs => this.configs = configs);
  }

  onScrollDown(ev: any) {
    this.sum += 15;
  }

  onClick(event: any): void {
    event.stopPropagation();
  }

  EditConfig(id: number) {
    let cf = this.configs.find(x => x.id == id);
    this.router.navigateByUrl('/ui/edit-config', {state: cf}).then()
  }

  openDialog(type: any, config: Config, client: number | null): void {
    if (type == 'Add') {
      const dialogRef = this.dialog.open(AddClientDialogComponent, {
        panelClass: 'custom-dialog-container',
        width: '800px',
        data: config.id,
      })
      dialogRef.afterClosed().subscribe(result => {
        console.log(result);
        if (result != false)
          this.configService.changeClients(config.id, result).subscribe(() => this.Reload());
      })
    } else if (type == 'Remove') {
      const dialogRef = this.dialog.open(RemoveDialogComponent, {
        panelClass: 'custom-dialog-container',
        width: '500px',
      })
      dialogRef.componentInstance.type = 'config';
      dialogRef.afterClosed().subscribe(result => {
        if (result == true) {
          this.configService.removeConfig(config.id).subscribe(() => this.Reload());
        }
      })
    } else if (type == 'RemoveClient') {
      const dialogRef = this.dialog.open(RemoveDialogComponent, {
        panelClass: 'custom-dialog-container',
        width: '500px'
      })
      dialogRef.componentInstance.type = 'clientFromConfig';
      dialogRef.afterClosed().subscribe(result => {
        if (result == true) { // @ts-ignore
          this.configService.removeClientFromConfig(config.id, client).subscribe(() => this.Reload());
        }
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
