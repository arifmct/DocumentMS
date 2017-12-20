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
    public class LoginHistoryController : Controller
    {
        private dbPaperLessEntities db = new dbPaperLessEntities();

        // GET: LoginHistory
        public ActionResult Index()
        {
            if (Session["login"].ToString() == "")
            {
                Session["msg"] = "You have to login to view these content";
                return RedirectToAction("Login", "Employee");
            }
            var loginHistories = db.LoginHistories.Include(l => l.Employee);
            return View(loginHistories.ToList());
        }

        // GET: LoginHistory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoginHistory loginHistory = db.LoginHistories.Find(id);
            if (loginHistory == null)
            {
                return HttpNotFound();
            }
            return View(loginHistory);
        }

        // GET: LoginHistory/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name");
            return View();
        }

        // POST: LoginHistory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmployeeId,DateTime,IPAddress")] LoginHistory loginHistory)
        {
            if (ModelState.IsValid)
            {
                db.LoginHistories.Add(loginHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name", loginHistory.EmployeeId);
            return View(loginHistory);
        }

        // GET: LoginHistory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoginHistory loginHistory = db.LoginHistories.Find(id);
            if (loginHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name", loginHistory.EmployeeId);
            return View(loginHistory);
        }

        // POST: LoginHistory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmployeeId,DateTime,IPAddress")] LoginHistory loginHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loginHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name", loginHistory.EmployeeId);
            return View(loginHistory);
        }

        // GET: LoginHistory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoginHistory loginHistory = db.LoginHistories.Find(id);
            if (loginHistory == null)
            {
                return HttpNotFound();
            }
            return View(loginHistory);
        }

        // POST: LoginHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoginHistory loginHistory = db.LoginHistories.Find(id);
            db.LoginHistories.Remove(loginHistory);
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
