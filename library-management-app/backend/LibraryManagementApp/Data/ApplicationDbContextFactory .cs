using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LibraryManagementApp.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseMySql(
                "Server=localhost;Port=3306;Database=ap-library-db;User=root;Password=root;",
                new MySqlServerVersion(new Version(9, 0, 0)) 
            );

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}