using CRM.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace CRM.DAL.Contexts
{

	public class BaseContext : DbContext
    {
        public BaseContext()
            : base("CRM_DB")
        {
            Database.SetInitializer(new CRMDBInitializer());            
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Lead> Leads { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Phone> Phones { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<LeadConvertedLog> LeadConvertedLogs { get; set; }
        public virtual DbSet<Call> Calls { get; set; }

        public virtual DbSet<DAddressType> DAddressTypes { get; set; }
        public virtual DbSet<DPhoneType> DPhoneTypes { get; set; }
        public virtual DbSet<DUserType> DUserTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Lead>().ToTable("Lead");
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<Phone>().ToTable("Phone");
            modelBuilder.Entity<Note>().ToTable("Note");
            modelBuilder.Entity<Email>().ToTable("Email");
            modelBuilder.Entity<LeadConvertedLog>().ToTable("LeadConvertedLog");
            modelBuilder.Entity<Call>().ToTable("Call");

            modelBuilder.Entity<DAddressType>().ToTable("DAddressType");
            modelBuilder.Entity<DPhoneType>().ToTable("DPhoneType");
            modelBuilder.Entity<DUserType>().ToTable("DUserType");

            modelBuilder.Entity<Address>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Address>()
                .HasOptional(e => e.AddressType)
                .WithMany(e => e.Addresses)
                .HasForeignKey(e => e.AddressTypeId);

            modelBuilder.Entity<User>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<User>()
                .HasRequired(e => e.UserType)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.UserTypeId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Calls)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Lead>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Lead>()
                .HasMany(e => e.Calls)
                .WithOptional(e => e.Lead)
                .HasForeignKey(e => e.LeadId);

            modelBuilder.Entity<Phone>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Call>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            base.OnModelCreating(modelBuilder);
        }
    }
}