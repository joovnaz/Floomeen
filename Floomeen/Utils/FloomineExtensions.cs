using System;
using System.Linq;
using Floomeen.Exceptions;

namespace Floomeen.Utils
{
    public static class FloomeenExtensions
    {
        public static string GetPropNameByAttribute<TAttribute>(object obj) where TAttribute : Attribute
        {
            var fellowType = obj.GetType();

            var props = fellowType.GetProperties();

            foreach (var prop in props)
            {
                var attrs = prop.GetCustomAttributes(true);

                if (attrs.OfType<TAttribute>().Any())
                {
                    return prop.Name;
                }
            }

            return string.Empty;
        }

        public static object GetPropValueByAttribute<TAttribute>(object obj) where TAttribute : Attribute
        {
            var propertyName = GetPropNameByAttribute<TAttribute>(obj);

            return obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
        }

        public static Type GetPropTypeByAttribute<TAttribute>(object obj) where TAttribute : Attribute
        {
            var propertyName = GetPropNameByAttribute<TAttribute>(obj);

            return obj.GetType().GetProperty(propertyName)?.PropertyType;
        }
        
        public static void SetPropValueByAttribute<TAttribute>(object obj, object value) where TAttribute : Attribute
        {
            var propertyName = GetPropNameByAttribute<TAttribute>(obj);

            if (string.IsNullOrEmpty(propertyName))

                throw new FloomeenException($"UnsupportedPseudoProperty.MissingAttribute{typeof(TAttribute).FullName}");

            obj.GetType().GetProperty(propertyName)?.SetValue(obj, value, null);
        }
        
    }
}
