using IffleyRoutesRecord.Logic.Entities;
using IffleyRoutesRecord.Logic.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Logic
{
    internal static class Utilities
    {
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
            else if (entityId < 0)
            {
                throw new EntityNotFoundException($"{typeof(TEntity).FullName} with ID {entityId} was not found.");
            }
            else
            {
                throw new InternalEntityNotFoundException();
            }
        }

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
            else
            {
                return;
            }
        }
    }
}
