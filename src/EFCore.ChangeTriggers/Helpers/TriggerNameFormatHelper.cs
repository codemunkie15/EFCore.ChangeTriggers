using EFCore.ChangeTriggers.Exceptions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.ChangeTriggers.Helpers
{
    internal static class TriggerNameFormatHelper
    {
        public static string GetTriggerNameFormat(Func<string, string> triggerNameFactory, IReadOnlyEntityType? trackedEntityType = null)
        {
            // Replace the func parameters with placeholders that will be formatted later
            var triggerName = triggerNameFactory.Invoke("{0}");

            if (triggerName.IndexOf("{0}") != triggerName.LastIndexOf("{0}"))
            {
                if (trackedEntityType != null)
                {
                    throw new ChangeTriggersConfigurationException(
                        $"Trigger name for tracked entity type '{trackedEntityType.Name}' cannot contain the value '{{0}}'.");
                }
                else
                {
                    throw new ChangeTriggersConfigurationException(
                        $"Trigger name cannot contain the value '{{0}}'.");
                }
            }

            return triggerName;
        }
    }
}
