namespace EFCore.ChangeTriggers.ChangeEventQueries.Configuration.Builders
{
    /// <summary>
    /// Provides an API for configuring a property included in change event queries.
    /// </summary>
    public class ChangeEventEntityPropertyConfigurationBuilder
    {
        private readonly ChangeEventEntityPropertyConfiguration propertyConfiguration;

        public ChangeEventEntityPropertyConfigurationBuilder(ChangeEventEntityPropertyConfiguration propertyConfiguration)
        {
            this.propertyConfiguration = propertyConfiguration;
        }

        /// <summary>
        /// Configures the description of this property.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public ChangeEventEntityPropertyConfigurationBuilder WithDescription(string description)
        {
            this.propertyConfiguration.Description = description;

            return this;
        }
    }
}