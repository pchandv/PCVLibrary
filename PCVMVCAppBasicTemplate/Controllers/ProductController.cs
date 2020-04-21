using PCVMVCAppBasicTemplate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PCVMVCAppBasicTemplate.Controllers
{
    public class ProductController : Controller
    {
        // GET: Dropdown
        public ActionResult Index()
        {
            return View(new ProductSearchVM());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ProductSearchVM input)
        {
            return View(input);
        }
        [HttpPost]
        public JsonResult SearchProducts(string SearchText)
        {
            var ls = new List<string>();
            ls.Add("IPhone");
            ls.Add("Redmi");
            ls.Add("RealMe");
            ls.Add("Samsung");
            ls.Add("Nokia");
            ls.Add("Sony");
            ls.Add("TATA");

            return Json(ls.Where(x=>x.ToLower().Contains(SearchText.ToLower())).Select(x=>new SelectListItem() { Text=x,Value=x }).ToList(), JsonRequestBehavior.AllowGet);
        }

    }
}