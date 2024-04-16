using System;
using System.ComponentModel.DataAnnotations;

namespace ShopBanDoDienTu_Nhom1.Models
{
    public class Product
    {
        [Key]
        public long ProductId { get; set; }

        [Display(Name = "Tên mặt hàng")]
        [Required(ErrorMessage = "Không được để trống")]
        [RegularExpression(@"^[A-Za-z 0-9]*$", ErrorMessage = "Không được nhập kí tự đặc biệt.")]
        [MinLength(2, ErrorMessage = "Tên mặt hàng ít nhất 2 kí tự.")]
        public string ProductName { get; set; }

        [Display(Name = "Giá")]
        [Required(ErrorMessage = "Không được để trống")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Range(0,100000, ErrorMessage = "Giá tiền nên trong khoảng 0 đến 100.000.00")]
        public Nullable<decimal> Price { get; set; }

        [Display(Name = "Ngày nhập")]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}")]
        public Nullable<System.DateTime> DateOfPurchase { get; set; }

        [Display(Name = "Tình trạng sẵn có")]
        public string AvailabilityStatus { get; set; }

        [Display(Name = "Loại mặt hàng")]
        [Required(ErrorMessage = "Yêu cầu chọn loại mặt hàng")]
        public Nullable<long> CategoryId { get; set; }

        [Display(Name = "Thương hiệu")]
        [Required(ErrorMessage = "Yêu cầu chọn thương hiệu")]
        public Nullable<long> BrandId { get; set; }

        [Display(Name = "Hoạt động")]
        public Nullable<bool> Active { get; set; }

        [Display(Name = "Ảnh mặt hàng")]
        public string Img { get; set; }        

        public virtual Brand Brand { get; set; }

        public virtual Category Category { get; set; } 

    }
}