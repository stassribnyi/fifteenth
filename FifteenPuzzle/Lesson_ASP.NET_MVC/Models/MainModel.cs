using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lesson_ASP.NET_MVC.Models
{
    public class MainModel
    {
        public string str1 { get; set; }
        public string str2 { get; set; }
        public string concat {
            get { return str1 + str2; }
        }
    }
}