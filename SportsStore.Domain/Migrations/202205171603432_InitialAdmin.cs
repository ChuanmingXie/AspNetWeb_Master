namespace SportsStore.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialAdmin : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false, maxLength: 512));
            AlterColumn("dbo.Products", "Category", c => c.String(nullable: false, maxLength: 60));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Category", c => c.String(maxLength: 60));
            AlterColumn("dbo.Products", "Name", c => c.String(maxLength: 512));
        }
    }
}
