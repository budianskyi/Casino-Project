using Casino_Project.Data;
using Casino_Project.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Casino_Project.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly AplicationDbContext _db;
        public User User { get; set; }

        public EditModel(AplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int id)
        {
            User = _db.User.Find(id);
        }

        public async Task<IActionResult> OnPost(User user)
        {
            if (user.UserName == user.Balance.ToString())
            {
                ModelState.AddModelError("User.UserName", "Тест. Баланс не може дорівнювати юзернейму");
            }
            if (ModelState.IsValid)
            {
                _db.User.Update(user);
                await _db.SaveChangesAsync();
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
