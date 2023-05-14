import {Component, OnInit, Input, Output} from '@angular/core';
import {FormGroup, FormControl, FormBuilder, Validators, FormArray} from "@angular/forms";
import {Config, Destination} from "../../models/config.model";
import {Client} from "../../models/client.model";
import {ClientsService} from "../../services/clients.service";
import {ConfigsService} from "../../services/configs.service";
import {LoginService} from "../../services/login.service";
import {Router} from '@angular/router';
import {MatSnackBar} from "@angular/material/snack-bar";

@Component({
  selector: 'app-stepper',
  templateUrl: './stepper.component.html',
  styleUrls: ['./stepper.component.scss']
})
export class StepperComponent implements OnInit {
  searchClient: FormControl = new FormControl('');
  clients: Client[] = [];
  frequentionBasic = true;

  //full ; diff ; inc
  backup = 'full'

  //daily ; weekly ; monthly
  frequention = 'daily'

  searchActive = false;
  @Input()
  public form: FormGroup;

  @Output()
  public formArray: FormArray

  public config: Config = new Config();

  tempDest: Destination[] = [{destType: "drive", path: ""}]
  tempClient = {};
  tempDaysWeek: number[] = [];


  constructor(private fb: FormBuilder,
              private service: ClientsService,
              private configService: ConfigsService,
              private loginService: LoginService,
              private router: Router,
              private snackBar: MatSnackBar) {
  }


  ngOnInit(): void {
    this.form = this.CreateForm(this.config);
    this.service.findAllClients().subscribe(x => this.clients = x.filter(x => x.active))
  }

  public AddRemoveClient(id: number): void {

    if (this.tempClient.hasOwnProperty(id)) {
      // @ts-ignore
      delete this.tempClient[id]
    } else {
      // @ts-ignore
      this.tempClient[id] = ''
    }
  }

  public AddRemoveDayWeek(id: number): void {
    if (this.tempDaysWeek.find(x => x == id))
      this.tempDaysWeek.splice(this.tempDaysWeek.indexOf(id), 1);
    else
      this.tempDaysWeek.push(id);
  }

  public Submit(): void {
    if (this.Name?.value == '') {
      this.snackBar.open('Please enter config name!', '', {duration: 2000, panelClass: ['snackbar']});
      return
    }

    if (Object.keys(this.tempClient).length == 0) {
      this.snackBar.open('No clients selected!', '', {duration: 2000, panelClass: ['snackbar']});
      return
    }
    if (this.frequention == 'weekly' && this.tempDaysWeek.length == 0) {
      this.snackBar.open('You must select days of week!', '', {
        duration: 2000,
        panelClass: ['snackbar']
      });
      return
    }
    let config: Config = new Config();
    let secondStep: SecondStep = this.form.controls['formArray'].value[1];

    if (this.frequention == 'monthly' && secondStep.dayOfMonth < 1 || secondStep.dayOfMonth > 31) {
      this.snackBar.open('Please select valid day of month!', '', {
        duration: 2000,
        panelClass: ['snackbar']
      });
      return
    }
    if (secondStep.backups < 1 || (secondStep.packages < 1 && secondStep.backupType != 'FB')) {
      this.snackBar.open('Please enter valid retention!', '', {
        duration: 2000,
        panelClass: ['snackbar']
      });
      return
    }
    if (this.Sources.value.length == 0) {
      this.snackBar.open('Please enter source!', '', {duration: 2000, panelClass: ['snackbar']});
      return
    }
    if (this.Dest.value.length == 0) {
      this.snackBar.open('Please enter destination!', '', {duration: 2000, panelClass: ['snackbar']});
      return
    }


    config.cron = this.BuildCron();
    config.clientNames = this.tempClient;

    config.name = this.Name?.value;
    config.backUpType = secondStep.backupType;
    config.idAdministrator = this.loginService.GetLogin().Id;
    config.retentionPackageSize = config.backUpType == 'FB' ? 1 : secondStep.packages;
    config.retentionPackages = secondStep.backups;
    config.backUpFormat = secondStep.backupFormat;

    for (let source of this.Sources.value) {
      if (source.path == '' || !RegExp(`((?!\\?|\\\\|\\/|\\:|\\*|\\"|\\<|\\>|\\|).)+`).test(source.path)) {
        this.snackBar.open('Please enter valid source!', '', {
          duration: 2000,
          panelClass: ['snackbar']
        });
        return
      }
      if (config.sources.find(x => x == source.path)) {
        this.snackBar.open('All sources must be unique!', '', {
          duration: 2000,
          panelClass: ['snackbar']
        });
        return
      }
      config.sources.push(source.path);
    }
    let re = RegExp("^(ftp:\\/\\/)[A-Za-z0-9\\-_.]{1,}:[A-Za-z0-9?\\-_.:]{1,}@[A-Za-z0-9\\/\\-_/:.]{1,}$");
    for (let dest of this.Dest.value) {
      if (dest.type == "FTP" && !re.test(dest.path)) {
        this.snackBar.open('Please enter valid FTP pattern!', '', {
          duration: 2000,
          panelClass: ['snackbar']
        });
        return
      }
      if (dest.type == 'DRIVE' && !RegExp(`((?!\\?|\\\\|\\/|\\:|\\*|\\"|\\<|\\>|\\|).)+`).test(dest.path)) {
        this.snackBar.open('Please enter valid destination!', '', {
          duration: 2000,
          panelClass: ['snackbar']
        });
        return
      }

      if (dest.path == '') {
        this.snackBar.open('Please enter valid destination!', '', {
          duration: 2000,
          panelClass: ['snackbar']
        });
        return
      }
      if (config.sources.find(x => x == dest.path)) {
        this.snackBar.open(`Destination can't be same as source!`, '', {
          duration: 2000,
          panelClass: ['snackbar']
        });
        return
      }

      if (config.destinations.find(x => x.path == dest.path)) {
        this.snackBar.open('All destinations must be unique!', '', {
          duration: 2000,
          panelClass: ['snackbar']
        });
        return
      }
      let destination: Destination = {destType: dest.type, path: dest.path}
      config.destinations.push(destination);
    }

    this.configService.sendConfig(config).subscribe(() => (
        this.router.navigateByUrl('/ui/dashboard'),
          this.snackBar.open('Config was successfully created!', '', {
            duration: 2000,
            panelClass: ['snackbar']
          })),
      () => this.snackBar.open('Config name is already taken!', '', {
        duration: 2000,
        panelClass: ['snackbar']
      }));
  }

  private BuildCron(): string {
    let cron = '';
    let secondStep: SecondStep = this.form.controls['formArray'].value[1];
    let minutes = secondStep.frequency.split(':');

    if (this.frequentionBasic) {
      if (this.frequention == 'daily') {
        cron = `${minutes[1]} ${minutes[0]} * * *`
      } else if (this.frequention == 'weekly') {
        let days = ''
        for (let day of this.tempDaysWeek) {
          switch (day) {
            case 1:
              days += '1,'
              break
            case 2:
              days += '2,'
              break
            case 3:
              days += '3,'
              break
            case 4:
              days += '4,'
              break
            case 5:
              days += '5,'
              break
            case 6:
              days += '6,'
              break
            case 7:
              days += '0,'
              break
          }
        }
        days = days.slice(0, -1);
        cron = `${minutes[1]} ${minutes[0]} * * ${days}`
      } else {
        cron = `${minutes[1]} ${minutes[0]} ${secondStep.dayOfMonth} * *`
      }
    } else {
      cron = `${secondStep.minute} ${secondStep.hour} ${secondStep.dayM} ${secondStep.month} ${secondStep.dayW}`
    }
    return cron;
  }


  private CreateForm(config: Config): FormGroup {
    return this.fb.group({
      formArray: this.fb.array([
        this.fb.group({
          clients: new FormArray([])
        }),
        this.fb.group({
          backupType: ['FB', Validators.required],
          backupFormat: ['PT', Validators.required],
          frequency: ['12:00', Validators.required],
          minute: ['*', Validators.required],
          hour: ['*', Validators.required],
          dayM: ['*', Validators.required],
          month: ['*', Validators.required],
          dayW: ['*', Validators.required],
          dayOfMonth: [1, Validators.required],
          packages: [1, Validators.required],
          backups: [1, Validators.required],
        }),
        this.fb.group({
          sources: this.fb.array([this.fb.group({
            path: ['', Validators.required]
          })]),
          destinations: this.fb.array([this.fb.group({
            type: ['DRIVE', Validators.required],
            path: ['', Validators.required]
          })]),
          configName: ['', Validators.required],
        }),
        this.fb.group({
          name: [config.name, Validators.required]
        })
      ])
    })
  }

  get Sources() {
    let test = this.form.controls['formArray'] as FormArray;
    let test2 = test.controls[2].get('sources') as FormArray;
    return test2 as FormArray;
  }

  get Name() {
    let test = this.form.controls['formArray'] as FormArray;
    return test.controls[2].get('configName')
  }

  get Dest() {
    let test = this.form.controls['formArray'] as FormArray;
    let test2 = test.controls[2].get('destinations') as FormArray;
    return test2 as FormArray;
  }

  public addSource(): void {
    const sourceTest = this.fb.group({path: ['', Validators.required]})
    this.Sources.push(sourceTest);
  }

  public removeSource(index: number): void {
    this.Sources.removeAt(index);
  }

  public addDest(): void {
    const destTest = this.fb.group({
      type: ['DRIVE', Validators.required],
      path: ['', Validators.required]
    })
    this.Dest.push(destTest);
  }

  public removeDest(index: number): void {
    this.Dest.removeAt(index);
  }
}

export interface SecondStep {
  backupType: any,
  retention: any,
  backupFormat: any,
  frequency: any
  minute: any,
  hour: any,
  dayM: any,
  month: any,
  dayW: any,
  dayOfMonth: any,
  packages: any,
  backups: any,
}
