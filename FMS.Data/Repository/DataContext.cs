using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using FMS.Data.Models;

namespace FMS.Data.Repository
{

    public class DataContext : DbContext
    {  
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Mot> Mots { get; set; }
        public DbSet<User> Users { get; set; }
               
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
            .UseSqlite("Filename=data.db");
            //  .LogTo(Console.WriteLine, LogLevel.Information);
        }

        // custom method used in development to keep database in sync with models
        public void Initialise() 
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        
    }
} 
