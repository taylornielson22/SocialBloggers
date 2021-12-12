using SocialBloggers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SocialBloggers.Controllers
{
    public class HomeController : Controller
    {
        public string[] BoxClasses = { "bluebox", "creambox", "greenbox", "pinkbox"};
    
        public ActionResult MyProfile()
        {
            if (!isLoggedIn())
            {
                return RedirectToAction("Login");
            }
            var myProfile = Request.Cookies.Get("user").Value;
            return View(myProfile);
        }
        public bool isLoggedIn()
        {
                var cookie = System.Web.HttpContext.Current.Request.Cookies.Get("user");
                if (cookie != null)
                {
                    return cookie.Value == "" ? false : true;
                }
                return false;
        }

        public ActionResult Posts(string user = null)
        {
            if (!isLoggedIn())
            {
                return RedirectToAction("Login");
            }
            HomeSummary homeView = new HomeSummary();
            homeView.CurrentUser = CurrentUser();
            if (user == null)
            {
                ViewBag.Title = "Blog Feed";
                homeView.AllPosts = GetAllPosts();
                homeView.CurrentProfile = homeView.CurrentUser;
            }
            else
            {
                
                if(user == "current")
                {
                    homeView.CurrentProfile = homeView.CurrentUser;
                } else
                {
                    var currentProfile = new CurrentUser(); 
                    currentProfile.Username = user.Trim();
                    currentProfile.Followers = Followers(user);
                    currentProfile.Following = Following(user);
                    homeView.CurrentProfile = currentProfile;
                }
                ViewBag.Title = homeView.CurrentProfile.Username;
                homeView.AllPosts = GetAllUsersPosts(homeView.CurrentProfile.Username);
                
            }
            return View(homeView);
        }


        public IEnumerable<User> AlllUsers()
        {
            using (BloggingEngineEntities db = new BloggingEngineEntities())
            {
                var user = Request.Cookies.Get("user").Value;
                var users = db.Users.ToList();
                var i = 0;
                foreach (var u in users)
                {
                    u.Follows = (from f in db.Followings
                                 where f.User == user
                                 && f.Follows == u.Username
                                 select f).Any();
                    if (i == 4)
                        i = 0;
                    u.ColorClass = BoxClasses[i];
                    i++;
                }
                return users;
            }
        }

        public ActionResult AllUsers()
        {
            if (!isLoggedIn())
            {
                return RedirectToAction("Login");
            }
            var view = new AllUserSummary();
            view.Users = AlllUsers();
            view.CurrentUser = CurrentUser();
            return View(view);
        }

        public ActionResult Logout()
        {
            HttpCookie cookie = new HttpCookie("user");
            cookie.Value = null;
            cookie.Expires = System.DateTime.Now.AddHours(1);
            System.Web.HttpContext.Current.Response.SetCookie(cookie);
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            if (isLoggedIn())
            {
                return RedirectToAction("Posts");
            }

            return View();
        }

        public List<Following> Following(string user)
        {
            using (BloggingEngineEntities db = new BloggingEngineEntities())
            {
                return db.Followings.Where(x => x.User == user && x.Follows != user).ToList();
            }
        }

        public List<Following> Followers(string user)
        {
            using (BloggingEngineEntities db = new BloggingEngineEntities())
            {
                return db.Followings.Where(x => x.Follows == user && x.User != user).ToList();
            }
        }

        public CurrentUser CurrentUser()
        {
            var user = new CurrentUser();
            user.Username = Request.Cookies.Get("user").Value.Trim();
            user.Followers = Followers(user.Username);
            user.Following = Following(user.Username);
            return user;
        }
        public IEnumerable<Post> GetAllUsersPosts(string user)
        {
            using(BloggingEngineEntities db = new BloggingEngineEntities())
            {
                var posts = (from p in db.Posts
                        where p.Username == user
                             select p).ToList();
                posts.OrderByDescending(p => p.Postid);
                int i = 0;
                foreach (var p in posts)
                {
                    if (i == 5)
                        i = 0;
                    var current = Request.Cookies.Get("user").Value;
                    p.AllComments = (from c in db.Comments
                                     where c.PostID == p.Postid
                                     select c).ToList();
                    p.LikedByUser = ((from l in db.Likes
                                          where l.Username == current
                                          && l.PostID == p.Postid
                                          select l).Count() != 0);
                    p.Date = p.Lastmodified.ToString("M/d/yy");
                    p.ColorClass = BoxClasses[i];
                    i++;
                }
                return posts;
            }
        }

        [HttpPost]
        public void AddPost(string title, string content)
        {
            using (BloggingEngineEntities db = new BloggingEngineEntities())
            {
                var user = Request.Cookies.Get("user").Value;
                db.Database.ExecuteSqlCommand("insert into [dbo].[Posts] (Username, Title, Content, Likes, Lastmodified) values ('" + user + "', '" + title + "', '" + content + "', 0, GETDATE())");
            }

        }

        [HttpPost]
        public ActionResult CreateAccount(string username, string password)
        {
            using (BloggingEngineEntities db = new BloggingEngineEntities())
            {
                db.Database.ExecuteSqlCommand("insert into [dbo].[Users] (Username, Password) values ('" + username.Trim() + "', '" + password + "')");
                db.Database.ExecuteSqlCommand("insert into [dbo].[Followings] ([User], [Follows]) values('" + username + "','" + username + "')");
                return View("Login");
            }
        }

        [HttpPost]
        public void LikePost(int postid)
        {
            using(var db = new BloggingEngineEntities())
            {
                var user = Request.Cookies.Get("user").Value;
                db.Database.ExecuteSqlCommand("insert [dbo].[Likes] (PostID, Username) values(" + postid + ",'" + user + "')");
                db.Database.ExecuteSqlCommand("update [dbo].[Posts] set Likes = Likes + 1 where Postid =" + postid);
            }
        }

        [HttpPost]
        public void UnlikePost(int postid)
        {
            using (var db = new BloggingEngineEntities())
            {
                var user = Request.Cookies.Get("user").Value;
                db.Database.ExecuteSqlCommand("delete [dbo].[Likes] where Username='" + user + "' and postid=" + postid);
                db.Database.ExecuteSqlCommand("update [dbo].[Posts] set Likes = Likes - 1 where Postid =" + postid);
            }
        }

        [HttpPost]
        public void Comment(int postid, string message)
        {
            using (var db = new BloggingEngineEntities())
            {
                var user = Request.Cookies.Get("user").Value;
                db.Database.ExecuteSqlCommand("insert [dbo].[Comments] (PostID, CommentBy, Comment) values(" + postid + ",'" + user + "', '" + message + "')");
                db.Database.ExecuteSqlCommand("update [dbo].[Posts] set Comments = Comments + 1 where Postid =" + postid);
            }
        }

        [HttpPost]
        public void DeleteComment(int commentId)
        {
            using (var db = new BloggingEngineEntities())
            {
                db.Database.ExecuteSqlCommand("delete [dbo].[Comments] where CommentId=" + commentId);
            }
        }

        public ActionResult CreateAccount(int userid =0)
        {
            var user = new User();
            return View(user);
        }

        public IEnumerable<Post> GetAllPosts()
        {
            using (BloggingEngineEntities db = new BloggingEngineEntities())
            {
                var user = Request.Cookies.Get("user").Value; 
                var result = db.spGetPosts(user).OrderByDescending(p => p.Postid);
                var posts = new List<Post>();
                int i = 0;
                foreach (var post in result.ToList())
                {
                    var p = new Post();
                    p.AllComments = (from c in db.Comments
                                      where c.PostID == post.Postid
                                      select c).ToList();
                    if (i == 4)
                        i = 0;
                    p.Content = post.Content;
                    p.Title = post.Title;
                    p.Postid = post.Postid;
                    p.Username = post.Username;
                    p.LikedByUser = ((from l in db.Likes
                                  where l.Username == user
                                  && l.PostID == p.Postid
                                  select l).Count() != 0);
                    p.ColorClass = BoxClasses[i];
                    p.Likes = post.Likes;
                    p.Date = post.Lastmodified.ToString("M/d/yy");
                    i++;
                    posts.Add(p);
                }
                return posts;
            }
        }

        [HttpPost]
        public void Follow(string username)
        {
            using (BloggingEngineEntities db = new BloggingEngineEntities())
            {
                var user = Request.Cookies.Get("user").Value;
                db.Database.ExecuteSqlCommand("insert into [dbo].[Followings] ([User], [Follows]) values('" + user + "','" + username + "')");
            }
        }

        [HttpPost]
        public void UnFollow(string username)
        {
            using (BloggingEngineEntities db = new BloggingEngineEntities())
            {
                var user = Request.Cookies.Get("user").Value;
                db.Database.ExecuteSqlCommand("delete [dbo].[Followings] where [User] ='" + user + "' AND [Follows] ='" + username + "'");
            }       
        }

        [HttpGet]
        public int LoginAttempt(string username, string password)
        {
            var result = 0;
            if (UserExists(username))
                result = -1;

            if (ValidLogin(username, password))
            {
                HttpCookie cookie = new HttpCookie("user");
                cookie.Value = username.Trim().ToLower();
                cookie.Expires = System.DateTime.Now.AddHours(1);
                System.Web.HttpContext.Current.Response.SetCookie(cookie);
                result = 1;
            }
            return result;
        }

        public bool ValidLogin(string username, string password)
        {
            using (BloggingEngineEntities db = new BloggingEngineEntities())
            {
                return (from u in db.Users
                        where u.Username == username
                         && u.Password == password
                        select u.Userid).Count() > 0;
            }
        }

        public bool UserExists(string username)
        {
            using (BloggingEngineEntities db = new BloggingEngineEntities())
            {
                return ((from u in db.Users
                         where u.Username == username
                         select u.Userid).Count() > 0);
            }
        }

        public void EditPost(int postid, string title, string content)
        {
            using (BloggingEngineEntities db = new BloggingEngineEntities())
            {
                db.Database.ExecuteSqlCommand("update [dbo].[Posts] set Title = '" + title + "', Content='" + content + "' where Postid =" + postid);
            }
        }

        public void DeletePost(int postid)
        {
            using (BloggingEngineEntities db = new BloggingEngineEntities())
            {
                db.Database.ExecuteSqlCommand("delete [dbo].[Posts] where Postid =" + postid);
                db.Database.ExecuteSqlCommand("delete from [dbo].[Likes] where PostID =" + postid);
            }
        }
    }
}