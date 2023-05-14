using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseTest.DatabaseTables
{
    public class LoginLog
    {
        [Key]
        
        public int Id { get; set; }
        
        [ForeignKey("Administrator")]
        public int IdAdministrator { get; set; }
        
        public string LoginTime { get; set; }
        
        public string IpAddress { get; set; }


        public Administrator Administrator { get; set; }
    }
}
