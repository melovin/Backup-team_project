import { Pipe, PipeTransform } from '@angular/core';
import { User } from "../../models/user.model";

@Pipe({ name: 'UsersUserSearch' })
export class UsersUserSearch implements PipeTransform {
  transform(users: User[], searchText: string) {
    return users.filter( function (item: any) {
      return (item.name.toLowerCase().includes(searchText.toLowerCase())
        || item.surname.toLowerCase().includes(searchText.toLowerCase()));
    });
  }
}
