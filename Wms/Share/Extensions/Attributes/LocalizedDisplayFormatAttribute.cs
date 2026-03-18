namespace Share.Extensions.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    /// <summary>
    /// 日付書式の多言語対応属性
    /// </summary>
    public class LocalizedDisplayFormatAttribute : DisplayFormatAttribute
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="resourceKey">リソースキー</param>
        /// <param name="resourceType">リソースタイプ</param>
        public LocalizedDisplayFormatAttribute(string resourceKey, Type resourceType) : base()
        {
            var propertyInfo = resourceType.GetProperty(resourceKey, BindingFlags.Static | BindingFlags.Public);
            if (propertyInfo == null) return;
            base.DataFormatString = (string)propertyInfo.GetValue(propertyInfo.DeclaringType, null);
            base.ApplyFormatInEditMode = true;
        }
    }
}