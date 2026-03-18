namespace Wms.Areas.Ship.Query.TransferReference
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
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.Models;
    using Wms.Areas.Ship.ViewModels.TransferReference;
    using Wms.Models;
    using Wms.Query;
    using Wms.Common;
    using static Wms.Areas.Ship.ViewModels.TransferReference.TransferReferenceSearchConditions;
    using StyleCop.CSharp;

    public class TransferReferenceQuery
    {
        #region TC出荷表一覧
        /// <summary>
        /// Get Tc List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IList<TransferReferenceTcRow> GetTcData(TransferReferenceSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();

            query.Append(@"
                WITH
                    BASE_T AS (
                        SELECT
                                SHIP_PLAN_DATE
                            ,   SHIP_TO_STORE_ID
                            ,   ITEM_SKU_ID
                            ,   ALLOC_QTY
                            ,   BATCH_NO
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   STOCK_OUT_FIX_QTY
                        FROM
                                T_STORE_SORT
                        WHERE
                                TCDC_CLASS = 1
                            AND SHIPPER_ID = :SHIPPER_ID
                            AND CENTER_ID = :CENTER_ID
                            AND BATCH_NO <> ' '
                )
                ,   SUB_A AS (
                        SELECT
                                SUB_H.SHIP_PLAN_DATE
                            ,   MAX(SUB_H.BATCH_NO) AS SUB_H_BATCH_NO
                            ,   COUNT(DISTINCT(SUB_H.SHIP_TO_STORE_ID) ) AS CNT_SHIP_TO_STORE_ID
                            ,   COUNT(DISTINCT(SUB_H.ITEM_SKU_ID) ) AS CNT_ITEM_SKU_ID
                            ,   COUNT(ROWID) AS CNT_SHIP_INSTRUCT_SEQ
                            ,   SUM(SUB_H.ALLOC_QTY) AS SUM_ALLOC_QTY
                            ,   SUB_H.CENTER_ID
                            ,   SUB_H.SHIPPER_ID
                        FROM
                                BASE_T SUB_H
                        GROUP BY
                                SUB_H.SHIP_PLAN_DATE
                            ,   SUB_H.CENTER_ID
                            ,   SUB_H.SHIPPER_ID
                ) 
                ,   SUBL AS (
                        SELECT
                                SUBQ1.SHIP_PLAN_DATE
                            ,   TLS.ITEM_SKU_ID
                            ,   TLS.ALLOC_QTY
                            ,   TLS.SORT_QTY
                            ,   DECODE(TLS.SORT_STATUS, 2, 1, 0) AS FINISH_SKU_FLG
                            ,   DECODE(TLS.SORT_STATUS, 2, TLS.SORT_QTY, 0) AS FINISH_SKU_QTY
                        FROM
                                T_LANE_SORT TLS
                        LEFT JOIN (
                            SELECT
                                    TS.BATCH_NO
                                ,   TS.SHIP_PLAN_DATE
                            FROM
                                    BASE_T TS
                            GROUP BY
                                    TS.SHIP_PLAN_DATE
                                ,   TS.BATCH_NO
                        ) SUBQ1
                        ON
                                SUBQ1.BATCH_NO = TLS.BATCH_NO
                        WHERE
                                TLS.TCDC_CLASS = 1
                            AND TLS.SHIPPER_ID = :SHIPPER_ID
                            AND TLS.CENTER_ID = :CENTER_ID
                            AND TLS.BATCH_NO IN ( SELECT TS2.BATCH_NO FROM BASE_T TS2)
                ) 
                ,   SUB_B AS (
                        SELECT
                                SUBT.SHIP_PLAN_DATE
                            ,   SUM(SUBT.FINISH_SKU_FLG) AS FINISH_SKU_FLG
                            ,   SUM(SUBT.FINISH_SORT_QTY) AS FINISH_SORT_QTY
                            ,   DECODE(SUM(SUBT.ALLOC_QTY) ,0,0,(TRUNC((SUM(SUBT.FINISH_SORT_QTY) / SUM(SUBT.ALLOC_QTY) ) * 100)))  AS STORE_LANE_PERCENT
                            ,   DECODE(SUM(SUBT.CNTR), SUM(SUBT.CNTS), '" + TransferReferenceResource.ComEnd + @"', '') AS FINISH_LANE_STS
                        FROM (
                            SELECT
                                    S.SHIP_PLAN_DATE
                                ,   S.ITEM_SKU_ID
                                ,   DECODE(COUNT(*), SUM(S.FINISH_SKU_FLG), 1, 0) AS FINISH_SKU_FLG
                                ,   SUM(S.ALLOC_QTY) AS ALLOC_QTY
                                ,   SUM(S.FINISH_SKU_QTY) AS FINISH_SORT_QTY
                                ,   COUNT(*) AS CNTR
                                ,   SUM(S.FINISH_SKU_FLG) AS CNTS
                            FROM
                                    SUBL S
                            GROUP BY
                                    S.SHIP_PLAN_DATE
                                ,   S.ITEM_SKU_ID
                        ) SUBT
                        GROUP BY
                                SUBT.SHIP_PLAN_DATE
                ) 
                ,   SUBS AS (
                        SELECT
                                SUBQ1.SHIP_PLAN_DATE
                            ,   TSS.BATCH_NO
                            ,   TSS.SHIP_TO_STORE_ID
                            ,   TSS.ITEM_SKU_ID
                            ,   TSS.ALLOC_QTY
                            ,   TSS.SORT_QTY
                            ,   TSS.STOCK_OUT_REG_QTY
                            ,   TSS.STOCK_OUT_FIX_QTY
                            ,   DECODE(TSS.SORT_STATUS, 3, 1, 0) AS FINISH_SORT_FLG
                        FROM
                                T_STORE_SORT TSS
                        LEFT OUTER JOIN (
                            SELECT
                                    TS.BATCH_NO
                                ,   TS.SHIP_PLAN_DATE
                            FROM
                                    BASE_T TS
                            GROUP BY
                                    TS.SHIP_PLAN_DATE
                                ,   TS.BATCH_NO
                        ) SUBQ1
                        ON
                                SUBQ1.BATCH_NO = TSS.BATCH_NO
                        WHERE
                                TSS.TCDC_CLASS = 1
                            AND TSS.SHIPPER_ID = :SHIPPER_ID
                            AND TSS.CENTER_ID = :CENTER_ID
                            AND TSS.BATCH_NO IN (
                                SELECT TS2.BATCH_NO FROM BASE_T TS2)
                ) 
                ,   SUB_C AS (
                        SELECT
                                TSS_SUM.SHIP_PLAN_DATE
                            ,   SUM(TSS_SUM.SKU_FIN_FLG) AS SKU_FIN_SUU
                            ,   SUM(TSS_SUM.FINISH_SORT_QTY) AS FINISH_SORT_QTY
                            ,   DECODE(SUM(TSS_SUM.ALLOC_QTY) ,0,0,(TRUNC(((SUM(TSS_SUM.FINISH_SORT_QTY) + SUM(TSS_SUM.STOCK_OUT_FIX_QTY) ) / SUM(TSS_SUM.ALLOC_QTY) ) * 100)))  AS STORE_SORT_PERCENT
                            ,   DECODE(SUM(TSS_SUM.CNTR) ,   SUM(TSS_SUM.CNTS), '" + TransferReferenceResource.ComEnd + @"', '') AS FINISH_ORDER_STS
                            ,   SUM(TSS_SUM.STOCK_OUT_REG_QTY) AS STOCK_OUT_REG_QTY
                            ,   SUM(TSS_SUM.STOCK_OUT_FIX_QTY) AS STOCK_OUT_FIX_QTY
                        FROM (
                            SELECT
                                    S.SHIP_PLAN_DATE
                                ,   DECODE(COUNT(*), SUM(S.FINISH_SORT_FLG), 1, 0) AS SKU_FIN_FLG
                                ,   SUM(S.ALLOC_QTY) AS ALLOC_QTY
                                ,   SUM(S.SORT_QTY) AS FINISH_SORT_QTY
                                ,   SUM(S.STOCK_OUT_REG_QTY) AS STOCK_OUT_REG_QTY
                                ,   SUM(S.STOCK_OUT_FIX_QTY) AS STOCK_OUT_FIX_QTY
                                ,   COUNT(*) AS CNTR
                                ,   SUM(S.FINISH_SORT_FLG) AS CNTS
                            FROM
                                    SUBS S
                            GROUP BY
                                    S.SHIP_PLAN_DATE
                                ,   S.ITEM_SKU_ID
                        ) TSS_SUM
                        GROUP BY
                                TSS_SUM.SHIP_PLAN_DATE
                ) 
                ,    TARGET_ALL_PACKING_DATA AS (
                        SELECT
                                PACK.BOX_NO
                            ,   PACK.SHIP_INSTRUCT_ID
                            ,   PACK.SHIP_INSTRUCT_SEQ
                            ,   PACK.CENTER_ID
                            ,   PACK.SHIPPER_ID
                            ,   PACK.RESULT_QTY
                            ,   PACK.NOUHIN_PRN_FLAG
                            ,   PACK.DELI_PRN_FLAG
                            ,   PACK.BATCH_NO
                        FROM
                                T_SHIP_PACKING_INFO PACK
                        WHERE
                                PACK.CENTER_ID = :CENTER_ID
                            AND PACK.SHIPPER_ID = :SHIPPER_ID
                        UNION
                        SELECT
                                PACK.BOX_NO
                            ,   PACK.SHIP_INSTRUCT_ID
                            ,   PACK.SHIP_INSTRUCT_SEQ
                            ,   PACK.CENTER_ID
                            ,   PACK.SHIPPER_ID
                            ,   PACK.RESULT_QTY
                            ,   PACK.NOUHIN_PRN_FLAG
                            ,   PACK.DELI_PRN_FLAG
                            ,   PACK.BATCH_NO
                        FROM
                                A_SHIP_PACKING_INFO PACK
                        WHERE
                                PACK.CENTER_ID = :CENTER_ID
                            AND PACK.SHIPPER_ID = :SHIPPER_ID
                )
                ,   GROUP_SHIP_INSTRUCT AS (
                        SELECT
                                BASE.SHIP_PLAN_DATE AS SHIP_PLAN_DATE
                            ,   SUM(CASE
                                        WHEN PACK.NOUHIN_PRN_FLAG IN (8, 9) AND PACK.DELI_PRN_FLAG <> 0 THEN PACK.RESULT_QTY
                                        ELSE 0
                                    END) + MAX(BASE.STOCK_OUT_FIX_QTY) AS PRN_QTY
                            ,   MAX(BASE.ALLOC_QTY) AS ALLOC_QTY
                        FROM
                                BASE_T BASE
                        LEFT OUTER JOIN
                                TARGET_ALL_PACKING_DATA PACK
                        ON
                                PACK.SHIP_INSTRUCT_ID = BASE.SHIP_INSTRUCT_ID
                            AND PACK.SHIP_INSTRUCT_SEQ = BASE.SHIP_INSTRUCT_SEQ
                            AND PACK.CENTER_ID = BASE.CENTER_ID
                            AND PACK.SHIPPER_ID = BASE.SHIPPER_ID
                        GROUP BY
                                BASE.SHIP_INSTRUCT_ID
                            ,   BASE.SHIP_INSTRUCT_SEQ
                            ,   BASE.CENTER_ID
                            ,   BASE.SHIPPER_ID
                            ,   BASE.SHIP_PLAN_DATE
                )
                ,   SUB_D AS (
                        SELECT
                                SHIP_PLAN_DATE
                            ,   SUM(PRN_QTY) AS HAKKOUSUU
                            ,   CASE
                                    WHEN SUM(PRN_QTY) = 0 THEN 0
                                    ELSE TRUNC((SUM(PRN_QTY) / SUM(ALLOC_QTY)) * 100)
                                END AS NOUHINSYO_PERCENT
                        FROM
                                GROUP_SHIP_INSTRUCT
                        GROUP BY
                                SHIP_PLAN_DATE
                ) 
                SELECT
                        A.SHIP_PLAN_DATE
                    ,   A.SUB_H_BATCH_NO
                    ,   A.CNT_SHIP_TO_STORE_ID SHIP_TO_STORE_QTY
                    ,   A.CNT_ITEM_SKU_ID ITEM_SKU_QTY
                    ,   A.CNT_SHIP_INSTRUCT_SEQ SHIP_INSTRUCT_SEQ_QTY
                    ,   A.SUM_ALLOC_QTY ALLOC_QTY
                    ,   B.FINISH_SKU_FLG LANE_SORT_ITEM_SKU_QTY
                    ,   B.FINISH_SORT_QTY LANE_SORT_SORT_QTY
                    ,   B.STORE_LANE_PERCENT LANE_SORT_PERCENT
                    ,   B.FINISH_LANE_STS LANE_SORT_END_STATUS
                    ,   C.SKU_FIN_SUU STORE_SORT_ITEM_SKU_QTY
                    ,   C.FINISH_SORT_QTY STORE_SORT_SORT_QTY
                    ,   C.STORE_SORT_PERCENT STORE_SORT_PERCENT
                    ,   C.STOCK_OUT_REG_QTY
                    ,   C.STOCK_OUT_FIX_QTY
                    ,   C.FINISH_ORDER_STS STORE_SORT_END_STATUS
                    ,   D.HAKKOUSUU NOUHIN_PRN_QTY
                    ,   D.NOUHINSYO_PERCENT NOUHIN_PRN_PERCENT
                FROM
                        SUB_A A
                LEFT OUTER JOIN
                        SUB_B B
                ON
                        B.SHIP_PLAN_DATE = A.SHIP_PLAN_DATE
                LEFT OUTER JOIN
                        SUB_C C
                ON
                        C.SHIP_PLAN_DATE = A.SHIP_PLAN_DATE
                LEFT OUTER JOIN
                        SUB_D D
                ON
                        D.SHIP_PLAN_DATE = A.SHIP_PLAN_DATE 
                ORDER BY
                        A.SHIP_PLAN_DATE
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<TransferReferenceTcRow>(query.ToString(), parameters).ToList();
        }
        #endregion

        #region DC出荷表一覧
        /// <summary>
        /// Get Dc List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<TransferReferenceDcRow> GetDcData(TransferReferenceSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
       
            query.AppendLine(@"
                WITH
                    BASE_T AS(
                        SELECT
                                BATCH_NO
                            ,   SHIP_TO_STORE_ID
                            ,   ITEM_SKU_ID
                            ,   ALLOC_QTY
                            ,   SHIPPER_ID
                            ,   CENTER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   STOCK_OUT_FIX_QTY
                        FROM
                                T_STORE_SORT
                        WHERE
                                TCDC_CLASS = 2
                            AND SHIPPER_ID = :SHIPPER_ID
                            AND CENTER_ID = :CENTER_ID
                            AND BATCH_NO <> ' '                 
                )
                --引当情報取得
                ,   SUB_F AS (
                        SELECT 
                                TAI.ALLOC_NO AS BATCH_NO
                            ,   TAI.BATCH_NAME
                            ,   TAI.SHIPPER_ID
                            ,   TAI.CENTER_ID
                            ,   TAI.PICK_KIND
                            ,   TAI.ALLOC_DATE
                        FROM
                                T_ALLOC_INFO TAI
                        WHERE
                                TAI.SHIP_KIND = 2
                            AND TAI.SHIPPER_ID = :SHIPPER_ID
                            AND TAI.CENTER_ID = :CENTER_ID
                )
                --店別仕分けをバッチ単位で取得
                ,   SUB_A AS(
                        SELECT
                                SUB_H.BATCH_NO
                            ,   COUNT(DISTINCT(SUB_H.SHIP_TO_STORE_ID) ) AS CNT_SHIP_TO_STORE_ID
                            ,   COUNT(DISTINCT(SUB_H.ITEM_SKU_ID) ) AS CNT_ITEM_SKU_ID
                            ,   COUNT(SUB_H.ROWID) AS CNT_SHIP_INSTRUCT_SEQ
                            ,   SUM(SUB_H.ALLOC_QTY) AS SUM_ALLOC_QTY
                            ,   SUB_H.CENTER_ID
                            ,   SUB_H.SHIPPER_ID
                            ,   MAX(F.BATCH_NAME) AS BATCH_NAME
                            ,   SUM(STOCK_OUT_FIX_QTY) AS STOCK_OUT_FIX_QTY
                            ,   MAX(F.PICK_KIND) AS PICK_KIND
                        FROM
                                BASE_T SUB_H
                        INNER JOIN
                                SUB_F F
                        ON
                                F.BATCH_NO = SUB_H.BATCH_NO
                            AND F.CENTER_ID = SUB_H.CENTER_ID
                            AND F.SHIPPER_ID = SUB_H.SHIPPER_ID
                        GROUP BY
                                SUB_H.BATCH_NO
                            ,   SUB_H.CENTER_ID
                            ,   SUB_H.SHIPPER_ID
                ) 
                    --出荷ピック情報取得
                ,   SUB_E AS (
                        SELECT
                                SUBF_T.BATCH_NO
                            ,   CASE
                                    WHEN SUBF_T.HIKI_QTY = 0 THEN 0
                                    ELSE TRUNC(((SUBF_T.PIC_QTY + SUBF_T2.PIC_QTY + SUBF_T.STOCK_OUT_FIX_QTY + SUBF_T2.STOCK_OUT_FIX_QTY) / SUBF_T.HIKI_QTY) * 100)
                                END AS PIC_PERCENT
                            ,   SUBF_T.PIC_QTY + SUBF_T2.PIC_QTY AS PIC_QTY
                            ,   SUBF_T.STOCK_OUT_REG_QTY + SUBF_T2.STOCK_OUT_REG_QTY AS PIC_STOCK_OUT_REG_QTY
                            ,   SUBF_T.STOCK_OUT_FIX_QTY + SUBF_T2.STOCK_OUT_FIX_QTY AS PIC_STOCK_OUT_FIX_QTY
                        FROM (
                            SELECT
                                    TP.BATCH_NO
                                ,   SUM(TP.HIKI_QTY) AS HIKI_QTY
                                ,   SUM(TP.PIC_QTY) AS PIC_QTY
                                ,   SUM(TP.STOCK_OUT_REG_QTY) AS STOCK_OUT_REG_QTY
                                ,   SUM(TP.STOCK_OUT_FIX_QTY) AS STOCK_OUT_FIX_QTY
                            FROM
                                    T_PIC TP
                            INNER JOIN
                                    SUB_A SUB
                            ON
                                    TP.BATCH_NO = SUB.BATCH_NO
                                AND TP.CENTER_ID = SUB.CENTER_ID
                                AND TP.SHIPPER_ID = SUB.SHIPPER_ID
                            WHERE
                                    TP.SHIPPER_ID = :SHIPPER_ID
                                AND TP.CENTER_ID = :CENTER_ID
                            GROUP BY
                                    TP.BATCH_NO
                        ) SUBF_T
                        LEFT JOIN (
                            SELECT
                                    TOP.BATCH_NO
                                ,   SUM(TOP.HIKI_QTY) AS HIKI_QTY
                                ,   SUM(TOP.PIC_QTY) AS PIC_QTY
                                ,   SUM(TOP.STOCK_OUT_REG_QTY) AS STOCK_OUT_REG_QTY
                                ,   SUM(TOP.STOCK_OUT_FIX_QTY) AS STOCK_OUT_FIX_QTY
                            FROM
                                    T_ORDER_PIC TOP
                            INNER JOIN
                                    SUB_A SUB
                            ON
                                    TOP.BATCH_NO = SUB.BATCH_NO
                                AND TOP.CENTER_ID = SUB.CENTER_ID
                                AND TOP.SHIPPER_ID = SUB.SHIPPER_ID
                            WHERE
                                    TOP.SHIPPER_ID = :SHIPPER_ID
                                AND TOP.CENTER_ID = :CENTER_ID
                            GROUP BY
                                    TOP.BATCH_NO
                        ) SUBF_T2
                        ON
                                SUBF_T2.BATCH_NO = SUBF_T.BATCH_NO
                ) 
                    --レーン仕分情報取得
                ,   SUBL AS (
                        SELECT
                                TLS.BATCH_NO
                            ,   TLS.ITEM_SKU_ID
                            ,   TLS.ALLOC_QTY
                            ,   TLS.SORT_QTY
                            ,   DECODE(TLS.SORT_STATUS, 2, 1, 0) AS FINISH_SKU_FLG
                            ,   DECODE(TLS.SORT_STATUS, 2, TLS.SORT_QTY, 0) AS FINISH_SKU_QTY
                        FROM
                                T_LANE_SORT TLS
                        INNER JOIN
                                SUB_A SUB
                        ON
                                TLS.BATCH_NO = SUB.BATCH_NO
                            AND TLS.CENTER_ID = SUB.CENTER_ID
                            AND TLS.SHIPPER_ID = SUB.SHIPPER_ID
                        WHERE
                                TLS.TCDC_CLASS = 2
                            AND TLS.SHIPPER_ID = :SHIPPER_ID
                            AND TLS.CENTER_ID = :CENTER_ID
                )
                    --レーン仕分情報バッチ単位
                ,   SUB_B AS(
                        SELECT
                                SUBT.BATCH_NO
                            ,   SUM(SUBT.FINISH_SKU_FLG) AS FINISH_SKU_FLG
                            ,   SUM(SUBT.FINISH_SORT_QTY) AS FINISH_SORT_QTY
                            ,   DECODE(SUM(SUBT.ALLOC_QTY) ,0,0,(TRUNC((SUM(SUBT.FINISH_SORT_QTY) / SUM(SUBT.ALLOC_QTY) ) * 100)))  AS STORE_LANE_PERCENT
                            ,   DECODE(SUM(SUBT.CNTR), SUM(SUBT.CNTS), '" + TransferReferenceResource.ComEnd + @"', '') AS FINISH_LANE_STS
                        FROM (
                            SELECT
                                    S.BATCH_NO
                                ,   S.ITEM_SKU_ID
                                ,   DECODE(COUNT(*), SUM(S.FINISH_SKU_FLG), 1, 0 ) AS FINISH_SKU_FLG
                                ,   SUM(S.ALLOC_QTY) AS ALLOC_QTY
                                ,   SUM(S.FINISH_SKU_QTY) AS FINISH_SORT_QTY
                                ,   COUNT(*) AS CNTR
                                ,   SUM(S.FINISH_SKU_FLG) AS CNTS
                            FROM
                                    SUBL S
                            GROUP BY
                                    S.BATCH_NO
                                ,   S.ITEM_SKU_ID
                        ) SUBT
                        GROUP BY
                                SUBT.BATCH_NO
                ) 
                    --店別仕分情報取得
                ,   SUBS AS (
                        SELECT
                                TSS.BATCH_NO
                            ,   TSS.SHIP_TO_STORE_ID
                            ,   TSS.ITEM_SKU_ID
                            ,   TSS.ALLOC_QTY
                            ,   TSS.SORT_QTY
                            ,   TSS.ORDER_PIC_QTY
                            ,   TSS.STOCK_OUT_REG_QTY
                            ,   TSS.STOCK_OUT_FIX_QTY
                            ,   DECODE(TSS.SORT_STATUS, 3, 1, 0) AS FINISH_SORT_FLG
                            ,   DECODE(TSS.SORT_STATUS, 3, TSS.SHIP_TO_STORE_ID, 'DAMY') AS FINISH_SHIP_TO_STORE_ID
                            ,   DECODE(TSS.ORDER_PIC_STATUS, 3, 1, 0) AS FINISH_ORDER_FLG
                            ,   DECODE(TSS.SORT_STATUS, 3, TSS.SORT_QTY, 0) AS FINISH_SORT_QTY
                            ,   DECODE(TSS.ORDER_PIC_STATUS, 3, TSS.ORDER_PIC_QTY, 0) AS FINISH_ORDER_PIC_QTY
                            ,   TSS.SORT_STATUS
                            ,   TSS.ORDER_PIC_STATUS
                        FROM
                                T_STORE_SORT TSS
                        INNER JOIN
                                SUB_A SUB
                        ON
                                TSS.BATCH_NO = SUB.BATCH_NO
                            AND TSS.CENTER_ID = SUB.CENTER_ID
                            AND TSS.SHIPPER_ID = SUB.SHIPPER_ID
                        WHERE
                                TSS.TCDC_CLASS = 2
                            AND TSS.SHIPPER_ID = :SHIPPER_ID
                            AND TSS.CENTER_ID = :CENTER_ID
                ) 
                    --店別仕分バッチ単位
                ,   SUB_C1 AS (
                        SELECT
                                TSS_SUM.BATCH_NO
                            ,   SUM(TSS_SUM.SKU_FIN_FLG) AS SKU_FIN_SUU
                            ,   SUM(TSS_SUM.FINISH_SORT_QTY) AS FINISH_SORT_QTY
                            ,   CASE 
                                    WHEN MAX(TSS_SUM.MAX_SORT_STATUS) <> 0 THEN
                                        DECODE(SUM(TSS_SUM.ALLOC_QTY) ,0,0,(TRUNC(((SUM(FINISH_SORT_QTY) + SUM(TSS_SUM.STOCK_OUT_FIX_QTY) ) / SUM(TSS_SUM.ALLOC_QTY) ) * 100)))
                                    ELSE 0
                                END AS STORE_SORT_PERCENT
                            ,   DECODE(SUM(TSS_SUM.CNTR) 
                            ,   SUM(TSS_SUM.CNTS), '" + TransferReferenceResource.ComEnd + @"', '') AS FINISH_SORT_STS
                            ,   SUM(TSS_SUM.STOCK_OUT_REG_QTY) AS STOCK_OUT_REG_QTY
                            ,   SUM(TSS_SUM.STOCK_OUT_FIX_QTY) AS STOCK_OUT_FIX_QTY
                        FROM (
                            SELECT
                                    S.BATCH_NO
                                ,   DECODE(COUNT(*), SUM(S.FINISH_SORT_FLG), 1, 0) AS SKU_FIN_FLG
                                ,   S.ITEM_SKU_ID
                                ,   SUM(S.ALLOC_QTY) AS ALLOC_QTY
                                ,   SUM(S.SORT_QTY) AS FINISH_SORT_QTY
                                ,   SUM(S.STOCK_OUT_REG_QTY) AS STOCK_OUT_REG_QTY
                                ,   SUM(S.STOCK_OUT_FIX_QTY) AS STOCK_OUT_FIX_QTY
                                ,   COUNT(*) AS CNTR
                                ,   SUM(S.FINISH_SORT_FLG) AS CNTS
                                ,   MAX(S.SORT_STATUS) AS MAX_SORT_STATUS
                            FROM
                                    SUBS S
                            GROUP BY
                                    S.BATCH_NO
                                ,   S.ITEM_SKU_ID
                        ) TSS_SUM
                        GROUP BY
                                TSS_SUM.BATCH_NO
                ) 
                    --摘取バッチ単位
                ,   SUB_C2 AS (
                        SELECT
                                TSS_SUM.BATCH_NO
                            ,   SUM(TSS_SUM.STORE_FIN_FLG) AS STORE_FIN_SUU_2
                            ,   SUM(TSS_SUM.FINISH_ORDER__QTY) AS FINISH_ORDER_QTY
                            ,   CASE
                                    WHEN MAX(TSS_SUM.MAX_ORDER_PIC_STATUS) <> 0 THEN
                                        DECODE(SUM(TSS_SUM.ALLOC_QTY) ,0,0,(TRUNC(((SUM(TSS_SUM.FINISH_ORDER__QTY) + SUM(TSS_SUM.STOCK_OUT_FIX_QTY) ) / SUM(TSS_SUM.ALLOC_QTY) ) * 100)))
                                    ELSE 0
                                END AS STORE_ORDER_PERCENT
                        FROM (
                            SELECT
                                    S.BATCH_NO
                                ,   DECODE(COUNT(*), SUM(S.FINISH_ORDER_FLG), 1, 0) AS STORE_FIN_FLG
                                ,   S.SHIP_TO_STORE_ID
                                ,   SUM(S.ALLOC_QTY) AS ALLOC_QTY
                                ,   SUM(S.ORDER_PIC_QTY) AS FINISH_ORDER__QTY
                                ,   SUM(S.STOCK_OUT_REG_QTY) AS STOCK_OUT_REG_QTY
                                ,   SUM(S.STOCK_OUT_FIX_QTY) AS STOCK_OUT_FIX_QTY
                                ,   COUNT(*) AS CNTR
                                ,   SUM(S.FINISH_ORDER_FLG) AS CNTS
                                ,   MAX(S.ORDER_PIC_STATUS) AS MAX_ORDER_PIC_STATUS
                            FROM
                                    SUBS S
                            GROUP BY
                                    S.BATCH_NO
                                ,   S.SHIP_TO_STORE_ID
                         ) TSS_SUM
                        GROUP BY
                                TSS_SUM.BATCH_NO
                )
                --出荷梱包実績情報取得
                ,    TARGET_ALL_PACKING_DATA AS (
                        SELECT
                                PACK.BOX_NO
                            ,   PACK.SHIP_INSTRUCT_ID
                            ,   PACK.SHIP_INSTRUCT_SEQ
                            ,   PACK.CENTER_ID
                            ,   PACK.SHIPPER_ID
                            ,   PACK.RESULT_QTY
                            ,   PACK.NOUHIN_PRN_FLAG
                            ,   PACK.DELI_PRN_FLAG
                            ,   PACK.BATCH_NO
                        FROM
                                T_SHIP_PACKING_INFO PACK
                        WHERE
                                PACK.CENTER_ID = :CENTER_ID
                            AND PACK.SHIPPER_ID = :SHIPPER_ID
                        UNION
                        SELECT
                                PACK.BOX_NO
                            ,   PACK.SHIP_INSTRUCT_ID
                            ,   PACK.SHIP_INSTRUCT_SEQ
                            ,   PACK.CENTER_ID
                            ,   PACK.SHIPPER_ID
                            ,   PACK.RESULT_QTY
                            ,   PACK.NOUHIN_PRN_FLAG
                            ,   PACK.DELI_PRN_FLAG
                            ,   PACK.BATCH_NO
                        FROM
                                A_SHIP_PACKING_INFO PACK
                        WHERE
                                PACK.CENTER_ID = :CENTER_ID
                            AND PACK.SHIPPER_ID = :SHIPPER_ID
                )
                ,   GROUP_SHIP_INSTRUCT AS (
                        SELECT
                                BAT.BATCH_NO AS BATCH_NO
                            ,   SUM(CASE
                                        WHEN PACK.NOUHIN_PRN_FLAG IN (8, 9) AND PACK.DELI_PRN_FLAG <> 0 THEN PACK.RESULT_QTY
                                        ELSE 0
                                    END) + MAX(BAT.STOCK_OUT_FIX_QTY) AS PRN_QTY
                            ,   MAX(BAT.SUM_ALLOC_QTY) AS ALLOC_QTY
                        FROM
                                SUB_A BAT
                        LEFT OUTER JOIN
                                TARGET_ALL_PACKING_DATA PACK
                        ON
                                PACK.BATCH_NO = BAT.BATCH_NO
                            AND PACK.CENTER_ID = BAT.CENTER_ID
                            AND PACK.SHIPPER_ID = BAT.SHIPPER_ID
                        GROUP BY
                                BAT.BATCH_NO
                            ,   BAT.CENTER_ID
                            ,   BAT.SHIPPER_ID
                )
                ,   SUB_D AS (
                        SELECT
                                BATCH_NO
                            ,   PRN_QTY AS HAKKOUSUU
                            ,   CASE
                                    WHEN PRN_QTY = 0 THEN 0
                                    ELSE TRUNC((PRN_QTY / ALLOC_QTY) * 100)
                                END AS NOUHINSYO_PERCENT
                        FROM
                                GROUP_SHIP_INSTRUCT
                ) 
                SELECT
                        A.BATCH_NO
                    ,   A.BATCH_NAME
                    ,   A.CNT_SHIP_TO_STORE_ID SHIP_TO_STORE_QTY
                    ,   A.CNT_ITEM_SKU_ID ITEM_SKU_QTY
                    ,   A.CNT_SHIP_INSTRUCT_SEQ SHIP_INSTRUCT_SEQ_QTY
                    ,   A.SUM_ALLOC_QTY ALLOC_QTY
                    ,   E.PIC_QTY PIC_PIC_QTY
                    ,   E.PIC_PERCENT PIC_PIC_PERCENT
                    ,   E.PIC_STOCK_OUT_REG_QTY
                    ,   E.PIC_STOCK_OUT_FIX_QTY
                    ,   B.FINISH_SKU_FLG LANE_SORT_ITEM_SKU_QTY
                    ,   B.FINISH_SORT_QTY LANE_SORT_SORT_QTY
                    ,   B.STORE_LANE_PERCENT LANE_SORT_PERCENT
                    ,   B.FINISH_LANE_STS LANE_SORT_END_STATUS
                    ,   C1.SKU_FIN_SUU STORE_SORT_ITEM_SKU_QTY
                    ,   C1.FINISH_SORT_QTY STORE_SORT_SORT_QTY
                    ,   C1.STORE_SORT_PERCENT 
                    ,   C1.FINISH_SORT_STS STORE_SORT_END_STATUS
                    ,   C2.STORE_FIN_SUU_2 ORDER_PIC_SHIP_TO_STORE_QTY
                    ,   C2.FINISH_ORDER_QTY ORDER_PIC_QTY
                    ,   C2.STORE_ORDER_PERCENT ORDER_PIC_PERCENT
                    ,   C1.STOCK_OUT_REG_QTY
                    ,   C1.STOCK_OUT_FIX_QTY
                    ,   D.HAKKOUSUU NOUHIN_PRN_QTY
                    ,   D.NOUHINSYO_PERCENT NOUHIN_PRN_PERCENT
                    ,   A.PICK_KIND
                FROM
                        SUB_A A
                LEFT JOIN
                        SUB_E E
                ON
                        E.BATCH_NO = A.BATCH_NO
                LEFT JOIN
                        SUB_B B
                ON
                        B.BATCH_NO = A.BATCH_NO
                LEFT JOIN
                        SUB_C1 C1
                ON
                        C1.BATCH_NO = A.BATCH_NO
                LEFT JOIN
                        SUB_C2 C2
                ON
                        C2.BATCH_NO = A.BATCH_NO
                LEFT JOIN
                        SUB_D D
                ON
                        D.BATCH_NO = A.BATCH_NO

                LEFT JOIN
                        SUB_F F
                ON
                        F.BATCH_NO = A.BATCH_NO ");

            
            // 条件記述簡略化のため記述
            if (!string.IsNullOrEmpty(condition.BatchName) || (condition.AllocDateFrom != null) || (condition.AllocDateTo != null))
            {
                query.AppendLine(" WHERE 1 = 1");
            }

            //バッチ名称
            if (!string.IsNullOrEmpty(condition.BatchName))
            { 
                query.AppendLine(" AND F.BATCH_NAME LIKE :BATCH_NAME ");
                parameters.Add(":BATCH_NAME", "%" + condition.BatchName + "%");
            }

            // 引当日From
            if (condition.AllocDateFrom != null)
            {
                query.AppendLine(" AND TRUNC(F.ALLOC_DATE) >= :ALLOC_DATE_FROM ");
                parameters.Add(":ALLOC_DATE_FROM", condition.AllocDateFrom);
            }

            // 引当日To
            if (condition.AllocDateTo != null)
            {
                query.AppendLine(" AND TRUNC(F.ALLOC_DATE) <= :ALLOC_DATE_TO ");
                parameters.Add(":ALLOC_DATE_TO", condition.AllocDateTo);

            }

            query.AppendLine(@"ORDER BY
                A.BATCH_NO
                ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);

            int totalCount = MvcDbContext.Current.Database.Connection.Query<TransferReferenceDcRow>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            var References = MvcDbContext.Current.Database.Connection.Query<TransferReferenceDcRow>(query.ToString(), parameters);

            // Fill data to memory
            return new StaticPagedList<TransferReferenceDcRow>(References, condition.Page, condition.PageSize, totalCount);
        }
        #endregion DC出荷表一覧

        #region EC出荷表一覧
        /// <summary>
        /// Get Ec List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IList<TransferReferenceEcRow> GetEcData(TransferReferenceSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                WITH
                    SELECTED_BATCH_NO AS (
                        SELECT
                                BATCH_NO
                            ,   COUNT(DISTINCT(SHIP_INSTRUCT_ID)) AS SHIP_INSTRUCT_QTY
                            ,   COUNT(DISTINCT(ITEM_SKU_ID)) AS ITEM_SKU_QTY
                            ,   COUNT(SHIP_INSTRUCT_SEQ) AS SHIP_INSTRUCT_SEQ_QTY
                            ,   SUM(ALLOC_QTY) AS ALLOC_QTY
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                        FROM
                                T_ECSHIPS
                        WHERE 
                                SHIPPER_ID = :SHIPPER_ID
                            AND CENTER_ID = :CENTER_ID
                            AND BATCH_NO <> ' '
                        GROUP BY
                                BATCH_NO
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                )
                ,   SELECTED_A_ECSHIPS AS (
                        SELECT
                                EC.BATCH_NO
                            ,   COUNT(DISTINCT(EC.SHIP_INSTRUCT_ID)) AS SHIP_INSTRUCT_QTY
                            ,   COUNT(DISTINCT(EC.ITEM_SKU_ID)) AS ITEM_SKU_QTY
                            ,   COUNT(EC.SHIP_INSTRUCT_SEQ) AS SHIP_INSTRUCT_SEQ_QTY
                            ,   SUM(EC.ALLOC_QTY) AS ALLOC_QTY
                            ,   EC.CENTER_ID
                            ,   EC.SHIPPER_ID
                        FROM
                                A_ECSHIPS EC
                        INNER JOIN
                                SELECTED_BATCH_NO BATCH
                        ON
                                EC.BATCH_NO = BATCH.BATCH_NO
                            AND EC.CENTER_ID = BATCH.CENTER_ID
                            AND EC.SHIPPER_ID = BATCH.SHIPPER_ID
                        WHERE
                                EC.CENTER_ID = :CENTER_ID
                            AND EC.SHIPPER_ID = :SHIPPER_ID
                        GROUP BY
                                EC.BATCH_NO
                            ,   EC.CENTER_ID
                            ,   EC.SHIPPER_ID
                )
                ,   ALL_ECSHIPS_DATA AS (
                        SELECT
                                TRN.BATCH_NO
                            ,   TRN.SHIP_INSTRUCT_QTY + NVL(ACM.SHIP_INSTRUCT_QTY, 0) AS SHIP_INSTRUCT_QTY
                            ,   TRN.ITEM_SKU_QTY + NVL(ACM.ITEM_SKU_QTY, 0) AS ITEM_SKU_QTY
                            ,   TRN.SHIP_INSTRUCT_SEQ_QTY + NVL(ACM.SHIP_INSTRUCT_SEQ_QTY, 0) AS SHIP_INSTRUCT_SEQ_QTY
                            ,   TRN.ALLOC_QTY + NVL(ACM.ALLOC_QTY, 0) AS ALLOC_QTY
                        FROM
                                SELECTED_BATCH_NO TRN
                        LEFT OUTER JOIN
                                SELECTED_A_ECSHIPS ACM
                        ON
                                TRN.BATCH_NO = ACM.BATCH_NO
                            AND TRN.CENTER_ID = ACM.CENTER_ID
                            AND TRN.SHIPPER_ID = ACM.SHIPPER_ID
                )
                ,   SELECTED_PIC AS (
                        SELECT
                                SUM(PIC.PIC_QTY) AS PIC_PIC_QTY
                            ,   SUM(PIC.STOCK_OUT_REG_QTY) AS PIC_STOCK_OUT_REG_QTY
                            ,   SUM(PIC.STOCK_OUT_FIX_QTY) AS PIC_STOCK_OUT_FIX_QTY
                            ,   CASE
                                    WHEN SUM(PIC.PIC_QTY + PIC.STOCK_OUT_FIX_QTY) = 0 THEN 0
                                    ELSE TRUNC((SUM(PIC.PIC_QTY + PIC.STOCK_OUT_FIX_QTY) / SUM(PIC.HIKI_QTY)) * 100)
                                END AS PIC_PIC_PERCENT
                            ,   PIC.BATCH_NO
                        FROM
                                T_PIC PIC
                        INNER JOIN
                                SELECTED_BATCH_NO ECSHIP
                        ON
                                PIC.BATCH_NO = ECSHIP.BATCH_NO
                        WHERE
                                PIC.SHIPPER_ID = :SHIPPER_ID
                            AND PIC.CENTER_ID = :CENTER_ID
                        GROUP BY
                                PIC.BATCH_NO
                )
                ,   SELECTED_ECUNIT_SORT AS (
                        SELECT
                                ECUNIT.BATCH_NO
                            ,   SUM(ECUNIT.SORT_QTY) AS ECUNIT_SORT_QTY
                            ,   SUM(ECUNIT.ALLOC_QTY) AS ALLOC_QTY
                            ,   CASE
                                    WHEN SUM(ECUNIT.SORT_QTY) = 0 THEN 0
                                    ELSE TRUNC((SUM(ECUNIT.SORT_QTY) / SUM(ECUNIT.ALLOC_QTY)) * 100)
                                END AS ECUNIT_SORT_PERCENT
                        FROM
                                T_ECUNIT_SORT ECUNIT
                        INNER JOIN
                                SELECTED_BATCH_NO ECSHIP
                        ON
                                ECUNIT.BATCH_NO = ECSHIP.BATCH_NO
                        WHERE
                                ECUNIT.SHIPPER_ID = :SHIPPER_ID
                            AND ECUNIT.CENTER_ID = :CENTER_ID
                        GROUP BY
                                ECUNIT.BATCH_NO
                )
                ,   SELECTED_GAS AS (
                        SELECT
                                GAS.BATCH_NO
                            ,   COUNT(DISTINCT GAS.GAS_BATCH_NO) AS GAS_BATCH_NO_QTY
                            ,   COUNT(DISTINCT(DECODE(GAS.GAS_DATE, NULL, NULL, GAS.GAS_BATCH_NO))) AS GAS_QTY
                            ,   DECODE(COUNT(DISTINCT(GAS.GAS_BATCH_NO)),0,0,(TRUNC((COUNT(DISTINCT(DECODE(GAS.GAS_DATE, NULL, NULL, GAS.GAS_BATCH_NO))) / COUNT(DISTINCT(GAS.GAS_BATCH_NO))) * 100)))  AS GAS_PERCENT
                            ,   SUM(GAS.GAS_STOCK_OUT_QTY) AS GAS_STOCK_OUT_QTY
                            ,   SUM(GAS.STOCK_OUT_FIX_QTY) AS STOCK_OUT_FIX_QTY
                        FROM
                                T_GAS GAS
                        INNER JOIN
                                SELECTED_BATCH_NO ECSHIP
                        ON
                                GAS.BATCH_NO = ECSHIP.BATCH_NO
                        WHERE
                                GAS.SHIPPER_ID = :SHIPPER_ID
                            AND GAS.CENTER_ID = :CENTER_ID
                        GROUP BY
                                GAS.BATCH_NO
                )
                ,    TARGET_ALL_PACKING_DATA AS (
                        SELECT
                                PACK.BOX_NO
                            ,   PACK.SHIP_INSTRUCT_ID
                            ,   PACK.SHIP_INSTRUCT_SEQ
                            ,   PACK.CENTER_ID
                            ,   PACK.SHIPPER_ID
                            ,   PACK.RESULT_QTY
                            ,   PACK.NOUHIN_PRN_FLAG
                            ,   PACK.DELI_PRN_FLAG
                            ,   PACK.BATCH_NO
                        FROM
                                T_SHIP_PACKING_INFO PACK
                        WHERE
                                PACK.CENTER_ID = :CENTER_ID
                            AND PACK.SHIPPER_ID = :SHIPPER_ID
                        UNION
                        SELECT
                                PACK.BOX_NO
                            ,   PACK.SHIP_INSTRUCT_ID
                            ,   PACK.SHIP_INSTRUCT_SEQ
                            ,   PACK.CENTER_ID
                            ,   PACK.SHIPPER_ID
                            ,   PACK.RESULT_QTY
                            ,   PACK.NOUHIN_PRN_FLAG
                            ,   PACK.DELI_PRN_FLAG
                            ,   PACK.BATCH_NO
                        FROM
                                A_SHIP_PACKING_INFO PACK
                        WHERE
                                PACK.CENTER_ID = :CENTER_ID
                            AND PACK.SHIPPER_ID = :SHIPPER_ID
                )
                ,   SELECTED_SHIP_PACKING AS (
                        SELECT
                                PACK.BATCH_NO
                            ,   SUM(CASE
                                        WHEN PACK.NOUHIN_PRN_FLAG IN (8, 9) AND PACK.DELI_PRN_FLAG <> 0 THEN PACK.RESULT_QTY
                                        ELSE 0
                                    END) AS NOUHIN_PRN_QTY
                            ,   CASE
                                    WHEN SUM(CASE WHEN PACK.NOUHIN_PRN_FLAG IN (8, 9) AND PACK.DELI_PRN_FLAG <> 0 THEN PACK.RESULT_QTY ELSE 0 END) = 0 THEN 0
                                    ELSE TRUNC(
                                            (SUM(CASE
                                                    WHEN PACK.NOUHIN_PRN_FLAG IN (8, 9) AND PACK.DELI_PRN_FLAG <> 0 THEN PACK.RESULT_QTY
                                                    ELSE 0
                                                 END) / MAX(ECSHIP.ALLOC_QTY)) * 100)
                                END AS NOUHIN_PRN_PERCENT
                        FROM
                                ALL_ECSHIPS_DATA ECSHIP
                        LEFT OUTER JOIN
                                TARGET_ALL_PACKING_DATA PACK
                        ON
                                PACK.BATCH_NO = ECSHIP.BATCH_NO
                        WHERE
                                PACK.SHIPPER_ID = :SHIPPER_ID
                            AND PACK.CENTER_ID = :CENTER_ID
                        GROUP BY
                                PACK.BATCH_NO
                )
                SELECT
                        ECSHIP.BATCH_NO
                    ,   ALLOC.BATCH_NAME
                    ,   ECSHIP.SHIP_INSTRUCT_QTY
                    ,   ECSHIP.ITEM_SKU_QTY
                    ,   ECSHIP.SHIP_INSTRUCT_SEQ_QTY
                    ,   ECSHIP.ALLOC_QTY
                    ,   PIC.PIC_PIC_QTY
                    ,   PIC.PIC_PIC_PERCENT
                    ,   PIC.PIC_STOCK_OUT_REG_QTY
                    ,   PIC.PIC_STOCK_OUT_FIX_QTY
                    ,   ECUNIT.ECUNIT_SORT_QTY
                    ,   ECUNIT.ECUNIT_SORT_PERCENT
                    ,   GAS.GAS_BATCH_NO_QTY
                    ,   GAS.GAS_QTY
                    ,   GAS.GAS_PERCENT
                    ,   GAS.GAS_STOCK_OUT_QTY
                    ,   GAS.STOCK_OUT_FIX_QTY
                    ,   PACK.NOUHIN_PRN_QTY
                    ,   PACK.NOUHIN_PRN_PERCENT
                FROM
                        ALL_ECSHIPS_DATA ECSHIP
                LEFT OUTER JOIN
                        SELECTED_PIC PIC
                ON
                        PIC.BATCH_NO = ECSHIP.BATCH_NO
                LEFT OUTER JOIN
                        SELECTED_ECUNIT_SORT ECUNIT
                ON
                        ECUNIT.BATCH_NO = ECSHIP.BATCH_NO
                LEFT OUTER JOIN
                        SELECTED_GAS GAS
                ON
                        GAS.BATCH_NO = ECSHIP.BATCH_NO
                LEFT OUTER JOIN
                        SELECTED_SHIP_PACKING PACK
                ON
                        PACK.BATCH_NO = ECSHIP.BATCH_NO
                LEFT JOIN
                        T_ALLOC_INFO ALLOC
                ON
                        ECSHIP.BATCH_NO = ALLOC.ALLOC_NO
                    AND ALLOC.SHIPPER_ID = :SHIPPER_ID
                    AND ALLOC.CENTER_ID = :CENTER_ID
                ORDER BY
                        ECSHIP.BATCH_NO
            ");

            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<TransferReferenceEcRow>(query.ToString(), parameters).ToList();
        }
        #endregion EC出荷表一覧

        #region ケース一覧
        /// <summary>
        /// Get Case List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<TransferReferenceCaseRow> GetCaseData(TransferReferenceSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                WITH
                    SELECTED_PIC_CASE AS (
                        SELECT
                                PIC.BATCH_NO
                            ,   PIC.CENTER_ID
                            ,   PIC.SHIPPER_ID
                            ,   SUM(PIC.PIC_QTY) AS PIC_QTY
                            ,   CASE
                                    WHEN SUM(PIC.HIKI_QTY) = 0 THEN 0 
                                    ELSE DECODE(SUM(PIC.HIKI_QTY), 0, 0, (TRUNC((SUM(PIC.PIC_QTY + PIC.STOCK_OUT_FIX_QTY) / SUM(PIC.HIKI_QTY)) * 100)))
                                END AS PIC_PERCENT
                            ,   COUNT(DISTINCT (CASE WHEN PICK_KIND = 4 THEN JAN ELSE NULL END)) AS JAN_QTY
                            ,   SUM(PIC.HIKI_QTY) AS HIKI_QTY
                            ,   SUM(PIC.STOCK_OUT_REG_QTY) AS STOCK_OUT_REG_QTY
                            ,   SUM(PIC.STOCK_OUT_FIX_QTY) AS STOCK_OUT_FIX_QTY
                            ,   PIC.PICK_KIND
                        FROM
                                T_PIC PIC
                        WHERE
                                PIC.CENTER_ID = :CENTER_ID
                            AND PIC.SHIPPER_ID = :SHIPPER_ID
                            AND PIC.PICK_KIND IN (3, 4)
                        GROUP BY
                                PIC.BATCH_NO
                            ,   PIC.CENTER_ID
                            ,   PIC.SHIPPER_ID
                            ,   PIC.PICK_KIND
                )
                ,   TARGET_ALL_SHIP_DATA AS (
                        SELECT
                                SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   SHIP_TO_STORE_ID
                            ,   SHIP_KIND
                            ,   BATCH_NO
                        FROM
                                T_SHIPS
                        WHERE
                                SHIP_KIND IN (4, 5)
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                        UNION
                        SELECT
                                SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   SHIP_TO_STORE_ID
                            ,   SHIP_KIND
                            ,   BATCH_NO
                        FROM
                                A_SHIPS
                        WHERE
                                SHIP_KIND IN (4, 5)
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                )
                ,   TARGET_ALL_PACKING_DATA AS (
                        SELECT
                                BOX_NO
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   RESULT_QTY
                            ,   NOUHIN_PRN_FLAG
                            ,   DELI_PRN_FLAG
                            ,   BATCH_NO
                        FROM
                                T_SHIP_PACKING_INFO 
                        WHERE
                                CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                            AND EC_FLAG = 0
                        UNION
                        SELECT
                                BOX_NO
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   RESULT_QTY
                            ,   NOUHIN_PRN_FLAG
                            ,   DELI_PRN_FLAG
                            ,   BATCH_NO
                        FROM
                                A_SHIP_PACKING_INFO 
                        WHERE
                                CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                            AND EC_FLAG = 0
                )
                ,   GROUP_PACKING_CASE_DATA AS (
                        SELECT
                                SHIP.BATCH_NO
                            ,   SHIP.SHIP_INSTRUCT_ID
                            ,   SHIP.CENTER_ID
                            ,   SHIP.SHIPPER_ID
                            ,   MAX(SHIP.SHIP_TO_STORE_ID) AS SHIP_TO_STORE_ID
                            ,   CASE
                                    WHEN MIN(PACK.NOUHIN_PRN_FLAG) IN (8, 9) AND MIN(PACK.DELI_PRN_FLAG) <> 0 THEN 1
                                    ELSE 0
                                END AS NOUHIN_PRN_QTY
                        FROM
                                TARGET_ALL_SHIP_DATA SHIP
                        LEFT OUTER JOIN
                                TARGET_ALL_PACKING_DATA PACK
                        ON
                                SHIP.SHIP_INSTRUCT_ID = PACK.SHIP_INSTRUCT_ID
                            AND SHIP.SHIP_INSTRUCT_SEQ = PACK.SHIP_INSTRUCT_SEQ
                            AND SHIP.CENTER_ID = PACK.CENTER_ID
                            AND SHIP.SHIPPER_ID = PACK.SHIPPER_ID
                        INNER JOIN
                                SELECTED_PIC_CASE PIC
                        ON
                                SHIP.BATCH_NO = PIC.BATCH_NO
                            AND SHIP.CENTER_ID = PIC.CENTER_ID
                            AND SHIP.SHIPPER_ID = PIC.SHIPPER_ID
                        WHERE
                                PIC.PICK_KIND = 3
                        GROUP BY
                                SHIP.BATCH_NO
                            ,   SHIP.SHIP_INSTRUCT_ID
                            ,   SHIP.CENTER_ID
                            ,   SHIP.SHIPPER_ID
                )
                ,   GROUP_PACKING_JAN_DATA AS (
                        SELECT
                                SHIP.BATCH_NO
                            ,   SHIP.SHIP_INSTRUCT_ID
                            ,   SHIP.SHIP_INSTRUCT_SEQ
                            ,   SHIP.CENTER_ID
                            ,   SHIP.SHIPPER_ID
                            ,   MAX(SHIP.SHIP_TO_STORE_ID) AS SHIP_TO_STORE_ID
                            ,   SUM(CASE
                                        WHEN PACK.NOUHIN_PRN_FLAG IN (8, 9) AND PACK.DELI_PRN_FLAG <> 0 THEN PACK.RESULT_QTY
                                        ELSE 0
                                    END) AS NOUHIN_PRN_QTY
                        FROM
                                TARGET_ALL_SHIP_DATA SHIP
                        LEFT OUTER JOIN
                                TARGET_ALL_PACKING_DATA PACK
                        ON
                                SHIP.SHIP_INSTRUCT_ID = PACK.SHIP_INSTRUCT_ID
                            AND SHIP.SHIP_INSTRUCT_SEQ = PACK.SHIP_INSTRUCT_SEQ
                            AND SHIP.CENTER_ID = PACK.CENTER_ID
                            AND SHIP.SHIPPER_ID = PACK.SHIPPER_ID
                        INNER JOIN
                                SELECTED_PIC_CASE PIC
                        ON
                                SHIP.BATCH_NO = PIC.BATCH_NO
                            AND SHIP.CENTER_ID = PIC.CENTER_ID
                            AND SHIP.SHIPPER_ID = PIC.SHIPPER_ID
                        WHERE
                                PIC.PICK_KIND = 4
                        GROUP BY
                                SHIP.BATCH_NO
                            ,   SHIP.SHIP_INSTRUCT_ID
                            ,   SHIP.SHIP_INSTRUCT_SEQ
                            ,   SHIP.CENTER_ID
                            ,   SHIP.SHIPPER_ID
                )
                ,   SELECTED_SHIP_NOUHIN_DATA AS (
                        SELECT
                                BATCH_NO
                            ,   COUNT(DISTINCT(SHIP_TO_STORE_ID)) AS SHIP_TO_STORE_QTY
                            ,   SUM(NVL(NOUHIN_PRN_QTY, 0)) AS NOUHIN_PRN_QTY
                        FROM
                                GROUP_PACKING_CASE_DATA
                        GROUP BY
                                BATCH_NO
                        UNION ALL
                        SELECT
                                BATCH_NO
                            ,   COUNT(DISTINCT(SHIP_TO_STORE_ID)) AS SHIP_TO_STORE_QTY
                            ,   SUM(NVL(NOUHIN_PRN_QTY, 0)) AS NOUHIN_PRN_QTY
                        FROM
                                GROUP_PACKING_JAN_DATA
                        GROUP BY
                                BATCH_NO
                )
                SELECT
                        PIC.BATCH_NO
                    ,   ALLOC.BATCH_NAME
                    ,   NOUHIN.SHIP_TO_STORE_QTY
                    ,   CASE WHEN PIC.PICK_KIND = 3 THEN PIC.HIKI_QTY ELSE 0 END AS INSTRUCT_CASE_QTY
                    ,   PIC.JAN_QTY
                    ,   CASE WHEN PIC.PICK_KIND = 4 THEN PIC.HIKI_QTY ELSE 0 END AS ALLOC_QTY
                    ,   PIC.PIC_QTY
                    ,   PIC.PIC_PERCENT
                    ,   PIC.STOCK_OUT_REG_QTY
                    ,   PIC.STOCK_OUT_FIX_QTY
                    ,   NVL(NOUHIN.NOUHIN_PRN_QTY, 0) + PIC.STOCK_OUT_FIX_QTY AS NOUHIN_PRN_QTY
                    ,   CASE
                            WHEN NVL(NOUHIN.NOUHIN_PRN_QTY, 0) + PIC.STOCK_OUT_FIX_QTY = 0 THEN 0
                            ELSE TRUNC(((NVL(NOUHIN.NOUHIN_PRN_QTY, 0) + PIC.STOCK_OUT_FIX_QTY) / PIC.HIKI_QTY) * 100)
                        END AS NOUHIN_PRN_PERCENT
                FROM
                        SELECTED_PIC_CASE PIC
                LEFT OUTER JOIN
                        SELECTED_SHIP_NOUHIN_DATA NOUHIN
                ON
                        PIC.BATCH_NO = NOUHIN.BATCH_NO
                LEFT OUTER JOIN
                        T_ALLOC_INFO ALLOC
                ON
                        PIC.BATCH_NO = ALLOC.ALLOC_NO
                    AND PIC.SHIPPER_ID = ALLOC.SHIPPER_ID
                    AND PIC.CENTER_ID = ALLOC.CENTER_ID
                ");

                // 条件記述簡略化のため記述
                if (!string.IsNullOrEmpty(condition.BatchName) || (condition.AllocDateFrom != null) || (condition.AllocDateTo != null))
                {
                    query.Append(" WHERE 1 = 1");
                 }

                //バッチ名称
                if (!string.IsNullOrEmpty(condition.BatchName))
                { 
                    query.Append(" AND ALLOC.BATCH_NAME LIKE :BATCH_NAME ");
                    parameters.Add(":BATCH_NAME", "%" + condition.BatchName + "%");
                }

                            // 引当日From
                if (condition.AllocDateFrom != null)
                {
                    query.Append(" AND TRUNC(ALLOC.ALLOC_DATE) >= :ALLOC_DATE_FROM ");
                    parameters.Add(":ALLOC_DATE_FROM", condition.AllocDateFrom);
                }

                // 引当日To
                if (condition.AllocDateTo != null)
                {
                    query.Append(" AND TRUNC(ALLOC.ALLOC_DATE) <= :ALLOC_DATE_TO ");
                    parameters.Add(":ALLOC_DATE_TO", condition.AllocDateTo);

                }

            query.Append(@"
                ORDER BY
                    PIC.BATCH_NO
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);

            // Fill data to memory
            int totalCount = MvcDbContext.Current.Database.Connection.Query<TransferReferenceCaseRow>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            var References = MvcDbContext.Current.Database.Connection.Query<TransferReferenceCaseRow>(query.ToString(), parameters);


            // Fill data to memory
            return new StaticPagedList<TransferReferenceCaseRow>(References, condition.Page, condition.PageSize, totalCount);
        }
        #endregion ケース一覧

        #region ECユニット仕分進捗
        /// <summary>
        /// Get Detail Data
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public EcunitReferenceViewModel GetEcunitReferenceData(string centerId, string batchNo)
        {
            EcunitReferenceViewModel vm = new EcunitReferenceViewModel();

            // 総数
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT TE.BATCH_NO || '　' || MAX(TAI.BATCH_NAME) BATCH_NO
                      ,COUNT(DISTINCT(TE.SHIP_INSTRUCT_ID)) SHIP_INSTRUCT_QTY
                      ,COUNT(DISTINCT(TE.ITEM_SKU_ID)) ITEM_SKU_QTY
                      ,COUNT(TE.SHIP_INSTRUCT_SEQ) SHIP_INSTRUCT_SEQ_QTY
                      ,SUM(TE.ALLOC_QTY) ALLOC_QTY
                  FROM T_ECSHIPS TE
                  LEFT JOIN T_ALLOC_INFO TAI
                    ON TE.BATCH_NO = TAI.ALLOC_NO
                   AND TE.SHIPPER_ID = TAI.SHIPPER_ID
                   AND TE.CENTER_ID = TAI.CENTER_ID
                 WHERE TE.BATCH_NO = :BATCH_NO
                   AND TE.SHIPPER_ID = :SHIPPER_ID
                   AND TE.CENTER_ID = :CENTER_ID
                 GROUP BY TE.BATCH_NO ");
            parameters.Add(":BATCH_NO", batchNo);
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", centerId);
            vm.Head = new EcunitReferenceHead();
            vm.Head = MvcDbContext.Current.Database.Connection.Query<EcunitReferenceHead>(query.ToString(), parameters).FirstOrDefault();

            // EC出荷形態
            parameters = new DynamicParameters();
            query = new StringBuilder(@"
                SELECT MAX(MG.GEN_NAME) EC_SHIP_CLASS_NAME
                      ,COUNT(DISTINCT(TE.SHIP_INSTRUCT_ID)) SHIP_INSTRUCT_QTY
                      ,COUNT(DISTINCT(TE.ITEM_SKU_ID)) ITEM_SKU_QTY
                      ,SUM(TE.ALLOC_QTY) ALLOC_QTY
                      ,SUM(TE.SORT_QTY) SORT_QTY
                      ,DECODE(SUM(TE.ALLOC_QTY),0,0,(TRUNC((SUM(TE.SORT_QTY) / SUM(TE.ALLOC_QTY)) * 100)))  AS PERCENT
                  FROM T_ECUNIT_SORT TE
                  LEFT JOIN M_GENERALS MG
                    ON TE.SHIPPER_ID = MG.SHIPPER_ID
                   AND MG.REGISTER_DIVI_CD = '1'
                   AND MG.CENTER_ID = '@@@'
                   AND MG.GEN_DIV_CD = 'EC_SHIP_CLASS'
                   AND TO_CHAR(TE.EC_SHIP_CLASS) = MG.GEN_CD
                 WHERE TE.BATCH_NO = :BATCH_NO
                   AND TE.EC_SHIP_CLASS = 1
                   AND TE.SHIPPER_ID = :SHIPPER_ID
                   AND TE.CENTER_ID = :CENTER_ID
                 GROUP BY TE.BATCH_NO ");
            parameters.Add(":BATCH_NO", batchNo);
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", centerId);
            vm.SinglePickResults = new DetailRow();
            vm.SinglePickResults = MvcDbContext.Current.Database.Connection.Query<DetailRow>(query.ToString(), parameters).FirstOrDefault();

            // オーダー出荷
            parameters = new DynamicParameters();
            query = new StringBuilder(@"
                SELECT TE.SHIP_INSTRUCT_ID EC_SHIP_CLASS_NAME
                      ,COUNT(DISTINCT(TE.SHIP_INSTRUCT_ID)) SHIP_INSTRUCT_QTY
                      ,COUNT(DISTINCT(TE.ITEM_SKU_ID)) ITEM_SKU_QTY
                      ,SUM(TE.ALLOC_QTY) ALLOC_QTY
                      ,SUM(TE.SORT_QTY) SORT_QTY
                      ,DECODE(SUM(TE.ALLOC_QTY),0,0,(TRUNC((SUM(TE.SORT_QTY) / SUM(TE.ALLOC_QTY)) * 100)))  AS PERCENT
                  FROM T_ECUNIT_SORT TE
                 WHERE TE.BATCH_NO = :BATCH_NO
                   AND TE.EC_SHIP_CLASS = 2
                   AND TE.SHIPPER_ID = :SHIPPER_ID
                   AND TE.CENTER_ID = :CENTER_ID
                 GROUP BY TE.SHIP_INSTRUCT_ID ");
            parameters.Add(":BATCH_NO", batchNo);
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", centerId);
            vm.OrderResults = new OrderResult();
            vm.OrderResults.OrderResults = MvcDbContext.Current.Database.Connection.Query<DetailRow>(query.ToString(), parameters).ToList();

            // GASバッチNo
            parameters = new DynamicParameters();
            query = new StringBuilder(@"
                SELECT TE.GAS_BATCH_NO EC_SHIP_CLASS_NAME
                      ,COUNT(DISTINCT(TE.SHIP_INSTRUCT_ID)) SHIP_INSTRUCT_QTY
                      ,COUNT(DISTINCT(TE.ITEM_SKU_ID)) ITEM_SKU_QTY
                      ,SUM(TE.ALLOC_QTY) ALLOC_QTY
                      ,SUM(TE.SORT_QTY) SORT_QTY
                      ,DECODE(SUM(TE.ALLOC_QTY),0,0,(TRUNC((SUM(TE.SORT_QTY) / SUM(TE.ALLOC_QTY)) * 100)))  AS PERCENT
                  FROM T_ECUNIT_SORT TE
                 WHERE TE.BATCH_NO = :BATCH_NO
                   AND TE.EC_SHIP_CLASS = 3
                   AND TE.SHIPPER_ID = :SHIPPER_ID
                   AND TE.CENTER_ID = :CENTER_ID
                 GROUP BY TE.GAS_BATCH_NO ");
            parameters.Add(":BATCH_NO", batchNo);
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", centerId);
            vm.GasResults = new GasResult();
            vm.GasResults.GasResults = MvcDbContext.Current.Database.Connection.Query<DetailRow>(query.ToString(), parameters).ToList();
            return vm;
        }
        #endregion

        #region レーン仕分詳細
        /// <summary>
        /// Get Detail Data
        /// </summary>
        /// <returns></returns>
        public LaneSortDetailViewModel GetLaneSortDetailData(Common.ShipKinds shipKind, string centerId, DateTime? shipPlanDate, string batchNo)
        {
            LaneSortDetailViewModel vm = new LaneSortDetailViewModel();

            // 総数
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            if (shipKind == Common.ShipKinds.Dc)
            {
                query = new StringBuilder(@"
                    SELECT
                            TS.BATCH_NO || '　' || MAX(TAI.BATCH_NAME) BATCH_NO
                        ,   COUNT(DISTINCT(TS.SHIP_TO_STORE_ID)) SHIP_TO_STORE_QTY
                        ,   COUNT(DISTINCT(TS.ITEM_SKU_ID)) ITEM_SKU_QTY
                        ,   COUNT(TS.SHIP_INSTRUCT_SEQ) SHIP_INSTRUCT_SEQ_QTY
                        ,   SUM(TS.ALLOC_QTY) ALLOC_QTY
                    FROM
                            T_STORE_SORT TS
                    INNER JOIN
                            T_ALLOC_INFO TAI
                    ON
                            TS.BATCH_NO = TAI.ALLOC_NO
                        AND TS.CENTER_ID = TAI.CENTER_ID
                        AND TS.SHIPPER_ID = TAI.SHIPPER_ID
                    WHERE
                            TS.BATCH_NO = :BATCH_NO
                        AND TS.SHIPPER_ID = :SHIPPER_ID
                        AND TS.CENTER_ID = :CENTER_ID
                        AND TAI.SHIP_KIND = 2
                    GROUP BY
                            TS.BATCH_NO
                ");
                parameters.Add(":BATCH_NO", batchNo);
            }
            try
            {
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":CENTER_ID", centerId);
                vm.Head = new LaneSortDetailHead();
                vm.Head = MvcDbContext.Current.Database.Connection.Query<LaneSortDetailHead>(query.ToString(), parameters).FirstOrDefault();
                vm.Head.ShipKind = shipKind;

            }
            catch (Exception ex)
            {

                throw ex;
            }

            // オーダー出荷
            parameters = new DynamicParameters();
            query = new StringBuilder(@"
                SELECT
                        TLS.BRAND_ID AS BRAND_ID
                    ,   MAX(MB.BRAND_SHORT_NAME) AS BRAND_SHORT_NAME
                    ,   TLS.LANE_NO AS LANE_NO                                       --レーンNo
                    ,   COUNT(DISTINCT(TSS.SHIP_TO_STORE_ID)) AS SHIP_TO_STORE_QTY     --出荷先数 
                    ,   MAX(SUB_ITEM_SKU_ID) AS ITEM_SKU_QTY                           --SKU数
                    ,   MAX(SUB.SUB_ALLOC_QTY) AS ALLOC_QTY
                    ,   MAX(SUB_SORT_QTY) AS SORT_QTY                                  --完了数
                    ,   CASE MAX(SUB.SUB_ALLOC_QTY)
                            WHEN 0 THEN 0 
                            ELSE TRUNC(( MAX(SUB_SORT_QTY) / MAX(SUB.SUB_ALLOC_QTY)) * 100) 
                        END AS PERCENT
                FROM
                        T_STORE_SORT TSS
                INNER JOIN
                        T_LANE_SORT TLS
                ON
                        TLS.ITEM_SKU_ID = TSS.ITEM_SKU_ID	
                    AND TLS.LANE_NO	= TSS.LANE_NO
                    AND TLS.BRAND_ID = TSS.BRAND_ID
                LEFT JOIN
                        M_BRANDS MB
                ON
                        MB.SHIPPER_ID = TLS.SHIPPER_ID
                    AND MB.BRAND_ID = TLS.BRAND_ID
                LEFT JOIN (
                    SELECT
                            MAX(SUB_TLS.BRAND_ID) BRAND_ID
                        ,   SUB_TLS.LANE_NO
                        ,   COUNT(DISTINCT(SUB_TLS.ITEM_SKU_ID))SUB_ITEM_SKU_ID	
                        ,   SUM(SUB_TLS.ALLOC_QTY) SUB_ALLOC_QTY
                        ,   SUM(SUB_TLS.SORT_QTY) SUB_SORT_QTY
                    FROM
                            T_LANE_SORT SUB_TLS
                    WHERE
                            SUB_TLS.TCDC_CLASS = :TCDC_CLASS
                        AND SUB_TLS.BATCH_NO = :BATCH_NO
                        AND SUB_TLS.SHIPPER_ID = :SHIPPER_ID
                        AND SUB_TLS.CENTER_ID = :CENTER_ID
                    GROUP BY
                            SUB_TLS.LANE_NO
                    ORDER BY
                            SUB_TLS.LANE_NO
                ) SUB
                ON
                        SUB.LANE_NO = TLS.LANE_NO
                WHERE
                        TSS.TCDC_CLASS = :TCDC_CLASS
                    AND TSS.BATCH_NO = :BATCH_NO
                    AND TSS.SHIPPER_ID = :SHIPPER_ID
                    AND TSS.CENTER_ID = :CENTER_ID
                GROUP BY
                        TLS.BRAND_ID
                    ,   TLS.LANE_NO
                ORDER BY
                        TLS.BRAND_ID
                    ,   TLS.LANE_NO
            ");
            try
            {
                //if (shipKind == ShipKinds.Tc)
                //{
                //    parameters.Add(":TCDC_CLASS", 1);
                //    parameters.Add(":SHIP_PLAN_DATE", shipPlanDate);
                //}
                if (shipKind == Common.ShipKinds.Dc)
                {
                    parameters.Add(":TCDC_CLASS", 2);
                    parameters.Add(":BATCH_NO", batchNo);
                }
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":CENTER_ID", centerId);
                vm.LaneResults = new LaneResult();
                vm.LaneResults.LaneResults = MvcDbContext.Current.Database.Connection.Query<LaneSortDetailRow>(query.ToString(), parameters).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return vm;
        }
        #endregion

        #region 店別仕分、摘取詳細
        /// <summary>
        /// Get Detail Data
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public StoreSortOrderPicDetailViewModel GetStoreSortOrderPicDetailData(Common.ShipKinds shipKind, string centerId, DateTime? shipPlanDate, string batchNo)
        {
            StoreSortOrderPicDetailViewModel vm = new StoreSortOrderPicDetailViewModel();

            // 総数
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            if (shipKind == Common.ShipKinds.Dc)
            {
                query = new StringBuilder(@"
                    SELECT
                            TS.BATCH_NO || '　' || MAX(TAI.BATCH_NAME) BATCH_NO
                        ,   COUNT(DISTINCT(TS.SHIP_TO_STORE_ID)) SHIP_TO_STORE_QTY
                        ,   COUNT(DISTINCT(TS.ITEM_SKU_ID)) ITEM_SKU_QTY
                        ,   COUNT(TS.SHIP_INSTRUCT_SEQ) SHIP_INSTRUCT_SEQ_QTY
                        ,   SUM(TS.ALLOC_QTY) ALLOC_QTY
                    FROM
                            T_STORE_SORT TS
                    INNER JOIN
                            T_ALLOC_INFO TAI
                    ON
                            TS.BATCH_NO = TAI.ALLOC_NO
                        AND TS.CENTER_ID = TAI.CENTER_ID
                        AND TS.SHIPPER_ID = TAI.SHIPPER_ID
                    WHERE
                            TS.BATCH_NO = :BATCH_NO
                        AND TS.SHIPPER_ID = :SHIPPER_ID
                        AND TS.CENTER_ID = :CENTER_ID
                        AND TAI.SHIP_KIND = 2
                    GROUP BY
                            TS.BATCH_NO 
                 ");
                parameters.Add(":BATCH_NO", batchNo);
            }
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", centerId);
            vm.Head = new StoreSortOrderPicDetailHead();
            vm.Head = MvcDbContext.Current.Database.Connection.Query<StoreSortOrderPicDetailHead>(query.ToString(), parameters).FirstOrDefault();
            vm.Head.ShipKind = shipKind;

            // 店別仕分用
            parameters = new DynamicParameters();
            query = new StringBuilder(@"
                 SELECT
                        TSS.BRAND_ID AS BRAND_ID
                    ,   MAX(MB.BRAND_SHORT_NAME) AS BRAND_SHORT_NAME
                    ,   TSS.LANE_NO AS LANE_NO
                    ,   COUNT(DISTINCT(TSS.SHIP_TO_STORE_ID)) AS SHIP_TO_STORE_QTY
                    ,   COUNT(DISTINCT(TSS.ITEM_SKU_ID) ) AS ITEM_SKU_QTY
                    ,   SUM(TSS.ALLOC_QTY) AS ALLOC_QTY
                    ,   SUM(TSS.SORT_QTY) AS SORT_QTY
                    ,   CASE
                            WHEN SUM(TSS.ALLOC_QTY) = 0 OR MAX(TSS.SORT_STATUS) = 0 THEN 0
                            ELSE TRUNC(((SUM(TSS.SORT_QTY) +  SUM(TSS.STOCK_OUT_FIX_QTY) ) / SUM(TSS.ALLOC_QTY) ) * 100)
                        END AS PERCENT
                    ,   CASE
                            WHEN MAX(TSS.SORT_STATUS) <> 0 THEN
                                SUM(TSS.STOCK_OUT_REG_QTY)
                            ELSE 0
                        END AS STOCK_OUT_REG_QTY
                    ,   CASE
                            WHEN MAX(TSS.SORT_STATUS) <> 0 THEN
                                SUM(TSS.STOCK_OUT_FIX_QTY)
                            ELSE 0
                        END  AS STOCK_OUT_FIX_QTY
                 FROM
                        T_STORE_SORT TSS
                 LEFT JOIN
                        M_BRANDS MB
                 ON
                        MB.SHIPPER_ID = TSS.SHIPPER_ID
                    AND MB.BRAND_ID = TSS.BRAND_ID
                 WHERE
                        TSS.TCDC_CLASS = :TCDC_CLASS
                    AND TSS.SHIPPER_ID = :SHIPPER_ID
                    AND TSS.CENTER_ID = :CENTER_ID
                    AND TSS.BATCH_NO = :BATCH_NO
                 GROUP BY 
                        TSS.BRAND_ID
                    ,   TSS.LANE_NO
                 ORDER BY 
                        TSS.BRAND_ID
                    ,   TSS.LANE_NO");
            try
            {
                parameters.Add(":TCDC_CLASS", 2);
                parameters.Add(":BATCH_NO", batchNo);
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":CENTER_ID", centerId);
                vm.StoreSortResults = new StoreSortResult();
                vm.StoreSortResults.StoreSortResults = MvcDbContext.Current.Database.Connection.Query<StoreSortOrderPicDetailRow>(query.ToString(), parameters).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            // 摘取用
            parameters = new DynamicParameters();
            query = new StringBuilder(@"
                SELECT
                        '摘取進捗' AS LANE_NO
                    ,   MAX(SUMT.SHIP_TO_STORE_ID_QTY) AS SHIP_TO_STORE_QTY
                    ,   COUNT(DISTINCT(SUMT.ITEM_SKU_ID) ) AS ITEM_SKU_QTY
                    ,   SUM(SUMT.ALLOC_QTY) AS ALLOC_QTY
                    ,   SUM(SUMT.ORDER_PIC_QTY) AS SORT_QTY
                    ,   CASE
                            WHEN MAX(SUMT.ORDER_PIC_STATUS) <> 0 THEN
                                DECODE(SUM(SUMT.ALLOC_QTY),0,0,(TRUNC(((SUM(ORDER_PIC_QTY) +SUM(SUMT.STOCK_OUT_FIX_QTY) ) / SUM(SUMT.ALLOC_QTY) ) * 100)))
                            ELSE 0
                        END AS PERCENT
                    ,   CASE
                            WHEN MAX(SUMT.ORDER_PIC_STATUS) <> 0 THEN
                                SUM(SUMT.STOCK_OUT_REG_QTY)
                            ELSE 0
                        END AS STOCK_OUT_REG_QTY
                    ,   CASE
                            WHEN MAX(SUMT.ORDER_PIC_STATUS) <> 0 THEN
                                SUM(SUMT.STOCK_OUT_FIX_QTY)
                            ELSE 0
                        END AS STOCK_OUT_FIX_QTY
                FROM (
                    SELECT
                            1 AS KEY
                        ,   TSS.ITEM_SKU_ID
                        ,   TSS.ALLOC_QTY
                        ,   TSS.ORDER_PIC_QTY
                        ,   TSS.ORDER_PIC_STATUS
                        ,   DECODE(TSS.ORDER_PIC_STATUS, 3, 1, 0) AS SORT_STATUS
                        ,   TSS.STOCK_OUT_REG_QTY
                        ,   TSS.STOCK_OUT_FIX_QTY
                        ,   TS_SUM.SHIP_TO_STORE_ID_QTY
                    FROM
                            T_STORE_SORT TSS
                    LEFT JOIN (
                            SELECT
                                    TS.BATCH_NO
                                ,   COUNT(DISTINCT(TS.SHIP_TO_STORE_ID) ) AS SHIP_TO_STORE_ID_QTY
                            FROM
                                    T_STORE_SORT TS
                            WHERE
                                    TS.SHIPPER_ID = :SHIPPER_ID
                                AND TS.CENTER_ID = :CENTER_ID
                                AND TS.TCDC_CLASS = :TCDC_CLASS
                                AND TS.BATCH_NO = :BATCH_NO
                            GROUP BY
                                    TS.BATCH_NO
                        ) TS_SUM
                        ON
                                TS_SUM.BATCH_NO = TSS.BATCH_NO
                        WHERE
                                TSS.TCDC_CLASS = :TCDC_CLASS
                            AND TSS.BATCH_NO = :BATCH_NO
                            AND TSS.CENTER_ID = :CENTER_ID
                            AND TSS.SHIPPER_ID = :SHIPPER_ID
                    ) SUMT
                    GROUP BY
                            SUMT.KEY");
            try
            {
                parameters.Add(":TCDC_CLASS", 2);
                parameters.Add(":BATCH_NO", batchNo);
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":CENTER_ID", centerId);
                vm.OrderPicResults = new OrderPicResult();
                vm.OrderPicResults.OrderPicResults = MvcDbContext.Current.Database.Connection.Query<StoreSortOrderPicDetailRow>(query.ToString(), parameters).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return vm;
        }
        #endregion

        #region ピック詳細
        /// <summary>
        /// Get Detail Data 
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public PickSortDetailViewModel GetPickSortDetailData(Common.ShipKinds shipKind, string centerId, DateTime? shipPlanDate, string batchNo, int Page, int PageSize)
        {
            PickSortDetailViewModel vm = new PickSortDetailViewModel();
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            if (shipKind == Common.ShipKinds.Dc)
            {
                // ピック詳細バッチ進捗 ヘッダ
                query = new StringBuilder(@"
                    SELECT
                            NVL(MAX(SUB_TSS.BATCH_NO),MAX(T_PIC.BATCH_NO))|| '　' || MAX(TAI.BATCH_NAME) AS BATCH_NO
                        ,   NVL(MAX(SUB_TSS.SHIP_TO_STORE_ID),0) AS  SHIP_TO_STORE_QTY   --総出荷先数
                        ,   NVL(COUNT(DISTINCT(T_PIC.ITEM_SKU_ID)),0) AS ITEM_SKU_QTY    --総SKU数
                        ,   NVL(SUM(T_PIC.HIKI_QTY),0) AS HIKI_QTY                       --総引当数
                        ,   NVL(MAX(SUB_TSS.BATCH_NO),MAX(T_PIC.BATCH_NO)) AS BATCH_NO_HID
                        ,   SUB_TSS.BATCH_NO AS BATCH_NO
                        ,   MAX(TAI.BATCH_NAME) AS BATCH_NAME
                        ,   TAI.PICK_KIND AS PIC_KIND
                    FROM (
                        SELECT 
                                TSS.BATCH_NO
                            ,   TSS.CENTER_ID
                            ,   TSS.SHIPPER_ID
                            ,   COUNT(DISTINCT(TSS.SHIP_TO_STORE_ID)) AS SHIP_TO_STORE_ID
                        FROM
                                T_STORE_SORT TSS
                        WHERE
                                TSS.BATCH_NO   = :BATCH_NO
                            AND TSS.SHIPPER_ID = :SHIPPER_ID
                            AND TSS.CENTER_ID  = :CENTER_ID
                        GROUP BY 
                                TSS.BATCH_NO
                            ,   TSS.CENTER_ID
                            ,   TSS.SHIPPER_ID
                         ) SUB_TSS
                    LEFT JOIN
                            T_ALLOC_INFO TAI
                    ON
                            SUB_TSS.BATCH_NO   = TAI.ALLOC_NO
                        AND SUB_TSS.SHIPPER_ID = TAI.SHIPPER_ID
                        AND SUB_TSS.CENTER_ID  = TAI.CENTER_ID
                    LEFT JOIN
                            T_PIC T_PIC
                    ON
                            T_PIC.BATCH_NO     = SUB_TSS.BATCH_NO
                        AND T_PIC.SHIPPER_ID   = SUB_TSS.SHIPPER_ID
                        AND T_PIC.CENTER_ID    = SUB_TSS.CENTER_ID
                    WHERE
                            SUB_TSS.BATCH_NO   = :BATCH_NO
                        AND SUB_TSS.SHIPPER_ID = :SHIPPER_ID
                        AND SUB_TSS.CENTER_ID  = :CENTER_ID 
                    GROUP BY
                            SUB_TSS.BATCH_NO
                        ,   TAI.PICK_KIND
                ");
            }
            parameters.Add(":BATCH_NO", batchNo);
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", centerId);
            vm.Head = new PickSortDetailHead();
            vm.Head = MvcDbContext.Current.Database.Connection.Query<PickSortDetailHead>(query.ToString(), parameters).FirstOrDefault();
            vm.Head.ShipKind = shipKind;
            try
            {
                //ピック詳細バッチ進捗 明細
                parameters = new DynamicParameters();
                if (vm.Head.PicKind == 5)
                {
                    query = new StringBuilder(@"
                        SELECT
                                TOP.ITEM_SKU_ID                        AS ITEM_SKU_ID
                            ,   MAX(TOP.ITEM_ID)                       AS ITEM_ID
                            ,   MAX(TOP.ITEM_NAME)                     AS ITEM_NAME
                            ,   MAX(TOP.ITEM_COLOR_ID)                 AS ITEM_COLOR_ID
                            ,   MAX(M_COLORS.ITEM_COLOR_NAME)            AS ITEM_COLOR_NAME
                            ,   MAX(TOP.ITEM_SIZE_ID)                  AS ITEM_SIZE_ID
                            ,   MAX(MIS.ITEM_SIZE_NAME)                  AS ITEM_SIZE_NAME
                            ,   MAX(TOP.JAN)                           AS JAN
                            ,   NVL(MAX(SUB2.SHIP_TO_STORE_ID),0)        AS SHIP_TO_STORE_QTY
                            ,   NVL(SUM(TOP.HIKI_QTY),0)               AS HIKI_QTY
                            ,   NVL(SUM(TOP.PIC_QTY),0)                AS PIC_QTY
                            ,   NVL(TRUNC((SUM(TOP.PIC_QTY) + SUM(TOP.STOCK_OUT_FIX_QTY)) /  SUM(TOP.HIKI_QTY) * 100),0) AS PERCENT
                            ,   NVL(SUM(TOP.STOCK_OUT_REG_QTY),0)      AS STOCK_OUT_REG_QTY
                            ,   NVL(SUM(TOP.STOCK_OUT_FIX_QTY),0)      AS STOCK_OUT_FIX_QTY
                        FROM
                                T_ORDER_PIC TOP
                        LEFT JOIN
                                M_COLORS M_COLORS
                        ON
                                M_COLORS.ITEM_COLOR_ID = TOP.ITEM_COLOR_ID
                            AND M_COLORS.SHIPPER_ID = TOP.SHIPPER_ID
                        LEFT JOIN
                                M_SIZES M_SIZES
                        ON
                                M_SIZES.ITEM_SIZE_ID = TOP.ITEM_SIZE_ID
                            AND M_SIZES.SHIPPER_ID = TOP.SHIPPER_ID
                        LEFT JOIN
                                M_ITEM_SKU MIS
                        ON
                                MIS.SHIPPER_ID = TOP.SHIPPER_ID
                            AND MIS.ITEM_SKU_ID = TOP.ITEM_SKU_ID
                        LEFT JOIN (
                            SELECT
                                    SUB.ITEM_SKU_ID
                                ,   SUB.BATCH_NO
                                ,   SUB.CENTER_ID
                                ,   SUB.SHIPPER_ID
                                ,   COUNT(DISTINCT(SUB.SHIP_TO_STORE_ID) ) AS SHIP_TO_STORE_ID
                            FROM 
                                    T_STORE_SORT SUB
                            WHERE
                                    SUB.BATCH_NO   = :BATCH_NO
                                AND SUB.SHIPPER_ID = :SHIPPER_ID
                                AND SUB.CENTER_ID  = :CENTER_ID
                            GROUP BY
                                    SUB.ITEM_SKU_ID 
                                ,   SUB.BATCH_NO
                                ,   SUB.CENTER_ID
                                ,   SUB.SHIPPER_ID
                        ) SUB2
                    ON
                            SUB2.BATCH_NO    = TOP.BATCH_NO
                        AND SUB2.ITEM_SKU_ID = TOP.ITEM_SKU_ID
                        AND SUB2.CENTER_ID   = TOP.CENTER_ID
                        AND SUB2.SHIPPER_ID  = TOP.SHIPPER_ID
                    WHERE
                            TOP.BATCH_NO = :BATCH_NO
                        AND TOP.SHIPPER_ID = :SHIPPER_ID
                        AND TOP.CENTER_ID = :CENTER_ID
                    GROUP BY
                            TOP.ITEM_SKU_ID 
                        ,   TOP.BATCH_NO
                        ,   TOP.CENTER_ID
                        ,   TOP.SHIPPER_ID
                    ORDER BY
                            TOP.ITEM_SKU_ID
                    ");
                }
                else{
                    query = new StringBuilder(@"
                        SELECT
                                T_PIC.ITEM_SKU_ID                        AS ITEM_SKU_ID
                            ,   MAX(T_PIC.ITEM_ID)                       AS ITEM_ID
                            ,   MAX(T_PIC.ITEM_NAME)                     AS ITEM_NAME
                            ,   MAX(T_PIC.ITEM_COLOR_ID)                 AS ITEM_COLOR_ID
                            ,   MAX(M_COLORS.ITEM_COLOR_NAME)            AS ITEM_COLOR_NAME
                            ,   MAX(T_PIC.ITEM_SIZE_ID)                  AS ITEM_SIZE_ID
                            ,   MAX(MIS.ITEM_SIZE_NAME)                  AS ITEM_SIZE_NAME
                            ,   MAX(T_PIC.JAN)                           AS JAN
                            ,   NVL(MAX(SUB2.SHIP_TO_STORE_ID),0)        AS SHIP_TO_STORE_QTY
                            ,   NVL(SUM(T_PIC.HIKI_QTY),0)               AS HIKI_QTY
                            ,   NVL(SUM(T_PIC.PIC_QTY),0)                AS PIC_QTY
                            ,   NVL(TRUNC((SUM(T_PIC.PIC_QTY) + SUM(T_PIC.STOCK_OUT_FIX_QTY)) /  SUM(T_PIC.HIKI_QTY) * 100),0) AS PERCENT
                            ,   NVL(SUM(T_PIC.STOCK_OUT_REG_QTY),0)      AS STOCK_OUT_REG_QTY
                            ,   NVL(SUM(T_PIC.STOCK_OUT_FIX_QTY),0)      AS STOCK_OUT_FIX_QTY
                        FROM
                                T_PIC T_PIC
                        LEFT JOIN
                                M_COLORS M_COLORS
                        ON
                                M_COLORS.ITEM_COLOR_ID = T_PIC.ITEM_COLOR_ID
                            AND M_COLORS.SHIPPER_ID = T_PIC.SHIPPER_ID
                        LEFT JOIN
                                M_SIZES M_SIZES
                        ON
                                M_SIZES.ITEM_SIZE_ID = T_PIC.ITEM_SIZE_ID
                            AND M_SIZES.SHIPPER_ID = T_PIC.SHIPPER_ID
                        LEFT JOIN
                                M_ITEM_SKU MIS
                        ON
                                MIS.SHIPPER_ID = T_PIC.SHIPPER_ID
                            AND MIS.ITEM_SKU_ID = T_PIC.ITEM_SKU_ID
                        LEFT JOIN (
                            SELECT
                                    SUB.ITEM_SKU_ID
                                ,   SUB.BATCH_NO
                                ,   SUB.CENTER_ID
                                ,   SUB.SHIPPER_ID
                                ,   COUNT(DISTINCT(SUB.SHIP_TO_STORE_ID) ) AS SHIP_TO_STORE_ID
                            FROM 
                                    T_STORE_SORT SUB
                            WHERE
                                    SUB.BATCH_NO   = :BATCH_NO
                                AND SUB.SHIPPER_ID = :SHIPPER_ID
                                AND SUB.CENTER_ID  = :CENTER_ID
                            GROUP BY
                                    SUB.ITEM_SKU_ID 
                                ,   SUB.BATCH_NO
                                ,   SUB.CENTER_ID
                                ,   SUB.SHIPPER_ID
                        ) SUB2
                    ON
                            SUB2.BATCH_NO    = T_PIC.BATCH_NO
                        AND SUB2.ITEM_SKU_ID = T_PIC.ITEM_SKU_ID
                        AND SUB2.CENTER_ID   = T_PIC.CENTER_ID
                        AND SUB2.SHIPPER_ID  = T_PIC.SHIPPER_ID
                    WHERE
                            T_PIC.BATCH_NO = :BATCH_NO
                        AND T_PIC.SHIPPER_ID = :SHIPPER_ID
                        AND T_PIC.CENTER_ID = :CENTER_ID
                    GROUP BY
                            T_PIC.ITEM_SKU_ID 
                        ,   T_PIC.BATCH_NO
                        ,   T_PIC.CENTER_ID
                        ,   T_PIC.SHIPPER_ID
                    ORDER BY
                            T_PIC.ITEM_SKU_ID
                    ");
                }
                
                parameters.Add(":BATCH_NO", batchNo);
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":CENTER_ID", centerId);

                int totalCount = MvcDbContext.Current.Database.Connection.Query<TransferReferenceDcRow>(query.ToString(), parameters).Count();

                query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
                parameters.Add(":PAGE_SIZE", PageSize);

                // Choose data corresponding on each page
                parameters.AddDynamicParams(new { OFFSET = (Page - 1) * PageSize });

                var References = MvcDbContext.Current.Database.Connection.Query<PickSortDetailRow>(query.ToString(), parameters);

                vm.PickResults = new PickResult();
                vm.PickResults.PickResults = new StaticPagedList<PickSortDetailRow>(References, Page, PageSize, totalCount);

                return vm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 店別仕分状況 
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public PickSortDetailStoreViewModel GetPickSortDetailStoreData(string centerId, string itemSkuId, string batchNo)
        {
            PickSortDetailStoreViewModel vm = new PickSortDetailStoreViewModel();
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();     
            query = new StringBuilder(@"
                SELECT
                        :BATCH_NO                   AS BATCH_NO
                     ,  :ITEM_SKU_ID                AS ITEM_SKU_ID
                     ,   MIS.ITEM_ID                AS ITEM_ID
                     ,   MIS.ITEM_NAME              AS ITEM_NAME
                     ,   MIS.ITEM_COLOR_ID          AS ITEM_COLOR_ID
                     ,   M_COLORS.ITEM_COLOR_NAME   AS ITEM_COLOR_NAME
                     ,   MIS.ITEM_SIZE_ID           AS ITEM_SIZE_ID
                     ,   MIS.ITEM_SIZE_NAME         AS ITEM_SIZE_NAME
                     ,   MIS.JAN                    AS JAN 
                FROM
                        M_ITEM_SKU MIS
                LEFT JOIN
                        M_COLORS M_COLORS
                ON
                        M_COLORS.ITEM_COLOR_ID = MIS.ITEM_COLOR_ID
                    AND M_COLORS.SHIPPER_ID = MIS.SHIPPER_ID
                LEFT JOIN
                        M_SIZES M_SIZES
                ON
                        M_SIZES.ITEM_SIZE_ID = MIS.ITEM_SIZE_ID
                    AND M_SIZES.SHIPPER_ID = MIS.SHIPPER_ID
                WHERE
                        MIS.SHIPPER_ID = :SHIPPER_ID
                    AND MIS.ITEM_SKU_ID  = :ITEM_SKU_ID
                    ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":ITEM_SKU_ID", itemSkuId);
            parameters.Add(":BATCH_NO", batchNo);
            vm.Head = new PickSortDetailStoreHead();
            vm.Head = MvcDbContext.Current.Database.Connection.Query<PickSortDetailStoreHead>(query.ToString(), parameters).FirstOrDefault();

            try
            {
                //ピック詳細バッチ進捗 明細
                parameters = new DynamicParameters();
                query = new StringBuilder(@"
                    SELECT
                            TSS.LANE_NO
                        ,   TSS.FRONTAGE_NO 
                        ,   TSS.SHIP_TO_STORE_ID
                        ,   MS.STORE_NAME1 AS STORE_NAME
                        ,   TSS.ALLOC_QTY
                        ,   MG.GEN_NAME AS SORT_STATUS
                        ,   TSS.SORT_QTY
                        ,   TSS.SORT_USER_ID
                        ,   MU.USER_NAME AS SORT_USER_NAME
                        ,   TO_CHAR(TSS.SORT_DATE,'YYYY/MM/DD HH24:MI:SS') SORT_DATE
                    FROM
                            T_STORE_SORT TSS
                    LEFT OUTER JOIN
                            M_STORES MS
                    ON
                            TSS.SHIPPER_ID = MS.SHIPPER_ID
                        AND TSS.SHIP_TO_STORE_ID = MS.STORE_ID
                    LEFT OUTER JOIN
                            M_USERS MU
                    ON
                            TSS.SHIPPER_ID = MU.SHIPPER_ID
                        AND TSS.SORT_USER_ID = MU.USER_ID
                    LEFT JOIN 
                            M_GENERALS MG
                    ON 
                            MG.SHIPPER_ID = TSS.SHIPPER_ID
                        AND MG.REGISTER_DIVI_CD = '1'
                        AND MG.CENTER_ID = '@@@'
                        AND MG.GEN_DIV_CD = 'SORT_STATUS'
                        AND MG.GEN_CD = TSS.SORT_STATUS
                    WHERE
                            TSS.BATCH_NO   = :BATCH_NO
                        AND TSS.ITEM_SKU_ID   = :ITEM_SKU_ID
                        AND TSS.SHIPPER_ID = :SHIPPER_ID
                        AND TSS.CENTER_ID  = :CENTER_ID
                    ORDER BY
                            TSS.LANE_NO
                        ,   TSS.FRONTAGE_NO
                       ");
                parameters.Add(":BATCH_NO", batchNo);
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":CENTER_ID", centerId);
                parameters.Add(":ITEM_SKU_ID", itemSkuId);
                vm.PickResults = new PickStoreResult();
                vm.PickResults.PickStoreResults = MvcDbContext.Current.Database.Connection.Query<PickSortDetailStoreRow>(query.ToString(), parameters).ToList();

                return vm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Detail Data 
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public PickResultViewModel GetPickResultData(Common.ShipKinds shipKind, string centerId, DateTime? shipPlanDate, string batchNo, int Page ,int PageSize)
        {
            PickResultViewModel vm = new PickResultViewModel();
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
                // ピック実績 ヘッダ
                query = new StringBuilder(@"
                    SELECT
                            :BATCH_NO AS BATCH_NO
                        ,   TAI.BATCH_NAME AS BATCH_NAME
                        ,   TAI.PICK_KIND AS PIC_KIND
                    FROM 
                            T_PIC TP
                    LEFT JOIN
                            T_ALLOC_INFO TAI
                    ON
                            TAI.ALLOC_NO = TP.BATCH_NO
                        AND TAI.SHIPPER_ID = TP.SHIPPER_ID
                        AND TAI.CENTER_ID = TP.CENTER_ID
                    WHERE
                            TP.BATCH_NO   = :BATCH_NO
                        AND TP.SHIPPER_ID = :SHIPPER_ID
                        AND TP.CENTER_ID  = :CENTER_ID 
                ");
            if (shipKind == Common.ShipKinds.Dc)
            {
                query.AppendLine(@"
                        AND TP.PICK_KIND IN (1, 5)
                       ");
            }
            else if (shipKind == Common.ShipKinds.Case)
            {
                query.AppendLine(@"
                        AND TP.PICK_KIND IN (3,4)
                       ");
            }

            parameters.Add(":BATCH_NO", batchNo);
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", centerId);
            vm.Head = new PickResultHead();
            vm.Head = MvcDbContext.Current.Database.Connection.Query<PickResultHead>(query.ToString(), parameters).FirstOrDefault();
            vm.Head.ShipKind = shipKind;
            try
            {
                //ピック実績 明細
                parameters = new DynamicParameters();
                if (vm.Head.PicKind == 5)
                {
                    query = new StringBuilder(@"
                            SELECT
                                    TOP.SHIP_TO_STORE_ID      AS SHIP_TO_STORE_ID
                                ,   VS.SHIP_TO_STORE_NAME1    AS SHIP_TO_STORE_NAME
                                ,   TOP.LOCATION_CD           AS LOCATION_CD
                                ,   TOP.ITEM_SKU_ID           AS ITEM_SKU_ID
                                ,   TOP.ITEM_ID               AS ITEM_ID
                                ,   TOP.ITEM_NAME             AS ITEM_NAME
                                ,   TOP.ITEM_COLOR_ID         AS ITEM_COLOR_ID
                                ,   MC.ITEM_COLOR_NAME        AS ITEM_COLOR_NAME
                                ,   TOP.ITEM_SIZE_ID          AS ITEM_SIZE_ID
                                ,   MIS.ITEM_SIZE_NAME        AS ITEM_SIZE_NAME
                                ,   TP.JAN                    AS JAN 
                                ,   TOP.PIC_STATUS            AS PIC_STATUS
                                ,   TOP.HIKI_QTY              AS ALLOC_QTY
                                ,   TOP.PIC_QTY               AS PIC_QTY
                                ,   TOP.PIC_DATE              AS PIC_DATE
                                ,   TOP.PIC_USER_ID           AS PIC_USER_ID
                                ,   MU_PIC.USER_NAME          AS PIC_USER_NAME
                                ,   TOP.STOCK_OUT_REG_QTY     AS STOCK_OUT_REG_QTY
                                ,   TOP.STOCK_OUT_REG_DATE    AS STOCK_OUT_REG_DATE
                                ,   TOP.STOCK_OUT_REG_USER_ID AS STOCK_OUT_REG_USER_ID
                                ,   MU_REG.USER_NAME          AS STOCK_OUT_REG_USER_NAME
                                ,   TOP.STOCK_OUT_FIX_QTY     AS STOCK_OUT_FIX_QTY
                                ,   TOP.STOCK_OUT_FIX_DATE    AS STOCK_OUT_FIX_DATE
                                ,   TOP.STOCK_OUT_FIX_USER_ID AS STOCK_OUT_FIX_USER_ID
                                ,   MU_FIX.USER_NAME          AS STOCK_OUT_FIX_USER_NAME
                            FROM
                                    T_ORDER_PIC TOP
                            LEFT OUTER JOIN
                                    T_PIC TP
                            ON
                                    TP.BATCH_NO = TOP.BATCH_NO
                                AND TP.LOCATION_CD = TOP.LOCATION_CD
                                AND TP.BOX_NO = TOP.BOX_NO
                                AND TP.ITEM_SKU_ID = TOP.ITEM_SKU_ID
                                AND TP.CENTER_ID = TOP.CENTER_ID
                                AND TP.SHIPPER_ID = TOP.SHIPPER_ID
                            LEFT OUTER JOIN
                                    M_COLORS MC
                            ON
                                    MC.ITEM_COLOR_ID = TOP.ITEM_COLOR_ID
                                AND MC.SHIPPER_ID = TOP.SHIPPER_ID
                            LEFT OUTER JOIN
                                    M_USERS MU_PIC
                            ON
                                    MU_PIC.SHIPPER_ID = TOP.SHIPPER_ID
                                AND MU_PIC.USER_ID = TOP.PIC_USER_ID
                            LEFT OUTER JOIN
                                    M_USERS MU_REG
                            ON
                                    MU_REG.SHIPPER_ID = TOP.SHIPPER_ID
                                AND MU_REG.USER_ID = TOP.STOCK_OUT_REG_USER_ID
                            LEFT OUTER JOIN
                                    M_USERS MU_FIX
                            ON
                                    MU_FIX.SHIPPER_ID = TOP.SHIPPER_ID
                                AND MU_FIX.USER_ID = TOP.STOCK_OUT_FIX_USER_ID
                            LEFT OUTER JOIN
                                    M_ITEM_SKU MIS
                            ON
                                    MIS.SHIPPER_ID = TOP.SHIPPER_ID
                                AND MIS.ITEM_SKU_ID = TOP.ITEM_SKU_ID
                            LEFT OUTER JOIN
                                    V_SHIP_TO_STORES VS
                            ON
                                    VS.SHIPPER_ID = TOP.SHIPPER_ID
                                AND VS.SHIP_TO_STORE_ID = TOP.SHIP_TO_STORE_ID
                            WHERE
                                    TOP.BATCH_NO = :BATCH_NO
                                AND TOP.SHIPPER_ID = :SHIPPER_ID
                                AND TOP.CENTER_ID = :CENTER_ID
                                AND TOP.PICK_KIND = 5
                            ORDER BY
                                    STORE_PIC_ORDER
                                ,   TOP.PIC_ORDER
                        ");
                }
                else
                {
                    query = new StringBuilder(@"
                    SELECT
                            T_PIC.PICKING_GROUP_NO      AS PICKING_GROUP_NO
                        ,   T_PIC.LOCSEC_1              AS LOCSEC_1
                        ,   T_PIC.LOCATION_CD           AS LOCATION_CD
                        ,   T_PIC.ITEM_SKU_ID           AS ITEM_SKU_ID
                        ,   T_PIC.ITEM_ID               AS ITEM_ID
                        ,   T_PIC.ITEM_NAME             AS ITEM_NAME
                        ,   T_PIC.ITEM_COLOR_ID         AS ITEM_COLOR_ID
                        ,   M_COLORS.ITEM_COLOR_NAME    AS ITEM_COLOR_NAME
                        ,   T_PIC.ITEM_SIZE_ID          AS ITEM_SIZE_ID
                        ,   M_ITEMSKU.ITEM_SIZE_NAME    AS ITEM_SIZE_NAME
                        ,   T_PIC.JAN                   AS JAN 
                        ,   T_PIC.PIC_STATUS            AS PIC_STATUS
                        ,   T_PIC.HIKI_QTY              AS ALLOC_QTY
                        ,   T_PIC.PIC_QTY               AS PIC_QTY
                        ,   T_PIC.PIC_DATE              AS PIC_DATE
                        ,   T_PIC.PIC_USER_ID           AS PIC_USER_ID
                        ,   MU_PIC.USER_NAME            AS PIC_USER_NAME
                        ,   T_PIC.STOCK_OUT_REG_QTY     AS STOCK_OUT_REG_QTY
                        ,   T_PIC.STOCK_OUT_REG_DATE    AS STOCK_OUT_REG_DATE
                        ,   T_PIC.STOCK_OUT_REG_USER_ID AS STOCK_OUT_REG_USER_ID
                        ,   MU_REG.USER_NAME            AS STOCK_OUT_REG_USER_NAME
                        ,   T_PIC.STOCK_OUT_FIX_QTY     AS STOCK_OUT_FIX_QTY
                        ,   T_PIC.STOCK_OUT_FIX_DATE    AS STOCK_OUT_FIX_DATE
                        ,   T_PIC.STOCK_OUT_FIX_USER_ID AS STOCK_OUT_FIX_USER_ID
                        ,   MU_FIX.USER_NAME            AS STOCK_OUT_FIX_USER_NAME
                        ,   T_PIC.BOX_NO                AS BOX_NO
                    FROM
                            T_PIC T_PIC
                    LEFT OUTER JOIN
                            M_COLORS M_COLORS
                    ON
                            M_COLORS.ITEM_COLOR_ID = T_PIC.ITEM_COLOR_ID
                        AND M_COLORS.SHIPPER_ID = T_PIC.SHIPPER_ID
                    LEFT OUTER JOIN
                            M_SIZES M_SIZES
                    ON
                            M_SIZES.ITEM_SIZE_ID = T_PIC.ITEM_SIZE_ID
                        AND M_SIZES.SHIPPER_ID = T_PIC.SHIPPER_ID
                    LEFT OUTER JOIN
                            M_USERS MU_PIC
                    ON
                            MU_PIC.SHIPPER_ID = T_PIC.SHIPPER_ID
                        AND MU_PIC.USER_ID = T_PIC.PIC_USER_ID
                    LEFT OUTER JOIN
                            M_USERS MU_REG
                    ON
                            MU_REG.SHIPPER_ID = T_PIC.SHIPPER_ID
                        AND MU_REG.USER_ID = T_PIC.STOCK_OUT_REG_USER_ID
                    LEFT OUTER JOIN
                            M_USERS MU_FIX
                    ON
                            MU_FIX.SHIPPER_ID = T_PIC.SHIPPER_ID
                        AND MU_FIX.USER_ID = T_PIC.STOCK_OUT_FIX_USER_ID
                    LEFT OUTER JOIN
                            M_ITEM_SKU M_ITEMSKU
                    ON
                            M_ITEMSKU.SHIPPER_ID = T_PIC.SHIPPER_ID
                        AND M_ITEMSKU.ITEM_SKU_ID = T_PIC.ITEM_SKU_ID
                    WHERE
                            T_PIC.BATCH_NO = :BATCH_NO
                        AND T_PIC.SHIPPER_ID = :SHIPPER_ID
                        AND T_PIC.CENTER_ID = :CENTER_ID
                    ");

                    // 並び順
                    if (vm.Head.PicKind == 1)
                    {
                        query.AppendLine(@"
                            AND T_PIC.PICK_KIND = 1
                        ");
                        query.AppendLine(@"
                            ORDER BY
                                T_PIC.PICKING_GROUP_NO
                            ,   T_PIC.LOCSEC_1
                            ,   T_PIC.PIC_ORDER
                            ");
                    }
                    else if (vm.Head.PicKind == 2)
                    {
                        query.AppendLine(@"
                            AND T_PIC.PICK_KIND = 2
                        ");
                        query.AppendLine(@"
                                ORDER BY
                                T_PIC.BATCH_NO
                            ,   T_PIC.PIC_ORDER
                        ");
                    }
                    else if (vm.Head.PicKind == 3)
                    {
                        query.AppendLine(@"
                            AND T_PIC.PICK_KIND = 3
                        ");
                        query.AppendLine(@"
                                ORDER BY
                                T_PIC.LOCSEC_1
                            ,   T_PIC.PIC_ORDER
                        ");
                    }
                    else if (vm.Head.PicKind == 4)
                    {
                        query.AppendLine(@"
                            AND T_PIC.PICK_KIND = 4
                        ");
                        query.AppendLine(@"
                                ORDER BY
                                T_PIC.BATCH_NO
                            ,   T_PIC.PIC_ORDER
                        ");
                    }
                }
                
                
                parameters.Add(":BATCH_NO", batchNo);
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":CENTER_ID", centerId);

                int totalCount = MvcDbContext.Current.Database.Connection.Query<TransferReferenceDcRow>(query.ToString(), parameters).Count();

                query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
                parameters.Add(":PAGE_SIZE", PageSize);

                // Choose data corresponding on each page
                parameters.AddDynamicParams(new { OFFSET = (Page - 1) *PageSize });

                var References = MvcDbContext.Current.Database.Connection.Query<PickResultRow>(query.ToString(), parameters);

                vm.PickResult = new PickResults();
                vm.PickResult.PickResult = new StaticPagedList<PickResultRow>(References, Page, PageSize, totalCount);

                return vm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region TC商品詳細
        /// <summary>
        /// Get Detail Data 
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        //public PickSortDetailViewModel GetItemDetailData(Common.ShipKinds shipKind, string centerId, DateTime? shipPlanDate)
        //{
        //    PickSortDetailViewModel vm = new PickSortDetailViewModel();
        //    DynamicParameters parameters = new DynamicParameters();
        //    StringBuilder query = new StringBuilder();

        //        // ピック詳細バッチ進捗 ヘッダ
        //        query = new StringBuilder(@"
        //                SELECT 
        //                        TO_CHAR(TSS.SHIP_PLAN_DATE,'YYYY/MM/DD') SHIP_PLAN_DATE
        //                    ,   COUNT(DISTINCT(TSS.SHIP_TO_STORE_ID)) AS SHIP_TO_STORE_QTY   --総出荷先数
        //                    ,   NVL(COUNT(DISTINCT(TSS.ITEM_SKU_ID)),0) AS ITEM_SKU_QTY    --総SKU数
        //                    ,   NVL(SUM(TSS.ALLOC_QTY),0) AS HIKI_QTY                       --総引当数
        //                FROM
        //                        T_STORE_SORT TSS
        //                WHERE
        //                        TSS.SHIP_PLAN_DATE   = :SHIP_PLAN_DATE
        //                    AND TSS.SHIPPER_ID = :SHIPPER_ID
        //                    AND TSS.CENTER_ID  = :CENTER_ID
        //                GROUP BY 
        //                        TSS.SHIP_PLAN_DATE
        //                    ,   TSS.CENTER_ID
        //                    ,   TSS.SHIPPER_ID
        //        ");
        //    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
        //    parameters.Add(":CENTER_ID", centerId);
        //    parameters.Add(":SHIP_PLAN_DATE", shipPlanDate);
        //    vm.Head = new PickSortDetailHead();
        //    vm.Head = MvcDbContext.Current.Database.Connection.Query<PickSortDetailHead>(query.ToString(), parameters).FirstOrDefault();
        //    vm.Head.ShipKind = shipKind;
        //    try
        //    {
        //        //ピック詳細バッチ進捗 明細
        //        parameters = new DynamicParameters();
        //        query = new StringBuilder(@"
        //            SELECT
        //                    TSS.BATCH_NO
        //                ,   TSS.ITEM_SKU_ID                 AS ITEM_SKU_ID
        //                ,   MAX(TSS.ITEM_ID)                AS ITEM_ID
        //                ,   MAX(TSS.ITEM_NAME)              AS ITEM_NAME
        //                ,   MAX(TSS.ITEM_COLOR_ID)          AS ITEM_COLOR_ID
        //                ,   MAX(M_COLORS.ITEM_COLOR_NAME)     AS ITEM_COLOR_NAME
        //                ,   MAX(TSS.ITEM_SIZE_ID)           AS ITEM_SIZE_ID
        //                ,   MAX(M_SIZES.ITEM_SIZE_NAME)       AS ITEM_SIZE_NAME
        //                ,   MAX(TSS.JAN)                    AS JAN 
        //                ,   NVL(COUNT(DISTINCT(TSS.SHIP_TO_STORE_ID)),0)  AS SHIP_TO_STORE_QTY 
        //                ,   NVL(SUM(TSS.ALLOC_QTY),0)               AS HIKI_QTY
        //                ,   NVL(SUM(TSS.ORDER_PIC_QTY),0)                AS PIC_QTY
        //                ,   NVL(TRUNC((SUM(TSS.ORDER_PIC_QTY) + SUM(TSS.STOCK_OUT_FIX_QTY)) /  SUM(TSS.ALLOC_QTY) * 100),0) AS PERCENT
        //                ,   NVL(SUM(TSS.STOCK_OUT_REG_QTY),0)      AS STOCK_OUT_REG_QTY
        //                ,   NVL(SUM(TSS.STOCK_OUT_FIX_QTY),0)      AS STOCK_OUT_FIX_QTY
        //                ,   MAX(TSS.SHIP_PLAN_DATE) SHIP_PLAN_DATE
        //            FROM
        //                    T_STORE_SORT TSS
        //            LEFT JOIN
        //                    M_COLORS M_COLORS
        //            ON
        //                    M_COLORS.ITEM_COLOR_ID = TSS.ITEM_COLOR_ID
        //                AND M_COLORS.SHIPPER_ID = TSS.SHIPPER_ID
        //            LEFT JOIN
        //                    M_SIZES M_SIZES
        //            ON
        //                    M_SIZES.ITEM_SIZE_ID = TSS.ITEM_SIZE_ID
        //                AND M_SIZES.SHIPPER_ID = TSS.SHIPPER_ID
        //            WHERE
        //                    TSS.SHIP_PLAN_DATE = :SHIP_PLAN_DATE
        //                AND TSS.SHIPPER_ID = :SHIPPER_ID
        //                AND TSS.CENTER_ID = :CENTER_ID
        //            GROUP BY
        //                    TSS.ITEM_SKU_ID 
        //                ,   TSS.BATCH_NO
        //                ,   TSS.CENTER_ID
        //                ,   TSS.SHIPPER_ID
        //            ORDER BY
        //                    TSS.BATCH_NO
        //                ,   TSS.ITEM_SKU_ID
        //               ");
        //        parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
        //        parameters.Add(":CENTER_ID", centerId);
        //        parameters.Add(":SHIP_PLAN_DATE", shipPlanDate);
        //        vm.PickResults = new PickResult();
        //        vm.PickResults.PickResults = MvcDbContext.Current.Database.Connection.Query<PickSortDetailRow>(query.ToString(), parameters).ToList();

        //        return vm;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion
    }
}