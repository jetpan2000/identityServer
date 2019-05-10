using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Dapper;
using Octacom.Odiss.Core.Contracts.Infrastructure;
using Octacom.Odiss.Core.Contracts.Validation;
using Octacom.Odiss.Core.Contracts.Validation.Entities;

namespace Octacom.Odiss.Core.Validation
{
    public class FieldValidationProvider : IFieldValidationProvider
    {
        private readonly ICachingService cachingService;
        private readonly IDbContextFactory<DbContext> dbContextFactory;
        private const string CACHE_KEY_GETRULES = "FileValidationProvider_GetRules";

        public FieldValidationProvider(ICachingService cachingService, IDbContextFactory<DbContext> dbContextFactory)
        {
            this.cachingService = cachingService;
            this.dbContextFactory = dbContextFactory;
        }

        public IEnumerable<ValidationRule> GetRules(string applicationIdentifier)
        {
            return cachingService.GetOrSet($"{CACHE_KEY_GETRULES}_{applicationIdentifier}", () =>
            {
                using (var ctx = dbContextFactory.Create())
                using (var db = new Database(ctx.Database.Connection.ConnectionString).Get)
                {
                    var results = db.Query<FieldResult>(@"
SELECT ID, UniqueName, Name, ValidationRulesJson FROM [dbo].[Fields]
WHERE IDApplication = @applicationIdentifier
", new { applicationIdentifier });

                    var list = new List<ValidationRule>();

                    foreach (var item in results)
                    {
                        string fieldIdentifier = item.UniqueName ?? item.ID.ToString().ToLower();

                        if (string.IsNullOrEmpty(item.ValidationRulesJson))
                        {
                            continue;
                        }

                        if (item.ValidationRules.IsAlpha)
                        {
                            list.Add(new ValidationRule
                            {
                                FieldIdentifier = fieldIdentifier,
                                Method = "IsAlpha",
                                InvalidMessage = $"{item.Name} must be alphanumeric"
                            });
                        }

                        if (item.ValidationRules.IsAlphanumeric)
                        {
                            list.Add(new ValidationRule
                            {
                                FieldIdentifier = fieldIdentifier,
                                Method = "IsAlphanumeric",
                                InvalidMessage = $"{item.Name} must be alphanumeric"
                            });
                        }

                        if (item.ValidationRules.IsRequired)
                        {
                            list.Add(new ValidationRule
                            {
                                FieldIdentifier = fieldIdentifier,
                                Method = "IsRequired",
                                InvalidMessage = $"{item.Name} is required"
                            });
                        }

                        if (item.ValidationRules.MinLength.HasValue)
                        {
                            list.Add(new ValidationRule
                            {
                                FieldIdentifier = fieldIdentifier,
                                Method = "MinLength",
                                InvalidMessage = $"{item.Name} must be at least ${item.ValidationRules.MinLength} characters long.",
                                Arguments = new object[] { item.ValidationRules.MinLength.Value }
                            });
                        }

                        if (item.ValidationRules.MaxLength.HasValue)
                        {
                            list.Add(new ValidationRule
                            {
                                FieldIdentifier = fieldIdentifier,
                                Method = "MaxLength",
                                InvalidMessage = $"{item.Name} must be at most ${item.ValidationRules.MaxLength} characters long.",
                                Arguments = new object[] { item.ValidationRules.MaxLength.Value }
                            });
                        }
                    }

                    return list;
                }
            });
        }
    }
}
