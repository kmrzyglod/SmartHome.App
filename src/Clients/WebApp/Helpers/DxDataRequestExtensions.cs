using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DevExpress.Blazor;
using DevExtreme.AspNet.Data;
using SmartHome.Application.Shared.Models;
using SmartHome.Clients.WebApp.Services.Logger;

namespace SmartHome.Clients.WebApp.Helpers
{
    public static class DxDataRequestExtensions
    {
        public static T ToApiQuery<T>(this DataSourceLoadOptionsBase dxDataSourceOptions) where T : PageRequest, new()
        {
            var query = new T {PageNumber = (dxDataSourceOptions.Skip / dxDataSourceOptions.Take) + 1, PageSize = dxDataSourceOptions.Take};
           
            var queryProperties = query.GetType().GetProperties();
           
            if (dxDataSourceOptions.Filter == null)
            {
                return query;
            }

            foreach (var filter in dxDataSourceOptions.Filter)
            {
                if (filter is string)
                {
                    continue;
                }

                var filterExpression = filter as IList<object>;
                if (filterExpression == null)
                {
                    continue;
                }

                (string? parameterName, object parameterValue) = (filterExpression.First() as string, filterExpression.Last());
                Logger.LogWarning($"{parameterName} {parameterValue}");
                var queryProperty = queryProperties.FirstOrDefault(x =>
                    string.Equals(parameterName, x.Name, StringComparison.InvariantCultureIgnoreCase));
                
                if (queryProperty == null)
                {
                    continue;
                }

                queryProperty.SetValue(query, parameterValue);
            }

            return query;
        }
    }
}