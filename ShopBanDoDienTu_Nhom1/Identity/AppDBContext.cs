using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using ShopBanDoDienTu_Nhom1.Models;

namespace ShopBanDoDienTu_Nhom1.Identity
{
    public class AppDBContext : IdentityDbContext<AppUser>
    {
        // Thêm DbSet cho thực thể Order
        public AppDBContext() : base("MyConnectionString") { }
    }
}