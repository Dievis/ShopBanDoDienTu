using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ShopBanDoDienTu_Nhom1.Identity;
using ShopBanDoDienTu_Nhom1.ViewModel;

namespace ShopBanDoDienTu_Nhom1.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        // GET: Admin/Users
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

        // GET: Admin/Users/Delete
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var UserManager = new AppUserManager(new AppUserStore(new AppDBContext()));
            var user = UserManager.FindById(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/DeleteConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var UserManager = new AppUserManager(new AppUserStore(new AppDBContext()));
            var user = UserManager.FindById(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            UserManager.Delete(user);

            return RedirectToAction("Index");
        }

        // GET: Admin/Users/Edit
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var UserManager = new AppUserManager(new AppUserStore(new AppDBContext()));
            var user = UserManager.FindById(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            // Khởi tạo một thể hiện của AppDBContext
            var db = new AppDBContext();

            // Lấy danh sách vai trò hiện tại của người dùng
            var roles = UserManager.GetRoles(user.Id);

            // Lấy danh sách tất cả các vai trò
            var allRoles = new SelectList(db.Roles, "Name", "Name");

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Roles = roles.ToList(),
                AllRoles = allRoles
            };

            return View(model);
        }

        // POST: Admin/Users/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var UserManager = new AppUserManager(new AppUserStore(new AppDBContext()));
                var user = UserManager.FindById(model.Id);

                if (user == null)
                {
                    return HttpNotFound();
                }

                // Thêm vai trò mới cho người dùng
                UserManager.AddToRole(user.Id, model.NewRole);

                // Kiểm tra nếu model.Roles không null trước khi sử dụng
                if (model.Roles != null)
                {
                    // Xóa vai trò cũ của người dùng
                    foreach (var role in model.Roles)
                    {
                        if (role != model.NewRole)
                        {
                            UserManager.RemoveFromRole(user.Id, role);
                        }
                    }
                }

                return RedirectToAction("Index");
            }

            // Nếu ModelState không hợp lệ, quay lại view để hiển thị lỗi
            return View(model);
        }



    }
}