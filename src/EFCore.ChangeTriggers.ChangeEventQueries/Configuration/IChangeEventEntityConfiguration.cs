using EFCore.ChangeTriggers.ChangeEventQueries.Configuration.Builders;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Configuration
{
    /// <summary>
    /// Allows configuration of a single change entity for change event queries.
    /// </summary>
    /// <typeparam name="TChangeEntity"></typeparam>
    public interface IChangeEventEntityConfiguration<TChangeEntity>
    {
        /// <summary>
        /// Configures an entity for change events.
        /// </summary>
        /// <param name="builder"></param>
        void Configure(ChangeEventEntityConfigurationBuilder<TChangeEntity> builder);
    }
}