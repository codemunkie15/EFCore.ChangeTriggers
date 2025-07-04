using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Repositories
{
    public class ChangedByEntityUserReadRepository : UserReadRepository<ChangedByEntityDbContext, ChangedByEntityUser, ChangedByEntityUserChange>
    {
        public ChangedByEntityUserReadRepository(ChangedByEntityDbContext dbContext)
            : base(dbContext)
        {
        }

        public override IQueryable<ChangedByEntityUser> GetUsers()
        {
            return dbContext.TestUsers
                .Include(u => u.Changes)
                    .ThenInclude(uc => uc.ChangedBy);
        }

        public override IQueryable<ChangedByEntityUserChange> GetUserChanges()
        {
            return dbContext.TestUserChanges
                .Include(uc => uc.ChangedBy);
        }
    }
}
