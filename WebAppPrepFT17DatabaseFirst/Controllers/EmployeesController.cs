using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppPrepFT17DatabaseFirst.Models;

namespace WebAppPrepFT17DatabaseFirst.Controllers
{

    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly EmployeesDbContext db = new EmployeesDbContext();


        public JsonResult IsUniqueNIF(int? Id, string NIF)
        {
            bool unique;

            if (Id == null)
            {
                unique = !db.Employees.Any(e => e.NIF == NIF);
            }
            else
            {
                unique = !db.Employees.Any(e => e.NIF == NIF && e.Id != Id);
            }

            return Json(unique, JsonRequestBehavior.AllowGet);
        }

        private void PopulateDdlDepartments()
        {
            var ddlDepartments = new List<SelectListItem>();

            foreach (var item in db.Departments.ToList())
            {
                ddlDepartments.Add(new SelectListItem()
                {
                    Value = item.Id.ToString(),
                    Text = item.DeptName
                });
            }

            ViewData["Departments"] = ddlDepartments;
        }


        [HttpGet]
        public ActionResult Index(string SearchBy, string SearchText, string OrderBy)
        {
            if ((!string.IsNullOrEmpty(SearchBy) && SearchBy != "Nome" && SearchBy != "NIF") 
                || 
                (!string.IsNullOrEmpty(OrderBy) && OrderBy != "NameAsc" && OrderBy != "DOBAsc" && OrderBy != "NameDesc" && OrderBy != "DOBDesc"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var employees = db.Employees.ToList();

            if (SearchBy == "Nome")
            {
                employees = employees.Where(e => e.Name.StartsWith(SearchText)).ToList();
            }
            else if(SearchBy == "NIF")
            {
                employees = employees.Where(e => e.NIF.StartsWith(SearchText)).ToList();
            }

            OrderBy = OrderBy ?? "NameAsc";
            
            switch (OrderBy)
            {
                case "NameAsc":
                    employees = employees.OrderBy(e => e.Name).ToList();
                    break;
                case "NameDesc":
                    employees = employees.OrderByDescending(e => e.Name).ToList();
                    break;
                case "DOBAsc":
                    employees = employees.OrderBy(e => e.DateOfBirth).ToList();
                    break;
                case "DOBDesc":
                    employees = employees.OrderByDescending(e => e.DateOfBirth).ToList();
                    break;
                default:
                    break;
            }

            ViewData["SearchBy"] = SearchBy;
            ViewData["SearchText"] = SearchText;
            ViewData["OrderBy"] = OrderBy;

            return View(employees);
        }


        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            //if (Session["email"] == null) return RedirectToAction("Login", "Accounts");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employees employees = db.Employees.Find(id);
            if (employees == null)
            {
                return HttpNotFound();
            }
            return View(employees);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            //if (Session["email"] == null) return RedirectToAction("Login", "Accounts");

            PopulateDdlDepartments();

            return View();
        }


        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address,DateOfBirth,Salary,Worktype,NIF,DeptId")] Employees employees)
        {
            //if (Session["email"] == null) return RedirectToAction("Login", "Accounts");

            if (db.Employees.Any(e => e.NIF == employees.NIF))
            {
                ModelState.AddModelError("NIF", "NIF já registado no sistema!");
            }

            if (ModelState.IsValid)
            {
                db.Employees.Add(employees);
                db.SaveChanges();
                TempData["successMessage"] = "Empregado registado com sucesso";
                return RedirectToAction("Index");
            }

            PopulateDdlDepartments();
            return View(employees);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            //if (Session["email"] == null) return RedirectToAction("Login", "Accounts");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employees employees = db.Employees.Find(id);
            if (employees == null)
            {
                return HttpNotFound();
            }

            var ddlDepartments = new List<SelectListItem>();

            foreach (var item in db.Departments.ToList())
            {
                ddlDepartments.Add(new SelectListItem()
                {
                    Value = item.Id.ToString(),
                    Text = item.DeptName
                });
            }

            ViewData["Departments"] = ddlDepartments;

            return View(employees);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address,DateOfBirth,Salary,Worktype,NIF,DeptId")] Employees employees)
        {
            //if (Session["email"] == null) return RedirectToAction("Login", "Accounts");

            if (ModelState.IsValid)
            {
                db.Entry(employees).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employees);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            //if (Session["email"] == null) return RedirectToAction("Login", "Accounts");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employees employees = db.Employees.Find(id);
            if (employees == null)
            {
                return HttpNotFound();
            }
            return View(employees);
        }

        //POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //if (Session["email"] == null) return RedirectToAction("Login", "Accounts");

            Employees employees = db.Employees.Find(id);
            db.Employees.Remove(employees);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSelectedEmployees(IEnumerable<int> SelectEmployee)
        {
            if (SelectEmployee == null)
            {
                return RedirectToAction("Index");
            }

            db.Employees.Where(e => SelectEmployee.Contains(e.Id)).ToList().ForEach(e => db.Employees.Remove(e));
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

        /*protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            filterContext.Result = RedirectToAction("Error", "Error");

            base.OnException(filterContext);
        }*/
    }
}
