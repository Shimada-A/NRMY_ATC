namespace Wms.Helpers
{
    using System;
    using System.Linq.Expressions;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Dapper;
    using Wms.Models;

    /// <summary>
    /// ドロップダウンヘルパー
    /// </summary>
    public static class DropDownListGeneralHelper
    {
        /// <summary>
        /// 汎用値のドロップダウンリストのHTMLを返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型。</typeparam>
        /// <typeparam name="TProperty">値の型。</typeparam>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式。</param>
        /// <param name="genDivCd">汎用分類</param>
        /// <param name="centerId">センターコード</param>
        /// <param name="optionLabel">空要素のTEXT</param>
        /// <param name="htmlAttributes">この要素に設定する HTML属性を格納するオブジェクト。</param>
        /// <returns>汎用値のドロップダウンリストのHTML</returns>
        public static IHtmlString GeneralDropDownFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string genDivCd, string centerId, string optionLabel, object htmlAttributes)
        {
            // 汎用値取得
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
                SELECT GEN_CD VALUE
                      ,GEN_NAME TEXT
                  FROM M_GENERALS
                  WHERE
                       GEN_DIV_CD = :GEN_DIV_CD
                    AND SHIPPER_ID = :SHIPPER_ID
                    AND ( CENTER_ID = '@@@' OR CENTER_ID = :CENTER_ID)
                    AND REGISTER_DIVI_CD = '1'
                  ORDER BY ORDER_NO
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":GEN_DIV_CD", genDivCd);
            parameters.Add(":CENTER_ID", centerId);

            System.Collections.Generic.IEnumerable<SelectListItem> generalList = MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters);
            if (string.IsNullOrWhiteSpace(optionLabel))
            {
                return SelectExtensions.DropDownListFor<TModel, TProperty>(htmlHelper, expression, generalList, htmlAttributes);
            }
            return SelectExtensions.DropDownListFor<TModel, TProperty>(htmlHelper, expression, generalList, optionLabel, htmlAttributes);
        }
    }
}