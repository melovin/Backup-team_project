import {Pipe, PipeTransform} from '@angular/core';
import {clientAdd} from "../dialogs/add-client-dialog/add-client-dialog.component";

@Pipe({name: 'ConfigsClientSearch'})
export class ConfigsClientSearch implements PipeTransform {
  transform(clients: clientAdd[], searchText: string) {
    return clients.filter(function (item: any) {
      return (item.name.toLowerCase().includes(searchText.toLowerCase()));
    });
  }
}
