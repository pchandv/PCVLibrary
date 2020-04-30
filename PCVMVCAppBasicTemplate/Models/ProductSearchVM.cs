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
            ProductName = new AutoCompleteTextBoxModel(Descriptor<ProductController>.ControllerName, Descriptor<ProductController>.ActionName(x=>x.SearchProducts("")));
        }
        [UIHint("AutoCompleteTextBox")]
        [Display(Name ="Product Name")]
        public AutoCompleteTextBoxModel ProductName { get; set; }
        public string Price { get; set; }
    }
}