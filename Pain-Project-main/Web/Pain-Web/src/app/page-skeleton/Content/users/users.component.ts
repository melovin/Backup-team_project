import {Component, OnInit} from '@angular/core';
import {User} from "../../../models/user.model";
import {UsersService} from "../../../services/users.service";
import {MatDialog} from "@angular/material/dialog";
import {AddUserDialogComponent} from "../../../components/dialogs/add-user-dialog/add-user-dialog.component";
import {RemoveDialogComponent} from "../../../components/dialogs/remove-dialog/remove-dialog.component";
import {Router} from "@angular/router";

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  sum = 15;
  searchedUser: string = '';
  users: User[] = [];

  constructor(private service: UsersService,
              private dialog: MatDialog,
              private router: Router) {
  }

  ngOnInit(): void {
    this.service.findAllUsers().subscribe(x => this.users = x);
  }

  onScrollDown(ev: any) {
    this.sum += 15;
  }

  onClick(event: any): void {
    event.stopPropagation();
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(AddUserDialogComponent, {
      panelClass: 'custom-dialog-container',
      width: '1000px'
    })
    dialogRef.afterClosed().subscribe()
  }

  removeDialog(idAdmin: number): void {
    const dialogRef = this.dialog.open(RemoveDialogComponent, {
      panelClass: 'custom-dialog-container',
      width: '500px'
    })
    dialogRef.componentInstance.type = 'user';
    dialogRef.afterClosed().subscribe(result => {
      if (result == true)
        this.service.removeUser(idAdmin).subscribe(() => this.Reload())
    })
  }

  private Reload(): void {
    let currentUrl = this.router.url;
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.router.onSameUrlNavigation = 'reload';
    this.router.navigate([currentUrl]).then();
  }
}
