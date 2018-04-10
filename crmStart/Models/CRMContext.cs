using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace crmStart.Models
{
    public class CRMContext : DbContext
    {
        public CRMContext() : base("CRMContext")
        {

        }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Lead> Leads { get; set; }

        public DbSet<Telephone> Telephones { get; set; }

        public DbSet<User> Users { get; set; }
    }
}