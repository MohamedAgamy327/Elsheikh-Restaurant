namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editbill : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bills", "Paid", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bills", "Paid", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
