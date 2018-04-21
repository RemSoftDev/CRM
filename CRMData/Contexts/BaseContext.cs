using CRMData.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;

namespace CRMData.Contexts
{
    public class CRMDBInitializer : CreateDatabaseIfNotExists<BaseContext>
    {
        protected override void Seed(BaseContext context)
        {
            context.DPhoneTypes.Add(new DPhoneType() { TypeName = "HomePhone" });
            context.DPhoneTypes.Add(new DPhoneType() { TypeName = "WorkPhone" });
            context.DPhoneTypes.Add(new DPhoneType() { TypeName = "MobilePhone" });
            context.DPhoneTypes.Add(new DPhoneType() { TypeName = "EmergencyContactPhone" });
            context.DPhoneTypes.Add(new DPhoneType() { TypeName = "Fax" });

            context.DAddressTypes.Add(new DAddressType() { TypeName = "BillingAddress" });
            context.DAddressTypes.Add(new DAddressType() { TypeName = "ContactAddress" });
            context.DAddressTypes.Add(new DAddressType() { TypeName = "EmergencyContactAddress" });

            context.SaveChanges();
        }
    }

    public class BaseContext : DbContext
    {
        public BaseContext()
            : base("CRM_DB")
        {
            Database.SetInitializer(new CRMDBInitializer());            
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Lead> Leads { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Phone> Phones { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<LeadConvertedLog> LeadConvertedLogs { get; set; }

        public virtual DbSet<DAddressType> DAddressTypes { get; set; }
        public virtual DbSet<DPhoneType> DPhoneTypes { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

    public class ContextFactory
    {
        private Type _dbContextType;
        private DbContext _dbContext;

        private static ContextFactory _singleContextFactory;
        public static ContextFactory SingleContextFactory
        {
            get
            {
                if (_singleContextFactory == null)
                {
                    _singleContextFactory = new ContextFactory();

                    return _singleContextFactory;
                }
                else
                    return _singleContextFactory;
            }
            set
            { }
        }

        public ContextFactory()
        {
            CreateFakeModelData();
        }

        public TDbContext Get<TDbContext>() where TDbContext : DbContext, new()
        {
            bool useMoqDB = bool.Parse(ConfigurationManager.AppSettings["useMoqDB"]);

            if ((_dbContext == null || _dbContextType != typeof(TDbContext)) || !useMoqDB)
            {
                return new TDbContext();
            }

            return (TDbContext)_dbContext;
        }

        public void Register<TDbContext>(TDbContext dbContext) where TDbContext : DbContext, new()
        {
            _dbContextType = typeof(TDbContext);
            _dbContext = dbContext;
        }

        private void CreateFakeModelData()
        {
            var users = GetUserMock();
            var leads = GetLeadMock();

            var mockContext = new Mock<BaseContext>();
            mockContext.Setup(m => m.Users).Returns(users);
            mockContext.Setup(m => m.Leads).Returns(leads);
            mockContext.Setup(m => m.Set<Lead>()).Returns(leads);

            this.Register(mockContext.Object);
        }


        private DbSet<Lead> GetLeadMock()
        {
            var leads = new List<Lead>() {
                new Lead() { Id = 1, Name = "testLead1", Email = "leademail1@test.com" },
                new Lead() { Id = 2, Name = "testLead2", Email = "leademail2@test.com" }
            };

            var mockLead = GetQueryableMockDbSet(leads);
            mockLead.Setup(m => m.Add(It.IsAny<Lead>())).Callback((Lead person) => leads.Add(person));
            mockLead.Setup(m => m.Remove(It.IsAny<Lead>())).Callback((Lead person) => leads.Remove(person));

            return mockLead.Object;
        }

        private DbSet<User> GetUserMock()
        {
            var users = new List<User>() {
                //"taxcode1"
                new User() { Id = 1, Password = "8Mhv+5D5Ji4qtx4Dv+RNpcjlLGOimaDmIkywKkNRjjE=", FirstName = "firstname1", LastName = "surname1", Email = "qwery1@test.com" },
                new User() { Id = 2, Password = "taxcode2", FirstName = "firstname2", LastName = "surname2", Email = "qwery2@test.com" }
            };

            var mockUser = GetQueryableMockDbSet(users);
            mockUser.Setup(m => m.Add(It.IsAny<User>())).Callback((User person) => users.Add(person));
            mockUser.Setup(m => m.Remove(It.IsAny<User>())).Callback((User person) => users.Remove(person));

            return mockUser.Object;
        }

        private Mock<DbSet<T>> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return mockSet;
        }
    }
}