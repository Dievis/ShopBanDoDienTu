using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopBanDoDienTu_Nhom1.ViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Không được để trống")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        public string Password { get; set; }

        public string captcha { get; set; }
    }
}