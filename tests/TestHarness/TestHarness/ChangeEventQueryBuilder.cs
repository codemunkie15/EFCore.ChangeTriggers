using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestHarness
{
    internal class ChangeEventQueryBuilder
    {
        private readonly DbContext context;
        private List<IQueryable<ChangeEvent>> changeQueries = new();

        public ChangeEventQueryBuilder(DbContext context)
        {
            this.context = context;
        }

        public ChangeEventQueryBuilder AddEntityQuery<TChange, TTracked, TChangeId>(
            IQueryable<TChange> dbSet,
            Action<ChangeEventEntityQueryBuilder<TChange, TTracked, TChangeId>> entityBuilder)
            where TChange : class, IChange<TTracked, TChangeId>
        {
            var entityBuilderInstance = new ChangeEventEntityQueryBuilder<TChange, TTracked, TChangeId>(context, dbSet);
            entityBuilder(entityBuilderInstance);

            changeQueries.Add(entityBuilderInstance.Build());

            return this;
        }

        public IQueryable<ChangeEvent> Build()
        {
            if (!changeQueries.Any())
            {
                throw new InvalidOperationException("There are no queries configured to build.");
            }

            IQueryable<ChangeEvent>? final = null;

            foreach (var query in changeQueries)
            {
                if (final == null)
                {
                    final = query;
                }
                else
                {
                    final = final.Union(query);
                }
            }

            return final!;
        }
    }
}
