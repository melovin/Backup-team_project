import {Pipe, PipeTransform} from '@angular/core';
import {Config} from "../../models/config.model";

@Pipe({name: 'ConfigsConfigSearch'})
export class ConfigsConfigSearch implements PipeTransform {
  transform(configs: Config[], searchText: string) {
    return configs.filter(function (item: any) {
      return (item.name.toLowerCase().includes(searchText.toLowerCase()));
    });
  }
}
