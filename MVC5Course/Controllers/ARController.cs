using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;

namespace MVC5Course.Controllers
{
    public class ARController : Controller
    {
        private FabricsEntities db = new FabricsEntities();
        // GET: AR
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FileTest()
        {
            string filepath = Server.MapPath("~/Content/maxresdefault.jpg");
            return File(filepath, "image/jpeg");
        }

        public ActionResult FileTest2()
        {
            string filepath = Server.MapPath("~/Content/maxresdefault.jpg");
            return File(filepath, "image/jpeg","PPAP.jpg");
        }

        public ActionResult JsonTest()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var data = db.Product.OrderBy(p => p.ProductId).Take(10);
            return Json(data,JsonRequestBehavior.AllowGet);
        }

    }
}