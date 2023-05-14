import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from "./page-skeleton/Content/dashboard/dashboard.component";
import { LogsComponent } from "./page-skeleton/Content/logs/logs.component";
import { ConfigurationsComponent } from "./page-skeleton/Content/configurations/configurations.component";
import { UsersComponent} from "./page-skeleton/Content/users/users.component";
import { ClientsComponent} from "./page-skeleton/Content/clients/clients.component";

import { LoginPageComponent} from './login-page/login-page.component';
import { PageSkeletonComponent} from "./page-skeleton/page-skeleton.component";


const routes: Routes = [
  {
    path: '', component: LoginPageComponent
  },
  {
    path: 'ui', component: PageSkeletonComponent,
    children: [
      {path : 'dashboard', component: DashboardComponent},
      {path : 'logs', component: LogsComponent},
      {path : 'configurations', component: ConfigurationsComponent},
      {path : 'users', component: UsersComponent},
      {path : 'clients', component: ClientsComponent},
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
