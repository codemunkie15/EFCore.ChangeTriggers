using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests
{
    public class ToChangeEventsTestParams
    {
        public ChangeEventConfiguration ChangeEventConfiguration { get; set; }

        public IServiceProvider CustomServiceProvider { get; set; }

        public static ToChangeEventsTestParams Build(
            ConfigurationProviderMode configurationProviderMode,
            ChangeEventConfiguration changeEventConfiguration,
            IServiceProvider customServiceProvider)
        {
            var toChangeEventsTestParams = new ToChangeEventsTestParams();

            if (configurationProviderMode == ConfigurationProviderMode.Parameter)
            {
                toChangeEventsTestParams.ChangeEventConfiguration = changeEventConfiguration;
            }
            else if (configurationProviderMode == ConfigurationProviderMode.DbContext)
            {
                toChangeEventsTestParams.CustomServiceProvider = customServiceProvider;
            }

            return toChangeEventsTestParams;
        }
    }
}