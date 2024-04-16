using ShopBanDoDienTu_Nhom1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBanDoDienTu_Nhom1.Areas.Manager.Controllers
{
    public class BrandsController : Controller
    {
        // GET: Manager/Brands
        public ActionResult Index()
        {
            MyDBContext db = new MyDBContext();
            List<Brand> brands = db.Brands.ToList();
            return View(brands);
        }

        // GET: Admin/Brands/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Brands/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Brand brand)
        {
            if (ModelState.IsValid)
            {
                using (MyDBContext db = new MyDBContext())
                {
                    db.Brands.Add(brand);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(brand);
        }

        // GET: Admin/Brands/Edit
        public ActionResult Edit(long id)
        {
            using (MyDBContext db = new MyDBContext())
            {
                Brand brand = db.Brands.Find(id);
                if (brand == null)
                {
                    return HttpNotFound();
                }
                return View(brand);
            }
        }

        // POST: Admin/Brands/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Brand brand)
        {
            if (ModelState.IsValid)
            {
                using (MyDBContext db = new MyDBContext())
                {
                    db.Entry(brand).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(brand);
        }
    }
}