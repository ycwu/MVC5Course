using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Net;

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
                ProductName = "sss",
                Active = true,
                Price = 10,
                Stock = 2
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
            product.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Update(int id)
        {
            var product = db.Product.Find(id);
            product.ProductName = product.ProductName + "吳!";
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
            string str = "%white%";
            db.Database.ExecuteSqlCommand("UPDATE dbo.Product SET Price=Price*1.2 WHERE ProductName LIKE @p0", str);
            return RedirectToAction("Index");
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,Price,Active,Stock,IsDeleted")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult ClientContribution()
        {
            var data = db.vw_ClientContribution.Take(10);
            return View(data);
        }

        public ActionResult ClientContribution2(string keyword = "Mary")
        {
            var data = db.Database.SqlQuery<ClientContribution>(@"
	SELECT
		 c.ClientId,
		 c.FirstName,
		 c.LastName,
		 (SELECT SUM(o.OrderTotal) 
		  FROM [dbo].[Order] o 
		  WHERE o.ClientId = c.ClientId) as OrderTotal
	FROM 
		[dbo].[Client] as c
    WHERE c.FirstName LIKE '%'+@p0+'%'
", keyword);
            return View(data);
        }

        public ActionResult ClientContribution3(string keyword = "Mary")
        {
            var data = db.usp_GetClientContribution(keyword);
            return View(data);
        }
    }
}