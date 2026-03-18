namespace Wms.Areas.Ship.Query.EcCancelUploadQuery
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
    using Wms.Areas.Ship.ViewModels.EcCancelUpload;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.EcCancelUpload.EcCancelUpload01SearchConditions;
    using static Wms.Areas.Ship.ViewModels.EcCancelUpload.EcCancelUpload02SearchConditions;

    //////////////////////////////using static Wms.Areas.Ship.ViewModels.EcCancelUpload.EcCancelUpload02SearchConditions;

    public class EcCancelUploadQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertShpEcCancelUpload01(EcCancelUpload01SearchConditions condition)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    // 1.ワークID採番
                    condition.Seq = new BaseQuery().GetWorkId();
                    condition.Page = 1;
                    StringBuilder query = new StringBuilder();
                    query.Append(@"
                    INSERT INTO WW_SHP_EC_CANCEL_UPLOAD(
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
                        ,   CU_DATE
                        ,   CU_CLASS
                        ,   SHIP_PLAN_DATE
                        ,   SHIP_INSTRUCT_ID
                        ,   ORDER_DATE
                        ,   TRANSPORTER_NAME
                        ,   ARRIVE_REQUEST_DATE
                        ,   ALLOC_DATE
                        ,   BATCH_NO
                        ,   EC_SHIP_CLASS_NAME
                        ,   ORDER_QTY
                        ,   GAS_QTY
                    )
                    SELECT
                            SYSTIMESTAMP
                        ,   :USER_ID
                        ,   :PROGRAM_NAME
                        ,   SYSTIMESTAMP
                        ,   :USER_ID
                        ,   :PROGRAM_NAME
                        ,   0
                        ,   :SHIPPER_ID
                        ,   :SEQ
                        ,   ROWNUM
                        ,   :CENTER_ID
                        ,   CASE SUB.STATUS
                                 WHEN 'C' 
                                     THEN SUB.AFT_ALLOC_CANCEL_DATE 
                                 WHEN 'U'
                                     THEN SUB.AFT_ALLOC_UP_DATE
                            END AS CU_DATE
                        ,   CASE SUB.STATUS
                                 WHEN 'C' THEN '");
                    query.Append(@EcCancelUploadResource.Cancel +
                                 @"' WHEN 'U' THEN '");
                        query.Append(@EcCancelUploadResource.Upload);
                    query.Append(@"'
                            END AS CU_STATUS
                        ,   SUB.SHIP_REQUEST_DATE AS SHIP_REQUEST_DATE
                        ,   SHIP_INSTRUCT_ID
                        ,   SUB.ORDER_DATE AS ORDER_DATE
                        ,   SUB.TRANSPORTER_NAME AS TRANSPORTER_NAME
                        ,   SUB.ARRIVE_REQUEST_DATE AS ARRIVE_REQUEST_DATE
                        ,   SUB.MAKE_DATE AS DATA_DATE
                        ,   SUB.BATCH_NO AS BATCH_NO
                        ,   SUB.EC_SHIP_CLASS AS EC_SHIP_CLASS
                        ,   SUB.ORDER_QTY AS ORDER_QTY
                        ,   SUB.GAS_QTY AS GAS_QTY
                     FROM(
                        SELECT
                                TE.SHIP_INSTRUCT_ID
                            ,   MAX(TE.AFT_ALLOC_CANCEL_DATE) AS AFT_ALLOC_CANCEL_DATE
                            ,   MAX(TE.AFT_ALLOC_UP_DATE) AS AFT_ALLOC_UP_DATE
                            ,   MAX(TE.SHIP_REQUEST_DATE) AS SHIP_REQUEST_DATE
                            ,   MAX(TE.ORDER_DATE) AS ORDER_DATE
                            ,   MAX(TE.TRANSPORTER_ID) AS TRANSPORTER_ID
                            ,   MAX(MT.TRANSPORTER_NAME) AS  TRANSPORTER_NAME
                            ,   MAX(TE.ARRIVE_REQUEST_DATE) AS ARRIVE_REQUEST_DATE
                            ,   MAX(TE.MAKE_DATE) AS MAKE_DATE
                            ,   MAX(TE.BATCH_NO) AS BATCH_NO
                            ,   MAX(MG.GEN_NAME) AS EC_SHIP_CLASS
                            ,   SUM(TE.ORDER_QTY) AS ORDER_QTY
                            ,   SUM(TG.GAS_QTY) AS GAS_QTY
                            ,   CASE WHEN MAX(TE.AFT_ALLOC_CANCEL_FLAG) = 1 AND MAX(TE.KAKU_FLAG) = 0
                                         THEN 'C'
                                     WHEN MAX(TE.AFT_ALLOC_UP_FLAG) = 1
                                         THEN 'U'
                                END AS STATUS
                        FROM
                                T_ECSHIPS TE
                        INNER JOIN
                                M_TRANSPORTERS MT
                        ON
                                TE.SHIPPER_ID = MT.SHIPPER_ID
                            AND TE.TRANSPORTER_ID = MT.TRANSPORTER_ID
                        LEFT OUTER JOIN
                                M_GENERALS MG
                        ON
                                TE.SHIPPER_ID = MG.SHIPPER_ID
                            AND MG.REGISTER_DIVI_CD = '1'
                            AND MG.CENTER_ID = '@@@'
                            AND MG.GEN_DIV_CD = 'EC_SHIP_CLASS'
                            AND TE.EC_SHIP_CLASS = MG.GEN_CD
                        LEFT OUTER JOIN
                                T_GAS TG
                        ON
                                TE.SHIPPER_ID = TG.SHIPPER_ID
                            AND TE.CENTER_ID = TG.CENTER_ID
                            AND TE.SHIP_INSTRUCT_ID = TG.SHIP_INSTRUCT_ID
                            AND TE.SHIP_INSTRUCT_SEQ = TG.SHIP_INSTRUCT_SEQ
                        WHERE
                                ((TE.AFT_ALLOC_CANCEL_FLAG = 1 AND TE.KAKU_FLAG = 0) --キャンセル
                               OR TE.AFT_ALLOC_UP_FLAG = 1) --更新
                            AND TE.SHIPPER_ID = :SHIPPER_ID
                            AND TE.CENTER_ID = :CENTER_ID
                    ");
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":USER_ID", Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "EcCancelUpload");
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                    parameters.Add(":CENTER_ID", condition.CenterId);

                    // 注文番号
                    if (!string.IsNullOrEmpty(condition.ShipInstructId))
                    {
                        query.Append(" AND TE.SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID ");
                        parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId);
                    }

                    query.Append(@"
                            GROUP BY
                                    TE.SHIP_INSTRUCT_ID
                                ,   TE.SHIPPER_ID
                                ,   TE.CENTER_ID
                        ) SUB
                        ");

                    var result = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);

                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    trans.Rollback();
                    return false;
                }
                trans.Commit();
                return true;
            }
        }

        /// <summary>
        /// Get Work List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<EcCancelUpload01ResultRow> EcCancelUpload01GetData(EcCancelUpload01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            
            string order;
            // Sort function
            switch (condition.SortKey)
            {
                case EcCancelUploadSortKey.ShipInstructId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            order = "ORDER BY WS.SHIP_INSTRUCT_ID DESC ";
                            break;

                        default:
                            order = "ORDER BY WS.SHIP_INSTRUCT_ID ASC ";
                            break;
                    }

                    break;
                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            order = "ORDER BY WS.CU_DATE DESC,WS.SHIP_INSTRUCT_ID DESC ";
                            break;

                        default:
                            order = "ORDER BY WS.CU_DATE ASC,WS.SHIP_INSTRUCT_ID ASC ";
                            break;
                    }

                    break;
            }
            StringBuilder query = new StringBuilder(@"
                SELECT
                        ROW_NUMBER() OVER (");
            query.AppendLine(order);
            query.AppendLine(@"
                        ) AS LINE_NO
                        ,   SHIPPER_ID
                        ,   SEQ
                        ,   CENTER_ID
                        ,   CU_DATE
                        ,   CU_CLASS
                        ,   SHIP_PLAN_DATE
                        ,   SHIP_INSTRUCT_ID
                        ,   ORDER_DATE
                        ,   TRANSPORTER_NAME
                        ,   ARRIVE_REQUEST_DATE
                        ,   ALLOC_DATE
                        ,   BATCH_NO
                        ,   EC_SHIP_CLASS_NAME
                        ,   ORDER_QTY
                        ,   GAS_QTY
                FROM
                        WW_SHP_EC_CANCEL_UPLOAD WS
                WHERE
                        WS.SHIPPER_ID = :SHIPPER_ID
                    AND WS.SEQ = :SEQ
            ");
            query.AppendLine(order);
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<EcCancelUpload01ResultRow>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var EcCancelUpload01s = MvcDbContext.Current.Database.Connection.Query<EcCancelUpload01ResultRow>(query.ToString(), parameters);

            //予定数合計
            var queryOrder = new StringBuilder(@"
                SELECT
                        NVL(SUM(ORDER_QTY),0) ORDER_QTY_SUM
                FROM
                        WW_SHP_EC_CANCEL_UPLOAD WS
                WHERE
                        WS.SHIPPER_ID = :SHIPPER_ID
                    AND WS.SEQ = :SEQ
            ");
            parameters = new DynamicParameters();
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);
            condition.InstructQtySum = MvcDbContext.Current.Database.Connection.Query<int>(queryOrder.ToString(), parameters).FirstOrDefault();

            //SKU数合計
            query = new StringBuilder(@"
                SELECT DISTINCT
                        ITEM_SKU_ID
                FROM
                        T_ECSHIPS TE
                INNER JOIN
                        M_TRANSPORTERS MT
                ON
                        TE.SHIPPER_ID = MT.SHIPPER_ID
                    AND TE.TRANSPORTER_ID = MT.TRANSPORTER_ID
                WHERE
                       ((TE.AFT_ALLOC_CANCEL_FLAG = 1 AND TE.KAKU_FLAG = 0) --キャンセル
                         OR TE.AFT_ALLOC_UP_FLAG = 1) --更新
                    AND TE.SHIPPER_ID = :SHIPPER_ID
                    AND TE.CENTER_ID = :CENTER_ID
                    ");
            parameters = new DynamicParameters();
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            condition.ItemSkuSum = MvcDbContext.Current.Database.Connection.Query<EcCancelUpload01ResultRow>(query.ToString(), parameters).Count();

            // Excute paging
            return new StaticPagedList<EcCancelUpload01ResultRow>(EcCancelUpload01s, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Select Detail
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IEnumerable<EcCancelUpload02ResultRow> GetDetailData(EcCancelUpload02SearchConditions condition)
        {
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
                            AND TSHIP.SHIP_INSTRUCT_SEQ = TPACK.SHIP_INSTRUCT_SEQ )
                SELECT
                        TE.SHIP_REQUEST_DATE AS SHIP_PLAN_DATE
                    ,   TE.SHIP_INSTRUCT_ID
                    ,   TE.DEST_PREF_NAME
                    ,   MG1.GEN_NAME AS EC_SHIP_CLASS
                    ,   TE.BATCH_NO
                    ,   TE.GAS_BATCH_NO
                    ,   TE.MAKE_DATE AS ALLOC_DATE
                    ,   TE.ORDER_DATE
                    ,   MT.TRANSPORTER_SHORT_NAME AS TRANSPORTER_NAME
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
                FROM
                    T_ECSHIPS_DATA TE 
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
                    AND MG2.GEN_DIV_CD = 'SAGAWA_ARRIVE_REQUEST_TIME'
                    AND MG2.GEN_CD = TE.ARRIVE_REQUEST_TIME
                WHERE
                        TE.SHIPPER_ID = :SHIPPER_ID
                    AND TE.CENTER_ID = :CENTER_ID
                    AND TE.SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId);
            // Sort function
            switch (condition.SortKey)
            {
                case EcCancelUpload02SortKey.ShipInstructSeqId:
                    query.AppendLine(@" ORDER BY TE.SHIP_INSTRUCT_SEQ ASC,TE.BOX_NO  ASC ");

                    break;
                default:
                    query.AppendLine(@" ORDER BY TE.BOX_NO ASC,TE.ITEM_SKU_ID ASC ");

                    break;
            }

            return MvcDbContext.Current.Database.Connection.Query<EcCancelUpload02ResultRow>(query.ToString(), parameters);
        }

        /// <summary>
        /// 出荷確定
        /// </summary>
        public void ShipConfirm(EcCancelUpload01SearchConditions searchConditions, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", searchConditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_SHIP_INSTRUCT_ID", searchConditions.HidShipInstructId, DbType.String, ParameterDirection.Input);
            param.Add("IN_PROC_KIND", searchConditions.Canup_Kbn, DbType.Int32, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_ECSHIP_CANUP",
                param,
                commandType: CommandType.StoredProcedure);

            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }


    }
}