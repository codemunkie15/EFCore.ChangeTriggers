namespace EFCore.ChangeTriggers.ChangeEventQueries.Configuration.Builders
{
    public class ChangeEventConfigurationBuilder
    {
        private ChangeEventConfiguration changeEventConfiguration;

        public ChangeEventConfigurationBuilder(ChangeEventConfiguration queryConfiguration)
        {
            this.changeEventConfiguration = queryConfiguration;
        }

        public ChangeEventConfigurationBuilder()
            : this(new())
        {
        }

        public ChangeEventConfigurationBuilder Configure<TChangeEntity>(Action<ChangeEventEntityConfigurationBuilder<TChangeEntity>> configureEntity)
        {
            var entityConfiguration = new ChangeEventEntityConfiguration(typeof(TChangeEntity));
            var entityBuilder = new ChangeEventEntityConfigurationBuilder<TChangeEntity>(entityConfiguration);
            configureEntity(entityBuilder);

            changeEventConfiguration.AddEntityConfigiration(entityConfiguration);

            return this;
        }

        internal void Configure<TChangeEntity>(IChangeEventEntityConfiguration<TChangeEntity> configuration)
        {
            Configure<TChangeEntity>(configuration.Configure);
        }

        internal ChangeEventConfiguration Build()
        {
            return changeEventConfiguration;
        }
    }
}