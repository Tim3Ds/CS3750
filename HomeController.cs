using Example.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Example.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public List<IssueList> GetIssues()
        {
            List<IssueList> list = new List<IssueList>();
            using (var db = new MvcAuctionEntities())
            {
                var issues = (from r in db.NinissueAdmin
                              select r);
                foreach (var e in issues)
                {
                    list.Add(new IssueList { IssueId = e.IssueId, Issue = e.Issue, Category = e.Category });
                }
            }
            return list;
        }

        [HttpPost]
        public void Edit(int id)
        {

        }

        [HttpPost]
        public void Delete(int id)
        {

        }

        [HttpPost]
        public void Add()
        {

        }

    }
}