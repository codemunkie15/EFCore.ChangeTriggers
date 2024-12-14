using EFCore.ChangeTriggers.ChangeEventQueries.Configuration.Builders;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Configuration
{
    public interface IChangeEventEntityConfiguration<TChangeEntity>
    {
        void Configure(ChangeEventEntityConfigurationBuilder<TChangeEntity> builder);
    }
}