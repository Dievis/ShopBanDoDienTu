using System.ComponentModel.DataAnnotations;

namespace ShopBanDoDienTu_Nhom1.Models
{
    public class Brand
    {
        [Key]
        public long BrandId { get; set; }
        public string BrandName { get; set; }
    }
}