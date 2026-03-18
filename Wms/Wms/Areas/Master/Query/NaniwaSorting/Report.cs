namespace Wms.Areas.Master.Query.NaniwaSorting
{
    using System.Collections.Generic;
    using System.Text;
    using Dapper;
    using Wms.Areas.Master.ViewModels.NaniwaSorting;
    using Wms.Models;
    using Wms.Query;

    public class Report : BaseQuery
    {
        private NaniwaSortingQuery _NaniwaSortingQuery;

        /// <summary>
        /// Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<ViewModels.NaniwaSorting.Report> NaniwaSortingListing(NaniwaSortingSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();

            //一覧表示のSQLを取得
            NaniwaSortingQuery.GetQuery(condition, ref query, ref parameters);

            return MvcDbContext.Current.Database.Connection.Query<ViewModels.NaniwaSorting.Report>(query.ToString(), parameters);
        }
    }
}