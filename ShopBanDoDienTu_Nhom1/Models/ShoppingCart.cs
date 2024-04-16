using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBanDoDienTu_Nhom1.Models
{
    public class ShoppingCart
    {
        private MyDBContext db = new MyDBContext();

        public long ShoppingCartId { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }

        public ShoppingCart()
        {
            CartItems = new HashSet<CartItem>();
        }

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            // Lấy giỏ hàng từ Session
            var cart = (ShoppingCart)context.Session["Cart"];

            // Nếu không có giỏ hàng, tạo mới và lưu vào Session
            if (cart == null)
            {
                cart = new ShoppingCart();
                context.Session["Cart"] = cart;
            }

            return cart;
        }

        public void AddItem(Product product, int quantity)
        {
            var existingItem = CartItems.FirstOrDefault(item => item.Product.ProductId == product.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                CartItems.Add(new CartItem { Product = product, Quantity = quantity });
            }
        }

        public void ClearCart()
        {
            foreach (var cartItem in CartItems.ToList())
            {
                CartItems.Remove(cartItem);
            }

            db.SaveChanges();
        }

        // Thêm phương thức để tính tổng giá trị của giỏ hàng
        public decimal GetTotal()
        {
            return CartItems.Sum(item => (item.Product.Price ?? 0m) * item.Quantity);
        }

        public void RemoveItem(long productId)
        {
            var itemToRemove = CartItems.FirstOrDefault(item => item.Product.ProductId == productId);
            if (itemToRemove != null)
            {
                CartItems.Remove(itemToRemove);
                db.SaveChanges();
            }
        }

        public int GetCount()
        {
            return CartItems.Sum(item => item.Quantity);
        }
    }
}
