using PCVLibrary.Extensions;
using PCVLibrary.MVCGrid;
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

        public ActionResult GridLandingScreen()
        {
            return View();
        }
        public ActionResult GridBind(GridViewFilter filter)
        {
            var mp = new Descriptor<ProductController>();
            List<object> vm = GetData();
            GridViewModel grid = new GridViewModel(mp.ControllerName, mp.ActionName(x => x.GridBind(null)), "Employee Details", SetColumns());
            grid.DataSource = vm;
            grid.IsSortEnable = true;
            grid.SetGridFilters(filter);
            return PartialView(grid._PartialGridView, grid);
        }

        public ActionResult GridBind2(GridViewFilter filter)
        {
            var mp = new Descriptor<ProductController>();
            List<object> vm = GetData2();
            GridViewModel grid = new GridViewModel(mp.ControllerName, mp.ActionName(x => x.GridBind2(null)), "Mobile Phone List", SetColumns2());
            grid.DataSource = vm;
            grid.SetGridFilters(filter);
            return PartialView(grid._PartialGridView, grid);
        }


        private static List<object> GetData()
        {
            var vm = new List<Object>();
            for (int i = 1; i <= 100250; i++)
            {
                vm.Add(new EmployeeVM() { EmpID = i, FirstNames = "" + i, LastName = "praveen_" + i });
            }

            return vm;
        }
        private static List<object> GetData2()
        {
            var vm = new List<Object>();
            for (int i = 1; i <= 100250; i++)
            {
                vm.Add(new ProductVM() { ProductID = i, ProductNames = "Iphone_" + i, Description = "Iphone Made in UK for _" + i + " Times" });
            }

            return vm;
        }

        public List<Column> SetColumns( )
        {
            var mp = new Descriptor<EmployeeVM>();
            List<Column> Headers = new List<Column>();
            Headers.Add(new Column() { DisplayName = "Employee First Name", Name = mp.Name(x=>x.FirstNames) });
            Headers.Add(new Column() { DisplayName = "Employee Last Name", Name = mp.Name(x => x.LastName) });
            Headers.Add(new Column() { DisplayName = "Employee ID", Name = mp.Name(x => x.EmpID)});
            return Headers;
        }
        public List<Column> SetColumns2()
        {
            var mp = new Descriptor<ProductVM>();
            List<Column> Headers = new List<Column>();
            Headers.Add(new Column() { DisplayName = "Product Name", Name = mp.Name(x => x.ProductNames) });
            Headers.Add(new Column() { DisplayName = "Product Description", Name = mp.Name(x => x.Description) });
            Headers.Add(new Column() { DisplayName = "Product ID", Name = mp.Name(x => x.ProductID), isHide = true });
            return Headers;
        }
    }
  
    public class EmployeeVM
    {
        public int EmpID { get; set; }
        public string FirstNames { get; set; }
        public string LastName { get; set; }
    }
    public class ProductVM
    {
        public int ProductID { get; set; }
        public string ProductNames { get; set; }
        public string Description { get; set; }
    }
}