using Casino_Project.Data;
using Casino_Project.Model;
using DiceGameCasino.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Casino_Project.Pages.Games.Dice_Game
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public DiceGame Game { get; set; } = new DiceGame();

        [BindProperty]
        public User User { get; set; } = new User();

        private readonly AplicationDbContext _context;

        public IndexModel(AplicationDbContext context)
        {
            _context = context;
        }

        public void OnPost()
        {
            // Тестово — вибір першого користувача
            var dbUser = _context.User.FirstOrDefault(); // в реальному проекті — через логін

            if (dbUser == null)
            {
                Game.ResultMessage = "❌ Користувач не знайдений.";
                return;
            }

            var rnd = new Random();
            int sum = 0;

            for (int i = 0; i < Game.NumberOfDice; i++)
            {
                sum += rnd.Next(1, 7);
            }

            Game.TotalDiceResult = sum;
            Game.Coefficient = Game.NumberOfDice switch
            {
                1 => 5,
                2 => 3,
                3 => 2,
                _ => 1
            };

            if (Game.PlayerBet == sum)
            {
                int win = Game.BetAmount * Game.Coefficient;
                dbUser.Balance += win;
                Game.ResultMessage = $"🎉 Ви виграли {win} монет! Випало: {sum}";
            }
            else
            {
                dbUser.Balance -= Game.BetAmount;
                Game.ResultMessage = $"😢 Ви програли {Game.BetAmount} монет. Випало: {sum}";
            }

            ViewData["Balance"] = dbUser.Balance;

            // Запис в базу
            Game.UserId = dbUser.Id;
            Game.PlayedAt = DateTime.Now;

            try
            {
                _context.DiceGames.Add(Game);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Game.ResultMessage = $"❌ Помилка збереження: {ex.InnerException?.Message ?? ex.Message}";
            }
        }

        public void OnGet()
        {
            ViewData["Balance"] = User.Balance;
        }
    }
}
