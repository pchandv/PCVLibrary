using PCVLibrary;
using PCVLibrary.MVC;
using PCVLibrary.MVCGrid;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PCVLibraryTESTConsole
{
    class Program
    {

        static void Main(string[] args)
        {
            // TestExcel();

            ProductVM m = new ProductVM();
            var s = m.Details.GetType().BaseType.Name;
            Console.WriteLine(s);
            Console.Read();



        }

        public class ProductVM
        {
            public ProductVM()
            {
                Details = new AutoCompleteTextBoxModel("", "", "");
            }
            public AutoCompleteTextBoxModel Details { get; set; }
        }




        #region TestLogic
        //private static void TestExcel()
        //{
        //    var vm = new List<CustomerVM>();
        //    //ReadExcel.RunExcelTest();

        //    for (int i = 1; i < 125; i++)
        //    {
        //        vm.Add(new CustomerVM() { index = i, Name = "Visam_" + i });
        //    }

        //    while (Console.ReadLine() != "stop")
        //    {
        //        CustomerGrid grid = new CustomerGrid();
        //        grid.DataSource = vm;
        //        Console.WriteLine("Enter Current Page Index");
        //        grid.CurrentPageIndex = Convert.ToInt32(Console.ReadLine());
        //        grid.IsSortEnable = true;
        //        grid.sortExpression = "index";
        //        grid.sortDirection = "D";
        //        Console.WriteLine("Page Count:->" + grid.PageCount);
        //        foreach (var item in grid.DataSource)
        //        {
        //            Console.WriteLine("|| index:{0} || Name{1} ||", item.index, item.Name);
        //        }
        //    }
        //}
        //public class CustomerGrid : GridViewModel<CustomerVM>
        //{
        //    public override void SetColums()
        //    {
        //        Headers = new List<Column>();
        //        Headers.Add(new Column() { DisplayName = "Product Name", Name = "Name" });
        //        Headers.Add(new Column() { DisplayName = "Product index", Name = "index", isHide = true });
        //    }
        //}
        //public class CustomerVM
        //{
        //    public int index { get; set; }
        //    public string Name { get; set; }
        //}
        #endregion

    }


}