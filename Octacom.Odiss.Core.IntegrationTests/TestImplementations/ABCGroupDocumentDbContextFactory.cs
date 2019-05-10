using System.Data.Entity;
using Octacom.Odiss.Core.DataLayer;

namespace Octacom.Odiss.Core.IntegrationTests.TestImplementations
{
    public class ABCGroupDocumentDbContextFactory : IDbContextFactory
    {
        public DbContext Get()
        {
            return new ABCGroupDocumentDbContext();
        }
    }
}
