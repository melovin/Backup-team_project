import {Observable} from "rxjs";

export interface InterfaceClientsCanDeactivate {
  canDeactivate: () => boolean | Observable<boolean>
}
