using Casino_Project.Data;
using Casino_Project.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Casino_Project.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly AplicationDbContext _db;
        public IEnumerable<User> Users { get; set; }

        public IndexModel(AplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            Users = _db.User;
        }
    }
}
