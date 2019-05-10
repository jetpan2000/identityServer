using System.Collections.Generic;

namespace Octacom.Odiss.Core.Contracts.Repositories
{
    public interface ISettingsRepository
    {
        Dictionary<string, object> GetDictionary();
    }
}
