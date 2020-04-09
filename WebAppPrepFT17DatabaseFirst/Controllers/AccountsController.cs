using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebAppPrepFT17DatabaseFirst.Models;
using WebAppPrepFT17DatabaseFirst.CryptoHelpers;

namespace WebAppPrepFT17DatabaseFirst.Controllers
{
    //[HandleError]
    public class AccountsController : Controller
    {
        private readonly EmployeesDbContext db = new EmployeesDbContext();

        public JsonResult IsUniqueEmail(string Email)
        {
            var res = !db.Users.Any(u => u.Email == Email);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Id,Email,Password")] Users user)
        {
            if (db.Users.Any(u => u.Email == user.Email))
            {
                //ViewBag.Mensagem = new HandleErrorInfo(new Exception("Esse email já se encontra registado no sistema. Tente novamente!"), "Accounts", "Register");
                //return View("Error");

                ModelState.AddModelError("Email", "Esse email já se encontra registado no sistema. Tente novamente!");
            }
            
            if (ModelState.IsValid)
            {
                user.Password = PasswordEncryptSHA256.GenerateSHA256String(user.Password);

                db.Users.Add(user);
                TempData["successMessage"] = "Utilizador registado com sucesso";
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Id,Email,Password")] Users user)
        {
            if (ModelState.IsValid)
            {
                var passwordEncrypted = PasswordEncryptSHA256.GenerateSHA256String(user.Password);
                var userLogged = db.Users.SingleOrDefault(u => u.Email == user.Email && u.Password == passwordEncrypted);

                if (userLogged != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    Session["email"] = userLogged.Email;
                    TempData["successMessage"] = "Login efetuado com sucesso";
                    return RedirectToAction("Index", "Employees");
                }
                TempData["errorMessage"] = "Email e/ou password erradas! Tente novamente";
                return View("Error", new HandleErrorInfo(new Exception("Email e/ou password erradas! Tente novamente"), "Accounts", "Login"));
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            if (Session["email"] != null)
            {
                Session.Abandon();
            }
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            filterContext.Result = View("Error", new HandleErrorInfo(new Exception(filterContext.Exception.Message), "Accounts", this.RouteData.Values["action"].ToString()));

            //filterContext.Result = RedirectToAction("Error", "Error", new HandleErrorInfo(new Exception(), "Accounts", this.RouteData.Values["action"].ToString()));
            //filterContext.Result = RedirectToAction("Error", "Error");

            base.OnException(filterContext);
        }
    }
}