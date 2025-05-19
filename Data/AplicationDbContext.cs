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
        public DbSet<User> Users { get; set; } // Замінив User на Users через конфлікт імен в Azure
        public DbSet<DiceGame> DiceGames { get; set; }
        public DbSet<MinerGameResult> MinerGameResults { get; set; }

    }
}
