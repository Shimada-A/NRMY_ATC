namespace Wms.Extensions.Classes
{
    using System.Linq;
    using System.Web.Mvc;
    using Wms.Models;

    /// <summary>
    /// ModelStateDictionary拡張メソッドクラス
    /// </summary>
    public static class ModelStateDictionaryExtensions
    {
        /// <summary>
        /// BaseModelで保持している項目を検証対象外にする
        /// </summary>
        /// <param name="list">モデルバインドされた結果リスト</param>
        /// <returns>BaseModel項目を除外したModelState</returns>
        public static ModelStateDictionary RemoveBase(this ModelStateDictionary list)
        {
            System.Reflection.PropertyInfo[] properties = typeof(BaseModel).GetProperties();
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                // List引数のモデルバインドをする際に、name属性"モデル名[0].MakeUserId"の様にセットされるため、EndsWithも条件に含めています
                System.Collections.Generic.List<string> target = list.Keys.Where(m => m == property.Name || m.EndsWith("." + property.Name)).ToList();
                foreach (string name in target)
                {
                    list.Remove(name);
                }
            }

            return list;
        }
    }
}