using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseTestAPIJoin.Database.Models
{
    public class Source
    {
        [Key]
        public int id { get; set; }
        [Column("idUser")]
        [ForeignKey("User")]
        public int idUser { get; set; }
        public string source { get; set; }


        public User User { get; set; }
    }
}
