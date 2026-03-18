namespace Wms.Areas.Inventory.Query.Start
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Common;
    using Share.Extensions.Classes;
    using Share.Helpers;
    using Wms.Areas.Inventory.Resources;
    using Wms.Areas.Inventory.ViewModels.Start;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Inventory.ViewModels.Start.StartSearchConditions;

    public class StartQuery
    {
        /// <summary>
        /// Get StockInquiry List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<StartResultRow> GetData(StartSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                WITH
                    SELETED_NVENTORY AS (
                        SELECT
                                ML.INVENTORY_NO
                            ,   ML.SHIPPER_ID
                            ,   ML.CENTER_ID
                            ,   ML.INVENTORY_START_DATE
                            ,   ML.INVENTORY_NAME
                            ,   MIN(NVL(TIP.INVENTORY_CLASS, 2)) AS INVENTORY_CLASS
                            ,   MAX(NVL(TIP.SIMPLE_INVENTORY_FLAG, 0)) AS SIMPLE_INVENTORY_FLAG
                        FROM
                                M_LOCATIONS ML
                        LEFT OUTER JOIN
                                T_INVENTORY_PLANS TIP
                        ON
                                TIP.INVENTORY_NO = ML.INVENTORY_NO
                            AND TIP.LOCATION_CD = ML.LOCATION_CD
                            AND TIP.SHIPPER_ID = ML.SHIPPER_ID
                            AND TIP.CENTER_ID = ML.CENTER_ID
                        WHERE
                                ML.SHIPPER_ID = :SHIPPER_ID
                            AND ML.CENTER_ID  = :CENTER_ID
                            AND ML.INVENTORY_CONFIRM_FLAG <> 3
                            AND ML.INVENTORY_CONFIRM_FLAG > 0
                            AND ML.INVENTORY_NO IS NOT NULL
                        GROUP BY
                                ML.INVENTORY_NO
                            ,   ML.INVENTORY_START_DATE
                            ,   ML.INVENTORY_NAME
                            ,   ML.CENTER_ID
                            ,   ML.SHIPPER_ID
                )
                SELECT
                        SLD.INVENTORY_NO
                    ,   SLD.INVENTORY_START_DATE
                    ,   SLD.INVENTORY_NAME
                    ,   MG.GEN_NAME INVENTORY_CLASS_NAME
                    ,   CASE
                            WHEN SLD.SIMPLE_INVENTORY_FLAG = 0 THEN '" + StartResource.SimpleIInventoryFlag0 + @"'
                            WHEN SLD.SIMPLE_INVENTORY_FLAG = 1 THEN '" + StartResource.SimpleIInventoryFlag1 + @"'
                            ELSE ''
                        END SIMPLE_INVENTORY_FLAG_NAME
                FROM
                        SELETED_NVENTORY SLD
                LEFT OUTER JOIN
                        M_GENERALS MG
                ON
                        SLD.SHIPPER_ID = MG.SHIPPER_ID
                    AND MG.REGISTER_DIVI_CD = '1'
                    AND MG.CENTER_ID = '@@@'
                    AND MG.GEN_DIV_CD = 'INVENTORY_CLASS'
                    AND TO_CHAR(SLD.INVENTORY_CLASS) = MG.GEN_CD
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<StartResultRow>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.Key)
            {
                case SortKey.InventoryStartDateNo:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY SLD.INVENTORY_START_DATE DESC,SLD.INVENTORY_NO DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY SLD.INVENTORY_START_DATE ASC,SLD.INVENTORY_NO ASC");
                            break;
                    }

                    break;

                case SortKey.InventoryNameNo:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY SLD.INVENTORY_NAME DESC,SLD.INVENTORY_NO DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY SLD.INVENTORY_NAME ASC,SLD.INVENTORY_NO ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY SLD.INVENTORY_NO DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY SLD.INVENTORY_NO ASC");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var Starts = MvcDbContext.Current.Database.Connection.Query<StartResultRow>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<StartResultRow>(Starts, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// 実績更新
        /// </summary>
        public void InventoryStart(StartSearchConditions searchConditions, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", searchConditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_INVENTORY_CLASS", searchConditions.InventoryClass, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_SIMPLE_INVENTORY_FLAG", searchConditions.SimpleIInventoryFlag?1:0, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_SEQ", searchConditions.Seq, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_FILE_NAME", string.IsNullOrWhiteSpace(searchConditions.FileName) ? searchConditions.FileName : searchConditions.FileName.TrimEnd(".xlsx".ToCharArray()), DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_INV_START",
                param,
                commandType: CommandType.StoredProcedure);

            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }
    }
}