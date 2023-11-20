using project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace project.Controllers
{
    public class LoginUserController : Controller
    {
        DBSportStoreEntities database = new DBSportStoreEntities();

        public ActionResult ManageAccount()
        {
            var userList = database.AdminUsers.ToList();
            return View(userList);
        }

        public ActionResult DeleteAccount(int id, AdminUser user)
        {
            try
            {
                user = database.AdminUsers.Where(s => s.ID == id).FirstOrDefault();
                database.AdminUsers.Remove(user);
                database.SaveChanges();
                return RedirectToAction("ManageAccount");
            }
            catch
            {
                return Content("This data is using in other table, Error Delete!");
            }
        }

        // GET: LoginUser
        public ActionResult LoginUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAccount(AdminUser _user)
        {
            var check = database.AdminUsers.Where(s => s.NameUser == _user.NameUser && s.PasswordUser == _user.PasswordUser).ToList();
            if (check == null )
            {
                ViewBag.ErrorInfo = "SaiInfo";
                return View("LoginUser");
            }
            else
            {
                database.Configuration.ValidateOnSaveEnabled = false;
                Session["UserName"] = _user.NameUser;
                return RedirectToAction("Index", "CustomerProducts");
            }
        }
        public ActionResult RegisterUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterUser(AdminUser _user)
        {
            if (ModelState.IsValid)
            {
                var check_ID = database.AdminUsers.Where(s => s.ID == _user.ID).FirstOrDefault();
                if (check_ID == null)
                {
                    database.Configuration.ValidateOnSaveEnabled = false;
                    _user.RoleUser = "Guest";
                    database.AdminUsers.Add(_user);
                    database.SaveChanges();
                    return RedirectToAction("LoginUser");
                }
                else
                {
                    ViewBag.ErrorRegister = "This ID is exist";
                    return View();
                }
            }
            return View();
        }
    }
}