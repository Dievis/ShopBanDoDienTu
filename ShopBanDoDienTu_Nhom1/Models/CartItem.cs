using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopBanDoDienTu_Nhom1.Models
{
    public class CartItem
    {
        [Key]
        public long CartItemId { get; set; }

        // Sửa đổi kiểu dữ liệu của ProductId để khớp với kiểu dữ liệu của ProductId trên entity Product
        [ForeignKey("Product")]
        public long ProductId { get; set; } // Sửa đổi kiểu dữ liệu từ int thành long
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }

        // Thêm trường khóa ngoại để liên kết với ShoppingCart
        [ForeignKey("ShoppingCart")]
        public long ShoppingCartId { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}
