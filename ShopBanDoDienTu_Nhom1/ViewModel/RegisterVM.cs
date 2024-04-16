using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShopBanDoDienTu_Nhom1.ViewModel
{
    public class RegisterVM
    {
        [Display(Name = "Tài khoản")]
        [Required(ErrorMessage = "Không được để trống")]
        public string Username { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Không được để trống")]
        public string Password { get; set; }

        [Display(Name = "Mật khẩu xác nhận")]
        [Required(ErrorMessage = "Không được để trống")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không giống mật khẩu")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        [MaxLength(10, ErrorMessage = "Số điện thoại ít tối đa là 10")]
        public string Mobile { get; set; }

        [Display(Name = "Ngày sinh")]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Thành phố")]
        public string City { get; set; }

        [Display(Name = "Lý do")]
        public string Reason { get; set; }
    }
}