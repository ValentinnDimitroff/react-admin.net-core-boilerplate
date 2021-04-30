using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using RaNetCore.Database.Interfaces;

namespace RaNetCore.Database.Extensions
{
    public static class NavigationExtensions
    {
        public static IEnumerable<string> GetIncludePaths(this IRaNetCoreDbContext context,
            Type clrEntityType)
        {
            HashSet<INavigation> includedNavigations = new HashSet<INavigation>();
            Stack<IEnumerator<INavigation>> stack = new Stack<IEnumerator<INavigation>>();

            IEntityType entityType = context
                .Model
                .FindEntityType(clrEntityType);

            while (true)
            {
                List<INavigation> entityNavigations = new List<INavigation>();

                foreach (INavigation navigation in entityType.GetNavigations())
                {
                    if (includedNavigations.Add(navigation))
                        entityNavigations.Add(navigation);
                }

                if (entityNavigations.Count == 0)
                {
                    if (stack.Count > 0)
                        yield return string.Join(".", stack.Reverse().Select(e => e.Current.Name));
                }
                else
                {
                    foreach (var navigation in entityNavigations)
                    {
                        var inverseNavigation = navigation.FindInverse();
                        if (inverseNavigation != null)
                            includedNavigations.Add(inverseNavigation);
                    }

                    stack.Push(entityNavigations.GetEnumerator());
                }

                while (stack.Count > 0 && !stack.Peek().MoveNext())
                    stack.Pop();

                if (stack.Count == 0) break;

                entityType = stack.Peek().Current.GetTargetType();
            }
        }
    }
}
