namespace EFCore.ChangeTriggers.ChangeEventQueries.Configuration.Builders
{
    public class ChangeEventEntityPropertyConfigurationBuilder
    {
        private readonly ChangeEventEntityPropertyConfiguration propertyConfiguration;

        public ChangeEventEntityPropertyConfigurationBuilder(ChangeEventEntityPropertyConfiguration propertyConfiguration)
        {
            this.propertyConfiguration = propertyConfiguration;
        }

        public ChangeEventEntityPropertyConfigurationBuilder WithDescription(string description)
        {
            this.propertyConfiguration.Description = description;

            return this;
        }
    }
}