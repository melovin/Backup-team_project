using System.Collections.Generic;

namespace DatabaseTestAPIJoin.ReturnModels
{
    public class CompleteUser
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string create_date { get; set; }
        public List<string> sources { get; set; }
    }
}
