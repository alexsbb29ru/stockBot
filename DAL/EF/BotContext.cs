using Microsoft.EntityFrameworkCore;
using Models.Enities;

namespace DAL.EF
{
    public sealed class BotContext : DbContext
    {
        //"Server=(localdb)\\mssqllocaldb;Database=StockBotDb;Trusted_Connection=True;"
        private string _connectionString => "Server=(localdb)\\mssqllocaldb;Database=StockBotDb;Trusted_Connection=True;";

        public DbSet<Users> Users { get; set; }

        public BotContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}