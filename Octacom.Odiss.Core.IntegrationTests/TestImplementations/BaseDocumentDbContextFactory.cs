using System.Data.Entity;
using Octacom.Odiss.Core.DataLayer;

namespace Octacom.Odiss.Core.IntegrationTests.TestImplementations
{
    public class BaseDocumentDbContextFactory : IDbContextFactory
    {
        public DbContext Get()
        {
            return new BaseDocumentDbContext();
        }
    }
}
