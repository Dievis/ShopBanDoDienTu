namespace ShopBanDoDienTu_Nhom1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShoppingCartAndCartItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CartItems", "Product_ProductId", "dbo.Products");
            DropIndex("dbo.CartItems", new[] { "Product_ProductId" });
            RenameColumn(table: "dbo.CartItems", name: "Product_ProductId", newName: "ProductId");
            DropPrimaryKey("dbo.CartItems");
            DropPrimaryKey("dbo.ShoppingCarts");
            AddColumn("dbo.CartItems", "ShoppingCartId", c => c.Long(nullable: false));
            AlterColumn("dbo.CartItems", "CartItemId", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.CartItems", "ProductId", c => c.Long(nullable: false));
            AlterColumn("dbo.ShoppingCarts", "ShoppingCartId", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.CartItems", "CartItemId");
            AddPrimaryKey("dbo.ShoppingCarts", "ShoppingCartId");
            CreateIndex("dbo.CartItems", "ProductId");
            CreateIndex("dbo.CartItems", "ShoppingCartId");
            AddForeignKey("dbo.CartItems", "ShoppingCartId", "dbo.ShoppingCarts", "ShoppingCartId", cascadeDelete: true);
            AddForeignKey("dbo.CartItems", "ProductId", "dbo.Products", "ProductId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartItems", "ProductId", "dbo.Products");
            DropForeignKey("dbo.CartItems", "ShoppingCartId", "dbo.ShoppingCarts");
            DropIndex("dbo.CartItems", new[] { "ShoppingCartId" });
            DropIndex("dbo.CartItems", new[] { "ProductId" });
            DropPrimaryKey("dbo.ShoppingCarts");
            DropPrimaryKey("dbo.CartItems");
            AlterColumn("dbo.ShoppingCarts", "ShoppingCartId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.CartItems", "ProductId", c => c.Long());
            AlterColumn("dbo.CartItems", "CartItemId", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.CartItems", "ShoppingCartId");
            AddPrimaryKey("dbo.ShoppingCarts", "ShoppingCartId");
            AddPrimaryKey("dbo.CartItems", "CartItemId");
            RenameColumn(table: "dbo.CartItems", name: "ProductId", newName: "Product_ProductId");
            CreateIndex("dbo.CartItems", "Product_ProductId");
            AddForeignKey("dbo.CartItems", "Product_ProductId", "dbo.Products", "ProductId");
        }
    }
}
