using System.Collections.Generic;
using System.Linq;
using Dapper;
using Octacom.Odiss.Core.Contracts.Repositories;

namespace Octacom.Odiss.Core.DataLayer
{
    public class SettingsRepository : ISettingsRepository
    {
        public Dictionary<string, object> GetDictionary()
        {
            using (var db = new MainDatabase().Get)
            {
                var result = db.Query("SELECT * FROM [dbo].[Settings]");

                return result.ToDictionary(x => (string)x.Name, x => x.Value);
            }
        }
    }
}
