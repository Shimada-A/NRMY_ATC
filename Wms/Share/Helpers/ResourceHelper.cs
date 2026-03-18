namespace Share.Helpers
{
    using System.Resources;
    using System.Web.Mvc;

    /// <summary>
    /// ラジオボタンのヘルパー
    /// </summary>
    public static class ResourceHelper
    {
        /// <summary>
        /// Get menu resource
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="resourceFileName"></param>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        public static string MenusResource(this HtmlHelper helper, string resourceFileName, string resourceKey)
        {
            // string culture = CultureInfo.CurrentCulture.ToString();
            // if (culture.Contains("ja"))
            // {
            //    resourceFileName += ".ja";
            // }
            // else if (culture.Contains("zh"))
            // {
            //    resourceFileName += ".zh";
            // }
            ResourceManager resourceManager = new ResourceManager("Wms.Common.Resources.Menu", System.Reflection.Assembly.GetExecutingAssembly());
            return resourceManager.GetString(resourceKey);
        }
    }
}