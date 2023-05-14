export class EmailSettingsModel{
    public port:number = 25;
    public smtp:string = '';
    public freq:number = 0;
    public sender:string = '';
    public password:string = '';
    public ssl:boolean = false;
}