namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updataorder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Order", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "Order");
        }
    }
}
