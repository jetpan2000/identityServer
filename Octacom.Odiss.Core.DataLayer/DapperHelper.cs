using System;
using System.Linq;
using Dapper;
using Octacom.DapperRepository;
using Octacom.Odiss.Core.Contracts.Repositories.Searching;

namespace Octacom.Odiss.Core.DataLayer
{
    internal static class DapperHelper
    {
        internal static SearchResult<TEntity> SearchDapper<TEntity, TSearchParameters, TDatabase>(TSearchParameters parameters, string fullTableName, TDatabase database)
            where TSearchParameters : SearchParameters
            where TDatabase : Database
        {
            var parametersType = parameters.GetType();
            var searchParameterFields = parametersType
                .GetProperties()
                .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(SearchFilter<>))
                .Select(p =>
                {
                    var searchFilter = p.GetValue(parameters);
                    var searchFilterType = searchFilter.GetType();

                    return new
                    {
                        key = p.Name,
                        value = new
                        {
                            value = searchFilterType.GetProperty("Value").GetValue(searchFilter),
                            filterType = (FilterType) searchFilterType.GetProperty("FilterType").GetValue(searchFilter),
                            sortOrder = (SortOrder)searchFilterType.GetProperty("SortOrder").GetValue(searchFilter)
                        }
                    };
                })
                .Where(x => x.value.value != null || x.value.sortOrder != SortOrder.None)
                .ToDictionary(x => x.key, x => x.value);

            var searchParameters = searchParameterFields.ToDictionary(x => x.Key, x => x.Value.value);

            var searchPartSplit = searchParameterFields
                .Where(x => x.Value.value != null)
                .Select(x => $"{x.Key} {x.Value.filterType.ToComparator(x.Key)}");

            var where = searchParameters.Any()
                ? $"WHERE {string.Join(" AND ", searchPartSplit)}"
                : null;

            string sortOrderText(SortOrder sortOrder) => sortOrder == SortOrder.Ascending ? "ASC" : "DESC";

            var orderByParts = searchParameterFields
                .Where(x => x.Value.sortOrder != SortOrder.None)
                .Select(x => $"{x.Key} {sortOrderText(x.Value.sortOrder)}");

            var orderBy = orderByParts.Any()
                ? $"ORDER BY { string.Join(", ", orderByParts) }"
                : null;

            string paginator = null;

            if (orderBy != null)
            {
                searchParameters.Add("__Starting__", (parameters.Page - 1) * parameters.PageSize);
                searchParameters.Add("__FetchNext__", parameters.PageSize);

                paginator = "OFFSET @__Starting__ ROWS FETCH NEXT @__FetchNext__ ROWS ONLY";
            }

            string query = $"SELECT * FROM {fullTableName} {where} {orderBy} {paginator}";

            using (var db = database.Get)
            {
                var records = db.Query<TEntity>(query, searchParameters).ToList();
                var totalCount = db.ExecuteScalar<int>($"SELECT COUNT(*) FROM {fullTableName}");
                var filterCount = db.ExecuteScalar<int>($"SELECT COUNT(*) FROM {fullTableName} {where}", searchParameters);

                return new SearchResult<TEntity>
                {
                    Records = records,
                    TotalCount = totalCount,
                    FilteredCount = filterCount
                };
            }
        }

        private static string ToComparator(this FilterType filterType, string parameter)
        {
            switch (filterType)
            {
                case FilterType.Like:
                    return $"LIKE '%' + @{parameter} + '%'";
                case FilterType.StartsWith:
                    return $"LIKE @{parameter} + '%'";
                case FilterType.EndsWith:
                    return $"LIKE '%' + @{parameter}";
                case FilterType.LessThan:
                    return $"< @{parameter}";
                case FilterType.LessThanOrEqual:
                    return $"<= @{parameter}";
                case FilterType.GreaterThan:
                    return $"> @{parameter}";
                case FilterType.GreaterThanOrEqual:
                    return $">= @{parameter}";
                case FilterType.IsNull:
                    return $"IS NULL";
                case FilterType.IsNotNull:
                    return $"IS NOT NULL";
                case FilterType.Equals:
                default: return $"= @{parameter}";
            }
        }
    }
}
