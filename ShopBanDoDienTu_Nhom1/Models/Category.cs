using System.ComponentModel.DataAnnotations;

namespace ShopBanDoDienTu_Nhom1.Models
{
    public class Category
    {
        [Key]
        public long CategoryId { get; set; }
        [Display(Name = "Tên loại")]
        public string CategoryName { get; set; }
    }
}