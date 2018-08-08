namespace CRM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newMigration : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.IgnoreNotifierConfig", "EmailSubject");
            //DropColumn("dbo.IgnoreNotifierConfig", "EmailBody");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.IgnoreNotifierConfig", "EmailBody", c => c.String());
            //AddColumn("dbo.IgnoreNotifierConfig", "EmailSubject", c => c.String());
        }
    }
}
