using CRMData.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CRMData.EntitiesConfiguration
{
    public class DGridConfiguration : EntityTypeConfiguration<DGrid>
    {
        public DGridConfiguration()
        {
            ToTable("DGrid");

            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
