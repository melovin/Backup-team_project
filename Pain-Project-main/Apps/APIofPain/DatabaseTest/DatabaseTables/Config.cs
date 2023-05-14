using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseTest.DatabaseTables
{
    public class Config
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Administrator")]
        public int? IdAdministrator { get; set; }

        public string Name { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public string Cron { get; set; }
        
        public string BackUpFormat { get; set; }
        
        public string BackUpType { get; set; }
        
        public int RetentionPackages { get; set; }
        
        public int RetentionPackageSize { get; set; }
        


        public Administrator? Administrator { get; set; }
        public List<Source>? Sources { get; set; }
        public List<Destination>? Destinations { get; set; }
    }
}
