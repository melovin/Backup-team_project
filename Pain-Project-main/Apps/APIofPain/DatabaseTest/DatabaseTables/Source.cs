using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseTest.DatabaseTables
{
    public class Source
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("Config")]
        public int IdConfig { get; set; }
        
        public string Path { get; set; } //v db Varchar(0)? (úmysl jako max -> nefunguje -> navýšeno na 260 zatím)


        public Config Config { get; set; }
    }
}
