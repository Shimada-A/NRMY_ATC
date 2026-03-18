namespace Share.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Enumのヘルパー
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class EnumHelper<T>
    {
        public static IList<T> GetValues(Enum value)
        {
            var enumValues = new List<T>();

            foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
            }

            return enumValues;
        }

        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static IList<string> GetNames(Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        public static IList<string> GetDisplayValues(Enum value)
        {
            return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }

        private static string LookupResource(Type resourceManagerProvider, string resourceKey)
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

        /// <summary>
        /// Display属性の値を取得する
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDisplayValue(T value)
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
        public static bool IsPossibleCastToEnum<TE>(TE value)
        {
            var type = typeof(T);
            if (!type.IsEnum) return false;
            foreach (var item in Enum.GetValues(type))
            {
                if (((TE)item).Equals(value))
                    return true;
            }

            return false;
        }
    }
}