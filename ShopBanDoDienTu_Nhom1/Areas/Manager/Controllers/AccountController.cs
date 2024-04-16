using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopBanDoDienTu_Nhom1.Models;
using ShopBanDoDienTu_Nhom1.ViewModel;
using ShopBanDoDienTu_Nhom1.Identity;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;


namespace ShopBanDoDienTu_Nhom1.Areas.Manager.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterVM rvm)
        {
            if (ModelState.IsValid)
            {
                var AppDBContext = new AppDBContext();
                var UserStore = new AppUserStore(AppDBContext);
                var UserManager = new AppUserManager(UserStore);

                // Kiểm tra xem tên đăng nhập đã tồn tại trong cơ sở dữ liệu chưa
                if (UserManager.FindByName(rvm.Username) != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.");
                    return View();
                }
                else if (UserManager.FindByEmail(rvm.Email) != null)
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại. Vui lòng chọn tên khác.");
                    return View();
                }

                var PasswordHash = Crypto.HashPassword(rvm.Password);

                var user = new AppUser()
                {
                    Email = rvm.Email,
                    UserName = rvm.Username,
                    PasswordHash = PasswordHash,
                };
                IdentityResult identityResult = UserManager.Create(user);
                if (identityResult.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Customer");

                    var authenManager = HttpContext.GetOwinContext().Authentication;
                    var userIdentity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    authenManager.SignIn(new AuthenticationProperties(), userIdentity);

                    var fromAddress = new MailAddress("superchip3010@gmail.com");
                    const string fromPassword = "nhzf ycrh hfcc xnmf";
                    var toAddress = new MailAddress(rvm.Email);
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                        Timeout = 20000
                    };
                    const string subject = "Đăng kí tài khoản ";
                    string body = "<p>Đăng kí thành công</p>";
                    body += $"<p>Đã đăng kí thành công</p>";

                    // Tạo đối tượng MailMessage với thông tin từ, tới, tiêu đề và nội dung của email
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Body = body,
                        Subject = subject,
                        IsBodyHtml = true
                    })
                    {
                        smtp.Send(message);
                    }

                    ViewBag.SuccessMessage = "Một đường link đã được gửi đến email của bạn.";
                    return RedirectToAction("Login", "Account");


                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("New Error", "Invalid Data");
                return View();
            }

        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM lvm)
        {
            var AppDBContext = new AppDBContext();
            var UserStore = new AppUserStore(AppDBContext);
            var UserManager = new AppUserManager(UserStore);
            var user = UserManager.Find(lvm.Username, lvm.Password);
            if (user != null)
            {
                var authenManager = HttpContext.GetOwinContext().Authentication;
                var userIdentity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                authenManager.SignIn(new AuthenticationProperties(), userIdentity);
                if (UserManager.IsInRole(user.Id, "Admin"))
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else if (UserManager.IsInRole(user.Id, "Manager")) 
                {
                    return RedirectToAction("Index", "Home", new { area = "Manager" });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("NewError", "Tài khoản hoặc Mật khẩu sai.");
                return View();
            }
        }

        public ActionResult Logout()
        {
            var authenManager = HttpContext.GetOwinContext().Authentication;
            authenManager.SignOut();

            // Xóa tất cả sản phẩm trong giỏ hàng khi người dùng đăng xuất
            var cart = ShoppingCart.GetCart(HttpContext);
            cart.ClearCart();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult UserProfile()
        {
            var userId = User.Identity.GetUserId();
            var AppDBContext = new AppDBContext();
            var UserStore = new AppUserStore(AppDBContext);
            var UserManager = new AppUserManager(UserStore);
            var user = UserManager.FindById(userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            var userProfile = new UserProfileVM
            {
                Username = user.UserName,
                Email = user.Email,
                City = user.City,
                DateOfBirth = user.Birthday,
                Address = user.Address,
                Mobile = user.PhoneNumber
            };

            return View(userProfile);
        }

        public ActionResult EditUserProfile()
        {
            var userId = User.Identity.GetUserId();
            var AppDBContext = new AppDBContext();
            var UserStore = new AppUserStore(AppDBContext);
            var UserManager = new AppUserManager(UserStore);
            var user = UserManager.FindById(userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            var userProfile = new UserProfileVM
            {
                Username = user.UserName,
                Email = user.Email,
                City = user.City,
                DateOfBirth = user.Birthday,
                Address = user.Address,
                Mobile = user.PhoneNumber
            };

            return View(userProfile);
        }

        [HttpPost]
        public ActionResult EditUserProfile(UserProfileVM pvm)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var AppDBContext = new AppDBContext();
                var UserStore = new AppUserStore(AppDBContext);
                var UserManager = new AppUserManager(UserStore);
                var user = UserManager.FindById(userId);

                if (user == null)
                {
                    return HttpNotFound();
                }

                // Update user profile information
                user.City = pvm.City;
                user.Birthday = pvm.DateOfBirth;
                user.Address = pvm.Address;
                user.PhoneNumber = pvm.Mobile;

                // Save changes to the database
                UserManager.Update(user);

                return RedirectToAction("UserProfile");
            }

            // If model state is not valid, return the view with errors
            return View(pvm);
        }

        //-------------------------------------------------------------------------------------//

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordVM cpvm)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var AppDBContext = new AppDBContext();
                var UserStore = new AppUserStore(AppDBContext);
                var UserManager = new AppUserManager(UserStore);
                var user = UserManager.FindById(userId);

                if (user == null)
                {
                    return HttpNotFound();
                }

                // Kiểm tra mật khẩu cũ
                if (!UserManager.CheckPassword(user, cpvm.OldPassword))
                {
                    ModelState.AddModelError("OldPassword", "Mật khẩu cũ không đúng.");
                    return View(cpvm);
                }

                // Hash mật khẩu mới trước khi lưu vào cơ sở dữ liệu
                var newPasswordHash = UserManager.PasswordHasher.HashPassword(cpvm.NewPassword);

                // Cập nhật mật khẩu mới cho người dùng
                user.PasswordHash = newPasswordHash;
                var result = UserManager.Update(user);

                if (result.Succeeded)
                {
                    // Đăng xuất người dùng để họ phải đăng nhập lại với mật khẩu mới
                    var authenManager = HttpContext.GetOwinContext().Authentication;
                    authenManager.SignOut();

                    // Redirect đến trang đăng nhập với thông báo thành công
                    TempData["SuccessMessage"] = "Đổi mật khẩu thành công. Vui lòng đăng nhập lại.";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    // Nếu có lỗi xảy ra trong quá trình cập nhật mật khẩu
                    ModelState.AddModelError("", "Có lỗi xảy ra khi đổi mật khẩu. Vui lòng thử lại sau.");
                    return View(cpvm);
                }
            }

            // Nếu model state không hợp lệ, trả về view với các lỗi
            return View(cpvm);
        }

        public bool IsEmailExists(string email)
        {
            var AppDBContext = new AppDBContext();
            var UserStore = new AppUserStore(AppDBContext);
            var UserManager = new AppUserManager(UserStore);

            // Kiểm tra xem có người dùng nào sử dụng địa chỉ email đã cho không
            var user = UserManager.FindByEmail(email);

            // Nếu có người dùng sử dụng địa chỉ email này, trả về true, ngược lại trả về false
            return user != null;
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                ViewBag.ErrorMessage = "Please provide an email address.";
                return View();
            }

            if (!IsEmailExists(emailAddress))
            {
                ViewBag.ErrorMessage = "Email does not exist in the database.";
                return View();
            }

            try
            {
                // Tạo mã token để đặt lại mật khẩu và lưu vào cơ sở dữ liệu
                var token = Guid.NewGuid().ToString();
                var AppDBContext = new AppDBContext();
                var UserStore = new AppUserStore(AppDBContext);
                var UserManager = new AppUserManager(UserStore);
                var user = UserManager.FindByEmail(emailAddress);

                if (user == null)
                {
                    ViewBag.ErrorMessage = "Email does not exist in the database.";
                    return View();
                }

                // Gán token cho người dùng
                user.ResetPasswordToken = token;
                user.ResetPasswordTokenExpiration = DateTime.Now.AddHours(1); // Thời gian hết hạn của token (ở đây là 1 giờ, bạn có thể điều chỉnh)

                // Lưu thay đổi vào cơ sở dữ liệu
                UserManager.Update(user);

                // Gửi email chứa liên kết đến trang đặt lại mật khẩu
                var fromAddress = new MailAddress("superchip3010@gmail.com");
                const string fromPassword = "nhzf ycrh hfcc xnmf";
                var toAddress = new MailAddress(emailAddress);
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };
                const string subject = "Reset Password Link";
                string resetPasswordLink = Url.Action("ResetPasswordConfirmation", "Account", new { emailAddress = emailAddress, token = token }, Request.Url.Scheme);
                string body = "<p>You have requested to reset your password.</p>";
                body += $"<p>Please click the following link to reset your password:</p>";
                body += $"<a href='{resetPasswordLink}'>Reset Password</a>";

                // Tạo đối tượng MailMessage với thông tin từ, tới, tiêu đề và nội dung của email
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Body = body,
                    Subject = subject,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }

                ViewBag.SuccessMessage = "Một đường link đã được gửi đến email của bạn.";
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                // Gửi thông báo lỗi nếu có lỗi xảy ra
                ViewBag.ErrorMessage = ex.Message;
            }
            return RedirectToAction("Login", "Account");

        }

        public ActionResult ResetPasswordConfirmation(string token)
        {
            // Kiểm tra xem token có tồn tại trong cơ sở dữ liệu không
            var AppDBContext = new AppDBContext();
            var UserStore = new AppUserStore(AppDBContext);
            var UserManager = new AppUserManager(UserStore);
            var user = UserManager.FindByResetPasswordToken(token);

            if (user == null || user.ResetPasswordTokenExpiration < DateTime.Now)
            {
                ViewBag.ErrorMessage = "Token không hợp lệ hoặc đã hết hạn.";
                return View("Error");
            }

            // Nếu token hợp lệ, hiển thị view để người dùng đặt lại mật khẩu mới
            var rvm = new ResetPasswordVM
            {
                Token = token
            };
            return View(rvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPasswordConfirmation(ResetPasswordVM rvm)
        {
            if (!ModelState.IsValid)
            {
                return View(rvm);
            }

            // Kiểm tra xem token có tồn tại trong cơ sở dữ liệu không
            var AppDBContext = new AppDBContext();
            var UserStore = new AppUserStore(AppDBContext);
            var UserManager = new AppUserManager(UserStore);
            var user = UserManager.FindByResetPasswordToken(rvm.Token);

            if (user == null || user.ResetPasswordTokenExpiration < DateTime.Now)
            {
                ViewBag.ErrorMessage = "Token không hợp lệ hoặc đã hết hạn.";
                return View("Error");
            }
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiration = null;
            // Đặt lại mật khẩu mới cho người dùng
            var newPasswordHash = UserManager.PasswordHasher.HashPassword(rvm.NewPassword);
            
            // Cập nhật mật khẩu mới cho người dùng
            user.PasswordHash = newPasswordHash;
            var result = UserManager.Update(user);

            if (result.Succeeded)
            {
                // Đăng xuất người dùng để họ phải đăng nhập lại với mật khẩu mới
                var authenManager = HttpContext.GetOwinContext().Authentication;
                authenManager.SignOut();

                // Redirect đến trang đăng nhập với thông báo thành công
                TempData["SuccessMessage"] = "Đổi mật khẩu thành công. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // Nếu có lỗi xảy ra trong quá trình đặt lại mật khẩu
                ViewBag.ErrorMessage = "Có một lỗi phát sinh khi đổi mật khẩu. Vui lòng thử lại sau.";
                return View("Error");
            }
        }
    }

}


