namespace CRM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConditionTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conditions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Lead", "IsSaved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lead", "IsSaved");
            DropTable("dbo.Conditions");
        }
    }
}
