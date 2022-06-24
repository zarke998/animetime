using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Utilities.Collections
{
    public static class IEnumerableExtensions
    {
        public static List<T> MapToList<T>(this IEnumerable enumerable) where T : class, new()
        {
            var list = new List<T>();
            if(enumerable == null)
                throw new ArgumentNullException($"Argument {nameof(enumerable)} cannot be null.");


            PropertyInfo[] sourceProperties = null;
            PropertyInfo[] targetProperties = typeof(T).GetProperties();
            foreach (var item in enumerable)
            {
                if(sourceProperties == null)
                    sourceProperties = item.GetType().GetProperties();

                var resObj = new T();
                foreach (var sourceProperty in sourceProperties)
                {
                    var targetProperty = targetProperties.FirstOrDefault(p => p.Name == sourceProperty.Name);
                    if (targetProperty == null || targetProperty.PropertyType != sourceProperty.PropertyType)
                        continue;

                    var value = sourceProperty.GetValue(item);
                    targetProperty.SetValue(resObj, value);
                }

                list.Add(resObj);
            }

            return list;
        }
    }
}
