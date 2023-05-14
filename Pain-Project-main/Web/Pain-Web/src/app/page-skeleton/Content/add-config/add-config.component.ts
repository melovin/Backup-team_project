import { Component, OnInit } from '@angular/core';
import {Config} from "../../../models/config.model";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-add-config',
  templateUrl: './add-config.component.html',
  styleUrls: ['./add-config.component.scss']
})
export class AddConfigComponent implements OnInit {
// public config: Config = new Config();
public form: FormGroup


  constructor() { }

  ngOnInit(): void {
  // this.form = this.CreateForm(this.config);
  }


// private CreateForm(config: Config):FormGroup{
//   return this.fb.group({
//       formArray: this.fb.array([
//         this.fb.group({
//           clients :[config.PCs, Validators.required]
//         }),
//         this.fb.group({
//           backupType :[config.backup_type, Validators.required],
//           retention :[config.retention, Validators.required],
//           backupFormat :[config.backup_format, Validators.required],
//           frequency :[config.frequency, Validators.required]
//         }),
//         this.fb.group({
//           sources :[config.sources, Validators.required],
//           destinations :[config.destinations, Validators.required]
//         }),
//         this.fb.group({
//           name :[config.name, Validators.required]
//         })
//       ])
//   })
// }
public SaveConfig():void{
  alert("Config saved");
}
}
