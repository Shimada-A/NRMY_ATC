namespace Wms.Areas.Ship.Query.BtoBInstructionInput
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
    using Wms.Areas.Ship.ViewModels.BtoBInstructionInput;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.BtoBInstructionInput.BtoBInstructionInput01SearchConditions;

    public class BtoBInstructionInputQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertShpBtoBInstructionInput(BtoBInstructionInput01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            // 1.ワークID採番
            var seq = new BaseQuery().GetWorkId();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    StringBuilder query = new StringBuilder(@"
                        INSERT INTO WW_SHP_BTO_B_RESULT_INPUT(
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
                            ,   STORE_CLASS
                            ,   SHIP_TO_STORE_ID
                            ,   SHIP_TO_STORE_NAME
                            ,   TRANSPORTER_NAME
                            ,   SHIP_TO_QTY
                            ,   INSTRUCT_QTY
                            ,   ALLOC_DATE
                            ,   ALLOC_QTY
                            ,   SEQ_PRE 
                        ) ");

                    if (condition.ResultType == ViewModels.BtoBInstructionInput.ResultTypes.Sku)
                    {
                        query.Append(@"
                        WITH
                            TARGET_SHIP_DATA AS (
                                SELECT 
                                      MAX(TS.MAKE_DATE) AS MAKE_DATE" +
                                        ", '" + Common.Profile.User.UserId + "' AS MAKE_USER_ID" +
                                        ",'BtoBInstructionInput'" + " AS MAKE_PROGRAM_NAME" +
                                        ",MAX(TS.MAKE_DATE) " + "AS UPDATE_DATE" +
                                        ", '" + Common.Profile.User.UserId + "' AS UPDATE_USER_ID" +
                                        ",'BtoBInstructionInput'" + " AS UPDATE_PROGRAM_NAME" +
                                         ",0" + " AS UPDATE_COUNT" +
                                        "," + "'" + Common.Profile.User.ShipperId + "'" + " AS SHIPPER_ID" +
                                        "," + seq + " AS SEQ"
                            );
                        query.Append(@"
                            ,   TS.CENTER_ID
                            ,   TS.SHIP_PLAN_DATE
                            ,   MAX(MG1.GEN_NAME) INSTRUCT_CLASS_NAME
                            ,   MAX(MG2.GEN_NAME) EMERGENCY_CLASS_NAME
                            ,   TS.SHIP_INSTRUCT_ID
                            ,   NULL AS SHIP_INSTRUCT_SEQ
                            ,   MAX(TS.ITEM_ID) ITEM_ID
                            ,   MAX(TS.ITEM_NAME) ITEM_NAME
                            ,   MAX(TS.ITEM_SKU_ID)ITEM_SKU_ID
                            ,   MAX(TS.ITEM_COLOR_ID) ITEM_COLOR_ID
                            ,   MAX(MC.ITEM_COLOR_NAME) ITEM_COLOR_NAME
                            ,   MAX(TS.ITEM_SIZE_ID) ITEM_SIZE_ID
                            ,   MAX(MS.ITEM_SIZE_NAME) ITEM_SIZE_NAME
                            ,   TS.JAN
                            ,   MAX(TS.STORE_CLASS) STORE_CLASS
                            ,   NULL AS SHIP_TO_STORE_ID
                            ,   NULL AS SHIP_TO_STORE_NAME
                            ,   NULL AS TRANSPORTER_NAME
                            ,   COUNT(DISTINCT(TS.SHIP_TO_STORE_ID)) SHIP_TO_QTY
                            ,   SUM(TS.INSTRUCT_QTY) INSTRUCT_QTY
                            ,   MAX(TS.MAKE_DATE) ALLOC_DATE
                            ,   SUM(TS.ALLOC_QTY) ALLOC_QTY");
                        query.Append(", " + (condition.ResultType == ViewModels.BtoBInstructionInput.ResultTypes.Sku ? seq : condition.Seq) + " AS SEQ_PRE ");
                    }
                    else
                    {

                        query.Append(@"
                        WITH
                            TARGET_SHIP_DATA AS (
                                SELECT 
                                      TS.MAKE_DATE AS MAKE_DATE" +
                                        ", '" + Common.Profile.User.UserId + "' AS MAKE_USER_ID" +
                                        ",'BtoBInstructionInput'" + " AS MAKE_PROGRAM_NAME" +
                                        ",TS.MAKE_DATE " + "AS UPDATE_DATE" +
                                        ", '" + Common.Profile.User.UserId + "' AS UPDATE_USER_ID" +
                                        ",'BtoBInstructionInput'" + " AS UPDATE_PROGRAM_NAME" +
                                         ",0" + " AS UPDATE_COUNT" +
                                        "," + "'" + Common.Profile.User.ShipperId + "'" + " AS SHIPPER_ID" +
                                        "," + seq + " AS SEQ"
                            );
                        query.Append(@"
                            ,   TS.CENTER_ID
                            ,   TS.SHIP_PLAN_DATE
                            ,   MG1.GEN_NAME INSTRUCT_CLASS_NAME
                            ,   MG2.GEN_NAME EMERGENCY_CLASS_NAME
                            ,   TS.SHIP_INSTRUCT_ID
                            ,   TS.SHIP_INSTRUCT_SEQ
                            ,   TS.ITEM_ID
                            ,   TS.ITEM_NAME
                            ,   TS.ITEM_SKU_ID
                            ,   TS.ITEM_COLOR_ID
                            ,   MC.ITEM_COLOR_NAME
                            ,   TS.ITEM_SIZE_ID
                            ,   MS.ITEM_SIZE_NAME
                            ,   NULL AS JAN
                            ,   TS.STORE_CLASS STORE_CLASS
                            ,   TS.SHIP_TO_STORE_ID
                            ,   VSTS.SHIP_TO_STORE_NAME1 SHIP_TO_STORE_NAME
                            ,   MT.TRANSPORTER_NAME
                            ,   NULL AS SHIP_TO_QTY
                            ,   TS.INSTRUCT_QTY
                            ,   NULL AS ALLOC_DATE
                            ,   TS.ALLOC_QTY ");
                        query.Append(", " + (condition.ResultType == ViewModels.BtoBInstructionInput.ResultTypes.Sku ? seq : condition.Seq) + " AS SEQ_PRE ");
                    }
                    query.Append(@"
                        FROM
                                T_SHIPS TS
                        LEFT OUTER JOIN
                                M_ITEM_SKU MIS
                        ON
                                TS.SHIPPER_ID = MIS.SHIPPER_ID
                            AND TS.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                        LEFT OUTER JOIN
                                M_BRANDS MB
                        ON
                                MIS.SHIPPER_ID   = MB.SHIPPER_ID
                            AND MIS.BRAND_ID = MB.BRAND_ID
                        LEFT OUTER JOIN
                                M_VENDORS MV
                        ON
                                MIS.SHIPPER_ID   = MV.SHIPPER_ID
                            AND MIS.MAIN_VENDOR_ID = MV.VENDOR_ID
                        LEFT OUTER JOIN
                                V_SHIP_TO_STORES VSTS
                        ON
                                TS.SHIPPER_ID = VSTS.SHIPPER_ID
                            AND TS.SHIP_TO_STORE_ID  = VSTS.SHIP_TO_STORE_ID
                        LEFT OUTER JOIN
                                M_TRANSPORTERS MT
                        ON
                                TS.SHIPPER_ID   = MT.SHIPPER_ID
                            AND TS.TRANSPORTER_ID = MT.TRANSPORTER_ID
                        LEFT OUTER JOIN
                                M_COLORS MC
                        ON
                                TS.SHIPPER_ID   = MC.SHIPPER_ID
                            AND TS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                        LEFT OUTER JOIN
                                M_SIZES MS
                        ON
                                TS.SHIPPER_ID   = MS.SHIPPER_ID
                            AND TS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                        LEFT OUTER JOIN
                                M_GENERALS MG1
                        ON
                                TS.SHIPPER_ID = MG1.SHIPPER_ID
                            AND MG1.REGISTER_DIVI_CD = '1'
                            AND MG1.CENTER_ID = '@@@'
                            AND MG1.GEN_DIV_CD = 'INSTRUCT_CLASS'
                            AND TO_CHAR(TS.INSTRUCT_CLASS) = MG1.GEN_CD
                        LEFT OUTER JOIN
                                M_GENERALS MG2
                        ON
                                TS.SHIPPER_ID = MG2.SHIPPER_ID
                            AND MG2.REGISTER_DIVI_CD = '1'
                            AND MG2.CENTER_ID = '@@@'
                            AND MG2.GEN_DIV_CD = 'EMERGENCY_CLASS'
                            AND TO_CHAR(TS.EMERGENCY_CLASS) = MG2.GEN_CD
                        WHERE
                                TS.SHIPPER_ID = :SHIPPER_ID
                            AND TS.CENTER_ID = :CENTER_ID
                            AND TS.SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND TS.BATCH_NO <> ' '
                            AND VSTS.INSPECTION_MUST_FLAG <> 1
                    ");
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                    parameters.Add(":CENTER_ID", condition.CenterId);
                    parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId);

                    if (condition.ResultType == ViewModels.BtoBInstructionInput.ResultTypes.Sku)
                    {
                        query.Append(@"
                            GROUP BY
                                    TS.CENTER_ID
                                ,   TS.SHIP_PLAN_DATE
                                ,   TS.SHIP_INSTRUCT_ID
                                ,   TS.JAN ");
                    }
                    query.Append(@"
                        )
                        SELECT 
                                MAKE_DATE
                            ,   MAKE_USER_ID 
                            ,   MAKE_PROGRAM_NAME
                            ,   UPDATE_DATE 
                            ,   UPDATE_USER_ID
                            ,   UPDATE_PROGRAM_NAME
                            ,   UPDATE_COUNT
                            ,   SHIPPER_ID
                            ,   SEQ
                            ,   ROWNUM LINE_NO
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
                            ,   STORE_CLASS
                            ,   SHIP_TO_STORE_ID
                            ,   SHIP_TO_STORE_NAME
                            ,   TRANSPORTER_NAME
                            ,   SHIP_TO_QTY
                            ,   INSTRUCT_QTY
                            ,   ALLOC_DATE
                            ,   ALLOC_QTY
                            ,   SEQ_PRE
                        FROM
                                TARGET_SHIP_DATA
                    ");

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
        public IPagedList<BtoBInstructionInput01ResultRow> BtoBInstructionInput01GetData(BtoBInstructionInput01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            // SKU一覧
            if (condition.ResultType == ViewModels.BtoBInstructionInput.ResultTypes.Sku)
            {
                StringBuilder query = new StringBuilder(@"
                SELECT *
                  FROM WW_SHP_BTO_B_RESULT_INPUT
                 WHERE SHIPPER_ID = :SHIPPER_ID
                   AND SEQ = :SEQ ");
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":SEQ", condition.Seq);

                // 全レコード数を取得
                int totalCount = MvcDbContext.Current.Database.Connection.Query<BtoBInstructionInput01ResultRow>(query.ToString(), parameters).Count();

                // Sort function
                switch (condition.SortKey)
                {
                    case BtoBInstructionInput01SortKey.SkuInstructId:
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
                var Inputs = MvcDbContext.Current.Database.Connection.Query<BtoBInstructionInput01ResultRow>(query.ToString(), parameters);
                var shpBtoBInstructionInput = MvcDbContext.Current.ShpBtoBInstructionInputs.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId);
                condition.ItemSkuSum = shpBtoBInstructionInput.Select(x => x.ItemSkuId).Distinct().Count();
                condition.InstructQtySum = shpBtoBInstructionInput.Select(x => x.InstructQty).Sum();
                condition.AllocQtySum = shpBtoBInstructionInput.Select(x => x.InstructQty).Sum();

                // Excute paging
                return new StaticPagedList<BtoBInstructionInput01ResultRow>(Inputs, condition.Page, condition.PageSize, totalCount);
            }

            // 指示明細
            else
            {
                StringBuilder query = new StringBuilder(@"
                SELECT *
                  FROM WW_SHP_BTO_B_RESULT_INPUT
                 WHERE SHIPPER_ID = :SHIPPER_ID
                   AND SEQ = :SEQ ");
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":SEQ", condition.Seq);

                // 全レコード数を取得
                int totalCount = MvcDbContext.Current.Database.Connection.Query<BtoBInstructionInput01ResultRow>(query.ToString(), parameters).Count();

                // Sort function
                switch (condition.DetailSortKey)
                {
                    case BtoBInstructionInputDetailSortKey.ShipToStoreInstructIdSeq:
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

                    case BtoBInstructionInputDetailSortKey.SkuInstructIdSeq:
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
                var Inputs = MvcDbContext.Current.Database.Connection.Query<BtoBInstructionInput01ResultRow>(query.ToString(), parameters);
                var shpBtoBInstructionInput = MvcDbContext.Current.ShpBtoBInstructionInputs.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId);
                condition.ItemSkuSum = shpBtoBInstructionInput.Select(x => x.ItemSkuId).Distinct().Count();
                condition.InstructQtySum = shpBtoBInstructionInput.Select(x => x.InstructQty).Sum();
                condition.AllocQtySum = shpBtoBInstructionInput.Select(x => x.InstructQty).Sum();

                // Excute paging
                return new StaticPagedList<BtoBInstructionInput01ResultRow>(Inputs, condition.Page, condition.PageSize, totalCount);
            }
        }
        /// <summary>
        /// 出荷実績一括入力
        /// </summary>
        public void Input(BtoBInstructionInput01SearchConditions conditions, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", conditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_WORK_ID", conditions.Seq, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_RESULT_INPUT",
                param,
                commandType: CommandType.StoredProcedure);
            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }

        /// <summary>
        /// レーン仕分実績登録チェック
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public int LaneResultCheck(long seq)
        {
            int status = 0;
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
                WITH
                    TARGET_INSTRUCT_ID AS (
                        SELECT
                                SHIP_INSTRUCT_ID
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                        FROM
                                WW_SHP_BTO_B_RESULT_INPUT
                        WHERE
                                SEQ = :WORK_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                        GROUP BY
                                SHIP_INSTRUCT_ID
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                )
                ,   TARGET_SHIP_DATA AS (
                        SELECT
                                SHIP.BATCH_NO
                            ,   SHIP.ITEM_SKU_ID
                            ,   SHIP.CENTER_ID
                            ,   SHIP.SHIPPER_ID
                        FROM
                                T_SHIPS SHIP
                        INNER JOIN
                                TARGET_INSTRUCT_ID TGT
                        ON
                                SHIP.SHIP_INSTRUCT_ID = TGT.SHIP_INSTRUCT_ID
                            AND SHIP.CENTER_ID = TGT.CENTER_ID
                            AND SHIP.SHIPPER_ID = TGT.SHIPPER_ID
                        LEFT OUTER JOIN
                                V_SHIP_TO_STORES STR
                        ON
                                STR.SHIP_TO_STORE_ID = SHIP.SHIP_TO_STORE_ID
                            AND STR.SHIPPER_ID = SHIP.SHIPPER_ID
                        WHERE
                                STR.INSPECTION_MUST_FLAG <> 1
                        GROUP BY
                                SHIP.BATCH_NO
                            ,   SHIP.ITEM_SKU_ID
                            ,   SHIP.CENTER_ID
                            ,   SHIP.SHIPPER_ID
                )
                SELECT
                        COUNT(*)
                FROM
                        T_LANE_SORT LANE
                INNER JOIN
                        TARGET_SHIP_DATA TGT
                ON
                        LANE.BATCH_NO = TGT.BATCH_NO
                    AND LANE.ITEM_SKU_ID = TGT.ITEM_SKU_ID
                    AND LANE.CENTER_ID = TGT.CENTER_ID
                    AND LANE.SHIPPER_ID = TGT.SHIPPER_ID
                WHERE
                        LANE.SORT_QTY > 0
                    OR  LANE.SORT_STATUS <> 0
            ");
            parameters.AddDynamicParams(new { WORK_ID = seq });
            parameters.AddDynamicParams(new { SHIPPER_ID = Profile.User.ShipperId });
            var laneResultData = MvcDbContext.Current.Database.Connection.Query<int>(query.ToString(), parameters).SingleOrDefault();
            if (laneResultData > 0)
            {
                status = 1;
            }
            return status;
        }
    }
}