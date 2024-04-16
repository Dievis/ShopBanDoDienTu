using ShopBanDoDienTu_Nhom1.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopBanDoDienTu_Nhom1.Models
{
    public class Order
    {
        [Key]
        public long OrderId { get; set; }
        
        public string UserId { get; set; } // Khóa ngoại tham chiếu đến Id của người dùng

        public string Email { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}")]
        public DateTime OrderDate { get; set; }
        
        public decimal TotalPrice { get; set; }
        
        [Required(ErrorMessage = "Shipping Address is required")]
        public string ShippingAddress { get; set; }
        
        public string Notes { get; set; }

        // Mối quan hệ với bảng người dùng
        [ForeignKey("UserId")]
        public AppUser User { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

    }
}