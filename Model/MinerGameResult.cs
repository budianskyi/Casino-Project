using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Casino_Project.Model;

namespace Casino_Project.Model
{
    public class MinerGameResult
    {
        [Key]
        public int Id { get; set; }

        public int BetAmount { get; set; }
        public double WinAmount { get; set; }
        public bool IsWin { get; set; }
        public DateTime PlayedAt { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
