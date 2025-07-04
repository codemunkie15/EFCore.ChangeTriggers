namespace EFCore.ChangeTriggers.ChangeEventQueries.Configuration
{
    public class ChangeEventEntityConfiguration
    {
        public Type EntityType { get; }

        public bool AddInserts { get; internal set; }

        public bool AddDeletes { get; internal set; }

        public IEnumerable<ChangeEventEntityPropertyConfiguration> PropertyConfigurations => propertyConfigurations.AsReadOnly();

        private readonly List<ChangeEventEntityPropertyConfiguration> propertyConfigurations = [];

        public ChangeEventEntityConfiguration(Type entityType)
        {
            EntityType = entityType;
        }

        internal void AddPropertyConfiguration(ChangeEventEntityPropertyConfiguration propertyConfiguration)
        {
            propertyConfigurations.Add(propertyConfiguration);
        }
    }
}