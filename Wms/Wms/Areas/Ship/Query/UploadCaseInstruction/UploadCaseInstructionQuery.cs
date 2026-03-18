namespace Wms.Areas.Ship.Query.UploadCaseInstruction
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using Wms.Areas.Ship.Models;
    using PagedList;
    using Share.Common;
    using Share.Extensions.Classes;
    using Share.Helpers;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.UploadCaseInstruction;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.UploadCaseInstruction.UploadCaseInstructionSearchConditions;
    using Wms.Areas.Master.Models;
    using System;

    public class UploadCaseInstructionQuery : BaseQuery
    {
        /// <summary>
        /// ワークテーブル作成
        /// </summary>
        public bool InsertCaseWk(UploadCaseInstructionSearchConditions condition)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    long workId = GetWorkId();
                    condition.Seq = workId;
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                    INSERT INTO WW_SHP_CASE_INSTRUCTION2(
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
                        ,   SHIP_KIND
                        ,   SHIP_INSTRUCT_NAME
                        ,   SHIP_INSTRUCT_DATE
                        ,   BATCH_NO
                        ,   DETAIL_ROW_QTY
                        ,   CASE_QTY
                        ,   STORE_QTY
                        ,   PIC_JAN_QTY
                        ,   PIC_INS_QTY
                        ,   HIKI_ERR_QTY
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
                        ,   TAI.SHIP_KIND
                        ,   TAI.BATCH_NAME AS IMPORT_CASE_NAME
                        ,   TAI.MAKE_DATE AS IMPORT_CASE_DATE
                        ,   TAI.ALLOC_NO AS BATCH_NO
                        ,   TS.BATCH_NO_CNT
                        ,   TS.BOX_NO_CNT
                        ,   TS.SHIP_TO_STORE_ID_CNT
                        ,   TS.ITEM_SKU_ID_CNT
                        ,   TS.INSTRUCT_QTY_SUM
                        ,   TS.ALLOC_QTY_ERR
                    FROM
                            T_ALLOC_INFO TAI
                    INNER JOIN
                        (   SELECT
                                    COUNT(BATCH_NO) AS BATCH_NO_CNT
                                ,   COUNT(DISTINCT SHIP_TO_STORE_ID) AS SHIP_TO_STORE_ID_CNT
                                ,   CASE MAX(SHIP_KIND)
                                        WHEN 4 THEN COUNT(DISTINCT BOX_NO)
                                        ELSE 0
                                    END BOX_NO_CNT
                                ,   COUNT(DISTINCT ITEM_SKU_ID) AS ITEM_SKU_ID_CNT
                                ,   SUM(INSTRUCT_QTY) AS INSTRUCT_QTY_SUM
                                ,   SUM(INSTRUCT_QTY) -SUM(ALLOC_QTY) AS ALLOC_QTY_ERR
                                ,   SHIPPER_ID
                                ,   CENTER_ID
                                ,   BATCH_NO
                            FROM
                                    T_SHIPS
                            WHERE
                                    SHIPPER_ID = :SHIPPER_ID
                                AND CENTER_ID = :CENTER_ID
                            GROUP BY
                                    SHIPPER_ID
                                ,   CENTER_ID
                                ,   BATCH_NO
                        )   TS
                    ON
                            TAI.SHIPPER_ID = TS.SHIPPER_ID
                        AND TAI.CENTER_ID = TS.CENTER_ID
                        AND TAI.ALLOC_NO = TS.BATCH_NO
                    WHERE
                            TAI.SHIPPER_ID = :SHIPPER_ID
                        AND TAI.CENTER_ID = :CENTER_ID
                        AND TAI.SHIP_KIND IN (4,5)
                    ");
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":USER_ID", Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "UploadCaseInstruction");
                    parameters.Add(":CENTER_ID", condition.CenterId);
                    parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

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
        /// ワークテーブルデータ取得
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<UploadCaseInstructionResultRow> GetCaseWkData(UploadCaseInstructionSearchConditions condition)
        {
            string order;
            switch (condition.Key)
            {
                case SortKey.ShipKindBatchNo:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            order = "ORDER BY WSCI2.SHIP_KIND DESC,WSCI2.BATCH_NO DESC";
                            break;

                        default:
                            order = "ORDER BY WSCI2.SHIP_KIND ASC,WSCI2.BATCH_NO ASC";
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            order = "ORDER BY WSCI2.BATCH_NO DESC";
                            break;

                        default:
                            order = "ORDER BY WSCI2.BATCH_NO ASC";
                            break;
                    }

                    break;
            }

            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT
                        ROW_NUMBER() OVER (");
            query.AppendLine(order);
            query.AppendLine(@"
                        ) AS LINE_NO
                    ,   WSCI2.BATCH_NO
                    ,   TO_CHAR(WSCI2.SHIP_INSTRUCT_DATE,'YYYY/MM/DD HH24:MI:SS') AS SHIP_INSTRUCT_DATE
                    ,   WSCI2.SHIP_INSTRUCT_NAME
                    ,   MG.GEN_NAME AS SHIP_KIND_NAME
                    ,   NVL(WSCI2.DETAIL_ROW_QTY,0) DETAIL_ROW_QTY
                    ,   NVL(WSCI2.CASE_QTY,0) CASE_QTY
                    ,   NVL(WSCI2.STORE_QTY,0) STORE_QTY
                    ,   NVL(WSCI2.PIC_JAN_QTY,0) PIC_JAN_QTY
                    ,   NVL(WSCI2.PIC_INS_QTY,0) PIC_INS_QTY
                    ,   NVL(WSCI2.HIKI_ERR_QTY,0) HIKI_ERR_QTY
                FROM
                        WW_SHP_CASE_INSTRUCTION2 WSCI2
                LEFT OUTER JOIN
                        M_GENERALS MG
                ON
                        MG.REGISTER_DIVI_CD = '1'
                    AND MG.GEN_DIV_CD = 'CASE_SHIP_KIND'
                    AND MG.GEN_CD = WSCI2.SHIP_KIND
                    AND MG.CENTER_ID = '@@@'
                    AND MG.SHIPPER_ID = WSCI2.SHIPPER_ID
                WHERE
                        WSCI2.SHIPPER_ID = :SHIPPER_ID
                    AND WSCI2.CENTER_ID  = :CENTER_ID
                    AND WSCI2.SEQ = :SEQ
            ");
            query.AppendLine(order);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SEQ", condition.Seq);


            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<UploadCaseInstructionResultRow>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var UploadCaseInstructions = MvcDbContext.Current.Database.Connection.Query<UploadCaseInstructionResultRow>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<UploadCaseInstructionResultRow>(UploadCaseInstructions, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// ケース出荷詳細データ取得
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<UploadCaseDetailsResultRow> GetCaseDetailData(UploadCaseInstructionSearchConditions condition)
        {
            string order;
            switch (condition.DetailCaseKey)
            {
                case DetailCaseSortKey.ShipToStoreCaseNo:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            order = "ORDER BY MAX(TS.SHIP_TO_STORE_ID) DESC,TS.BOX_NO DESC";
                            break;

                        default:
                            order = "ORDER BY MAX(TS.SHIP_TO_STORE_ID) ASC,TS.BOX_NO ASC";
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            order = "ORDER BY TS.BOX_NO DESC,MAX(TS.SHIP_TO_STORE_ID) DESC";
                            break;

                        default:
                            order = "ORDER BY TS.BOX_NO ASC,MAX(TS.SHIP_TO_STORE_ID) ASC";
                            break;
                    }

                    break;
            }
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT
                        SUB.*
                FROM (
                        SELECT
                                MAX(TO_CHAR(TS.SHIP_PLAN_DATE,'YYYY/MM/DD')) AS SHIP_PLAN_DATE
                            ,   TS.BOX_NO
                            ,   MAX(TS.SHIP_TO_STORE_ID) AS SHIP_TO_STORE_ID
                            ,   MAX(MS.SHIP_TO_STORE_NAME1) AS STORE_NAME
                            ,   MAX(TS.PRIORITY_ORDER) AS PRIORITY_ORDER
                            ,   COUNT (DISTINCT TS.ITEM_SKU_ID) AS SKU_QTY
                            ,   SUM(TS.INSTRUCT_QTY) AS INSTRUCT_QTY
                            ,   ROW_NUMBER() OVER(");
            query.AppendLine(order);
            query.AppendLine(@"                    ) AS LINE_NO
                        FROM
                                T_SHIPS TS
                        INNER JOIN
                                V_SHIP_TO_STORES MS
                        ON
                                TS.SHIPPER_ID = MS.SHIPPER_ID
                            AND TS.SHIP_TO_STORE_ID = MS.SHIP_TO_STORE_ID
                        WHERE
                                TS.SHIPPER_ID = :SHIPPER_ID
                            AND TS.CENTER_ID = :CENTER_ID
                            AND TS.BATCH_NO = :BATCH_NO
                        GROUP BY
                                TS.BOX_NO
                ) SUB
                ORDER BY SUB.LINE_NO
            ");

            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.HidCenterId);
            parameters.Add(":BATCH_NO", condition.HidBatchNo);


            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<UploadCaseInstructionResultRow>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.HidPageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.HidPage - 1) * condition.HidPageSize });

            // Fill data to memory
            var UploadCaseInstructions = MvcDbContext.Current.Database.Connection.Query<UploadCaseDetailsResultRow>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<UploadCaseDetailsResultRow>(UploadCaseInstructions, condition.HidPage, condition.HidPageSize, totalCount);
        }

        /// <summary>
        /// JAN抜き取り詳細データ取得
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<UploadJanDetailsResultRow> GetJanDetailData(UploadCaseInstructionSearchConditions condition)
        {
            string order;
            switch (condition.DetailJanKey)
            {
                case DetailJanSortKey.ShipToStoreJan:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            order = "ORDER BY TS.SHIP_TO_STORE_ID DESC,TS.JAN DESC,TS.SHIP_INSTRUCT_ID DESC,TS.SHIP_INSTRUCT_SEQ DESC";
                            break;

                        default:
                            order = "ORDER BY TS.SHIP_TO_STORE_ID ASC,TS.JAN ASC,TS.SHIP_INSTRUCT_ID ASC,TS.SHIP_INSTRUCT_SEQ ASC";
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            order = "ORDER BY TS.JAN DESC,TS.SHIP_TO_STORE_ID DESC,TS.SHIP_INSTRUCT_ID DESC,TS.SHIP_INSTRUCT_SEQ DESC";
                            break;

                        default:
                            order = "ORDER BY TS.JAN ASC,TS.SHIP_TO_STORE_ID ASC,TS.SHIP_INSTRUCT_ID ASC,TS.SHIP_INSTRUCT_SEQ ASC";
                            break;
                    }

                    break;
            }
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT
                        TO_CHAR(TS.SHIP_PLAN_DATE,'YYYY/MM/DD') AS SHIP_PLAN_DATE
                    ,   TS.ITEM_ID
                    ,   TS.ITEM_NAME
                    ,   TS.ITEM_COLOR_ID
                    ,   MC.ITEM_COLOR_NAME
                    ,   TS.ITEM_SIZE_ID
                    ,   MIS.ITEM_SIZE_NAME
                    ,   TS.JAN
                    ,   TS.SHIP_TO_STORE_ID
                    ,   MS.SHIP_TO_STORE_NAME1 AS STORE_NAME
                    ,   TS.PRIORITY_ORDER
                    ,   TS.INSTRUCT_QTY
                    ,   TS.ALLOC_QTY
                    ,   ROW_NUMBER() OVER(");
            query.AppendLine(order);
            query.AppendLine(@"           ) AS LINE_NO
                FROM
                        T_SHIPS TS
                INNER JOIN
                        V_SHIP_TO_STORES MS
                ON
                        TS.SHIPPER_ID = MS.SHIPPER_ID
                    AND TS.SHIP_TO_STORE_ID = MS.SHIP_TO_STORE_ID
                LEFT OUTER JOIN
                        M_COLORS MC
                ON
                        TS.SHIPPER_ID = MC.SHIPPER_ID
                    AND TS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                LEFT OUTER JOIN
                        M_ITEM_SKU MIS
                ON
                        TS.SHIPPER_ID = MIS.SHIPPER_ID
                    AND TS.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                WHERE
                        TS.SHIPPER_ID = :SHIPPER_ID
                    AND TS.CENTER_ID = :CENTER_ID
                    AND TS.BATCH_NO = :BATCH_NO
            ");
            query.AppendLine(order);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.HidCenterId);
            parameters.Add(":BATCH_NO", condition.HidBatchNo);


            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<UploadJanDetailsResultRow>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.HidPageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.HidPage - 1) * condition.HidPageSize });

            // Fill data to memory
            var UploadCaseInstructions = MvcDbContext.Current.Database.Connection.Query<UploadJanDetailsResultRow>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<UploadJanDetailsResultRow>(UploadCaseInstructions, condition.HidPage, condition.HidPageSize, totalCount);
        }

        /// <summary>
        /// 引当ストアド実行処理
        /// </summary>
        public void InsertUploadCaseInstructionIns(UploadCaseInstructionSearchConditions searchConditions, out ProcedureStatus status, out string message)
        {
            int shipKind;
            if (searchConditions.HidShipKind == UploadCaseInstructionSearchConditions.ShipKinds.KindCase)
            {
                shipKind = 4;
            }
            else{
                shipKind = 5;
            }

            //int saleClass = 1;

            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", searchConditions.HidCenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_BATCH_NAME", searchConditions.HidCaseShipName, DbType.String, ParameterDirection.Input);
            param.Add("IN_WORK_ID", searchConditions.Seq, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_SHIP_KIND", shipKind, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_SALES_CLASS", searchConditions.HidSaleClass, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_OFF_RATE", searchConditions.HidDiscount, DbType.Int32, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
            "SP_W_SHP_CASE_INSTRUCTION",
            param,
            commandType: CommandType.StoredProcedure);
            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }

        /// <summary>
        /// 引当解除・削除ストアド実行処理
        /// </summary>
        public void DeleteCaseInstructionIns(UploadCaseInstructionSearchConditions searchConditions, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", searchConditions.HidCenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_BATCH_NO", searchConditions.HidBatchNo, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
            "SP_W_SHP_CASEALLOC_CANCEL",
            param,
            commandType: CommandType.StoredProcedure);
            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }

        /// <summary>
        /// 引当進捗ワークデータ取得
        /// </summary>
        /// <returns>引当進捗ワークデータ</returns>
        public AllocStatus GetAllocStatus(long Seq)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@" select * from W_ALLOC_STATUS where SHIPPER_ID = :SHIPPER_ID and SEQ = :SEQ
                ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":SEQ", Seq);
            var allocStatus = MvcDbContext.Current.Database.Connection.Query<AllocStatus>(query.ToString(), parameters).FirstOrDefault();
            return allocStatus;
        }

        /// <summary>
        /// 売上区分コードデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListSaleClasses()
        {
            return MvcDbContext.Current.Generals
                .Where(m => m.ShipperId == Profile.User.ShipperId && m.CenterId == "@@@" && m.RegisterDiviCd == "1" && m.GenDivCd == "SALE_CLASS")
                .OrderBy(m => m.OrderNo)
                .Select(m => new SelectListItem
                {
                    Value = m.GenCd,
                    Text = m.GenName
                });
        }
    }
}