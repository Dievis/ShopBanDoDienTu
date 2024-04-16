using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBanDoDienTu_Nhom1.ViewModel
{
    public class CartItemVM
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}