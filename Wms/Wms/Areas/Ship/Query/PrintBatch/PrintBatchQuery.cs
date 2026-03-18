namespace Wms.Areas.Ship.Query.PrintBatch
{
    using System;
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
    using Wms.Areas.Ship.Models;
    using Wms.Areas.Ship.ViewModels.PrintBatch;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.PrintBatch.PrintBatchSearchConditions;

    public class PrintBatchQuery
    {
        /// <summary>
        /// Get List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IList<PrintBatchResultRow> GetData(PrintBatchSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
                SELECT TO_NUMBER(MAX(SHIP_KIND)) SHIP_KIND
                      ,TO_NUMBER(MAX(PICK_KIND)) PICK_KIND
                  FROM T_ALLOC_INFO
                  WHERE SHIPPER_ID = :SHIPPER_ID
                    AND CENTER_ID = :CENTER_ID
                   AND ALLOC_GROUP_NO = :ALLOC_GROUP_NO
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":ALLOC_GROUP_NO", condition.AllocGroupNo);
            var KindS = MvcDbContext.Current.Database.Connection.Query(query.ToString(), parameters).FirstOrDefault();
            condition.ShipKind = int.Parse(KindS.SHIP_KIND.ToString());
            condition.PickKind = int.Parse(KindS.PICK_KIND.ToString());
            StringBuilder query2 = new StringBuilder(@"
                SELECT
                        1 DISP_NO
                    ,   1 NO
                    ,   CASE WHEN MAX(SHIPPER_ID) IS NOT NULL THEN 1 ELSE 0 END ACTIVE
                    ,   'バッチカンバン' REPORT_NAME
                    ,   CASE 
                            WHEN MAX(BATCHLIST_PRN_FLAG) = 0 THEN '未出力'
                            WHEN MIN(BATCHLIST_PRN_FLAG) = 1 THEN '出力済'
                            WHEN MIN(BATCHLIST_PRN_FLAG) = 0 AND MAX(BATCHLIST_PRN_FLAG) = 1 THEN '一部出力済'
                            ELSE '－'
                        END STATUS_NAME
                FROM 
                        T_ALLOC_INFO
                WHERE 
                        SHIPPER_ID = :SHIPPER_ID
                    AND CENTER_ID = :CENTER_ID
                    AND ALLOC_GROUP_NO = :ALLOC_GROUP_NO
                    AND SHIP_KIND = 2 ");

            if (condition.PickKind == 1){
                query2.Append(@"
                UNION
                SELECT 
                        2 DISP_NO
                    ,   2 NO
                    ,   CASE WHEN MAX(TP.SHIPPER_ID) IS NOT NULL THEN 1 ELSE 0 END ACTIVE
                    ,   'トータルピッキングリスト' REPORT_NAME
                    ,   CASE 
                            WHEN MAX(TP.LIST_PRN_FLAG) = 0 THEN '未出力'
                            WHEN MIN(TP.LIST_PRN_FLAG) = 1 THEN '出力済'
                            WHEN MIN(TP.LIST_PRN_FLAG) = 0 AND MAX(TP.LIST_PRN_FLAG) = 1 THEN '一部出力済'
                            ELSE '－' 
                        END STATUS_NAME
                FROM
                        T_PIC TP
                INNER JOIN 
                        T_ALLOC_INFO TAI
                    ON
                        TP.SHIPPER_ID = TAI.SHIPPER_ID
                    AND TP.CENTER_ID = TAI.CENTER_ID
                    AND TP.BATCH_NO = TAI.ALLOC_NO
                    AND TAI.SHIP_KIND = 2
                    AND TAI.ALLOC_GROUP_NO = :ALLOC_GROUP_NO
                WHERE
                        TP.SHIPPER_ID = :SHIPPER_ID
                    AND TP.CENTER_ID = :CENTER_ID
                UNION
                SELECT 
                        3 DISP_NO
                    ,   10 NO
                    ,   0 ACTIVE
                    ,   '店別ピッキングリスト' REPORT_NAME
                    ,   '－' STATUS_NAME
                FROM
                        DUAL
                ");

            }else if (condition.PickKind == 5){
                query2.Append(@"
                UNION
                SELECT 
                        2 DISP_NO
                    ,   2 NO
                    ,   0 ACTIVE
                    ,   'トータルピッキングリスト' REPORT_NAME
                    ,   '－' STATUS_NAME
                FROM
                        DUAL
                UNION
                SELECT 
                        3 DISP_NO
                    ,   10 NO
                    ,   CASE WHEN MAX(TP.SHIPPER_ID) IS NOT NULL THEN 1 ELSE 0 END ACTIVE
                    ,   '店別ピッキングリスト' REPORT_NAME
                    ,   CASE
                            WHEN MAX(TP.LIST_PRN_FLAG) = 0 THEN '未出力'
                            WHEN MIN(TP.LIST_PRN_FLAG) = 1 THEN '出力済'
                            WHEN MIN(TP.LIST_PRN_FLAG) = 0 AND MAX(TP.LIST_PRN_FLAG) = 1 THEN '一部出力済'
                            ELSE '－' 
                        END STATUS_NAME
                FROM
                        T_ORDER_PIC TP
                INNER JOIN 
                        T_ALLOC_INFO TAI
                    ON 
                        TP.SHIPPER_ID = TAI.SHIPPER_ID
                    AND TP.CENTER_ID = TAI.CENTER_ID
                    AND TP.BATCH_NO = TAI.ALLOC_NO
                    AND TAI.SHIP_KIND = 2
                    AND TAI.ALLOC_GROUP_NO = :ALLOC_GROUP_NO
                WHERE
                        TP.SHIPPER_ID = :SHIPPER_ID
                    AND TP.CENTER_ID = :CENTER_ID
                ");
            } 

              query2.Append(@"
                UNION
                SELECT 
                        4 DISP_NO
                    ,   6 NO
                    ,  0 ACTIVE
                    , 'ケース出荷　ケースピッキングリスト' REPORT_NAME
                    , '－' STATUS_NAME
                FROM
                        DUAL
                UNION
                SELECT
                        5 DISP_NO
                    ,   7 NO
                    ,   0 ACTIVE
                    ,   'ケース出荷　JAN抜き取りピッキングリスト' REPORT_NAME
                    ,   '－' STATUS_NAME
                FROM 
                        DUAL
                UNION
                SELECT 
                        6 DISP_NO
                    ,   8 NO
                    ,CASE WHEN MAX(SHIPPER_ID) IS NOT NULL THEN 1 ELSE 0 END  ACTIVE
                    ,'摘取用カンバン' REPORT_NAME
                    ,CASE WHEN MAX(LIST2_PRN_FLAG) = 0 THEN '未出力'
                            WHEN MIN(LIST2_PRN_FLAG) = 1 THEN '出力済'
                            WHEN MIN(LIST2_PRN_FLAG) = 0 AND MAX(LIST2_PRN_FLAG) = 1 THEN '一部出力済'
                            ELSE '－' END STATUS_NAME
                FROM 
                        T_PIC
                WHERE 
                        SHIPPER_ID = :SHIPPER_ID
                    AND CENTER_ID = :CENTER_ID
                    AND BATCH_NO = :ALLOC_GROUP_NO
                    AND PICK_KIND = 1
            ");

            StringBuilder query3 = new StringBuilder(@"
                SELECT 1 NO
                      ,CASE WHEN MAX(SHIPPER_ID) IS NOT NULL THEN 1 ELSE 0 END  ACTIVE
                      ,'バッチカンバン' REPORT_NAME
                      ,CASE WHEN MAX(BATCHLIST_PRN_FLAG) = 0 THEN '未出力'
                            WHEN MIN(BATCHLIST_PRN_FLAG) = 1 THEN '出力済'
                            WHEN MIN(BATCHLIST_PRN_FLAG) = 0 AND MAX(BATCHLIST_PRN_FLAG) = 1 THEN '一部出力済'
                            ELSE '－' END STATUS_NAME
                  FROM T_ALLOC_INFO
                 WHERE SHIPPER_ID = :SHIPPER_ID
                   AND CENTER_ID = :CENTER_ID
                   AND ALLOC_GROUP_NO = :ALLOC_GROUP_NO
                   AND SHIP_KIND = 3 ");
            if (KindS.PICK_KIND == 1)
            {
                query3.Append(@"
                UNION
                SELECT 2 NO
                      ,CASE WHEN MAX(TP.SHIPPER_ID) IS NOT NULL THEN 1 ELSE 0 END  ACTIVE
                      ,'トータルピッキングリスト' REPORT_NAME
                      ,CASE WHEN MAX(TP.LIST_PRN_FLAG) = 0 THEN '未出力'
                            WHEN MIN(TP.LIST_PRN_FLAG) = 1 THEN '出力済'
                            WHEN MIN(TP.LIST_PRN_FLAG) = 0 AND MAX(TP.LIST_PRN_FLAG) = 1 THEN '一部出力済'
                            ELSE '－' END STATUS_NAME
                  FROM T_PIC TP
                 INNER JOIN T_ALLOC_INFO TAI
                    ON TP.SHIPPER_ID = TAI.SHIPPER_ID
                   AND TP.CENTER_ID = TAI.CENTER_ID
                   AND TP.BATCH_NO = TAI.ALLOC_NO
                   AND TAI.SHIP_KIND = 3
                   AND TAI.ALLOC_GROUP_NO = :ALLOC_GROUP_NO
                 WHERE TP.SHIPPER_ID = :SHIPPER_ID
                   AND TP.CENTER_ID = :CENTER_ID
                UNION
                SELECT 3 NO
                      ,0 ACTIVE
                      ,'ECバッチ単位ピッキングリスト' REPORT_NAME
                      ,'－' STATUS_NAME
                  FROM DUAL");
            }
            else
            {
                query3.Append(@"
                UNION
                SELECT 2 NO
                      ,0 ACTIVE
                      ,'トータルピッキングリスト' REPORT_NAME
                      ,'－' STATUS_NAME
                  FROM DUAL
                UNION
                SELECT 3 NO
                      ,CASE WHEN MAX(TP.SHIPPER_ID) IS NOT NULL THEN 1 ELSE 0 END  ACTIVE
                      ,'ECバッチ単位ピッキングリスト' REPORT_NAME
                      ,CASE WHEN MAX(TP.LIST_PRN_FLAG) = 0 THEN '未出力'
                            WHEN MIN(TP.LIST_PRN_FLAG) = 1 THEN '出力済'
                            WHEN MIN(TP.LIST_PRN_FLAG) = 0 AND MAX(TP.LIST_PRN_FLAG) = 1 THEN '一部出力済'
                            ELSE '－' END STATUS_NAME
                  FROM T_PIC TP
                 INNER JOIN T_ALLOC_INFO TAI
                    ON TP.SHIPPER_ID = TAI.SHIPPER_ID
                   AND TP.CENTER_ID = TAI.CENTER_ID
                   AND TP.BATCH_NO = TAI.ALLOC_NO
                   AND TAI.SHIP_KIND = 3
                   AND TAI.ALLOC_GROUP_NO = :ALLOC_GROUP_NO
                 WHERE TP.SHIPPER_ID = :SHIPPER_ID
                   AND TP.CENTER_ID = :CENTER_ID ");
            }
            if (KindS.PICK_KIND == 2)
            {
                query3.Append(@"
                UNION
                SELECT 4 NO
                      ,0 ACTIVE
                      ,'ECユニット仕分　カンバン' REPORT_NAME
                      ,'－' STATUS_NAME
                  FROM DUAL");
            }
            else
            {
                query3.Append(@"
                UNION
                SELECT 4 NO
                      ,CASE WHEN MAX(TE.SHIPPER_ID) IS NULL THEN 0 ELSE 1 END ACTIVE
                      ,'ECユニット仕分　カンバン' REPORT_NAME
                      ,CASE WHEN MAX(TE.SHIPPER_ID) IS NULL THEN '－'
                            ELSE (CASE WHEN MAX(TE.UNIT_BORD_PRN_FLAG) = 0 THEN '未出力'
                                       WHEN MIN(TE.UNIT_BORD_PRN_FLAG) = 1 THEN '出力済'
                                       ELSE '一部出力済' END) END STATUS_NAME
                  FROM T_ECUNITBORD TE
                 WHERE TE.SHIPPER_ID = :SHIPPER_ID
                   AND TE.CENTER_ID = :CENTER_ID
                   AND TE.BATCH_NO = :ALLOC_GROUP_NO");
            }
            query3.Append(@"
                UNION
                SELECT 5 NO
                      ,CASE WHEN MAX(TE.SHIPPER_ID) IS NULL THEN 0 ELSE 1 END ACTIVE
                      ,'GASカンバン' REPORT_NAME
                      ,CASE WHEN MAX(TE.SHIPPER_ID) IS NULL THEN '－'
                            ELSE (CASE WHEN MAX(TE.GASBORD_PRN_FLAG) = 0 THEN '未出力'
                                       WHEN MIN(TE.GASBORD_PRN_FLAG) = 1 THEN '出力済'
                                       ELSE '一部出力済' END) END STATUS_NAME
                  FROM T_ECGASBORD TE
                 WHERE TE.SHIPPER_ID = :SHIPPER_ID
                   AND TE.CENTER_ID = :CENTER_ID
                   AND TE.ALLOC_GROUP_NO = :ALLOC_GROUP_NO
                UNION
                SELECT 6 NO
                      ,0 ACTIVE
                      ,'ケース出荷　ケースピッキングリスト' REPORT_NAME
                      ,'－' STATUS_NAME
                  FROM DUAL
                UNION
                SELECT 7 NO
                      ,0 ACTIVE
                      ,'ケース出荷　JAN抜き取りピッキングリスト' REPORT_NAME
                      ,'－' STATUS_NAME
                  FROM DUAL
                UNION
                SELECT 8 NO
                      ,0 ACTIVE
                      ,'摘取用カンバン' REPORT_NAME
                      ,'－' STATUS_NAME
                  FROM DUAL
            ");

            StringBuilder query45 = new StringBuilder(@"
                SELECT 
                        1 DISP_NO
                    ,   1 NO
                    ,   CASE
                            WHEN MAX(SHIPPER_ID) IS NOT NULL THEN 1 
                            ELSE 0 
                        END ACTIVE
                    ,   'バッチカンバン' REPORT_NAME
                    ,   CASE 
                            WHEN MAX(BATCHLIST_PRN_FLAG) = 0 THEN '未出力'
                            WHEN MIN(BATCHLIST_PRN_FLAG) = 1 THEN '出力済'
                            WHEN MIN(BATCHLIST_PRN_FLAG) = 0 AND MAX(BATCHLIST_PRN_FLAG) = 1 THEN '一部出力済'
                            ELSE '－' 
                        END STATUS_NAME
                FROM
                        T_ALLOC_INFO
                WHERE 
                        SHIPPER_ID = :SHIPPER_ID
                    AND CENTER_ID = :CENTER_ID
                    AND ALLOC_GROUP_NO = :ALLOC_GROUP_NO
                    AND (SHIP_KIND = 4 OR SHIP_KIND = 5) 
                UNION
                SELECT
                        2 DISP_NO
                    ,   2 NO
                    ,   0 ACTIVE
                    ,   'トータルピッキングリスト' REPORT_NAME
                    ,   '－' STATUS_NAME
                FROM
                        DUAL
                UNION
                SELECT 
                        3 DISP_NO
                    ,   10 NO
                    ,   0 ACTIVE
                    ,   '店別ピッキングリスト' REPORT_NAME
                    ,   '－' STATUS_NAME
                FROM
                        DUAL");
            if (KindS.SHIP_KIND == 4)
            {
                query45.Append(@"
                UNION
                SELECT 
                        4 DISP_NO
                    ,   6 NO
                    ,   CASE
                            WHEN MAX(TP.SHIPPER_ID) IS NOT NULL THEN 1 
                            ELSE 0 
                        END ACTIVE
                    ,   'ケース出荷　ケースピッキングリスト' REPORT_NAME
                    ,   CASE
                            WHEN MAX(TP.LIST_PRN_FLAG) = 0 THEN '未出力'
                            WHEN MIN(TP.LIST_PRN_FLAG) = 1 THEN '出力済'
                            WHEN MIN(TP.LIST_PRN_FLAG) = 0 AND MAX(TP.LIST_PRN_FLAG) = 1 THEN '一部出力済'
                            ELSE '－'
                        END STATUS_NAME
                FROM 
                        T_PIC TP
                INNER JOIN 
                        T_ALLOC_INFO TAI
                    ON  TP.SHIPPER_ID = TAI.SHIPPER_ID
                    AND TP.CENTER_ID = TAI.CENTER_ID
                    AND TP.BATCH_NO = TAI.ALLOC_NO
                    AND TAI.SHIP_KIND = 4
                    AND TAI.ALLOC_GROUP_NO = :ALLOC_GROUP_NO
                WHERE
                        TP.SHIPPER_ID = :SHIPPER_ID
                    AND TP.CENTER_ID = :CENTER_ID
                UNION
                SELECT 
                        5 DISP_NO
                    ,   7 NO
                    ,   0 ACTIVE
                    ,   'ケース出荷　JAN抜き取りピッキングリスト' REPORT_NAME
                    ,   '－' STATUS_NAME
                  FROM
                        DUAL");
            }
            else
            {
                query45.Append(@"
                UNION
                SELECT 
                        4 DISP_NO
                    ,   6 NO
                    ,   0 ACTIVE
                    ,   'ケース出荷　ケースピッキングリスト' REPORT_NAME
                    ,   '－' STATUS_NAME
                FROM
                        DUAL
                UNION
                SELECT 
                        5 DISP_NO
                    ,   7 NO
                    ,   CASE 
                            WHEN MAX(TP.SHIPPER_ID) IS NOT NULL THEN 1 
                            ELSE 0 
                        END ACTIVE
                        
                    ,   'ケース出荷　JAN抜き取りピッキングリスト' REPORT_NAME
                    ,   CASE 
                            WHEN MAX(TP.LIST_PRN_FLAG) = 0 THEN '未出力'
                            WHEN MIN(TP.LIST_PRN_FLAG) = 1 THEN '出力済'
                            WHEN MIN(TP.LIST_PRN_FLAG) = 0 AND MAX(TP.LIST_PRN_FLAG) = 1 THEN '一部出力済'
                            ELSE '－'
                        END STATUS_NAME
                FROM
                        T_PIC TP
                INNER JOIN
                        T_ALLOC_INFO TAI
                    ON  TP.SHIPPER_ID = TAI.SHIPPER_ID
                    AND TP.CENTER_ID = TAI.CENTER_ID
                    AND TP.BATCH_NO = TAI.ALLOC_NO
                    AND TAI.SHIP_KIND = 5
                    AND TAI.ALLOC_GROUP_NO = :ALLOC_GROUP_NO
                 WHERE
                        TP.SHIPPER_ID = :SHIPPER_ID
                    AND TP.CENTER_ID = :CENTER_ID");
            }
            query45.Append(@"
                UNION
                SELECT
                        6 DISP_NO
                    ,   8 NO
                    ,   0 ACTIVE
                    ,   '摘取用カンバン' REPORT_NAME
                    ,   '－' STATUS_NAME
                FROM 
                        DUAL
            ");
            if (KindS.SHIP_KIND == 2)
            {
                return MvcDbContext.Current.Database.Connection.Query<PrintBatchResultRow>(query2.ToString(), parameters).ToList();
            }
            else if (KindS.SHIP_KIND == 3)
            {
                return MvcDbContext.Current.Database.Connection.Query<PrintBatchResultRow>(query3.ToString(), parameters).ToList();
            }
            else
            {
                return MvcDbContext.Current.Database.Connection.Query<PrintBatchResultRow>(query45.ToString(), parameters).ToList();
            }
        }

        /// <summary>
        /// バッチNo取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListBatchNos(string centerId)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
                SELECT ALLOC_GROUP_NO VALUE
                      ,ALLOC_GROUP_NO || ':' || MAX(BATCH_NAME) TEXT
                  FROM T_ALLOC_INFO
                  WHERE SHIPPER_ID = :SHIPPER_ID
                    AND CENTER_ID = :CENTER_ID
                    AND SHIP_KIND IN (2, 3, 4, 5)
                  GROUP BY ALLOC_GROUP_NO,CENTER_ID,SHIPPER_ID
                  ORDER BY MAX(ALLOC_DATE) DESC
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", string.IsNullOrWhiteSpace(centerId) ?Profile.User.CenterId : centerId);

            return MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters);
        }

        /// <summary>
        /// 印字フラグ更新
        /// </summary>
        public void PrintFlagUpdate(PrintBatchSearchConditions searchConditions, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", searchConditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_BATCH_NO", searchConditions.AllocGroupNo, DbType.String, ParameterDirection.Input);
            param.Add("IN_LIST_KIND", searchConditions.No, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_START_LOCATION", searchConditions.LocationCdFrom, DbType.String, ParameterDirection.Input);
            param.Add("IN_END_LOCATION", searchConditions.LocationCdTo, DbType.String, ParameterDirection.Input);
            param.Add("IN_ITEM_ID", searchConditions.ItemId, DbType.String, ParameterDirection.Input);
            param.Add("IN_JAN", searchConditions.Jan, DbType.String, ParameterDirection.Input);
            param.Add("IN_ITEM_SKU_ID", searchConditions.ItemSkuId, DbType.String, ParameterDirection.Input);
            param.Add("IN_ORDER_NO", searchConditions.BatchNo, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_SHP_PRINTBATCH_UP",
                param,
                commandType: CommandType.StoredProcedure);

            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }
    }
}