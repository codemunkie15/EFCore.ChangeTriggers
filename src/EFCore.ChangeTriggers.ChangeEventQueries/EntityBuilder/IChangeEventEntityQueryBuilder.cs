using System.Linq.Expressions;
using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.EntityBuilder
{
    /// <summary>
    /// Represents a query builder that queries <seealso cref="IChange"/> entities and transforms them into human readable change events.
    /// </summary>
    /// <typeparam name="TChange">The change entity type.</typeparam>
    /// <typeparam name="TChangeEvent">The change event that will be built.</typeparam>
    public interface IChangeEventEntityQueryBuilder<TChange, TChangeEvent>
    {
        /// <summary>
        /// Adds a change entity property to the builder.
        /// </summary>
        /// <param name="description">A human readable description of the property.</param>
        /// <param name="valueSelector">A function to select the property value as a string.</param>
        /// <returns>The same entity query builder so further calls can be chained.</returns>
        IChangeEventEntityQueryBuilder<TChange, TChangeEvent> AddProperty(
            string description,
            Expression<Func<TChange, string>> valueSelector);

        /// <summary>
        /// Adds all the properties on the change entity to the builder.
        /// </summary>
        /// <remarks>Excludes primary keys, shadow properties and navigation properties.</remarks>
        /// <param name="descriptionBuilder">An optional function to format the description of properties.</param>
        /// <returns>The same entity query builder so further calls can be chained.</returns>
        IChangeEventEntityQueryBuilder<TChange, TChangeEvent> AddEntityProperties(
            Func<string, string>? descriptionBuilder = null);

        /// <summary>
        /// Builds the query and returns an <see cref="IQueryable{T}"/>.
        /// </summary>
        /// <returns>The built <see cref="IQueryable{T}"/>.</returns>
        IQueryable<TChangeEvent> Build();
    }
}
