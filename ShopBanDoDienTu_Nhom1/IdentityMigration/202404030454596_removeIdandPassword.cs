namespace ShopBanDoDienTu_Nhom1.IdentityMigration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeIdandPassword : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "UserId");
            DropColumn("dbo.AspNetUsers", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Password", c => c.String());
            AddColumn("dbo.AspNetUsers", "UserId", c => c.String());
        }
    }
}
