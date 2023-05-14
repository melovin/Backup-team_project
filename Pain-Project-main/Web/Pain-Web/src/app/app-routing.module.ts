import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginPageComponent} from "./login-page/login-page.component";
import {PageSkeletonComponent} from "./page-skeleton/page-skeleton.component";
import {DashboardComponent} from "./page-skeleton/Content/dashboard/dashboard.component";
import {ClientsComponent} from "./page-skeleton/Content/clients/clients.component";
import {ConfigsComponent} from "./page-skeleton/Content/configs/configs.component";
import {UsersComponent} from "./page-skeleton/Content/users/users.component";
import {LogsComponent} from "./page-skeleton/Content/logs/logs.component";
import {ErrorPageComponent} from "./error-page/error-page.component";
import {AddConfigComponent} from "./page-skeleton/Content/add-config/add-config.component";
import {EditConfigComponent} from "./page-skeleton/Content/edit-config/edit-config.component";
import {GuardClientDirtyGuard} from "./Guards/guard-client-dirty.guard";
import {AuthGuard} from "./Guards/auth.guard";

const routes: Routes = [
  {path: '', component: LoginPageComponent},
  {
    path: 'ui', component: PageSkeletonComponent, canActivate: [AuthGuard],
    children: [
      {path: 'dashboard', component: DashboardComponent},
      {path: 'logs', component: LogsComponent},
      {path: 'logs/:id', component: LogsComponent},
      {path: 'clients', component: ClientsComponent, canDeactivate: [GuardClientDirtyGuard]},
      {path: 'configs', component: ConfigsComponent},
      {path: 'users', component: UsersComponent},
      {path: 'add-config', component: AddConfigComponent},
      {path: 'edit-config', component: EditConfigComponent},
    ]

  },
  {path: '**', component: ErrorPageComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
