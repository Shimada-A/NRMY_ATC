namespace Wms.Areas.Ship.Query.EcConfirmProgress
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Dapper;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.EcConfirmProgress;
    using Wms.Areas.Ships.ViewModels.EcConfirmProgress;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.EcConfirmProgress.EcConfirmProgress01SearchConditions;
    using static Wms.Areas.Ship.ViewModels.EcConfirmProgress.EcConfirmProgress02SearchConditions;

    public class Report : BaseQuery
    {
        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<EcConfirmProgressReport> EcConfirmProgress01Listing(EcConfirmProgress01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                WITH
                    SELECTED_RELATED_ORDER AS (
                        SELECT
                                TEC.SHIP_INSTRUCT_ID
                            ,   TEC.CENTER_ID
                            ,   TEC.SHIPPER_ID
                            ,   MAX(TEC.RELATED_ORDER_NO) AS RELATED_ORDER_NO
                        FROM
                                T_ECSHIPS TEC
                        WHERE
                                TEC.SHIPPER_ID = :SHIPPER_ID
                            AND TEC.CENTER_ID  = :CENTER_ID
                        GROUP BY
                                TEC.SHIP_INSTRUCT_ID
                            ,   TEC.CENTER_ID
                            ,   TEC.SHIPPER_ID

            ");

            if (condition.ShipStatusOld == true || condition.ShipStatus == "6")
            {
                query.Append(@"
                        UNION ALL
                        SELECT
                                AEC.SHIP_INSTRUCT_ID
                            ,   AEC.CENTER_ID
                            ,   AEC.SHIPPER_ID
                            ,   MAX(AEC.RELATED_ORDER_NO) AS RELATED_ORDER_NO
                        FROM
                                A_ECSHIPS AEC
                        WHERE
                                AEC.SHIPPER_ID = :SHIPPER_ID
                            AND AEC.CENTER_ID  = :CENTER_ID
                        GROUP BY
                                AEC.SHIP_INSTRUCT_ID
                            ,   AEC.CENTER_ID
                            ,   AEC.SHIPPER_ID

                ");
            }
                query.Append(@"

                )

                    --出荷梱包実績
                ,   GROUP_T_PACK_DATA AS (
                        SELECT
                                T_PACK.SHIP_INSTRUCT_ID
                            ,   T_PACK.CENTER_ID
                            ,   T_PACK.SHIPPER_ID
                            ,   MAX(T_PACK.DELI_NO) || SF_GET_DELI_NO_TWO_ALL(T_PACK.CENTER_ID,T_PACK.SHIPPER_ID,T_PACK.SHIP_INSTRUCT_ID)  AS DELI_NO
                        FROM
                                T_SHIP_PACKING_INFO T_PACK
                        WHERE
                                T_PACK.EC_FLAG = 1
                            AND T_PACK.SHIPPER_ID = :SHIPPER_ID
                            AND T_PACK.CENTER_ID  = :CENTER_ID
                        GROUP BY
                                T_PACK.SHIP_INSTRUCT_ID
                            ,   T_PACK.CENTER_ID
                            ,   T_PACK.SHIPPER_ID
                ");

            if (condition.ShipStatusOld == true || condition.ShipStatus == "6")
            {
                query.Append(@"
                        UNION ALL
                        SELECT
                                A_PACK.SHIP_INSTRUCT_ID
                            ,   A_PACK.CENTER_ID
                            ,   A_PACK.SHIPPER_ID
                            ,   MAX(A_PACK.DELI_NO) || SF_GET_DELI_NO_TWO_ALL(A_PACK.CENTER_ID,A_PACK.SHIPPER_ID, A_PACK.SHIP_INSTRUCT_ID)  AS DELI_NO
                        FROM
                                A_SHIP_PACKING_INFO A_PACK
                        WHERE
                                A_PACK.EC_FLAG = 1
                            AND A_PACK.SHIPPER_ID = :SHIPPER_ID
                            AND A_PACK.CENTER_ID  = :CENTER_ID
                        GROUP BY
                                A_PACK.SHIP_INSTRUCT_ID
                            ,   A_PACK.CENTER_ID
                            ,   A_PACK.SHIPPER_ID            
                ");
            }

            query.Append(@"

                )

            SELECT DISTINCT
                        WK.*
                    ,   EC.RELATED_ORDER_NO
                    ,   TPACK.DELI_NO 
               FROM
                        WW_SHP_EC_CONFIRM_PROGRESS WK
                LEFT OUTER JOIN
                        SELECTED_RELATED_ORDER EC
                ON
                        WK.SHIP_INSTRUCT_ID = EC.SHIP_INSTRUCT_ID
                    AND WK.CENTER_ID = EC.CENTER_ID
                    AND WK.SHIPPER_ID = EC.SHIPPER_ID

                LEFT OUTER JOIN
                        GROUP_T_PACK_DATA TPACK
                ON
                        EC.SHIP_INSTRUCT_ID = TPACK.SHIP_INSTRUCT_ID
                    AND EC.CENTER_ID = TPACK.CENTER_ID
                    AND EC.SHIPPER_ID = TPACK.SHIPPER_ID

                WHERE
                        WK.SHIPPER_ID = :SHIPPER_ID
                    AND WK.SEQ = :SEQ
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SEQ", condition.Seq);

            // Sort function
            switch (condition.SortKey)
            {
                case EcConfirmProgressSortKey.ShipPlanDateShipInstructId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WK.SHIP_PLAN_DATE DESC, WK.SHIP_INSTRUCT_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WK.SHIP_PLAN_DATE ASC, WK.SHIP_INSTRUCT_ID ASC ");
                            break;
                    }

                    break;
                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WK.SHIP_INSTRUCT_ID DESC, WK.SHIP_PLAN_DATE DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WK.SHIP_INSTRUCT_ID ASC, WK.SHIP_PLAN_DATE ASC ");
                            break;
                    }

                    break;
            }

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<EcConfirmProgressReport>(query.ToString(), parameters);
        }

        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<EcConfirmProgressReportDetail> EcConfirmProgress01ListingDetail(EcConfirmProgress01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder queryCommon = new StringBuilder();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                WITH
                    SELECTED_WORK_DATA AS (
                        SELECT
                                *
                        FROM
                                WW_SHP_EC_CONFIRM_PROGRESS
                        WHERE
                                SHIPPER_ID = :SHIPPER_ID
                            AND SEQ = :SEQ
                )
                    --出荷梱包実績
                ,   GROUP_T_PACK_DATA AS (
                        SELECT
                                SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   MAX(BOX_NO) AS BOX_NO
                            ,   MAX(SHIP_TO_STORE_ID) AS SHIP_TO_STORE_ID
                            ,   MIN(NOUHIN_PRN_FLAG) AS NOUHIN_PRN_FLAG
                            ,   MIN(DELI_PRN_FLAG) AS DELI_PRN_FLAG
                            ,   MAX(DELI_NO) || SF_GET_DELI_NO_TWO_ALL(CENTER_ID,SHIPPER_ID,SHIP_INSTRUCT_ID)   AS DELI_NO
                        FROM
                                T_SHIP_PACKING_INFO
                        WHERE
                                EC_FLAG = 1
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
            ");
            query.Append(@"
                        GROUP BY
                                SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                )
                    --EC出荷データ
                ,   T_ECSHIP_DATA AS (
                        SELECT
                                TSHIP.CENTER_ID
                            ,   TSHIP.SHIPPER_ID
                            ,   TSHIP.SHIP_INSTRUCT_ID
                            ,   TSHIP.SHIP_INSTRUCT_SEQ
                            ,   TSHIP.SHIP_REQUEST_DATE
                            ,   TSHIP.ORDER_DATE
                            ,   TSHIP.ARRIVE_REQUEST_DATE
                            ,   TSHIP.MAKE_DATE
                            ,   TSHIP.BATCH_NO
                            ,   TSHIP.IF_STATE_ERP
                            ,   TSHIP.KAKU_FLAG
                            ,   TSHIP.KAKU_DATE
                            ,   TSHIP.EC_SHIP_CLASS
                            ,   GAS.GAS_STOCK_OUT_QTY AS STOCKOUT_QTY
                            ,   TSHIP.CANCEL_FLAG
                            ,   TSHIP.ITEM_SKU_ID
                            ,   TSHIP.ORDER_QTY
                            ,   TSHIP.TRANSPORTER_ID
                            ,   TSHIP.GAS_BATCH_NO
                            ,   TSHIP.AFT_ALLOC_CANCEL_FLAG
                            ,   TSHIP.ITEM_ID
                            ,   TSHIP.JAN
                            ,   TPACK.BOX_NO
                            ,   TPACK.SHIP_TO_STORE_ID
                            ,   TPACK.NOUHIN_PRN_FLAG
                            ,   TPACK.DELI_PRN_FLAG
                            ,   0 AS DAY_FLAG
                            ,   TSHIP.RELATED_ORDER_NO
                            ,   TPACK.DELI_NO
                        FROM
                                T_ECSHIPS TSHIP
                        LEFT OUTER JOIN
                                GROUP_T_PACK_DATA TPACK
                        ON
                                TSHIP.SHIP_INSTRUCT_ID = TPACK.SHIP_INSTRUCT_ID
                            AND TSHIP.SHIP_INSTRUCT_SEQ = TPACK.SHIP_INSTRUCT_SEQ
                            AND TSHIP.CENTER_ID = TPACK.CENTER_ID
                            AND TSHIP.SHIPPER_ID = TPACK.SHIPPER_ID
                        LEFT OUTER JOIN
                                T_GAS GAS
                        ON
                                GAS.BATCH_NO = TSHIP.BATCH_NO
                            AND GAS.GAS_BATCH_NO = TSHIP.GAS_BATCH_NO
                            AND GAS.MAGUCHI_NO = TSHIP.GAS_MAGUCHI_NO
                            AND GAS.SHIP_INSTRUCT_ID = TSHIP.SHIP_INSTRUCT_ID
                            AND GAS.SHIP_INSTRUCT_SEQ = TSHIP.SHIP_INSTRUCT_SEQ
                            AND GAS.CENTER_ID = TSHIP.CENTER_ID
                            AND GAS.SHIPPER_ID = TSHIP.SHIPPER_ID
                        WHERE
                                TSHIP.CENTER_ID = :CENTER_ID
                            AND TSHIP.SHIPPER_ID = :SHIPPER_ID
                )
            ");
            if (condition.ShipStatusOld == true || condition.ShipStatus == "6")
            {
                query.Append(@"
                    --累積梱包実績
                    ,   GROUP_A_PACK_DATA AS(
                            SELECT
                                    SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   CENTER_ID
                                ,   SHIPPER_ID
                                ,   MAX(BOX_NO) AS BOX_NO
                                ,   MAX(SHIP_TO_STORE_ID) AS SHIP_TO_STORE_ID
                                ,   MIN(NOUHIN_PRN_FLAG) AS NOUHIN_PRN_FLAG
                                ,   MIN(DELI_PRN_FLAG) AS DELI_PRN_FLAG
                                ,   MAX(DELI_NO) || SF_GET_DELI_NO_TWO_ALL(CENTER_ID,SHIPPER_ID,SHIP_INSTRUCT_ID)  AS DELI_NO
                            FROM
                                    A_SHIP_PACKING_INFO
                            WHERE
                                    EC_FLAG = 1
                                AND CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                ");
                query.Append(@"
                        GROUP BY
                                SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                    )
                    ,   A_ECSHIP_DATA AS(
                            SELECT
                                    ASHIP.CENTER_ID
                                ,   ASHIP.SHIPPER_ID
                                ,   ASHIP.SHIP_INSTRUCT_ID
                                ,   ASHIP.SHIP_INSTRUCT_SEQ
                                ,   ASHIP.SHIP_REQUEST_DATE
                                ,   ASHIP.ORDER_DATE
                                ,   ASHIP.ARRIVE_REQUEST_DATE
                                ,   ASHIP.MAKE_DATE
                                ,   ASHIP.BATCH_NO
                                ,   ASHIP.IF_STATE_ERP
                                ,   ASHIP.KAKU_FLAG
                                ,   ASHIP.KAKU_DATE
                                ,   ASHIP.EC_SHIP_CLASS
                                ,   GAS.GAS_STOCK_OUT_QTY AS STOCKOUT_QTY
                                ,   ASHIP.CANCEL_FLAG
                                ,   ASHIP.ITEM_SKU_ID
                                ,   ASHIP.ORDER_QTY
                                ,   ASHIP.TRANSPORTER_ID
                                ,   ASHIP.GAS_BATCH_NO
                                ,   ASHIP.AFT_ALLOC_CANCEL_FLAG
                                ,   ASHIP.ITEM_ID
                                ,   ASHIP.JAN
                                ,   APACK.BOX_NO
                                ,   APACK.SHIP_TO_STORE_ID
                                ,   APACK.NOUHIN_PRN_FLAG
                                ,   APACK.DELI_PRN_FLAG
                                ,   CASE WHEN TRIM(APACK.BOX_NO) IS NOT NULL THEN 1 ELSE 0 END AS DAY_FLAG
                                ,   ASHIP.RELATED_ORDER_NO
                                ,   APACK.DELI_NO
                            FROM
                                    A_ECSHIPS ASHIP
                            LEFT OUTER JOIN
                                    GROUP_A_PACK_DATA APACK
                            ON
                                    ASHIP.SHIP_INSTRUCT_ID = APACK.SHIP_INSTRUCT_ID
                                AND ASHIP.SHIP_INSTRUCT_SEQ = APACK.SHIP_INSTRUCT_SEQ
                                AND ASHIP.CENTER_ID = APACK.CENTER_ID
                                AND ASHIP.SHIPPER_ID = APACK.SHIPPER_ID
                            LEFT OUTER JOIN
                                    A_GAS GAS
                            ON
                                    GAS.BATCH_NO = ASHIP.BATCH_NO
                                AND GAS.GAS_BATCH_NO = ASHIP.GAS_BATCH_NO
                                AND GAS.MAGUCHI_NO = ASHIP.GAS_MAGUCHI_NO
                                AND GAS.SHIP_INSTRUCT_ID = ASHIP.SHIP_INSTRUCT_ID
                                AND GAS.SHIP_INSTRUCT_SEQ = ASHIP.SHIP_INSTRUCT_SEQ
                                AND GAS.CENTER_ID = ASHIP.CENTER_ID
                                AND GAS.SHIPPER_ID = ASHIP.SHIPPER_ID
                            WHERE
                                    ASHIP.CENTER_ID = :CENTER_ID
                                AND ASHIP.SHIPPER_ID = :SHIPPER_ID
                    )
                    ,   ALL_ECSHIP_DATA AS(
                            SELECT * FROM T_ECSHIP_DATA
                            UNION ALL
                            SELECT * FROM A_ECSHIP_DATA
                    )
                ");

            };
            StringBuilder queryMain = new StringBuilder(@"
                SELECT
                        TE.CENTER_ID AS  CENTER_ID
                    ,   TE.SHIP_REQUEST_DATE AS SHIP_PLAN_DATE
                    ,   TE.SHIP_INSTRUCT_ID
                    ,   TE.SHIP_INSTRUCT_SEQ
                    ,   TE.ORDER_DATE AS ORDER_DATE
                    ,   MT.TRANSPORTER_NAME AS TRANSPORTER_NAME
                    ,   TE.ARRIVE_REQUEST_DATE AS ARRIVE_REQUEST_DATE
                    ,   TE.MAKE_DATE AS ALLOC_DATE
                    ,   CASE
                            WHEN TE.BATCH_NO = ' ' THEN '" + EcConfirmProgressResource.UnAlloc + @"'
                            WHEN TE.DAY_FLAG = 1 THEN '" + EcConfirmProgressResource.Donedaily + @"'
                            WHEN TE.IF_STATE_ERP = 2 THEN '" + EcConfirmProgressResource.Send + @"'
                            WHEN TE.KAKU_FLAG = 1 THEN '" + EcConfirmProgressResource.Fixed + @"'
                            WHEN TE.NOUHIN_PRN_FLAG = 9 THEN '" + EcConfirmProgressResource.DeliveryIssued + @"'
                            ELSE '" + EcConfirmProgressResource.ShipDuring + @"'
                        END AS SHIP_STATUS_NAME
                    ,   TE.KAKU_DATE AS KAKU_DATE
                    ,   CASE
                            WHEN TE.EC_SHIP_CLASS = 1 THEN '" + EcConfirmProgressResource.SingleFlag + @"'
                            WHEN TE.EC_SHIP_CLASS = 2 THEN '" + EcConfirmProgressResource.OrderFlag + @"'
                            WHEN TE.EC_SHIP_CLASS = 3 THEN '" + EcConfirmProgressResource.GasFlag + @"'
                            ELSE ''
                        END AS EC_SHIP_CLASS_NAME
                    ,   CASE WHEN TE.STOCKOUT_QTY = 0 THEN NULL ELSE TE.STOCKOUT_QTY END AS STOCKOUT_QTY
                    ,   CASE
                            WHEN TE.CANCEL_FLAG = 1 OR TE.AFT_ALLOC_CANCEL_FLAG = 1 THEN 'ｷｬﾝｾﾙ' ELSE '' END AS CANCEL_FLAG
                    ,   TE.ITEM_SKU_ID AS ITEM_SKU_ID
                    ,   TE.ORDER_QTY AS  ORDER_QTY
                    ,   TE.BOX_NO AS BOX_NO
                    ,   TE.SHIP_TO_STORE_ID AS SHIP_TO_STORE_ID 
                    ,   TE.JAN
                    ,   MIS.ITEM_NAME
                    ,   TE.RELATED_ORDER_NO
                    ,   TE.DELI_NO
            ");

            if (condition.ShipStatusOld == true || condition.ShipStatus == "6")
            {
                queryCommon.Append(@"
                    FROM
                            ALL_ECSHIP_DATA TE
                ");
            }
            else
            {
                queryCommon.Append(@"
                    FROM
                            T_ECSHIP_DATA TE
                ");
            }

            queryCommon.Append(@"
                INNER JOIN
                        SELECTED_WORK_DATA WK
                ON
                        TE.SHIP_INSTRUCT_ID = WK.SHIP_INSTRUCT_ID
                    AND TE.CENTER_ID = WK.CENTER_ID
                    AND TE.SHIPPER_ID = WK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_TRANSPORTERS MT
                ON
                        MT.SHIPPER_ID = TE.SHIPPER_ID
                    AND MT.TRANSPORTER_ID = TE.TRANSPORTER_ID
                LEFT OUTER JOIN
                        M_ITEM_SKU MIS
                ON
                        TE.SHIPPER_ID = MIS.SHIPPER_ID
                    AND TE.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                 WHERE
                       1 = 1
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SEQ", condition.Seq);

            // Sort function
            switch (condition.SortKey)
            {
                case EcConfirmProgressSortKey.ShipPlanDateShipInstructId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            queryCommon.AppendLine(@"ORDER BY TE.SHIP_REQUEST_DATE DESC,TE.SHIP_INSTRUCT_ID DESC,TE.SHIP_INSTRUCT_SEQ DESC ");
                            break;

                        default:
                            queryCommon.AppendLine(@"ORDER BY TE.SHIP_REQUEST_DATE ASC,TE.SHIP_INSTRUCT_ID ASC,TE.SHIP_INSTRUCT_SEQ ASC ");
                            break;
                    }

                    break;
                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            queryCommon.AppendLine(@"ORDER BY TE.SHIP_INSTRUCT_ID DESC,TE.SHIP_INSTRUCT_SEQ DESC,TE.SHIP_REQUEST_DATE DESC ");
                            break;

                        default:
                            queryCommon.AppendLine(@"ORDER BY TE.SHIP_INSTRUCT_ID ASC,TE.SHIP_INSTRUCT_SEQ ASC,TE.SHIP_REQUEST_DATE ASC ");
                            break;
                    }

                    break;
            }

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<EcConfirmProgressReportDetail>(query.ToString() + queryMain.ToString() + queryCommon.ToString(), parameters);
        }

        //帳票発行
        public IEnumerable<EcConfirmProgressReportRowForCsv> GetResultRowList(EcConfirmProgress02SearchConditions condition)
        {
            var EcConfirmProgress01 = MvcDbContext.Current.ShpEcConfirmProgress.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == condition.Seq && x.LineNo == condition.LineNo).FirstOrDefault();
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                WITH
                    T_ECSHIPS_DATA AS (
                        SELECT
                                TSHIP.MAKE_DATE AS MOVE_DATE
                            ,   TSHIP.MAKE_USER_ID AS MOVE_USER_ID
                            ,   TSHIP.MAKE_PROGRAM_NAME AS MOVE_PROGRAM_NAME
                            ,   TSHIP.*
                            ,   TPACK.BOX_NO
                            ,   TPACK.DELI_NO
                            ,   TPACK.RESULT_QTY AS PACK_RESULT_QTY
                            ,   TPACK.NOUHIN_PRN_USER_ID
                        FROM
                                T_ECSHIPS TSHIP
                        LEFT OUTER JOIN
                                T_SHIP_PACKING_INFO TPACK
                        ON
                                TSHIP.SHIPPER_ID = TPACK.SHIPPER_ID
                            AND TSHIP.CENTER_ID = TPACK.CENTER_ID
                            AND TSHIP.SHIP_INSTRUCT_ID = TPACK.SHIP_INSTRUCT_ID
                            AND TSHIP.SHIP_INSTRUCT_SEQ = TPACK.SHIP_INSTRUCT_SEQ
                )
            ");
            if (condition.ShipStatus == "B")
            {
                query.Append(@"
                    ,   A_ECSHIPS_DATA AS (
                            SELECT
                                    ASHIP.*
                                ,   APACK.BOX_NO
                                ,   APACK.DELI_NO
                                ,   APACK.RESULT_QTY AS PACK_RESULT_QTY
                                ,   APACK.NOUHIN_PRN_USER_ID
                            FROM
                                    A_ECSHIPS ASHIP
                            LEFT OUTER JOIN
                                    A_SHIP_PACKING_INFO APACK
                            ON
                                    ASHIP.SHIPPER_ID = APACK.SHIPPER_ID
                                AND ASHIP.CENTER_ID = APACK.CENTER_ID
                                AND ASHIP.SHIP_INSTRUCT_ID = APACK.SHIP_INSTRUCT_ID
                                AND ASHIP.SHIP_INSTRUCT_SEQ = APACK.SHIP_INSTRUCT_SEQ
                    )
                    ,   ALL_ECSHIPS_DATA AS (
                            SELECT * FROM T_ECSHIPS_DATA
                            UNION ALL
                            SELECT * FROM A_ECSHIPS_DATA
                    )
                ");
            };
            query.Append(@"
                SELECT
                        TE.SHIP_INSTRUCT_ID
                    ,   TE.SHIP_INSTRUCT_SEQ
                    ,   TE.BOX_NO
                    ,   TE.ITEM_SKU_ID
                    ,   TE.ITEM_ID
                    ,   TE.ITEM_NAME
                    ,   TE.ITEM_COLOR_ID
                    ,   MC.ITEM_COLOR_NAME
                    ,   TE.ITEM_SIZE_ID
                    ,   MS.ITEM_SIZE_NAME
                    ,   SUBSTR(TE.JAN,1,12) JAN
                    ,   SUBSTR(TE.JAN,1,7) JAN1
                    ,   SUBSTR(TE.JAN,8,6) JAN2
                    ,   CASE WHEN TE.PACK_RESULT_QTY = 0 THEN NULL ELSE TE.PACK_RESULT_QTY END RESULT_QTY
                    ,   TE.CENTER_ID || '　' || MCE.CENTER_NAME1 CENTER
                    ,   :USER_ID || '　' || MU.USER_NAME PRINT_USER
                FROM
            ");
            // B
            if (condition.ShipStatus == "B")
            {
                query.Append(@" ALL_ECSHIPS_DATA TE ");
            }
            else
            {
                query.Append(@" T_ECSHIPS_DATA TE ");
            }
            query.Append(@" 
                LEFT JOIN
                        M_ITEM_SKU MIS
                ON
                        MIS.SHIPPER_ID = TE.SHIPPER_ID
                    AND MIS.ITEM_SKU_ID = TE.ITEM_SKU_ID
                LEFT JOIN
                        M_COLORS MC
                ON
                        MC.SHIPPER_ID = TE.SHIPPER_ID
                    AND MC.ITEM_COLOR_ID = TE.ITEM_COLOR_ID
                LEFT JOIN
                        M_SIZES MS
                ON
                        MS.SHIPPER_ID = TE.SHIPPER_ID
                    AND MS.ITEM_SIZE_ID = TE.ITEM_SIZE_ID
                LEFT JOIN
                        M_CENTERS MCE
                ON
                        TE.SHIPPER_ID = MCE.SHIPPER_ID
                    AND TE.CENTER_ID = MCE.CENTER_ID
                LEFT JOIN 
                        M_USERS MU
                ON
                        TE.SHIPPER_ID   = MU.SHIPPER_ID
                    AND :USER_ID = MU.USER_ID
                WHERE
                        TE.SHIPPER_ID = :SHIPPER_ID
                    AND TE.CENTER_ID = :CENTER_ID
                    AND TE.SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", EcConfirmProgress01.CenterId);
            parameters.Add(":SHIP_INSTRUCT_ID", EcConfirmProgress01.ShipInstructId);
            parameters.Add(":USER_ID", Common.Profile.User.UserId);
            // Sort function
            switch (condition.SortKey)
            {
                case EcConfirmProgress02SortKey.ShipInstructSeqId:
                    query.AppendLine(@" ORDER BY TE.SHIP_INSTRUCT_SEQ ASC,TE.BOX_NO  ASC ");

                    break;
                default:
                    query.AppendLine(@" ORDER BY TE.BOX_NO ASC,TE.ITEM_SKU_ID ASC ");

                    break;
            }

            return MvcDbContext.Current.Database.Connection.Query<EcConfirmProgressReportRowForCsv>(query.ToString(), parameters);
        }
    }
}