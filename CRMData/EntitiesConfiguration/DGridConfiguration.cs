using CRM.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CRM.DAL.EntitiesConfiguration
{
    public class DGridConfiguration : EntityTypeConfiguration<DGrid>
    {
        public DGridConfiguration()
        {
            ToTable("DGrid");

            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Type).IsRequired();
        }
    }
}
