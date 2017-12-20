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
    public class DocumentController : Controller
    {
        private dbPaperLessEntities db = new dbPaperLessEntities();

        [AllowAnonymous]
        // GET: Document
        public ActionResult Index(string search = "", int labelFrom = 0, int labelTo = 0, int CategoryId = 0)
        {
            if(Session["login"].ToString() == "")
            {
                Session["msg"] = "You have to login to view these content";
                return RedirectToAction("Login", "Employee");
            }

            int? lbl = ((Models.Employee)Session["emp"]).Level;


            var documents = db.Documents.Where(d=>d.level <= lbl).Include(d => d.Category).ToList();

            if(search != "")
            {
                documents = documents.Where(d => d.Name.ToLower().Contains(search.ToLower()) || d.Description.ToLower().Contains(search.ToLower())).ToList();
            }

            if(labelFrom > 0)
            {
                documents = documents.Where(d => d.level >= labelFrom).ToList();
            }
            if(labelTo > 0)
            {
                documents = documents.Where(d => d.level <= labelTo).ToList();
            }

            if(CategoryId > 0)
            {
                documents = documents.Where(d => d.CategoryId == CategoryId).ToList();
            }

            ViewBag.search = search;
            ViewBag.labelFrom = labelFrom;
            ViewBag.labelTo = labelTo;
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View(documents.ToList());
        }

        // GET: Document/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // GET: Document/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Document/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,CategoryId,level")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Documents.Add(document);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", document.CategoryId);
            return View(document);
        }

        // GET: Document/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", document.CategoryId);
            return View(document);
        }

        // POST: Document/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,CategoryId,level")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", document.CategoryId);
            return View(document);
        }

        // GET: Document/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Document/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = db.Documents.Find(id);
            db.Documents.Remove(document);
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
