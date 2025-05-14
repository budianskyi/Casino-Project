using Casino_Project.Model;
using DiceGameCasino.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace Casino_Project.Data
{
    public class AplicationDbContext: DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> option): base(option) 
        {
            
        }
        public DbSet<User> User { get; set; }
        public DbSet<DiceGame> DiceGames { get; set; } 
    }
}
