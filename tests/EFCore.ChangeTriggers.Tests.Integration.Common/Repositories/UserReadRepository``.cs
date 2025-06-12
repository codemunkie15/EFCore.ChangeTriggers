using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Repositories
{
    public class UserReadRepository<TDbContext, TUser, TUserChange> : IUserReadRepository<TUser, TUserChange>
        where TDbContext : TestDbContext<TUser, TUserChange>
        where TUser : UserBase, ITracked<TUserChange>
        where TUserChange : class, IHasChangeId
    {
        protected readonly TDbContext dbContext;

        public UserReadRepository(TDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public virtual IQueryable<TUser> GetUsers()
        {
            return dbContext.TestUsers
                .Include(u => u.Changes);
        }

        public virtual IQueryable<TUserChange> GetUserChanges()
        {
            return dbContext.TestUserChanges;
        }
    }
}
