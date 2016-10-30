using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class ShareDataAttribute : ActionFilterAttribute
    {
        //應用1=>共用Code提出來這裡
        //應用2=>Logdata,Start end time
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewData["Temp1"] = "暫存資料 Temp11";
        }
    }
}