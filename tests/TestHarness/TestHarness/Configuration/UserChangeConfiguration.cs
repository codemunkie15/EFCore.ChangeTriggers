using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestHarness.DbModels.Users;

namespace TestHarness.Configuration
{
    public class UserChangeConfiguration : IChangeEventEntityConfiguration<UserChange>, IEntityTypeConfiguration<UserChange>
    {
        public void Configure(ChangeEventEntityConfigurationBuilder<UserChange> builder)
        {
            builder.AddProperty(uc => uc.Name);
            builder.AddProperty(uc => uc.DateOfBirth);
            builder.AddProperty(uc => uc.PrimaryPaymentMethod!.Id.ToString())
                .WithDescription("Primary payment method changed");
        }

        public void Configure(EntityTypeBuilder<UserChange> builder)
        {
            throw new NotImplementedException();
        }
    }
}