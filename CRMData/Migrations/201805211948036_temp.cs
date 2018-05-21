namespace CRM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class temp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
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
                .ForeignKey("dbo.DAddressType", t => t.DAddressTypeId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.DAddressTypeId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.DAddressType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
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
                .ForeignKey("dbo.DUserType", t => t.UserTypeId, cascadeDelete: true)
                .Index(t => t.UserTypeId);
            
            CreateTable(
                "dbo.Call",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        UserId = c.Int(),
                        LeadId = c.Int(),
                        CallTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lead", t => t.LeadId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.LeadId);
            
            CreateTable(
                "dbo.Lead",
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
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Email",
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
                .ForeignKey("dbo.Lead", t => t.LeadId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.LeadId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Note",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        LeadId = c.Int(),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lead", t => t.LeadId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.LeadId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Phone",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PhoneNumber = c.String(),
                        TypeId = c.Int(),
                        UserId = c.Int(),
                        LeadId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lead", t => t.LeadId)
                .ForeignKey("dbo.DPhoneType", t => t.TypeId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.TypeId)
                .Index(t => t.UserId)
                .Index(t => t.LeadId);
            
            CreateTable(
                "dbo.DPhoneType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GridProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProfileName = c.String(),
                        DGridId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        IsDefault = c.Boolean(nullable: false),
                        SearchValue = c.String(),
                        SearchField = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DGrids", t => t.DGridId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.DGridId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.DGrids",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GridFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ColumnName = c.String(),
                        Order = c.Int(nullable: false),
                        GridOrderDirection = c.Int(nullable: false),
                        GridProfileId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GridProfiles", t => t.GridProfileId, cascadeDelete: true)
                .Index(t => t.GridProfileId);
            
            CreateTable(
                "dbo.DUserType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LeadConvertedLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConvertDateTime = c.DateTime(nullable: false),
                        LeadId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ConvertedBy_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lead", t => t.LeadId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.LeadId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LeadConvertedLog", "UserId", "dbo.User");
            DropForeignKey("dbo.LeadConvertedLog", "LeadId", "dbo.Lead");
            DropForeignKey("dbo.User", "UserTypeId", "dbo.DUserType");
            DropForeignKey("dbo.GridProfiles", "UserId", "dbo.User");
            DropForeignKey("dbo.GridFields", "GridProfileId", "dbo.GridProfiles");
            DropForeignKey("dbo.GridProfiles", "DGridId", "dbo.DGrids");
            DropForeignKey("dbo.Call", "UserId", "dbo.User");
            DropForeignKey("dbo.Lead", "User_Id", "dbo.User");
            DropForeignKey("dbo.Phone", "UserId", "dbo.User");
            DropForeignKey("dbo.Phone", "TypeId", "dbo.DPhoneType");
            DropForeignKey("dbo.Phone", "LeadId", "dbo.Lead");
            DropForeignKey("dbo.Note", "UserId", "dbo.User");
            DropForeignKey("dbo.Note", "LeadId", "dbo.Lead");
            DropForeignKey("dbo.Email", "UserId", "dbo.User");
            DropForeignKey("dbo.Email", "LeadId", "dbo.Lead");
            DropForeignKey("dbo.Call", "LeadId", "dbo.Lead");
            DropForeignKey("dbo.Address", "UserId", "dbo.User");
            DropForeignKey("dbo.Address", "DAddressTypeId", "dbo.DAddressType");
            DropIndex("dbo.LeadConvertedLog", new[] { "UserId" });
            DropIndex("dbo.LeadConvertedLog", new[] { "LeadId" });
            DropIndex("dbo.GridFields", new[] { "GridProfileId" });
            DropIndex("dbo.GridProfiles", new[] { "UserId" });
            DropIndex("dbo.GridProfiles", new[] { "DGridId" });
            DropIndex("dbo.Phone", new[] { "LeadId" });
            DropIndex("dbo.Phone", new[] { "UserId" });
            DropIndex("dbo.Phone", new[] { "TypeId" });
            DropIndex("dbo.Note", new[] { "UserId" });
            DropIndex("dbo.Note", new[] { "LeadId" });
            DropIndex("dbo.Email", new[] { "UserId" });
            DropIndex("dbo.Email", new[] { "LeadId" });
            DropIndex("dbo.Lead", new[] { "User_Id" });
            DropIndex("dbo.Call", new[] { "LeadId" });
            DropIndex("dbo.Call", new[] { "UserId" });
            DropIndex("dbo.User", new[] { "UserTypeId" });
            DropIndex("dbo.Address", new[] { "UserId" });
            DropIndex("dbo.Address", new[] { "DAddressTypeId" });
            DropTable("dbo.LeadConvertedLog");
            DropTable("dbo.DUserType");
            DropTable("dbo.GridFields");
            DropTable("dbo.DGrids");
            DropTable("dbo.GridProfiles");
            DropTable("dbo.DPhoneType");
            DropTable("dbo.Phone");
            DropTable("dbo.Note");
            DropTable("dbo.Email");
            DropTable("dbo.Lead");
            DropTable("dbo.Call");
            DropTable("dbo.User");
            DropTable("dbo.DAddressType");
            DropTable("dbo.Address");
        }
    }
}
