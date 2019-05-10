using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Octacom.Odiss.Core.Contracts.DataLayer.Search;
using Octacom.Odiss.Core.Contracts.Settings;
using Octacom.Odiss.Core.Contracts.Settings.Entities;

namespace Octacom.Odiss.Core.DataLayer.Search.EF
{
    /// <summary>
    /// Middleware to perform extra actions on SearchOptions before they are passed to the search engine. For example, to allow for resolving Search Options parameters per Odiss field names
    /// </summary>
    internal class SearchOptionMiddleware
    {
        private readonly IApplicationService applicationService;
        private readonly SearchEngineRegistry searchEngineRegistry;
        private readonly Type type;

        public SearchOptionMiddleware(IApplicationService applicationService, SearchEngineRegistry searchEngineRegistry, Type type)
        {
            this.applicationService = applicationService;
            this.searchEngineRegistry = searchEngineRegistry;
            this.type = type;
        }

        public void Execute(SearchOptions searchOptions)
        {
            ExecuteOdissSettings(searchOptions);
        }

        /// <summary>
        /// For Search Parameters which match a field name of the type's application perform substitution of the Search Parameter name using the field's Search Options
        /// </summary>
        private void ExecuteOdissSettings(SearchOptions searchOptions)
        {
            var application = GetApplication(searchOptions);

            if (application == null || application.SearchFields == null || searchOptions.SearchParameters == null)
            {
                return;
            }

            var searchableFields = new List<ISearchableField>();
            searchableFields.AddRange(application.SearchFields);
            searchableFields.AddRange(application.LookupPropertyFields.Where(field => !searchableFields.Any(existing => existing.Identifier == field.Identifier)));

            var crossProduct = (from parameter in searchOptions.SearchParameters
                                from field in searchableFields
                                where parameter.Key == field.Identifier
                                select new { key = parameter.Key, parameter, field }).ToList();

            foreach (var item in crossProduct)
            {
                searchOptions.SearchParameters.Add(GetSearchParameterKey(item.field), item.parameter.Value);
                searchOptions.SearchParameters.Remove(item.key);
            }
        }

        private Application GetApplication(SearchOptions searchOptions)
        {
            if (searchOptions is GlobalSearchOptions)
            {
                var globalSearchOptions = (GlobalSearchOptions) searchOptions;

                if (!string.IsNullOrEmpty(globalSearchOptions.CallingApplicationIdentifier))
                {
                    return applicationService.Get(globalSearchOptions.CallingApplicationIdentifier);
                }
            }

            return applicationService.Get(this.type);
        }

        private string GetSearchParameterKey(ISearchableField field)
        {
            var searchConfiguration = field.SearchConfiguration;

            if (searchConfiguration == null)
            {
                return field.MapTo; // Allowed until Odiss 6
            }

            if (string.IsNullOrEmpty(searchConfiguration.EntityName))
            {
                return searchConfiguration.SearchFields;
            }

            var searchEntityType = this.searchEngineRegistry.GetEntityType(searchConfiguration.EntityName);

            return BuildNavigationParameterForField(searchConfiguration.SearchFields, searchEntityType, this.type);
        }

        private static string BuildNavigationParameterForField(string searchFieldsString, Type searchEntityType, Type engineEntity)
        {
            if (searchEntityType == engineEntity)
            {
                return searchFieldsString;
            }

            var properties = engineEntity.GetProperties();
            var navigationProperty = properties.FirstOrDefault(x => x.Name == searchEntityType.Name);

            string logicalSeparatorPattern = @"(?:^|[ ])(.*?)($| AND| OR)";

            return Regex.Replace(searchFieldsString, logicalSeparatorPattern, delegate (Match match)
            {
                string suffix = match.Groups[2].Value;
                if (suffix.Length > 0)
                {
                    suffix += " ";
                }

                var value = match.Groups[1].Value + suffix;

                if (navigationProperty != null)
                {
                    return $"{navigationProperty.Name}.{value}";
                }
                else
                {
                    var dotSplit = value.Split('.'); // TODO - Modify the regular expression to use a capture group for this instead (quick hack for now)
                    string matchPropertyName = $"{searchEntityType.Name}{dotSplit[0]}";
                    string afterPropertyName = string.Join("", dotSplit.Skip(1));

                    if (afterPropertyName.Length > 0)
                    {
                        afterPropertyName = "." + afterPropertyName;
                    }

                    var matchProperty = properties.FirstOrDefault(x => x.Name == matchPropertyName);

                    if (matchProperty == null)
                    {
                        return null; // Can't use this property as it doesn't exist so we avoid searching for it
                    }
                    else
                    {
                        return $"{matchPropertyName}{afterPropertyName}";
                    }
                }
            });
        }
    }
}