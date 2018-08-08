namespace CRM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newMigration1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IgnoreNotifierConfig", "EmailSubject", c => c.String());
            AddColumn("dbo.IgnoreNotifierConfig", "EmailBody", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.IgnoreNotifierConfig", "EmailBody");
            DropColumn("dbo.IgnoreNotifierConfig", "EmailSubject");
        }
    }
}
