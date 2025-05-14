using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Casino_Project.Data;
using Casino_Project.Model;
using System.Linq;

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

        [IgnoreAntiforgeryToken]
        public IActionResult OnPostSaveResult([FromBody] MinerGameResult result)
        {
            var user = _context.User.FirstOrDefault();
            if (user == null)
                return NotFound();

            result.UserId = user.Id;
            result.PlayedAt = DateTime.Now;

            if (result.IsWin)
                user.Balance += (int)result.WinAmount;
            else
                user.Balance -= result.BetAmount;

            _context.MinerGameResults.Add(result);
            _context.SaveChanges();

            return new JsonResult(new { success = true, newBalance = user.Balance });
        }

        public class BalanceUpdateModel
        {
            public double Amount { get; set; }
        }
    }
}
