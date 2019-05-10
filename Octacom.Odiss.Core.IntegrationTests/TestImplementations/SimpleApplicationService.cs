using Octacom.Odiss.Core.Contracts.Services;

namespace Octacom.Odiss.Core.IntegrationTests.TestImplementations
{
    public class SimpleApplicationService : IApplicationService
    {
        public string GetBaseUrl()
        {
            return "http://localhost:3950/";
        }
    }
}
