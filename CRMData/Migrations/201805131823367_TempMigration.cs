namespace CRM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TempMigration : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Addresses", newName: "Address");
            RenameTable(name: "dbo.DAddressTypes", newName: "DAddressType");
            RenameTable(name: "dbo.Users", newName: "User");
            RenameTable(name: "dbo.Emails", newName: "Email");
            RenameTable(name: "dbo.Leads", newName: "Lead");
            RenameTable(name: "dbo.Phones", newName: "Phone");
            RenameTable(name: "dbo.DPhoneTypes", newName: "DPhoneType");
            RenameTable(name: "dbo.Notes", newName: "Note");
            RenameTable(name: "dbo.DUserTypes", newName: "DUserType");
            RenameTable(name: "dbo.LeadConvertedLogs", newName: "LeadConvertedLog");
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Call", "UserId", "dbo.User");
            DropForeignKey("dbo.Call", "LeadId", "dbo.Lead");
            DropIndex("dbo.Call", new[] { "LeadId" });
            DropIndex("dbo.Call", new[] { "UserId" });
            DropTable("dbo.Call");
            RenameTable(name: "dbo.LeadConvertedLog", newName: "LeadConvertedLogs");
            RenameTable(name: "dbo.DUserType", newName: "DUserTypes");
            RenameTable(name: "dbo.Note", newName: "Notes");
            RenameTable(name: "dbo.DPhoneType", newName: "DPhoneTypes");
            RenameTable(name: "dbo.Phone", newName: "Phones");
            RenameTable(name: "dbo.Lead", newName: "Leads");
            RenameTable(name: "dbo.Email", newName: "Emails");
            RenameTable(name: "dbo.User", newName: "Users");
            RenameTable(name: "dbo.DAddressType", newName: "DAddressTypes");
            RenameTable(name: "dbo.Address", newName: "Addresses");
        }
    }
}
