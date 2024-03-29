﻿namespace EFCore.ChangeTriggers.Constants
{
    internal static class AnnotationConstants
    {
        private const string Prefix = "ChangeTriggers";

        public const string UseChangeTriggers = $"{Prefix}:Use";
        public const string TrackedEntityTypeName = $"{Prefix}:{nameof(TrackedEntityTypeName)}";
        public const string ChangeEntityTypeName = $"{Prefix}:{nameof(ChangeEntityTypeName)}";
        public const string TriggerNameFormat = $"{Prefix}:{nameof(TriggerNameFormat)}";
        public const string HasNoCheckConstraint = $"{Prefix}:{nameof(HasNoCheckConstraint)}";

        public const string IsOperationTypeColumn = $"{Prefix}:{nameof(IsOperationTypeColumn)}";
        public const string IsChangeSourceColumn = $"{Prefix}:{nameof(IsChangeSourceColumn)}";
        public const string IsChangedAtColumn = $"{Prefix}:{nameof(IsChangedAtColumn)}";
        public const string IsChangedByColumn = $"{Prefix}:{nameof(IsChangedByColumn)}";
    }
}