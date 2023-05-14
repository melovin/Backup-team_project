import {Config} from "./config.model";

export class Client {
  public id: number;
  public name : string = '';
  public ip : string = '';
  public mac : string = '';
  public active : boolean = false;
  public online : boolean = false;
  public configs : Config[];
}
