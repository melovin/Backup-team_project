namespace DatabaseTest.DatabaseTables
{
    public class Client
    {
        public Client()
        {

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
        public bool Active { get; set; }
    }
}
