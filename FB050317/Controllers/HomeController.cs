using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook.Data;
using System.Web.Security;
using System.IO;
using FB050317.Models;

namespace FB050317.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ImagesVM vm = new ImagesVM();
            UserAuthenDb db = new UserAuthenDb(Properties.Settings.Default.ConStr);
            User user = db.GetByEmail(User.Identity.Name);
            if (user != null)
            {
                vm.User = user;
            }
            return View(vm);
        }
        
        public ActionResult GetPopImages()
        {
            ImagesDb db = new ImagesDb(Properties.Settings.Default.ConStr);
            return Json(db.GetPopularImages(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRecentImages()
        {
            ImagesDb db = new ImagesDb(Properties.Settings.Default.ConStr);
            return Json(db.GetRecentImages(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLikedImages()
        {
            ImagesDb db = new ImagesDb(Properties.Settings.Default.ConStr);
            return Json(db.GetLikedImages(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Register(User User, string Password)
        {
            UserAuthenDb db = new UserAuthenDb(Properties.Settings.Default.ConStr);
            db.AddUser(User, Password);
            return Redirect("/");
        }

        [HttpPost]
        public ActionResult Login(string UserName, string Password)
        {
            UserAuthenDb db = new UserAuthenDb(Properties.Settings.Default.ConStr);
            User user = db.Login(UserName, Password);
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(UserName, true);
                return Redirect("/home/index?Id=user.Id");
            }
            else
            {
                TempData["TD"] = "Wrong User Name or Password";
                return Redirect("/home/index");
            }
        }

        [HttpPost]
        public ActionResult Upload(Image image, HttpPostedFileBase file)
        {
            ImagesDb db = new ImagesDb(Properties.Settings.Default.ConStr);
            ImagesVM vm = new ImagesVM();
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(file.FileName);
            fileName += extension;
            file.SaveAs(Server.MapPath("~/Images/") + fileName);
            db.AddUpload(image, fileName);
            vm.Image = db.GetByFileName(fileName);
            return View(vm);
        }

        public ActionResult ViewImage(int Id)
        {
            ImagesDb db = new ImagesDb(Properties.Settings.Default.ConStr);
            UserAuthenDb db2 = new UserAuthenDb(Properties.Settings.Default.ConStr);
            ImagesVM vm = new ImagesVM();
            db.UpdateImageViews(Id);
            vm.Image = db.GetById(Id);
            vm.User = db2.GetByEmail(User.Identity.Name);
            return View(vm);
        }

        public ActionResult Liked(int Id)
        {
            ImagesDb db = new ImagesDb(Properties.Settings.Default.ConStr);
            UserAuthenDb db2 = new UserAuthenDb(Properties.Settings.Default.ConStr);
            ImagesVM vm = new ImagesVM();
            vm.Likes = db.GetLikesPerUser(Id);
            vm.User = db2.GetByEmail(User.Identity.Name);
            return View(vm);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        public ActionResult LikeImage(int UserId, int ImageId)
        {
            ImagesDb db = new ImagesDb(Properties.Settings.Default.ConStr);
            ImagesVM vm = new ImagesVM();
            db.LikeImage(UserId, ImageId);
            return Json(db.GetImageLikedCount(ImageId));
        }

        public ActionResult DidUserLike(int ImageId, int UserId)
        {
            ImagesDb db = new ImagesDb(Properties.Settings.Default.ConStr);
            return Json(db.CheckIfUserAlreadyLikedThis(ImageId, UserId), JsonRequestBehavior.AllowGet);
        }

    }
}