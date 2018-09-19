using IffleyRoutesRecord.Logic.Exceptions;
using IffleyRoutesRecord.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace IffleyRoutesRecord.Logic.StaticHelpers
{
    internal static class Utilities
    {
        /// <summary>
        /// Verifies that <paramref name="entities"/> contains an entity with ID <paramref name="entityId"/>
        /// including unsaved changes
        /// </summary>
        /// <typeparam name="TEntity">Type of the entities</typeparam>
        /// <param name="entities">DbSet to search in</param>
        /// <param name="entityId">ID to search for</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="InternalEntityNotFoundException"></exception>
        internal static void VerifyEntityWithIdExists<TEntity>(this DbSet<TEntity> entities, int entityId) where TEntity : BaseEntity
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            if (entities.Any(entity => entity.Id == entityId) || entities.Local.Any(entity => entity.Id == entityId))
            {
                return;
            }
            // If the ID is not positive it was generated internally, not due to user error.
            else if (entityId > 0)
            {
                throw new EntityNotFoundException($"{typeof(TEntity).FullName} with ID {entityId} was not found.");
            }
            else
            {
                throw new InternalEntityNotFoundException();
            }
        }

        /// <summary>
        /// Verifies that <paramref name="entities"/> does not contain an entity with the given name
        /// including unsaved changes
        /// </summary>
        /// <typeparam name="TEntity">Type of the entities</typeparam>
        /// <param name="entities">DbSet to search in</param>
        /// <param name="name">Name to search for</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="EntityWithNameAlreadyExistsException"></exception>
        internal static void VerifyEntityWithNameDoesNotExists<TEntity>(this DbSet<TEntity> entities, string name) where TEntity : BaseNamedEntity
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            if (entities.Any(entity => entity.Name == name) || entities.Local.Any(entity => entity.Name == name))
            {
                throw new EntityWithNameAlreadyExistsException($"A {typeof(TEntity).Name} with name '{name}' already exists.");
            }
        }
    }
}
