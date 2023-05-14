using DatabaseTestAPIJoin.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseTest.Controllers
{
    public class MyContext : DbContext
    {
        public DbSet<Source> tbSources { get; set; }
        public DbSet<User> tbUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=mysqlstudenti.litv.sssvt.cz;database=3b2_stankomichal_db1;user=stankomichal;password=123456;SslMode=none");
        }

    }

}
