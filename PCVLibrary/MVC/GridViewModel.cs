using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PCVLibrary.MVCGrid
{
    /// <summary>
    /// Provide necessary inputs to create gridview model object
    /// </summary>
    public class GridViewModel : IGridViewModel
    {
        /// <summary>
        /// using Discriptor helper class to get name of controller , actionname
        /// and also column names
        /// </summary>
        /// <param name="ControllerName"></param>
        /// <param name="ActionName"></param>
        /// <param name="GridTitle"></param>
        /// <param name="Headers"></param>
        public GridViewModel(string ControllerName, string ActionName, string GridTitle, List<Column> Headers)
        {
            datasourceUrl = string.Format("/{0}/{1}", ControllerName, ActionName);
            this.GridTitle = GridTitle;
            this._Headers = Headers;
        }
        #region Local Variables
        int _CurrentPageIndex;
        string _GridPartialView = "../GridView/_GridView";
        private List<object> _dataSource;
        private int _MaxRows = 10;
        List<Column> _Headers;
        string _sortOrder = "A";
        #endregion

        #region Public Properties

        /// <summary>
        /// Provide name to GridView
        /// </summary>
        public string GridTitle { get; set; }

        public int CurrentPageIndex
        {
            get
            {
                return _CurrentPageIndex <= this.TotalPageCount ?
                       _CurrentPageIndex : this.TotalPageCount;
            }
            set { _CurrentPageIndex = value; }
        }
        public virtual int TotalPageCount
        {
            get
            {
                return (int)Math.Ceiling((double)((decimal)TotalRecords / MaxRows));
            }
        }
        public virtual List<object> DataSource
        {
            get
            {
                if (IsSortEnable)
                {
                    return SortDataSource().Skip((CurrentPageIndex - 1) * MaxRows)
                        .Take(MaxRows).ToList();
                }
                else
                {
                    return this._dataSource.Skip((CurrentPageIndex - 1) * MaxRows)
                       .Take(MaxRows).ToList();
                }
            }
            set { _dataSource = value; }
        }
        public virtual int TotalRecords { get { return _dataSource.Count(); } }
        public virtual int MaxRows { get { return _MaxRows; } set { _MaxRows = value; } }

        public string sortDirection
        {
            get
            {
                return _sortOrder == "A" ? "D" : "A";
            }
            set { _sortOrder = value; }
        }
        public string sortExpression { get; set; }
        public bool IsSortEnable { get; set; }

        public List<Column> Headers { get { return _Headers; } }
        public string datasourceUrl { get; set; }

        public int NextPage { get { return CurrentPageIndex + 1; } }
        public int PreviousPage { get { return CurrentPageIndex - 1; } }
        public string isPreviousDisabled
        {
            get
            {
                return (PreviousPage > 0 && this.PreviousPage <= this.TotalPageCount) ?
                        "" : "disabled";
            }
        }
        public string isNextDisabled
        {
            get
            {
                return (NextPage <= TotalPageCount) ? "" : "disabled";
            }
        }

        public string _PartialGridView { get { return _GridPartialView; } }

        #endregion

        #region Public Methods
        /// <summary>
        /// Sort data source based on sort experission and sort directions
        /// </summary>
        /// <returns></returns>
        public virtual List<object> SortDataSource()
        {

            List<object> data_sorted = new List<object>();
            //Ascending
            if (sortDirection == "A")
            {
                data_sorted = (from n in _dataSource
                               orderby GetPropValue(n, sortExpression) ascending
                               select n).ToList();
            }
            //Descending
            else if (sortDirection == "D")
            {
                data_sorted = (from n in _dataSource
                               orderby GetPropValue(n, sortExpression) descending
                               select n).ToList();

            }

            return data_sorted;

        }
        /// <summary>
        /// Using reflection to get value of property from given source object
        /// </summary>
        /// <param name="src"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public object GetPropValue(object src, string propName)
        {
            return !string.IsNullOrEmpty(propName) ? src.GetType().GetProperty(propName).GetValue(src, null) : null;
        }
        /// <summary>
        /// To override Partial View of gridView
        /// </summary>
        /// <param name="viewName"></param>
        public void SetPartialGridView(string viewName)
        {
            _GridPartialView = viewName;
        }
        /// <summary>
        /// Set Grid filters which contains currentpageindex, sort direction,sort expression
        /// </summary>
        /// <param name="filter"></param>
        public void SetGridFilters(GridViewFilter filter)
        {
            this._sortOrder = filter.sortDirection;
            this.sortExpression = filter.sortExpression;
            this.CurrentPageIndex = filter.currentPageIndex;
        }

        public void SetColumns(List<Column> columns)
        {
            if (columns != null && columns.Count() != 0)
            {
                _Headers = columns;
            }
        }
        #endregion
    }

    public interface IGridViewModel
    {
        void SetGridFilters(GridViewFilter filter);
        /// <summary>
        /// Generic List of type object and this can set as below
        ///  var vm = new List<Object>();
        ///  vm.Add(new EmployeeVM() { EmpID = i, FirstNames = "Test", LastName = "Visam" });
        /// </summary>
        List<object> DataSource { get; set; }
        /// <summary>
        /// Current page index and it is default to 0
        /// this is will be incremented or decremeted based on NextPageIndex and PreviousPageIndex
        /// Handled in GridView.cshtml
        /// </summary>
        int CurrentPageIndex { get; set; }
        /// <summary>
        /// Returns total number of pages
        /// </summary>
        int TotalPageCount { get; }
        /// <summary>
        /// Returns total records in datasource
        /// </summary>
        int TotalRecords { get; }
        /// <summary>
        /// Page Size cane be set to MaxRows and default value is 10
        /// </summary>
        int MaxRows { get; set; }
        /// <summary>
        /// A - Ascending order
        /// D - Descending Order
        /// these values are hard code and will switched when IsSortEnable set to true
        /// </summary>
        string sortDirection { get; }
        /// <summary>
        /// Provide Sort expression like ViewModel property name
        /// </summary>
        string sortExpression { get; set; }
        /// <summary>
        /// Default false and can be used to sort the gridvew with hyper link.
        /// </summary>
        bool IsSortEnable { get; set; }
        /// <summary>
        /// Sorting datasource based on sort experission and direction
        /// </summary>
        /// <returns></returns>
        List<object> SortDataSource();

        /// <summary>
        /// To set Column names  to this grid follow below example
        /// var mp = new Descriptor<EmployeeVM>();
        //  List<Column> Headers = new List<Column>();
        //  Headers.Add(new Column() { DisplayName = "Employee First Name", Name = mp.Name(x => x.FirstNames) });
        //  Headers.Add(new Column() { DisplayName = "Employee Last Name", Name = mp.Name(x => x.LastName) });
        //  Headers.Add(new Column() { DisplayName = "Employee ID", Name = mp.Name(x => x.EmpID)});
        //  return Headers;
        /// </summary>
        void SetColumns(List<Column> columns);


    }
    /// <summary>
    /// GridView Column type and should provide display name and class property name of ViewModel
    /// </summary>
    public class Column
    {
        /// <summary>
        /// Name is class property name  
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// DisplayName is Column  display name
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// set to true of column should be hide during rendering
        /// </summary>
        public bool isHide { get; set; }

    }
    /// <summary>
    /// This is generic class to get class name,property name,method names
    /// instead of hard coding of class name this can be used.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Descriptor<T>
    {
        /// <summary>
        /// Returns Property Name of type T
        /// </summary>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="propertySelector"></param>
        /// <returns></returns>
        public string Name<TProp>(Expression<Func<T, TProp>> propertySelector)
        {
            MemberExpression body = (MemberExpression)propertySelector.Body;
            return body.Member.Name;
        }
        /// <summary>
        /// Returns Controller Name or Class name of T
        /// </summary>
        /// <returns></returns>
        public string ControllerName
        {
            get
            {
                return typeof(T).Name.Replace("Controller", "");
            }
        }
        /// <summary>
        /// Returns Action name of Controller type T
        /// </summary>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="propertySelector"></param>
        /// <returns></returns>
        public string ActionName<TProp>(Expression<Func<T, TProp>> propertySelector)
        {
            MethodCallExpression body = (MethodCallExpression)propertySelector.Body;
            return body.Method.Name;
        }
    }

    /// <summary>
    /// GridViewfilter is used as input for controller action when gridview model is returned to view
    /// GridViewfilter values will be loaded from jquery in _GridView.cshtml
    /// </summary>
    public class GridViewFilter
    {
        public string sortDirection { get; set; }
        public string sortExpression { get; set; }
        public int currentPageIndex { get; set; }
    }

}