using System.ComponentModel.DataAnnotations;

namespace DatabaseTest.DatabaseTables
{
    public class Administrator
    {
        [Key]
        public int Id { get; set; }
        
        public string Login { get; set; }
        
        public string Password { get; set; }
        
        public string AccountCreation { get; set; } 
        
        public string Name { get; set; }
        
        public string Surname { get; set; }
        
        public string Email { get; set; }
        
        public bool DarkMode { get; set; }
    }
}
