﻿using CRMData.Entities;
using System.Data.Entity;

namespace CRMData.Contexts
{
    public class BaseContext : DbContext
    {
        public BaseContext() 
            : base("CRM_DB")
        { }

        public DbSet<User> Users { get; set; }
    }
}