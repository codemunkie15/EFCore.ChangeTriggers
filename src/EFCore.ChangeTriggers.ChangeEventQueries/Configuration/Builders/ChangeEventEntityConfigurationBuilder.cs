using System.Linq.Expressions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Configuration.Builders
{
    public class ChangeEventEntityConfigurationBuilder<TChangeEntity>
    {
        private readonly ChangeEventEntityConfiguration entityConfiguration;

        public ChangeEventEntityConfigurationBuilder(ChangeEventEntityConfiguration EntityConfiguration)
        {
            this.entityConfiguration = EntityConfiguration;
        }

        public ChangeEventEntityPropertyConfigurationBuilder AddProperty(Expression<Func<TChangeEntity, string>> propertySelector)
        {
            var propertyConfiguration = new ChangeEventEntityPropertyConfiguration(propertySelector);

            entityConfiguration.AddPropertyConfiguration(propertyConfiguration);

            return new ChangeEventEntityPropertyConfigurationBuilder(propertyConfiguration);
        }

        public ChangeEventEntityConfigurationBuilder<TChangeEntity> AddInserts()
        {
            entityConfiguration.AddInserts = true;
            return this;
        }

        public ChangeEventEntityConfigurationBuilder<TChangeEntity> AddDeletes()
        {
            entityConfiguration.AddInserts = true;
            return this;
        }
    }
}
