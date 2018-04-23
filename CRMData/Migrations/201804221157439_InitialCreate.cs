namespace CRMData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Line1 = c.String(),
                        Line2 = c.String(),
                        Town = c.String(),
                        County = c.String(),
                        PostCode = c.String(),
                        Country = c.String(),
                        DAddressTypeId = c.Int(),
                        CustomerId = c.Int(),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DAddressTypes", t => t.DAddressTypeId)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.DAddressTypeId)
                .Index(t => t.CustomerId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.DAddressTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Title = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Leads", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Leads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Message = c.String(),
                        Discipline = c.String(),
                        AgeGroup = c.String(),
                        Status = c.String(),
                        StatusNotes = c.String(),
                        IssueRaised = c.String(),
                        LeadOwner = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Phones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PhoneNumber = c.String(),
                        TypeId = c.Int(nullable: false),
                        CustomerId = c.Int(),
                        UserId = c.Int(),
                        LeadId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Leads", t => t.LeadId)
                .ForeignKey("dbo.DPhoneTypes", t => t.TypeId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.TypeId)
                .Index(t => t.CustomerId)
                .Index(t => t.UserId)
                .Index(t => t.LeadId);
            
            CreateTable(
                "dbo.DPhoneTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Role = c.Int(nullable: false),
                        Password = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        LeadId = c.Int(),
                        CustomerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Leads", t => t.LeadId)
                .Index(t => t.LeadId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.LeadConvertedLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LeadId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        ConvertDateTime = c.DateTime(nullable: false),
                        ConvertedBy_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "LeadId", "dbo.Leads");
            DropForeignKey("dbo.Notes", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Customers", "Id", "dbo.Leads");
            DropForeignKey("dbo.Phones", "UserId", "dbo.Users");
            DropForeignKey("dbo.Addresses", "UserId", "dbo.Users");
            DropForeignKey("dbo.Phones", "TypeId", "dbo.DPhoneTypes");
            DropForeignKey("dbo.Phones", "LeadId", "dbo.Leads");
            DropForeignKey("dbo.Phones", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Addresses", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Addresses", "DAddressTypeId", "dbo.DAddressTypes");
            DropIndex("dbo.Notes", new[] { "CustomerId" });
            DropIndex("dbo.Notes", new[] { "LeadId" });
            DropIndex("dbo.Phones", new[] { "LeadId" });
            DropIndex("dbo.Phones", new[] { "UserId" });
            DropIndex("dbo.Phones", new[] { "CustomerId" });
            DropIndex("dbo.Phones", new[] { "TypeId" });
            DropIndex("dbo.Customers", new[] { "Id" });
            DropIndex("dbo.Addresses", new[] { "UserId" });
            DropIndex("dbo.Addresses", new[] { "CustomerId" });
            DropIndex("dbo.Addresses", new[] { "DAddressTypeId" });
            DropTable("dbo.LeadConvertedLogs");
            DropTable("dbo.Notes");
            DropTable("dbo.Users");
            DropTable("dbo.DPhoneTypes");
            DropTable("dbo.Phones");
            DropTable("dbo.Leads");
            DropTable("dbo.Customers");
            DropTable("dbo.DAddressTypes");
            DropTable("dbo.Addresses");
        }
    }
}
