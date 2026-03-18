namespace Wms.Areas.Move.Query.TransferReference
{
    using System.Collections.Generic;
    using System.Text;
    using Dapper;
    using Wms.Areas.Move.ViewModels.TransferReference;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Move.ViewModels.TransferReference.TransferReference01SearchConditions;
    using static Wms.Areas.Move.ViewModels.TransferReference.TransferReference02SearchConditions;

    public class Report : BaseQuery
    {
        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<TransferReference01Report> TransferReference01Listing(TransferReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
            WITH
                WORK_DATA AS (
                    SELECT
                            SHIPPER_ID
                        ,   CENTER_ID
                        ,   SLIP_NO
                        ,   ITEM_SKU_ID
                        ,   MAX(LINE_NO) AS LINE_NO
                        ,   MAX(SEQ) AS SEQ
                        ,   MAX(TRANSFER_FROM_STORE_ID) AS TRANSFER_FROM_STORE_ID
                        ,   MAX(TRANSFER_FROM_STORE_NAME) AS TRANSFER_FROM_STORE_NAME
                        ,   MAX(PLAN_SLIP_NO) AS PLAN_SLIP_NO
                        ,   MAX(RESULT_SLIP_NO) AS RESULT_SLIP_NO
                        ,   MAX(ARRIVE_PLAN_QTY) AS ARRIVE_PLAN_QTY
                        ,   SUM(RESULT_QTY) AS RESULT_QTY
                        ,   MAX(STATUS) AS STATUS
                        ,   MAX(IF_STATE) AS IF_STATE
                        ,   MAX(CONFIRM_DATE) AS CONFIRM_DATE
                        ,   MAX(UNPLANNED_FLAG) AS UNPLANNED_FLAG
                        ,   MAX(SLIP_DATE) AS SLIP_DATE
                        ,   MAX(TRANSFER_CLASS) AS TRANSFER_CLASS
                    FROM
                            WW_ARR_TRANS_REF01
                    WHERE
                            SHIPPER_ID = :SHIPPER_ID
                        AND SEQ = :SEQ
                    GROUP BY
                            SHIPPER_ID
                        ,   CENTER_ID
                        ,   SLIP_NO
                        ,   ITEM_SKU_ID
                )
                ,WORK_TBL AS (
                    SELECT
                            MAX(WW.LINE_NO) AS LINE_NO
                        ,   MAX(WW.SEQ) AS SEQ
                        ,   WW.SHIPPER_ID
                        ,   WW.CENTER_ID
                        ,   WW.TRANSFER_FROM_STORE_ID
                        ,   MAX(WW.TRANSFER_FROM_STORE_NAME) AS TRANSFER_FROM_STORE_NAME
                        ,   NVL(COUNT(DISTINCT(WW.PLAN_SLIP_NO)),0) AS SLIP_NO_PLAN_QTY
                        ,   CASE COUNT(DISTINCT(WW.RESULT_SLIP_NO))
                                WHEN 0 THEN NULL
                                ELSE COUNT(DISTINCT(WW.RESULT_SLIP_NO))
                            END AS SLIP_NO_RESULT_QTY
                        ,   COUNT(DISTINCT(WW.ITEM_SKU_ID)) AS ITEM_SKU_QTY
                        ,   NVL(SUM(WW.ARRIVE_PLAN_QTY),0) AS ARRIVE_PLAN_QTY
                        ,   SUM(WW.RESULT_QTY) AS RESULT_QTY
                        ,   CASE
                                WHEN SUM(WW.STATUS) = 0 THEN 1 --1:未入荷
                                WHEN MAX(WW.STATUS) > 0 AND MIN(WW.STATUS) = 0 THEN 2 --2:一部検品済
                                WHEN MIN(WW.STATUS) > 0 AND MIN(WW.IF_STATE) = 2 THEN 4 --4:実績送信済
                                ELSE 3 --3:検品済
                            END AS TRANSFER_STATUS
                        ,   WW.CONFIRM_DATE
                        ,   NVL(MIN(WW.UNPLANNED_FLAG),0) AS UNPLANNED_FLAG
                        ,   WW.SLIP_DATE
                        ,   WW.TRANSFER_CLASS
                    FROM
                            WORK_DATA WW
                    GROUP BY
                            WW.SHIPPER_ID
                        ,   WW.CENTER_ID
                        ,   WW.TRANSFER_CLASS
                        ,   WW.SLIP_DATE
                        ,   WW.TRANSFER_FROM_STORE_ID
                        ,   WW.CONFIRM_DATE
            )
            SELECT
                    WORK_TBL.*
                ,   TRANSFER_STATUS.GEN_NAME AS TRANSFER_STATUS_NAME 
            FROM
                    WORK_TBL
            LEFT OUTER JOIN
                    M_GENERALS TRANSFER_STATUS
            ON
                    TRANSFER_STATUS.SHIPPER_ID = WORK_TBL.SHIPPER_ID
                AND TRANSFER_STATUS.CENTER_ID = '@@@'
                AND TRANSFER_STATUS.REGISTER_DIVI_CD = '1'
                AND TRANSFER_STATUS.GEN_DIV_CD = 'TRANSFER_STATUS'
                AND TRANSFER_STATUS.GEN_CD = TO_CHAR(WORK_TBL.TRANSFER_STATUS)
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 状況
            if (!string.IsNullOrEmpty(condition.TransferStatus))
            {
                query.AppendLine(@"WHERE WORK_TBL.TRANSFER_STATUS = :TRANSFER_STATUS ");
                parameters.Add(":TRANSFER_STATUS", condition.TransferStatus);
            }

            // Sort function
            switch (condition.SortKey)
            {
                case TransferReference01SortKey.DenpyoDateTransferClassStoreId:
                    switch (condition.Sort)
                    {
                        case TransferReference01SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY SLIP_DATE DESC,TRANSFER_CLASS DESC,TRANSFER_FROM_STORE_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY SLIP_DATE ASC,TRANSFER_CLASS ASC,TRANSFER_FROM_STORE_ID ASC ");
                            break;
                    }

                    break;
                case TransferReference01SortKey.StoreIdDenpyoDate:
                    switch (condition.Sort)
                    {
                        case TransferReference01SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY TRANSFER_FROM_STORE_ID DESC,SLIP_DATE DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY TRANSFER_FROM_STORE_ID ASC,SLIP_DATE ASC ");
                            break;
                    }

                    break;
                default:
                    switch (condition.Sort)
                    {
                        case TransferReference01SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY TRANSFER_CLASS DESC,SLIP_DATE DESC,TRANSFER_FROM_STORE_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY TRANSFER_CLASS ASC,SLIP_DATE ASC,TRANSFER_FROM_STORE_ID ASC ");
                            break;
                    }

                    break;
            }

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<TransferReference01Report>(query.ToString(), parameters);
        }

        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<TransferReference02Report> TransferReference02Listing(TransferReference02SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            var orderBySql = "";

            switch (condition.SortKey)
            {
                case TransferReference02SortKey.TransferClassDenpyoDateStoreIdSkuDenpyoNo:
                    switch (condition.Sort)
                    {
                        case TransferReference02SearchConditions.AscDescSort.Desc:
                            orderBySql = @"ORDER BY TRANSFER_CLASS DESC, SLIP_DATE DESC, TRANSFER_FROM_STORE_ID DESC, ITEM_SKU_ID DESC, SLIP_NO DESC, BOX_NO DESC ";
                            break;

                        default:
                            orderBySql = @"ORDER BY TRANSFER_CLASS ASC, SLIP_DATE ASC, TRANSFER_FROM_STORE_ID ASC, ITEM_SKU_ID ASC, SLIP_NO ASC, BOX_NO ASC ";
                            break;
                    }

                    break;
                case TransferReference02SortKey.SkuStoreIdDenpyoDate:
                    switch (condition.Sort)
                    {
                        case TransferReference02SearchConditions.AscDescSort.Desc:
                            orderBySql = @"ORDER BY ITEM_SKU_ID DESC, TRANSFER_FROM_STORE_ID DESC, SLIP_DATE DESC, BOX_NO DESC ";
                            break;

                        default:
                            orderBySql = @"ORDER BY ITEM_SKU_ID ASC, TRANSFER_FROM_STORE_ID ASC, SLIP_DATE ASC, BOX_NO ASC ";
                            break;
                    }

                    break;
                default:
                    switch (condition.Sort)
                    {
                        case TransferReference02SearchConditions.AscDescSort.Desc:
                            orderBySql = @"ORDER BY TRANSFER_CLASS DESC, SLIP_DATE DESC, TRANSFER_FROM_STORE_ID DESC, SLIP_NO DESC, ITEM_SKU_ID DESC, BOX_NO DESC ";
                            break;

                        default:
                            orderBySql = @"ORDER BY TRANSFER_CLASS ASC, SLIP_DATE ASC, TRANSFER_FROM_STORE_ID ASC, SLIP_NO ASC, ITEM_SKU_ID ASC, BOX_NO ASC ";
                            break;
                    }

                    break;
            }

            StringBuilder query = new StringBuilder($@"
            WITH
                WORK_DATA AS (
                    SELECT
                            WW.*
                        ,   ROW_NUMBER() OVER (PARTITION BY WW.SLIP_NO, WW.ITEM_SKU_ID {orderBySql}) AS SKU_ROW_NUMBER
                    FROM
                            WW_ARR_TRANS_REF02 WW
                    WHERE
                            WW.SHIPPER_ID = :SHIPPER_ID
                        AND WW.SEQ = :SEQ
                )
            SELECT
                    SLIP_DATE
                ,   TRANSFER_FROM_STORE_ID
                ,   TRANSFER_FROM_STORE_NAME
                ,   SLIP_NO
                ,   SLIP_SEQ
                ,   CATEGORY_NAME1
                ,   ITEM_ID
                ,   ITEM_NAME
                ,   ITEM_COLOR_ID
                ,   ITEM_COLOR_NAME
                ,   ITEM_SIZE_ID
                ,   ITEM_SIZE_NAME
                ,   JAN
                ,   CASE WHEN SKU_ROW_NUMBER = 1 THEN ARRIVE_PLAN_QTY ELSE NULL END AS ARRIVE_PLAN_QTY
                ,   RESULT_QTY
                ,   TRANSFER_STATUS_NAME
                ,   CONFIRM_DATE
                ,   CASE
                        WHEN UNPLANNED_FLAG = 1 THEN '*'
                        ELSE NULL
                    END AS UNPLANNED_FLAG
                ,   BOX_NO
                ,   CASE WHEN SKU_ROW_NUMBER = 1 THEN DIFFERENCE_QTY ELSE NULL END AS DIFFERENCE_QTY
            FROM
                    WORK_DATA
            ");

            query.AppendLine(orderBySql);

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<TransferReference02Report>(query.ToString(), parameters);
        }

        /// <summary>
        /// 帳票に出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        public IEnumerable<TransferReferenceReportRowForCsv> GetResultRowList(TransferReference02SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
            SELECT
                    TO_CHAR(WATR.ARRIVE_PLAN_DATE,'YYYY/MM/DD') AS ARRIVE_PLAN_DATE
                ,   MC.CENTER_ID
                ,   MC.CENTER_NAME1
                ,   WATR.TRANSFER_FROM_STORE_ID
                ,   WATR.TRANSFER_FROM_STORE_NAME
                ,   WATR.SLIP_NO AS SLIP_NO
                ,   WATR.ITEM_ID
                ,   WATR.ITEM_NAME
                ,   WATR.ITEM_COLOR_ID
                ,   WATR.ITEM_COLOR_NAME
                ,   WATR.ITEM_SIZE_ID
                ,   WATR.ITEM_SIZE_NAME
                ,   SUBSTR(WATR.JAN,1,12) JAN
                ,   WATR.ARRIVE_PLAN_QTY
                ,:USER_ID || '　' || MU.USER_NAME PRINT_USER
                ,   WATR.SLIP_NO FULL_SLIP_NO
            FROM
                    WW_ARR_TRANS_REF02 WATR
            INNER JOIN
                    M_CENTERS MC
            ON
                    WATR.SHIPPER_ID = MC.SHIPPER_ID
                AND WATR.CENTER_ID = MC.CENTER_ID
            INNER JOIN 
                    M_USERS MU
            ON
                    WATR.SHIPPER_ID   = MU.SHIPPER_ID
                AND :USER_ID = MU.USER_ID
            WHERE
                    WATR.SHIPPER_ID = :SHIPPER_ID
                AND WATR.SEQ = :SEQ
            ORDER BY 
                    ARRIVE_PLAN_DATE,TRANSFER_FROM_STORE_ID,SLIP_NO,ITEM_SKU_ID
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);
            parameters.Add(":USER_ID", Common.Profile.User.UserId);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<TransferReferenceReportRowForCsv>(query.ToString(), parameters);
        }
    }
}