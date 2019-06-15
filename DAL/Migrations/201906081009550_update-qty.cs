namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateqty : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BillItems", "Qty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BillItems", "Qty", c => c.Int(nullable: false));
        }
    }
}
