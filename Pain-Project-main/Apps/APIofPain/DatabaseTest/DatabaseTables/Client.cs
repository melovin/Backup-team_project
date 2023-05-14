using System;
using System.ComponentModel.DataAnnotations;

namespace DatabaseTest.DatabaseTables
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
        public bool Active { get; set; }
        public DateTimeOffset LastSeen { get; set; }
        public string Hash { get; set; }
    }
}
