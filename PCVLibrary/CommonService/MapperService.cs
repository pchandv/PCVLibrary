using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PCVLibrary.MapperService
{
    public class Mapper
    {
        /// <summary>
        /// Map will be used to bind the source and destination objects of the same structure or different but with same property names.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static void Map<T1, T2>(T1 source, T2 destination)
        {
            foreach (PropertyInfo propertyInfo in source.GetType().GetProperties())
            {
                if (destination.GetType().GetProperty(propertyInfo.Name) != null)
                { destination.GetType().GetProperty(propertyInfo.Name).SetValue(destination, propertyInfo.GetValue(source)); }
            }
        }
    }
}
