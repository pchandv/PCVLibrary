using System;
using System.Collections.Generic;
using System.Linq;

namespace PCVLibrary.MVCGrid
{
    public abstract class PageViewModel<T> : IPageViewModel<T>
    {
        public PageViewModel()
        {
            _dataSource = new List<T>();
        }
        private List<T> _dataSource;
        private int _MaxRows = 10;
        public int CurrentPageIndex { get; set; }
        public virtual int PageCount
        {
            get
            {
                return (int)Math.Ceiling((double)((decimal)TotalRecords / MaxRows));
            }
        }
        public virtual List<T> DataSource
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

        public string sortDirection { get; set; } = "A";
        public string sortExpression { get; set; }
        public bool IsSortEnable { get; set; }

       public virtual List<T> Sort_List()
        {

            List<T> data_sorted = new List<T>();
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
            //Use reflection to get order type
            return item.GetType().GetProperty(propName).GetValue(item, null);
        }
    }

    public interface IPageViewModel<T>
    {
        List<T> DataSource { get; set; }
        int CurrentPageIndex { get; set; }
        int PageCount { get; }
        int TotalRecords { get; }
        int MaxRows { get; set; }
        string sortDirection { get; set; }
        string sortExpression { get; set; }
        bool IsSortEnable { get; set; }
        List<T> Sort_List();
    }
}