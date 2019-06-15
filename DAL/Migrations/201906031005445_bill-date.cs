namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class billdate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BillItems", "RegistrationDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BillItems", "RegistrationDate", c => c.DateTime(nullable: false));
        }
    }
}
