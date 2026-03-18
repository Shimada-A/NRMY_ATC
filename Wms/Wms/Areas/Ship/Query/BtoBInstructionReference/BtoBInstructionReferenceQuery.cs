namespace Wms.Areas.Ship.Query.BtoBInstructionReference
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
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.BtoBInstructionReference;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.BtoBInstructionReference.BtoBInstructionReference01SearchConditions;
    using static Wms.Areas.Ship.ViewModels.BtoBInstructionReference.BtoBInstructionReference02SearchConditions;

    public class BtoBInstructionReferenceQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertShpBtoBInstructionReference(BtoBInstructionReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                // 1.ワークID採番
                var seq = new BaseQuery().GetWorkId();
                try
                {
                    query = new StringBuilder(@"
                        INSERT INTO WW_SHP_BTO_B_INSTRUCTION_REFERENCE (
                                MAKE_DATE
                            ,   MAKE_USER_ID
                            ,   MAKE_PROGRAM_NAME
                            ,   UPDATE_DATE
                            ,   UPDATE_USER_ID
                            ,   UPDATE_PROGRAM_NAME
                            ,   UPDATE_COUNT
                            ,   SHIPPER_ID
                            ,   SEQ
                            ,   LINE_NO
                            ,   IS_CHECK
                            ,   CENTER_ID
                            ,   SHIP_PLAN_DATE
                            ,   INSTRUCT_CLASS_NAME
                            ,   EMERGENCY_CLASS_NAME
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   ITEM_ID
                            ,   ITEM_NAME
                            ,   ITEM_SKU_ID
                            ,   ITEM_COLOR_ID
                            ,   ITEM_COLOR_NAME
                            ,   ITEM_SIZE_ID
                            ,   ITEM_SIZE_NAME
                            ,   JAN
                            ,   SHIP_TO_STORE_ID
                            ,   SHIP_TO_STORE_NAME
                            ,   TRANSPORTER_NAME
                            ,   SHIP_TO_QTY
                            ,   INSTRUCT_QTY
                            ,   ALLOC_DATE
                            ,   COMPLETE_FLAG_NAME
                            ,   ALLOC_QTY
                            ,   PIC_QTY
                            ,   STOCK_OUT_REG_QTY
                            ,   STOCK_OUT_FIX_QTY
                            ,   RESULT_QTY
                            ,   SEQ_PRE
                        )
                        WITH
                            SELECTED_SHIP_PACKING AS (
                                SELECT
                                        SUM(CASE WHEN IF_STATE_ERP = 0 THEN RESULT_QTY ELSE 0 END) AS RESULT_QTY
                                    ,   SUM(CASE WHEN KAKU_FLAG = 1 AND IF_STATE_ERP = 0 THEN RESULT_QTY ELSE 0 END) AS KAKU_RESULT_QTY
                                    ,   SUM(CASE WHEN NOUHIN_PRN_FLAG IN (8, 9) AND DELI_PRN_FLAG <> 0 AND KAKU_FLAG = 0 THEN RESULT_QTY ELSE 0 END) AS NOUHIN_RESULT_QTY
                                    ,   SHIPPER_ID
                                    ,   CENTER_ID
                                    ,   SHIP_INSTRUCT_ID
                                    ,   SHIP_INSTRUCT_SEQ
                                    ,   MAX(BOX_NO) AS BOX_NO
                                    ,   0 AS PAST_FLAG
                                FROM
                                        T_SHIP_PACKING_INFO
                                WHERE
                                        CENTER_ID = :CENTER_ID
                                    AND SHIPPER_ID = :SHIPPER_ID
                                    AND EC_FLAG = 0
                    ");
                    // ケースNo
                    if (!string.IsNullOrEmpty(condition.BoxNo))
                    {
                        query.Append(@" AND BOX_NO = :BOX_NO ");
                        parameters.Add(":BOX_NO", condition.BoxNo);
                    }
                    //送り状No
                    if (!string.IsNullOrEmpty(condition.DeliNo))
                    {
                        query.Append(@" AND DELI_NO = :DELI_NO ");
                        parameters.Add(":DELI_NO", condition.DeliNo);
                    }
                    query.Append(@"
                                GROUP BY
                                        SHIP_INSTRUCT_ID
                                    ,   SHIP_INSTRUCT_SEQ
                                    ,   CENTER_ID
                                    ,   SHIPPER_ID
                        )
                    ");

                    if (condition.ShipAllocStatusOld)
                    {
                        query.Append(@"
                            ,   SELECTED_SHIP_A_PACKING AS (
                                    SELECT
                                            SUM(RESULT_QTY) RESULT_QTY
                                        ,   SUM(RESULT_QTY) AS KAKU_RESULT_QTY
                                        ,   SUM(RESULT_QTY) AS NOUHIN_RESULT_QTY
                                        ,   SHIPPER_ID
                                        ,   CENTER_ID
                                        ,   SHIP_INSTRUCT_ID
                                        ,   SHIP_INSTRUCT_SEQ
                                        ,   MAX(BOX_NO) AS BOX_NO
                                        ,   1 AS PAST_FLAG
                                    FROM
                                            A_SHIP_PACKING_INFO
                                    WHERE
                                            CENTER_ID = :CENTER_ID
                                        AND SHIPPER_ID = :SHIPPER_ID
                                        AND EC_FLAG = 0
                        ");
                        // ケースNo
                        if (!string.IsNullOrEmpty(condition.BoxNo))
                        {
                            query.Append(" AND BOX_NO = :BOX_NO ");
                            parameters.Add(":BOX_NO", condition.BoxNo);
                        }
                        //送り状No
                        if (!string.IsNullOrEmpty(condition.DeliNo))
                        {
                            query.Append(@" AND DELI_NO = :DELI_NO ");
                            parameters.Add(":DELI_NO", condition.DeliNo);
                        }
                        query.Append(@"
                                    GROUP BY
                                            SHIP_INSTRUCT_ID
                                        ,   SHIP_INSTRUCT_SEQ
                                        ,   CENTER_ID
                                        ,   SHIPPER_ID
                            )
                            ,   SELECTED_ALL_PACKING_DATA AS (
                                    SELECT * FROM SELECTED_SHIP_PACKING
                                    UNION
                                    SELECT * FROM SELECTED_SHIP_A_PACKING
                            )
                        ");
                    }
                    query.Append(@"
                        ,   SELECTED_SHIP_DATA AS (
                                SELECT
                                        SHIP_PLAN_DATE
                                    ,   COMPLETE_DATE
                                    ,   INSTRUCT_CLASS
                                    ,   EMERGENCY_CLASS
                                    ,   TRANSPORTER_ID
                                    ,   ITEM_SKU_ID
                                    ,   ITEM_ID
                                    ,   ITEM_NAME
                                    ,   ITEM_COLOR_ID
                                    ,   ITEM_SIZE_ID
                                    ,   JAN
                                    ,   SHIP_TO_STORE_ID
                                    ,   INSTRUCT_QTY
                                    ,   MAKE_DATE
                                    ,   ALLOC_QTY
                                    ,   STOCKOUT_QTY
                                    ,   SHIPPER_ID
                                    ,   CENTER_ID
                                    ,   SHIP_INSTRUCT_ID
                                    ,   SHIP_INSTRUCT_SEQ
                                    ,   BATCH_NO
                                    ,   BOX_NO
                                    ,   SHIP_KIND
                                    ,   AFT_ALLOC_STOP_FLAG
                                    ,   ALLOC_FLAG
                                    ,   COMPLETE_FLAG
                                    ,   STOP_FLAG
                                    ,   RESULT_QTY
                                    ,   IF_STATE_ERP
                                    ,   0 AS PAST_FLAG
                                FROM
                                        T_SHIPS
                                WHERE
                                        CENTER_ID = :CENTER_ID
                                    AND SHIPPER_ID = :SHIPPER_ID
                        )
                    ");
                    if (condition.ShipAllocStatusOld)
                    {
                        query.Append(@"
                            ,   SELECTED_A_SHIP_DATA AS (
                                    SELECT
                                            SHIP_PLAN_DATE
                                        ,   COMPLETE_DATE
                                        ,   INSTRUCT_CLASS
                                        ,   EMERGENCY_CLASS
                                        ,   TRANSPORTER_ID
                                        ,   ITEM_SKU_ID
                                        ,   ITEM_ID
                                        ,   ITEM_NAME
                                        ,   ITEM_COLOR_ID
                                        ,   ITEM_SIZE_ID
                                        ,   JAN
                                        ,   SHIP_TO_STORE_ID
                                        ,   INSTRUCT_QTY
                                        ,   MAKE_DATE
                                        ,   ALLOC_QTY
                                        ,   STOCKOUT_QTY
                                        ,   SHIPPER_ID
                                        ,   CENTER_ID
                                        ,   SHIP_INSTRUCT_ID
                                        ,   SHIP_INSTRUCT_SEQ
                                        ,   BATCH_NO
                                        ,   BOX_NO
                                        ,   SHIP_KIND
                                        ,   AFT_ALLOC_STOP_FLAG
                                        ,   ALLOC_FLAG
                                        ,   COMPLETE_FLAG
                                        ,   STOP_FLAG
                                        ,   RESULT_QTY
                                        ,   IF_STATE_ERP
                                        ,   1 AS PAST_FLAG
                                    FROM
                                            A_SHIPS
                                    WHERE
                                            CENTER_ID = :CENTER_ID
                                        AND SHIPPER_ID = :SHIPPER_ID
                            )
                            ,   SELECTED_ALL_SHIP_DATA AS (
                                    SELECT * FROM SELECTED_SHIP_DATA
                                    UNION
                                    SELECT * FROM SELECTED_A_SHIP_DATA
                            )
                        ");
                    }

                    if (condition.ResultType == ViewModels.BtoBInstructionReference.ResultTypes.Sku)
                    {
                        query.Append(@"
                            SELECT 
                                    SYSTIMESTAMP AS MAKE_DATE" +
                                ",  '" + Common.Profile.User.UserId + "' AS MAKE_USER_ID" +
                                ",  'BtoBInstructionReference'" + " AS MAKE_PROGRAM_NAME" +
                                ",  SYSTIMESTAMP AS UPDATE_DATE" +
                                ",  '" + Common.Profile.User.UserId + "' AS UPDATE_USER_ID" +
                                ",  'BtoBInstructionReference'" + " AS UPDATE_PROGRAM_NAME" +
                                ",  0 " + " AS UPDATE_COUNT" +
                                ",  " + "'" + Common.Profile.User.ShipperId + "'" + " AS SHIPPER_ID" +
                                ",  " + seq + " AS SEQ");
                        query.Append(@"
                                ,   ROW_NUMBER() OVER(ORDER BY SHIP.SHIP_INSTRUCT_ID, SHIP.ITEM_SKU_ID)
                                ,   0 AS IS_CHECK
                                ,   SHIP.CENTER_ID
                                ,   SHIP.SHIP_PLAN_DATE
                                ,   MAX(MG1.GEN_NAME) INSTRUCT_CLASS_NAME
                                ,   MAX(MG2.GEN_NAME) EMERGENCY_CLASS_NAME
                                ,   SHIP.SHIP_INSTRUCT_ID
                                ,   NULL AS SHIP_INSTRUCT_SEQ
                                ,   MAX(SHIP.ITEM_ID) ITEM_ID
                                ,   MAX(SHIP.ITEM_NAME) ITEM_NAME
                                ,   SHIP.ITEM_SKU_ID AS ITEM_SKU_ID
                                ,   MAX(SHIP.ITEM_COLOR_ID) ITEM_COLOR_ID
                                ,   MAX(MC.ITEM_COLOR_NAME) ITEM_COLOR_NAME
                                ,   MAX(SHIP.ITEM_SIZE_ID) ITEM_SIZE_ID
                                ,   MAX(MIS.ITEM_SIZE_NAME) ITEM_SIZE_NAME
                                ,   MAX(SHIP.JAN) AS JAN
                                ,   NULL AS SHIP_TO_STORE_ID
                                ,   NULL AS SHIP_TO_STORE_NAME
                                ,   NULL AS TRANSPORTER_NAME
                                ,   COUNT(DISTINCT(SHIP.SHIP_TO_STORE_ID)) SHIP_TO_QTY
                                ,   SUM(SHIP.INSTRUCT_QTY) INSTRUCT_QTY
                                ,   MAX(SHIP.MAKE_DATE) ALLOC_DATE
                                ,   CASE
                                        WHEN MAX(SHIP.PAST_FLAG) = 0 AND MIN(SHIP.ALLOC_FLAG) = 0 THEN '" + EcConfirmProgressResource.UnAlloc + @"'
                                        WHEN (  MIN(SHIP.COMPLETE_FLAG) = 1
                                            OR  MIN(SHIP.PAST_FLAG) = 1
                                            OR  SUM(SHIP.ALLOC_QTY) = (SUM(SHIP.RESULT_QTY) + SUM(NVL(PACK.KAKU_RESULT_QTY, 0)) + SUM(NVL(SRT.STOCK_OUT_FIX_QTY, 0)) + SUM(SHIP.STOCKOUT_QTY))
                                        ) THEN '" + BtoBInstructionReferenceResource.Complete + @"'
                                        ELSE '" + BtoBInstructionReferenceResource.Incomplete + @"'
                                    END COMPLETE_FLAG_NAME
                                ,   SUM(SHIP.ALLOC_QTY) ALLOC_QTY
                                ,   NULL AS PIC_QTY
                                ,   SUM(NVL(SRT.STOCK_OUT_REG_QTY, 0)) AS STOCK_OUT_REG_QTY
                                ,   SUM(NVL(SRT.STOCK_OUT_FIX_QTY, 0)) + SUM(SHIP.STOCKOUT_QTY) AS STOCK_OUT_FIX_QTY
                                ,   SUM(SHIP.RESULT_QTY) + SUM(NVL(PACK.RESULT_QTY, 0)) AS RESULT_QTY
                                ,   " + seq + " AS SEQ_PRE" +
                        "   FROM ");
                    }
                    else
                    {
                        query.Append(@"
                            SELECT 
                                    SYSTIMESTAMP AS MAKE_DATE" +
                                ",  '" + Common.Profile.User.UserId + "' AS MAKE_USER_ID" +
                                ",  'BtoBInstructionReference'" + " AS MAKE_PROGRAM_NAME" +
                                ",  SYSTIMESTAMP AS UPDATE_DATE" +
                                ",  '" + Common.Profile.User.UserId + "' AS UPDATE_USER_ID" +
                                ",  'BtoBInstructionReference'" + " AS UPDATE_PROGRAM_NAME" +
                                ",  0 " + " AS UPDATE_COUNT" +
                                ",  " + "'" + Common.Profile.User.ShipperId + "'" + " AS SHIPPER_ID" +
                                ",  " + seq + " AS SEQ");
                        query.Append(@"
                                ,   ROW_NUMBER() OVER(ORDER BY SHIP.SHIP_INSTRUCT_ID, SHIP.SHIP_INSTRUCT_SEQ)
                                ,   0 AS IS_CHECK
                                ,   SHIP.CENTER_ID
                                ,   SHIP.SHIP_PLAN_DATE
                                ,   MG1.GEN_NAME INSTRUCT_CLASS_NAME
                                ,   MG2.GEN_NAME EMERGENCY_CLASS_NAME
                                ,   SHIP.SHIP_INSTRUCT_ID
                                ,   SHIP.SHIP_INSTRUCT_SEQ
                                ,   SHIP.ITEM_ID
                                ,   SHIP.ITEM_NAME
                                ,   SHIP.ITEM_SKU_ID
                                ,   SHIP.ITEM_COLOR_ID
                                ,   MC.ITEM_COLOR_NAME
                                ,   SHIP.ITEM_SIZE_ID
                                ,   MIS.ITEM_SIZE_NAME
                                ,   SHIP.JAN AS JAN
                                ,   SHIP.SHIP_TO_STORE_ID
                                ,   VSTS.SHIP_TO_STORE_NAME1 SHIP_TO_STORE_NAME
                                ,   MT.TRANSPORTER_NAME
                                ,   NULL AS SHIP_TO_QTY
                                ,   SHIP.INSTRUCT_QTY
                                ,   SHIP.MAKE_DATE AS ALLOC_DATE
                                ,   CASE
                                        WHEN SHIP.PAST_FLAG = 1 THEN '" + BtoBReferenceResource.Daily + @"'
                                        WHEN SHIP.STOP_FLAG = 1 THEN '" + BtoBReferenceResource.ShipStop + @"'
                                        WHEN SHIP.ALLOC_FLAG = 0 THEN '" + EcConfirmProgressResource.UnAlloc + @"'
                                        WHEN SHIP.ALLOC_FLAG = 1 AND SHIP.ALLOC_QTY = 0 THEN '" + BtoBReferenceResource.AllStock + @"'
                                        WHEN SHIP.IF_STATE_ERP = 2 THEN '" + BtoBReferenceResource.Send + @"'
                                        WHEN (  SHIP.PAST_FLAG = 0
                                            AND SHIP.ALLOC_FLAG = 1 
                                            AND SHIP.ALLOC_QTY = (SHIP.RESULT_QTY + NVL(PACK.KAKU_RESULT_QTY, 0) + NVL(SRT.STOCK_OUT_FIX_QTY, 0) + SHIP.STOCKOUT_QTY)
                                            ) THEN '" + BtoBReferenceResource.Confirm + @"'
                                        WHEN (  SHIP.PAST_FLAG = 0
                                            AND SHIP.ALLOC_FLAG = 1
                                            AND SHIP.ALLOC_QTY = (SHIP.RESULT_QTY + NVL(PACK.NOUHIN_RESULT_QTY, 0) + NVL(SRT.STOCK_OUT_FIX_QTY, 0) + SHIP.STOCKOUT_QTY)
                                            ) THEN '" + BtoBReferenceResource.Print + @"'
                                        ELSE '" + BtoBReferenceResource.Shipping + @"'
                                    END COMPLETE_FLAG_NAME
                                ,   SHIP.ALLOC_QTY
                                ,   NULL AS PIC_QTY
                                ,   CASE
                                        WHEN SHIP.PAST_FLAG = 1 THEN 0
                                        ELSE NVL(SRT.STOCK_OUT_REG_QTY, 0)
                                    END AS STOCK_OUT_REG_QTY
                                ,   CASE
                                        WHEN SHIP.PAST_FLAG = 1 OR SHIP.SHIP_KIND = 4 THEN SHIP.STOCKOUT_QTY
                                        ELSE NVL(SRT.STOCK_OUT_FIX_QTY, 0)
                                    END AS STOCK_OUT_FIX_QTY
                                ,   CASE
                                        WHEN SHIP.PAST_FLAG = 1 THEN SHIP.RESULT_QTY
                                        ELSE SHIP.RESULT_QTY + NVL(PACK.RESULT_QTY, 0)
                                    END AS RESULT_QTY
                                ,   " + condition.Seq + " AS SEQ_PRE" +
                        "   FROM "); 
                    }
                    if (condition.ShipAllocStatusOld)
                    {
                        //過去分含む
                        query.Append(@" SELECTED_ALL_SHIP_DATA SHIP ");
                        if (!string.IsNullOrEmpty(condition.BoxNo) || !string.IsNullOrEmpty(condition.DeliNo))
                        {
                            query.Append(" INNER JOIN ");
                        }
                        else
                        {
                            query.Append(" LEFT OUTER JOIN ");
                        }
                        query.Append(@" SELECTED_ALL_PACKING_DATA PACK ");
                    }
                    else
                    {
                        //過去分含まない
                        query.Append(@" SELECTED_SHIP_DATA SHIP ");
                        if (!string.IsNullOrEmpty(condition.BoxNo) || !string.IsNullOrEmpty(condition.DeliNo))
                        {
                            query.Append(" INNER JOIN ");
                        }
                        else
                        {
                            query.Append(" LEFT OUTER JOIN ");
                        }
                        query.Append(@" SELECTED_SHIP_PACKING PACK ");
                    }
                    query.Append(@"
                        ON
                                SHIP.SHIP_INSTRUCT_ID = PACK.SHIP_INSTRUCT_ID
                            AND SHIP.SHIP_INSTRUCT_SEQ = PACK.SHIP_INSTRUCT_SEQ
                            AND SHIP.CENTER_ID = PACK.CENTER_ID
                            AND SHIP.SHIPPER_ID = PACK.SHIPPER_ID
                            AND PACK.PAST_FLAG = 0
                        LEFT OUTER JOIN
                                T_STORE_SORT SRT
                        ON
                                SHIP.BATCH_NO = SRT.BATCH_NO
                            AND SHIP.SHIP_INSTRUCT_ID = SRT.SHIP_INSTRUCT_ID
                            AND SHIP.SHIP_INSTRUCT_SEQ = SRT.SHIP_INSTRUCT_SEQ
                            AND SHIP.CENTER_ID = SRT.CENTER_ID
                            AND SHIP.SHIPPER_ID = SRT.SHIPPER_ID
                        LEFT OUTER JOIN
                                M_ITEM_SKU MIS
                        ON
                                SHIP.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                            AND SHIP.SHIPPER_ID = MIS.SHIPPER_ID
                        LEFT OUTER JOIN
                                M_BRANDS MB
                        ON
                                MIS.BRAND_ID = MB.BRAND_ID
                            AND MIS.SHIPPER_ID   = MB.SHIPPER_ID
                        LEFT OUTER JOIN
                                M_VENDORS MV
                        ON
                                MIS.MAIN_VENDOR_ID = MV.VENDOR_ID
                            AND MIS.SHIPPER_ID   = MV.SHIPPER_ID
                        LEFT OUTER JOIN
                                V_SHIP_TO_STORES VSTS
                        ON
                                SHIP.SHIP_TO_STORE_ID  = VSTS.SHIP_TO_STORE_ID
                            AND SHIP.SHIPPER_ID = VSTS.SHIPPER_ID
                        LEFT OUTER JOIN
                                M_TRANSPORTERS MT
                        ON
                                SHIP.TRANSPORTER_ID = MT.TRANSPORTER_ID
                            AND SHIP.SHIPPER_ID   = MT.SHIPPER_ID
                        LEFT OUTER JOIN
                                M_COLORS MC
                        ON
                                SHIP.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                            AND SHIP.SHIPPER_ID   = MC.SHIPPER_ID
                        LEFT OUTER JOIN
                                M_SIZES MS
                        ON
                                SHIP.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                            AND SHIP.SHIPPER_ID   = MS.SHIPPER_ID
                        LEFT OUTER JOIN
                                M_GENERALS MG1
                        ON
                                SHIP.SHIPPER_ID = MG1.SHIPPER_ID
                            AND MG1.REGISTER_DIVI_CD = '1'
                            AND MG1.CENTER_ID = '@@@'
                            AND MG1.GEN_DIV_CD = 'INSTRUCT_CLASS'
                            AND TO_CHAR(SHIP.INSTRUCT_CLASS) = MG1.GEN_CD
                        LEFT OUTER JOIN
                                M_GENERALS MG2
                        ON
                                SHIP.SHIPPER_ID = MG2.SHIPPER_ID
                            AND MG2.REGISTER_DIVI_CD = '1'
                            AND MG2.CENTER_ID = '@@@'
                            AND MG2.GEN_DIV_CD = 'EMERGENCY_CLASS'
                            AND TO_CHAR(SHIP.EMERGENCY_CLASS) = MG2.GEN_CD
                        WHERE
                                1 = 1
                     ");
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                    parameters.Add(":CENTER_ID", condition.CenterId);
                    // 受信日時(From-To)
                    if (condition.MakeDateFrom != null)
                    {
                        query.Append(" AND TO_CHAR(SHIP.MAKE_DATE,'yyyy/MM/dd hh24:mi:ss') >= :MAKE_DATE_FROM ");
                        parameters.Add(":MAKE_DATE_FROM", condition.MakeTimeFrom == null ?
                                                                string.Format("{0:yyyy/MM/dd}", condition.MakeDateFrom) + " 00:00:00" :
                                                                string.Format("{0:yyyy/MM/dd}", condition.MakeDateFrom) + " " + condition.MakeTimeFrom);
                    }
                    if (condition.MakeDateTo != null)
                    {
                        query.Append(" AND TO_CHAR(SHIP.MAKE_DATE,'yyyy/MM/dd hh24:mi:ss') <= :MAKE_DATE_TO ");
                        parameters.Add(":MAKE_DATE_TO", condition.MakeTimeTo == null ?
                                                                string.Format("{0:yyyy/MM/dd}", condition.MakeDateTo) + " 23:59:59" :
                                                                string.Format("{0:yyyy/MM/dd}", condition.MakeDateTo) + " " + condition.MakeTimeTo);
                    }

                    // 出荷予定日(From-To)
                    if (condition.ShipPlanDateFrom != null)
                    {
                        query.Append(" AND SHIP.SHIP_PLAN_DATE >= :SHIP_PLAN_DATE_FROM ");
                        parameters.Add(":SHIP_PLAN_DATE_FROM", condition.ShipPlanDateFrom);
                    }

                    if (condition.ShipPlanDateTo != null)
                    {
                        query.Append(" AND SHIP.SHIP_PLAN_DATE <= :SHIP_PLAN_DATE_TO ");
                        parameters.Add(":SHIP_PLAN_DATE_TO", condition.ShipPlanDateTo);
                    }

                    // 出荷完了日(From-To)
                    if (condition.CompleteDateFrom != null)
                    {
                        query.Append(" AND TRUNC(SHIP.COMPLETE_DATE) >= :COMPLETE_DATE_FROM ");
                        parameters.Add(":COMPLETE_DATE_FROM", condition.CompleteDateFrom);
                    }

                    if (condition.CompleteDateTo != null)
                    {
                        query.Append(" AND TRUNC(SHIP.COMPLETE_DATE) <= :COMPLETE_DATE_TO ");
                        parameters.Add(":COMPLETE_DATE_TO", condition.CompleteDateTo);
                    }

                    // 確定状態
                    if (!string.IsNullOrEmpty(condition.ShipAllocStatus))
                    {
                        if (condition.ShipAllocStatus == "0")   //未引当
                        {
                            query.Append(" AND SHIP.PAST_FLAG = 0 AND SHIP.ALLOC_FLAG = 0 ");
                        }
                        else if (condition.ShipAllocStatus == "1")  //未完了
                        {
                            query.Append(@" AND SHIP.ALLOC_FLAG = 1
                                            AND SHIP.COMPLETE_FLAG = 0
                                            AND SHIP.ALLOC_QTY <> (SHIP.RESULT_QTY + NVL(PACK.KAKU_RESULT_QTY, 0) + NVL(SRT.STOCK_OUT_FIX_QTY, 0) + SHIP.STOCKOUT_QTY)
                            ");
                        }
                        else if (condition.ShipAllocStatus == "2")  //完了
                        {
                            query.Append(@" AND SHIP.ALLOC_FLAG = 1
                                            AND (
                                                    SHIP.COMPLETE_FLAG = 1
                                                OR  SHIP.PAST_FLAG = 1
                                                OR  SHIP.ALLOC_QTY = (SHIP.RESULT_QTY + NVL(PACK.KAKU_RESULT_QTY, 0) + NVL(SRT.STOCK_OUT_FIX_QTY, 0) + SHIP.STOCKOUT_QTY)
                                            )
                            ");
                        }
                        else if (condition.ShipAllocStatus == "3")  //引当済全て
                        {
                            query.Append(@" AND SHIP.ALLOC_FLAG = 1 ");
                        }
                    }

                    // 指示区分
                    if (!string.IsNullOrEmpty(condition.InstructClass))
                    {
                        query.Append(" AND SHIP.INSTRUCT_CLASS = :INSTRUCT_CLASS ");
                        parameters.Add(":INSTRUCT_CLASS", condition.InstructClass);
                    }

                    // 出荷先区分
                    if (!string.IsNullOrEmpty(condition.StoreClass))
                    {
                        query.Append(" AND VSTS.SHIP_TO_STORE_CLASS = :SHIP_TO_STORE_CLASS ");
                        parameters.Add(":SHIP_TO_STORE_CLASS", condition.StoreClass);
                    }

                    // 出荷先
                    if (!string.IsNullOrEmpty(condition.ShipToStoreId))
                    {
                        query.Append(" AND SHIP.SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID ");
                        parameters.Add(":SHIP_TO_STORE_ID", condition.ShipToStoreId);
                    }

                    // 配送業者
                    if (!string.IsNullOrEmpty(condition.TransporterId))
                    {
                        query.Append(" AND SHIP.TRANSPORTER_ID = :TRANSPORTER_ID ");
                        parameters.Add(":TRANSPORTER_ID", condition.TransporterId);
                    }

                    // 緊急
                    if (!string.IsNullOrEmpty(condition.EmergencyClass))
                    {
                        query.Append(" AND SHIP.EMERGENCY_CLASS = :EMERGENCY_CLASS ");
                        parameters.Add(":EMERGENCY_CLASS", condition.EmergencyClass);
                    }

                    // 出荷指示ID
                    if (!string.IsNullOrEmpty(condition.ShipInstructId))
                    {
                        query.Append(" AND SHIP.SHIP_INSTRUCT_ID LIKE :SHIP_INSTRUCT_ID ");
                        parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId + "%");
                    }

                    // バッチNo
                    if (!string.IsNullOrEmpty(condition.BatchNo))
                    {
                        query.Append(" AND SHIP.BATCH_NO = :BATCH_NO ");
                        parameters.Add(":BATCH_NO", condition.BatchNo);
                    }

                    // キャンセル
                    if (condition.CancelFlag)
                    {
                        query.Append(" AND SHIP.AFT_ALLOC_STOP_FLAG = 1 ");
                    }

                    // 事業部
                    if (!string.IsNullOrEmpty(condition.DivisionId))
                    {
                        query.Append(" AND MIS.DIVISION_ID = :DIVISION_ID ");
                        parameters.Add(":DIVISION_ID", condition.DivisionId);
                    }

                    // ブランド
                    if (!string.IsNullOrEmpty(condition.BrandId))
                    {
                        query.Append(" AND MIS.BRAND_ID LIKE :BRAND_ID ");
                        parameters.Add(":BRAND_ID", condition.BrandId + "%");
                    }

                    // ブランド名
                    if (string.IsNullOrEmpty(condition.BrandId) && !string.IsNullOrEmpty(condition.BrandName))
                    {
                        query.Append(" AND MB.BRAND_NAME LIKE :BRAND_NAME ");
                        parameters.Add(":BRAND_NAME", condition.BrandName + "%");
                    }

                    // 代表仕入先
                    if (!string.IsNullOrEmpty(condition.MainVendorId))
                    {
                        query.Append(" AND MIS.MAIN_VENDOR_ID LIKE :MAIN_VENDOR_ID ");
                        parameters.Add(":MAIN_VENDOR_ID", condition.MainVendorId + "%");
                    }

                    // 代表仕入先名
                    if (string.IsNullOrEmpty(condition.MainVendorId) && !string.IsNullOrEmpty(condition.MainVendorName))
                    {
                        query.Append(" AND MV.VENDOR_NAME1 LIKE :VENDOR_NAME1 ");
                        parameters.Add(":VENDOR_NAME1", condition.MainVendorName + "%");
                    }

                    // 分類
                    if (!string.IsNullOrEmpty(condition.CategoryId1))
                    {
                        query.Append(" AND MIS.CATEGORY_ID1 = :CATEGORY_ID1 ");
                        parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId2))
                    {
                        query.Append(" AND MIS.CATEGORY_ID2 = :CATEGORY_ID2 ");
                        parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId3))
                    {
                        query.Append(" AND MIS.CATEGORY_ID3 = :CATEGORY_ID3 ");
                        parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId4))
                    {
                        query.Append(" AND MIS.CATEGORY_ID4 = :CATEGORY_ID4 ");
                        parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
                    }

                    // アイテム
                    if (!string.IsNullOrEmpty(condition.ItemCode))
                    {
                        query.Append(" AND MIS.ITEM_CODE = :ITEM_CODE ");
                        parameters.Add(":ITEM_CODE", condition.ItemCode);
                    }

                    // 品番
                    if (!string.IsNullOrEmpty(condition.ItemId))
                    {
                        query.Append(" AND SHIP.ITEM_ID LIKE :ITEM_ID ");
                        parameters.Add(":ITEM_ID", condition.ItemId + "%");
                    }

                    // JAN
                    if (!string.IsNullOrEmpty(condition.Jan))
                    {
                        query.Append(" AND SHIP.JAN LIKE :JAN ");
                        parameters.Add(":JAN", condition.Jan + "%");
                    }

                    // SKU
                    if (!string.IsNullOrEmpty(condition.ItemSkuId))
                    {
                        query.Append(" AND SHIP.ITEM_SKU_ID LIKE :ITEM_SKU_ID ");
                        parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId + "%");
                    }
                    //欠品項目チェックあり
                    if(condition.StockOutRegFlag == true || condition.StockOutFixFlag == true || condition.LackOfResultFlag == true)
                    {
                        query.Append(@" AND ( ");
                        // 欠品登録あり
                        if (condition.StockOutRegFlag)
                        {
                            query.Append(@" NVL(SRT.STOCK_OUT_REG_QTY, 0) > 0 ");
                        }
                        // 欠品確定あり
                        if (condition.StockOutFixFlag)
                        {
                            if (condition.StockOutRegFlag) { query.Append(@" OR ");  };
                            query.Append(" ( NVL(SRT.STOCK_OUT_FIX_QTY, 0) > 0 OR NVL(SHIP.STOCKOUT_QTY, 0) > 0 )");
                        }
                        // 実績数不足あり
                        if (condition.LackOfResultFlag)
                        {
                            if (condition.StockOutRegFlag == true || condition.StockOutFixFlag == true) { query.Append(@" OR "); };
                            query.Append(@"
                                (   NVL(SRT.STOCK_OUT_REG_QTY, 0) = 0
                                AND NVL(SRT.STOCK_OUT_FIX_QTY, 0) = 0
                                AND NVL(SHIP.STOCKOUT_QTY, 0) = 0
                                AND SHIP.ALLOC_QTY > SHIP.RESULT_QTY + NVL(PACK.RESULT_QTY, 0) )
                            ");
                        }
                        query.Append(@" ) ");
                    }

                    //SKU一覧の場合、集約
                    if (condition.ResultType == ViewModels.BtoBInstructionReference.ResultTypes.Sku)
                    {
                        query.Append(@"
                            GROUP BY
                                    SHIP.CENTER_ID
                                ,   SHIP.SHIP_PLAN_DATE
                                ,   SHIP.SHIP_INSTRUCT_ID
                                ,   SHIP.ITEM_SKU_ID ");
                    }
                    // 2.検索・ワーク作成
                    var result = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }
                trans.Commit();
                condition.Seq = seq;
                condition.Page = 1;
            }

            return true;
        }

        /// <summary>
        /// Insert Work Table 出荷指示明細
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertSelectedShpBtoBInstructionReference(BtoBInstructionReference01SearchConditions condition)
        {
            // 1.ワークID採番
            var seq = new BaseQuery().GetWorkId();

            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    query = new StringBuilder(@"
                        INSERT INTO WW_SHP_BTO_B_INSTRUCTION_REFERENCE (
                                MAKE_DATE
                            ,   MAKE_USER_ID
                            ,   MAKE_PROGRAM_NAME
                            ,   UPDATE_DATE
                            ,   UPDATE_USER_ID
                            ,   UPDATE_PROGRAM_NAME
                            ,   UPDATE_COUNT
                            ,   SHIPPER_ID
                            ,   SEQ
                            ,   LINE_NO
                            ,   IS_CHECK
                            ,   CENTER_ID
                            ,   SHIP_PLAN_DATE
                            ,   INSTRUCT_CLASS_NAME
                            ,   EMERGENCY_CLASS_NAME
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   ITEM_ID
                            ,   ITEM_NAME
                            ,   ITEM_SKU_ID
                            ,   ITEM_COLOR_ID
                            ,   ITEM_COLOR_NAME
                            ,   ITEM_SIZE_ID
                            ,   ITEM_SIZE_NAME
                            ,   JAN
                            ,   SHIP_TO_STORE_ID
                            ,   SHIP_TO_STORE_NAME
                            ,   TRANSPORTER_NAME
                            ,   SHIP_TO_QTY
                            ,   INSTRUCT_QTY
                            ,   ALLOC_DATE
                            ,   COMPLETE_FLAG_NAME
                            ,   ALLOC_QTY
                            ,   PIC_QTY
                            ,   STOCK_OUT_REG_QTY
                            ,   STOCK_OUT_FIX_QTY
                            ,   RESULT_QTY
                            ,   SEQ_PRE
                        )
                        WITH
                            SELECTED_WORK_DATA AS (
                                SELECT
                                        SHIP_INSTRUCT_ID
                                    ,   ITEM_SKU_ID
                                    ,   CENTER_ID
                                    ,   SHIPPER_ID
                                FROM
                                        WW_SHP_BTO_B_INSTRUCTION_REFERENCE
                                WHERE
                                        SEQ = :SEQ
                                    AND SHIPPER_ID = :SHIPPER_ID
                                    AND IS_CHECK = 1
                        )
                        ,   SELECTED_SHIP_PACKING AS (
                                SELECT
                                        SUM(CASE WHEN IF_STATE_ERP = 0 THEN RESULT_QTY ELSE 0 END) AS RESULT_QTY
                                    ,   SUM(CASE WHEN KAKU_FLAG = 1 AND IF_STATE_ERP = 0 THEN RESULT_QTY ELSE 0 END) AS KAKU_RESULT_QTY
                                    ,   SUM(CASE WHEN NOUHIN_PRN_FLAG IN (8, 9) AND DELI_PRN_FLAG <> 0 AND KAKU_FLAG = 0 THEN RESULT_QTY ELSE 0 END) AS NOUHIN_RESULT_QTY
                                    ,   SHIPPER_ID
                                    ,   CENTER_ID
                                    ,   SHIP_INSTRUCT_ID
                                    ,   SHIP_INSTRUCT_SEQ
                                    ,   MAX(BOX_NO) AS BOX_NO
                                    ,   0 AS PAST_FLAG
                                FROM
                                        T_SHIP_PACKING_INFO
                                WHERE
                                        CENTER_ID = :CENTER_ID
                                    AND SHIPPER_ID = :SHIPPER_ID
                                    AND EC_FLAG = 0
                                GROUP BY
                                        SHIP_INSTRUCT_ID
                                    ,   SHIP_INSTRUCT_SEQ
                                    ,   CENTER_ID
                                    ,   SHIPPER_ID
                        )
                    ");
                    if (condition.ShipAllocStatusOld)
                    {
                        query.Append(@"
                            ,   SELECTED_SHIP_A_PACKING AS (
                                    SELECT
                                            SUM(RESULT_QTY) RESULT_QTY
                                        ,   SUM(RESULT_QTY) AS KAKU_RESULT_QTY
                                        ,   SUM(RESULT_QTY) AS NOUHIN_RESULT_QTY
                                        ,   SHIPPER_ID
                                        ,   CENTER_ID
                                        ,   SHIP_INSTRUCT_ID
                                        ,   SHIP_INSTRUCT_SEQ
                                        ,   MAX(BOX_NO) AS BOX_NO
                                        ,   1 AS PAST_FLAG
                                    FROM
                                            A_SHIP_PACKING_INFO
                                    WHERE
                                            CENTER_ID = :CENTER_ID
                                        AND SHIPPER_ID = :SHIPPER_ID
                                        AND EC_FLAG = 0
                                    GROUP BY
                                            SHIP_INSTRUCT_ID
                                        ,   SHIP_INSTRUCT_SEQ
                                        ,   CENTER_ID
                                        ,   SHIPPER_ID
                            )
                            ,   SELECTED_ALL_PACKING_DATA AS (
                                    SELECT * FROM SELECTED_SHIP_PACKING
                                    UNION
                                    SELECT * FROM SELECTED_SHIP_A_PACKING
                            )
                        ");
                    }
                    query.Append(@"
                        ,   SELECTED_SHIP_DATA AS (
                                SELECT
                                        SHIP_PLAN_DATE
                                    ,   COMPLETE_DATE
                                    ,   INSTRUCT_CLASS
                                    ,   EMERGENCY_CLASS
                                    ,   TRANSPORTER_ID
                                    ,   ITEM_SKU_ID
                                    ,   ITEM_ID
                                    ,   ITEM_NAME
                                    ,   ITEM_COLOR_ID
                                    ,   ITEM_SIZE_ID
                                    ,   JAN
                                    ,   SHIP_TO_STORE_ID
                                    ,   INSTRUCT_QTY
                                    ,   MAKE_DATE
                                    ,   ALLOC_QTY
                                    ,   STOCKOUT_QTY
                                    ,   SHIPPER_ID
                                    ,   CENTER_ID
                                    ,   SHIP_INSTRUCT_ID
                                    ,   SHIP_INSTRUCT_SEQ
                                    ,   BATCH_NO
                                    ,   BOX_NO
                                    ,   SHIP_KIND
                                    ,   AFT_ALLOC_STOP_FLAG
                                    ,   ALLOC_FLAG
                                    ,   COMPLETE_FLAG
                                    ,   STOP_FLAG
                                    ,   RESULT_QTY
                                    ,   IF_STATE_ERP
                                    ,   0 AS PAST_FLAG
                                FROM
                                        T_SHIPS
                                WHERE
                                        CENTER_ID = :CENTER_ID
                                    AND SHIPPER_ID = :SHIPPER_ID
                        )
                    ");
                    if (condition.ShipAllocStatusOld)
                    {
                        query.Append(@"
                            ,   SELECTED_A_SHIP_DATA AS (
                                    SELECT
                                            SHIP_PLAN_DATE
                                        ,   COMPLETE_DATE
                                        ,   INSTRUCT_CLASS
                                        ,   EMERGENCY_CLASS
                                        ,   TRANSPORTER_ID
                                        ,   ITEM_SKU_ID
                                        ,   ITEM_ID
                                        ,   ITEM_NAME
                                        ,   ITEM_COLOR_ID
                                        ,   ITEM_SIZE_ID
                                        ,   JAN
                                        ,   SHIP_TO_STORE_ID
                                        ,   INSTRUCT_QTY
                                        ,   MAKE_DATE
                                        ,   ALLOC_QTY
                                        ,   STOCKOUT_QTY
                                        ,   SHIPPER_ID
                                        ,   CENTER_ID
                                        ,   SHIP_INSTRUCT_ID
                                        ,   SHIP_INSTRUCT_SEQ
                                        ,   BATCH_NO
                                        ,   BOX_NO
                                        ,   SHIP_KIND
                                        ,   AFT_ALLOC_STOP_FLAG
                                        ,   ALLOC_FLAG
                                        ,   COMPLETE_FLAG
                                        ,   STOP_FLAG
                                        ,   RESULT_QTY
                                        ,   IF_STATE_ERP
                                        ,   1 AS PAST_FLAG
                                    FROM
                                            A_SHIPS
                                    WHERE
                                            CENTER_ID = :CENTER_ID
                                        AND SHIPPER_ID = :SHIPPER_ID
                            )
                            ,   SELECTED_ALL_SHIP_DATA AS (
                                    SELECT * FROM SELECTED_SHIP_DATA
                                    UNION
                                    SELECT * FROM SELECTED_A_SHIP_DATA
                            )
                        ");
                    }
                    query.Append(@"
                        SELECT 
                                SYSTIMESTAMP AS MAKE_DATE" +
                            ",  '" + Common.Profile.User.UserId + "' AS MAKE_USER_ID" +
                            ",  'BtoBInstructionReference'" + " AS MAKE_PROGRAM_NAME" +
                            ",  SYSTIMESTAMP AS UPDATE_DATE" +
                            ",  '" + Common.Profile.User.UserId + "' AS UPDATE_USER_ID" +
                            ",  'BtoBInstructionReference'" + " AS UPDATE_PROGRAM_NAME" +
                            ",  0 " + " AS UPDATE_COUNT" +
                            ",  " + "'" + Common.Profile.User.ShipperId + "'" + " AS SHIPPER_ID" +
                            ",  " + seq + " AS SEQ");
                    query.Append(@"
                            ,   ROW_NUMBER() OVER(ORDER BY SHIP.SHIP_INSTRUCT_ID, SHIP.SHIP_INSTRUCT_SEQ)
                            ,   0 AS IS_CHECK
                            ,   SHIP.CENTER_ID
                            ,   SHIP.SHIP_PLAN_DATE
                            ,   MG1.GEN_NAME INSTRUCT_CLASS_NAME
                            ,   MG2.GEN_NAME EMERGENCY_CLASS_NAME
                            ,   SHIP.SHIP_INSTRUCT_ID
                            ,   SHIP.SHIP_INSTRUCT_SEQ
                            ,   SHIP.ITEM_ID
                            ,   SHIP.ITEM_NAME
                            ,   SHIP.ITEM_SKU_ID
                            ,   SHIP.ITEM_COLOR_ID
                            ,   MC.ITEM_COLOR_NAME
                            ,   SHIP.ITEM_SIZE_ID
                            ,   MIS.ITEM_SIZE_NAME
                            ,   SHIP.JAN AS JAN
                            ,   SHIP.SHIP_TO_STORE_ID
                            ,   VSTS.SHIP_TO_STORE_NAME1 SHIP_TO_STORE_NAME
                            ,   MT.TRANSPORTER_NAME
                            ,   NULL AS SHIP_TO_QTY
                            ,   SHIP.INSTRUCT_QTY
                            ,   SHIP.MAKE_DATE AS ALLOC_DATE
                            ,   CASE
                                    WHEN SHIP.PAST_FLAG = 1 THEN '" + BtoBReferenceResource.Daily + @"'
                                    WHEN SHIP.STOP_FLAG = 1 THEN '" + BtoBReferenceResource.ShipStop + @"'
                                    WHEN SHIP.ALLOC_FLAG = 0 THEN '" + EcConfirmProgressResource.UnAlloc + @"'
                                    WHEN SHIP.ALLOC_FLAG = 1 AND SHIP.ALLOC_QTY = 0 THEN '" + BtoBReferenceResource.AllStock + @"'
                                    WHEN SHIP.IF_STATE_ERP = 2 THEN '" + BtoBReferenceResource.Send + @"'
                                    WHEN (  SHIP.PAST_FLAG = 0
                                        AND SHIP.ALLOC_FLAG = 1 
                                        AND SHIP.ALLOC_QTY = (SHIP.RESULT_QTY + NVL(PACK.KAKU_RESULT_QTY, 0) + NVL(SRT.STOCK_OUT_FIX_QTY, 0))
                                        ) THEN '" + BtoBReferenceResource.Confirm + @"'
                                    WHEN (  SHIP.PAST_FLAG = 0
                                        AND SHIP.ALLOC_FLAG = 1
                                        AND SHIP.ALLOC_QTY = (SHIP.RESULT_QTY + NVL(PACK.NOUHIN_RESULT_QTY, 0) + NVL(SRT.STOCK_OUT_FIX_QTY, 0))
                                        ) THEN '" + BtoBReferenceResource.Print + @"'
                                    ELSE '" + BtoBReferenceResource.Shipping + @"'
                                END COMPLETE_FLAG_NAME
                            ,   SHIP.ALLOC_QTY
                            ,   NULL AS PIC_QTY
                            ,   CASE
                                    WHEN SHIP.PAST_FLAG = 1 THEN 0
                                    ELSE NVL(SRT.STOCK_OUT_REG_QTY, 0)
                                END AS STOCK_OUT_REG_QTY
                            ,   CASE
                                    WHEN SHIP.PAST_FLAG = 1 OR SHIP.SHIP_KIND = 4 THEN SHIP.STOCKOUT_QTY
                                    ELSE NVL(SRT.STOCK_OUT_FIX_QTY, 0)
                                END AS STOCK_OUT_FIX_QTY
                            ,   CASE
                                    WHEN SHIP.PAST_FLAG = 1 THEN SHIP.RESULT_QTY
                                    ELSE SHIP.RESULT_QTY + NVL(PACK.RESULT_QTY, 0)
                                END AS RESULT_QTY
                            ,   :SEQ AS SEQ_PRE
                        FROM ");
                    if (condition.ShipAllocStatusOld)
                    {
                        //過去分含む
                        query.Append(@" SELECTED_ALL_SHIP_DATA SHIP ");
                    }
                    else
                    {
                        //過去分含まない
                        query.Append(@" SELECTED_SHIP_DATA SHIP ");
                    }
                    query.Append(@"
                        INNER JOIN
                                SELECTED_WORK_DATA WK
                        ON
                                SHIP.SHIP_INSTRUCT_ID = WK.SHIP_INSTRUCT_ID
                            AND SHIP.CENTER_ID = WK.CENTER_ID
                            AND SHIP.SHIPPER_ID = WK.SHIPPER_ID
                            AND SHIP.ITEM_SKU_ID = WK.ITEM_SKU_ID
                    ");
                    if (condition.ShipAllocStatusOld)
                    {
                        //過去分含む
                        query.Append(@" LEFT OUTER JOIN ");
                        query.Append(@" SELECTED_ALL_PACKING_DATA PACK ");
                    }
                    else
                    {
                        //過去分含まない
                        query.Append(@" LEFT OUTER JOIN ");
                        query.Append(@" SELECTED_SHIP_PACKING PACK ");
                    }
                    query.Append(@"
                        ON
                                SHIP.SHIP_INSTRUCT_ID = PACK.SHIP_INSTRUCT_ID
                            AND SHIP.SHIP_INSTRUCT_SEQ = PACK.SHIP_INSTRUCT_SEQ
                            AND SHIP.CENTER_ID = PACK.CENTER_ID
                            AND SHIP.SHIPPER_ID = PACK.SHIPPER_ID
                            AND PACK.PAST_FLAG = 0
                        LEFT OUTER JOIN
                                T_STORE_SORT SRT
                        ON
                                SHIP.BATCH_NO = SRT.BATCH_NO
                            AND SHIP.SHIP_INSTRUCT_ID = SRT.SHIP_INSTRUCT_ID
                            AND SHIP.SHIP_INSTRUCT_SEQ = SRT.SHIP_INSTRUCT_SEQ
                            AND SHIP.CENTER_ID = SRT.CENTER_ID
                            AND SHIP.SHIPPER_ID = SRT.SHIPPER_ID
                        LEFT OUTER JOIN
                                V_SHIP_TO_STORES VSTS
                        ON
                                SHIP.SHIP_TO_STORE_ID  = VSTS.SHIP_TO_STORE_ID
                            AND SHIP.SHIPPER_ID = VSTS.SHIPPER_ID
                        LEFT OUTER JOIN
                                M_TRANSPORTERS MT
                        ON
                                SHIP.TRANSPORTER_ID = MT.TRANSPORTER_ID
                            AND SHIP.SHIPPER_ID   = MT.SHIPPER_ID
                        LEFT OUTER JOIN
                                M_COLORS MC
                        ON
                                SHIP.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                            AND SHIP.SHIPPER_ID   = MC.SHIPPER_ID
                        LEFT OUTER JOIN
                                M_ITEM_SKU MIS
                        ON
                                SHIP.SHIPPER_ID = MIS.SHIPPER_ID
                            AND SHIP.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                        LEFT OUTER JOIN
                                M_GENERALS MG1
                        ON
                                SHIP.SHIPPER_ID = MG1.SHIPPER_ID
                            AND MG1.REGISTER_DIVI_CD = '1'
                            AND MG1.CENTER_ID = '@@@'
                            AND MG1.GEN_DIV_CD = 'INSTRUCT_CLASS'
                            AND TO_CHAR(SHIP.INSTRUCT_CLASS) = MG1.GEN_CD
                        LEFT OUTER JOIN
                                M_GENERALS MG2
                        ON
                                SHIP.SHIPPER_ID = MG2.SHIPPER_ID
                            AND MG2.REGISTER_DIVI_CD = '1'
                            AND MG2.CENTER_ID = '@@@'
                            AND MG2.GEN_DIV_CD = 'EMERGENCY_CLASS'
                            AND TO_CHAR(SHIP.EMERGENCY_CLASS) = MG2.GEN_CD
                        WHERE
                                1 = 1
                     ");
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                    parameters.Add(":CENTER_ID", condition.CenterId);
                    parameters.Add(":SEQ", condition.Seq);

                    // 2.検索・ワーク作成
                    var result = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }
                trans.Commit();
                condition.Seq = seq;
                condition.Page = 1;
            }
            return true;
        }

        /// <summary>
        /// Get StockInquiry List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<BtoBInstructionReference01ResultRow> BtoBInstructionReference01GetData(BtoBInstructionReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            // SKU一覧
            if (condition.ResultType == ViewModels.BtoBInstructionReference.ResultTypes.Sku)
            {
                StringBuilder query = new StringBuilder(@"
                SELECT *
                  FROM WW_SHP_BTO_B_INSTRUCTION_REFERENCE
                 WHERE SHIPPER_ID = :SHIPPER_ID
                   AND SEQ = :SEQ ");
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":SEQ", condition.Seq);

                // 全レコード数を取得
                int totalCount = MvcDbContext.Current.Database.Connection.Query<BtoBInstructionReference01ResultRow>(query.ToString(), parameters).Count();

                // Sort function
                switch (condition.SortKey)
                {
                    case BtoBInstructionReference01SortKey.SkuInstructId:
                        switch (condition.Sort)
                        {
                            case AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY ITEM_SKU_ID DESC,SHIP_INSTRUCT_ID DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY ITEM_SKU_ID ASC,SHIP_INSTRUCT_ID ASC ");
                                break;
                        }

                        break;

                    default:
                        switch (condition.Sort)
                        {
                            case AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY SHIP_PLAN_DATE DESC,SHIP_INSTRUCT_ID DESC,ITEM_SKU_ID DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY SHIP_PLAN_DATE ASC,SHIP_INSTRUCT_ID ASC,ITEM_SKU_ID ASC ");
                                break;
                        }

                        break;
                }

                query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
                parameters.Add(":PAGE_SIZE", condition.PageSize);

                // Choose data corresponding on each page
                parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

                // Fill data to memory
                var References = MvcDbContext.Current.Database.Connection.Query<BtoBInstructionReference01ResultRow>(query.ToString(), parameters);
                var shpBtoBInstructionReferences = MvcDbContext.Current.ShpBtoBInstructionReferences.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId);
                condition.ShipInstructIdSum = shpBtoBInstructionReferences.Select(x => x.ShipInstructId).Distinct().Count();
                condition.ItemSkuSum = shpBtoBInstructionReferences.Select(x => x.ItemSkuId).Distinct().Count();
                condition.InstructQtySum = shpBtoBInstructionReferences.Select(x => x.InstructQty).Sum();
                condition.SelectedCnt = MvcDbContext.Current.ShpBtoBInstructionReferences.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId && x.IsCheck).Count();

                // Excute paging
                return new StaticPagedList<BtoBInstructionReference01ResultRow>(References, condition.Page, condition.PageSize, totalCount);
            }

            // 指示明細
            else
            {
                StringBuilder query = new StringBuilder(@"
                SELECT *
                  FROM WW_SHP_BTO_B_INSTRUCTION_REFERENCE
                 WHERE SHIPPER_ID = :SHIPPER_ID
                   AND SEQ = :SEQ ");
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":SEQ", condition.Seq);

                // 全レコード数を取得
                int totalCount = MvcDbContext.Current.Database.Connection.Query<BtoBInstructionReference01ResultRow>(query.ToString(), parameters).Count();

                // Sort function
                switch (condition.DetailSortKey)
                {
                    case BtoBInstructionReferenceDetailSortKey.ShipToStoreInstructIdSeq:
                        switch (condition.Sort)
                        {
                            case AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY SHIP_TO_STORE_ID DESC,SHIP_INSTRUCT_ID DESC,SHIP_INSTRUCT_SEQ DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY SHIP_TO_STORE_ID ASC,SHIP_INSTRUCT_ID ASC,SHIP_INSTRUCT_SEQ ASC ");
                                break;
                        }

                        break;

                    case BtoBInstructionReferenceDetailSortKey.SkuInstructIdSeq:
                        switch (condition.Sort)
                        {
                            case AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY ITEM_SKU_ID DESC,SHIP_INSTRUCT_ID DESC,SHIP_INSTRUCT_SEQ DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY ITEM_SKU_ID ASC,SHIP_INSTRUCT_ID ASC,SHIP_INSTRUCT_SEQ ASC ");
                                break;
                        }

                        break;

                    default:
                        switch (condition.Sort)
                        {
                            case AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY SHIP_PLAN_DATE DESC,SHIP_INSTRUCT_ID DESC,SHIP_INSTRUCT_SEQ DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY SHIP_PLAN_DATE ASC,SHIP_INSTRUCT_ID ASC,SHIP_INSTRUCT_SEQ ASC ");
                                break;
                        }

                        break;
                }

                query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
                parameters.Add(":PAGE_SIZE", condition.PageSize);

                // Choose data corresponding on each page
                parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

                // Fill data to memory
                var References = MvcDbContext.Current.Database.Connection.Query<BtoBInstructionReference01ResultRow>(query.ToString(), parameters);
                var shpBtoBInstructionReferences = MvcDbContext.Current.ShpBtoBInstructionReferences.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId);
                condition.ItemSkuSum = shpBtoBInstructionReferences.Select(x => x.ItemSkuId).Distinct().Count();
                condition.SeqSum = shpBtoBInstructionReferences.Count();
                condition.InstructQtySum = shpBtoBInstructionReferences.Select(x => x.InstructQty).Sum();
                condition.SelectedCnt = MvcDbContext.Current.ShpBtoBInstructionReferences.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId && x.IsCheck).Count();

                // Excute paging
                return new StaticPagedList<BtoBInstructionReference01ResultRow>(References, condition.Page, condition.PageSize, totalCount);
            }
        }

        /// <summary>
        /// Update Work Table
        /// </summary>
        /// <param name="References"></param>
        /// <returns></returns>
        public bool UpdateShpBtoBInstructionReference(IList<SelectedBtoBInstructionReference01ViewModel> References)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in References)
                {
                    // 在庫明細
                    var reference = MvcDbContext.Current.ShpBtoBInstructionReferences
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == u.Seq && m.LineNo == u.LineNo)
                                  .SingleOrDefault();

                    if (reference == null)
                    {
                        return false;
                    }

                    reference.SetBaseInfoUpdate();
                    reference.IsCheck = u.IsCheck;
                    try
                    {
                        MvcDbContext.Current.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return false;
                    }
                }

                trans.Commit();
            }

            return true;
        }

        /// <summary>
        /// Update Work Table
        /// </summary>
        /// <param name="References"></param>
        /// <returns></returns>
        public bool ShpBtoBInstructionReferenceAllChange(BtoBInstructionReference01SearchConditions conditions, bool check)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in MvcDbContext.Current.ShpBtoBInstructionReferences
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq))
                {
                    u.SetBaseInfoUpdate();
                    u.IsCheck = check;
                    try
                    {
                        MvcDbContext.Current.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return false;
                    }
                }

                trans.Commit();
            }

            conditions.SelectedCnt = MvcDbContext.Current.ShpBtoBInstructionReferences
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq && m.IsCheck).Count();
            return true;
        }

        /// <summary>
        /// 出荷キャンセル
        /// </summary>
        public void ShipCancel(long seq, out string message, out string batchNo)
        {
            //var param = new DynamicParameters();
            //param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            //param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            //param.Add("IN_CENTER_ID", Profile.User.CenterId, DbType.String, ParameterDirection.Input);
            //param.Add("IN_SEQ", seq, DbType.Int32, ParameterDirection.Input);
            //param.Add("OUT_BATCH_NO", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);
            //param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            //param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            //var db = MvcDbContext.Current.Database;

            //db.Connection.Execute(
            //    "PK_W_ARR_INPUTPURCHASE02.UpdateResult",
            //    param,
            //    commandType: CommandType.StoredProcedure);

            //message = param.Get<string>("OUT_MESSAGE");
            //batchNo = param.Get<string>("OUT_BATCH_NO");
            message = "";
            batchNo = "BATCH0001";
        }

        /// <summary>
        /// 出荷梱包明細情報取得
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public BtoBInstructionReference02ViewModel GetPackageData(BtoBInstructionReference01SearchConditions condition)
        {
            var vm = new BtoBInstructionReference02ViewModel();

            //ヘッダ情報取得
            StringBuilder query = new StringBuilder();
            DynamicParameters parameters = new DynamicParameters();
            query.Append(@"
                WITH
                    SELECTED_WORK_DATA AS (
                        SELECT
                                *
                        FROM
                                WW_SHP_BTO_B_INSTRUCTION_REFERENCE
                        WHERE
                                SEQ = :SEQ
                            AND SHIPPER_ID = :SHIPPER_ID
                            AND LINE_NO = :LINE_NO
                )
                SELECT
                        WK.SHIP_PLAN_DATE
                    ,   WK.ALLOC_DATE
                    ,   WK.INSTRUCT_CLASS_NAME
                    ,   WK.EMERGENCY_CLASS_NAME
                    ,   WK.SHIP_INSTRUCT_ID
                    ,   WK.SHIP_INSTRUCT_SEQ
                    ,   WK.SHIP_TO_STORE_ID
                    ,   WK.SHIP_TO_STORE_NAME
                    ,   WK.TRANSPORTER_NAME
                    ,   SHIP.DELI_SHIWAKE_CD
                    ,   SHIP.BATCH_NO
                    ,   SHIP.IF_STATE_ERP
                    ,   WK.CENTER_ID
                    ,   0 AS PAST_FLAG
                FROM
                        T_SHIPS SHIP
                INNER JOIN
                        SELECTED_WORK_DATA WK
                ON
                        SHIP.SHIP_INSTRUCT_ID = WK.SHIP_INSTRUCT_ID
                    AND SHIP.SHIP_INSTRUCT_SEQ = WK.SHIP_INSTRUCT_SEQ
                    AND SHIP.CENTER_ID = WK.CENTER_ID
                    AND SHIP.SHIPPER_ID = WK.SHIPPER_ID
            ");
            parameters.Add(":SEQ", condition.Seq);
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":LINE_NO", condition.LineNo);
            vm.SearchConditions = MvcDbContext.Current.Database.Connection.Query<BtoBInstructionReference02SearchConditions>(query.ToString(), parameters).FirstOrDefault();

            //累積ヘッダ
            if(vm.SearchConditions == null)
            {
                query = new StringBuilder();
                parameters = new DynamicParameters();
                query.Append(@"
                    WITH
                        SELECTED_WORK_DATA AS (
                            SELECT
                                    *
                            FROM
                                    WW_SHP_BTO_B_INSTRUCTION_REFERENCE
                            WHERE
                                    SEQ = :SEQ
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND LINE_NO = :LINE_NO
                    )
                    SELECT
                            WK.SHIP_PLAN_DATE
                        ,   WK.ALLOC_DATE
                        ,   WK.INSTRUCT_CLASS_NAME
                        ,   WK.EMERGENCY_CLASS_NAME
                        ,   WK.SHIP_INSTRUCT_ID
                        ,   WK.SHIP_INSTRUCT_SEQ
                        ,   WK.SHIP_TO_STORE_ID
                        ,   WK.SHIP_TO_STORE_NAME
                        ,   WK.TRANSPORTER_NAME
                        ,   SHIP.DELI_SHIWAKE_CD
                        ,   SHIP.BATCH_NO
                        ,   SHIP.IF_STATE_ERP
                        ,   WK.CENTER_ID
                        ,   1 AS PAST_FLAG
                    FROM
                            A_SHIPS SHIP
                    INNER JOIN
                            SELECTED_WORK_DATA WK
                    ON
                            SHIP.SHIP_INSTRUCT_ID = WK.SHIP_INSTRUCT_ID
                        AND SHIP.SHIP_INSTRUCT_SEQ = WK.SHIP_INSTRUCT_SEQ
                        AND SHIP.CENTER_ID = WK.CENTER_ID
                        AND SHIP.SHIPPER_ID = WK.SHIPPER_ID
                ");
                parameters.Add(":SEQ", condition.Seq);
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":LINE_NO", condition.LineNo);
                vm.SearchConditions = MvcDbContext.Current.Database.Connection.Query<BtoBInstructionReference02SearchConditions>(query.ToString(), parameters).FirstOrDefault();
            }

            //明細情報取得
            query = new StringBuilder();
            parameters = new DynamicParameters();
            if (vm.SearchConditions.PastFlag == 0)
            {
                //トランデータ(T)
                query.Append(@"
                    WITH
                        SELECTED_PACK_SUM AS (
                            SELECT
                                    SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   CENTER_ID
                                ,   SHIPPER_ID
                                ,   SUM(RESULT_QTY) AS RESULT_QTY
                            FROM
                                    T_SHIP_PACKING_INFO
                            WHERE
                                    SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                                AND SHIP_INSTRUCT_SEQ = :SHIP_INSTRUCT_SEQ
                                AND CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND IF_STATE_ERP = 0
                                AND EC_FLAG = 0
                            GROUP BY
                                    SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   CENTER_ID
                                ,   SHIPPER_ID
                    )
                    SELECT
                            MIC.CATEGORY_NAME1
                        ,   SHIP.ITEM_ID
                        ,   SHIP.ITEM_NAME
                        ,   SHIP.ITEM_COLOR_ID
                        ,   MC.ITEM_COLOR_NAME
                        ,   SHIP.ITEM_SIZE_ID
                        ,   MIS.ITEM_SIZE_NAME
                        ,   SHIP.JAN
                        ,   SHIP.INSTRUCT_QTY
                        ,   SHIP.ALLOC_QTY
                        ,   CASE
                                WHEN SHIP.SHIP_KIND = 4 THEN NVL(SHIP.STOCKOUT_QTY, 0)
                                ELSE NVL(SRT.STOCK_OUT_FIX_QTY, 0)
                            END AS STOCK_OUT_FIX_QTY
                        ,   NVL(SHIP.RESULT_QTY, 0) + NVL(PKSUM.RESULT_QTY, 0) AS RESULT_QTY
                        ,   CASE
                                WHEN NVL(SRT.STOCK_OUT_REG_QTY, 0) = 0 AND SHIP.SHIP_KIND <> 4 THEN 1
                                ELSE 0
                            END AS STOCK_OUT_REG_DISP_FLAG
                    FROM
                            T_SHIPS SHIP
                    LEFT OUTER JOIN
                            SELECTED_PACK_SUM PKSUM
                    ON
                            SHIP.SHIP_INSTRUCT_ID = PKSUM.SHIP_INSTRUCT_ID
                        AND SHIP.SHIP_INSTRUCT_SEQ = PKSUM.SHIP_INSTRUCT_SEQ
                        AND SHIP.CENTER_ID = PKSUM.CENTER_ID
                        AND SHIP.SHIPPER_ID = PKSUM.SHIPPER_ID
                    LEFT OUTER JOIN
                            T_STORE_SORT SRT
                    ON
                            SHIP.BATCH_NO = SRT.BATCH_NO
                        AND SHIP.SHIP_INSTRUCT_ID = SRT.SHIP_INSTRUCT_ID
                        AND SHIP.SHIP_INSTRUCT_SEQ = SRT.SHIP_INSTRUCT_SEQ
                        AND SHIP.CENTER_ID = SRT.CENTER_ID
                        AND SHIP.SHIPPER_ID = SRT.SHIPPER_ID
                ");
            }
            else
            {
                //累積データ(A)
                query.Append(@"
                    SELECT
                            MIC.CATEGORY_NAME1
                        ,   SHIP.ITEM_ID
                        ,   SHIP.ITEM_NAME
                        ,   SHIP.ITEM_COLOR_ID
                        ,   MC.ITEM_COLOR_NAME
                        ,   SHIP.ITEM_SIZE_ID
                        ,   MIS.ITEM_SIZE_NAME
                        ,   SHIP.JAN
                        ,   SHIP.INSTRUCT_QTY
                        ,   SHIP.ALLOC_QTY
                        ,   NVL(SHIP.STOCKOUT_QTY, 0) AS STOCK_OUT_FIX_QTY
                        ,   NVL(SHIP.RESULT_QTY, 0) AS RESULT_QTY
                        ,   0 AS STOCK_OUT_REG_DISP_FLAG
                    FROM
                            A_SHIPS SHIP
                ");
            }
            query.Append(@"
                    LEFT OUTER JOIN
                            M_ITEM_SKU MIS
                    ON
                            SHIP.SHIPPER_ID = MIS.SHIPPER_ID
                        AND SHIP.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                    LEFT OUTER JOIN
                            M_ITEM_CATEGORIES4 MIC
                    ON
                            MIS.SHIPPER_ID = MIC.SHIPPER_ID
                        AND MIS.CATEGORY_ID1 = MIC.CATEGORY_ID1
                        AND MIS.CATEGORY_ID2 = MIC.CATEGORY_ID2
                        AND MIS.CATEGORY_ID3 = MIC.CATEGORY_ID3
                        AND MIS.CATEGORY_ID4 = MIC.CATEGORY_ID4
                    LEFT OUTER JOIN
                            M_COLORS MC
                    ON
                            SHIP.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                        AND SHIP.SHIPPER_ID = MC.SHIPPER_ID
                    LEFT OUTER JOIN
                            M_SIZES MS
                    ON
                            SHIP.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                        AND SHIP.SHIPPER_ID   = MS.SHIPPER_ID
                    WHERE
                            SHIP.SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                        AND SHIP.SHIP_INSTRUCT_SEQ = :SHIP_INSTRUCT_SEQ
                        AND SHIP.CENTER_ID = :CENTER_ID
                        AND SHIP.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", vm.SearchConditions.CenterId);
            parameters.Add(":SHIP_INSTRUCT_ID", vm.SearchConditions.ShipInstructId);
            parameters.Add(":SHIP_INSTRUCT_SEQ", vm.SearchConditions.ShipInstructSeq);
            vm.DetailResults = new BtoBInstructionReference02DetailResult()
            {
                BtoBInstructionReference02Details = MvcDbContext.Current.Database.Connection.Query<BtoBInstructionReference02DetailResultRow>(query.ToString(), parameters)
            };

            //梱包明細情報取得
            query = new StringBuilder();
            parameters = new DynamicParameters();
            if (vm.SearchConditions.PastFlag == 0)
            {
                query.Append(@"
                    WITH
                        SELECTED_T_PACK_DATA AS (
                            SELECT
                                    BOX_NO
                                ,   DELI_NO
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   CENTER_ID
                                ,   SHIPPER_ID
                                ,   RESULT_QTY
                                ,   NOUHIN_PRN_USER_ID
                                ,   NOUHIN_PRN_FLAG
                                ,   DELI_PRN_FLAG
                                ,   KAKU_FLAG
                                ,   IF_STATE_ERP
                                ,   0 AS PAST_FLAG
                            FROM
                                    T_SHIP_PACKING_INFO
                            WHERE
                                    SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                                AND SHIP_INSTRUCT_SEQ = :SHIP_INSTRUCT_SEQ
                                AND CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND EC_FLAG = 0
                    )
                    ,   SELECTED_A_PACK_DATA AS (
                            SELECT
                                    BOX_NO
                                ,   DELI_NO
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   CENTER_ID
                                ,   SHIPPER_ID
                                ,   RESULT_QTY
                                ,   NOUHIN_PRN_USER_ID
                                ,   NOUHIN_PRN_FLAG
                                ,   DELI_PRN_FLAG
                                ,   KAKU_FLAG
                                ,   IF_STATE_ERP
                                ,   1 AS PAST_FLAG
                            FROM
                                    A_SHIP_PACKING_INFO
                            WHERE
                                    SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                                AND SHIP_INSTRUCT_SEQ = :SHIP_INSTRUCT_SEQ
                                AND CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND EC_FLAG = 0
                    )
                    ,   SELECTED_PACK_DATA AS (
                            SELECT * FROM SELECTED_T_PACK_DATA
                            UNION
                            SELECT * FROM SELECTED_A_PACK_DATA
                    )
                    SELECT
                            PACK.BOX_NO
                        ,   PACK.DELI_NO
                        ,   CASE
                                WHEN PACK.PAST_FLAG = 1 THEN '" + BtoBReferenceResource.Daily + @"'
                                WHEN PACK.IF_STATE_ERP = 2 THEN  '" + BtoBReferenceResource.Send + @"'
                                WHEN PACK.KAKU_FLAG = 1 THEN '" + BtoBReferenceResource.Confirm + @"'
                                WHEN PACK.NOUHIN_PRN_FLAG = 1 THEN '" + BtoBReferenceResource.RePrint + @"'
                                WHEN PACK.NOUHIN_PRN_FLAG IN (8, 9) AND PACK.DELI_PRN_FLAG <> 0  THEN '" + BtoBReferenceResource.Print + @"'
                                ELSE '" + BtoBReferenceResource.Shipping + @"'
                            END AS SHIP_STATUS_NAME
                        ,   SHIP.SHIP_PLAN_DATE AS SHIP_PLAN_DATE
                        ,   MIC.CATEGORY_NAME1
                        ,   SHIP.ITEM_ID
                        ,   SHIP.ITEM_NAME
                        ,   SHIP.ITEM_COLOR_ID
                        ,   MC.ITEM_COLOR_NAME
                        ,   SHIP.ITEM_SIZE_ID
                        ,   MIS.ITEM_SIZE_NAME
                        ,   SHIP.JAN
                        ,   NVL(PACK.RESULT_QTY, 0) AS RESULT_QTY
                        ,   NVL(TRIM(SOR_USE.USER_NAME), ORD_USE.USER_NAME) AS ASORT_USER_NAME
                        ,   NOU_USE.USER_NAME AS NOUHIN_PRN_USER_NAME
                        ,   SRT.STOCK_OUT_FIX_DATE
                        ,   NVL(SRT.SORT_STATUS,0) AS SORT_STATUS
                    FROM
                            T_SHIPS SHIP
                    LEFT OUTER JOIN
                ");
                if (vm.SearchConditions.IfStateErp == 0)
                {
                    //未送信データ(T)
                    query.Append(@"
                        SELECTED_T_PACK_DATA PACK
                    ");
                }
                else
                {
                    //一部送信もしくは送信済データ(T+A)
                    query.Append(@"
                        SELECTED_PACK_DATA PACK
                    ");
                }
                query.Append(@"
                    ON
                            SHIP.SHIP_INSTRUCT_ID = PACK.SHIP_INSTRUCT_ID
                        AND SHIP.SHIP_INSTRUCT_SEQ = PACK.SHIP_INSTRUCT_SEQ
                        AND SHIP.CENTER_ID = PACK.CENTER_ID
                        AND SHIP.SHIPPER_ID = PACK.SHIPPER_ID
                    LEFT OUTER JOIN
                            T_STORE_SORT SRT
                    ON
                            SHIP.BATCH_NO = SRT.BATCH_NO
                        AND SHIP.SHIP_INSTRUCT_ID = SRT.SHIP_INSTRUCT_ID
                        AND SHIP.SHIP_INSTRUCT_SEQ = SRT.SHIP_INSTRUCT_SEQ
                        AND SHIP.CENTER_ID = SRT.CENTER_ID
                        AND SHIP.SHIPPER_ID = SRT.SHIPPER_ID
                ");
            }
            else
            {
                //累積データ(A)
                query.Append(@"
                    WITH
                        SELECTED_STORE_SORT AS (
                            SELECT
                                    SHIPPER_ID
                                ,   CENTER_ID
                                ,   BATCH_NO
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   SORT_USER_ID
                                ,   ORDER_PIC_USER_ID
                                ,   STOCK_OUT_FIX_DATE
                                ,   SORT_STATUS
                            FROM
                                    T_STORE_SORT
                            WHERE
                                    SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                                AND SHIP_INSTRUCT_SEQ = :SHIP_INSTRUCT_SEQ
                                AND CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID

                            UNION ALL

                            SELECT
                                    SHIPPER_ID
                                ,   CENTER_ID
                                ,   BATCH_NO
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   SORT_USER_ID
                                ,   ORDER_PIC_USER_ID
                                ,   STOCK_OUT_FIX_DATE
                                ,   SORT_STATUS
                            FROM
                                    A_STORE_SORT
                            WHERE
                                    SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                                AND SHIP_INSTRUCT_SEQ = :SHIP_INSTRUCT_SEQ
                                AND CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                        )
                    SELECT
                            PACK.BOX_NO
                        ,   PACK.DELI_NO
                        ,   '" + BtoBReferenceResource.Daily + @"' AS SHIP_STATUS_NAME
                        ,   SHIP.SHIP_PLAN_DATE AS SHIP_PLAN_DATE
                        ,   MIC.CATEGORY_NAME1
                        ,   SHIP.ITEM_ID
                        ,   SHIP.ITEM_NAME
                        ,   SHIP.ITEM_COLOR_ID
                        ,   MC.ITEM_COLOR_NAME
                        ,   SHIP.ITEM_SIZE_ID
                        ,   MIS.ITEM_SIZE_NAME
                        ,   SHIP.JAN
                        ,   NVL(PACK.RESULT_QTY, 0) AS RESULT_QTY
                        ,   NVL(TRIM(SOR_USE.USER_NAME), ORD_USE.USER_NAME) AS ASORT_USER_NAME
                        ,   NOU_USE.USER_NAME AS NOUHIN_PRN_USER_NAME
                        ,   SRT.STOCK_OUT_FIX_DATE
                        ,   NVL(SRT.SORT_STATUS,0) AS SORT_STATUS
                    FROM
                            A_SHIPS SHIP
                    LEFT OUTER JOIN
                            A_SHIP_PACKING_INFO PACK
                    ON
                            SHIP.SHIP_INSTRUCT_ID = PACK.SHIP_INSTRUCT_ID
                        AND SHIP.SHIP_INSTRUCT_SEQ = PACK.SHIP_INSTRUCT_SEQ
                        AND SHIP.CENTER_ID = PACK.CENTER_ID
                        AND SHIP.SHIPPER_ID = PACK.SHIPPER_ID
                    LEFT OUTER JOIN
                            SELECTED_STORE_SORT SRT
                    ON
                            SHIP.BATCH_NO = SRT.BATCH_NO
                        AND SHIP.SHIP_INSTRUCT_ID = SRT.SHIP_INSTRUCT_ID
                        AND SHIP.SHIP_INSTRUCT_SEQ = SRT.SHIP_INSTRUCT_SEQ
                        AND SHIP.CENTER_ID = SRT.CENTER_ID
                        AND SHIP.SHIPPER_ID = SRT.SHIPPER_ID
                ");
            }
            query.Append(@"
                LEFT OUTER JOIN
                        M_ITEM_SKU MIS
                ON
                        SHIP.SHIPPER_ID = MIS.SHIPPER_ID
                    AND SHIP.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                LEFT OUTER JOIN
                        M_ITEM_CATEGORIES4 MIC
                ON
                        MIS.CATEGORY_ID1 = MIC.CATEGORY_ID1
                    AND MIS.CATEGORY_ID2 = MIC.CATEGORY_ID2
                    AND MIS.CATEGORY_ID3 = MIC.CATEGORY_ID3
                    AND MIS.CATEGORY_ID4 = MIC.CATEGORY_ID4
                    AND MIS.SHIPPER_ID = MIC.SHIPPER_ID
                LEFT OUTER JOIN
                        M_COLORS MC
                ON
                        SHIP.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                    AND SHIP.SHIPPER_ID = MC.SHIPPER_ID
                LEFT OUTER JOIN
                        M_SIZES MS
                ON
                        SHIP.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                    AND SHIP.SHIPPER_ID   = MS.SHIPPER_ID
                LEFT OUTER JOIN
                        M_USERS SOR_USE
                ON
                        SRT.SORT_USER_ID = SOR_USE.USER_ID
                    AND SRT.SHIPPER_ID   = SOR_USE.SHIPPER_ID
                LEFT OUTER JOIN
                        M_USERS ORD_USE
                ON
                        SRT.ORDER_PIC_USER_ID = ORD_USE.USER_ID
                    AND SRT.SHIPPER_ID   = ORD_USE.SHIPPER_ID
                LEFT OUTER JOIN
                        M_USERS NOU_USE
                ON
                        PACK.NOUHIN_PRN_USER_ID = NOU_USE.USER_ID
                    AND PACK.SHIPPER_ID   = NOU_USE.SHIPPER_ID
                WHERE
                        SHIP.SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                    AND SHIP.SHIP_INSTRUCT_SEQ = :SHIP_INSTRUCT_SEQ
                    AND SHIP.CENTER_ID = :CENTER_ID
                    AND SHIP.SHIPPER_ID = :SHIPPER_ID
                ORDER BY
                        PACK.BOX_NO
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", vm.SearchConditions.CenterId);
            parameters.Add(":SHIP_INSTRUCT_ID", vm.SearchConditions.ShipInstructId);
            parameters.Add(":SHIP_INSTRUCT_SEQ", vm.SearchConditions.ShipInstructSeq);
            vm.PackageDetailResults = new BtoBInstructionReference02PackageDetailResult
            {
                BtoBInstructionReference02PackageDetails = MvcDbContext.Current.Database.Connection.Query<BtoBInstructionReference02PackageDetailResultRow>(query.ToString(), parameters)
            };
            if (vm.PackageDetailResults.BtoBInstructionReference02PackageDetails.Where(x => x.StockOutFixDate == null).Any())
            {
                vm.SearchConditions.StockOutFixDateFlag = true;
            }
            vm.SearchConditions.ResultQtySum = vm.PackageDetailResults.BtoBInstructionReference02PackageDetails.Select(x => x.ResultQty).Sum();
            return vm;
        }

        /// <summary>
        /// 欠品確定更新
        /// </summary>
        public void StockOutFix(BtoBInstructionReference02SearchConditions conditions, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", conditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_SHIP_INSTRUCT_ID", conditions.ShipInstructId, DbType.String, ParameterDirection.Input);
            param.Add("IN_SHIP_INSTRUCT_SEQ", conditions.ShipInstructSeq, DbType.Int32, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_CONFIRMPROGRESS_STOCKOUT_REG",
                param,
                commandType: CommandType.StoredProcedure);
            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }

        /// <summary>
        /// 事業部データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListDivisions()
        {
            return MvcDbContext.Current.Divisions
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.DivisionId,
                    Text = m.DivisionId + ":" + m.DivisionName
                })
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// アイテムデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListItems()
        {
            return MvcDbContext.Current.Items
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.ItemCode,
                    Text = m.ItemCode + ":" + m.ItemCodeName
                })
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類1データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys1()
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId1,
                    Text = m.CategoryId1.ToString() + ":" + m.CategoryName1
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類2データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys2(string categoryId1 = "")
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId2,
                    Text = m.CategoryId2.ToString() + ":" + m.CategoryName2
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類3データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys3(string categoryId1 = "", string categoryId2 = "")
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId 
                && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1)
                && (categoryId2 == null ? 1 == 1 : m.CategoryId2 == categoryId2))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId3,
                    Text = m.CategoryId3.ToString() + ":" + m.CategoryName3
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類4データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys4(string categoryId1 = "", string categoryId2 = "", string categoryId3 = "")
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId 
                && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1)
                && (categoryId2 == null ? 1 == 1 : m.CategoryId2 == categoryId2)
                && (categoryId3 == null ? 1 == 1 : m.CategoryId3 == categoryId3))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId4,
                    Text = m.CategoryId4.ToString() + ":" + m.CategoryName4
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 配送業者データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListTransporters()
        {
            return MvcDbContext.Current.Transporters
                .Where(m => m.ShipperId == Profile.User.ShipperId )
                .Select(m => new SelectListItem
                {
                    Value = m.TransporterId.ToString(),
                    Text = m.TransporterName
                })
                .OrderBy(m => m.Value);
        }
    }
}