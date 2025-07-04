using System.Linq.Expressions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Configuration.Builders
{
    /// <summary>
    /// Provides an API for configuring an entity for change events.
    /// </summary>
    /// <typeparam name="TChangeEntity"></typeparam>
    public class ChangeEventEntityConfigurationBuilder<TChangeEntity>
    {
        private readonly ChangeEventEntityConfiguration entityConfiguration;

        public ChangeEventEntityConfigurationBuilder(ChangeEventEntityConfiguration EntityConfiguration)
        {
            this.entityConfiguration = EntityConfiguration;
        }

        /// <summary>
        /// Configures a property to be included in change event queries.
        /// </summary>
        /// <param name="propertySelector">A function to select the property to configure.</param>
        /// <returns>The same builder instance so multiple calls can be chained.</returns>
        public ChangeEventEntityPropertyConfigurationBuilder AddProperty(Expression<Func<TChangeEntity, string>> propertySelector)
        {
            var propertyConfiguration = new ChangeEventEntityPropertyConfiguration(propertySelector);

            entityConfiguration.AddPropertyConfiguration(propertyConfiguration);

            return new ChangeEventEntityPropertyConfigurationBuilder(propertyConfiguration);
        }

        /// <summary>
        /// Configures inserts to be included in change event queries.
        /// </summary>
        /// <returns>The same builder instance so multiple calls can be chained.</returns>
        public ChangeEventEntityConfigurationBuilder<TChangeEntity> AddInserts()
        {
            entityConfiguration.AddInserts = true;
            return this;
        }

        /// <summary>
        /// Configures deletes to be included in change event queries.
        /// </summary>
        /// <returns>The same builder instance so multiple calls can be chained.</returns>
        public ChangeEventEntityConfigurationBuilder<TChangeEntity> AddDeletes()
        {
            entityConfiguration.AddDeletes = true;
            return this;
        }
    }
}
