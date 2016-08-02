using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmailTest.CommonHelper;

namespace EmailTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            //Email test
            EmailHelper email = new EmailHelper();
            string mailSub = "新的任务";
            string mailBody = "Test Email";
            email.SendEmail(mailSub, mailBody, "693567439@qq.com");


            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}