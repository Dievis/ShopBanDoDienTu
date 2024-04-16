using System.Linq;
using System.Web.Mvc;
using ShopBanDoDienTu_Nhom1.Models;
using ShopBanDoDienTu_Nhom1.ViewModel;

namespace ShopBanDoDienTu_Nhom1.Controllers
{
    public class ShoppingCartController : Controller
    {
        private MyDBContext db = new MyDBContext();

        // GET: /ShoppingCart/
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

        public ActionResult AddToCart(long id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Session["RedirectUrl"] = Url.Action("Index", new { id = id });
                return RedirectToAction("Login", "Account");
            }

            var addedProduct = db.Products.FirstOrDefault(product => product.ProductId == id);

            if (addedProduct == null)
            {
                return HttpNotFound();
            }

            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddItem(addedProduct, 1);

            return RedirectToAction("Index", new { id = id });
        }

        [HttpPost]
        public ActionResult UpdateCartItemQuantity(long id, int quantity)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            var cartItem = cart.CartItems.FirstOrDefault(item => item.Product.ProductId == id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }

            cartItem.Quantity = quantity;
            db.SaveChanges();

            TempData["Message"] = "Số lượng sản phẩm đã được cập nhật.";

            return RedirectToAction("Index");
        }

        public ActionResult ClearCart()
        {
            var cart = ShoppingCart.GetCart(HttpContext);
            cart.ClearCart();

            TempData["Message"] = "Đã loại bỏ tất cả mặt hàng trong giỏ hàng.";

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(long id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            var product = db.Products.FirstOrDefault(item => item.ProductId == id);
            if (product == null)
            {
                return HttpNotFound();
            }

            string productName = product.ProductName;

            cart.RemoveItem(id);

            TempData["Message"] = $"Sản phẩm '{productName}' đã được loại bỏ khỏi giỏ hàng.";

            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.CartItems.Count();

            return PartialView("CartSummary");
        }
    }
}
