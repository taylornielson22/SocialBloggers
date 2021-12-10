using SocialBloggers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialBloggers.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public int Login(string username, string password)
        {
            using (BloggingEngineEntities db = new BloggingEngineEntities())
            {
                if (UserExists(username))
                {
                    var userid = (from u in db.Users
                        where u.Username == username
                         && u.Password == password
                         select u.Userid).FirstOrDefault();

                    return userid;
                }

                return 0;
            }
        }

        public bool UserExists(string username)
        {
            using (BloggingEngineEntities db = new BloggingEngineEntities())
            {
                return db.Users.Where(x => x.Username == username).Any();
            }
        }
    }
}