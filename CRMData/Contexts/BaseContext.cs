using CRMData.Entities;
using System.Data.Entity;

namespace CRMData.Contexts
{
    public class BaseContext : DbContext
    {
        public BaseContext() 
            : base("CRM_DB")
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Note> Notes { get; set; }

        public DbSet<DAddressType> DAddressTypes { get; set; }
        public DbSet<DPhoneType> DPhoneTypes { get; set; }
    }
}