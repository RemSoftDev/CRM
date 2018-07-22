namespace CRM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConditionTable : DbMigration
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Conditions");
        }
    }
}
