using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

using Newtonsoft.Json.Linq;

namespace RaNetCore.Services.BaseServices.Helpers
{
    public class FilterQueryBuilder<TEntity>
        where TEntity : class
    {
        private const string GenericSearchPropName = "q";

        private IQueryable<TEntity> initialQuery;

        public FilterQueryBuilder(IQueryable<TEntity> initialQuery)
        {
            this.initialQuery = initialQuery;
        }

        public IQueryable<TEntity> Query { get; private set; }

        // Public Methods

        public void PerformGenericSearch(JObject filterJsonDict)
        {
            JToken genericSearchPropValue = filterJsonDict.GetValue(GenericSearchPropName);
            string genericSearchParam = GetStrFromJToken(genericSearchPropValue);

            if (String.IsNullOrEmpty(genericSearchParam))
            {
                return;
            }

            // Searching by all string fields
            List<PropertyInfo> stringTypeProperties = typeof(TEntity)
                .GetProperties()
                .Where(p => p.PropertyType == typeof(string) || p.Name == "Id")
                .ToList();

            stringTypeProperties
              .ForEach(p => this.Query = UniteQueries(
                  entity => (typeof(TEntity)
                                .GetProperty(p.Name)
                                .GetValue(entity) ?? String.Empty)
                                .ToString()
                                .ToLowerInvariant()
                                .Contains(genericSearchParam))
              );

            // Remove handled filter kvp
            filterJsonDict.Remove(GenericSearchPropName);
        }

        public void HandleDefault(string propName, JToken propValue)
        {
            // Get the type of the current property
            Type currentPropType = typeof(TEntity)
                .GetProperty(propName)
                .PropertyType;

            switch (currentPropType)
            {
                case Type strType when strType == typeof(string):
                    this.HandleStringProp(propName, propValue);
                    break;
                case Type intType when intType == typeof(int):
                    this.HandleIntProp<int>(propName, propValue);
                    break;
                case Type intType when intType == typeof(int?):
                    this.HandleIntProp<int?>(propName, propValue);
                    break;
                case Type enumType when (enumType.IsEnum):
                    this.HandleEnumProp(propName, propValue);
                    break;
                case Type strType when strType == typeof(bool):
                    this.HandleBoolProp(propName, propValue);
                    break;
                //case Type collectionType when (collectionType.IsEnumerableType()):
                //filterQueryBuilder.HandleEnumerableProp(propName, propValue);
                default:
                    break;
            }
        }

        public void AttachQueryToBuilder(Func<TEntity, bool> func)
        {
            this.Query = this.AddFilterToQuery(func);
        }

        // Private Methods

        private void HandleEnumProp(string propName, JToken propValue)
        {
            string propValueStr = GetStrFromJToken(propValue);

            this.Query = this.AddFilterToQuery(
                entity => typeof(TEntity)
                            .GetProperty(propName)
                            .GetValue(entity)
                            .ToString()
                            .ToLowerInvariant()
                            .Contains(propValueStr));
        }

        private void HandleIntProp<Т>(string propName, JToken propValue)
        {
            List<Т> ints = new List<Т>();

            switch (propValue.Type)
            {
                case JTokenType.Integer:
                    ints.Add(propValue.Value<Т>());
                    break;
                case JTokenType.Array:
                    ints.AddRange(propValue.Children().Values<Т>());
                    break;
                default:
                    break;
            }

            this.Query = AddFilterToQuery($"@0.Contains({propName})", ints);
        }

        private void HandleStringProp(string propName, JToken propValue)
        {
            this.Query = AddFilterToQuery($"{propName}.Contains(@0)", propValue.Value<string>());
        }

        private void HandleBoolProp(string propName, JToken propValue)
        {
            this.Query = AddFilterToQuery($"{propName} == @0", propValue.Value<bool>());
        }

        private IQueryable<TEntity> UniteQueries(Func<TEntity, bool> func)
        {
            if (this.Query is null)
            {
                // Initilize query first row
                return this.initialQuery
                           .Where(x => func.Invoke(x));
            }

            return this.Query
                       .Union(this.initialQuery
                                  .Where(x => func.Invoke(x))
                        );
        }

        private IQueryable<TEntity> AddFilterToQuery(Func<TEntity, bool> func)
        {
            if (func == null)
            {
                return this.Query;
            }

            if (this.Query is null)
            {
                // Initilize query first row
                return this.initialQuery
                    .Where(x => func.Invoke(x));
            }

            return this.Query.Where(x => func.Invoke(x));
        }

        private IQueryable<TEntity> AddFilterToQuery(string predicate, params object[] paramsArr)
        {
            if (this.Query is null)
            {
                // Initilize query first row
                return this.initialQuery.Where(predicate, paramsArr);
            }

            // Attach next query row 
            return this.Query.Where(predicate, paramsArr);
        }

        private static string GetStrFromJToken(JToken jToken) => jToken?.Value<string>()?.ToLowerInvariant() ?? null;
    }
}
