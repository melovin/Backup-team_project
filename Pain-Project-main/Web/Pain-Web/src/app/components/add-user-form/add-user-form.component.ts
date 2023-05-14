import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormGroup} from "@angular/forms";


@Component({
  selector: 'app-add-user-form',
  templateUrl: './add-user-form.component.html',
  styleUrls: ['./add-user-form.component.scss']
})
export class AddUserFormComponent implements OnInit {

  @Input()
  public form: FormGroup;

  @Output()
  public FormEvent: EventEmitter<void> = new EventEmitter<void>();

  constructor() { }

  ngOnInit(): void {
  }

  public submit(): void {
    if (this.form.valid && this.confirmPassword()) {
      this.FormEvent.emit();
    }
  }
  public invalid(name: string): boolean {
    if (name == 'email')
      return !RegExp(/^[a-zA-Z0-9.^_`-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)+$/).test(this.form.get(name)?.value) && !!this.form.get(name)?.touched;
    if (name == 'login')
      return !(this.form.get(name)?.value.length > 4)  && !!this.form.get(name)?.touched;
    if (name == 'password')
      return !(this.form.get(name)?.value.length > 7)  && !!this.form.get(name)?.touched;

    return !!this.form.get(name)?.invalid && !!this.form.get(name)?.touched;
  }

  public confirmPassword() : boolean {
    if (this.form.get('confirmPassword')?.touched) {
      return this.form.get('password')?.value === this.form.get('confirmPassword')?.value;
    }
    return true;
  }
}
