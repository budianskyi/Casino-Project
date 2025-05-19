using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Casino_Project.Data;
using Casino_Project.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Casino_Project.Pages.Games.Miner
{
    public class IndexModel : PageModel
    {
        private readonly AplicationDbContext _context;

        public IndexModel(AplicationDbContext context)
        {
            _context = context;
        }

        public User User { get; set; }

        public void OnGet()
        {
            User = _context.Users.FirstOrDefault();
        }

        [IgnoreAntiforgeryToken]
        public IActionResult OnPostSaveResult([FromBody] MinerGameResult result)
        {
            var user = _context.Users.FirstOrDefault();
            if (user == null)
            {
                Console.WriteLine("❌ Користувача не знайдено");
                return NotFound();
            }

            result.UserId = user.Id;
            result.PlayedAt = DateTime.Now;

            Console.WriteLine($"🎯 Виграш: {result.IsWin}, Сума: {result.WinAmount}, Ставка: {result.BetAmount}");

            if (result.IsWin)
            {
                Console.WriteLine($"💰 Баланс ДО: {user.Balance}");
                user.Balance += (int)result.WinAmount;
                Console.WriteLine($"✅ Баланс ПІСЛЯ: {user.Balance}");
            }
            else
            {
                user.Balance -= result.BetAmount;
            }

            _context.MinerGameResults.Add(result);

            // Примусово вказуємо EF, що баланс змінено
            _context.Entry(user).Property(u => u.Balance).IsModified = true;

            int changes = _context.SaveChanges();
            Console.WriteLine($"🧾 SaveChanges(): {changes}");

            return new JsonResult(new { success = true, newBalance = user.Balance });
        }
    }
}
