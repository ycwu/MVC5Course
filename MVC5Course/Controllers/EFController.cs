using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using System.Data.Entity.Validation;

namespace MVC5Course.Controllers
{
    public class EFController : Controller
    {
        FabricsEntities db = new FabricsEntities();
        // GET: EF
        public ActionResult Index()
        {
            var data = db.Product.Where(p => p.ProductName.Contains("White"));
            return View(data);
        }

        public ActionResult Create()
        {
            var client = new Product()
            {
                ProductName = "White Cat",
                Active = true,
                Price=10,
                Stock=2
            };
            db.Product.Add(client);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            var product = db.Product.Find(id);
            return View(product);
        }

        public ActionResult Delete(int? id)
        {
            var product = db.Product.Find(id);
            db.OrderLine.RemoveRange(product.OrderLine);
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Update(int id)
        {
            var product = db.Product.Find(id);
            product.ProductName = product.ProductName + "!";
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eError in ex.EntityValidationErrors)
                {
                    foreach (var sError in eError.ValidationErrors)
                    {
                        throw new DbEntityValidationException(sError.ErrorMessage + "," + sError.PropertyName);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Add20Percent()
        {
            var data = db.Product.Where(p => p.ProductName.Contains("White"));
            foreach (var item in data)
            {
                if (item.Price.HasValue)
                    item.Price = item.Price * 1.2m;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}