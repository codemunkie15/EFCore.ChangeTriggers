namespace EntityFrameworkCore.ChangeTrackingTriggers.Constants
{
    internal static class ChangeTrackingContextConstants
    {
        private const string Prefix = "ChangeTrackingContext";

        public const string ChangedByContextName = $"{Prefix}.ChangedBy";
        public const string ChangeSourceContextName = $"{Prefix}.ChangeSource";
    }
}
