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
    public class EmployeeController : Controller
    {
        private dbPaperLessEntities db = new dbPaperLessEntities();

        // GET: Employee
        public ActionResult Index(string search="", int labelFrom = 0, int labelTo = 0)
        {
            if (Session["login"].ToString() == "")
            {
                Session["msg"] = "You have to login to view these content";
                return RedirectToAction("Login", "Employee");
            }

            var v = db.Employees.ToList();
            
            if(search != "")
            {
                v = v.Where(e=>e.Name.ToLower().Contains(search.ToLower()) || e.Email.ToLower().Contains(search.ToLower())).ToList();
            }

            if(labelFrom > 0)
            {
                v = v.Where(e => e.Level >= labelFrom).ToList();
            }

            if(labelTo > 0)
            {
                v = v.Where(e => e.Level <= labelTo).ToList();
            }

            ViewBag.search = search;
            ViewBag.labelFrom = labelFrom;
            ViewBag.labelTo = labelTo;

            return View(v);
        }


        public ActionResult Logout()
        {
            Session["emp"] = new Models.Employee();
            Session["login"] = "";
            Session["msg"] = "";
            return View();
        }

        //Get, Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.Employee emp)
        {
            var v = db.Employees.Where(e => e.Email.ToLower() == emp.Email && e.Password == emp.Password).ToList();

            if(v.Count <= 0)
            {
                Session["msg"] = "Invalid Login";
                return View(emp);
            }

            Session["emp"] = v.First();
            Session["login"] = "True";

            return RedirectToAction("Index", "Document");
        }

        // GET: Employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Contact,Email,Password,Level")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Contact,Email,Password,Level")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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
