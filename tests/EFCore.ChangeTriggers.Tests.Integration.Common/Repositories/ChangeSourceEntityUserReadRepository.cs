using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Repositories
{
    public class ChangeSourceEntityUserReadRepository : UserReadRepository<ChangeSourceEntityDbContext, ChangeSourceEntityUser, ChangeSourceEntityUserChange>
    {
        public ChangeSourceEntityUserReadRepository(ChangeSourceEntityDbContext dbContext)
            : base(dbContext)
        {
        }

        public override IQueryable<ChangeSourceEntityUser> GetUsers()
        {
            return dbContext.TestUsers
                .Include(u => u.Changes)
                    .ThenInclude(uc => uc.ChangeSource);
        }

        public override IQueryable<ChangeSourceEntityUserChange> GetUserChanges()
        {
            return dbContext.TestUserChanges
                .Include(uc => uc.ChangeSource);
        }
    }
}
