namespace Share.Helpers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Resources;
    using System.Web;
    using System.Web.Mvc;

    public static class DisplayEnumHelper
    {
        /// <summary>
        /// Enumの名称を表示
        /// </summary>
        /// <param name="helper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="value">格納しているオブジェクト</param>
        /// <returns></returns>
        public static IHtmlString DisplayEnum(this HtmlHelper helper, object value)
        {
            if (value == null) return MvcHtmlString.Create(string.Empty);
            string name = Enum.GetName(value.GetType(), value);

            if (string.IsNullOrEmpty(name))
            {
                return MvcHtmlString.Create(value.ToString());
            }

            var attribute = value.GetType()
                .GetField(name)
                .GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false)
                .Cast<System.ComponentModel.DataAnnotations.DisplayAttribute>()
                .FirstOrDefault();

            if (attribute != null)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                ResourceManager rm = new ResourceManager(attribute.ResourceType.FullName, assembly);
                string s = rm.GetString(attribute.Name);
                return MvcHtmlString.Create(s);
            }

            return MvcHtmlString.Create(value.ToString());
        }

        /// <summary>
        /// Enumの名称を表示
        /// </summary>
        /// <typeparam name="TModel">モデルの型。</typeparam>
        /// <param name="helper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式。</param>
        /// <returns></returns>
        public static IHtmlString DisplayEnumFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            if (helper == null)
                throw new ArgumentNullException(nameof(helper));

            object value = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model;
            return DisplayEnum(helper, value);
        }
    }
}