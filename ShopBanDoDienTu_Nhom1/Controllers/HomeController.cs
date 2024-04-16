using ShopBanDoDienTu_Nhom1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBanDoDienTu_Nhom1.Controllers
{
    public class HomeController : Controller
    {
        private MyDBContext db = new MyDBContext();
        public ActionResult Index()
        {

            // Truy vấn danh sách các loại mặt hàng từ cơ sở dữ liệu
            var categories = db.Categories.ToList();

            // Truyền danh sách các loại mặt hàng vào ViewBag.Categories
            ViewBag.Categories = categories;
            // Truy vấn danh sách các brand từ cơ sở dữ liệu
            List<Brand> brands = db.Brands.ToList();

            // Set danh sách các brand vào ViewBag.Brands
            ViewBag.Brands = brands;

            // Truy vấn danh sách các sản phẩm từ cơ sở dữ liệu
            List<Product> products = db.Products.ToList();

            // Trả về view Index với danh sách các sản phẩm
            return View(products);
        }

        


        public ActionResult Detail(int id)
        {
            MyDBContext db = new MyDBContext();
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
    }
}