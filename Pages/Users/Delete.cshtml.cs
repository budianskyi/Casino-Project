using Casino_Project.Data;
using Casino_Project.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Casino_Project.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly AplicationDbContext _db;
        [BindProperty]
        public User User { get; set; }

        public DeleteModel(AplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int id)
        {
            User = _db.Users.Find(id);
        }

        public async Task<IActionResult> OnPost()
        {
            var userFromDb = _db.Users.Find(User.Id);
            if(userFromDb != null)
            {
                _db.Users.Remove(userFromDb);
                await _db.SaveChangesAsync();

                TempData["success"] = "User deleted successfully";
                return RedirectToPage("Index");
            }          
            
            return Page();
        }
    }
}
