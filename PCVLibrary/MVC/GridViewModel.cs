using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PCVLibrary.MVCGrid
{
    public class GridViewModel : IGridViewModel
    {
        public GridViewModel(string ControllerName, string ActionName, string GridTitle,List<Column> Headers)
        {
            datasourceUrl = string.Format("/{0}/{1}", ControllerName, ActionName);
            //this.CurrentPageIndex = currentPageIndex;
            this.GridTitle = GridTitle;
            this._Headers = Headers;
        }
        public string GridTitle { get; set; }


        int _CurrentPageIndex;
        string _GridPartialView = "../GridView/_GridView";
        private List<object> _dataSource;
        private int _MaxRows = 10;
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
                    return Sort_List().Skip((CurrentPageIndex - 1) * MaxRows)
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
        string _sortOrder = "A";
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
        public virtual List<object> Sort_List()
        {

            List<object> data_sorted = new List<object>();
            //Ascending
            if (sortDirection == "A")
            {
                data_sorted = (from n in _dataSource
                               orderby GetDynamicSortProperty(n, sortExpression) ascending
                               select n).ToList();
            }
            //Descending
            else if (sortDirection == "D")
            {
                data_sorted = (from n in _dataSource
                               orderby GetDynamicSortProperty(n, sortExpression) descending
                               select n).ToList();

            }

            return data_sorted;

        }
        private object GetDynamicSortProperty(object item, string propName)
        {
            if (!string.IsNullOrEmpty(propName))
            {
                //Use reflection to get order type
                return item.GetType().GetProperty(propName).GetValue(item, null);
            }
            return null;
        }
        List<Column> _Headers;
        public List<Column> Headers { get { return _Headers; }  }
        public string datasourceUrl { get; set; }
        public object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
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
        public void SetPartialGridView(string viewName)
        {
            _GridPartialView = viewName;
        }


        public void SetGridFilters(GridViewFilter filter)
        {
            this._sortOrder = filter.sortDirection;
            this.sortExpression = filter.sortExpression;
            this.CurrentPageIndex = filter.currentPageIndex;
        }


    }

    public interface IGridViewModel
    {
        List<object> DataSource { get; set; }
        int CurrentPageIndex { get; set; }
        int TotalPageCount { get; }
        int TotalRecords { get; }
        int MaxRows { get; set; }
        string sortDirection { get;  }
        string sortExpression { get; set; }
        bool IsSortEnable { get; set; }
        List<object> Sort_List();

    }

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
        public string ControllerName()
        {
            return typeof(T).Name.Replace("Controller", "");
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


    public class GridViewFilter
    {
        public string sortDirection { get; set; }
        public string sortExpression { get; set; }
        public int currentPageIndex { get; set; }
    }

}