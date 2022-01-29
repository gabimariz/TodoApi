using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data
{
    public class TodoDbContext : DbContext
    {
        public DbSet<Todo>? Todo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySql("server=localhost;username=root;password=77539783;database=todo",
                new MariaDbServerVersion(new Version(10, 6, 5)));
        }
    }
}
