using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using A_AspNetCore.Data;
using A_AspNetCore.Models.Roles;

namespace A_AspNetCore.Pages.Roles
{
    public class IndexModel : PageModel
    {
        private readonly SampleDbContext _context;

        public IndexModel(SampleDbContext context)
        {
            _context = context;
        }

        public IList<Role> Role { get;set; } = default!;

        public IList<RoleChange> RoleChanges { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Role = await _context.Roles.ToListAsync();
            RoleChanges = await _context.RoleChanges.OrderBy(c => c.ChangedAt).ToListAsync();
        }
    }
}
