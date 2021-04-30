using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using RaNetCore.Common.Extensions;

namespace RaNetCore.Services.BaseServices.Helpers
{
    public static class FilterHelpers
    {
        public static (string, JToken) ParseFilterOption(KeyValuePair<string, JToken> filterRow)
        {
            string name = filterRow.Key.ToPascalCase();
            JToken value = filterRow.Value;

            return (name, value);
        }
    }
}
