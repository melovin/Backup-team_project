export interface Destination {
  destType: string;
  path: string;
}
export interface Client {
  id: number;
  name: string
}
export class Config {
  id: number;
  name: string;
  createDate: string;
  cron: string;
  backUpFormat: string;
  backUpType: string;
  retentionPackages: number;
  retentionPackageSize: number;
  adminName: string;
  idAdministrator: number;
  sources: string[] = [];
  destinations: Destination[] = [];
  clientNames = {};

}
