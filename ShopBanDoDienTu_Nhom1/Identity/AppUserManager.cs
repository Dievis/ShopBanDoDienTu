using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ShopBanDoDienTu_Nhom1.Identity
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store) : base(store) { }

        public async Task<AppUser> FindByResetPasswordTokenAsync(string resetPasswordToken)
        {
            // Thực hiện truy vấn để tìm người dùng với token đặt lại mật khẩu tương ứng
            var user = await Users.FirstOrDefaultAsync(u => u.ResetPasswordToken == resetPasswordToken);
            return user;
        }

        public AppUser FindByResetPasswordToken(string resetPasswordToken)
        {
            // Thực hiện truy vấn để tìm người dùng với token đặt lại mật khẩu tương ứng
            var user = Users.FirstOrDefault(u => u.ResetPasswordToken == resetPasswordToken);
            return user;
        }
    }
}