using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example.Models
{
    public class IssuesAdminModel
    {
        public List<IssueList> IssuesCollection { get; set; }
        public IssueList Issues { get; set; }
    }

    public class IssueList
    {
        public int IssueId { get; set; }
        public string Issue { get; set; }
        public string Category { get; set; }
    }
}