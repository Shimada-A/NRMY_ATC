namespace Share.Helpers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    /// <summary>
    /// Modelプロパティのメタデータヘルパー
    /// </summary>
    public static class MetaDataHelper
    {
        /// <summary>
        /// Display属性の表示名称をリソース値で取得する
        /// </summary>
        /// <param name="dataType">型</param>
        /// <param name="fieldName">プロパティ名</param>
        /// <returns></returns>
        public static string GetDisplayName(Type dataType, string fieldName)
        {
            var prop = dataType.GetTypeInfo().GetProperty(fieldName);
            var displayAttribute = prop.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
            if (displayAttribute != null)
            {
                return displayAttribute.GetName();
            }

            return string.Empty;
        }

        /// <summary>
        /// Display属性のOrder値を取得する
        /// </summary>
        /// <param name="dataType">型</param>
        /// <param name="fieldName">プロパティ名</param>
        /// <returns></returns>
        public static int GetOrderNo(Type dataType, string fieldName)
        {
            var prop = dataType.GetTypeInfo().GetProperty(fieldName);
            var displayAttribute = prop.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
            if (displayAttribute != null)
            {
                return displayAttribute.Order;
            }

            return -1;
        }
    }
}