import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {SessionsService} from "../services/sessions.service";
import {filter} from "rxjs";
import {LoginService} from "../services/login.service";

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})

export class LoginPageComponent implements OnInit {
  hide = true;
  name = '';
  password = '';

  failed = false;

  public form: FormGroup

  constructor(
              private router : Router,
              private sessions : SessionsService,
              private fb: FormBuilder,
              ) { }

  ngOnInit(): void {
    this.form = this.fb.group( {
      Login: ["", Validators.required],
      Password: ["", Validators.required]
    })
  }
  buttonOpen(event : any): void {
    event.stopPropagation();
  }
  Submit() : void {
    this.sessions.login(this.form.value).subscribe( (bool) => bool ? this.router.navigate(['ui', 'dashboard']) : (this.failed = true, this.form.reset()))
  }
}
