using System;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ShopBanDoDienTu_Nhom1.Models;
using ShopBanDoDienTu_Nhom1.ViewModel;

namespace ShopBanDoDienTu_Nhom1.Controllers
{
    public class CheckoutController : Controller
    {
        private MyDBContext db = new MyDBContext();

        // GET: /Checkout
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            var viewModel = new ShoppingCartVM
            {
                CartItems = cart.CartItems.Select(item => new CartItemVM
                {
                    ProductId = item.Product.ProductId,
                    ProductName = item.Product.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Product.Price ?? 0m,
                    TotalPrice = (item.Quantity * (item.Product.Price ?? 0m))
                }).ToList(),
                CartTotal = cart.CartItems.Sum(item => item.Product.Price * item.Quantity ?? 0m)
            };

            return View(viewModel);
        }

        public ActionResult CompleteOrder()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteOrder(string email, string shippingAddress, string notes)
        {
            // Lấy giỏ hàng từ Session
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Kiểm tra xem giỏ hàng có mặt hàng không
            if (!cart.CartItems.Any())
            {
                // Trả về một thông báo JavaScript
                TempData["ErrorMessage"] = "Không có mặt hàng nào trong giỏ hàng.";
                return RedirectToAction("Index");
            }
            // Kiểm tra nếu các trường bắt buộc không được nhập
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(shippingAddress))
            {
                // Thêm lỗi vào ModelState
                ModelState.AddModelError("", "Email và Địa chỉ giao hàng là bắt buộc.");

                // Trả về view với ModelState hiện tại
                return View();
            }

            // Tạo một đơn hàng mới
            var order = new Order
            {
                UserId = User.Identity.GetUserId(), // Lấy ID của người dùng đang đăng nhập
                Email = email, // Gán giá trị cho trường email
                OrderDate = DateTime.Now,
                ShippingAddress = shippingAddress,
                Notes = notes,
                TotalPrice = cart.GetTotal() // Tính tổng giá tiền từ giỏ hàng
            };

            // Lưu đơn hàng vào cơ sở dữ liệu
            db.Orders.Add(order);
            db.SaveChanges();

            // Lưu chi tiết đơn hàng
            foreach (var item in cart.CartItems)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = item.Product.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price ?? 0m
                };
                db.OrderDetails.Add(orderDetail);
            }
            db.SaveChanges();

            // Lấy OrderDetailId của chi tiết đơn hàng mới tạo
            long newOrderDetailId = db.OrderDetails.OrderByDescending(od => od.OrderDetailId).FirstOrDefault()?.OrderDetailId ?? 0;

            // Xóa giỏ hàng sau khi đã thanh toán
            cart.ClearCart();

            // Redirect đến action CompleteOrder và truyền OrderId và OrderDetailId
            return RedirectToAction("ViewOrderDetail", new { orderId = order.OrderId, orderDetailId = newOrderDetailId });
        }

        public ActionResult ViewOrderDetail(long orderDetailId)
        {
            // Truy vấn cơ sở dữ liệu để lấy thông tin chi tiết của OrderDetail dựa trên OrderDetailId
            var orderDetail = db.OrderDetails.FirstOrDefault(od => od.OrderDetailId == orderDetailId);

            // Kiểm tra xem OrderDetail có tồn tại không
            if (orderDetail == null)
            {
                // Nếu không tồn tại, chuyển hướng đến trang lỗi hoặc trang không tìm thấy
                return HttpNotFound();
            }

            // Trả về view hiển thị thông tin chi tiết của OrderDetail
            return View(orderDetail);
        }


    }
}
