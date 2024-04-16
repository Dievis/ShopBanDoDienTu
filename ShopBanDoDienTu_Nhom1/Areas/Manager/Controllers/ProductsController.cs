using ShopBanDoDienTu_Nhom1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBanDoDienTu_Nhom1.Areas.Manager.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Manager/Products
        private readonly MyDBContext db = new MyDBContext();

        // GET: Products
        public ActionResult Index(string search = "", string sortColumn = "ProductId", string iconClass = "fa-sort-asc", int page = 1)
        {
            //Search
            var products = db.Products.Where(row => row.ProductName.Contains(search));

            //Sort
            switch (sortColumn)
            {
                case "ProductId":
                    products = iconClass == "fa-sort-asc" ? products.OrderBy(row => row.ProductId) : products.OrderByDescending(row => row.ProductId);
                    break;
                case "ProductName":
                    products = iconClass == "fa-sort-asc" ? products.OrderBy(row => row.ProductName) : products.OrderByDescending(row => row.ProductName);
                    break;
                case "Price":
                    products = iconClass == "fa-sort-asc" ? products.OrderBy(row => row.Price) : products.OrderByDescending(row => row.Price);
                    break;
            }

            //Paging
            int pageSize = 5;
            int pageNumber = (page < 1) ? 1 : page;
            int totalRecords = products.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            int skip = (pageNumber - 1) * pageSize;

            var pagedProducts = products.Skip(skip).Take(pageSize).ToList();

            ViewBag.Search = search;
            ViewBag.SortColumn = sortColumn;
            ViewBag.IconClass = iconClass;
            ViewBag.Page = pageNumber;
            ViewBag.TotalPages = totalPages;

            return View(pagedProducts);
        }

        //--------------------------------------Detail------------------------------------//
        public ActionResult Detail(int id)
        {
            var product = db.Products.FirstOrDefault(row => row.ProductId == id);
            return View(product);
        }

        //--------------------------------------Create------------------------------------//
        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.Brands = new SelectList(db.Brands, "BrandId", "BrandName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Brands = db.Brands.ToList();
            return View(product);
        }

        //--------------------------------------Edit------------------------------------//
        public ActionResult Edit(long id)
        {
            var product = db.Products.FirstOrDefault(row => row.ProductId == id);
            ViewBag.Categories = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.Brands = new SelectList(db.Brands, "BrandId", "BrandName");
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = db.Products.Find(product.ProductId);
                if (existingProduct != null)
                {
                    existingProduct.ProductName = product.ProductName;
                    existingProduct.Price = product.Price;
                    existingProduct.DateOfPurchase = product.DateOfPurchase;
                    existingProduct.AvailabilityStatus = product.AvailabilityStatus;
                    existingProduct.CategoryId = product.CategoryId;
                    existingProduct.BrandId = product.BrandId;
                    existingProduct.Active = product.Active;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Brands = db.Brands.ToList();
            return View(product);
        }
    }
}