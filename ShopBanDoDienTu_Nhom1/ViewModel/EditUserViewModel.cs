using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBanDoDienTu_Nhom1.ViewModel
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public SelectList AllRoles { get; set; }
        public string NewRole { get; set; }
    }
}
