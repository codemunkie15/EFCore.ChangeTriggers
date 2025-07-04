using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration.Builders;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Configuration
{
    public abstract class UserChangeEventConfiguration<TUserChange> : IChangeEventEntityConfiguration<TUserChange>
        where TUserChange : UserBase
    {
        public void Configure(ChangeEventEntityConfigurationBuilder<TUserChange> builder)
        {
            builder.AddProperty(u => u.Username);
            builder.AddProperty(u => u.DateOfBirth);
            builder.AddProperty(u => u.IsAdmin.ToString());
            builder.AddProperty(u => u.LastUpdatedAt.ToString());
        }
    }
}