import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree} from '@angular/router';
import {Observable} from 'rxjs';
import {InterfaceClientsCanDeactivate} from "./interface-clients-can-deactivate";

@Injectable({
  providedIn: 'root'
})
export class GuardClientDirtyGuard implements CanDeactivate<InterfaceClientsCanDeactivate> {
  constructor() {
  }

  canDeactivate(
    component: InterfaceClientsCanDeactivate,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot,
    nextState?: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (!component.canDeactivate())
      return confirm('Are you sure you want to leave?')
    return component.canDeactivate();
  }

}
