namespace Wms.Areas.Master.Query.Division
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Wms.Areas.Master.ViewModels.Division;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.Division.DivisionSearchCondition;

    public class DivisionQuery
    {
        #region Index
        /// <summary>
        /// Get Country List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<Models.Division> GetData(DivisionSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                 SELECT MD.DIVISION_ID
                       ,MD.DIVISION_NAME
                       ,MD.DELETE_FLAG
                  FROM M_DIVISIONS MD
                 WHERE MD.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            if (!string.IsNullOrEmpty(condition.DivisionId))
            {
                query.Append(" AND MD.DIVISION_ID LIKE :DIVISION_ID ");
                parameters.Add(":DIVISION_ID", condition.DivisionId + "%");
            }

            if (!string.IsNullOrEmpty(condition.DivisionName))
            {
                query.Append(@" AND MD.DIVISION_NAME LIKE :DIVISION_NAME");
                parameters.Add(":DIVISION_NAME", "%" + condition.DivisionName + "%");
            }

            // 削除フラグfalseの時は削除フラグ1は非表示
            if (!condition.DeleteFlag)
            {
                query.Append(" AND MD.DELETE_FLAG <> 1");
            }

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<Models.Division>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case DivisionSortKey.DivisionName:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@" 
                                               ORDER BY MD.DIVISION_NAME DESC,MD.DIVISION_ID DESC");
                            break;

                        default:
                            query.AppendLine(@" 
                                               ORDER BY MD.DIVISION_NAME ASC,MD.DIVISION_ID ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@" 
                                               ORDER BY MD.DIVISION_ID DESC,MD.DIVISION_NAME DESC");
                            break;

                        default:
                            query.AppendLine(@" 
                                               ORDER BY MD.DIVISION_ID ASC,MD.DIVISION_NAME ASC");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var Divisions = MvcDbContext.Current.Database.Connection.Query<Models.Division>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<Models.Division>(Divisions, condition.Page, condition.PageSize, totalCount);
        }
        #endregion

        #region Detail
        /// <summary>
        /// Get Color By Id
        /// </summary>
        /// <param name="itemDivisionId">storeId</param>
        /// <param name="shipperId">shipperId</param>
        /// <returns>Country</returns>
        public Detail GetTargetById(string itemDivisionId, string shipperId)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                       MD.UPDATE_COUNT
                      ,MD.SHIPPER_ID
                      ,MD.DIVISION_ID
                      ,MD.DIVISION_NAME
                      ,MD.DIVISION_MARK
                      ,MD.DELETE_FLAG
                  FROM M_DIVISIONS MD
                 WHERE MD.SHIPPER_ID = :SHIPPER_ID
                   AND MD.DIVISION_ID = :DIVISION_ID
            ");
            parameters.Add(":SHIPPER_ID", shipperId);
            parameters.Add(":DIVISION_ID", itemDivisionId);
            return MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters).FirstOrDefault();
        }
        #endregion

    }
}