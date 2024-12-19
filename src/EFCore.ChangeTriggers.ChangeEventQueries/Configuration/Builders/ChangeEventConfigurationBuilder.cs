namespace EFCore.ChangeTriggers.ChangeEventQueries.Configuration.Builders
{
    /// <summary>
    /// Provides an API for configuring change events.
    /// </summary>
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

        /// <summary>
        /// Configures an entity for change events.
        /// </summary>
        /// <typeparam name="TChangeEntity">The change entity type to configure.</typeparam>
        /// <param name="configureEntity">An action to configure the entity.</param>
        /// <returns></returns>
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