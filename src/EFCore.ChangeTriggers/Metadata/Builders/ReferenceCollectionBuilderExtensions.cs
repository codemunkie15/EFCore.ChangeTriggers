﻿using EFCore.ChangeTriggers.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.ChangeTriggers.Metadata.Builders
{
    internal static class ReferenceCollectionBuilderExtensions
    {
        public static ReferenceCollectionBuilder IsTrackedEntityForeignKey(this ReferenceCollectionBuilder builder)
        {
            return builder.HasAnnotation(AnnotationConstants.IsTrackedEntityForeignKey, true);
        }

        public static ReferenceCollectionBuilder IsChangedByForeignKey(this ReferenceCollectionBuilder builder)
        {
            return builder.HasAnnotation(AnnotationConstants.IsChangedByColumn, true);
        }

        public static ReferenceCollectionBuilder IsChangeSourceForeignKey(this ReferenceCollectionBuilder builder)
        {
            return builder.HasAnnotation(AnnotationConstants.IsChangeSourceColumn, true);
        }
    }
}
