using Octacom.Odiss.Core.Entities.Documents;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Octacom.Odiss.Core.DataLayer.EntityTypeMappings
{
    public class DocumentMapping : AbstractDocumentTypeMapping<Document>
    {
        public DocumentMapping() : base()
        {
            Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("tblGroup");
            });
        }
    }

    public class AbstractDocumentTypeMapping<TDocument> : EntityTypeConfiguration<TDocument>
        where TDocument : Document
    {
        public AbstractDocumentTypeMapping()
        {
            this.HasKey(x => x.GUID);
            this.Property(x => x.GUID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(x => x.CaptureDate).HasColumnName("I_CaptureDate");
            this.Property(x => x.DirectoryId).HasColumnName("DirectoryID");
        }
    }
}
