namespace ShopBanDoDienTu_Nhom1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPaymentHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentHistories",
                c => new
                    {
                        PaymentId = c.Int(nullable: false, identity: true),
                        OrderId = c.Long(nullable: false),
                        PaymentDate = c.DateTime(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OrderDetail_OrderDetailId = c.Long(),
                    })
                .PrimaryKey(t => t.PaymentId)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.OrderDetails", t => t.OrderDetail_OrderDetailId)
                .Index(t => t.OrderId)
                .Index(t => t.OrderDetail_OrderDetailId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentHistories", "OrderDetail_OrderDetailId", "dbo.OrderDetails");
            DropForeignKey("dbo.PaymentHistories", "OrderId", "dbo.Orders");
            DropIndex("dbo.PaymentHistories", new[] { "OrderDetail_OrderDetailId" });
            DropIndex("dbo.PaymentHistories", new[] { "OrderId" });
            DropTable("dbo.PaymentHistories");
        }
    }
}
