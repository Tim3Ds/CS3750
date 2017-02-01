using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class TodoListsController : Controller
    {
        private Project1TodoEntities db = new Project1TodoEntities();

        // GET: TodoLists
        public ActionResult Index()
        {
            return View(db.TodoLists.ToList());
        }

        // GET: TodoLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TodoList todoList = db.TodoLists.Find(id);
            if (todoList == null)
            {
                return HttpNotFound();
            }
            return View(todoList);
        }

        // GET: TodoLists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TodoLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TodoList todoList)
        {
            if (ModelState.IsValid)
            {
                using (var db = new Project1TodoEntities())
                {
                    todoList.lastChangedDate = DateTime.Now;
                    db.TodoLists.Add(todoList);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(todoList);
        }

        // GET: TodoLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var db = new Project1TodoEntities())
            {
                TodoList todoList = db.TodoLists.Find(id);

                if (todoList == null)
                {
                    return HttpNotFound();
                }
                return View(todoList);
            }
        }

        // POST: TodoLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "list_id,listName,lastChangedDate")] TodoList todoList)
        {
            using (var db = new Project1TodoEntities())
            {
                if (ModelState.IsValid)
                {
                    todoList.lastChangedDate = DateTime.Now;
                    db.Entry(todoList).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(todoList);
            }
        }

        // GET: TodoLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var db = new Project1TodoEntities())
            {
                TodoList todoList = db.TodoLists.Find(id);
                if (todoList == null)
                {
                    return HttpNotFound();
                }
                return View(todoList);
            }
        }

        // POST: TodoLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            List<Item> itemlist = new List<Item>();
            List<int> catIds = new List<int>();
            using (var db = new Project1TodoEntities())
            {
                //catIds = (from s in db.Items
                //          where s.list_id == id
                //          select s.category_id).ToList();

                //foreach(int e in catIds)
                //{
                //    Category element = (from r in db.Categories
                //                        select r).SingleOrDefault();
                //    db.Categories.Remove(element);
                //}

                itemlist = (from r in db.Items
                            where r.list_id == id
                            select r).ToList();
                foreach (var e in itemlist)
                {
                    db.Items.Remove(e);
                }

                TodoList todoList = db.TodoLists.Find(id);
                db.TodoLists.Remove(todoList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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

