using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using RaNetCore.Common.Entities.Interfaces;
using RaNetCore.Database.Interfaces;

namespace RaNetCore.Database.Extensions
{
    public static class UpdateExtensions
    {
        public static IQueryable<T> Include<T>(this IQueryable<T> source, IEnumerable<string> navigationPropertyPaths)
            where T : class
        {
            return navigationPropertyPaths
                .Aggregate(source, (query, path) => query.Include(path));
        }

        public static IEnumerable<T> Except<T, TKey>(this IEnumerable<T> items, IEnumerable<T> other, Func<T, TKey> getKeyFunc)
        {
            var resultGroupJoin = items
                 .GroupJoin(other, getKeyFunc, getKeyFunc, (item, tempItems) => new { item, tempItems });
            var resultSelect = resultGroupJoin
                .SelectMany(t => t.tempItems.DefaultIfEmpty(), (t, temp) => new { t, temp });
            var resultWhere = resultSelect
                .Where(t => ReferenceEquals(null, t.temp) || t.temp.Equals(default(T)));
            var result = resultWhere
                .Select(t => t.t.item);

            return result;
        }

        public static void TryUpdateManyToMany<T, TKey>(this IRaNetCoreDbContext db, IEnumerable<T> currentItems, IEnumerable<T> newItems, Func<T, TKey> getKey) where T : class
        {
            db.Set<T>().RemoveRange(currentItems.Except(newItems, getKey));
            db.Set<T>().AddRange(newItems.Except(currentItems, getKey));
        }

        public static T DetachLocal<T>(this IRaNetCoreDbContext db, T t, int entryId)
            where T : class, IIdentifiable
        {
            var local = db.Set<T>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(entryId));

            if (local != null)
            {
                db.Entry(local).State = EntityState.Detached;
            }

            db.Entry(t).State = EntityState.Modified;

            return t;
        }
    }
}
