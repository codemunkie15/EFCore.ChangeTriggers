namespace EntityFrameworkCore.ChangeTrackingTriggers.Constants
{
    internal static class AnnotationConstants
    {
        private const string Prefix = "ChangeTrackingTriggers";

        public const string UseChangeTrackingTriggers = $"{Prefix}:Use";
        public const string TrackedEntityTypeName = $"{Prefix}:{nameof(TrackedEntityTypeName)}";
        public const string ChangeEntityTypeName = $"{Prefix}:{nameof(ChangeEntityTypeName)}";
        public const string TriggerNameFormat = $"{Prefix}:{nameof(TriggerNameFormat)}";
        public const string HasNoCheckConstraint = $"{Prefix}:{nameof(HasNoCheckConstraint)}";

        public const string IsChangeContextColumn = $"{Prefix}:{nameof(IsChangeContextColumn)}";
        public const string IsOperationTypeColumn = $"{Prefix}:{nameof(IsOperationTypeColumn)}";
        public const string IsChangeSourceColumn = $"{Prefix}:{nameof(IsChangeSourceColumn)}";
        public const string IsChangedAtColumn = $"{Prefix}:{nameof(IsChangedAtColumn)}";
        public const string IsChangedByColumn = $"{Prefix}:{nameof(IsChangedByColumn)}";
    }
}