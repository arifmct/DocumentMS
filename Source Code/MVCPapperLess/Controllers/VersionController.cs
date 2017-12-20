using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCPapperLess.Models;

namespace MVCPapperLess.Controllers
{
    public class VersionController : Controller
    {
        private dbPaperLessEntities db = new dbPaperLessEntities();

        // GET: Version
        public ActionResult Index()
        {
            if (Session["login"].ToString() == "")
            {
                Session["msg"] = "You have to login to view these content";
                return RedirectToAction("Login", "Employee");
            }
            var versions = db.Versions.Include(v => v.Document);
            return View(versions.ToList());
        }

        // GET: Version/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Version version = db.Versions.Find(id);
            if (version == null)
            {
                return HttpNotFound();
            }
            return View(version);
        }

        // GET: Version/Create
        public ActionResult Create()
        {
            ViewBag.documentId = new SelectList(db.Documents, "Id", "Name");
            return View();
        }

        // POST: Version/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,VersionNumber,documentId")] Models.Version version, HttpPostedFileBase filename)
        {
            version.CreateDateTime = DateTime.Now;
            version.filename = System.IO.Path.GetFileName(filename.FileName);

            if (ModelState.IsValid)
            {
                db.Versions.Add(version);
                db.SaveChanges();
                string p = Server.MapPath("../Uploads/Documents/" + version.Id.ToString() + "_" + version.filename);
                filename.SaveAs(p);
                return RedirectToAction("Index");
            }

            ViewBag.documentId = new SelectList(db.Documents, "Id", "Name", version.documentId);
            return View(version);
        }

        // GET: Version/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Version version = db.Versions.Find(id);
            if (version == null)
            {
                return HttpNotFound();
            }
            ViewBag.documentId = new SelectList(db.Documents, "Id", "Name", version.documentId);
            return View(version);
        }

        // POST: Version/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,filename,CreateDateTime,VersionNumber,documentId")] Models.Version version)
        {
            if (ModelState.IsValid)
            {
                db.Entry(version).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.documentId = new SelectList(db.Documents, "Id", "Name", version.documentId);
            return View(version);
        }

        // GET: Version/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Version version = db.Versions.Find(id);
            if (version == null)
            {
                return HttpNotFound();
            }
            return View(version);
        }

        // POST: Version/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Models.Version version = db.Versions.Find(id);
            db.Versions.Remove(version);
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
    }
}
