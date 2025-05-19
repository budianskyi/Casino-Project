using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Casino_Project.Model;

namespace Casino_Project.Model
{
    public class Match3Result  // ненужен
    {
        [Key]
        public double BetAmount { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
