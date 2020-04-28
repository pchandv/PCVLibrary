using PCVLibrary.MVC;
using PCVLibrary.MVCGrid;
using PCVMVCAppBasicTemplate.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCVMVCAppBasicTemplate.Models
{
    public class ProductSearchVM
    {
        public ProductSearchVM()
        {
            var d = new Descriptor<ProductController>();
            ProductName = new AutoCompleteTextBoxModel(d.ControllerName, d.ActionName(x=>x.SearchProducts("")));
        }
        [UIHint("AutoCompleteTextBox")]
        [Display(Name ="Product Name")]
        public AutoCompleteTextBoxModel ProductName { get; set; }
        public string Price { get; set; }
    }
}