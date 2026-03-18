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
    public static class DropDownListCenterHelper
    {
        /// <summary>
        /// 汎用値のドロップダウンリストのHTMLを返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型。</typeparam>
        /// <typeparam name="TProperty">値の型。</typeparam>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式。</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="optionLabel">空要素のTEXT</param>
        /// <param name="htmlAttributes">この要素に設定する HTML属性を格納するオブジェクト。</param>
        /// <returns>汎用値のドロップダウンリストのHTML</returns>
        public static IHtmlString CenterDropDownFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,string userId, string optionLabel, object htmlAttributes)
        {
            // 汎用値取得
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
                SELECT
                    MC.CENTER_ID VALUE
                  , MC.CENTER_ID || ':' || MC.CENTER_NAME1 TEXT 
                FROM
                    M_CENTERS MC
                  , ( 
                    SELECT
                        CENTER_ID
                      , ( 
                        SELECT
                              1 
                        FROM
                              M_GENERALS MG 
                        WHERE
                              SHIPPER_ID = :SHIPPER_ID
                          AND GEN_DIV_CD = 'CENTER_CHANGE_LEVEL' 
                          AND MG.GEN_CD = TO_CHAR(MU.PERMISSION_LEVEL)
                      ) EXI 
                    FROM
                          M_USERS MU 
                    WHERE
                          SHIPPER_ID = :SHIPPER_ID
                      AND USER_ID = :USER_ID
                  ) INLINE 
                WHERE
                      MC.SHIPPER_ID = :SHIPPER_ID
                  AND MC.CENTER_ID = CASE 
                                        WHEN INLINE.EXI = 1 THEN MC.CENTER_ID 
                                        ELSE INLINE.CENTER_ID 
                                    END
                  AND MC.WMS_CLASS = '1'
                ORDER BY
                      MC.CENTER_ID
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":USER_ID", userId);

            System.Collections.Generic.IEnumerable<SelectListItem> centerList = MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters);
            if (string.IsNullOrWhiteSpace(optionLabel))
            {
                return SelectExtensions.DropDownListFor<TModel, TProperty>(htmlHelper, expression, centerList, htmlAttributes);
            }
            return SelectExtensions.DropDownListFor<TModel, TProperty>(htmlHelper, expression, centerList, optionLabel, htmlAttributes);
        }
    }
}