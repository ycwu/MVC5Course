﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using MVC5Course.Models.ViewModels;

namespace MVC5Course.Controllers
{
    [Authorize]
    //[Authorize(Users = "admin,manager")]
    //[RequireHttps]
    public class ClientsController : BaseController
    {
        //private FabricsEntities db = new FabricsEntities();

        //[OutputCache(Duration = 30, Location = System.Web.UI.OutputCacheLocation.Server)]
        // GET: Clients
        public ActionResult Index(string search, int? CreditRating, string Gender)
        {
            var client = db.Client.Include(c => c.Occupation);
            client = client.OrderByDescending(p => p.ClientId).Take(10);

            if (!string.IsNullOrEmpty(search))
                client = client.Where(p => p.FirstName.Contains(search));
            int iTryParse;
            if (!string.IsNullOrEmpty(CreditRating.ToString()))
                client = client.Where(p => p.CreditRating== CreditRating);
            if (!string.IsNullOrEmpty(Gender))
                client = client.Where(p => p.Gender == Gender);

            var option = (from p in db.Client select p.CreditRating).Distinct().OrderBy(p => p).ToList();
            ViewBag.CreditRating = new SelectList(option);
            ViewBag.Gender = new SelectList(new string[] { "M", "F" });

            return View(client.ToList());
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        [ChildActionOnly]
        public ActionResult Create()
        {
            Client client = new Client();
            client.Gender = "M";
            ViewBag.OccupationId = new SelectList(db.Occupation, "OccupationId", "OccupationName");
            return View(client);
        }

        // POST: Clients/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClientId,FirstName,MiddleName,LastName,Gender,DateOfBirth,CreditRating,XCode,OccupationId,TelephoneNumber,Street1,Street2,City,ZipCode,Longitude,Latitude,Notes")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Client.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OccupationId = new SelectList(db.Occupation, "OccupationId", "OccupationName", client.OccupationId);
            return View(client);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            ViewBag.OccupationId = new SelectList(db.Occupation, "OccupationId", "OccupationName", client.OccupationId);
            return View(client);
        }

        // POST: Clients/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ClientId,FirstName,MiddleName,LastName,Gender,DateOfBirth,CreditRating,XCode,OccupationId,TelephoneNumber,Street1,Street2,City,ZipCode,Longitude,Latitude,Notes")] Client client)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(client).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.OccupationId = new SelectList(db.Occupation, "OccupationId", "OccupationName", client.OccupationId);
        //    return View(client);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection form)
        {
            var client = db.Client.Find(id);
            if (TryUpdateModel(client, null, null, new string[] { "IsAdmin" }))
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OccupationId = new SelectList(db.Occupation, "OccupationId", "OccupationName", client.OccupationId);
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Client.Find(id);
            db.Client.Remove(client);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(ClientLoginViewModel client)
        {
            return View("LoginResult", client);
        }
    }
}
