namespace CRM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewLeadColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lead", "IsSaved", e => e.Boolean(nullable: false));
        }
        
        public override void Down()
        {

        }
    }
}
