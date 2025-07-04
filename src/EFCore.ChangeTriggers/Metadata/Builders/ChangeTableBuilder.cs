using EFCore.ChangeTriggers.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.ChangeTriggers.Metadata.Builders
{
    /// <summary>
    /// Provides an API for configuring a change table.
    /// </summary>
    public class ChangeTableBuilder
    {
        private readonly EntityTypeBuilder builder;

        public ChangeTableBuilder(EntityTypeBuilder builder)
        {
            this.builder = builder;
        }

        /// <summary>
        /// Configures the change table to track who made changes.
        /// </summary>
        /// <returns>The same change table builder so further calls can be chained</returns>
        public ChangeTableBuilder HasChangedBy()
        {
            builder.HasAnnotation(AnnotationConstants.HasChangedBy, true);

            return this;
        }

        /// <summary>
        /// Configures the change table to track where changes originated from.
        /// </summary>
        /// <returns>The same change table builder so further calls can be chained</returns>
        public ChangeTableBuilder HasChangeSource()
        {
            builder.HasAnnotation(AnnotationConstants.HasChangeSource, true);

            return this;
        }
    }
}