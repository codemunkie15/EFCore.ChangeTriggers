using EFCore.ChangeTriggers.ChangeEventQueries.Configuration.Builders;
using EFCore.ChangeTriggers.ChangeEventQueries.Exceptions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Configuration
{
    public class ChangeEventConfiguration
    {
        public IReadOnlyDictionary<Type, ChangeEventEntityConfiguration> EntityConfigurations => entityConfigurations.AsReadOnly();

        private readonly Dictionary<Type, ChangeEventEntityConfiguration> entityConfigurations = [];

        public ChangeEventConfiguration()
        {
            
        }

        public ChangeEventConfiguration(Action<ChangeEventConfigurationBuilder> buildAction)
        {
            var builder = new ChangeEventConfigurationBuilder(this);
            buildAction(builder);
        }

        internal void AddEntityConfigiration(ChangeEventEntityConfiguration entityConfiguration)
        {
            if (!entityConfigurations.TryAdd(entityConfiguration.EntityType, entityConfiguration))
            {
                throw new ChangeEventQueryException(ExceptionStrings.ConfigurationAlreadyAdded(entityConfiguration.EntityType.Name));
            }
        }
    }
}