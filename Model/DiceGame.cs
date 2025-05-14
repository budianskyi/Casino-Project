using System.ComponentModel.DataAnnotations;
using Casino_Project.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiceGameCasino.Models
{
    public class DiceGame
    {
        [Key]
        public int Id { get; set; }

        public int BetAmount { get; set; }
        public int PlayerBet { get; set; }
        public int NumberOfDice { get; set; }
        public int TotalDiceResult { get; set; }
        public int Coefficient { get; set; }
        public string ResultMessage { get; set; }
        public DateTime PlayedAt { get; set; }

        // Зв'язок з користувачем
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
