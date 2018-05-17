namespace CRM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tempMigration : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.LeadConvertedLog", "LeadId");
            CreateIndex("dbo.LeadConvertedLog", "UserId");
            AddForeignKey("dbo.LeadConvertedLog", "LeadId", "dbo.Lead", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LeadConvertedLog", "UserId", "dbo.User", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LeadConvertedLog", "UserId", "dbo.User");
            DropForeignKey("dbo.LeadConvertedLog", "LeadId", "dbo.Lead");
            DropIndex("dbo.LeadConvertedLog", new[] { "UserId" });
            DropIndex("dbo.LeadConvertedLog", new[] { "LeadId" });
        }
    }
}
