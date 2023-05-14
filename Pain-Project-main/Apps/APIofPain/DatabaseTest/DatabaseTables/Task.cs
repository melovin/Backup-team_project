using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseTest.DatabaseTables
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("Assignment")]
        public int IdAssignment { get; set; }
        
        public string State { get; set; }
        
        public string Message { get; set; } //v db Varchar(0)? (úmysl jako max -> nefunguje -> navýšeno na 255 zatím)
        
        public DateTime Date { get; set; }
        
        [Column("Size[MB]")]
        public int Size { get; set; } 

        public Assignment Assignment { get; set; }
    }
}
