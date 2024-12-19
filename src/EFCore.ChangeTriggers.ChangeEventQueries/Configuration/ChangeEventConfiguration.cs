using EFCore.ChangeTriggers.ChangeEventQueries.Configuration.Builders;
using EFCore.ChangeTriggers.ChangeEventQueries.Exceptions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Configuration
{
    /// <summary>
    /// Provides global configuration for change event queries.
    /// </summary>
    public class ChangeEventConfiguration
    {
        /// <summary>
        /// Gets all entity configurations.
        /// </summary>
        public IReadOnlyDictionary<Type, ChangeEventEntityConfiguration> EntityConfigurations => entityConfigurations.AsReadOnly();

        private readonly Dictionary<Type, ChangeEventEntityConfiguration> entityConfigurations = [];

        public ChangeEventConfiguration()
        {
            
        }

        /// <summary>
        /// Creates a new change event configuration instance.
        /// </summary>
        /// <param name="buildAction">An action to build the configuration.</param>
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