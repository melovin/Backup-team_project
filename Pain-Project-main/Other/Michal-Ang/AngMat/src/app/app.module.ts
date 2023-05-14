import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginPageComponent } from './login-page/login-page.component';
import { PageSkeletonComponent } from './page-skeleton/page-skeleton.component';
import { NavBarComponent } from './page-skeleton/nav-bar/nav-bar.component';
import { DashboardComponent } from './page-skeleton/Content/dashboard/dashboard.component';
import {MatToolbarModule} from "@angular/material/toolbar";
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import {MatDividerModule} from "@angular/material/divider";
import { LogsComponent } from './page-skeleton/Content/logs/logs.component';
import { ConfigurationsComponent } from './page-skeleton/Content/configurations/configurations.component';
import { ClientsComponent } from './page-skeleton/Content/clients/clients.component';
import { UsersComponent } from './page-skeleton/Content/users/users.component';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatButtonModule} from "@angular/material/button";

@NgModule({
  declarations: [
    AppComponent,
    LoginPageComponent,
    PageSkeletonComponent,
    NavBarComponent,
    DashboardComponent,
    LogsComponent,
    ConfigurationsComponent,
    ClientsComponent,
    UsersComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    NgbModule,
    MatDividerModule,
    MatExpansionModule,
    MatProgressSpinnerModule,
    MatButtonModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
