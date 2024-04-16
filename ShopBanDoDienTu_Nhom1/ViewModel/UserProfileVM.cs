using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ShopBanDoDienTu_Nhom1.ViewModel
{
    public class UserProfileVM
    {
        [Display(Name = "Tài khoản")]
        public string Username { get; set; }

        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        [MaxLength(10, ErrorMessage = "Số điện thoại ít tối đa là 10")]
        public string Mobile { get; set; }

        [Display(Name = "Ngày sinh")]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Địa chỉ")]
        [AllowHtml]
        public string Address { get; set; }

        [Display(Name = "Thành phố")]
        public string City { get; set; }
    }
}
