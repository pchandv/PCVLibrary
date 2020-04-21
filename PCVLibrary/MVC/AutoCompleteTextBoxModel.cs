using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCVLibrary.MVC
{
    public class AutoCompleteTextBoxModel : IAutoCompleteTextBox
    {
        public string _paraName { get; set; }

        public AutoCompleteTextBoxModel(string controller, string action, string paraName = "SearchText")
        {
            Url = string.Format("../{0}/{1}", controller, action);
            _paraName = paraName;

        }
        public string SelectedValue { get; set; }

        public string Url
        {
            get; set;
        }
    }
    internal interface IAutoCompleteTextBox
    {
        string Url { get; set; }
        string SelectedValue { get; set; }
    }
}
