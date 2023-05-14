import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {User} from "../../../models/user.model";
import {UsersService} from "../../../services/users.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Router} from "@angular/router";
import {MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-add-user-dialog',
  templateUrl: './add-user-dialog.component.html',
  styleUrls: ['./add-user-dialog.component.scss']
})
export class AddUserDialogComponent implements OnInit {

  public user: User = new User();
  public form: FormGroup;

  constructor(private fb: FormBuilder,
              private service: UsersService,
              private snackBar: MatSnackBar,
              private router: Router,
              public dialogRef: MatDialogRef<AddUserDialogComponent>) {
  }

  ngOnInit(): void {
    this.form = this.createForm(this.user);
  }

  private createForm(user: User): FormGroup {
    return this.fb.group({
      name: [user.name, Validators.required],
      surname: [user.surname, Validators.required],
      login: [user.login, Validators.required],
      email: [user.email, Validators.required],
      password: [user.password, Validators.required],
      confirmPassword: [user.password, Validators.required],
    });
  }

  public submit(): void {
    // noinspection JSDeprecatedSymbols
    this.service.addUser(this.form.value).subscribe(() => (this.snackBar.open('User has been successfully added!', '', {
        duration: 2000,
        panelClass: ['snackbar']
      }),
      this.Reload(),
      this.dialogRef.close()
      )
      , () => this.snackBar.open(`User hasn't been added`, '', {
        duration: 2000,
        panelClass: ['snackbar']
      })
    );

  }
  private Reload(): void {
    let currentUrl = this.router.url;
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.router.onSameUrlNavigation = 'reload';
    this.router.navigate([currentUrl]).then();
  }
}
