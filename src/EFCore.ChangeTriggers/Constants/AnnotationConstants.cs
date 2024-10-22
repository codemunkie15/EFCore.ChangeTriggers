namespace EFCore.ChangeTriggers.Constants
{
    internal static class AnnotationConstants
    {
        private const string Prefix = "ChangeTriggers";

        public const string HasChangeTrigger = $"{Prefix}:{nameof(HasChangeTrigger)}";
        public const string IsChangeTable = $"{Prefix}:{nameof(IsChangeTable)}";
        public const string ChangedByClrTypeName = $"{Prefix}:{nameof(ChangedByClrTypeName)}";
        public const string ChangeSourceClrTypeName = $"{Prefix}:{nameof(ChangeSourceClrTypeName)}";
        public const string TriggerNameFormat = $"{Prefix}:{nameof(TriggerNameFormat)}";
        public const string HasNoCheckConstraint = $"{Prefix}:{nameof(HasNoCheckConstraint)}";

        public const string IsOperationTypeColumn = $"{Prefix}:{nameof(IsOperationTypeColumn)}";
        public const string IsChangeSourceColumn = $"{Prefix}:{nameof(IsChangeSourceColumn)}";
        public const string IsChangedAtColumn = $"{Prefix}:{nameof(IsChangedAtColumn)}";
        public const string IsChangedByColumn = $"{Prefix}:{nameof(IsChangedByColumn)}";
        public const string IsTrackedEntityForeignKey = $"{Prefix}:{nameof(IsTrackedEntityForeignKey)}";
    }
}