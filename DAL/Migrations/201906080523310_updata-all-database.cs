namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updataalldatabase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BillDevices", "BillID", "dbo.Bills");
            DropForeignKey("dbo.Devices", "BillID", "dbo.Bills");
            DropForeignKey("dbo.BillDevices", "DeviceID", "dbo.Devices");
            DropForeignKey("dbo.Bills", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.ClientMembershipMinutes", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.ClientMemberships", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.ClientMemberships", "MembershipID", "dbo.Memberships");
            DropForeignKey("dbo.Memberships", "DeviceTypeID", "dbo.DeviceTypes");
            DropForeignKey("dbo.ClientMemberships", "UserID", "dbo.Users");
            DropForeignKey("dbo.ClientMembershipMinutes", "DeviceTypeID", "dbo.DeviceTypes");
            DropForeignKey("dbo.Devices", "DeviceTypeID", "dbo.DeviceTypes");
            DropIndex("dbo.Bills", new[] { "ClientID" });
            DropIndex("dbo.BillDevices", new[] { "BillID" });
            DropIndex("dbo.BillDevices", new[] { "DeviceID" });
            DropIndex("dbo.Devices", new[] { "BillID" });
            DropIndex("dbo.Devices", new[] { "DeviceTypeID" });
            DropIndex("dbo.ClientMembershipMinutes", new[] { "ClientID" });
            DropIndex("dbo.ClientMembershipMinutes", new[] { "DeviceTypeID" });
            DropIndex("dbo.ClientMemberships", new[] { "UserID" });
            DropIndex("dbo.ClientMemberships", new[] { "MembershipID" });
            DropIndex("dbo.ClientMemberships", new[] { "ClientID" });
            DropIndex("dbo.Memberships", new[] { "DeviceTypeID" });
            AddColumn("dbo.Bills", "RegistrationDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Bills", "ClientID");
            DropColumn("dbo.Bills", "PlayedMinutes");
            DropColumn("dbo.Bills", "CurrentMembershipMinutes");
            DropColumn("dbo.Bills", "MembershipMinutesPaid");
            DropColumn("dbo.Bills", "MembershipMinutesAfterPaid");
            DropColumn("dbo.Bills", "RemainderMinutes");
            DropColumn("dbo.Bills", "CurrentPoints");
            DropColumn("dbo.Bills", "UsedPoints");
            DropColumn("dbo.Bills", "PointsAfterUsed");
            DropColumn("dbo.Bills", "EarnedPoints");
            DropColumn("dbo.Bills", "CancelReason");
            DropColumn("dbo.Bills", "Minimum");
            DropColumn("dbo.Bills", "DevicesSum");
            DropColumn("dbo.Bills", "ItemsSum");
            DropColumn("dbo.Bills", "Paid");
            DropColumn("dbo.Bills", "Change");
            DropColumn("dbo.Bills", "Discount");
            DropColumn("dbo.Bills", "Ratio");
            DropColumn("dbo.Bills", "TotalAfterDiscount");
            DropColumn("dbo.Bills", "Deleted");
            DropColumn("dbo.Bills", "Canceled");
            DropColumn("dbo.Bills", "StartDate");
            DropColumn("dbo.Bills", "EndDate");
            DropColumn("dbo.Shifts", "Income");
            DropColumn("dbo.Shifts", "TotalMinimum");
            DropColumn("dbo.Shifts", "TotalDevices");
            DropColumn("dbo.Shifts", "TotalDiscount");
            DropTable("dbo.BillDevices");
            DropTable("dbo.Devices");
            DropTable("dbo.DeviceTypes");
            DropTable("dbo.ClientMembershipMinutes");
            DropTable("dbo.Clients");
            DropTable("dbo.ClientMemberships");
            DropTable("dbo.Memberships");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Memberships",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DeviceTypeID = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Minutes = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsAvailable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ClientMemberships",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        MembershipID = c.Int(nullable: false),
                        ClientID = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegistrationDate = c.DateTime(nullable: false),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Points = c.Int(),
                        Code = c.String(),
                        Name = c.String(nullable: false),
                        Telephone = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ClientMembershipMinutes",
                c => new
                    {
                        ClientID = c.Int(nullable: false),
                        DeviceTypeID = c.Int(nullable: false),
                        Minutes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ClientID, t.DeviceTypeID });
            
            CreateTable(
                "dbo.DeviceTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        SingleHourPrice = c.Decimal(precision: 18, scale: 2),
                        SingleMinutePrice = c.Decimal(precision: 18, scale: 2),
                        MultiHourPrice = c.Decimal(precision: 18, scale: 2),
                        MultiMinutePrice = c.Decimal(precision: 18, scale: 2),
                        Birthday = c.Boolean(nullable: false),
                        BirthdayHourPrice = c.Decimal(precision: 18, scale: 2),
                        BirthdayMinutePrice = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BillID = c.Int(),
                        DeviceTypeID = c.Int(nullable: false),
                        IsAvailable = c.Boolean(nullable: false),
                        Case = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.BillDevices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BillID = c.Int(nullable: false),
                        DeviceID = c.Int(nullable: false),
                        MinutePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                        Duration = c.Int(),
                        GameType = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Shifts", "TotalDiscount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Shifts", "TotalDevices", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Shifts", "TotalMinimum", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Shifts", "Income", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Bills", "EndDate", c => c.DateTime());
            AddColumn("dbo.Bills", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Bills", "Canceled", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bills", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bills", "TotalAfterDiscount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Bills", "Ratio", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Bills", "Discount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Bills", "Change", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Bills", "Paid", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Bills", "ItemsSum", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Bills", "DevicesSum", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Bills", "Minimum", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Bills", "CancelReason", c => c.String());
            AddColumn("dbo.Bills", "EarnedPoints", c => c.Int());
            AddColumn("dbo.Bills", "PointsAfterUsed", c => c.Int());
            AddColumn("dbo.Bills", "UsedPoints", c => c.Int());
            AddColumn("dbo.Bills", "CurrentPoints", c => c.Int());
            AddColumn("dbo.Bills", "RemainderMinutes", c => c.Int());
            AddColumn("dbo.Bills", "MembershipMinutesAfterPaid", c => c.Int());
            AddColumn("dbo.Bills", "MembershipMinutesPaid", c => c.Int());
            AddColumn("dbo.Bills", "CurrentMembershipMinutes", c => c.Int());
            AddColumn("dbo.Bills", "PlayedMinutes", c => c.Int());
            AddColumn("dbo.Bills", "ClientID", c => c.Int());
            DropColumn("dbo.Bills", "RegistrationDate");
            CreateIndex("dbo.Memberships", "DeviceTypeID");
            CreateIndex("dbo.ClientMemberships", "ClientID");
            CreateIndex("dbo.ClientMemberships", "MembershipID");
            CreateIndex("dbo.ClientMemberships", "UserID");
            CreateIndex("dbo.ClientMembershipMinutes", "DeviceTypeID");
            CreateIndex("dbo.ClientMembershipMinutes", "ClientID");
            CreateIndex("dbo.Devices", "DeviceTypeID");
            CreateIndex("dbo.Devices", "BillID");
            CreateIndex("dbo.BillDevices", "DeviceID");
            CreateIndex("dbo.BillDevices", "BillID");
            CreateIndex("dbo.Bills", "ClientID");
            AddForeignKey("dbo.Devices", "DeviceTypeID", "dbo.DeviceTypes", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ClientMembershipMinutes", "DeviceTypeID", "dbo.DeviceTypes", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ClientMemberships", "UserID", "dbo.Users", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Memberships", "DeviceTypeID", "dbo.DeviceTypes", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ClientMemberships", "MembershipID", "dbo.Memberships", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ClientMemberships", "ClientID", "dbo.Clients", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ClientMembershipMinutes", "ClientID", "dbo.Clients", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Bills", "ClientID", "dbo.Clients", "ID");
            AddForeignKey("dbo.BillDevices", "DeviceID", "dbo.Devices", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Devices", "BillID", "dbo.Bills", "ID");
            AddForeignKey("dbo.BillDevices", "BillID", "dbo.Bills", "ID", cascadeDelete: true);
        }
    }
}
