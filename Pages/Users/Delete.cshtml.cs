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
            User = _db.User.Find(id);
        }

        public async Task<IActionResult> OnPost()
        {
            var userFromDb = _db.User.Find(User.Id);
            if(userFromDb != null)
            {
                _db.User.Remove(userFromDb);
                await _db.SaveChangesAsync();
                return RedirectToPage("Index");
            }          
            
            return Page();
        }
    }
}
