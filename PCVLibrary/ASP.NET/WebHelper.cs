using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PCVLibrary.ASP.NET
{
    /// <summary>
    /// Below method help to read the gridview data when cells contains textbox and label 
    /// And It should have proper naming convention 
    /// To read each cell properly then name of the control should match with ViewModel propertyName
    /// GridView Column should only contain either Textbox or Label.
    /// </summary>
    public class WebControlHelpers
    {
        /// <summary>
        /// Contentplace holder for asp.net page
        /// </summary>
        public ContentPlaceHolder _contentPlaceHolder { get; set; }

        public WebControlHelpers(ContentPlaceHolder placeHolder)
        {
            _contentPlaceHolder = placeHolder;
        }

        /// <summary>
        /// T is the type of ViewModel that will be used in the Gridview Data binding
        /// DataTable cannot work on this
        /// </summary>
        /// <typeparam name="T">Type of model that binded to grid</typeparam>
        /// <param name="grid">instance of page level grid</param>
        /// <returns></returns>
        public List<T> ReadGrid<T>(GridView grid) where T : new()
        {
            List<T> dataSource = new List<T>();
            foreach (GridViewRow row in grid.Rows)
            {
                var viewModelRecord = new T();
                var typeOfViewModel = viewModelRecord.GetType();
                if (row.RowType == DataControlRowType.DataRow)
                {
                    foreach (PropertyInfo propertyInfo in typeOfViewModel.GetProperties())
                    {
                        var control = row.FindControl(propertyInfo.Name.ToString());
                        dynamic objValue = null;
                        #region Convert Control Type
                        if (control is TextBox)
                        {
                            objValue = (control as TextBox).Text;
                        }
                        else if (control is Label)
                        {
                            objValue = (control as Label).Text;
                        }
                        else if (control is CheckBox)
                        {
                            objValue = (control as CheckBox).Checked;
                        }
                        else if (control is HiddenField)
                        {
                            objValue = (control as HiddenField).Value;
                        }
                        #endregion
                        #region Convert to Property Data Type

                        if (objValue != null)
                        {
                            if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
                            {
                                propertyInfo.SetValue(viewModelRecord, DateTime.ParseExact(objValue, "mmddyyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                            }
                            else if (propertyInfo.PropertyType == typeof(string))
                            {
                                objValue = (string)objValue;
                                propertyInfo.SetValue(viewModelRecord, objValue);
                            }
                            else if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?))
                            {
                                int _value = 0;
                                if (int.TryParse(objValue, out _value)) propertyInfo.SetValue(viewModelRecord, _value);
                            }

                            else if (propertyInfo.PropertyType == typeof(bool))
                            {
                                propertyInfo.SetValue(viewModelRecord, (bool)objValue);
                            }
                            else if (propertyInfo.PropertyType == typeof(double) || propertyInfo.PropertyType == typeof(double?))
                            {
                                double _value = 0;
                                if (double.TryParse(objValue, out _value)) propertyInfo.SetValue(viewModelRecord, _value);
                            }
                            #endregion

                        }
                    }
                    dataSource.Add(viewModelRecord);
                }
            }
            return dataSource;
        }

        /// <summary>
        /// Generic Method to bind the dropdown data source with ListItems
        /// </summary>
        /// <param name="dropDownList">instance of Dropdown control in the page</param>
        /// <param name="dataSource">List of Listitem</param>
        /// <param name="isAddSelect"> --Select-- value will be added if it is true</param>
        public void DropDownBind(DropDownList dropDownList, List<ListItem> dataSource, bool isAddSelect = false, bool IsSortOrderByText = false)
        {
            dropDownList.Items.Clear();
            if (dataSource != null && dataSource.Count > 0)
            {
                if (isAddSelect)
                {
                    dropDownList.Items.Add(new ListItem() { Text = "-Select-", Value = "-1", Selected = true });

                }
                var SortedOrderAsc = dataSource.ToArray();
                if (IsSortOrderByText)
                {

                    SortedOrderAsc = dataSource.OrderBy(x => x.Text).ToArray();
                }
                //else
                //{
                //    SortedOrderAsc = dataSource.OrderBy(x => x.Value).ToArray();
                //}

                dropDownList.Items.AddRange(SortedOrderAsc);
            }
            else
            {
                dropDownList.Items.Add(new ListItem() { Text = "-No records found-", Value = "-1", Selected = true });
            }
            dropDownList.DataTextField = "Text";
            dropDownList.DataValueField = "Value";
            dropDownList.DataBind();
        }
        /// <summary>
        /// Binds the gridview with data source and also displays the label if no records found.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="gv"></param>
        /// <param name="dataSource"></param>
        /// <param name="lblNoRecords"></param>
        /// <param name="IsLabelVisible"></param>
        public void GridBind<T1>(GridView gv, List<T1> dataSource, Label lblNoRecords = null, bool IsLabelVisible = true, bool IsExportEnabled = true) where T1 : new()
        {
            if (lblNoRecords == null)
            { lblNoRecords = _contentPlaceHolder.FindControl("lblNoRecords") as Label; }
            var btnExport = _contentPlaceHolder.FindControl("btnExport") as Button;

            if (dataSource != null && dataSource.Count() != 0)
            {
                gv.DataSource = dataSource;
                gv.DataBind();
                gv.Visible = true;
                if (lblNoRecords != null)
                {
                    (lblNoRecords as Label).Visible = false;
                }
                if (btnExport != null && IsExportEnabled)
                {
                    btnExport.Visible = true;
                }
            }
            else
            {
                gv.Visible = false;
                if (lblNoRecords != null)
                {
                    var label = (lblNoRecords as Label);
                    label.Visible = IsLabelVisible;
                }
                if (btnExport != null && IsExportEnabled)
                {
                    btnExport.Visible = false;
                }
            }
        }
        /// <summary>
        /// Get the form data based on ViewModel
        /// ViewMode property should match with asp.net control ids
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetFormInputModel<T>() where T : new()
        {
            var QueryModel = new T();
            var typeOfViewModel = QueryModel.GetType();
            foreach (PropertyInfo propertyInfo in typeOfViewModel.GetProperties())
            {
                var control = _contentPlaceHolder.FindControl(propertyInfo.Name.ToString());
                dynamic objValue = null;
                #region Convert Control Type

                if (control is TextBox)
                {
                    objValue = (control as TextBox).Text;
                }
                else if (control is Label)
                {
                    objValue = (control as Label).Text;
                }
                else if (control is CheckBox)
                {
                    objValue = (control as CheckBox).Checked;
                }
                else if (control is DropDownList)
                {
                    if ((control as DropDownList).SelectedValue != "-1")
                    {
                        objValue = (control as DropDownList).SelectedValue;
                    }
                }
                else if (control is HiddenField)
                {
                    objValue = (control as HiddenField).Value;
                }
                #endregion
                #region Convert to Property Data Type
                if (objValue != null)
                {
                    if (propertyInfo.PropertyType == typeof(DateTime?) || propertyInfo.PropertyType == typeof(DateTime))
                    {
                        DateTime dateTime = new DateTime();
                        if (!string.IsNullOrEmpty(objValue) && DateTime.TryParse(objValue, out dateTime))
                        {
                            propertyInfo.SetValue(QueryModel, dateTime);
                        }
                        else
                        {
                            propertyInfo.SetValue(QueryModel, null);
                        }
                    }
                    else if (propertyInfo.PropertyType == typeof(string))
                    {
                        //if (objValue != "")
                        //{
                        propertyInfo.SetValue(QueryModel, (string)objValue);
                        // }
                    }
                    else if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?))
                    {
                        int value = 0;
                        if (objValue != "" && int.TryParse(objValue, out value))
                        {
                            propertyInfo.SetValue(QueryModel, value);
                        }
                    }
                    else if (propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(Nullable<bool>))
                    {
                        propertyInfo.SetValue(QueryModel, (bool)objValue);
                    }

                    #endregion
                }
            }

            return QueryModel;
        }
        /// <summary>
        /// Clears the entire form data from asp.net controls like textbox, dropdown,checkbox
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ClearForm<T>() where T : new()
        {
            var QueryModel = new T();
            var typeOfViewModel = QueryModel.GetType();
            foreach (PropertyInfo propertyInfo in typeOfViewModel.GetProperties())
            {
                var control = _contentPlaceHolder.FindControl(propertyInfo.Name.ToString());

                #region Convert Control Type
                if (control is TextBox)
                {
                    (control as TextBox).Text = string.Empty;
                }
                else if (control is CheckBox)
                {
                    (control as CheckBox).Checked = false;
                }
                else if (control is DropDownList)
                {
                    (control as DropDownList).ClearSelection();
                }
                #endregion
            }


        }
        /// <summary>
        /// Binds the viewModel data to Asp.net control labels only
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="readOnlyModel"></param>
        public void BindReadOnlyForm<T>(T readOnlyModel) where T : new()
        {
            //var QueryModel = new T();
            var typeOfViewModel = readOnlyModel.GetType();
            foreach (PropertyInfo propertyInfo in typeOfViewModel.GetProperties())
            {
                var control = _contentPlaceHolder.FindControl(propertyInfo.Name.ToString());

                #region Convert Control Type
                if (control != null && control is Label && propertyInfo.GetValue(readOnlyModel) != null)
                {
                    (control as Label).Text = propertyInfo.GetValue(readOnlyModel).ToString();
                }
                #endregion
            }


        }
        /// <summary>
        /// Hide the asp.net control with control ID
        /// </summary>
        /// <param name="htmlGenericControl"></param>
        public void HideHtmlControl(HtmlGenericControl htmlGenericControl)
        {
            htmlGenericControl.Visible = false;
        }
        /// <summary>
        /// Display asp.net controls based on Control ID
        /// </summary>
        /// <param name="htmlGenericControl"></param>
        public void DisplayHtmlControl(HtmlGenericControl htmlGenericControl)
        {
            htmlGenericControl.Visible = true;
        }


        public void BindFormTextBoxes<T>(T readOnlyModel) where T : new()
        {
            //var QueryModel = new T();
            var typeOfViewModel = readOnlyModel.GetType();
            foreach (PropertyInfo propertyInfo in typeOfViewModel.GetProperties())
            {
                var control = _contentPlaceHolder.FindControl(propertyInfo.Name.ToString());

                #region Convert Control Type
                if (control != null && control is Label && propertyInfo.GetValue(readOnlyModel) != null)
                {
                    (control as Label).Text = propertyInfo.GetValue(readOnlyModel).ToString();
                }
                else if (control != null && control is TextBox && propertyInfo.GetValue(readOnlyModel) != null)
                {
                    (control as TextBox).Text = propertyInfo.GetValue(readOnlyModel).ToString();
                }
                //else if (control != null && control is DropDownList && propertyInfo.GetValue(readOnlyModel) != null)
                //{
                //    control = control as DropDownList;
                //    if (control.dat.FindByValue(propertyInfo.GetValue(readOnlyModel).ToString()) != null)
                //    {
                //        control.Items.FindByValue(propertyInfo.GetValue(readOnlyModel).ToString()).Selected = true;
                //    }


                //}
                #endregion
            }


        }


        public void BindDropdownSelectedValue(DropDownList control, string selectedValue)
        {
            control.ClearSelection();
            if (control.Items.FindByValue(selectedValue) != null)
            {
                control.Items.FindByValue(selectedValue).Selected = true;
            }
            else if (control.Items.FindByText(selectedValue) != null)
            {
                control.Items.FindByText(selectedValue).Selected = true;
            }




        }
    }
}
