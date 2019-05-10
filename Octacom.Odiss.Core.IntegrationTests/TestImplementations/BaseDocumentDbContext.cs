using System.Data.Entity;

namespace Octacom.Odiss.Core.IntegrationTests.TestImplementations
{
    public class BaseDocumentDbContext : DbContext
    {
        public BaseDocumentDbContext() : base("main")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(typeof(DataLayer.EntityTypeMappings.DocumentMapping).Assembly);
        }
    }
}
