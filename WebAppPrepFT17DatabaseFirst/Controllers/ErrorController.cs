using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppPrepFT17DatabaseFirst.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            return View("NotFound");
        }

        
        public ActionResult Error()
        {
            string action = Request.QueryString["actionname"];
            string controller = Request.QueryString["controllername"];
            Exception exception = new Exception("Ocorreu um erro inesperado! Contacte o administrador do sistema!");


            if (action != null && controller != null)
            {
                return View("Error", new HandleErrorInfo(exception, controller, action));
            }

            return View("Error");
        }

    }
}

