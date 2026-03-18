namespace Wms.Areas.Ship.Query.EcAllocation
{
    using System.Collections.Generic;
    using System.Text;
    using Dapper;
    using Wms.Areas.Ship.ViewModels.EcAllocation;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.EcAllocation.EcAllocationSearchConditions;

    public class Report : BaseQuery
    {
        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、引当エラー(商品別)のデータを作る。</returns>
        public IEnumerable<EcAllocationItemReport> EcAllocationItemListing(EcAllocationSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT
                  T.CENTER_ID
                  , T.ITEM_ID
                  , T.ITEM_NAME
                  , T.ITEM_COLOR_ID
                  , T.ITEM_COLOR_NAME
                  , T.ITEM_SIZE_ID
                  , T.ITEM_SIZE_NAME
                  , T.JAN
                  , CASE 
                    WHEN ROW_NUMBER() OVER ( 
                      PARTITION BY
                        T.SHIPPER_ID
                        , T.CENTER_ID
                        , T.ITEM_SKU_ID 
                      ORDER BY
                        T.ITEM_SKU_ID DESC
                        , T.LOCATION_CD
                    ) = 1 
                      THEN T.ALLOC_ERROR_QTY 
                    ELSE NULL 
                    END ALLOC_ERROR_QTY
                  , T.LOCATION_CD
                  , T.GRADE_ID
                  , T.STOCK_QTY
                  , T.ALLOC_QTY 
                FROM
                  ( 
                    SELECT
                      T.SHIPPER_ID
                      , T.CENTER_ID
                      , T.ITEM_SKU_ID
                      , T.ITEM_ID
                      , T.ITEM_NAME
                      , T.ITEM_COLOR_ID
                      , T.ITEM_COLOR_NAME
                      , T.ITEM_SIZE_ID
                      , T.ITEM_SIZE_NAME
                      , T.JAN
                      , T.ALLOC_ERROR_QTY
                      , TST.LOCATION_CD
                      , TST.GRADE_ID
                      , TST.STOCK_QTY
                      , TST.ALLOC_QTY 
                    FROM
                      ( 
                        SELECT
                          TE.SHIPPER_ID
                          , TE.CENTER_ID
                          , TE.ITEM_SKU_ID
                          , MAX(TE.ITEM_ID) ITEM_ID
                          , MAX(TE.ITEM_NAME) ITEM_NAME
                          , MAX(TE.ITEM_COLOR_ID) ITEM_COLOR_ID
                          , MAX(MC.ITEM_COLOR_NAME) ITEM_COLOR_NAME
                          , MAX(TE.ITEM_SIZE_ID) ITEM_SIZE_ID
                          , MAX(MS.ITEM_SIZE_NAME) ITEM_SIZE_NAME
                          , MAX(TE.JAN) JAN
                          , SUM(TE.ORDER_QTY - TE.ALLOC_QTY) ALLOC_ERROR_QTY 
                        FROM
                          T_ECSHIPS TE 
                          INNER JOIN M_COLORS MC 
                            ON TE.SHIPPER_ID = MC.SHIPPER_ID 
                            AND TE.ITEM_COLOR_ID = MC.ITEM_COLOR_ID 
                          INNER JOIN M_SIZES MS 
                            ON TE.SHIPPER_ID = MS.SHIPPER_ID 
                            AND TE.ITEM_SIZE_ID = MS.ITEM_SIZE_ID 
                        WHERE
                          TE.ALLOC_FLAG = 1 
                          AND TE.ALLOC_ERR_FLAG = 1 
                          AND TE.CANCEL_FLAG = 0 
                          AND TE.CENTER_ID = :CENTER_ID 
                          AND TE.SHIPPER_ID = :SHIPPER_ID 
                        GROUP BY
                          TE.SHIPPER_ID
                          , TE.CENTER_ID
                          , TE.ITEM_SKU_ID
                      ) T 
                      LEFT JOIN T_STOCKS TST 
                        ON T.SHIPPER_ID = TST.SHIPPER_ID 
                        AND T.CENTER_ID = TST.CENTER_ID 
                        AND T.ITEM_SKU_ID = TST.ITEM_SKU_ID
                      ORDER BY T.ITEM_SKU_ID
                  ) T
                 ORDER BY T.ITEM_SKU_ID ");
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<EcAllocationItemReport>(query.ToString(), parameters);
        }

        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、引当エラー(注文別)のデータを作る。</returns>
        public IEnumerable<EcAllocationOrderReport> EcAllocationOrderListing(EcAllocationSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT TE.CENTER_ID
                      ,TE.SHIP_INSTRUCT_ID
                      ,TE.SHIP_INSTRUCT_SEQ
                      ,TO_CHAR(TE.MAKE_DATE,'YYYY/MM/DD HH:MM:SS') DATA_DATE
                      ,TO_CHAR(TE.ALLOC_DATE,'YYYY/MM/DD HH:MM:SS') ALLOC_DATE
                      ,TO_CHAR(TE.RE_ALLOC_DATE,'YYYY/MM/DD HH:MM:SS') RE_ALLOC_DATE
                      ,TE.ITEM_ID
                      ,TE.ITEM_NAME
                      ,TE.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,TE.ITEM_SIZE_ID
                      ,MS.ITEM_SIZE_NAME
                      ,TE.JAN
                      ,TE.ORDER_QTY
                      ,TE.ALLOC_QTY
                      ,CASE WHEN TE.ORDER_QTY - TE.ALLOC_QTY = 0 THEN NULL ELSE TE.ORDER_QTY - TE.ALLOC_QTY END ALLOC_ERROR_QTY
                  FROM T_ECSHIPS TE
                 INNER JOIN M_COLORS MC
                    ON TE.SHIPPER_ID   = MC.SHIPPER_ID
                   AND TE.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                 INNER JOIN M_SIZES MS
                    ON TE.SHIPPER_ID   = MS.SHIPPER_ID
                   AND TE.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                 WHERE TE.ALLOC_FLAG = 1
                   AND TE.CANCEL_FLAG = 0
                   AND TE.CENTER_ID = :CENTER_ID
                   AND TE.SHIPPER_ID = :SHIPPER_ID
                   AND (SELECT MAX(TE1.ALLOC_ERR_FLAG)
                          FROM T_ECSHIPS TE1
                         WHERE TE1.SHIPPER_ID = TE.SHIPPER_ID
                           AND TE1.CENTER_ID = TE.CENTER_ID
                           AND TE1.SHIP_INSTRUCT_ID = TE.SHIP_INSTRUCT_ID) = 1
                 ORDER BY TE.SHIP_INSTRUCT_ID, TE.SHIP_INSTRUCT_SEQ ");
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<EcAllocationOrderReport>(query.ToString(), parameters);
        }

        /// <summary>
        /// 配分Csvに出力するデータを取得する。
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>表形式で表示するため、引当エラー(商品別)のデータを作る。</returns>
        public IEnumerable<EcAllocationItemForCsv> EcAllocationItemListingForCsv(EcAllocationSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT
                  T.CENTER_ID
                  , T.CENTER_NAME
                  , T.PRINT_USER_ID
                  , T.PRINT_USER_NAME
                  , T.ITEM_ID || '_' || T.ITEM_NAME || '_' || T.ITEM_COLOR_ID || '_' || T.ITEM_COLOR_NAME || '_' || T.ITEM_SIZE_ID || '_' || T.ITEM_SIZE_NAME || '_' || T.JAN ID
                  , T.ITEM_ID
                  , T.ITEM_NAME
                  , T.ITEM_COLOR_ID
                  , T.ITEM_COLOR_NAME
                  , T.ITEM_SIZE_ID
                  , T.ITEM_SIZE_NAME
                  ,SUBSTR(T.JAN,1,7) JAN1
                  ,SUBSTR(T.JAN,8,6) JAN2
                  , CASE 
                    WHEN ROW_NUMBER() OVER ( 
                      PARTITION BY
                        T.SHIPPER_ID
                        , T.CENTER_ID
                        , T.ITEM_SKU_ID 
                      ORDER BY
                        T.ITEM_SKU_ID DESC
                        , T.LOCATION_CD
                    ) = 1 
                      THEN T.ALLOC_ERROR_QTY 
                    ELSE NULL 
                    END ALLOC_ERROR_QTY
                  , T.LOCATION_CD
                  , T.GRADE_ID
                  , T.STOCK_QTY
                  , T.ALLOC_QTY 
                FROM
                  ( 
                    SELECT
                      T.SHIPPER_ID
                      , T.CENTER_ID
                      , T.CENTER_NAME
                      , T.PRINT_USER_ID
                      , T.PRINT_USER_NAME
                      , T.ITEM_SKU_ID
                      , T.ITEM_ID
                      , T.ITEM_NAME
                      , T.ITEM_COLOR_ID
                      , T.ITEM_COLOR_NAME
                      , T.ITEM_SIZE_ID
                      , T.ITEM_SIZE_NAME
                      , T.JAN
                      , T.ALLOC_ERROR_QTY
                      , TST.LOCATION_CD
                      , TST.GRADE_ID
                      , TST.STOCK_QTY
                      , TST.ALLOC_QTY 
                    FROM
                      ( 
                        SELECT
                          TE.SHIPPER_ID
                          , TE.CENTER_ID
                          , MT.CENTER_NAME1 CENTER_NAME
                          , MU.USER_ID PRINT_USER_ID
                          , MU.USER_NAME PRINT_USER_NAME
                          , TE.ITEM_SKU_ID
                          , MAX(TE.ITEM_ID) ITEM_ID
                          , MAX(TE.ITEM_NAME) ITEM_NAME
                          , MAX(TE.ITEM_COLOR_ID) ITEM_COLOR_ID
                          , MAX(MC.ITEM_COLOR_NAME) ITEM_COLOR_NAME
                          , MAX(TE.ITEM_SIZE_ID) ITEM_SIZE_ID
                          , MAX(MS.ITEM_SIZE_NAME) ITEM_SIZE_NAME
                          , MAX(TE.JAN) JAN
                          , SUM(TE.ORDER_QTY - TE.ALLOC_QTY) ALLOC_ERROR_QTY 
                        FROM
                          T_ECSHIPS TE 
                          INNER JOIN M_COLORS MC 
                            ON TE.SHIPPER_ID = MC.SHIPPER_ID 
                            AND TE.ITEM_COLOR_ID = MC.ITEM_COLOR_ID 
                          INNER JOIN M_SIZES MS 
                            ON TE.SHIPPER_ID = MS.SHIPPER_ID 
                            AND TE.ITEM_SIZE_ID = MS.ITEM_SIZE_ID 
                          INNER JOIN M_CENTERS MT
                            ON TE.SHIPPER_ID   = MT.SHIPPER_ID
                            AND TE.CENTER_ID = MT.CENTER_ID
                          INNER JOIN M_USERS MU
                            ON TE.SHIPPER_ID   = MU.SHIPPER_ID
                            AND :USER_ID = MU.USER_ID
                        WHERE
                          TE.ALLOC_FLAG = 1 
                          AND TE.ALLOC_ERR_FLAG = 1 
                          AND TE.CANCEL_FLAG = 0 
                          AND TE.CENTER_ID = :CENTER_ID 
                          AND TE.SHIPPER_ID = :SHIPPER_ID 
                        GROUP BY
                          TE.SHIPPER_ID
                          , TE.CENTER_ID
                          , MT.CENTER_NAME1
                          , MU.USER_ID
                          , MU.USER_NAME
                          , TE.ITEM_SKU_ID
                      ) T 
                      LEFT JOIN T_STOCKS TST 
                        ON T.SHIPPER_ID = TST.SHIPPER_ID 
                        AND T.CENTER_ID = TST.CENTER_ID 
                        AND T.ITEM_SKU_ID = TST.ITEM_SKU_ID
                      ORDER BY T.ITEM_SKU_ID
                  ) T
                 ORDER BY T.ITEM_SKU_ID ");
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":USER_ID", Profile.User.UserId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<EcAllocationItemForCsv>(query.ToString(), parameters);
        }

        /// <summary>
        /// 配分Csvに出力するデータを取得する。
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>表形式で表示するため、引当エラー(注文別)のデータを作る。</returns>
        public IEnumerable<EcAllocationOrderForCsv> EcAllocationOrderListingForCsv(EcAllocationSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT TE.CENTER_ID
                      ,MT.CENTER_NAME1 CENTER_NAME
                      ,MU.USER_ID PRINT_USER_ID
                      ,MU.USER_NAME PRINT_USER_NAME
                      ,TE.SHIP_INSTRUCT_ID || '_' || TE.MAKE_DATE || '_' || TE.ALLOC_DATE || '_' || TE.RE_ALLOC_DATE ID
                      ,TE.SHIP_INSTRUCT_ID
                      ,TE.SHIP_INSTRUCT_SEQ
                      ,TO_CHAR(TE.MAKE_DATE,'YYYY/MM/DD HH:MM:SS') DATA_DATE
                      ,TO_CHAR(TE.ALLOC_DATE,'YYYY/MM/DD HH:MM:SS') ALLOC_DATE
                      ,TO_CHAR(TE.RE_ALLOC_DATE,'YYYY/MM/DD HH:MM:SS') RE_ALLOC_DATE
                      ,TE.ITEM_ID
                      ,TE.ITEM_NAME
                      ,TE.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,TE.ITEM_SIZE_ID
                      ,MS.ITEM_SIZE_NAME
                      ,SUBSTR(TE.JAN,1,7) JAN1
                      ,SUBSTR(TE.JAN,8,6) JAN2
                      ,TE.ORDER_QTY
                      ,TE.ALLOC_QTY
                      ,CASE WHEN TE.ORDER_QTY - TE.ALLOC_QTY = 0 THEN NULL ELSE TE.ORDER_QTY - TE.ALLOC_QTY END ALLOC_ERROR_QTY
                  FROM T_ECSHIPS TE
                 INNER JOIN M_COLORS MC
                    ON TE.SHIPPER_ID   = MC.SHIPPER_ID
                   AND TE.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                 INNER JOIN M_SIZES MS
                    ON TE.SHIPPER_ID   = MS.SHIPPER_ID
                   AND TE.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                 INNER JOIN M_CENTERS MT
                    ON TE.SHIPPER_ID   = MT.SHIPPER_ID
                   AND TE.CENTER_ID = MT.CENTER_ID
                 INNER JOIN M_USERS MU
                    ON TE.SHIPPER_ID   = MU.SHIPPER_ID
                   AND :USER_ID = MU.USER_ID
                 WHERE TE.ALLOC_FLAG = 1
                   AND TE.CANCEL_FLAG = 0
                   AND TE.CENTER_ID = :CENTER_ID
                   AND TE.SHIPPER_ID = :SHIPPER_ID
                   AND (SELECT MAX(TE1.ALLOC_ERR_FLAG)
                          FROM T_ECSHIPS TE1
                         WHERE TE1.SHIPPER_ID = TE.SHIPPER_ID
                           AND TE1.CENTER_ID = TE.CENTER_ID
                           AND TE1.SHIP_INSTRUCT_ID = TE.SHIP_INSTRUCT_ID) = 1
                 ORDER BY TE.SHIP_INSTRUCT_ID, TE.SHIP_INSTRUCT_SEQ ");
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":USER_ID", Profile.User.UserId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<EcAllocationOrderForCsv>(query.ToString(), parameters);
        }
    }
}