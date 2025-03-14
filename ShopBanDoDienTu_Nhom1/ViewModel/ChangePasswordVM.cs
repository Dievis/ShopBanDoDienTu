﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace ShopBanDoDienTu_Nhom1.ViewModel
{
    public class ChangePasswordVM
    {
        [Required (ErrorMessage = "Yêu cầu nhập mật khẩu cũ")]
        [Display(Name = "Mật khẩu")]
        public string OldPassword { get; set; }

        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [Display(Name = "Mật khẩu xác nhận")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không giống mật khẩu mới")]
        public string ConfirmPassword { get; set; }
    }
}