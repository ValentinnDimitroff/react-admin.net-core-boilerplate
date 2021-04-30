using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

namespace RaNetCore.Web.BaseControllers.Extensions
{
    public static class RaExtensions
    {
        public static IQueryable<T> RaSort<T>(this IQueryable<T> query, string sort, Func<string, string, IQueryable<T>, IQueryable<T>> customSorting = null)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                List<string> sortVal = JsonConvert.DeserializeObject<List<string>>(sort);
                string condition = sortVal.First();
                string order = sortVal.Last() == "ASC" ? "" : "descending";

                if (customSorting != null)
                {
                    query = customSorting.Invoke(condition, order, query);
                }
                else
                {
                    query = DefaultSorting(condition, order, query);
                }
            }

            return query;
        }

        public static IQueryable<T> RaPaginate<T>(this IQueryable<T> query, string range, HttpResponse response)
        {
            int from = 0, to = 0, count = query.Count();

            // Pagination
            if (!string.IsNullOrEmpty(range))
            {
                var rangeVal = JsonConvert.DeserializeObject<List<int>>(range);
                from = rangeVal.First();
                to = rangeVal.Last();
                query = query.Skip(from).Take(to - from + 1);
            }

            if (response != null)
            {
                response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
                response.Headers.Add("Content-Range", $"{typeof(T).Name.ToLowerInvariant()} {from}-{to}/{count}");
            }

            return query;
        }

        private static IQueryable<T> DefaultSorting<T>(string condition, string order, IQueryable<T> query)
           => query.OrderBy($"{condition} {order}");

    }
}
