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
      //  [OutputCache(Duration =1000)]
        public ActionResult GridBind(GridViewFilter filter)
        {
            EmployeeGridView grid = new EmployeeGridView(filter);
            return PartialView(grid._PartialGridView, grid);
        }

      
        public ActionResult EditEmployee(int EmpID)
        {
            return View("_EditEmployee",EmpID);
        }
        public ActionResult DeleteEmployee(int EmpID)
        {
            return View("_EditEmployee", EmpID);
        }
        public ActionResult SaveEmployee(int EmpID)
        {
            return View("_EditEmployee", EmpID);
        }


        #region MyRegion
        private static List<object> GetData()
        {
            var vm = new List<Object>();
            for (int i = 1; i <= 100250; i++)
            {
                vm.Add(new EmployeeVM() { EmpID = i, FirstNames = "" + i, LastName = "praveen_" + i });
            }

            return vm;
        }

        
        #endregion
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

    public class EmployeeGridView : GridViewModel
    {
        public EmployeeGridView(GridViewFilter filter) : 
            base(Descriptor<ProductController>.ControllerName,Descriptor<ProductController>.ActionName(x=>x.GridBind(null)), "Employee Grid")
        {
            this.IsEditable = true;
            this.EditView = new GridViewEditView
                ("Employee Edit", Descriptor<ProductController>.ControllerName,
                Descriptor<ProductController>.ActionName(x => x.EditEmployee(0)), Descriptor<ProductController>.ActionName(x => x.SaveEmployee(0))
                , Descriptor<ProductController>.ActionName(x => x.DeleteEmployee(0)),
                Descriptor<EmployeeVM>.Name(x => x.EmpID));
            this.SetGridFilters(filter);
            this._MaxRows = 15;
        }
        public override List<Column> SetColumns()
        {
            var Headers = new List<Column>();
            Headers.Add(new Column() { DisplayName = "Employee First Name", Name = Descriptor<EmployeeVM>.Name(x => x.FirstNames) });
            Headers.Add(new Column() { DisplayName = "Employee Last Name", Name = Descriptor<EmployeeVM>.Name(x => x.LastName) });
            Headers.Add(new Column() { DisplayName = "Employee ID", Name = Descriptor<EmployeeVM>.Name(x => x.EmpID) });
            Headers.Add(new Column() { DisplayName = "Employee First Name", Name = Descriptor<EmployeeVM>.Name(x => x.FirstNames) });
            Headers.Add(new Column() { DisplayName = "Employee Last Name", Name = Descriptor<EmployeeVM>.Name(x => x.LastName) });
            Headers.Add(new Column() { DisplayName = "Employee ID", Name = Descriptor<EmployeeVM>.Name(x => x.EmpID) });
            Headers.Add(new Column() { DisplayName = "Employee First Name", Name = Descriptor<EmployeeVM>.Name(x => x.FirstNames) });
            //Headers.Add(new Column() { DisplayName = "Employee Last Name", Name = Descriptor<EmployeeVM>.Name(x => x.LastName) });
            //Headers.Add(new Column() { DisplayName = "Employee ID", Name = Descriptor<EmployeeVM>.Name(x => x.EmpID) });
            //Headers.Add(new Column() { DisplayName = "Employee First Name", Name = Descriptor<EmployeeVM>.Name(x => x.FirstNames) });
            //Headers.Add(new Column() { DisplayName = "Employee Last Name", Name = Descriptor<EmployeeVM>.Name(x => x.LastName) });
            //Headers.Add(new Column() { DisplayName = "Employee ID", Name = Descriptor<EmployeeVM>.Name(x => x.EmpID) });
            //Headers.Add(new Column() { DisplayName = "Employee First Name", Name = Descriptor<EmployeeVM>.Name(x => x.FirstNames) });
            //Headers.Add(new Column() { DisplayName = "Employee Last Name", Name = Descriptor<EmployeeVM>.Name(x => x.LastName) });
            //Headers.Add(new Column() { DisplayName = "Employee ID", Name = Descriptor<EmployeeVM>.Name(x => x.EmpID) });
            //Headers.Add(new Column() { DisplayName = "Employee First Name", Name = Descriptor<EmployeeVM>.Name(x => x.FirstNames) });
            //Headers.Add(new Column() { DisplayName = "Employee Last Name", Name = Descriptor<EmployeeVM>.Name(x => x.LastName) });
            //Headers.Add(new Column() { DisplayName = "Employee ID", Name = Descriptor<EmployeeVM>.Name(x => x.EmpID) });
            //Headers.Add(new Column() { DisplayName = "Employee First Name", Name = Descriptor<EmployeeVM>.Name(x => x.FirstNames) });
            //Headers.Add(new Column() { DisplayName = "Employee Last Name", Name = Descriptor<EmployeeVM>.Name(x => x.LastName) });
            //Headers.Add(new Column() { DisplayName = "Employee ID", Name = Descriptor<EmployeeVM>.Name(x => x.EmpID) });
            //Headers.Add(new Column() { DisplayName = "Employee First Name", Name = Descriptor<EmployeeVM>.Name(x => x.FirstNames) });
            //Headers.Add(new Column() { DisplayName = "Employee Last Name", Name = Descriptor<EmployeeVM>.Name(x => x.LastName) });
            //Headers.Add(new Column() { DisplayName = "Employee ID", Name = Descriptor<EmployeeVM>.Name(x => x.EmpID) });
            //Headers.Add(new Column() { DisplayName = "Employee First Name", Name = Descriptor<EmployeeVM>.Name(x => x.FirstNames) });
            //Headers.Add(new Column() { DisplayName = "Employee Last Name", Name = Descriptor<EmployeeVM>.Name(x => x.LastName) });
            //Headers.Add(new Column() { DisplayName = "Employee ID", Name = Descriptor<EmployeeVM>.Name(x => x.EmpID) });
            return Headers;
        }
        public override List<object> GetDataSource()
        {
            var vm = new List<EmployeeVM>();
            for (int i = 1; i <= 100250; i++)
            {
                vm.Add(new EmployeeVM() { EmpID = i, FirstNames = "Visam_Praveen" + i, LastName = "Chandu" + i });
            }
            return vm.ToList<object>();
        }
    }

    

}