using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopBanDoDienTu_Nhom1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Trangchu",
               url: "trang-dang-nhap",
               defaults: new { controller = "Account", action = "Login",},
               namespaces: new[] { "ShopBanDoDienTu_Nhom1.Controllers" }
            );

            routes.MapRoute(
              name: "QuanTri",
              url: "QuanTri",
              defaults: new { controller = "Home", action = "Index", },
              namespaces: new[] { "ShopBanDoDienTu_Nhom1.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "ShopBanDoDienTu_Nhom1.Controllers" }
            );
        }
    }
}
