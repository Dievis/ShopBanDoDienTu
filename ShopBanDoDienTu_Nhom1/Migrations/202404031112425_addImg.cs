namespace ShopBanDoDienTu_Nhom1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addImg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Img", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Img");
        }
    }
}
