using CRMData.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CRMData.EntitiesConfiguration
{
    public class GridProfileConfiguration : EntityTypeConfiguration<GridProfile>
    {
        public GridProfileConfiguration()
        {
            ToTable("GridProfile");

            Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(e => e.User)
                .WithMany(e => e.GridProfiles)
                .HasForeignKey(e => e.UserId);

            HasRequired(e => e.DGrid)
                .WithMany(e => e.GridProfiles)
                .HasForeignKey(e => e.DGridId);
        }
    }
}
