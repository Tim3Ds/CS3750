using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ItemsController : Controller
    {
        private Project1TodoEntities db = new Project1TodoEntities();

        // GET: Items
        public ActionResult Index()
        {
            var items = db.Items.Include(i => i.Category).Include(i => i.TodoList);
            return View(items.ToList());
        }

        public JsonResult GetTask(string taskName)
        {
            var cn = new SqlConnection();
            var ds = new DataSet();
            string strCn = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
            cn.ConnectionString = strCn;
            var cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.Text,
                CommandText = "SELECT taskName FROM dbo.Item WHERE taskName like @myParameter and taskName != @myParameter2"
            };

            cmd.Parameters.AddWithValue("@myParameter", "%" + taskName + "%");
            cmd.Parameters.AddWithValue("@myParameter2", taskName);

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                var da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception)
            {
            }
            finally
            {
                cn.Close();
            }
            DataTable dt = ds.Tables[0];


            var txtItems = (from DataRow row in dt.Rows
                            select row["taskName"].ToString()
                                into dbValues
                            select dbValues.ToLower()).ToList();
            return Json(txtItems, JsonRequestBehavior.AllowGet);
        }


        // GET: Items/Details/5
        public ActionResult Details(int? id)
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
            return View(item);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "categoryName");
            ViewBag.list_id = new SelectList(db.TodoLists, "list_id", "listName");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "item_id,list_id,category_id,taskName,isDone,lastChangedDate")] Item item)
        {
            using (var db = new Project1TodoEntities())
            {
                if (ModelState.IsValid)
                {
                    item.lastChangedDate = DateTime.Now;
                    db.Items.Add(item);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.category_id = new SelectList(db.Categories, "category_id", "categoryName", item.category_id);
                ViewBag.list_id = new SelectList(db.TodoLists, "list_id", "listName", item.list_id);
                return View(item);
            }
        }
        // GET: Items/Edit/5
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

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            using (var db = new Project1TodoEntities())
            {
                if (ModelState.IsValid)
                {
                    item.lastChangedDate = DateTime.Now;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.category_id = new SelectList(db.Categories, "category_id", "categoryName", item.category_id);
                ViewBag.list_id = new SelectList(db.TodoLists, "list_id", "listName", item.list_id);
                return View(item);
            }
        }

        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
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
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
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
    }
}
