using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using A_AspNetCore.Data;
using A_AspNetCore.Models.Users;

namespace A_AspNetCore.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly SampleDbContext _context;

        public IndexModel(SampleDbContext context)
        {
            _context = context;
        }

        public IList<User> Users { get;set; } = default!;

        public IList<UserChange> UserChanges { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Users = await _context.Users.Include(u => u.Role).ToListAsync();
            UserChanges = await _context.UserChanges.OrderBy(c => c.ChangedAt).ToListAsync();
        }
    }
}
