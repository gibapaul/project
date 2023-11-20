using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using project.Models;
using System.Net;
using System.Xml.Linq;
using System.Data.Entity;
using PagedList;
using PagedList.Mvc;

namespace project.Controllers
{
    public class CustomerProductsController : Controller
    {
        private DBSportStoreEntities db = new DBSportStoreEntities();
        // GET: CustomerProducts
      /*  public ActionResult Index(string _name)
        {
            if (_name == null)
                return View(db.Products.ToList());
            else
                return View(db.Products.Where(s => s.NamePro.Contains(_name)).ToList());
        }
*/
        public ActionResult Index(string _name, int? page, double min = double.MinValue, double max = double.MaxValue)
        {
            int pageSize = 3;
            int pageNum = (page ?? 1);
            if (_name == null)
            {
                var productList = db.Products.OrderByDescending(x => x.NamePro);
                return View(productList.ToPagedList(pageNum, pageSize));
            }
            else
            {
                var productList = db.Products.OrderByDescending(x => x.NamePro).Where(s => s.NamePro.Contains(_name));
                return View(productList);
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        public ActionResult GetProductsByCategory()
        {
            var categories = db.Categories.ToList();
            return PartialView("CategoriesPartialView", categories);
        }
        public ActionResult GetProductsByCateId(int id)
        {
            var products = db.Products.Where(p => p.Category.Id == id).ToList();
            return View("Index", products);
        }
    }
}