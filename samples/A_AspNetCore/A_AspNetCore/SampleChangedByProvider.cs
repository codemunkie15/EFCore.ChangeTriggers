using A_AspNetCore.Data;
using A_AspNetCore.Models.Users;
using EFCore.ChangeTriggers.Abstractions;

namespace A_AspNetCore
{
    public class SampleChangedByProvider : ChangedByProvider<User>
    {
        private readonly SampleDbContext dbContext;

        public SampleChangedByProvider(SampleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public override User GetChangedBy()
        {
            return dbContext.Users.Find(1)
                ?? throw new Exception("ChangedBy user doesn't exist.");
        }

        public override async Task<User> GetChangedByAsync()
        {
            return await dbContext.Users.FindAsync(1)
                ?? throw new Exception("ChangedBy user doesn't exist.");
        }

        public override User GetMigrationChangedBy()
        {
            return new User { Id = 2 };
        }

        public override Task<User> GetMigrationChangedByAsync()
        {
            return Task.FromResult(new User { Id = 2 });
        }
    }
}