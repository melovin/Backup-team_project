import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatRadioModule} from "@angular/material/radio";
import {LoginPageComponent} from './login-page/login-page.component';
import {PageSkeletonComponent} from './page-skeleton/page-skeleton.component';
import {NavBarComponent} from './page-skeleton/nav-bar/nav-bar.component';
import {DashboardComponent} from './page-skeleton/Content/dashboard/dashboard.component';
import {LogsComponent} from './page-skeleton/Content/logs/logs.component';
import {ConfigsComponent} from './page-skeleton/Content/configs/configs.component';
import {ClientsComponent} from './page-skeleton/Content/clients/clients.component';
import {UsersComponent} from './page-skeleton/Content/users/users.component';
import {MatToolbarModule} from "@angular/material/toolbar";
import {MatExpansionModule} from "@angular/material/expansion";
import {MatButtonModule} from "@angular/material/button";
import {ScrollingModule} from "@angular/cdk/scrolling";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatDialogModule} from '@angular/material/dialog';
import {MatInputModule} from '@angular/material/input';
import {MatDividerModule} from '@angular/material/divider';
import {MatGridListModule} from '@angular/material/grid-list';
import {ErrorPageComponent} from './error-page/error-page.component';
import {SettingsComponent} from './components/dialogs/settings-dialog/settings.component';
import {MatSlideToggleModule} from "@angular/material/slide-toggle";
import {MatIconModule} from "@angular/material/icon";
import {MatSelectModule} from "@angular/material/select";
import {NgSelectModule} from "@ng-select/ng-select";
import {MatRippleModule} from "@angular/material/core";
import {RemoveDialogComponent} from './components/dialogs/remove-dialog/remove-dialog.component';
import {AddUserDialogComponent} from './components/dialogs/add-user-dialog/add-user-dialog.component';
import {AddUserFormComponent} from './components/add-user-form/add-user-form.component';
import {Stats1dayComponent} from './page-skeleton/Content/dashboard/stats1day/stats1day.component';
import {Stats7dayComponent} from './page-skeleton/Content/dashboard/stats7day/stats7day.component';
import {ProblemsComponent} from './page-skeleton/Content/dashboard/problems/problems.component';
import {AddconfigbuttonComponent} from './page-skeleton/Content/dashboard/addconfigbutton/addconfigbutton.component';
import {GrafComponent} from './page-skeleton/Content/dashboard/graf/graf.component';
import {CompletedComponent} from './page-skeleton/Content/dashboard/completed/completed.component';
import {AwaitsComponent} from './page-skeleton/Content/dashboard/awaits/awaits.component';
import {Backupsize} from './page-skeleton/Content/dashboard/backupsize/backupsize';
import {AddConfigComponent} from './page-skeleton/Content/add-config/add-config.component';
import {StepperComponent} from './components/stepper/stepper.component';
import {MatStepperModule} from '@angular/material/stepper';
import {EditConfigComponent} from './page-skeleton/Content/edit-config/edit-config.component';
import {AddClientDialogComponent} from './components/dialogs/add-client-dialog/add-client-dialog.component';
import {Ng2SearchPipeModule} from "ng2-search-filter";
import {ConfigsClientSearch} from "./components/pipes/Configs-ClientSearch";
import {ConfigsConfigSearch} from "./components/pipes/Configs-ConfigSearch";
import {LogsLogFilter} from "./components/pipes/Logs-LogFilter";
import {ClientsClientFilter} from "./components/pipes/Clients-ClientFilter";
import {UsersUserSearch} from "./components/pipes/Users-UserSearch";
import {InfiniteScrollModule} from "ngx-infinite-scroll";
import {AddClientSearch} from "./components/pipes/AddClient-ClientSearch";
import {HttpClientModule} from "@angular/common/http";
import {JwtModule} from "@auth0/angular-jwt";
import {StepperEditComponent} from "./components/stepper-edit/stepperEdit.component";
import {MatSnackBarModule} from '@angular/material/snack-bar';

function tokenGetter() {
  return sessionStorage.getItem('token');
}

@NgModule({
  declarations: [
    AppComponent,
    LoginPageComponent,
    PageSkeletonComponent,
    NavBarComponent,
    DashboardComponent,
    LogsComponent,
    ConfigsComponent,
    ClientsComponent,
    UsersComponent,
    SettingsComponent,
    RemoveDialogComponent,
    ErrorPageComponent,
    AddUserDialogComponent,
    AddUserFormComponent,
    Stats1dayComponent,
    Stats7dayComponent,
    ProblemsComponent,
    AddconfigbuttonComponent,
    GrafComponent,
    CompletedComponent,
    AwaitsComponent,
    Backupsize,
    AddConfigComponent,
    StepperComponent,
    EditConfigComponent,
    AddClientDialogComponent,
    ConfigsClientSearch,
    ConfigsConfigSearch,
    LogsLogFilter,
    ClientsClientFilter,
    UsersUserSearch,
    AddClientSearch,
    StepperEditComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    BrowserAnimationsModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatToolbarModule,
    MatExpansionModule,
    MatButtonModule,
    ScrollingModule,
    MatCheckboxModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule,
    MatInputModule,
    MatDividerModule,
    MatGridListModule,
    MatSlideToggleModule,
    MatIconModule,
    MatSelectModule,
    NgSelectModule,
    MatRippleModule,
    ReactiveFormsModule,
    MatStepperModule,
    Ng2SearchPipeModule,
    InfiniteScrollModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter
      },
    }),
    MatSnackBarModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
