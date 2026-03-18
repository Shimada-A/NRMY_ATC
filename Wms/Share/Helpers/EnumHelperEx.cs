using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Share.Helpers
{
    public static class EnumHelperEx
    {
        /// <summary>
        /// Display属性の値を取得する
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDisplayValue<T>(T value)
        {
            if (!IsPossibleCastToEnum(value))
            {
                return string.Empty;
            }

            var fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null)
            {
                return string.Empty;
            }

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes != null && descriptionAttributes.Count() > 0)
            {
                if (descriptionAttributes[0].ResourceType != null)
                {
                    return LookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);
                }
                else
                {
                    return descriptionAttributes[0].Name ?? value.ToString();
                }
            }

            return string.Empty;
        }

        ///
        /// <returns></returns><summary>
        /// Enum<E>にキャストできるのかをチェック
        /// </summary>
        /// <typeparam name="TE"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPossibleCastToEnum<T>(T value)
        {
            var type = typeof(T);
            if (!type.IsEnum) return false;
            foreach (var item in Enum.GetValues(type))
            {
                if (((T)item).Equals(value))
                    return true;
            }

            return false;
        }

        public static string LookupResource(Type resourceManagerProvider, string resourceKey)
        {
            foreach (PropertyInfo staticProperty in resourceManagerProvider.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (staticProperty.PropertyType == typeof(System.Resources.ResourceManager))
                {
                    System.Resources.ResourceManager resourceManager = (System.Resources.ResourceManager)staticProperty.GetValue(null, null);
                    return resourceManager.GetString(resourceKey);
                }
            }

            return resourceKey; // Fallback with the key name
        }
    }
}
