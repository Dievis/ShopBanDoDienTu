using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopBanDoDienTu_Nhom1.ViewModel
{
    public class ShoppingCartVM
    {
        public List<CartItemVM> CartItems { get; set; }
        public decimal CartTotal { get; set; }

        [Display(Name = "Ghi chú")]
        public string Notes { get; set; }

        [Display(Name = "Địa chỉ giao hàng")]
        [Required(ErrorMessage = "Không được để trống")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
    }
}