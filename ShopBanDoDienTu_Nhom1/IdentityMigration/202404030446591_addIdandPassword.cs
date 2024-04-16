namespace ShopBanDoDienTu_Nhom1.IdentityMigration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIdandPassword : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserId", c => c.String());
            AddColumn("dbo.AspNetUsers", "Password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Password");
            DropColumn("dbo.AspNetUsers", "UserId");
        }
    }
}
