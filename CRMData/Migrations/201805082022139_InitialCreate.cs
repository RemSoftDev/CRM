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
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DAddressTypes", t => t.DAddressTypeId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.DAddressTypeId)
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
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Email = c.String(),
                        Role = c.Int(nullable: false),
                        Password = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        UserTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DUserTypes", t => t.UserTypeId, cascadeDelete: true)
                .Index(t => t.UserTypeId);
            
            CreateTable(
                "dbo.Emails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Subject = c.String(),
                        SentDate = c.DateTime(nullable: false),
                        WasReceived = c.Boolean(nullable: false),
                        LeadId = c.Int(),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Leads", t => t.LeadId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.LeadId)
                .Index(t => t.UserId);
            
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
                        IsConverted = c.Boolean(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Phones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PhoneNumber = c.String(),
                        TypeId = c.Int(),
                        UserId = c.Int(),
                        LeadId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Leads", t => t.LeadId)
                .ForeignKey("dbo.DPhoneTypes", t => t.TypeId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.TypeId)
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
                "dbo.Notes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        LeadId = c.Int(),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Leads", t => t.LeadId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.LeadId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.DUserTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LeadConvertedLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LeadId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ConvertDateTime = c.DateTime(nullable: false),
                        ConvertedBy_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "UserTypeId", "dbo.DUserTypes");
            DropForeignKey("dbo.Notes", "UserId", "dbo.Users");
            DropForeignKey("dbo.Notes", "LeadId", "dbo.Leads");
            DropForeignKey("dbo.Emails", "UserId", "dbo.Users");
            DropForeignKey("dbo.Leads", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Phones", "UserId", "dbo.Users");
            DropForeignKey("dbo.Phones", "TypeId", "dbo.DPhoneTypes");
            DropForeignKey("dbo.Phones", "LeadId", "dbo.Leads");
            DropForeignKey("dbo.Emails", "LeadId", "dbo.Leads");
            DropForeignKey("dbo.Addresses", "UserId", "dbo.Users");
            DropForeignKey("dbo.Addresses", "DAddressTypeId", "dbo.DAddressTypes");
            DropIndex("dbo.Notes", new[] { "UserId" });
            DropIndex("dbo.Notes", new[] { "LeadId" });
            DropIndex("dbo.Phones", new[] { "LeadId" });
            DropIndex("dbo.Phones", new[] { "UserId" });
            DropIndex("dbo.Phones", new[] { "TypeId" });
            DropIndex("dbo.Leads", new[] { "User_Id" });
            DropIndex("dbo.Emails", new[] { "UserId" });
            DropIndex("dbo.Emails", new[] { "LeadId" });
            DropIndex("dbo.Users", new[] { "UserTypeId" });
            DropIndex("dbo.Addresses", new[] { "UserId" });
            DropIndex("dbo.Addresses", new[] { "DAddressTypeId" });
            DropTable("dbo.LeadConvertedLogs");
            DropTable("dbo.DUserTypes");
            DropTable("dbo.Notes");
            DropTable("dbo.DPhoneTypes");
            DropTable("dbo.Phones");
            DropTable("dbo.Leads");
            DropTable("dbo.Emails");
            DropTable("dbo.Users");
            DropTable("dbo.DAddressTypes");
            DropTable("dbo.Addresses");
        }
    }
}
