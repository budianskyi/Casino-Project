using Casino_Project.Data;
using Casino_Project.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Casino_Project.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly AplicationDbContext _db;
        public User User { get; set; }

        public CreateModel(AplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(User user)
        {
            if (user.UserName == user.Balance.ToString())
            {
                ModelState.AddModelError("User.UserName", "Тест. Баланс не може дорівнювати юзернейму");
            }
            if (ModelState.IsValid)
            {
                await _db.User.AddAsync(user);
                await _db.SaveChangesAsync();

                TempData["success"] = "User created successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
