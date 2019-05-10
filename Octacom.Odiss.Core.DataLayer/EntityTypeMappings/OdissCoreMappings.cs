using System.Data.Entity.ModelConfiguration;

namespace Octacom.Odiss.Core.DataLayer.EntityTypeMappings
{
    public class DirectoryConfiguration : EntityTypeConfiguration<Core.Entities.Storage.Directory>
    {
        public DirectoryConfiguration()
        {
            this.ToTable("tblDirectory");

            this.Property(m => m.Name).HasColumnName("Directory");
            this.Property(m => m.Id).HasColumnName("DirectoryID");
            this.Property(m => m.LocationId).HasColumnName("LocationID");
        }
    }

    public class LocationConfiguration : EntityTypeConfiguration<Core.Entities.Storage.Location>
    {
        public LocationConfiguration()
        {
            this.ToTable("tblLocation");

            this.Property(m => m.Id).HasColumnName("LocationId");
        }
    }
}
