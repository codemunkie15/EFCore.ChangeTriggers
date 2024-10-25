using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using A_AspNetCore.Data;
using A_AspNetCore.Models.Users;
using EFCore.ChangeTriggers.ChangeEventQueries;
using A_AspNetCore.Models;
using EFCore.ChangeTriggers.ChangeEventQueries.Extensions;

namespace A_AspNetCore.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly SampleDbContext _context;

        public DetailsModel(SampleDbContext context)
        {
            _context = context;
        }

        public User Users { get; set; } = default!;

        public IList<ChangeEvent<User, ChangeSource>> UserChangeEvents { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                Users = user;
                UserChangeEvents = await _context
                    .CreateChangeEventQueryBuilder<User, ChangeSource>()
                    .AddChanges(_context.UserChanges.Where(c => c.Id == id), builder =>
                    {
                        builder
                            .AddProperty("Name changed", c => c.Name)
                            .AddProperty("Date of birth changed", c => c.DateOfBirth)
                            .AddProperty("Role changed", c => c.Role.Name);
                    })
                    .Build()
                    .ToListAsync();
            }
            return Page();
        }
    }
}
