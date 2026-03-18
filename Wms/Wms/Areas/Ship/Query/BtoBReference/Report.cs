namespace Wms.Areas.Ship.Query.BtoBReference
{
    using System.Collections.Generic;
    using System.Text;
    using Dapper;
    using Wms.Areas.Ship.ViewModels.BtoBReference;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using Wms.Areas.Ship.Resources;
    using static Wms.Areas.Ship.ViewModels.BtoBReference.BtoBReference01SearchConditions;
    using System.Linq;

    public class Report : BaseQuery
    {
        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、引当エラー(商品別)のデータを作る。</returns>
        public IEnumerable<BtoBReferenceReport> BtoBReferenceListing(BtoBReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
                WITH
                    SELECTED_WORK_DATA AS (
                        SELECT
                                *
                        FROM
                                WW_SHP_BTO_B_REFERENCE01
                        WHERE
                                SEQ = :SEQ
                            AND SHIPPER_ID = :SHIPPER_ID
                    )
            ");
            // B (過去分含む)
            if (string.IsNullOrWhiteSpace(condition.ShipBoxStatus) && condition.ShipBoxStatusOld)
            {
                query.Append(@"
                ,   SELECTED_PACK_DATA AS (
                        SELECT
                                BOX_NO
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   ITEM_SKU_ID
                            ,   ITEM_NAME
                            ,   ITEM_ID
                            ,   ITEM_COLOR_ID
                            ,   ITEM_SIZE_ID
                            ,   JAN
                            ,   RESULT_QTY
                            ,   DELI_NO
                            ,   KEN_DATE
                            ,   KEN_USER_ID
                            ,   NOUHIN_PRN_DATE
                            ,   NOUHIN_PRN_USER_ID
                            ,   BATCH_NO
                            ,   DELI_NO2
                            ,   TRANSPORTER_ID
                            ,   CASE_KAKU_DATE
                            ,   CASE_KAKU_USER_ID
                            ,   NOUHIN_NO

                        FROM
                                T_SHIP_PACKING_INFO
                        WHERE
                                SHIPPER_ID = :SHIPPER_ID
                            AND EC_FLAG = 0
                        UNION ALL
                            SELECT
                                BOX_NO
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   ITEM_SKU_ID
                            ,   ITEM_NAME
                            ,   ITEM_ID
                            ,   ITEM_COLOR_ID
                            ,   ITEM_SIZE_ID
                            ,   JAN
                            ,   RESULT_QTY
                            ,   DELI_NO
                            ,   KEN_DATE
                            ,   KEN_USER_ID
                            ,   NOUHIN_PRN_DATE
                            ,   NOUHIN_PRN_USER_ID
                            ,   BATCH_NO
                            ,   DELI_NO2
                            ,   TRANSPORTER_ID
                            ,   CASE_KAKU_DATE
                            ,   CASE_KAKU_USER_ID
                            ,   NOUHIN_NO

                        FROM
                                A_SHIP_PACKING_INFO
                        WHERE
                                SHIPPER_ID = :SHIPPER_ID
                            AND EC_FLAG = 0
                    )
                ");
            }
            // C (過去分含むかつ日次済み)
            else if (condition.ShipBoxStatus == "5" && !condition.ShipBoxStatusOld)
            {
                query.Append(@"
                ,   SELECTED_PACK_DATA AS (
                        SELECT
                                BOX_NO
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   ITEM_SKU_ID
                            ,   ITEM_NAME
                            ,   ITEM_ID
                            ,   ITEM_COLOR_ID
                            ,   ITEM_SIZE_ID
                            ,   JAN
                            ,   RESULT_QTY
                            ,   DELI_NO
                            ,   KEN_DATE
                            ,   KEN_USER_ID
                            ,   NOUHIN_PRN_DATE
                            ,   NOUHIN_PRN_USER_ID
                            ,   BATCH_NO
                            ,   DELI_NO2
                            ,   TRANSPORTER_ID
                            ,   CASE_KAKU_DATE
                            ,   CASE_KAKU_USER_ID
                            ,   NOUHIN_NO

                        FROM
                                A_SHIP_PACKING_INFO
                        WHERE
                                SHIPPER_ID = :SHIPPER_ID
                            AND EC_FLAG = 0
                    )
                ");
            }
            // A
            else
            {
                query.Append(@"
                ,   SELECTED_PACK_DATA AS (
                        SELECT
                                BOX_NO
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   ITEM_SKU_ID
                            ,   ITEM_NAME
                            ,   ITEM_ID
                            ,   ITEM_COLOR_ID
                            ,   ITEM_SIZE_ID
                            ,   JAN
                            ,   RESULT_QTY
                            ,   DELI_NO
                            ,   KEN_DATE
                            ,   KEN_USER_ID
                            ,   NOUHIN_PRN_DATE
                            ,   NOUHIN_PRN_USER_ID
                            ,   BATCH_NO
                            ,   DELI_NO2
                            ,   TRANSPORTER_ID
                            ,   CASE_KAKU_DATE
                            ,   CASE_KAKU_USER_ID
                            ,   NOUHIN_NO
                        FROM
                                T_SHIP_PACKING_INFO
                        WHERE
                                SHIPPER_ID = :SHIPPER_ID
                            AND EC_FLAG = 0
                    )
                ");
            }
            query.Append(@"
                ,   TARGET_ALLOC_INFO AS (
                        SELECT
                                ALLOC_NO
                            ,   BATCH_NAME
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                        FROM
                                T_ALLOC_INFO
                        WHERE
                                SHIPPER_ID = :SHIPPER_ID
                        UNION
                        SELECT
                                ALLOC_NO
                            ,   BATCH_NAME
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                        FROM
                                A_ALLOC_INFO
                        WHERE
                                SHIPPER_ID = :SHIPPER_ID
                    )
                ,   TARGET_PACK_DATA AS (
                        SELECT
                                PACK.BOX_NO
                            ,   PACK.SHIP_INSTRUCT_ID
                            ,   PACK.SHIP_INSTRUCT_SEQ
                            ,   PACK.CENTER_ID
                            ,   PACK.SHIPPER_ID
                            ,   PACK.ITEM_SKU_ID
                            ,   PACK.ITEM_ID
                            ,   PACK.ITEM_NAME
                            ,   PACK.ITEM_COLOR_ID
                            ,   PACK.ITEM_SIZE_ID
                            ,   PACK.JAN
                            ,   PACK.RESULT_QTY
                            ,   PACK.DELI_NO
                            ,   PACK.CASE_KAKU_DATE
                            ,   PACK.CASE_KAKU_USER_ID || '　' || MU_CASE_KAKU.USER_NAME AS CASE_KAKU_USER
                            ,   PACK.KEN_DATE
                            ,   PACK.KEN_USER_ID || '　' || MU_KEN.USER_NAME AS KEN_USER
                            ,   PACK.NOUHIN_PRN_DATE
                            ,   PACK.NOUHIN_PRN_USER_ID || '　' || MU_NOUHIN.USER_NAME AS NOUHIN_USER
                            ,   WK.SHIP_TO_STORE_ID
                            ,   WK.SHIP_TO_STORE_NAME
                            ,   PACK.BATCH_NO
                            ,   WK.SHIP_STATUS_NAME
                            ,   WK.KAKU_DATE
                            ,   PACK.DELI_NO2
                            ,   PACK.TRANSPORTER_ID
                            ,   PACK.NOUHIN_NO

                        FROM
                                SELECTED_PACK_DATA PACK
                        INNER JOIN
                                SELECTED_WORK_DATA WK
                        ON
                                WK.BOX_NO = PACK.BOX_NO
                            AND WK.CENTER_ID = PACK.CENTER_ID
                            AND WK.SHIPPER_ID = PACK.SHIPPER_ID
                        LEFT OUTER JOIN
                                M_USERS MU_CASE_KAKU
                        ON
                                MU_CASE_KAKU.USER_ID = PACK.CASE_KAKU_USER_ID
                            AND MU_CASE_KAKU.SHIPPER_ID = PACK.SHIPPER_ID
                        LEFT OUTER JOIN
                                M_USERS MU_KEN
                        ON
                                MU_KEN.USER_ID = PACK.KEN_USER_ID
                            AND MU_KEN.SHIPPER_ID = PACK.SHIPPER_ID
                        LEFT OUTER JOIN
                                M_USERS MU_NOUHIN
                        ON
                                MU_NOUHIN.USER_ID = PACK.NOUHIN_PRN_USER_ID
                            AND MU_NOUHIN.SHIPPER_ID = PACK.SHIPPER_ID
                )
                SELECT
                        PACK.CENTER_ID
                    ,   PACK.SHIP_TO_STORE_ID
                    ,   PACK.SHIP_TO_STORE_NAME
                    ,   PACK.BOX_NO
                    ,   PACK.BATCH_NO
                    ,   ALLOC.BATCH_NAME
                    ,   PACK.SHIP_INSTRUCT_ID
                    ,   PACK.SHIP_INSTRUCT_SEQ
                    ,   PACK.ITEM_SKU_ID
                    ,   PACK.ITEM_ID
                    ,   PACK.ITEM_NAME
                    ,   PACK.ITEM_COLOR_ID
                    ,   COL.ITEM_COLOR_NAME
                    ,   PACK.ITEM_SIZE_ID
                    ,   MIS.ITEM_SIZE_NAME
                    ,   PACK.JAN
                    ,   PACK.RESULT_QTY
                    ,   PACK.SHIP_STATUS_NAME
                    ,   PACK.KAKU_DATE
                    ,   NVL(PACK.DELI_NO2, PACK.DELI_NO) AS DELI_NO
                    ,   PACK.CASE_KAKU_DATE
                    ,   PACK.CASE_KAKU_USER
                    ,   PACK.KEN_DATE
                    ,   PACK.KEN_USER
                    ,   PACK.NOUHIN_PRN_DATE
                    ,   PACK.NOUHIN_USER
                    ,   MT.TRANSPORTER_NAME
                    ,   PACK.NOUHIN_NO
                    ,   MIS.NORMAL_SELLING_PRICE_EX_TAX
                FROM
                        TARGET_PACK_DATA PACK
                LEFT OUTER JOIN
                        TARGET_ALLOC_INFO ALLOC
                ON
                        PACK.BATCH_NO = ALLOC.ALLOC_NO
                    AND PACK.CENTER_ID = ALLOC.CENTER_ID
                    AND PACK.SHIPPER_ID = ALLOC.SHIPPER_ID
                LEFT OUTER JOIN
                        M_COLORS COL
                ON
                        PACK.ITEM_COLOR_ID = COL.ITEM_COLOR_ID
                    AND PACK.SHIPPER_ID = COL.SHIPPER_ID
                LEFT JOIN
                        M_ITEM_SKU MIS
                ON
                        PACK.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                    AND PACK.SHIPPER_ID = MIS.SHIPPER_ID
                LEFT JOIN
                        M_TRANSPORTERS MT
                ON
                        PACK.SHIPPER_ID   = MT.SHIPPER_ID
                    AND PACK.TRANSPORTER_ID = MT.TRANSPORTER_ID

                ORDER BY
                        PACK.CENTER_ID
                    ,   PACK.SHIP_TO_STORE_ID
                    ,   PACK.BOX_NO
                    ,   PACK.BATCH_NO
                    ,   PACK.SHIP_INSTRUCT_ID
                    ,   PACK.SHIP_INSTRUCT_SEQ
            ");
            parameters.Add(":SEQ", condition.Seq);
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

            try
            {
                // Fill data to memory
                return MvcDbContext.Current.Database.Connection.Query<BtoBReferenceReport>(query.ToString(), parameters);
            }
            catch (System.Exception ex)
            {
                Mvc.Common.AppError.PutLogREF(ex, "BtoBReferenceListing");
                throw;
            }

        }

        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IEnumerable<BtoBReferenceReportJan> GetDetailJanData(BtoBReference02SearchConditions condition)
        {
            var BtoBReference01 = MvcDbContext.Current.ShpBtoBReference01s.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == condition.Seq && x.LineNo == condition.LineNo).FirstOrDefault();
            DynamicParameters parameters = new DynamicParameters();
            var query = new StringBuilder();
            // B（過去分含む）
            if (condition.Parten == "B")
            {
                query.Append(@"
                    WITH
                        SELECTED_PACKING_DATA AS (
                            SELECT
                                    BOX_NO
                                ,   SHIP_TO_STORE_ID
                                ,   DELI_NO
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   BATCH_NO
                                ,   ITEM_SKU_ID
                                ,   ITEM_ID
                                ,   ITEM_NAME
                                ,   ITEM_COLOR_ID
                                ,   ITEM_SIZE_ID
                                ,   JAN
                                ,   RESULT_QTY
                                ,   SHIPPER_ID
                                ,   CENTER_ID
                                ,   TRANSPORTER_ID
                                ,   NOUHIN_PRN_USER_ID
                                ,   KEN_USER_ID
                                ,   TO_CHAR(KEN_DATE, 'YYYY/MM/DD HH24:MI:SS') AS KEN_DATE
                                ,   TO_CHAR(NOUHIN_PRN_DATE, 'YYYY/MM/DD HH24:MI:SS') AS NOUHIN_PRN_DATE
                                ,   TO_CHAR(KAKU_DATE, 'YYYY/MM/DD') AS KAKU_DATE
                                ,   KAKU_USER_ID
                            FROM
                                    T_SHIP_PACKING_INFO
                            WHERE
                                    BOX_NO = :BOX_NO
                                AND CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID
                                AND EC_FLAG = 0
                            UNION
                            SELECT
                                    BOX_NO
                                ,   SHIP_TO_STORE_ID
                                ,   DELI_NO
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   BATCH_NO
                                ,   ITEM_SKU_ID
                                ,   ITEM_ID
                                ,   ITEM_NAME
                                ,   ITEM_COLOR_ID
                                ,   ITEM_SIZE_ID
                                ,   JAN
                                ,   RESULT_QTY
                                ,   SHIPPER_ID
                                ,   CENTER_ID
                                ,   TRANSPORTER_ID
                                ,   NOUHIN_PRN_USER_ID
                                ,   KEN_USER_ID
                                ,   TO_CHAR(KEN_DATE, 'YYYY/MM/DD HH24:MI:SS') AS KEN_DATE
                                ,   TO_CHAR(NOUHIN_PRN_DATE, 'YYYY/MM/DD HH24:MI:SS') AS NOUHIN_PRN_DATE
                                ,   TO_CHAR(KAKU_DATE, 'YYYY/MM/DD') AS KAKU_DATE
                                ,   KAKU_USER_ID
                            FROM
                                    A_SHIP_PACKING_INFO
                            WHERE
                                    BOX_NO = :BOX_NO
                                AND CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID
                                AND EC_FLAG = 0
                    )
                    ,   SELECTED_SHIP_DATA AS(
                            SELECT
                                    DELI_SHIWAKE_CD
                                ,   SHIP_PLAN_DATE
                                ,   ALLOC_QTY
                                ,   SHIPPER_ID
                                ,   CENTER_ID
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                            FROM
                                    T_SHIPS
                            WHERE
                                    CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID
                            UNION
                            SELECT
                                    DELI_SHIWAKE_CD
                                ,   SHIP_PLAN_DATE
                                ,   ALLOC_QTY
                                ,   SHIPPER_ID
                                ,   CENTER_ID
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                            FROM
                                    A_SHIPS
                            WHERE
                                    CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID
                    )
                ");
            }
            // C(過去分含むかつ日次済み
            else if (condition.Parten == "C")
            {
                query.Append(@"
                    WITH
                        SELECTED_PACKING_DATA AS (
                            SELECT
                                    BOX_NO
                                ,   SHIP_TO_STORE_ID
                                ,   DELI_NO
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   BATCH_NO
                                ,   ITEM_SKU_ID
                                ,   ITEM_ID
                                ,   ITEM_NAME
                                ,   ITEM_COLOR_ID
                                ,   ITEM_SIZE_ID
                                ,   JAN
                                ,   RESULT_QTY
                                ,   SHIPPER_ID
                                ,   CENTER_ID
                                ,   TRANSPORTER_ID
                                ,   NOUHIN_PRN_USER_ID
                                ,   KEN_USER_ID
                                ,   TO_CHAR(KEN_DATE, 'YYYY/MM/DD HH24:MI:SS') AS KEN_DATE
                                ,   TO_CHAR(NOUHIN_PRN_DATE, 'YYYY/MM/DD HH24:MI:SS') AS NOUHIN_PRN_DATE
                                ,   TO_CHAR(KAKU_DATE, 'YYYY/MM/DD') AS KAKU_DATE
                                ,   KAKU_USER_ID
                            FROM
                                    A_SHIP_PACKING_INFO
                            WHERE
                                    BOX_NO = :BOX_NO
                                AND CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID
                                AND EC_FLAG = 0
                    )
                    ,   SELECTED_SHIP_DATA AS(
                            SELECT
                                    DELI_SHIWAKE_CD
                                ,   SHIP_PLAN_DATE
                                ,   ALLOC_QTY
                                ,   SHIPPER_ID
                                ,   CENTER_ID
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                            FROM
                                    A_SHIPS
                            WHERE
                                    CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID
                    )
                ");
            }
            // A
            else
            {
                query.Append(@"
                    WITH
                        SELECTED_PACKING_DATA AS (
                            SELECT
                                    BOX_NO
                                ,   SHIP_TO_STORE_ID
                                ,   DELI_NO
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   BATCH_NO
                                ,   ITEM_SKU_ID
                                ,   ITEM_ID
                                ,   ITEM_NAME
                                ,   ITEM_COLOR_ID
                                ,   ITEM_SIZE_ID
                                ,   JAN
                                ,   RESULT_QTY
                                ,   SHIPPER_ID
                                ,   CENTER_ID
                                ,   TRANSPORTER_ID
                                ,   NOUHIN_PRN_USER_ID
                                ,   KEN_USER_ID
                                ,   TO_CHAR(KEN_DATE, 'YYYY/MM/DD HH24:MI:SS') AS KEN_DATE
                                ,   TO_CHAR(NOUHIN_PRN_DATE, 'YYYY/MM/DD HH24:MI:SS') AS NOUHIN_PRN_DATE
                                ,   TO_CHAR(KAKU_DATE, 'YYYY/MM/DD') AS KAKU_DATE
                                ,   KAKU_USER_ID
                            FROM
                                    T_SHIP_PACKING_INFO
                            WHERE
                                    BOX_NO = :BOX_NO
                                AND CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID
                                AND EC_FLAG = 0
                    )
                    ,   SELECTED_SHIP_DATA AS(
                            SELECT
                                    DELI_SHIWAKE_CD
                                ,   SHIP_PLAN_DATE
                                ,   ALLOC_QTY
                                ,   SHIPPER_ID
                                ,   CENTER_ID
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                            FROM
                                    T_SHIPS
                            WHERE
                                    CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID
                    )
                ");
            }
            query.Append(@"
                ,   TARGET_PACK_DATA AS (
                        SELECT
                                PACK.*
                            ,   SHIP.DELI_SHIWAKE_CD
                            ,   SHIP.SHIP_PLAN_DATE
                            ,   SHIP.ALLOC_QTY
                            ,   MC.CENTER_NAME1
                        FROM
                                SELECTED_PACKING_DATA PACK
                        LEFT OUTER JOIN
                                SELECTED_SHIP_DATA SHIP
                        ON
                                SHIP.SHIP_INSTRUCT_ID = PACK.SHIP_INSTRUCT_ID
                            AND SHIP.SHIP_INSTRUCT_SEQ = PACK.SHIP_INSTRUCT_SEQ
                            AND SHIP.CENTER_ID = PACK.CENTER_ID
                            AND SHIP.SHIPPER_ID = PACK.SHIPPER_ID
                        LEFT OUTER JOIN
                            M_CENTERS MC
                        ON
                                MC.SHIPPER_ID = PACK.SHIPPER_ID
                            AND MC.CENTER_ID = PACK.CENTER_ID
                )
                SELECT
                        TGT.CENTER_ID || '  ' || TGT.CENTER_NAME1 AS CENTER
                    ,   TGT.SHIP_TO_STORE_ID || '  ' || VSTS.SHIP_TO_STORE_NAME1 AS SHIP_TO_STORE
                    ,   TGT.BOX_NO
                    ,   :USER_ID AS USER_ID
                    ,   MU1.USER_NAME USER_NAME
                    ,   TGT.SHIP_INSTRUCT_ID
                    ,   TGT.SHIP_INSTRUCT_SEQ
                    ,   TO_CHAR(TGT.SHIP_PLAN_DATE,'YYYY/MM/DD') AS SHIP_PLAN_DATE
                    ,   TGT.BATCH_NO
                    ,   MIC.CATEGORY_NAME1 AS CATEGORY1
                    ,   TGT.ITEM_ID
                    ,   TGT.ITEM_NAME
                    ,   TGT.ITEM_COLOR_ID
                    ,   MC.ITEM_COLOR_NAME
                    ,   TGT.ITEM_SIZE_ID
                    ,   MIS.ITEM_SIZE_NAME
                    ,   TGT.JAN
                    ,   TGT.ALLOC_QTY
                    ,   TGT.RESULT_QTY
                FROM
                        TARGET_PACK_DATA TGT
                INNER JOIN
                        M_USERS MU1
                ON
                        :USER_ID = MU1.USER_ID
                    AND TGT.SHIPPER_ID   = MU1.SHIPPER_ID
                LEFT JOIN
                        M_ITEM_SKU MIS
                ON
                        TGT.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                    AND TGT.SHIPPER_ID = MIS.SHIPPER_ID
                LEFT JOIN
                        V_SHIP_TO_STORES VSTS
                ON
                        TGT.SHIP_TO_STORE_ID  = VSTS.SHIP_TO_STORE_ID
                    AND TGT.SHIPPER_ID = VSTS.SHIPPER_ID
                LEFT JOIN
                        M_COLORS MC
                ON
                        TGT.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                    AND TGT.SHIPPER_ID   = MC.SHIPPER_ID
                LEFT JOIN
                        M_ITEM_CATEGORIES4 MIC
                ON
                        MIS.CATEGORY_ID1 = MIC.CATEGORY_ID1
                    AND MIS.CATEGORY_ID2 = MIC.CATEGORY_ID2
                    AND MIS.CATEGORY_ID3 = MIC.CATEGORY_ID3
                    AND MIS.CATEGORY_ID4 = MIC.CATEGORY_ID4
                    AND MIS.SHIPPER_ID   = MIC.SHIPPER_ID
                ORDER BY TGT.SHIP_INSTRUCT_ID ASC,TGT.SHIP_INSTRUCT_SEQ ASC
                ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":BOX_NO", BtoBReference01.BoxNo);
            parameters.Add(":SHIP_TO_STORE_ID", BtoBReference01.ShipToStoreId);
            parameters.Add(":USER_ID", Common.Profile.User.UserId);

            return MvcDbContext.Current.Database.Connection.Query<BtoBReferenceReportJan>(query.ToString(), parameters);
        }

        /// <summary>
        /// 出荷梱包進捗照会 ダウンロード（ケース単位）t
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IEnumerable<BtoBReferenceCaseReport> BtoBReferenceCaseListing(BtoBReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                WITH
                    SELECTED_WORK_DATA AS ( 
                        SELECT
                                *
                        FROM
                                WW_SHP_BTO_B_REFERENCE01
                        WHERE
                                SHIPPER_ID = :SHIPPER_ID
                            AND SEQ = :SEQ
                    )
            ");
            // B (過去分含む)
            if (string.IsNullOrWhiteSpace(condition.ShipBoxStatus) && condition.ShipBoxStatusOld)
            {
                query.Append(@"
                ,   SELECTED_PACK_DATA AS (
                        SELECT
                                BOX_NO
                            ,   SHIP_TO_STORE_ID
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   RESULT_QTY
                            ,   BATCH_NO
                            ,   NOUHIN_NO
                            ,   NOUHIN_PRN_DATE
                            ,   NOUHIN_PRN_USER_ID
                            ,   TRANSPORTER_ID
                            ,   CLIENT_CD
                            ,   ITEM_SKU_ID
                            ,   DELI_NO
                            ,   DELI_NO2
                        FROM
                                T_SHIP_PACKING_INFO
                        WHERE
                                SHIPPER_ID = :SHIPPER_ID
                            AND EC_FLAG IN (0,2)
                        UNION ALL
                        SELECT
                                BOX_NO
                            ,   SHIP_TO_STORE_ID
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   RESULT_QTY
                            ,   BATCH_NO
                            ,   NOUHIN_NO
                            ,   NOUHIN_PRN_DATE
                            ,   NOUHIN_PRN_USER_ID
                            ,   TRANSPORTER_ID
                            ,   CLIENT_CD
                            ,   ITEM_SKU_ID
                            ,   DELI_NO
                            ,   DELI_NO2
                        FROM
                                A_SHIP_PACKING_INFO
                        WHERE
                                SHIPPER_ID = :SHIPPER_ID
                            AND EC_FLAG IN (0,2)
                    )
                ");
            }
            // C (過去分含むかつ日次済み)
            else if (condition.ShipBoxStatus == "5" && !condition.ShipBoxStatusOld)
            {
                query.Append(@"
                ,   SELECTED_PACK_DATA AS (
                        SELECT
                                BOX_NO
                            ,   SHIP_TO_STORE_ID
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   RESULT_QTY
                            ,   BATCH_NO
                            ,   NOUHIN_NO
                            ,   NOUHIN_PRN_DATE
                            ,   NOUHIN_PRN_USER_ID
                            ,   TRANSPORTER_ID
                            ,   CLIENT_CD
                            ,   ITEM_SKU_ID
                            ,   DELI_NO
                            ,   DELI_NO2
                        FROM
                                A_SHIP_PACKING_INFO
                        WHERE
                                SHIPPER_ID = :SHIPPER_ID
                            AND EC_FLAG IN (0,2)
                    )
                ");
            }
            // A
            else
            {
                query.Append(@"
                ,   SELECTED_PACK_DATA AS (
                        SELECT
                                BOX_NO
                            ,   SHIP_TO_STORE_ID
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   RESULT_QTY
                            ,   BATCH_NO
                            ,   NOUHIN_NO
                            ,   NOUHIN_PRN_DATE
                            ,   NOUHIN_PRN_USER_ID
                            ,   TRANSPORTER_ID
                            ,   CLIENT_CD
                            ,   ITEM_SKU_ID
                            ,   DELI_NO
                            ,   DELI_NO2
                        FROM
                                T_SHIP_PACKING_INFO
                        WHERE
                                SHIPPER_ID = :SHIPPER_ID
                            AND EC_FLAG IN (0,2)
                    )
                ");
            }
            query.Append($@"
                    ,TARGET_PACK_DATA AS (
                        SELECT
                                PACK.SHIPPER_ID
                            ,   PACK.CENTER_ID
                            ,   PACK.BOX_NO
                            ,   PACK.SHIP_TO_STORE_ID
                            ,   MAX(WK.SHIP_TO_STORE_NAME) AS SHIP_TO_STORE_NAME
                            ,   SUM(PACK.RESULT_QTY) AS RESULT_QTY
                            ,   MAX(PACK.BATCH_NO) AS MAX_BATCH_NO
                            ,   MIN(PACK.BATCH_NO) AS MIN_BATCH_NO
                            ,   MAX(PACK.NOUHIN_NO) AS MAX_NOUHIN_NO
                            ,   MIN(PACK.NOUHIN_NO) AS MIN_NOUHIN_NO
                            ,   MAX(PACK.NOUHIN_PRN_DATE) AS NOUHIN_PRN_DATE
                            ,   MAX(PACK.NOUHIN_PRN_USER_ID) AS NOUHIN_PRN_USER_ID
                            ,   MAX(WK.KAKU_DATE) AS KAKU_DATE
                            ,   MAX(PACK.TRANSPORTER_ID) AS TRANSPORTER_ID
                            ,   MAX(WK.TRANSPORTER_NAME) AS TRANSPORTER_NAME
                            ,   MAX(PACK.CLIENT_CD) AS CLIENT_CD
                            ,   MAX(WK.SHIP_STATUS_NAME) AS SHIP_STATUS_NAME
                            ,   MIS.BRAND_ID
                            ,   MAX(PACK.DELI_NO) AS DELI_NO
                            ,   MAX(PACK.DELI_NO2) AS DELI_NO2
                        FROM
                                SELECTED_PACK_DATA PACK
                        INNER JOIN
                                SELECTED_WORK_DATA WK
                        ON
                                PACK.BOX_NO = WK.BOX_NO
                            AND PACK.SHIP_TO_STORE_ID = WK.SHIP_TO_STORE_ID
                            AND PACK.CENTER_ID = WK.CENTER_ID
                            AND PACK.SHIPPER_ID = WK.SHIPPER_ID
                        INNER JOIN
                                M_ITEM_SKU MIS
                        ON
                                MIS.ITEM_SKU_ID = PACK.ITEM_SKU_ID
                            AND MIS.SHIPPER_ID = PACK.SHIPPER_ID
                        GROUP BY
                                PACK.BOX_NO
                            ,   PACK.SHIP_TO_STORE_ID
                            ,   PACK.CENTER_ID
                            ,   PACK.SHIPPER_ID
                            ,   MIS.BRAND_ID
                    )
                SELECT
                        PACK.CENTER_ID
                    ,   PACK.BOX_NO
                    ,   PACK.SHIP_TO_STORE_ID
                    ,   MAX(PACK.SHIP_TO_STORE_NAME) AS SHIP_TO_STORE_NAME
                    ,   SUM(PACK.RESULT_QTY) AS RESULT_QTY
                    ,   MAX(PACK.MAX_BATCH_NO) || (CASE WHEN MIN(PACK.MIN_BATCH_NO) <> MAX(PACK.MAX_BATCH_NO) THEN '　{BtoBReferenceResource.Other}' ELSE '' END) AS BATCH_NO
                    ,   MAX(PACK.MAX_NOUHIN_NO) || (CASE WHEN MIN(PACK.MIN_NOUHIN_NO) <> MAX(PACK.MAX_NOUHIN_NO) THEN '　{BtoBReferenceResource.Other}' ELSE '' END) AS NOUHIN_NO
                    ,   MAX(PACK.NOUHIN_PRN_DATE) AS NOUHIN_PRN_DATE
                    ,   MAX(PACK.NOUHIN_PRN_USER_ID) AS NOUHIN_PRN_USER_ID
                    ,   MAX(MU.USER_NAME) AS NOUHIN_PRN_USER_NAME
                    ,   MAX(PACK.KAKU_DATE) AS KAKU_DATE
                    ,   MAX(PACK.TRANSPORTER_ID) AS TRANSPORTER_ID
                    ,   MAX(PACK.TRANSPORTER_NAME) AS TRANSPORTER_NAME
                    ,   MAX(PACK.CLIENT_CD) AS CLIENT_CD
                    ,   MAX(PACK.SHIP_STATUS_NAME) AS SHIP_STATUS_NAME
                    ,   LISTAGG(MB.BRAND_SHORT_NAME, ',') WITHIN GROUP (ORDER BY PACK.BRAND_ID) AS BRAND_NAME
                    ,   MAX(NVL(PACK.DELI_NO2, PACK.DELI_NO)) AS DELI_NO
                FROM
                        TARGET_PACK_DATA PACK
                LEFT OUTER JOIN
                        M_USERS MU
                ON
                        PACK.NOUHIN_PRN_USER_ID = MU.USER_ID
                    AND PACK.SHIPPER_ID = MU.SHIPPER_ID
                LEFT OUTER JOIN
                        M_BRANDS MB
                ON
                        MB.BRAND_ID = PACK.BRAND_ID
                    AND MB.SHIPPER_ID = PACK.SHIPPER_ID
                GROUP BY
                        PACK.BOX_NO
                    ,   PACK.SHIP_TO_STORE_ID
                    ,   PACK.CENTER_ID
                    ,   PACK.SHIPPER_ID
                ORDER BY
                        PACK.BOX_NO ASC


            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);


            return MvcDbContext.Current.Database.Connection.Query<BtoBReferenceCaseReport>(query.ToString(), parameters);
        }
    }
}