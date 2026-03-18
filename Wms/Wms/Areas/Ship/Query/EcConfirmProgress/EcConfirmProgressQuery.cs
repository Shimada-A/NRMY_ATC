namespace Wms.Areas.Ship.Query.EcConfirmProgressQuery
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
    using Wms.Areas.Ship.ViewModels.EcConfirmProgress;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.EcConfirmProgress.EcConfirmProgress01SearchConditions;
    using static Wms.Areas.Ship.ViewModels.EcConfirmProgress.EcConfirmProgress02SearchConditions;

    public class EcConfirmProgressQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertShpEcConfirmProgress01(EcConfirmProgress01SearchConditions condition)
        {
            condition.Seq = new BaseQuery().GetWorkId();
            condition.Page = 1;
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder queryCommon = new StringBuilder();
            StringBuilder queryGroup = new StringBuilder();
            StringBuilder query = new StringBuilder();

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    query.Append(@"
                        INSERT INTO WW_SHP_EC_CONFIRM_PROGRESS (
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
                             ,   CENTER_ID          
                             ,   SHIP_PLAN_DATE     
                             ,   SHIP_INSTRUCT_ID   
                             ,   ORDER_DATE         
                             ,   TRANSPORTER_NAME   
                             ,   ARRIVE_REQUEST_DATE
                             ,   ALLOC_DATE         
                             ,   SHIP_STATUS_NAME   
                             ,   EC_SHIP_CLASS_NAME
                             ,   KAKU_DATE          
                             ,   STOCKOUT_QTY       
                             ,   CANCEL_FLAG        
                             ,   ITEM_SKU_ID        
                             ,   ORDER_QTY          
                             ,   BOX_NO             
                             ,   SHIP_TO_STORE_ID
                             ,   SEQ_PRE
                        )
                        WITH
                            --出荷梱包実績
                            GROUP_T_PACK_DATA AS (
                                SELECT
                                        SHIP_INSTRUCT_ID
                                    ,   SHIP_INSTRUCT_SEQ
                                    ,   CENTER_ID
                                    ,   SHIPPER_ID
                                    ,   MAX(BOX_NO) AS BOX_NO
                                    ,   MAX(SHIP_TO_STORE_ID) AS SHIP_TO_STORE_ID
                                    ,   MIN(NOUHIN_PRN_FLAG) AS NOUHIN_PRN_FLAG
                                    ,   MIN(DELI_PRN_FLAG) AS DELI_PRN_FLAG
                                    ,   MAX(DELI_NO) AS DELI_NO
                                FROM
                                        T_SHIP_PACKING_INFO
                                WHERE
                                        EC_FLAG = 1
                                    AND CENTER_ID = :CENTER_ID
                                    AND SHIPPER_ID = :SHIPPER_ID
                    ");
                    // 送り状No
                    if (!string.IsNullOrEmpty(condition.DeliNo))
                    {
                        query.Append(" AND 1 = CASE WHEN DELI_NO = :DELI_NO THEN 1 WHEN SF_GET_DELI_NO_EXIST(CENTER_ID,SHIPPER_ID,SHIP_INSTRUCT_ID,:DELI_NO) = 1 THEN 1 ELSE 0 END ");
                        parameters.Add(":DELI_NO", condition.DeliNo);
                    }

                    // ケースNo
                    if (!string.IsNullOrEmpty(condition.BoxNo))
                    {
                        query.Append(" AND BOX_NO = :BOX_NO ");
                        parameters.Add(":BOX_NO", condition.BoxNo);
                    }

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
                                        ,   MAX(DELI_NO) AS DELI_NO
                                    FROM
                                            A_SHIP_PACKING_INFO
                                    WHERE
                                            EC_FLAG = 1
                                        AND CENTER_ID = :CENTER_ID
                                        AND SHIPPER_ID = :SHIPPER_ID
                        ");
                        // 送り状No
                        if (!string.IsNullOrEmpty(condition.DeliNo))
                        {
    
                            query.Append(" AND 1 = CASE WHEN DELI_NO = :DELI_NO THEN 1 WHEN SF_GET_DELI_NO_EXIST(CENTER_ID,SHIPPER_ID,SHIP_INSTRUCT_ID,:DELI_NO) = 1 THEN 1 ELSE 0 END ");
                            parameters.Add(":DELI_NO", condition.DeliNo);
                        }

                        // ケースNo
                        if (!string.IsNullOrEmpty(condition.BoxNo))
                        {
                            query.Append(" AND BOX_NO = :BOX_NO ");
                            parameters.Add(":BOX_NO", condition.BoxNo);
                        }
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
                                    SELECT* FROM T_ECSHIP_DATA
                                    UNION ALL
                                    SELECT *FROM A_ECSHIP_DATA
                            )
                        ");
                    };
                    queryCommon.Append(@"
                        ,   TARGET_INSTRUCT_DATA AS (
                                SELECT
                                        TE.SHIP_INSTRUCT_ID
                                    ,   TE.CENTER_ID
                                    ,   TE.SHIPPER_ID
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
                         WHERE
                               1 = 1
                    ");

                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                    parameters.Add(":CENTER_ID", condition.CenterId);
                    parameters.Add(":USER_ID", Common.Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "InsertShpEcConfirmProgress01");
                    parameters.Add(":SEQ", condition.Seq);

                    // 受信日時(From-To)
                    if (condition.MakeDateFrom != null)
                    {
                        queryCommon.Append(" AND TO_CHAR(TE.MAKE_DATE,'yyyy/MM/dd hh24:mi:ss') >= :MAKE_DATE_FROM ");
                        parameters.Add(":MAKE_DATE_FROM", condition.MakeTimeFrom == null ?
                                                                string.Format("{0:yyyy/MM/dd}", condition.MakeDateFrom) + " 00:00:00" :
                                                                string.Format("{0:yyyy/MM/dd}", condition.MakeDateFrom) + " " + condition.MakeTimeFrom);
                    }

                    if (condition.MakeDateTo != null)
                    {
                        queryCommon.Append(" AND TO_CHAR(TE.MAKE_DATE,'yyyy/MM/dd hh24:mi:ss') <= :MAKE_DATE_TO ");
                        parameters.Add(":MAKE_DATE_TO", condition.MakeTimeTo == null ?
                                                                string.Format("{0:yyyy/MM/dd}", condition.MakeDateTo) + " 23:59:59" :
                                                                string.Format("{0:yyyy/MM/dd}", condition.MakeDateTo) + " " + condition.MakeTimeTo);
                    }

                    // 出荷予定日(From-To)
                    if (condition.ShipPlanDateFrom != null)
                    {
                        queryCommon.Append(" AND TE.SHIP_REQUEST_DATE >= :SHIP_PLAN_DATE_FROM ");
                        parameters.Add(":SHIP_PLAN_DATE_FROM", condition.ShipPlanDateFrom);
                    }

                    if (condition.ShipPlanDateTo != null)
                    {
                        queryCommon.Append(" AND TE.SHIP_REQUEST_DATE <= :SHIP_PLAN_DATE_TO ");
                        parameters.Add(":SHIP_PLAN_DATE_TO", condition.ShipPlanDateTo);
                    }

                    // 出荷確定日(From-To)
                    if (condition.KakuDateFrom != null)
                    {
                        queryCommon.Append(" AND TE.KAKU_DATE >= :KAKU_DATE_FROM ");
                        parameters.Add(":KAKU_DATE_FROM", condition.KakuDateFrom);
                    }

                    if (condition.KakuDateTo != null)
                    {
                        queryCommon.Append(" AND TE.KAKU_DATE <= :KAKU_DATE_TO ");
                        parameters.Add(":KAKU_DATE_TO", condition.KakuDateTo);
                    }

                    // 配送業者
                    if (!string.IsNullOrEmpty(condition.TransporterId))
                    {
                        queryCommon.Append(" AND TE.TRANSPORTER_ID = :TRANSPORTER_ID ");
                        parameters.Add(":TRANSPORTER_ID", condition.TransporterId);
                    }

                    // 配送指定日
                    if (condition.ArriveRequestDateFrom != null)
                    {
                        queryCommon.Append(" AND TE.ARRIVE_REQUEST_DATE >= :ARRIVE_REQUEST_DATE_FROM ");
                        parameters.Add(":ARRIVE_REQUEST_DATE_FROM", condition.ArriveRequestDateFrom);
                    }

                    if (condition.ArriveRequestDateTo != null)
                    {
                        queryCommon.Append(" AND TE.ARRIVE_REQUEST_DATE <= :ARRIVE_REQUEST_DATE_TO ");
                        parameters.Add(":ARRIVE_REQUEST_DATE_TO", condition.ArriveRequestDateTo);
                    }

                    // バッチNo
                    if (!string.IsNullOrEmpty(condition.BatchNo))
                    {
                        queryCommon.Append(" AND TE.BATCH_NO = :BATCH_NO ");
                        parameters.Add(":BATCH_NO", condition.BatchNo);
                    }

                    // GASバッチNo
                    if (!string.IsNullOrEmpty(condition.GASBatchNo))
                    {
                        queryCommon.Append(" AND TE.GAS_BATCH_NO = :GAS_BATCH_NO ");
                        parameters.Add(":GAS_BATCH_NO", condition.GASBatchNo);
                    }

                    // EC出荷形態
                    if (condition.SingleFlag || condition.OrderFlag || condition.GasFlag)
                    {
                        if (condition.SingleFlag && condition.OrderFlag && condition.GasFlag)
                        {
                            queryCommon.Append(" AND TE.EC_SHIP_CLASS IN (1,2,3) ");
                        }
                        else if (condition.SingleFlag && condition.OrderFlag)
                        {
                            queryCommon.Append(" AND TE.EC_SHIP_CLASS IN (1,2) ");
                        }
                        else if (condition.SingleFlag && condition.GasFlag)
                        {
                            queryCommon.Append(" AND TE.EC_SHIP_CLASS IN (1,3) ");
                        }
                        else if (condition.OrderFlag && condition.GasFlag)
                        {
                            queryCommon.Append(" AND TE.EC_SHIP_CLASS IN (2,3) ");
                        }
                        else if (condition.SingleFlag)
                        {
                            queryCommon.Append(" AND TE.EC_SHIP_CLASS = 1 ");
                        }
                        else if (condition.OrderFlag)
                        {
                            queryCommon.Append(" AND TE.EC_SHIP_CLASS = 2 ");
                        }
                        else if (condition.GasFlag)
                        {
                            queryCommon.Append(" AND TE.EC_SHIP_CLASS = 3 ");
                        }
                    }

                    // キャンセル
                    if (condition.CancelFlag)
                    {
                        queryCommon.Append(" AND TE.AFT_ALLOC_CANCEL_FLAG = 1 ");
                    }

                    // 注文番号
                    if (!string.IsNullOrEmpty(condition.ShipInstructId))
                    {
                        queryCommon.Append(" AND TE.SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID ");
                        parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId);
                    }

                    // 品番
                    if (!string.IsNullOrEmpty(condition.ItemId))
                    {
                        queryCommon.Append(" AND TE.ITEM_ID LIKE :ITEM_ID ");
                        parameters.Add(":ITEM_ID", condition.ItemId + "%");
                    }

                    // JAN
                    if (!string.IsNullOrEmpty(condition.Jan))
                    {
                        queryCommon.Append(" AND TE.JAN LIKE :JAN ");
                        parameters.Add(":JAN", condition.Jan + "%");
                    }

                    // SKU
                    if (!string.IsNullOrEmpty(condition.ItemSkuId))
                    {
                        queryCommon.Append(" AND TE.ITEM_SKU_ID LIKE :ITEM_SKU_ID ");
                        parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId + "%");
                    }

                    // 送り状No
                    if (!string.IsNullOrEmpty(condition.DeliNo))
                    {
                        queryCommon.Append(" AND 1 = CASE WHEN TE.DELI_NO = :DELI_NO THEN  1 WHEN SF_GET_DELI_NO_EXIST(TE.CENTER_ID,TE.SHIPPER_ID,TE.SHIP_INSTRUCT_ID,:DELI_NO) = 1  THEN 1 ELSE 0  END");
                        parameters.Add(":DELI_NO", condition.DeliNo);
                    }

                    // ケースNo
                    if (!string.IsNullOrEmpty(condition.BoxNo))
                    {
                        queryCommon.Append(" AND TE.BOX_NO = :BOX_NO ");
                        parameters.Add(":BOX_NO", condition.BoxNo);
                    }

                    //状態
                    switch (condition.ShipStatus)
                    {
                        case "1":   //未引当
                            queryCommon.Append(" AND TRIM(TE.BATCH_NO) IS NULL ");
                            break;
                        case "2":   //出荷中
                            queryCommon.Append(" AND TRIM(TE.BATCH_NO) IS NOT NULL ");
                            queryCommon.Append(" AND TE.DAY_FLAG = 0 ");
                            queryCommon.Append(" AND (TE.NOUHIN_PRN_FLAG <> 9 OR TE.NOUHIN_PRN_FLAG IS NULL) ");
                            queryCommon.Append(" AND TE.KAKU_FLAG = 0 ");
                            queryCommon.Append(" AND TE.IF_STATE_ERP = 0 ");
                            break;
                        case "3":   //納品書発行済
                            queryCommon.Append(" AND TE.NOUHIN_PRN_FLAG = 9 ");
                            queryCommon.Append(" AND TE.KAKU_FLAG = 0 ");
                            queryCommon.Append(" AND TE.IF_STATE_ERP = 0 ");
                            queryCommon.Append(" AND TE.DAY_FLAG = 0 ");
                            break;
                        case "4":   //確定済
                            queryCommon.Append(" AND TE.KAKU_FLAG = 1 ");
                            queryCommon.Append(" AND TE.IF_STATE_ERP = 0 ");
                            queryCommon.Append(" AND TE.DAY_FLAG = 0 ");
                            break;
                        case "5":   //送信済
                            queryCommon.Append(" AND TE.IF_STATE_ERP = 2 ");
                            queryCommon.Append(" AND TE.DAY_FLAG = 0 ");
                            break;
                        case "6":   //日次済
                            queryCommon.Append(" AND TE.DAY_FLAG = 1");
                            break;
                        default:
                            break;
                    }
                    queryGroup.Append(@"
                        GROUP BY
                                TE.SHIP_INSTRUCT_ID
                            ,   TE.CENTER_ID
                            ,   TE.SHIPPER_ID
                    ");
                    StringBuilder queryMain = new StringBuilder(@"
                        )
                        SELECT
                                SYSTIMESTAMP
                            ,   :USER_ID
                            ,   :PROGRAM_NAME
                            ,   SYSTIMESTAMP
                            ,   :USER_ID
                            ,   :PROGRAM_NAME
                            ,   0
                            ,   TE.SHIPPER_ID
                            ,   :SEQ
                            ,   ROW_NUMBER() OVER(ORDER BY TE.SHIPPER_ID,TE.CENTER_ID,TE.SHIP_INSTRUCT_ID)
                            ,   TE.CENTER_ID AS  CENTER_ID
                            ,   MAX(TE.SHIP_REQUEST_DATE) AS SHIP_PLAN_DATE
                            ,   TE.SHIP_INSTRUCT_ID
                            ,   MAX(TE.ORDER_DATE) AS ORDER_DATE
                            ,   MAX(MT.TRANSPORTER_NAME) AS TRANSPORTER_NAME
                            ,   MAX(TE.ARRIVE_REQUEST_DATE) AS ARRIVE_REQUEST_DATE
                            ,   MAX(TE.MAKE_DATE) AS ALLOC_DATE
                            ,   CASE
                                    WHEN MAX(TE.BATCH_NO) = ' ' THEN '" + EcConfirmProgressResource.UnAlloc + @"'
                                    WHEN MAX(TE.DAY_FLAG) = 1 THEN '" + EcConfirmProgressResource.Donedaily + @"'
                                    WHEN MAX(TE.IF_STATE_ERP) = 2 THEN '" + EcConfirmProgressResource.Send + @"'
                                    WHEN MAX(TE.KAKU_FLAG) = 1 THEN '" + EcConfirmProgressResource.Fixed + @"'
                                    WHEN MAX(TE.NOUHIN_PRN_FLAG) = 9 THEN '" + EcConfirmProgressResource.DeliveryIssued + @"'
                                    ELSE '" + EcConfirmProgressResource.ShipDuring + @"'
                                END AS SHIP_STATUS_NAME
                            ,   CASE
                                    WHEN MAX(TE.EC_SHIP_CLASS) = 1 THEN '" + EcConfirmProgressResource.SingleFlag + @"'
                                    WHEN MAX(TE.EC_SHIP_CLASS) = 2 THEN '" + EcConfirmProgressResource.OrderFlag + @"'
                                    WHEN MAX(TE.EC_SHIP_CLASS) = 3 THEN '" + EcConfirmProgressResource.GasFlag + @"'
                                    ELSE ''
                                END AS EC_SHIP_CLASS_NAME
                            ,   MAX(TE.KAKU_DATE) AS KAKU_DATE
                            ,   CASE WHEN MAX(TE.STOCKOUT_QTY) = 0 THEN NULL ELSE MAX(TE.STOCKOUT_QTY) END AS STOCKOUT_QTY
                            ,   CASE
                                    WHEN MAX(TE.CANCEL_FLAG) = 1 OR MAX(TE.AFT_ALLOC_CANCEL_FLAG) = 1 THEN 'ｷｬﾝｾﾙ' ELSE '' END AS CANCEL_FLAG
                            ,   MAX(TE.ITEM_SKU_ID) AS ITEM_SKU_ID
                            ,   SUM(TE.ORDER_QTY) AS ORDER_QTY
                            ,   MAX(TE.BOX_NO) AS BOX_NO
                            ,   MAX(TE.SHIP_TO_STORE_ID) AS SHIP_TO_STORE_ID
                            ,   0 
                    ");
                    if (condition.ShipStatusOld == true || condition.ShipStatus == "6")
                    {
                        queryMain.Append(@"
                            FROM
                                    ALL_ECSHIP_DATA TE
                        ");
                    }
                    else
                    {
                        queryMain.Append(@"
                            FROM
                                    T_ECSHIP_DATA TE
                        ");
                    }
                    queryMain.Append(@"
                        INNER JOIN
                                TARGET_INSTRUCT_DATA TGT
                        ON
                                TGT.SHIP_INSTRUCT_ID = TE.SHIP_INSTRUCT_ID
                            AND TGT.CENTER_ID = TE.CENTER_ID
                            AND TGT.SHIPPER_ID = TE.SHIPPER_ID
                        LEFT OUTER JOIN
                                M_TRANSPORTERS MT
                        ON
                                MT.SHIPPER_ID = TE.SHIPPER_ID
                            AND MT.TRANSPORTER_ID = TE.TRANSPORTER_ID
                        GROUP BY
                                TE.SHIP_INSTRUCT_ID
                            ,   TE.CENTER_ID
                            ,   TE.SHIPPER_ID
                    ");

                    // ワーク登録
                    MvcDbContext.Current.Database.Connection.Execute(query.ToString() + queryCommon.ToString() + queryGroup.ToString() + queryMain.ToString(), parameters);
                }
                catch (Exception ex) //デバッグ用
                {
                    Mvc.Common.AppError.PutLogREF(ex, "InsertShpEcConfirmProgress01");
                    trans.Rollback();
                    return false;
                }
                trans.Commit();
            }

            //検索結果注文番号内情報の合計値
            StringBuilder querySkuCnt = new StringBuilder(@"
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
             ");
            if (condition.ShipStatusOld == true || condition.ShipStatus == "6")
            {
                querySkuCnt.Append(@"
                    ,   SELECTED_ECSHIP_DATA AS (
                            SELECT
                                    SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   CENTER_ID
                                ,   SHIPPER_ID
                                ,   ITEM_SKU_ID
                                ,   ORDER_QTY
                            FROM
                                    T_ECSHIPS
                            WHERE
                                    SHIPPER_ID = :SHIPPER_ID
                            UNION
                            SELECT
                                    SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   CENTER_ID
                                ,   SHIPPER_ID
                                ,   ITEM_SKU_ID
                                ,   ORDER_QTY
                            FROM
                                    A_ECSHIPS
                            WHERE
                                    SHIPPER_ID = :SHIPPER_ID
                    )
                ");
            }
            else
            {
                querySkuCnt.Append(@"
                    ,   SELECTED_ECSHIP_DATA AS (
                            SELECT
                                    SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   CENTER_ID
                                ,   SHIPPER_ID
                                ,   ITEM_SKU_ID
                                ,   ORDER_QTY
                            FROM
                                    T_ECSHIPS
                            WHERE
                                    SHIPPER_ID = :SHIPPER_ID
                    )
                ");
            }
            querySkuCnt.Append(@"
                SELECT
                        COUNT(DISTINCT EC.SHIP_INSTRUCT_ID) AS SHIP_INSTRUCT_SUM
                    ,   COUNT(DISTINCT EC.ITEM_SKU_ID) AS ITEM_SKU_SUM
                    ,   SUM(EC.ORDER_QTY) AS INSTRUCT_QTY_SUM
                FROM
                        SELECTED_ECSHIP_DATA EC
                INNER JOIN
                        SELECTED_WORK_DATA WK
                ON
                        EC.SHIP_INSTRUCT_ID = WK.SHIP_INSTRUCT_ID
                    AND EC.CENTER_ID = WK.CENTER_ID
                    AND EC.SHIPPER_ID = WK.SHIPPER_ID
            ");
            var allEcConfirmData = MvcDbContext.Current.Database.Connection.Query<EcConfirmProgress01SearchConditions>(querySkuCnt.ToString(), parameters).FirstOrDefault();
            condition.ShipInstructSum = allEcConfirmData.ShipInstructSum;
            condition.InstructQtySum = allEcConfirmData.InstructQtySum;
            condition.ItemSkuSum = allEcConfirmData.ItemSkuSum;

            return true;
        }

        /// <summary>
        /// Get Work List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<EcConfirmProgress01ResultRow> EcConfirmProgress01GetData(EcConfirmProgress01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                SELECT
                        *
                FROM
                        WW_SHP_EC_CONFIRM_PROGRESS
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                    AND SEQ = :SEQ
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<EcConfirmProgress01ResultRow>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case EcConfirmProgressSortKey.ShipPlanDateShipInstructId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY SHIP_PLAN_DATE DESC,SHIP_INSTRUCT_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY SHIP_PLAN_DATE ASC,SHIP_INSTRUCT_ID ASC ");
                            break;
                    }

                    break;
                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY SHIP_INSTRUCT_ID DESC,SHIP_PLAN_DATE DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY SHIP_INSTRUCT_ID ASC,SHIP_PLAN_DATE ASC ");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var EcConfirmProgress01s = MvcDbContext.Current.Database.Connection.Query<EcConfirmProgress01ResultRow>(query.ToString(), parameters);

            condition.SelectedCnt = MvcDbContext.Current.ShpEcConfirmProgress.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId && x.IsCheck).Count();

            // Excute paging
            return new StaticPagedList<EcConfirmProgress01ResultRow>(EcConfirmProgress01s, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Select Detail
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IEnumerable<EcConfirmProgress02ResultRow> GetDetailData(EcConfirmProgress02SearchConditions condition)
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
                            ,   TPACK.KEN_DATE
                            ,   TPACK.KEN_USER_ID
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
                                ,   APACK.KEN_DATE
                                ,   APACK.KEN_USER_ID
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
                        TE.SHIP_REQUEST_DATE AS SHIP_PLAN_DATE
                    ,   TE.SHIP_INSTRUCT_ID
                    ,   TE.DEST_PREF_NAME
                    ,   MG1.GEN_NAME AS EC_SHIP_CLASS
                    ,   TE.BATCH_NO
                    ,   TE.GAS_BATCH_NO
                    ,   TE.MAKE_DATE AS ALLOC_DATE
                    ,   TE.ORDER_DATE
                    ,   MT.TRANSPORTER_SHORT_NAME || CASE WHEN TE.DAIBIKI_FLAG = 1 THEN '" + EcConfirmProgressResource.DaibikiName + @"' ELSE '' END AS TRANSPORTER_NAME
                    ,   TE.ARRIVE_REQUEST_DATE
                    ,   TE.GAS_MAGUCHI_NO
                    ,   TE.DELI_SHIWAKE_CD
                    ,   MG2.GEN_NAME AS ARRIVE_REQUEST_TIME
                    ,   TE.BOX_NO
                    ,   TE.DELI_NO
                    ,   TE.SHIP_INSTRUCT_SEQ
                    ,   MIC.CATEGORY_NAME1
                    ,   TE.ITEM_SKU_ID
                    ,   TE.ITEM_ID
                    ,   TE.ITEM_NAME
                    ,   TE.ITEM_COLOR_ID
                    ,   MC.ITEM_COLOR_NAME
                    ,   TE.ITEM_SIZE_ID
                    ,   MS.ITEM_SIZE_NAME
                    ,   TE.JAN
                    ,   TE.ORDER_QTY ALLOC_QTY
                    ,   CASE WHEN TE.PACK_RESULT_QTY = 0 THEN NULL ELSE TE.PACK_RESULT_QTY END RESULT_QTY
                    ,   TE.NOUHIN_PRN_USER_ID
                    ,   TE.RELATED_ORDER_NO
                    ,   TO_CHAR(TE.KEN_DATE, 'YYYY/MM/DD HH24:MI:SS') AS KEN_DATE
                    ,   USERS.USER_NAME AS KEN_USER_NAME
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
                        M_TRANSPORTERS MT
                ON
                        MT.SHIPPER_ID = TE.SHIPPER_ID
                    AND MT.TRANSPORTER_ID = TE.TRANSPORTER_ID
                LEFT JOIN
                        M_ITEM_SKU MIS
                ON
                        MIS.SHIPPER_ID = TE.SHIPPER_ID
                    AND MIS.ITEM_SKU_ID = TE.ITEM_SKU_ID
                LEFT JOIN
                        M_ITEM_CATEGORIES4 MIC
                ON
                        MIC.SHIPPER_ID = MIS.SHIPPER_ID
                    AND MIC.CATEGORY_ID1 = MIS.CATEGORY_ID1
                    AND MIC.CATEGORY_ID2 = MIS.CATEGORY_ID2
                    AND MIC.CATEGORY_ID3 = MIS.CATEGORY_ID3
                    AND MIC.CATEGORY_ID4 = MIS.CATEGORY_ID4
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
                        M_GENERALS MG1
                ON
                        MG1.SHIPPER_ID = TE.SHIPPER_ID
                    AND MG1.CENTER_ID = '@@@'
                    AND MG1.REGISTER_DIVI_CD = '1'
                    AND MG1.GEN_DIV_CD = 'EC_SHIP_CLASS'
                    AND MG1.GEN_CD = TO_CHAR(TE.EC_SHIP_CLASS)
                LEFT JOIN
                        M_GENERALS MG2
                ON
                        MG2.SHIPPER_ID = TE.SHIPPER_ID
                    AND MG2.CENTER_ID = '@@@'
                    AND MG2.REGISTER_DIVI_CD = '1'
                    AND MG2.GEN_DIV_CD = (CASE WHEN TE.TRANSPORTER_ID = '01' THEN 'YAMATO_ARRIVE_REQUEST_TIME' WHEN TE.TRANSPORTER_ID = '02' THEN 'SAGAWA_ARRIVE_REQUEST_TIME' ELSE ' ' END)
                    AND MG2.GEN_CD = TE.ARRIVE_REQUEST_TIME
                LEFT OUTER JOIN
                        M_USERS USERS
                ON
                        TE.KEN_USER_ID = USERS.USER_ID
                    AND TE.SHIPPER_ID   = USERS.SHIPPER_ID
                WHERE
                        TE.SHIPPER_ID = :SHIPPER_ID
                    AND TE.CENTER_ID = :CENTER_ID
                    AND TE.SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", EcConfirmProgress01.CenterId);
            parameters.Add(":SHIP_INSTRUCT_ID", EcConfirmProgress01.ShipInstructId);
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

            return MvcDbContext.Current.Database.Connection.Query<EcConfirmProgress02ResultRow>(query.ToString(), parameters);
        }

        /// <summary>
        /// 出荷確定
        /// </summary>
        public void ShipConfirm(long seq, string center_id, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", center_id, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_WORK_ID", seq, DbType.Int32, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_EC_CONFIRMPROGRESSPACKING_ACTUAL",
                param,
                commandType: CommandType.StoredProcedure);

            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }

        /// <summary>
        /// Update Work Table
        /// </summary>
        /// <param name="References"></param>
        /// <returns></returns>
        public bool UpdateShpEcConfirmProgress(IList<SelectedEcConfirmProgress01ViewModel> References)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in References)
                {
                    // 在庫明細
                    var reference = MvcDbContext.Current.ShpEcConfirmProgress
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
        public bool ShpEcConfirmProgressAllChange(EcConfirmProgress01SearchConditions conditions, bool check)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query;
                    query = new StringBuilder(@"
                        UPDATE  WW_SHP_EC_CONFIRM_PROGRESS
                        SET
                                UPDATE_DATE = :UPDATE_DATE
                            ,   UPDATE_USER_ID = :UPDATE_USER_ID
                            ,   UPDATE_PROGRAM_NAME = 'EcConfirmProgress'
                            ,   UPDATE_COUNT = UPDATE_COUNT + 1
                            ,   IS_CHECK = :IS_CHECK
                        WHERE
                                SEQ = :SEQ
                            AND SHIPPER_ID = :SHIPPER_ID
                            AND SHIP_STATUS_NAME = :SHIP_STATUS_NAME
                    ");
                    parameters.Add(":UPDATE_DATE", DateTimeOffset.Now);
                    parameters.Add(":UPDATE_USER_ID", Profile.User.UserId);
                    parameters.Add(":IS_CHECK", (check == true) ? 1 : 0);
                    parameters.Add(":SEQ", conditions.Seq);
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                    parameters.Add(":SHIP_STATUS_NAME", EcConfirmProgressResource.DeliveryIssued);
                    var result = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }
                trans.Commit();
            }

            conditions.SelectedCnt = MvcDbContext.Current.ShpEcConfirmProgress
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq && m.IsCheck).Count();
            return true;
        }

        /// <summary>
        /// 配送業者データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListTransporters()
        {
            return MvcDbContext.Current.Transporters
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.TransporterId.ToString(),
                    Text = m.TransporterName
                })
                .OrderBy(m => m.Value);
        }
    }
}