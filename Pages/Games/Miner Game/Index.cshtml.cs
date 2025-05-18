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
            User = _context.User.FirstOrDefault();
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostSaveResult([FromBody] MinerGameResult result)
        {
            var user = _context.User.FirstOrDefault();
            if (user == null)
                return NotFound();

            // Перевірка, чи такий результат вже був зафіксований
            bool alreadySaved = _context.MinerGameResults.Any(r => r.UserId == user.Id && r.PlayedAt == result.PlayedAt);
            if (alreadySaved)
            {
                return new JsonResult(new { success = false, message = "Result already saved." });
            }

            result.UserId = user.Id;
            result.PlayedAt = DateTime.Now;

            //Console.WriteLine($"[DEBUG] Баланс до: {user.Balance}");

            if (result.IsWin)
                user.Balance += (int)result.WinAmount;
            else
                user.Balance -= result.BetAmount;

            Console.WriteLine($"[DEBUG] Баланс після: {user.Balance}");

            _context.MinerGameResults.Add(result);
            _context.Entry(user).State = EntityState.Modified;
            _context.Entry(user).Property(u => u.Balance).IsModified = true;

            var saved = _context.SaveChanges();
            //Console.WriteLine($"[DEBUG] SaveChanges -> {saved} рядків");

            return new JsonResult(new { success = true, newBalance = user.Balance });
        }
    }
}
