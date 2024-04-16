using Microsoft.AspNet.Identity;
using ShopBanDoDienTu_Nhom1.Identity;
using ShopBanDoDienTu_Nhom1.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ShopBanDoDienTu_Nhom1.Areas.Manager.Controllers
{
    public class UsersController : Controller
    {
        // GET: Manager/Users
        public ActionResult Index()
        {
            AppDBContext db = new AppDBContext();
            List<AppUser> users = db.Users.ToList();
            return View(users);
        }

        // GET: Account
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(RegisterVM rvm)
        {
            if (ModelState.IsValid)
            {
                var AppDBContext = new AppDBContext();
                var UserStore = new AppUserStore(AppDBContext);
                var UserManager = new AppUserManager(UserStore);
                var PasswordHash = Crypto.HashPassword(rvm.Password);
                var user = new AppUser()
                {
                    Email = rvm.Email,
                    UserName = rvm.Username,
                    PasswordHash = PasswordHash,
                    City = rvm.City,
                    Birthday = rvm.DateOfBirth,
                    Address = rvm.Address,
                    PhoneNumber = rvm.Mobile
                };
                IdentityResult identityResult = UserManager.Create(user);
                if (identityResult.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Customer");
                    return RedirectToAction("Index", "Users");
                }
            }
            // Đảm bảo rằng ModelState sẽ được cập nhật đúng nếu có lỗi xảy ra
            ModelState.AddModelError("New Error", "Invalid Data");
            return View();
        }
    }
}