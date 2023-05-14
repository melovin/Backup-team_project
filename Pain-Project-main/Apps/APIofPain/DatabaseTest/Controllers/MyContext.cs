using DatabaseTest.DatabaseTables;
using Microsoft.EntityFrameworkCore;

namespace DatabaseTest.Controllers
{
    public class MyContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<LoginLog> LoginLog { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Task> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=mysqlstudenti.litv.sssvt.cz;database=3b2_hasekfrantisek_db1;user=hasekfrantisek;password=123456;SslMode=none");
        }

    }

}
