import { Config } from './ConfigInterface';

export const CONFIGS: Config[] = [
  { id: 1, name:'config1', backup_Type: 'ARCHIVE', create_Date: '15.1.2021', creator: 'Pepa', PC:['PC01','PC12'], retention:'1/,1', destinations: ['D:/Data', 'FTP://'], frequency:' * * * * * ', sources: ['C:/Users/Desktop','C:/ProgramFiles']},
  { id: 2, name:'config2', backup_Type: 'ARCHIVE', create_Date: '25.1.2021', creator: 'Alfons', PC:['PC23','PC12'], retention:'1/,1', destinations: ['D:/Data', 'FTP://'], frequency:' * * * * * ', sources: ['C:/Users/Desktop','C:/ProgramFiles','C:/ProgramFiles','C:/ProgramFiles','C:/ProgramFiles','C:/ProgramFiles']},
  { id: 3, name:'config3', backup_Type: 'ARCHIVE', create_Date: '15.2.2021', creator: 'Jakub', PC:['PC11','PC12', 'PC13', 'PC13', 'PC13', 'PC13', 'PC13'], retention:'1/,1', destinations: ['D:/Data', 'FTP://'], frequency:' * * * * * ', sources: ['C:/Users/Desktop','C:/ProgramFiles']},
  { id: 23, name:'config4', backup_Type: 'ARCHIVE', create_Date: '15.1.2011', creator: 'Admin', PC:['PC01'], retention:'1/,1', destinations: ['D:/Data', 'FTP://'], frequency:' * * * * * ', sources: ['C:/Users/Desktop','C:/ProgramFiles']},
  { id: 69, name:'adasda', backup_Type: 'ARCHIVE', create_Date: '15.1.2011', creator: 'Admin', PC:['PC01'], retention:'1/,1', destinations: ['D:/Data', 'FTP://'], frequency:' * * * * * ', sources: ['C:/Users/Desktop','C:/ProgramFiles']},
  { id: 420, name:'poslední', backup_Type: 'ARCHIVE', create_Date: '15.1.2011', creator: 'Admin', PC:['PC01'], retention:'1/,1', destinations: ['D:/Data', 'FTP://'], frequency:' * * * * * ', sources: ['C:/Users/Desktop','C:/ProgramFiles']},
];
