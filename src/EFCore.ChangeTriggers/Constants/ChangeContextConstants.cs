namespace EFCore.ChangeTriggers.Constants
{
    internal static class ChangeContextConstants
    {
        private const string Prefix = "ChangeContext";

        public const string ChangedByContextName = $"{Prefix}.ChangedBy";
        public const string ChangeSourceContextName = $"{Prefix}.ChangeSource";
    }
}
