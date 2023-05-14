import {Pipe, PipeTransform} from '@angular/core';
import {Client} from "../../models/client.model";

@Pipe({name: 'ClientsClientFilter'})
export class ClientsClientFilter implements PipeTransform {
  transform(log: Client[], searchText: string, filterText: string) {
    return log.filter(function (item: any) {
      return (item.name.toLowerCase().includes(searchText.toLowerCase()) && (item.active == filterText || filterText == 'none'))
    });
  }
}
