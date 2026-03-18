namespace Wms.Areas.Others.Query.WorkingReference
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using Wms.Areas.Others.Models;
    using PagedList;
    using Share.Common;
    using Share.Extensions.Classes;
    using Share.Helpers;
    using Wms.Areas.Others.Resources;
    using Wms.Areas.Others.ViewModels.WorkingReference;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Others.ViewModels.WorkingReference.WorkingReferenceSearchConditions;
    using Wms.Areas.Master.Models;
    using System;

    public class WorkingReferenceQuery : BaseQuery
    {
        /// <summary>
        /// スマホ仕入入荷計上ワークデータ作成
        /// </summary>
        public bool InsertCaseWk(WorkingReferenceSearchConditions condition)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    long workId = GetWorkId();
                    condition.Seq = workId;
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                    INSERT INTO WW_OTH_WORKING(
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
                        ,   INVOICE_NO
                        ,   START_DATE
                        ,   WORK_USER_ID
                        ,   WORK_USER_NAME
                        ,   TERMINAL_INFO
                        ,   ITEM_SKU_ID
                        ,   ITEM_ID
                        ,   ITEM_NAME
                        ,   ITEM_COLOR_ID
                        ,   ITEM_COLOR_NAME
                        ,   ITEM_SIZE_ID
                        ,   ITEM_SIZE_NAME
                        ,   JAN
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
                        ,   WH.INVOICE_NO
                        ,   WH.MAKE_DATE
                        ,   WH.MAKE_USER_ID
                        ,   MU.USER_NAME
                        ,   WH.TERMINAL_INFO
                        ,   WH.ITEM_SKU_ID
                        ,   MIS.ITEM_ID
                        ,   MIS.ITEM_NAME
                        ,   MIS.ITEM_COLOR_ID
                        ,   MC.ITEM_COLOR_NAME
                        ,   MIS.ITEM_SIZE_ID
                        ,   MS.ITEM_SIZE_NAME
                        ,   WH.JAN
                    FROM
                            WW_HHT_ARRIVE_SORTS_EX WH
                    INNER JOIN
                            M_USERS MU
                    ON
                            MU.SHIPPER_ID = WH.SHIPPER_ID
                        AND MU.USER_ID = WH.MAKE_USER_ID
                    INNER JOIN
                            M_ITEM_SKU MIS
                    ON
                            MIS.SHIPPER_ID = WH.SHIPPER_ID
                        AND MIS.ITEM_SKU_ID = WH.ITEM_SKU_ID
                    INNER JOIN
                            M_COLORS MC
                    ON
                            MC.SHIPPER_ID = MIS.SHIPPER_ID
                        AND MC.ITEM_COLOR_ID = MIS.ITEM_COLOR_ID
                    INNER JOIN
                            M_SIZES MS
                    ON
                            MS.SHIPPER_ID = MIS.SHIPPER_ID
                        AND MS.ITEM_SIZE_ID = MIS.ITEM_SIZE_ID
                    WHERE
                            WH.SHIPPER_ID = :SHIPPER_ID
                        AND WH.CENTER_ID = :CENTER_ID
                    ");
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":USER_ID", Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "WorkingReference");
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
        /// スマホ移動入荷検品ワークデータ作成
        /// </summary>
        public bool InsertIdoWk(WorkingReferenceSearchConditions condition)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    long workId = GetWorkId();
                    condition.Seq = workId;
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                    INSERT INTO WW_OTH_WORKING(
                            MAKE_DATE
                        ,   MAKE_USER_ID
                        ,   MAKE_PROGRAM_NAME
                        ,   UPDATE_DATE
                        ,   UPDATE_USER_ID
                        ,   UPDATE_PROGRAM_NAME
                        ,   UPDATE_COUNT
                        ,   SHIPPER_ID
                        ,   SEQ
                        ,   HAITA_SEQ
                        ,   LINE_NO
                        ,   CENTER_ID
                        ,   START_DATE
                        ,   WORK_USER_ID
                        ,   WORK_USER_NAME
                        ,   SLIP_NO
                        ,   TRANSFER_CLASS
                        ,   TRANS_FROM_STORE_CLASS
                        ,   TRANSFER_FROM_STORE_NAME
                        ,   ARRIVE_PLAN_QTY
                        ,   RESULT_QTY
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
                        ,   SEQ
                        ,   ROWNUM
                        ,   :CENTER_ID
                        ,   MAKE_DATE
                        ,   MAKE_USER_ID
                        ,   USER_NAME
                        ,   SLIP_NO
                        ,   TRANSFER_CLASS
                        ,   TRANS_FROM_STORE_CLASS
                        ,   TRANSFER_FROM_STORE_NAME
                        ,   ARRIVE_PLAN_QTY
                        ,   RESULT_QTY
                    FROM (
                        SELECT
                                MAX(WH.SEQ) AS SEQ
                            ,   MIN(WH.MAKE_DATE) AS MAKE_DATE
                            ,   MAX(WH.MAKE_USER_ID) AS MAKE_USER_ID
                            ,   MAX(MU.USER_NAME) AS USER_NAME
                            ,   WH.SLIP_NO
                            ,   MAX(WH.TRANSFER_CLASS) AS TRANSFER_CLASS
                            ,   MAX(WH.TRANS_FROM_STORE_CLASS) AS TRANS_FROM_STORE_CLASS
                            ,   MAX(WH.TRANSFER_FROM_STORE_NAME) AS TRANSFER_FROM_STORE_NAME
                            ,   SUM(WH.ARRIVE_PLAN_QTY) AS ARRIVE_PLAN_QTY
                            ,   SUM(WH.RESULT_QTY) AS RESULT_QTY
                        FROM
                                W_HHT_TRANSFER WH
                        INNER JOIN
                                M_USERS MU
                        ON
                                MU.SHIPPER_ID = WH.SHIPPER_ID
                            AND MU.USER_ID = WH.MAKE_USER_ID
                        WHERE
                                WH.SHIPPER_ID = :SHIPPER_ID
                            AND WH.CENTER_ID = :CENTER_ID
                        GROUP BY
                                WH.SHIPPER_ID
                            ,   WH.CENTER_ID
                            ,   WH.SLIP_NO
                    )
                    ");
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":USER_ID", Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "WorkingReference");
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
        /// スマホトータルピック中ワークデータ作成
        /// </summary>
        public bool InsertPicWk(WorkingReferenceSearchConditions condition)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    long workId = GetWorkId();
                    condition.Seq = workId;
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                    INSERT INTO WW_OTH_WORKING(
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
                        ,   START_DATE
                        ,   WORK_USER_ID
                        ,   WORK_USER_NAME
                        ,   LOCATION_CD
                        ,   HIKI_QTY
                        ,   RESULT_QTY
                        ,   ITEM_SKU_ID
                        ,   ITEM_ID
                        ,   ITEM_NAME
                        ,   ITEM_COLOR_ID
                        ,   ITEM_COLOR_NAME
                        ,   ITEM_SIZE_ID
                        ,   ITEM_SIZE_NAME
                        ,   JAN
                        ,   BATCH_NO
                        ,   PICK_KIND
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
                        ,   TP.PIC_DATE
                        ,   TP.PIC_USER_ID
                        ,   MU.USER_NAME
                        ,   TP.LOCATION_CD
                        ,   TP.HIKI_QTY
                        ,   TP.PIC_QTY
                        ,   TP.ITEM_SKU_ID
                        ,   TP.ITEM_ID
                        ,   TP.ITEM_NAME
                        ,   TP.ITEM_COLOR_ID
                        ,   MC.ITEM_COLOR_NAME
                        ,   TP.ITEM_SIZE_ID
                        ,   MS.ITEM_SIZE_NAME
                        ,   TP.JAN
                        ,   TP.BATCH_NO
                        ,   TP.PICK_KIND
                    FROM
                            T_PIC TP
                    INNER JOIN
                            M_USERS MU
                    ON
                            MU.SHIPPER_ID = TP.SHIPPER_ID
                        AND MU.USER_ID = TP.PIC_USER_ID
                    INNER JOIN
                            M_ITEM_SKU MIS
                    ON
                            MIS.SHIPPER_ID = TP.SHIPPER_ID
                        AND MIS.ITEM_SKU_ID = TP.ITEM_SKU_ID
                    INNER JOIN
                            M_COLORS MC
                    ON
                            MC.SHIPPER_ID = TP.SHIPPER_ID
                        AND MC.ITEM_COLOR_ID = TP.ITEM_COLOR_ID
                    INNER JOIN
                            M_SIZES MS
                    ON
                            MS.SHIPPER_ID = TP.SHIPPER_ID
                        AND MS.ITEM_SIZE_ID = TP.ITEM_SIZE_ID
                    WHERE
                            TP.SHIPPER_ID = :SHIPPER_ID
                        AND TP.CENTER_ID = :CENTER_ID
                        AND TP.PIC_STATUS = 1
                    ");
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":USER_ID", Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "WorkingReference");
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
        /// スマホ店別仕分ワークデータ作成
        /// </summary>
        public bool InsertStoreWk(WorkingReferenceSearchConditions condition)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    long workId = GetWorkId();
                    condition.Seq = workId;
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                    INSERT INTO WW_OTH_WORKING(
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
                        ,   START_DATE
                        ,   WORK_USER_ID
                        ,   WORK_USER_NAME
                        ,   HIKI_QTY
                        ,   RESULT_QTY
                        ,   ITEM_SKU_ID
                        ,   ITEM_ID
                        ,   ITEM_NAME
                        ,   ITEM_COLOR_ID
                        ,   ITEM_COLOR_NAME
                        ,   ITEM_SIZE_ID
                        ,   ITEM_SIZE_NAME
                        ,   JAN
                        ,   BATCH_NO
                        ,   LANE_NO
                        ,   FRONTAGE_NO
                        ,   SHIP_TO_STORE_ID
                        ,   STORE_NAME
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
                        ,   TSS.SORT_DATE
                        ,   TSS.SORT_USER_ID
                        ,   MU.USER_NAME
                        ,   TSS.ALLOC_QTY
                        ,   TSS.SORT_QTY
                        ,   TSS.ITEM_SKU_ID
                        ,   TSS.ITEM_ID
                        ,   TSS.ITEM_NAME
                        ,   TSS.ITEM_COLOR_ID
                        ,   MC.ITEM_COLOR_NAME
                        ,   TSS.ITEM_SIZE_ID
                        ,   MS.ITEM_SIZE_NAME
                        ,   TSS.JAN
                        ,   TSS.BATCH_NO
                        ,   TSS.LANE_NO
                        ,   TSS.FRONTAGE_NO
                        ,   TSS.SHIP_TO_STORE_ID
                        ,   MSS.STORE_NAME1
                    FROM
                            T_STORE_SORT TSS
                    INNER JOIN
                            M_USERS MU
                    ON
                            MU.SHIPPER_ID = TSS.SHIPPER_ID
                        AND MU.USER_ID = TSS.SORT_USER_ID
                    INNER JOIN
                            M_ITEM_SKU MIS
                    ON
                            MIS.SHIPPER_ID = TSS.SHIPPER_ID
                        AND MIS.ITEM_SKU_ID = TSS.ITEM_SKU_ID
                    INNER JOIN
                            M_COLORS MC
                    ON
                            MC.SHIPPER_ID = TSS.SHIPPER_ID
                        AND MC.ITEM_COLOR_ID = TSS.ITEM_COLOR_ID
                    INNER JOIN
                            M_SIZES MS
                    ON
                            MS.SHIPPER_ID = TSS.SHIPPER_ID
                        AND MS.ITEM_SIZE_ID = TSS.ITEM_SIZE_ID
                    INNER JOIN
                            M_STORES MSS
                    ON
                            MSS.SHIPPER_ID = TSS.SHIPPER_ID
                        AND MSS.STORE_ID = TSS.SHIP_TO_STORE_ID
                    WHERE
                            TSS.SHIPPER_ID = :SHIPPER_ID
                        AND TSS.CENTER_ID = :CENTER_ID
                        AND TSS.SORT_STATUS IN (1,2)
                    ");
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":USER_ID", Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "WorkingReference");
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
        /// スマホ在庫仕分ワークデータ作成
        /// </summary>
        public bool InsertStockWk(WorkingReferenceSearchConditions condition)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    long workId = GetWorkId();
                    condition.Seq = workId;
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                    INSERT INTO WW_OTH_WORKING(
                            MAKE_DATE
                        ,   MAKE_USER_ID
                        ,   MAKE_PROGRAM_NAME
                        ,   UPDATE_DATE
                        ,   UPDATE_USER_ID
                        ,   UPDATE_PROGRAM_NAME
                        ,   UPDATE_COUNT
                        ,   SHIPPER_ID
                        ,   SEQ
                        ,   HAITA_SEQ
                        ,   LINE_NO
                        ,   CENTER_ID
                        ,   START_DATE
                        ,   WORK_USER_ID
                        ,   WORK_USER_NAME
                        ,   BOX_NO
                        ,   SKU_QTY
                        ,   SORT_PLAN_QTY
                        ,   SORT_RESULT_QTY
                        ,   TERMINAL_INFO
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
                        ,   SEQ
                        ,   ROWNUM
                        ,   :CENTER_ID
                        ,   MAKE_DATE
                        ,   MAKE_USER_ID
                        ,   USER_NAME
                        ,   BOX_NO
                        ,   SKU_QTY
                        ,   SORT_PLAN_QTY
                        ,   SORT_RESULT_QTY
                        ,   TERMINAL_INFO
                    FROM (
                        SELECT
                                WH.SEQ
                            ,   MIN(WH.MAKE_DATE) AS MAKE_DATE
                            ,   MAX(WH.MAKE_USER_ID) AS MAKE_USER_ID
                            ,   MAX(MU.USER_NAME) AS USER_NAME
                            ,   MAX(WH.BOX_NO_FROM) AS BOX_NO
                            ,   COUNT(DISTINCT WH.ITEM_SKU_ID) AS SKU_QTY
                            ,   SUM(WH.SORT_PLAN_QTY) AS SORT_PLAN_QTY
                            ,   SUM(WH.SORT_RESULT_QTY) AS SORT_RESULT_QTY
                            ,   MAX(TERMINAL_INFO) AS TERMINAL_INFO
                        FROM
                                WW_HHT_STOCK_SORTS WH
                        INNER JOIN
                                M_USERS MU
                        ON
                                MU.SHIPPER_ID = WH.SHIPPER_ID
                            AND MU.USER_ID = WH.MAKE_USER_ID
                        WHERE
                                WH.SHIPPER_ID = :SHIPPER_ID
                            AND WH.CENTER_ID = :CENTER_ID
                        GROUP BY
                                WH.SHIPPER_ID
                            ,   WH.CENTER_ID
                            ,   WH.SEQ
                    )
                    ");
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":USER_ID", Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "WorkingReference");
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
        /// スマホ仕入入荷計上ワークデータ取得
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<WorkingReferenceResultRow> GetShiireWkData(WorkingReferenceSearchConditions condition)
        {
            string order;
            switch (condition.ShiireKey)
            {
                case ShiireSortKey.StartDateWorkingUser:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            order = "ORDER BY WOW.START_DATE DESC,WOW.WORK_USER_ID DESC";
                            break;

                        default:
                            order = "ORDER BY WOW.START_DATE ASC,WOW.WORK_USER_ID ASC";
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            order = "ORDER BY WOW.INVOICE_NO DESC,WOW.ITEM_SKU_ID DESC";
                            break;

                        default:
                            order = "ORDER BY WOW.INVOICE_NO ASC,WOW.ITEM_SKU_ID ASC";
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
                    ,   TO_CHAR(WOW.START_DATE,'YYYY/MM/DD HH24:MI:SS') AS START_DATE
                    ,   WOW.WORK_USER_ID
                    ,   WOW.WORK_USER_NAME
                    ,   WOW.TERMINAL_INFO
                    ,   WOW.ITEM_SKU_ID
                    ,   WOW.ITEM_ID
                    ,   WOW.ITEM_NAME
                    ,   WOW.ITEM_COLOR_ID
                    ,   WOW.ITEM_COLOR_NAME
                    ,   WOW.ITEM_SIZE_ID
                    ,   WOW.ITEM_SIZE_NAME
                    ,   WOW.JAN
                    ,   WOW.INVOICE_NO
                    ,   WOW.SEQ
                    ,   WOW.CENTER_ID
                    ,   WOW.UPDATE_COUNT
                FROM
                        WW_OTH_WORKING WOW
                WHERE
                        WOW.SHIPPER_ID = :SHIPPER_ID
                    AND WOW.CENTER_ID  = :CENTER_ID
                    AND WOW.SEQ = :SEQ
            ");
            query.AppendLine(order);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SEQ", condition.Seq);


            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<WorkingReferenceResultRow>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var WorkingReferences = MvcDbContext.Current.Database.Connection.Query<WorkingReferenceResultRow>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<WorkingReferenceResultRow>(WorkingReferences, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// スマホ移動入荷検品ワークデータ取得
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<WorkingReferenceResultRow> GetIdoWkData(WorkingReferenceSearchConditions condition)
        {
            string order;
            switch (condition.IdoKey)
            {
                case IdoSortKey.StartDateWorkingUser:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            order = "ORDER BY WOW.START_DATE DESC,WOW.WORK_USER_ID DESC";
                            break;

                        default:
                            order = "ORDER BY WOW.START_DATE ASC,WOW.WORK_USER_ID ASC";
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            order = "ORDER BY WOW.SLIP_NO DESC,WOW.SLIP_SEQ DESC";
                            break;

                        default:
                            order = "ORDER BY WOW.SLIP_NO ASC,WOW.SLIP_SEQ ASC";
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
                    ,   TO_CHAR(WOW.START_DATE,'YYYY/MM/DD HH24:MI:SS') AS START_DATE
                    ,   WOW.WORK_USER_ID
                    ,   WOW.WORK_USER_NAME
                    ,   WOW.SLIP_NO
                    ,   WOW.TRANSFER_CLASS
                    ,   MG.GEN_NAME AS TRANS_FROM_STORE_CLASS
                    ,   WOW.TRANSFER_FROM_STORE_NAME
                    ,   WOW.ARRIVE_PLAN_QTY
                    ,   WOW.RESULT_QTY
                    ,   WOW.SEQ
                    ,   WOW.HAITA_SEQ
                    ,   WOW.CENTER_ID
                    ,   WOW.UPDATE_COUNT
                FROM
                        WW_OTH_WORKING WOW
                LEFT JOIN
                        M_GENERALS MG
                ON
                        WOW.SHIPPER_ID = MG.SHIPPER_ID
                    AND MG.CENTER_ID = '@@@'
                    AND MG.REGISTER_DIVI_CD = '1'
                    AND MG.GEN_DIV_CD = 'STORE_CLASS'
                    AND WOW.TRANS_FROM_STORE_CLASS = MG.GEN_CD
                WHERE
                        WOW.SHIPPER_ID = :SHIPPER_ID
                    AND WOW.CENTER_ID  = :CENTER_ID
                    AND WOW.SEQ = :SEQ
            ");
            query.AppendLine(order);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SEQ", condition.Seq);


            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<WorkingReferenceResultRow>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var WorkingReferences = MvcDbContext.Current.Database.Connection.Query<WorkingReferenceResultRow>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<WorkingReferenceResultRow>(WorkingReferences, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// スマホトータルピック中ワークデータ取得
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<WorkingReferenceResultRow> GetPicWkData(WorkingReferenceSearchConditions condition)
        {
            string order;
            switch (condition.PicKey)
            {
                case PicSortKey.PicDatePicUser:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            order = "ORDER BY WOW.START_DATE DESC,WOW.WORK_USER_ID DESC";
                            break;

                        default:
                            order = "ORDER BY WOW.START_DATE ASC,WOW.WORK_USER_ID ASC";
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            order = "ORDER BY WOW.BATCH_NO DESC,WOW.LOCATION_CD DESC,WOW.ITEM_SKU_ID DESC";
                            break;

                        default:
                            order = "ORDER BY WOW.BATCH_NO ASC,WOW.LOCATION_CD ASC,WOW.ITEM_SKU_ID ASC";
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
                    ,   TO_CHAR(WOW.START_DATE,'YYYY/MM/DD HH24:MI:SS') AS START_DATE
                    ,   WOW.WORK_USER_ID
                    ,   WOW.WORK_USER_NAME
                    ,   WOW.ITEM_ID
                    ,   WOW.ITEM_NAME
                    ,   WOW.ITEM_COLOR_ID
                    ,   WOW.ITEM_COLOR_NAME
                    ,   WOW.ITEM_SIZE_ID
                    ,   WOW.ITEM_SIZE_NAME
                    ,   WOW.JAN
                    ,   WOW.LOCATION_CD
                    ,   WOW.HIKI_QTY
                    ,   WOW.RESULT_QTY
                    ,   WOW.SEQ
                    ,   WOW.CENTER_ID
                    ,   WOW.UPDATE_COUNT
                    ,   MG.GEN_NAME AS PIC_KIND
                    ,   WOW.BATCH_NO
                FROM
                        WW_OTH_WORKING WOW
                LEFT JOIN
                        M_GENERALS MG
                ON
                        WOW.SHIPPER_ID = MG.SHIPPER_ID
                    AND MG.CENTER_ID = '@@@'
                    AND MG.REGISTER_DIVI_CD = '1'
                    AND MG.GEN_DIV_CD = 'PICK_KIND'
                    AND WOW.PICK_KIND = MG.GEN_CD
                WHERE
                        WOW.SHIPPER_ID = :SHIPPER_ID
                    AND WOW.CENTER_ID  = :CENTER_ID
                    AND WOW.SEQ = :SEQ
            ");
            query.AppendLine(order);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SEQ", condition.Seq);


            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<WorkingReferenceResultRow>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var WorkingReferences = MvcDbContext.Current.Database.Connection.Query<WorkingReferenceResultRow>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<WorkingReferenceResultRow>(WorkingReferences, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// スマホ店別ワークデータ取得
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<WorkingReferenceResultRow> GetStoreWkData(WorkingReferenceSearchConditions condition)
        {
            string order;
            switch (condition.StoreKey)
            {
                case StoreSortKey.ShiwakeDateWorkingUser:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            order = "ORDER BY WOW.START_DATE DESC,WOW.WORK_USER_ID DESC";
                            break;

                        default:
                            order = "ORDER BY WOW.START_DATE ASC,WOW.WORK_USER_ID ASC";
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            order = "ORDER BY WOW.BATCH_NO DESC,WOW.ITEM_SKU_ID DESC,WOW.LANE_NO DESC,WOW.FRONTAGE_NO DESC";
                            break;

                        default:
                            order = "ORDER BY WOW.BATCH_NO ASC,WOW.ITEM_SKU_ID ASC,WOW.LANE_NO ASC,WOW.FRONTAGE_NO ASC";
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
                    ,   TO_CHAR(WOW.START_DATE,'YYYY/MM/DD HH24:MI:SS') AS START_DATE
                    ,   WOW.WORK_USER_ID
                    ,   WOW.WORK_USER_NAME
                    ,   WOW.ITEM_ID
                    ,   WOW.ITEM_NAME
                    ,   WOW.ITEM_COLOR_ID
                    ,   WOW.ITEM_COLOR_NAME
                    ,   WOW.ITEM_SIZE_ID
                    ,   WOW.ITEM_SIZE_NAME
                    ,   WOW.JAN
                    ,   WOW.HIKI_QTY
                    ,   WOW.RESULT_QTY
                    ,   WOW.SEQ
                    ,   WOW.CENTER_ID
                    ,   WOW.UPDATE_COUNT
                    ,   WOW.BATCH_NO
                    ,   WOW.LANE_NO
                    ,   WOW.FRONTAGE_NO
                    ,   WOW.SHIP_TO_STORE_ID
                    ,   WOW.STORE_NAME AS SHIP_TO_STORE_NAME
                FROM
                        WW_OTH_WORKING WOW
                WHERE
                        WOW.SHIPPER_ID = :SHIPPER_ID
                    AND WOW.CENTER_ID  = :CENTER_ID
                    AND WOW.SEQ = :SEQ
            ");
            query.AppendLine(order);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SEQ", condition.Seq);


            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<WorkingReferenceResultRow>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var WorkingReferences = MvcDbContext.Current.Database.Connection.Query<WorkingReferenceResultRow>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<WorkingReferenceResultRow>(WorkingReferences, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// スマホ在庫仕分ワークデータ取得
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<WorkingReferenceResultRow> GetStockWkData(WorkingReferenceSearchConditions condition)
        {
            string order;
            switch (condition.StockKey)
            {
                case StockSortKey.StartDateWorkingUser:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            order = "ORDER BY WOW.START_DATE DESC,WOW.WORK_USER_ID DESC";
                            break;

                        default:
                            order = "ORDER BY WOW.START_DATE ASC,WOW.WORK_USER_ID ASC";
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            order = "ORDER BY WOW.BOX_NO DESC";
                            break;

                        default:
                            order = "ORDER BY WOW.BOX_NO ASC";
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
                    ,   TO_CHAR(WOW.START_DATE,'YYYY/MM/DD HH24:MI:SS') AS START_DATE
                    ,   WOW.WORK_USER_ID
                    ,   WOW.WORK_USER_NAME
                    ,   WOW.BOX_NO
                    ,   WOW.SKU_QTY
                    ,   WOW.SORT_PLAN_QTY
                    ,   WOW.SORT_RESULT_QTY
                    ,   WOW.SEQ
                    ,   WOW.HAITA_SEQ
                    ,   WOW.CENTER_ID
                    ,   WOW.UPDATE_COUNT
                    ,   WOW.TERMINAL_INFO
                FROM
                        WW_OTH_WORKING WOW
                WHERE
                        WOW.SHIPPER_ID = :SHIPPER_ID
                    AND WOW.CENTER_ID  = :CENTER_ID
                    AND WOW.SEQ = :SEQ
            ");
            query.AppendLine(order);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SEQ", condition.Seq);


            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<WorkingReferenceResultRow>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var WorkingReferences = MvcDbContext.Current.Database.Connection.Query<WorkingReferenceResultRow>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<WorkingReferenceResultRow>(WorkingReferences, condition.Page, condition.PageSize, totalCount);
        }


        /// <summary>
        /// 引当解除・削除ストアド実行処理
        /// </summary>
        public void DeleteWorkingData(WorkingReferenceSearchConditions searchConditions, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", searchConditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_SEARCH_KIND", (int)searchConditions.SearchKind, DbType.Int32, ParameterDirection.Input);

            param.Add("IN_INVOICE_NO", searchConditions.InvoiceNo, DbType.String, ParameterDirection.Input);
            param.Add("IN_ITEM_SKU_ID", searchConditions.ItemSkuId, DbType.String, ParameterDirection.Input);
            param.Add("IN_HAITA_SEQ", searchConditions.HaitaSeq, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_SLIP_NO", searchConditions.SlipNo, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_NO", searchConditions.BoxNo, DbType.String, ParameterDirection.Input);

            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
            "SP_W_OTH_DEL_HAITA",
            param,
            commandType: CommandType.StoredProcedure);
            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }

    }
}