using System.Collections.Generic;

namespace DatabaseTestAPIJoin.Database.Models
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string createDate { get; set; }


        public List<Source> Sources { get; set; }
    }
}
