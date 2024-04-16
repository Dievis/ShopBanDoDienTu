namespace ShopBanDoDienTu_Nhom1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderDetails", "Product_ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.OrderDetails", new[] { "Product_ProductId" });
            DropColumn("dbo.OrderDetails", "ProductId");
            RenameColumn(table: "dbo.OrderDetails", name: "Product_ProductId", newName: "ProductId");
            DropPrimaryKey("dbo.OrderDetails");
            DropPrimaryKey("dbo.Orders");
            AlterColumn("dbo.OrderDetails", "OrderDetailId", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.OrderDetails", "OrderId", c => c.Long(nullable: false));
            AlterColumn("dbo.OrderDetails", "ProductId", c => c.Long(nullable: false));
            AlterColumn("dbo.OrderDetails", "ProductId", c => c.Long(nullable: false));
            AlterColumn("dbo.Orders", "OrderId", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.OrderDetails", "OrderDetailId");
            AddPrimaryKey("dbo.Orders", "OrderId");
            CreateIndex("dbo.OrderDetails", "OrderId");
            CreateIndex("dbo.OrderDetails", "ProductId");
            AddForeignKey("dbo.OrderDetails", "ProductId", "dbo.Products", "ProductId", cascadeDelete: true);
            AddForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders", "OrderId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderDetails", "ProductId", "dbo.Products");
            DropIndex("dbo.OrderDetails", new[] { "ProductId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropPrimaryKey("dbo.Orders");
            DropPrimaryKey("dbo.OrderDetails");
            AlterColumn("dbo.Orders", "OrderId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.OrderDetails", "ProductId", c => c.Long());
            AlterColumn("dbo.OrderDetails", "ProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.OrderDetails", "OrderId", c => c.Int(nullable: false));
            AlterColumn("dbo.OrderDetails", "OrderDetailId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Orders", "OrderId");
            AddPrimaryKey("dbo.OrderDetails", "OrderDetailId");
            RenameColumn(table: "dbo.OrderDetails", name: "ProductId", newName: "Product_ProductId");
            AddColumn("dbo.OrderDetails", "ProductId", c => c.Int(nullable: false));
            CreateIndex("dbo.OrderDetails", "Product_ProductId");
            CreateIndex("dbo.OrderDetails", "OrderId");
            AddForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders", "OrderId", cascadeDelete: true);
            AddForeignKey("dbo.OrderDetails", "Product_ProductId", "dbo.Products", "ProductId");
        }
    }
}
