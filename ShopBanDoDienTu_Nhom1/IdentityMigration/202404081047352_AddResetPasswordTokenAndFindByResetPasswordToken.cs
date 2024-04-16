namespace ShopBanDoDienTu_Nhom1.IdentityMigration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResetPasswordTokenAndFindByResetPasswordToken : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ResetPasswordToken", c => c.String());
            AddColumn("dbo.AspNetUsers", "ResetPasswordTokenExpiration", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ResetPasswordTokenExpiration");
            DropColumn("dbo.AspNetUsers", "ResetPasswordToken");
        }
    }
}
