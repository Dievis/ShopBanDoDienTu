namespace ShopBanDoDienTu_Nhom1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removePaymentHistory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaymentHistories", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.PaymentHistories", "OrderDetail_OrderDetailId", "dbo.OrderDetails");
            DropIndex("dbo.PaymentHistories", new[] { "OrderId" });
            DropIndex("dbo.PaymentHistories", new[] { "OrderDetail_OrderDetailId" });
            DropTable("dbo.PaymentHistories");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.PaymentId);
            
            CreateIndex("dbo.PaymentHistories", "OrderDetail_OrderDetailId");
            CreateIndex("dbo.PaymentHistories", "OrderId");
            AddForeignKey("dbo.PaymentHistories", "OrderDetail_OrderDetailId", "dbo.OrderDetails", "OrderDetailId");
            AddForeignKey("dbo.PaymentHistories", "OrderId", "dbo.Orders", "OrderId", cascadeDelete: true);
        }
    }
}
