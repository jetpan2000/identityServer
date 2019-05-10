using Octacom.Odiss.Core.DataLayer.EntityTypeMappings;
using System.Data.Entity;

namespace Octacom.Odiss.Core.IntegrationTests.TestImplementations
{
    public class ABCGroupDocumentDbContext : DbContext
    {
        public ABCGroupDocumentDbContext() : base("main")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ABCGroupDocumentEntityConfiguration());
        }
    }

    public class ABCGroupDocumentEntityConfiguration : AbstractDocumentTypeMapping<ABCGroupDocument>
    {
        public ABCGroupDocumentEntityConfiguration() : base()
        {
            Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("tblGroup");
            });
        }
    }
}
