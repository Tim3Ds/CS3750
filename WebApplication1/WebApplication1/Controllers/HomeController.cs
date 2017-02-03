using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Data;
using System.Data.Entity;
using System.Net;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private Project1TodoEntities db = new Project1TodoEntities();


        public ActionResult Index()
        {
            
            using(var db = new Project1TodoEntities())
            {
                var items = db.TodoLists.Include(i => i.Items);
                return View(items.ToList());
            }
            
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "categoryName", item.category_id);
            ViewBag.list_id = new SelectList(db.TodoLists, "list_id", "listName", item.list_id);
            return View(item);
        }
    }
}