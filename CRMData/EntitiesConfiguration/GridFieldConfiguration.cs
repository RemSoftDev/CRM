using CRM.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CRM.DAL.EntitiesConfiguration
{
    public class GridFieldConfiguration : EntityTypeConfiguration<GridField>
    {
        public GridFieldConfiguration()
        {
            ToTable("GridField");

            Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.ColumnName).IsRequired();
            Property(e => e.Order).IsRequired();
            Property(e => e.GridProfileId).IsRequired();

            HasRequired(e => e.GridProfile)
                .WithMany(e => e.GridFields)
                .HasForeignKey(e => e.GridProfileId);
        }
    }
}
