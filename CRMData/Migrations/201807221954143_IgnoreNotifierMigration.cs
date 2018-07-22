namespace CRM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IgnoreNotifierMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IgnoreNotifierConfig",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstDuration = c.Time(nullable: false, precision: 7),
                        SecondDuration = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IgnoreNotifierWorkDayConfig",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DayOfWeek = c.Int(nullable: false),
                        StartWorkTime = c.Time(nullable: false, precision: 7),
                        EndWorkTime = c.Time(nullable: false, precision: 7),
                        IgnoreNotifierConfig_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IgnoreNotifierConfig", t => t.IgnoreNotifierConfig_Id)
                .Index(t => t.IgnoreNotifierConfig_Id);
            
            AddColumn("dbo.User", "IgnoreNotifierConfig_Id", c => c.Int());
            AddColumn("dbo.User", "IgnoreNotifierConfig_Id1", c => c.Int());
            CreateIndex("dbo.User", "IgnoreNotifierConfig_Id");
            CreateIndex("dbo.User", "IgnoreNotifierConfig_Id1");
            AddForeignKey("dbo.User", "IgnoreNotifierConfig_Id", "dbo.IgnoreNotifierConfig", "Id");
            AddForeignKey("dbo.User", "IgnoreNotifierConfig_Id1", "dbo.IgnoreNotifierConfig", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IgnoreNotifierWorkDayConfig", "IgnoreNotifierConfig_Id", "dbo.IgnoreNotifierConfig");
            DropForeignKey("dbo.User", "IgnoreNotifierConfig_Id1", "dbo.IgnoreNotifierConfig");
            DropForeignKey("dbo.User", "IgnoreNotifierConfig_Id", "dbo.IgnoreNotifierConfig");
            DropIndex("dbo.IgnoreNotifierWorkDayConfig", new[] { "IgnoreNotifierConfig_Id" });
            DropIndex("dbo.User", new[] { "IgnoreNotifierConfig_Id1" });
            DropIndex("dbo.User", new[] { "IgnoreNotifierConfig_Id" });
            DropColumn("dbo.User", "IgnoreNotifierConfig_Id1");
            DropColumn("dbo.User", "IgnoreNotifierConfig_Id");
            DropTable("dbo.IgnoreNotifierWorkDayConfig");
            DropTable("dbo.IgnoreNotifierConfig");
        }
    }
}
