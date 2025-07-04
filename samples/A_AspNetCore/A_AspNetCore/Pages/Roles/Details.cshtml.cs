using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using A_AspNetCore.Data;
using A_AspNetCore.Models.Roles;
using A_AspNetCore.Models.Users;
using A_AspNetCore.Models;
using EFCore.ChangeTriggers.ChangeEventQueries;
using EFCore.ChangeTriggers.ChangeEventQueries.Extensions;

namespace A_AspNetCore.Pages.Roles
{
    public class DetailsModel : PageModel
    {
        private readonly SampleDbContext _context;

        public DetailsModel(SampleDbContext context)
        {
            _context = context;
        }

        public Role Role { get; set; } = default!;

        public IList<ChangeEvent<User, ChangeSource>> RoleChangeEvents { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FirstOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }
            else
            {
                Role = role;
                RoleChangeEvents = await _context
                    .CreateChangeEventQueryBuilder<User, ChangeSource>()
                    .AddChanges(_context.RoleChanges.Where(c => c.Id == id), builder =>
                    {
                        builder
                            .AddProperty("Name changed", c => c.Name)
                            .AddProperty("Enabled changed", c => c.Enabled.ToString());
                    })
                    .Build()
                    .ToListAsync();
            }
            return Page();
        }
    }
}
