using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DocumentEntity = Octacom.Odiss.Core.Contracts.DataLayer.Repository.Document.Document;

namespace Octacom.Odiss.Core.DataLayer.Repository.Document.EF
{
    public class DocumentMapping : AbstractDocumentTypeMapping<DocumentEntity>
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
        where TDocument : DocumentEntity
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
