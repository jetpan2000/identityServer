using Octacom.Odiss.Core.Entities.Application;
using Octacom.Odiss.Core.Entities.Settings;
using System;
using System.Collections.Generic;

namespace Octacom.Odiss.Core.Contracts.Services
{
    [Obsolete("Use Octacom.Odiss.Core.Contracts.Settings instead")]
    public interface IConfigService
    {
        ApplicationSettings GetApplicationSettings();
        IEnumerable<Application> GetApplications();
        string GetDefaultEmail();
    }
}
