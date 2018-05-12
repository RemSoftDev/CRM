using CRMData.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CRMData.EntitiesConfiguration
{
    public class GridFieldConfiguration : EntityTypeConfiguration<GridField>
    {
        public GridFieldConfiguration()
        {
            ToTable("GridField");

            Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(e => e.GridProfile)
                .WithMany(e => e.GridFields)
                .HasForeignKey(e => e.GridProfileId);
        }
    }
}
