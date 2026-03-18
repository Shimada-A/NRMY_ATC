namespace Wms.Areas.Inventory.Query.Reference
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
    using Share.Helpers;
    using Wms.Areas.Inventory.Models;
    using Wms.Areas.Inventory.Resources;
    using Wms.Areas.Inventory.ViewModels.Confirm;
    using Wms.Areas.Inventory.ViewModels.Reference;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Inventory.ViewModels.Reference.Reference01SearchConditions;
    using static Wms.Areas.Inventory.ViewModels.Reference.Reference02SearchConditions;

    public class ReferenceQuery
    {

        /// <summary>
        /// Insert Work01 Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertInvReference01(Reference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                WITH
                    INVENTORY_DATA AS (
                        SELECT 
                                TP.CENTER_ID CENTER_ID
                            ,   TP.INVENTORY_NO INVENTORY_NO
                            ,   TP.INVENTORY_START_DATE INVENTORY_START_DATE
                            ,   TP.INVENTORY_CLASS INVENTORY_CLASS
                            ,   TP.INVENTORY_NAME INVENTORY_NAME
                            ,   TP.STOCK_QTY_START STOCK_QTY_START
                            ,   TR.RESULT_QTY RESULT_QTY
                            ,   CASE WHEN TR.RESULT_QTY - TP.STOCK_QTY_START > 0 THEN TR.RESULT_QTY - TP.STOCK_QTY_START ELSE 0 END DIFFERENCE_PLUS
                            ,   CASE WHEN TR.RESULT_QTY - TP.STOCK_QTY_START < 0 THEN TR.RESULT_QTY - TP.STOCK_QTY_START ELSE 0 END DIFFERENCE_MINUS
                            ,   TP.INVENTORY_CONFIRM_FLAG INVENTORY_CONFIRM_FLAG
                            ,   TP.DIFFERENCE_LIST_FLAG DIFFERENCE_LIST_FLAG
                            ,   TP.LOCATION_CD LOCATION_CD
                            ,   TP.ITEM_SKU_ID ITEM_SKU_ID
                        FROM
                                T_INVENTORY_PLANS TP
                        LEFT OUTER JOIN
                                T_INVENTORY_RESULTS TR
                        ON
                                TP.SHIPPER_ID = TR.SHIPPER_ID
                            AND TP.CENTER_ID = TR.CENTER_ID
                            AND TP.INVENTORY_NO = TR.INVENTORY_NO
                            AND TP.INVENTORY_SEQ = TR.INVENTORY_SEQ
                            AND TR.LAST_COUNT_FLAG = 1
                        WHERE
                                TP.SHIPPER_ID = :SHIPPER_ID

                        UNION ALL

                        SELECT 
                                ML.CENTER_ID CENTER_ID
                            ,   ML.INVENTORY_NO INVENTORY_NO
                            ,   ML.INVENTORY_START_DATE INVENTORY_START_DATE
                            ,   2 INVENTORY_CLASS
                            ,   ML.INVENTORY_NAME INVENTORY_NAME
                            ,   0 STOCK_QTY_START
                            ,   0 RESULT_QTY
                            ,   0 DIFFERENCE_PLUS
                            ,   0 DIFFERENCE_MINUS
                            ,   ML.INVENTORY_CONFIRM_FLAG INVENTORY_CONFIRM_FLAG
                            ,   0 DIFFERENCE_LIST_FLAG
                            ,   ML.LOCATION_CD LOCATION_CD
                            ,   NULL ITEM_SKU_ID
                        FROM
                                M_LOCATIONS ML
                        WHERE
                                ML.SHIPPER_ID = :SHIPPER_ID
                            AND ML.INVENTORY_NO IS NOT NULL
                            AND ML.INVENTORY_CONFIRM_FLAG > 0
                            AND NOT EXISTS (
                                    SELECT
                                            *
                                    FROM
                                            T_INVENTORY_PLANS TP
                                    WHERE
                                            TP.SHIPPER_ID = ML.SHIPPER_ID
                                        AND TP.CENTER_ID = ML.CENTER_ID
                                        AND TP.INVENTORY_NO = ML.INVENTORY_NO
                                )
                    )
                SELECT 
                       CENTER_ID
                      ,INVENTORY_NO
                      ,INVENTORY_START_DATE
                      ,INVENTORY_CLASS
                      ,INVENTORY_NAME
                      ,STOCK_QTY_START
                      ,RESULT_QTY
                      ,DIFFERENCE_PLUS
                      ,DIFFERENCE_MINUS
                      ,CASE WHEN INVENTORY_CONFIRM_FLAG = 1 AND  DIFFERENCE_LIST_FLAG = 0 THEN '" + ReferenceResource.Counting + @"'
                            WHEN INVENTORY_CONFIRM_FLAG = 1 AND  DIFFERENCE_LIST_FLAG = 1 THEN '" + ReferenceResource.Investigating + @"'
                            WHEN INVENTORY_CONFIRM_FLAG = 2 THEN '" + ReferenceResource.ProvisionalDecision + @"'
                            WHEN INVENTORY_CONFIRM_FLAG = 3 THEN '" + ReferenceResource.DefiniteDecision + @"' END INVENTORY_STATUS
                      ,INVENTORY_CONFIRM_FLAG
                      ,DIFFERENCE_LIST_FLAG
                FROM (
                        SELECT 
                               CENTER_ID
                              ,INVENTORY_NO
                              ,INVENTORY_START_DATE
                              ,MIN(INVENTORY_CLASS)　INVENTORY_CLASS
                              ,INVENTORY_NAME
                              ,SUM(STOCK_QTY_START) STOCK_QTY_START
                              ,CASE WHEN MAX(RESULT_QTY) IS NULL THEN NULL ELSE SUM(RESULT_QTY) END RESULT_QTY
                              ,SUM(DIFFERENCE_PLUS) DIFFERENCE_PLUS
                              ,SUM(DIFFERENCE_MINUS) DIFFERENCE_MINUS
                              ,MAX(INVENTORY_CONFIRM_FLAG) INVENTORY_CONFIRM_FLAG
                              ,MAX(DIFFERENCE_LIST_FLAG) DIFFERENCE_LIST_FLAG
                        FROM 
                                INVENTORY_DATA
                        WHERE
                                1 = 1
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

            // Add search condition
            // センター
            if (!string.IsNullOrEmpty(condition.CenterId))
            {
                query.Append(" AND CENTER_ID = :CENTER_ID ");
                parameters.Add(":CENTER_ID", condition.CenterId);
            }

            // 棚卸区分
            if (!string.IsNullOrEmpty(condition.InventoryClass))
            {
                query.Append(" AND INVENTORY_CLASS = :INVENTORY_CLASS ");
                parameters.Add(":INVENTORY_CLASS", condition.InventoryClass);
            }

            // 棚卸名称
            if (!string.IsNullOrEmpty(condition.InventoryName))
            {
                query.Append(" AND INVENTORY_NAME LIKE :INVENTORY_NAME ");
                parameters.Add(":INVENTORY_NAME", condition.InventoryName + "%");
            }

            // 棚卸開始日
            if (condition.InventoryDateFrom != null)
            {
                query.Append(" AND TRUNC(INVENTORY_START_DATE) >= :INVENTORY_START_DATE_FROM ");
                parameters.Add(":INVENTORY_START_DATE_FROM", condition.InventoryDateFrom);
            }

            if (condition.InventoryDateTo != null)
            {
                query.Append(" AND TRUNC(INVENTORY_START_DATE) <= :INVENTORY_START_DATE_TO ");
                parameters.Add(":INVENTORY_START_DATE_TO", condition.InventoryDateTo);
            }

            // 棚卸状況
            if (!string.IsNullOrEmpty(condition.InventoryStatus))
            {
                if (condition.InventoryStatus == "1")
                {
                    query.Append(" AND INVENTORY_CONFIRM_FLAG = :INVENTORY_CONFIRM_FLAG ");
                    parameters.Add(":INVENTORY_CONFIRM_FLAG", 1);
                    query.Append(" AND DIFFERENCE_LIST_FLAG = :DIFFERENCE_LIST_FLAG ");
                    parameters.Add(":DIFFERENCE_LIST_FLAG", 0);
                }
                else if (condition.InventoryStatus == "2")
                {
                    query.Append(" AND INVENTORY_CONFIRM_FLAG = :INVENTORY_CONFIRM_FLAG ");
                    parameters.Add(":INVENTORY_CONFIRM_FLAG", 1);
                    query.Append(" AND DIFFERENCE_LIST_FLAG = :DIFFERENCE_LIST_FLAG ");
                    parameters.Add(":DIFFERENCE_LIST_FLAG", 1);
                }
                else if (condition.InventoryStatus == "3")
                {
                    query.Append(" AND INVENTORY_CONFIRM_FLAG = :INVENTORY_CONFIRM_FLAG ");
                    parameters.Add(":INVENTORY_CONFIRM_FLAG", 2);
                }
                else if (condition.InventoryStatus == "4")
                {
                    query.Append(" AND INVENTORY_CONFIRM_FLAG = :INVENTORY_CONFIRM_FLAG ");
                    parameters.Add(":INVENTORY_CONFIRM_FLAG", 3);
                }
            }

            // 棚卸No
            if (!string.IsNullOrEmpty(condition.InventoryNo))
            {
                query.Append(" AND INVENTORY_NO = :INVENTORY_NO ");
                parameters.Add(":INVENTORY_NO", condition.InventoryNo);
            }

            // ロケーション
            if (!string.IsNullOrEmpty(condition.LocationCd))
            {
                query.Append(" AND LOCATION_CD LIKE :LOCATION_CD ");
                parameters.Add(":LOCATION_CD", condition.LocationCd + "%");
            }

            // SKU
            if (!string.IsNullOrEmpty(condition.ItemSkuId))
            {
                query.Append(" AND ITEM_SKU_ID LIKE :ITEM_SKU_ID ");
                parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId + "%");
            }

            query.Append(@" GROUP BY CENTER_ID
                                    ,INVENTORY_NO
                                    ,INVENTORY_START_DATE
                                    ,INVENTORY_NAME
                    )

            ");

            // 1.ワークID採番
            condition.Seq = new BaseQuery().GetWorkId();
            condition.Page = 1;
            // 2.検索・ワーク作成
            var result = MvcDbContext.Current.Database.Connection.Query<InvReference01>(query.ToString(), parameters);
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var row in result.Select((v, i) => new { v, i }))
                {
                    var invReference01 = new InvReference01();
                    invReference01.SetBaseInfoInsert();
                    invReference01.Seq = condition.Seq;
                    invReference01.LineNo = row.i + 1;
                    invReference01.IsCheck = false;
                    invReference01.CenterId = row.v.CenterId;
                    invReference01.InventoryNo = row.v.InventoryNo;
                    invReference01.InventoryStartDate = row.v.InventoryStartDate;
                    invReference01.InventoryClass = row.v.InventoryClass;
                    invReference01.InventoryName = row.v.InventoryName;
                    invReference01.StockQtyStart = row.v.StockQtyStart;
                    invReference01.ResultQty = row.v.ResultQty;
                    invReference01.DifferencePlus = row.v.DifferencePlus;
                    invReference01.DifferenceMinus = row.v.DifferenceMinus;
                    invReference01.DifferenceSum = row.v.DifferencePlus + row.v.DifferenceMinus;
                    invReference01.InventoryStatus = row.v.InventoryStatus;
                    MvcDbContext.Current.InvReference01s.Add(invReference01);
                    try
                    {
                        MvcDbContext.Current.SaveChanges();
                    }
                    catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                    {
                        return false;
                    }
                }

                trans.Commit();

            }

            if (condition.InventoryStatusOld)
            {
                var deleteUserProgram = new StringBuilder();

                // 共通ヘッダ
                deleteUserProgram.Append(@"
                DELETE FROM
                        WW_INV_REFERENCE01
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                AND     SEQ = :SEQ
                AND     DIFFERENCE_PLUS = 0
                AND     DIFFERENCE_MINUS = 0");

                // 荷主ID
                parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

                // ワークID
                parameters.Add(":SEQ", condition.Seq);

                using (var trans = MvcDbContext.Current.Database.BeginTransaction())
                {

                    MvcDbContext.Current.Database.Connection.Execute(deleteUserProgram.ToString(), parameters);

                    try
                    {
                        MvcDbContext.Current.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return false;
                    }

                    trans.Commit();
                }
            }

            return true;
        }

        /// <summary>
        /// Get Work01 List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<Reference01ResultRow> GetData(Reference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
               SELECT  W.CENTER_ID
                      ,W.INVENTORY_NO
                      ,W.INVENTORY_START_DATE
                      ,W.INVENTORY_CLASS
                      ,W.INVENTORY_NAME
                      ,W.STOCK_QTY_START
                      ,W.RESULT_QTY
                      ,CASE WHEN W.DIFFERENCE_PLUS = 0 THEN NULL ELSE W.DIFFERENCE_PLUS END DIFFERENCE_PLUS
                      ,CASE WHEN W.DIFFERENCE_MINUS = 0 THEN NULL ELSE W.DIFFERENCE_MINUS END DIFFERENCE_MINUS
                      ,CASE WHEN W.DIFFERENCE_SUM = 0 THEN NULL ELSE W.DIFFERENCE_SUM END DIFFERENCE_SUM
                      ,W.INVENTORY_STATUS
                      ,W.SEQ
                      ,W.LINE_NO
                      ,W.IS_CHECK
                      ,(SELECT CASE WHEN T.INVENTORY_NO IS NULL THEN 0 ELSE 1 END INVENTORY_NO
                         FROM  T_INVENTORY_RESULTS T
                         WHERE W.SHIPPER_ID = T.SHIPPER_ID
                         AND   W.INVENTORY_NO = T.INVENTORY_NO
                         AND   W.CENTER_ID = T.CENTER_ID
                         AND   ROWNUM = 1
                       ) RESULTS_DATA_FLAG
                 FROM  WW_INV_REFERENCE01 W
                WHERE W.SHIPPER_ID = :SHIPPER_ID
                  AND W.SEQ = :SEQ
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<Reference01ResultRow>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case ReferenceSortKey.InventoryStartDateNo:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY INVENTORY_START_DATE DESC, INVENTORY_NO DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY INVENTORY_START_DATE ASC, INVENTORY_NO ASC");
                            break;
                    }

                    break;

                case ReferenceSortKey.InventoryNameNo:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY INVENTORY_NAME DESC, INVENTORY_NO DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY INVENTORY_NAME ASC, INVENTORY_NO ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY INVENTORY_NO DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY INVENTORY_NO ASC");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var stockInquirys = MvcDbContext.Current.Database.Connection.Query<Reference01ResultRow>(query.ToString(), parameters);
            condition.SelectedCnt = MvcDbContext.Current.InvReference01s.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId && x.IsCheck).Count();

            //選択中実績済数取得
            query = new StringBuilder(@"
                            SELECT
                                    COUNT(1)
                            FROM
                                    WW_INV_REFERENCE01 WW
                            WHERE
                                    WW.SHIPPER_ID = :SHIPPER_ID
                                AND WW.CENTER_ID = :CENTER_ID
                                AND WW.SEQ = :SEQ
                                AND WW.IS_CHECK = 1
                                AND WW.INVENTORY_STATUS = '本確定'
            ");
            parameters = new DynamicParameters();
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SEQ", condition.Seq);
            int resultCnt = MvcDbContext.Current.Database.Connection.Query<int>(query.ToString(), parameters).FirstOrDefault();
            condition.ResultCnt = resultCnt;

            //選択中棚卸状況取得
            query = new StringBuilder(@"
                            SELECT
                                    COUNT(1)
                            FROM
                                    WW_INV_REFERENCE01 WW
                            WHERE
                                    WW.SHIPPER_ID = :SHIPPER_ID
                                AND WW.CENTER_ID = :CENTER_ID
                                AND WW.SEQ = :SEQ
                                AND WW.IS_CHECK = 1
                                AND WW.INVENTORY_STATUS IN ('本確定','仮確定')
            ");
            parameters = new DynamicParameters();
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SEQ", condition.Seq);
            int statusCnt = MvcDbContext.Current.Database.Connection.Query<int>(query.ToString(), parameters).FirstOrDefault();
            condition.InventoryCnt = statusCnt;

            // Excute paging
            return new StaticPagedList<Reference01ResultRow>(stockInquirys, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Insert Work02 Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertInvReference02(Reference02SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder($@"
                INSERT 
                INTO WW_INV_REFERENCE02( 
                    MAKE_USER_ID
                    , MAKE_PROGRAM_NAME
                    , UPDATE_USER_ID
                    , UPDATE_PROGRAM_NAME
                    , SHIPPER_ID
                    , SEQ
                    , LINE_NO
                    , CENTER_ID
                    , INVENTORY_NO
                    , INVENTORY_START_DATE
                    , INVENTORY_CLASS
                    , INVENTORY_NAME
                    , SIMPLE_INVENTORY_FLAG
                    , INVENTORY_STATUS
                    , INVENTORY_CONFIRM_FLAG
                    , INVENTORY_CONFIRM_DATE
                    , LOCATION_CD
                    , CASE_CLASS
                    , BOX_NO
                    , GRADE_ID
                    , STOCK_QTY_START
                    , RESULT_QTY
                    , DIFFERENCE_PLUS
                    , DIFFERENCE_MINUS
                    , DIFFERENCE_SUM
                ) 
                WITH
                    INVENTORY_DATA AS (
                        SELECT 
                                TP.CENTER_ID CENTER_ID
                            ,   TP.INVENTORY_NO INVENTORY_NO
                            ,   TP.INVENTORY_START_DATE INVENTORY_START_DATE
                            ,   TP.INVENTORY_CLASS INVENTORY_CLASS
                            ,   TP.INVENTORY_NAME INVENTORY_NAME
                            ,   TP.SIMPLE_INVENTORY_FLAG SIMPLE_INVENTORY_FLAG
                            ,   TP.INVENTORY_CONFIRM_FLAG INVENTORY_CONFIRM_FLAG
                            ,   TP.INVENTORY_CONFIRM_DATE INVENTORY_CONFIRM_DATE
                            ,   TP.DIFFERENCE_LIST_FLAG DIFFERENCE_LIST_FLAG
                            ,   TP.LOCATION_CD LOCATION_CD
                            ,   TP.CASE_CLASS CASE_CLASS
                            ,   CASE
                                    WHEN TP.BOX_NO = ' ' THEN '{ReferenceResource.Rose}'
                                    ELSE '{ReferenceResource.Cases}' 
                                END BOX_NO
                            ,   TP.GRADE_ID GRADE_ID
                            ,   TP.STOCK_QTY_START STOCK_QTY_START
                            ,   TR.RESULT_QTY RESULT_QTY
                            ,   CASE WHEN TR.RESULT_QTY - TP.STOCK_QTY_START > 0 THEN TR.RESULT_QTY - TP.STOCK_QTY_START ELSE 0 END DIFFERENCE_PLUS
                            ,   CASE WHEN TR.RESULT_QTY - TP.STOCK_QTY_START < 0 THEN TR.RESULT_QTY - TP.STOCK_QTY_START ELSE 0 END DIFFERENCE_MINUS
                            ,   ML.LOCSEC_1
                            ,   ML.LOCSEC_2
                            ,   TP.ITEM_ID
                            ,   TP.ITEM_SKU_ID
                            ,   TP.JAN
                        FROM
                                T_INVENTORY_PLANS TP
                        INNER JOIN
                                M_LOCATIONS ML
                        ON
                                ML.LOCATION_CD = TP.LOCATION_CD
                            AND ML.CENTER_ID = TP.CENTER_ID
                            AND ML.SHIPPER_ID = TP.SHIPPER_ID
                        LEFT OUTER JOIN
                                T_INVENTORY_RESULTS TR
                        ON
                                TP.SHIPPER_ID = TR.SHIPPER_ID
                            AND TP.CENTER_ID = TR.CENTER_ID
                            AND TP.INVENTORY_NO = TR.INVENTORY_NO
                            AND TP.INVENTORY_SEQ = TR.INVENTORY_SEQ
                            AND TR.LAST_COUNT_FLAG = 1
                        WHERE
                                TP.SHIPPER_ID = :SHIPPER_ID
                            AND TP.CENTER_ID = :CENTER_ID

                        UNION ALL

                        SELECT 
                                ML.CENTER_ID CENTER_ID
                            ,   ML.INVENTORY_NO INVENTORY_NO
                            ,   ML.INVENTORY_START_DATE INVENTORY_START_DATE
                            ,   2 INVENTORY_CLASS
                            ,   ML.INVENTORY_NAME INVENTORY_NAME
                            ,   0 SIMPLE_INVENTORY_FLAG
                            ,   ML.INVENTORY_CONFIRM_FLAG INVENTORY_CONFIRM_FLAG
                            ,   ML.INVENTORY_CONFIRM_DATE INVENTORY_CONFIRM_DATE
                            ,   0 DIFFERENCE_LIST_FLAG
                            ,   ML.LOCATION_CD LOCATION_CD
                            ,   CASE
                                    WHEN ML.CASE_CLASS = 1 THEN 1
                                    ELSE 2
                                END CASE_CLASS
                            ,   CASE
                                    WHEN ML.CASE_CLASS = 1 THEN '{ReferenceResource.Cases}'
                                    ELSE '{ReferenceResource.Rose}'
                                END BOX_NO
                            ,   ML.GRADE_ID GRADE_ID
                            ,   0 STOCK_QTY_START
                            ,   0 RESULT_QTY
                            ,   0 DIFFERENCE_PLUS
                            ,   0 DIFFERENCE_MINUS
                            ,   ML.LOCSEC_1
                            ,   ML.LOCSEC_2
                            ,   NULL ITEM_ID
                            ,   NULL ITEM_SKU_ID
                            ,   NULL JAN
                        FROM
                                M_LOCATIONS ML
                        WHERE
                                ML.SHIPPER_ID = :SHIPPER_ID
                            AND ML.CENTER_ID = :CENTER_ID
                            AND ML.INVENTORY_NO IS NOT NULL
                            AND ML.INVENTORY_CONFIRM_FLAG > 0
                            AND NOT EXISTS (
                                    SELECT
                                            *
                                    FROM
                                            T_INVENTORY_PLANS TP
                                    WHERE
                                            TP.SHIPPER_ID = ML.SHIPPER_ID
                                        AND TP.CENTER_ID = ML.CENTER_ID
                                        AND TP.INVENTORY_NO = ML.INVENTORY_NO
                                        AND TP.LOCATION_CD = ML.LOCATION_CD
                                )
                    )
                SELECT
                    :USER_ID
                    , :PROGRAM_NAME
                    , :USER_ID
                    , :PROGRAM_NAME
                    , :SHIPPER_ID
                    , :SEQ
                    , ROW_NUMBER() OVER (ORDER BY INVENTORY_NO)
                    , CENTER_ID
                    , INVENTORY_NO
                    , INVENTORY_START_DATE
                    , INVENTORY_CLASS
                    , INVENTORY_NAME
                    , SIMPLE_INVENTORY_FLAG
                    , CASE 
                        WHEN INVENTORY_CONFIRM_FLAG = 1 
                        AND DIFFERENCE_LIST_FLAG = 0 
                            THEN '" + ReferenceResource.Counting + @"' 
                        WHEN INVENTORY_CONFIRM_FLAG = 1 
                        AND DIFFERENCE_LIST_FLAG = 1 
                            THEN '" + ReferenceResource.Investigating + @"' 
                        WHEN INVENTORY_CONFIRM_FLAG = 2 
                            THEN '" + ReferenceResource.ProvisionalDecision + @"' 
                        WHEN INVENTORY_CONFIRM_FLAG = 3 
                            THEN '" + ReferenceResource.DefiniteDecision + @"' 
                        END INVENTORY_STATUS 
                    , INVENTORY_CONFIRM_FLAG
                    , INVENTORY_CONFIRM_DATE
                    , LOCATION_CD
                    , CASE_CLASS
                    , BOX_NO
                    , GRADE_ID
                    , STOCK_QTY_START
                    , RESULT_QTY
                    , DIFFERENCE_PLUS
                    , DIFFERENCE_MINUS
                    , DIFFERENCE_PLUS + DIFFERENCE_MINUS 
                FROM (
                        SELECT 
                               CENTER_ID
                              ,INVENTORY_NO
                              ,INVENTORY_START_DATE
                              ,MIN(INVENTORY_CLASS)　INVENTORY_CLASS
                              ,INVENTORY_NAME
                              ,SIMPLE_INVENTORY_FLAG
                              ,MAX(INVENTORY_CONFIRM_FLAG) AS INVENTORY_CONFIRM_FLAG
                              ,MAX(INVENTORY_CONFIRM_DATE) AS INVENTORY_CONFIRM_DATE
                              ,MAX(DIFFERENCE_LIST_FLAG) AS DIFFERENCE_LIST_FLAG 
                              ,LOCATION_CD
                              ,CASE_CLASS
                              ,BOX_NO
                              ,GRADE_ID
                              ,SUM(STOCK_QTY_START) STOCK_QTY_START
                              ,CASE WHEN MAX(RESULT_QTY) IS NULL THEN NULL ELSE SUM(RESULT_QTY) END RESULT_QTY
                              ,SUM(DIFFERENCE_PLUS) DIFFERENCE_PLUS
                              ,SUM(DIFFERENCE_MINUS) DIFFERENCE_MINUS
                        FROM 
                                INVENTORY_DATA
                        WHERE
                                1 = 1
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);

            // Add search condition
            // 棚卸No
            if (!string.IsNullOrEmpty(condition.InventoryNo))
            {
                query.Append(" AND INVENTORY_NO = :INVENTORY_NO ");
                parameters.Add(":INVENTORY_NO", condition.InventoryNo);
            }

            // エリア
            if (!string.IsNullOrEmpty(condition.Area))
            {
                query.Append(" AND LOCSEC_1 = :AREA ");
                parameters.Add(":AREA", condition.Area);
            }

            // 棚列
            if (!string.IsNullOrEmpty(condition.InventoryRow))
            {
                query.Append(" AND LOCSEC_2 = :INVENTORY_ROW ");
                parameters.Add(":INVENTORY_ROW", condition.InventoryRow);
            }

            // ロケーション
            if (!string.IsNullOrEmpty(condition.LocationCdFrom))
            {
                query.Append(" AND LOCATION_CD >= :LOCATION_CD_FROM ");
                parameters.Add(":LOCATION_CD_FROM", condition.LocationCdFrom);
            }

            // ロケーション
            if (!string.IsNullOrEmpty(condition.LocationCdTo))
            {
                query.Append(" AND LOCATION_CD <= :LOCATION_CD_TO ");
                parameters.Add(":LOCATION_CD_TO", condition.LocationCdTo);
            }

            // 格付
            if (!string.IsNullOrEmpty(condition.GradeId))
            {
                query.Append(" AND GRADE_ID = :GRADE_ID ");
                parameters.Add(":GRADE_ID", condition.GradeId);
            }

            // 品番
            if (!string.IsNullOrEmpty(condition.ItemId))
            {
                query.Append(" AND ITEM_ID LIKE :ITEM_ID ");
                parameters.Add(":ITEM_ID", condition.ItemId + "%");
            }

            // ケースNo
            if (!string.IsNullOrEmpty(condition.BoxNo))
            {
                query.Append(" AND BOX_NO LIKE :BOX_NO ");
                parameters.Add(":BOX_NO", condition.BoxNo + "%");
            }

            // SKU
            if (!string.IsNullOrEmpty(condition.ItemSkuId))
            {
                query.Append(" AND ITEM_SKU_ID LIKE :ITEM_SKU_ID ");
                parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId + "%");
            }

            // JAN
            if (!string.IsNullOrEmpty(condition.Jan))
            {
                query.Append(" AND JAN LIKE :JAN ");
                parameters.Add(":JAN", condition.Jan + "%");
            }

            query.Append(@" GROUP BY CENTER_ID
                                     ,INVENTORY_NO
                                     ,INVENTORY_START_DATE
                                     ,INVENTORY_NAME
                                     ,LOCATION_CD
                                     ,CASE_CLASS
                                     ,BOX_NO
                                     ,SIMPLE_INVENTORY_FLAG
                                     ,GRADE_ID 
                    ) 
            ");

            // 1.ワークID採番
            condition.Seq2 = new BaseQuery().GetWorkId();
            condition.Page = 1;
            parameters.AddDynamicParams(
                new { SEQ = condition.Seq2,
                PROGRAM_NAME = GetProgramId(),
                USER_ID = Profile.User.UserId
            });

            // 2.検索・ワーク作成
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    var result = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }

                trans.Commit();

            }

            if (condition.InventoryStatusOld)
            {
                var deleteUserProgram = new StringBuilder();

                // 共通ヘッダ
                deleteUserProgram.Append(@"
                DELETE FROM
                        WW_INV_REFERENCE02
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                AND     SEQ = :SEQ
                AND     DIFFERENCE_PLUS = 0
                AND     DIFFERENCE_MINUS = 0");

                // 荷主ID
                parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

                // ワークID
                parameters.Add(":SEQ", condition.Seq2);

                using (var trans = MvcDbContext.Current.Database.BeginTransaction())
                {

                    MvcDbContext.Current.Database.Connection.Execute(deleteUserProgram.ToString(), parameters);

                    try
                    {
                        MvcDbContext.Current.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return false;
                    }

                    trans.Commit();
                }
            }

            return true;
        }

        /// <summary>
        /// Insert Work04 Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertInvReferenceLoc04(Reference02SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                INSERT
                INTO WW_INV_REFERENCE04(
                    MAKE_USER_ID
                    , MAKE_PROGRAM_NAME
                    , UPDATE_USER_ID
                    , UPDATE_PROGRAM_NAME
                    , SHIPPER_ID
                    , SEQ
                    , CENTER_ID
                    , INVENTORY_NO
                    , LOCATION_COUNT
                    , STOCK_SKU
                    , RESULT_SKU
                    , STOCK_CASE
                    , RESULT_CASE
                    , STOCK_QTY
                    , RESULT_QTY
                )
                WITH
                    INVENTORY_DATA AS (
                        SELECT 
                                TP.CENTER_ID CENTER_ID
                            ,   TP.INVENTORY_NO INVENTORY_NO
                            ,   TP.LOCATION_CD LOCATION_CD
                            ,   CASE WHEN TP.STOCK_FLAG = 1 THEN TP.ITEM_SKU_ID ELSE NULL END STOCK_SKU
                            ,   TR.ITEM_SKU_ID RESULT_SKU
                            ,   CASE WHEN TP.BOX_NO <> ' ' AND TP.STOCK_FLAG = 1 THEN TP.BOX_NO ELSE NULL END STOCK_CASE
                            ,   CASE WHEN TR.BOX_NO <> ' ' THEN TR.BOX_NO ELSE NULL END RESULT_CASE
                            ,   TP.STOCK_QTY_START STOCK_QTY
                            ,   NVL(TR.RESULT_QTY, 0) RESULT_QTY
                        FROM
                                T_INVENTORY_PLANS TP
                        LEFT OUTER JOIN
                                T_INVENTORY_RESULTS TR
                        ON
                                TP.SHIPPER_ID = TR.SHIPPER_ID
                            AND TP.CENTER_ID = TR.CENTER_ID
                            AND TP.INVENTORY_NO = TR.INVENTORY_NO
                            AND TP.INVENTORY_SEQ = TR.INVENTORY_SEQ
                            AND TR.LAST_COUNT_FLAG = 1
                        WHERE
                                TP.SHIPPER_ID = :SHIPPER_ID
                            AND TP.CENTER_ID = :CENTER_ID
                            AND TP.INVENTORY_NO = :INVENTORY_NO

                        UNION ALL

                        SELECT 
                                ML.CENTER_ID CENTER_ID
                            ,   ML.INVENTORY_NO INVENTORY_NO
                            ,   ML.LOCATION_CD LOCATION_CD
                            ,   NULL STOCK_SKU
                            ,   NULL RESULT_SKU
                            ,   NULL STOCK_CASE
                            ,   NULL RESULT_CASE
                            ,   0 STOCK_QTY
                            ,   0 RESULT_QTY
                        FROM
                                M_LOCATIONS ML
                        WHERE
                                ML.SHIPPER_ID = :SHIPPER_ID
                            AND ML.CENTER_ID = :CENTER_ID
                            AND ML.INVENTORY_NO = :INVENTORY_NO
                            AND NOT EXISTS (
                                    SELECT
                                            *
                                    FROM
                                            T_INVENTORY_PLANS TP
                                    WHERE
                                            TP.SHIPPER_ID = ML.SHIPPER_ID
                                        AND TP.CENTER_ID = ML.CENTER_ID
                                        AND TP.INVENTORY_NO = ML.INVENTORY_NO
                                        AND TP.LOCATION_CD = ML.LOCATION_CD
                                )
                    )
                SELECT
                      :USER_ID
                    , :PROGRAM_ID
                    , :USER_ID
                    , :PROGRAM_ID
                    , :SHIPPER_ID
                    , :SEQ
                    , CENTER_ID
                    , INVENTORY_NO
                    , COUNT(DISTINCT(LOCATION_CD))
                    , COUNT(DISTINCT(STOCK_SKU))
                    , COUNT(DISTINCT(RESULT_SKU))
                    , COUNT(DISTINCT(STOCK_CASE))
                    , COUNT(DISTINCT(RESULT_CASE))
                    , SUM(STOCK_QTY)
                    , SUM(RESULT_QTY)
                FROM
                      INVENTORY_DATA
                GROUP BY
                      CENTER_ID
                    , INVENTORY_NO
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":INVENTORY_NO", condition.InventoryNo);

            // 1.ワークID採番
            condition.Seq4 = new BaseQuery().GetWorkId();
            condition.Page = 1;
            parameters.AddDynamicParams(
                new
                {
                    SEQ = condition.Seq4,
                    USER_ID = Profile.User.UserId,
                    PROGRAM_ID=GetProgramId()
                });

            // 2.検索・ワーク作成
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    var result = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }

                trans.Commit();

            }

            return true;
        }

        /// <summary>
        /// Get Head Data
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public Reference02Head GetReference02Head(Reference02SearchConditions condition)
        {
            Reference02Head rh = new Reference02Head();
            //Head
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                WITH
                    INVENTORY_DATA AS (
                        SELECT 
                                TP.SHIPPER_ID
                            ,   TP.CENTER_ID CENTER_ID
                            ,   TP.INVENTORY_NO
                            ,   TP.INVENTORY_START_DATE
                            ,   TP.INVENTORY_CLASS
                            ,   TP.INVENTORY_NAME
                            ,   TP.SIMPLE_INVENTORY_FLAG
                            ,   MAX(TP.INVENTORY_CONFIRM_FLAG) OVER() MAX_INVENTORY_CONFIRM_FLAG
                            ,   MAX(TP.DIFFERENCE_LIST_FLAG) OVER() MAX_DIFFERENCE_LIST_FLAG
                            ,   TP.INVENTORY_CONFIRM_DATE
                        FROM
                                T_INVENTORY_PLANS TP
                        WHERE
                                TP.SHIPPER_ID = :SHIPPER_ID
                            AND TP.CENTER_ID = :CENTER_ID
                            AND TP.INVENTORY_NO = :INVENTORY_NO

                        UNION ALL

                        SELECT 
                                ML.SHIPPER_ID
                            ,   ML.CENTER_ID CENTER_ID
                            ,   ML.INVENTORY_NO
                            ,   ML.INVENTORY_START_DATE
                            ,   2 INVENTORY_CLASS
                            ,   ML.INVENTORY_NAME
                            ,   0 SIMPLE_INVENTORY_FLAG
                            ,   ML.INVENTORY_CONFIRM_FLAG MAX_INVENTORY_CONFIRM_FLAG
                            ,   0 MAX_DIFFERENCE_LIST_FLAG
                            ,   ML.INVENTORY_CONFIRM_DATE
                        FROM
                                M_LOCATIONS ML
                        WHERE
                                ML.SHIPPER_ID = :SHIPPER_ID
                            AND ML.CENTER_ID = :CENTER_ID
                            AND ML.INVENTORY_NO = :INVENTORY_NO
                            AND NOT EXISTS (
                                    SELECT
                                            *
                                    FROM
                                            T_INVENTORY_PLANS TP
                                    WHERE
                                            TP.SHIPPER_ID = ML.SHIPPER_ID
                                        AND TP.CENTER_ID = ML.CENTER_ID
                                        AND TP.INVENTORY_NO = ML.INVENTORY_NO
                                )
                    )
               SELECT  MAX(MC.CENTER_NAME1) AS CENTER_ID
                      ,TP.INVENTORY_NO
                      ,TP.INVENTORY_START_DATE
                      ,MAX(MG.GEN_NAME) AS INVENTORY_CLASS
                      ,TP.INVENTORY_NAME
                      ,CASE
                            WHEN MAX(TP.SIMPLE_INVENTORY_FLAG) IS NULL THEN '" + ReferenceResource.NotPermission + @"'
                            WHEN MAX(TP.SIMPLE_INVENTORY_FLAG) = 0 THEN '" + ReferenceResource.NotPermission + @"'
                            WHEN MAX(TP.SIMPLE_INVENTORY_FLAG) = 1 THEN '" + ReferenceResource.Permission + @"'
                            ELSE '' 
                       END SIMPLE_INVENTORY_FLAG
                      ,CASE
                            WHEN MAX(TP.MAX_INVENTORY_CONFIRM_FLAG) = 1 AND MAX(TP.MAX_DIFFERENCE_LIST_FLAG) = 0 THEN '" + ReferenceResource.Counting + @"'
                            WHEN MAX(TP.MAX_INVENTORY_CONFIRM_FLAG) = 1 AND MAX(TP.MAX_DIFFERENCE_LIST_FLAG) = 1 THEN '" + ReferenceResource.Investigating + @"'
                            WHEN MAX(TP.MAX_INVENTORY_CONFIRM_FLAG) = 2 THEN '" + ReferenceResource.ProvisionalDecision + @"'
                            WHEN MAX(TP.MAX_INVENTORY_CONFIRM_FLAG) = 3 THEN '" + ReferenceResource.DefiniteDecision + @"'
                       END AS INVENTORY_STATUS
                      ,MAX(TP.INVENTORY_CONFIRM_DATE) AS INVENTORY_CONFIRM_DATE
                 FROM
                        INVENTORY_DATA TP
                    LEFT OUTER JOIN
                        M_CENTERS MC
                    ON
                            TP.SHIPPER_ID = MC.SHIPPER_ID
                        AND TP.CENTER_ID = MC.CENTER_ID
                    LEFT OUTER JOIN
                        M_GENERALS MG
                    ON
                            TP.SHIPPER_ID = MG.SHIPPER_ID
                        AND MG.CENTER_ID = '@@@'
                        AND MG.REGISTER_DIVI_CD = '1'
                        AND MG.GEN_DIV_CD = 'INVENTORY_CLASS'
                        AND TP.INVENTORY_CLASS = MG.GEN_CD
                GROUP BY
                        TP.CENTER_ID
                      , TP.INVENTORY_NO
                      , TP.INVENTORY_START_DATE
                      , TP.INVENTORY_CLASS
                      , TP.INVENTORY_NAME
            ");
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":INVENTORY_NO", condition.InventoryNo);
            rh = new Reference02Head();
            rh = MvcDbContext.Current.Database.Connection.Query<Reference02Head>(query.ToString(), parameters).FirstOrDefault();

            return rh;
        }

        /// <summary>
        /// Get Head Data
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public Reference02Total GetReference02Total(Reference02SearchConditions condition)
        {
            Reference02Total rt = new Reference02Total();
            //Total
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
               SELECT  LOCATION_COUNT
                      ,STOCK_SKU
                      ,RESULT_SKU
                      ,STOCK_CASE
                      ,RESULT_CASE
                      ,STOCK_QTY
                      ,RESULT_QTY
                 FROM WW_INV_REFERENCE04
                WHERE SHIPPER_ID = :SHIPPER_ID
                  AND SEQ = :SEQ
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq4);
            rt = new Reference02Total();
            rt = MvcDbContext.Current.Database.Connection.Query<Reference02Total>(query.ToString(), parameters).FirstOrDefault();

            return rt;
        }

        /// <summary>
        /// Get Work02 List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<Reference02ResultRow> GetLocListData(Reference02SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
               SELECT  WW.LOCATION_CD
                      ,WW.BOX_NO CASE_CLASS
                      ,MG.GRADE_NAME GRADE_ID
                      ,WW.STOCK_QTY_START
                      ,WW.RESULT_QTY
                      ,CASE WHEN WW.DIFFERENCE_PLUS = 0 THEN NULL ELSE WW.DIFFERENCE_PLUS END DIFFERENCE_PLUS
                      ,CASE WHEN WW.DIFFERENCE_MINUS = 0 THEN NULL ELSE WW.DIFFERENCE_MINUS END DIFFERENCE_MINUS
                      ,CASE WHEN WW.DIFFERENCE_SUM = 0 THEN NULL ELSE WW.DIFFERENCE_SUM END DIFFERENCE_SUM
                      ,WW.SEQ
                      ,WW.LINE_NO
                      ,WW.IS_CHECK
                 FROM WW_INV_REFERENCE02 WW
                LEFT JOIN
                      M_GRADES MG
                   ON WW.SHIPPER_ID = MG.SHIPPER_ID
                  AND WW.GRADE_ID = MG.GRADE_ID
                WHERE WW.SHIPPER_ID = :SHIPPER_ID
                  AND WW.SEQ = :SEQ
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq2);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<Reference02ResultRow>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey02)
            {
                case Reference02SortKey.LocationCaseGrade:
                    switch (condition.Sort)
                    {
                        case DetailAscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WW.LOCATION_CD DESC NULLS LAST, WW.CASE_CLASS DESC NULLS LAST, WW.GRADE_ID DESC NULLS LAST");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WW.LOCATION_CD ASC NULLS FIRST, WW.CASE_CLASS ASC NULLS FIRST, WW.GRADE_ID ASC NULLS FIRST");
                            break;
                    }

                    break;

                case Reference02SortKey.ResultQtyLocation:
                    switch (condition.Sort)
                    {
                        case DetailAscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WW.RESULT_QTY DESC NULLS LAST, WW.LOCATION_CD DESC NULLS LAST");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WW.RESULT_QTY ASC NULLS FIRST, WW.LOCATION_CD ASC NULLS FIRST");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case DetailAscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WW.DIFFERENCE_SUM DESC NULLS LAST, WW.LOCATION_CD DESC NULLS LAST");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WW.DIFFERENCE_SUM ASC NULLS FIRST, WW.LOCATION_CD ASC NULLS FIRST");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var Reference02 = MvcDbContext.Current.Database.Connection.Query<Reference02ResultRow>(query.ToString(), parameters);

            // Fill data to memory
            var stockInquirys = MvcDbContext.Current.Database.Connection.Query<Reference02ResultRow>(query.ToString(), parameters);
            condition.SelectedCnt = MvcDbContext.Current.InvReference02s.Where(x => x.Seq == condition.Seq2 && x.ShipperId == Profile.User.ShipperId && x.IsCheck).Count();

            // Excute paging
            return new StaticPagedList<Reference02ResultRow>(stockInquirys, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Insert Work03 Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertInvReference03(Reference02SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    // 1.ワークID採番
                    condition.Seq3 = new BaseQuery().GetWorkId();
                    condition.Page = 1;

                    StringBuilder query = new StringBuilder(@"
                        INSERT INTO WW_INV_REFERENCE03(
                               MAKE_DATE
                              ,MAKE_USER_ID
                              ,MAKE_PROGRAM_NAME
                              ,UPDATE_DATE
                              ,UPDATE_USER_ID
                              ,UPDATE_PROGRAM_NAME
                              ,UPDATE_COUNT
                              ,SHIPPER_ID
                              ,SEQ
                              ,LINE_NO
                              ,CENTER_ID
                              ,IS_CHECK
                              ,INVENTORY_NO
                              ,INVENTORY_START_DATE
                              ,INVENTORY_CLASS
                              ,INVENTORY_NAME
                              ,SIMPLE_INVENTORY_FLAG
                              ,INVENTORY_STATUS
                              ,INVENTORY_CONFIRM_FLAG
                              ,INVENTORY_CONFIRM_DATE
                              ,LOCATION_CD
                              ,ITEM_SKU_ID
                              ,ITEM_ID
                              ,ITEM_NAME
                              ,JAN
                              ,ITEM_COLOR_ID
                              ,ITEM_SIZE_ID
                              ,CASE_CLASS
                              ,GRADE_ID
                              ,BOX_NO
                              ,STOCK_QTY_START
                              ,RESULT_QTY
                              ,DIFFERENCE_PLUS
                              ,DIFFERENCE_MINUS
                              ,COUNT_SEQ
                              ,USER_ID)
                        SELECT 
                              " + "SYSTIMESTAMP " + " AS MAKE_DATE" +
                              ", '" + Common.Profile.User.UserId + "' AS MAKE_USER_ID" +
                              ",'InputReference'" + " AS MAKE_PROGRAM_NAME" +
                              ",SYSTIMESTAMP " + "AS UPDATE_DATE" +
                              ", '" + Common.Profile.User.UserId + "' AS UPDATE_USER_ID" +
                              ",'InputReference'" + " AS UPDATE_PROGRAM_NAME" +
                              ",0" + " AS UPDATE_COUNT" +
                              "," + "'" + Common.Profile.User.ShipperId + "'" + " AS SHIPPER_ID" +
                              "," + condition.Seq3 + " AS SEQ");
                    query.Append(@"
                              ,ROWNUM AS LINE_NO
                              ,CENTER_ID
                              ,0 AS IS_CHECK
                              ,INVENTORY_NO
                              ,INVENTORY_START_DATE
                              ,INVENTORY_CLASS
                              ,INVENTORY_NAME
                              ,SIMPLE_INVENTORY_FLAG
                              ,CASE WHEN INVENTORY_CONFIRM_FLAG = 1 AND  DIFFERENCE_LIST_FLAG = 0 THEN '" + ReferenceResource.Counting + @"'
                                    WHEN INVENTORY_CONFIRM_FLAG = 1 AND  DIFFERENCE_LIST_FLAG = 1 THEN '" + ReferenceResource.Investigating + @"'
                                    WHEN INVENTORY_CONFIRM_FLAG = 2 THEN '" + ReferenceResource.ProvisionalDecision + @"'
                                    WHEN INVENTORY_CONFIRM_FLAG = 3 THEN '" + ReferenceResource.DefiniteDecision + @"' END INVENTORY_STATUS
                              ,INVENTORY_CONFIRM_FLAG
                              ,INVENTORY_CONFIRM_DATE
                              ,LOCATION_CD
                              ,ITEM_SKU_ID
                              ,ITEM_ID
                              ,ITEM_NAME
                              ,JAN
                              ,ITEM_COLOR_ID
                              ,ITEM_SIZE_ID
                              ,CASE_CLASS
                              ,GRADE_ID
                              ,BOX_NO
                              ,STOCK_QTY_START
                              ,RESULT_QTY
                              ,DIFFERENCE_PLUS
                              ,DIFFERENCE_MINUS
                              ,COUNT_SEQ
                              ,USER_ID
                        FROM (
                                SELECT TP.CENTER_ID CENTER_ID
                                      ,TP.INVENTORY_NO INVENTORY_NO
                                      ,TP.INVENTORY_START_DATE INVENTORY_START_DATE
                                      ,TP.INVENTORY_CLASS INVENTORY_CLASS
                                      ,TP.INVENTORY_NAME INVENTORY_NAME
                                      ,TP.SIMPLE_INVENTORY_FLAG SIMPLE_INVENTORY_FLAG
                                      ,TP.INVENTORY_CONFIRM_FLAG INVENTORY_CONFIRM_FLAG
                                      ,TP.INVENTORY_CONFIRM_DATE INVENTORY_CONFIRM_DATE
                                      ,TP.DIFFERENCE_LIST_FLAG DIFFERENCE_LIST_FLAG
                                      ,TP.LOCATION_CD LOCATION_CD
                                      ,TP.ITEM_SKU_ID ITEM_SKU_ID
                                      ,TP.ITEM_ID ITEM_ID
                                      ,TP.ITEM_NAME ITEM_NAME
                                      ,TP.JAN JAN
                                      ,TP.ITEM_COLOR_ID ITEM_COLOR_ID
                                      ,TP.ITEM_SIZE_ID ITEM_SIZE_ID
                                      ,TP.CASE_CLASS CASE_CLASS
                                      ,TP.GRADE_ID GRADE_ID
                                      ,TP.BOX_NO BOX_NO
                                      ,TP.STOCK_QTY_START STOCK_QTY_START
                                      ,TR.RESULT_QTY RESULT_QTY
                                      ,CASE WHEN TR.RESULT_QTY - TP.STOCK_QTY_START > 0 THEN TR.RESULT_QTY - TP.STOCK_QTY_START ELSE 0 END DIFFERENCE_PLUS
                                      ,CASE WHEN TR.RESULT_QTY - TP.STOCK_QTY_START < 0 THEN TR.RESULT_QTY - TP.STOCK_QTY_START ELSE 0 END DIFFERENCE_MINUS
                                      ,NVL(TR.COUNT_SEQ,0) COUNT_SEQ
                                      ,TR.MAKE_USER_ID USER_ID
                                  FROM T_INVENTORY_PLANS TP
                                  LEFT JOIN
                                       T_INVENTORY_RESULTS TR
                                  ON
                                       TP.SHIPPER_ID = TR.SHIPPER_ID
                                  AND  TP.CENTER_ID = TR.CENTER_ID
                                  AND  TP.INVENTORY_NO = TR.INVENTORY_NO
                                  AND  TP.INVENTORY_SEQ = TR.INVENTORY_SEQ
                                  AND  TR.LAST_COUNT_FLAG = 1
                                 
                                 WHERE TP.SHIPPER_ID = :SHIPPER_ID
                                   AND TP.CENTER_ID = :CENTER_ID
                    ");

                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                    parameters.Add(":CENTER_ID", condition.CenterId);

                    // Add search condition
                    // 棚卸No
                    if (!string.IsNullOrEmpty(condition.InventoryNo))
                    {
                        query.Append(" AND TP.INVENTORY_NO = :INVENTORY_NO ");
                        parameters.Add(":INVENTORY_NO", condition.InventoryNo);
                    }

                    // エリア
                    if (!string.IsNullOrEmpty(condition.Area))
                    {
                        query.Append(" AND SUBSTR(TP.LOCATION_CD, 1, 3) = :AREA ");
                        parameters.Add(":AREA", condition.Area);
                    }

                    // 棚列
                    if (!string.IsNullOrEmpty(condition.InventoryRow))
                    {
                        query.Append(" AND SUBSTR(TP.LOCATION_CD, 5, 3) = :INVENTORY_ROW ");
                        parameters.Add(":INVENTORY_ROW", condition.InventoryRow);
                    }

                    // ロケーション
                    if (!string.IsNullOrEmpty(condition.LocationCdFrom))
                    {
                        query.Append(" AND TP.LOCATION_CD >= :LOCATION_CD_FROM ");
                        parameters.Add(":LOCATION_CD_FROM", condition.LocationCdFrom);
                    }

                    // ロケーション
                    if (!string.IsNullOrEmpty(condition.LocationCdTo))
                    {
                        query.Append(" AND TP.LOCATION_CD <= :LOCATION_CD_TO ");
                        parameters.Add(":LOCATION_CD_TO", condition.LocationCdTo);
                    }

                    // 格付
                    if (!string.IsNullOrEmpty(condition.GradeId))
                    {
                        query.Append(" AND TP.GRADE_ID = :GRADE_ID ");
                        parameters.Add(":GRADE_ID", condition.GradeId);
                    }

                    // 品番
                    if (!string.IsNullOrEmpty(condition.ItemId))
                    {
                        query.Append(" AND TP.ITEM_ID LIKE :ITEM_ID ");
                        parameters.Add(":ITEM_ID", condition.ItemId + "%");
                    }

                    // ケースNo
                    if (!string.IsNullOrEmpty(condition.BoxNo))
                    {
                        query.Append(" AND TP.BOX_NO LIKE :BOX_NO ");
                        parameters.Add(":BOX_NO", condition.BoxNo + "%");
                    }

                    // SKU
                    if (!string.IsNullOrEmpty(condition.ItemSkuId))
                    {
                        query.Append(" AND TP.ITEM_SKU_ID LIKE :ITEM_SKU_ID ");
                        parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId + "%");
                    }

                    // JAN
                    if (!string.IsNullOrEmpty(condition.Jan))
                    {
                        query.Append(" AND TP.JAN LIKE :JAN ");
                        parameters.Add(":JAN", condition.Jan + "%");
                    }

                    query.Append(" ) ");

                    // 2.検索・ワーク作成
                     var result = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);

                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }
                trans.Commit();
            }


            if (condition.InventoryStatusOld)
            {
                var deleteUserProgram = new StringBuilder();

                // 共通ヘッダ
                deleteUserProgram.Append(@"
                DELETE FROM
                        WW_INV_REFERENCE03
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                AND     SEQ = :SEQ
                AND     DIFFERENCE_PLUS = 0
                AND     DIFFERENCE_MINUS = 0");

                // 荷主ID
                parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

                // ワークID
                parameters.Add(":SEQ", condition.Seq3);

                using (var trans = MvcDbContext.Current.Database.BeginTransaction())
                {

                    MvcDbContext.Current.Database.Connection.Execute(deleteUserProgram.ToString(), parameters);

                    try
                    {
                        MvcDbContext.Current.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return false;
                    }

                    trans.Commit();
                }
            }

            return true;
        }

        /// <summary>
        /// Insert Work04 Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertInvReferenceSku04(Reference02SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                INSERT 
                INTO WW_INV_REFERENCE04( 
                    MAKE_USER_ID
                    , MAKE_PROGRAM_NAME
                    , UPDATE_USER_ID
                    , UPDATE_PROGRAM_NAME
                    , SHIPPER_ID
                    , SEQ
                    , CENTER_ID
                    , INVENTORY_NO
                    , LOCATION_COUNT
                    , STOCK_SKU
                    , RESULT_SKU
                    , STOCK_CASE
                    , RESULT_CASE
                    , STOCK_QTY
                    , RESULT_QTY
                ) 
                SELECT
                    :USER_ID
                    , :PROGRAM_ID
                    , :USER_ID
                    , :PROGRAM_ID
                    , :SHIPPER_ID
                    , :SEQ
                    , TP.CENTER_ID CENTER_ID
                    , TP.INVENTORY_NO INVENTORY_NO
                    , COUNT(DISTINCT TP.LOCATION_CD) LOCATION_COUNT
                    , COUNT(DISTINCT TI.ITEM_SKU_ID) STOCK_SKU
                    , COUNT(DISTINCT TR.ITEM_SKU_ID) RESULT_SKU
                    , COUNT(DISTINCT TI1.BOX_NO) STOCK_CASE
                    , COUNT(DISTINCT TR1.BOX_NO) RESULT_CASE
                    , SUM(TP.STOCK_QTY_START) STOCK_QTY
                    , NVL(SUM(TR.RESULT_QTY), 0) RESULT_QTY
                FROM
                    T_INVENTORY_PLANS TP 
                    LEFT JOIN T_INVENTORY_RESULTS TR 
                        ON TP.SHIPPER_ID = TR.SHIPPER_ID 
                        AND TP.CENTER_ID = TR.CENTER_ID 
                        AND TP.INVENTORY_NO = TR.INVENTORY_NO 
                        AND TP.INVENTORY_SEQ = TR.INVENTORY_SEQ 
                        AND TR.LAST_COUNT_FLAG = 1 
                    LEFT JOIN T_INVENTORY_RESULTS TR1 
                        ON TP.SHIPPER_ID = TR1.SHIPPER_ID 
                        AND TP.CENTER_ID = TR1.CENTER_ID 
                        AND TP.INVENTORY_NO = TR1.INVENTORY_NO 
                        AND TP.INVENTORY_SEQ = TR1.INVENTORY_SEQ 
                        AND TR1.BOX_NO <> ' ' 
                        AND TR1.LAST_COUNT_FLAG = 1 
                    LEFT JOIN T_INVENTORY_PLANS TI 
                        ON TI.SHIPPER_ID = TP.SHIPPER_ID 
                        AND TI.CENTER_ID = TP.CENTER_ID 
                        AND TI.INVENTORY_NO = TP.INVENTORY_NO 
                        AND TI.INVENTORY_SEQ = TP.INVENTORY_SEQ 
                        AND TI.STOCK_FLAG = 1 
                    LEFT JOIN T_INVENTORY_PLANS TI1 
                        ON TI1.SHIPPER_ID = TP.SHIPPER_ID 
                        AND TI1.CENTER_ID = TP.CENTER_ID 
                        AND TI1.INVENTORY_NO = TP.INVENTORY_NO 
                        AND TI1.INVENTORY_SEQ = TP.INVENTORY_SEQ 
                        AND TI1.BOX_NO <> ' ' 
                        AND TI1.STOCK_FLAG = 1 
                WHERE
                        TP.SHIPPER_ID = :SHIPPER_ID 
                    AND TP.CENTER_ID = :CENTER_ID 
                    AND TP.INVENTORY_NO = :INVENTORY_NO 
                GROUP BY
                      TP.CENTER_ID
                    , TP.INVENTORY_NO

            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":INVENTORY_NO", condition.InventoryNo);

            // 1.ワークID採番
            condition.Seq4 = new BaseQuery().GetWorkId();
            condition.Page = 1;
            parameters.AddDynamicParams(new
            {
                USER_ID = Profile.User.UserId,
                PROGRAM_ID = GetProgramId(),
                SEQ = condition.Seq4
            });


            // 2.検索・ワーク作成
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    var result = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }

                trans.Commit();

            }

            return true;
        }

        /// <summary>
        /// Get Head Data
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public Reference02Head GetReference03Head(Reference02SearchConditions condition)
        {
            return GetReference02Head(condition);
        }

        /// <summary>
        /// Get Head Data
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public Reference02Total GetReference03Total(Reference02SearchConditions condition)
        {
            Reference02Total rt = new Reference02Total();
            //Total
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
               SELECT  LOCATION_COUNT
                      ,STOCK_SKU
                      ,RESULT_SKU
                      ,STOCK_CASE
                      ,RESULT_CASE
                      ,STOCK_QTY
                      ,RESULT_QTY
                 FROM WW_INV_REFERENCE04
                WHERE SHIPPER_ID = :SHIPPER_ID
                  AND SEQ = :SEQ
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq4);
            rt = new Reference02Total();
            rt = MvcDbContext.Current.Database.Connection.Query<Reference02Total>(query.ToString(), parameters).FirstOrDefault();

            return rt;
        }

        /// <summary>
        /// Get Work02 List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<Reference02ResultRow> GetSkuListData(Reference02SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
               SELECT  WW.LOCATION_CD
                      ,CASE WHEN WW.BOX_NO = ' ' THEN '" + ReferenceResource.Rose + @"'
                                                 ELSE '" + ReferenceResource.Cases + @"' END CASE_CLASS
                      ,MG.GRADE_NAME GRADE_ID
                      ,WW.BOX_NO
                      ,WW.ITEM_ID
                      ,WW.ITEM_NAME
                      ,WW.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,WW.ITEM_SIZE_ID
                      ,MIS.ITEM_SIZE_NAME
                      ,WW.JAN
                      ,WW.STOCK_QTY_START
                      ,WW.RESULT_QTY
                      ,CASE WHEN WW.DIFFERENCE_PLUS = 0 THEN NULL ELSE WW.DIFFERENCE_PLUS END DIFFERENCE_PLUS
                      ,CASE WHEN WW.DIFFERENCE_MINUS = 0 THEN NULL ELSE WW.DIFFERENCE_MINUS END DIFFERENCE_MINUS
                      ,WW.COUNT_SEQ
                      ,WW.USER_ID
                      ,MU.USER_NAME
                      ,WW.SEQ
                      ,WW.LINE_NO
                      ,WW.IS_CHECK
                 FROM WW_INV_REFERENCE03 WW
                LEFT JOIN
                      M_GRADES MG
                   ON WW.SHIPPER_ID = MG.SHIPPER_ID
                  AND WW.GRADE_ID = MG.GRADE_ID
                LEFT JOIN
                      M_COLORS MC
                   ON WW.SHIPPER_ID = MC.SHIPPER_ID
                  AND WW.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                LEFT JOIN
                      M_SIZES MS
                   ON WW.SHIPPER_ID = MS.SHIPPER_ID
                  AND WW.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                LEFT JOIN
                      M_ITEM_SKU MIS
                   ON WW.SHIPPER_ID = MIS.SHIPPER_ID
                  AND WW.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                LEFT JOIN
                      M_USERS MU
                   ON WW.SHIPPER_ID = MU.SHIPPER_ID
                  AND WW.USER_ID = MU.USER_ID
                WHERE WW.SHIPPER_ID = :SHIPPER_ID
                  AND WW.SEQ = :SEQ
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq3);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<Reference02ResultRow>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey03)
            {
                case Reference03SortKey.LocationCaseItemColorSize:
                    switch (condition.Sort)
                    {
                        case DetailAscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WW.LOCATION_CD DESC, WW.CASE_CLASS DESC, WW.BOX_NO DESC, WW.ITEM_ID DESC, WW.ITEM_COLOR_ID DESC, WW.ITEM_SIZE_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WW.LOCATION_CD ASC, WW.CASE_CLASS ASC, WW.BOX_NO ASC, WW.ITEM_ID ASC, WW.ITEM_COLOR_ID ASC, WW.ITEM_SIZE_ID ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case DetailAscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WW.ITEM_ID DESC, WW.ITEM_COLOR_ID DESC, WW.ITEM_SIZE_ID DESC, WW.LOCATION_CD DESC, WW.CASE_CLASS DESC, WW.BOX_NO DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WW.ITEM_ID ASC, WW.ITEM_COLOR_ID ASC, WW.ITEM_SIZE_ID ASC, WW.LOCATION_CD ASC, WW.CASE_CLASS ASC, WW.BOX_NO ASC");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var Reference02 = MvcDbContext.Current.Database.Connection.Query<Reference02ResultRow>(query.ToString(), parameters);

            // Fill data to memory
            var stockInquirys = MvcDbContext.Current.Database.Connection.Query<Reference02ResultRow>(query.ToString(), parameters);
            condition.SelectedCnt = MvcDbContext.Current.InvReference03s.Where(x => x.Seq == condition.Seq3 && x.ShipperId == Profile.User.ShipperId && x.IsCheck).Count();

            // Excute paging
            return new StaticPagedList<Reference02ResultRow>(stockInquirys, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// 仮確定
        /// </summary>
        public void InventoryConfirm(string inventoryNo, string centerId,out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", centerId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_INVENTORY_NO", inventoryNo, DbType.String, ParameterDirection.Input);
            param.Add("IN_CONFIRM_FLAG", 2, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_EXECUTE_FLAG", 0, DbType.Int32, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_INV_CONFIRM",
                param,
                commandType: CommandType.StoredProcedure);

            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }

        /// <summary>
        /// もう一度仮確定
        /// </summary>
        public void InventoryAgainConfirm(string inventoryNo, string centerId, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", centerId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_INVENTORY_NO", inventoryNo, DbType.String, ParameterDirection.Input);
            param.Add("IN_CONFIRM_FLAG", 2, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_EXECUTE_FLAG", 1, DbType.Int32, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_INV_CONFIRM",
                param,
                commandType: CommandType.StoredProcedure);

            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }

        /// <summary>
        /// 棚卸取消
        /// </summary>
        public void InventoryCancel(string inventoryNo, string centerId, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", centerId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_INVENTORY_NO", inventoryNo, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_INV_CANCEL",
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
        public bool UpdateInvReference01s(IList<SelectedReferenceViewModel> References)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in References)
                {
                    // 在庫明細
                    var reference = MvcDbContext.Current.InvReference01s
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
        public bool InvReferenceAllChange(Reference01SearchConditions conditions, bool check)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in MvcDbContext.Current.InvReference01s
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

            conditions.SelectedCnt = MvcDbContext.Current.InvReference01s
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq && m.IsCheck).Count();
            return true;
        }

        /// <summary>
        /// 棚卸Noデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListInventoryNo(Reference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                WITH
                    INVENTORY_DATA AS (
                        SELECT 
                                TP.CENTER_ID
                            ,   TP.INVENTORY_NO 
                            ,   TP.INVENTORY_NAME
                            ,   TP.INVENTORY_START_DATE
                            ,   TP.INVENTORY_CLASS
                            ,   TP.INVENTORY_CONFIRM_FLAG
                            ,   TP.DIFFERENCE_LIST_FLAG
                            ,   TP.LOCATION_CD
                            ,   TP.ITEM_SKU_ID
                        FROM
                                T_INVENTORY_PLANS TP
                        WHERE
                                TP.SHIPPER_ID = :SHIPPER_ID

                        UNION ALL

                        SELECT
                                ML.CENTER_ID
                            ,   ML.INVENTORY_NO
                            ,   ML.INVENTORY_NAME
                            ,   ML.INVENTORY_START_DATE
                            ,   2 INVENTORY_CLASS
                            ,   ML.INVENTORY_CONFIRM_FLAG
                            ,   0 DIFFERENCE_LIST_FLAG
                            ,   ML.LOCATION_CD
                            ,   NULL ITEM_SKU_ID
                        FROM
                                M_LOCATIONS ML
                        WHERE
                                ML.SHIPPER_ID = :SHIPPER_ID
                            AND ML.INVENTORY_NO IS NOT NULL
                            AND ML.INVENTORY_CONFIRM_FLAG > 0
                            AND NOT EXISTS (
                                    SELECT
                                            *
                                    FROM
                                            T_INVENTORY_PLANS TP
                                    WHERE
                                            TP.SHIPPER_ID = ML.SHIPPER_ID
                                        AND TP.CENTER_ID = ML.CENTER_ID
                                        AND TP.INVENTORY_NO = ML.INVENTORY_NO
                                )
                    )
                SELECT DISTINCT 
                        INVENTORY_NO VALUE
                    ,   INVENTORY_NO || ':' || INVENTORY_NAME TEXT
                    ,   INVENTORY_START_DATE
                FROM 
                        INVENTORY_DATA
                WHERE
                        1 = 1
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

            // Add search condition
            // センター
            if (!string.IsNullOrEmpty(condition.CenterId))
            {
                query.Append(" AND CENTER_ID = :CENTER_ID ");
                parameters.Add(":CENTER_ID", condition.CenterId);
            }

            // 棚卸区分
            if (!string.IsNullOrEmpty(condition.InventoryClass))
            {
                query.Append(" AND INVENTORY_CLASS = :INVENTORY_CLASS ");    //2:循環棚卸
                parameters.Add(":INVENTORY_CLASS", condition.InventoryClass);
            }

            // 棚卸名称
            if (!string.IsNullOrEmpty(condition.InventoryName))
            {
                query.Append(" AND INVENTORY_NAME LIKE :INVENTORY_NAME ");
                parameters.Add(":INVENTORY_NAME", condition.InventoryName + "%");
            }

            // 棚卸開始日
            if (condition.InventoryDateFrom != null)
            {
                query.Append(" AND TRUNC(INVENTORY_START_DATE) >= :INVENTORY_START_DATE_FROM ");
                parameters.Add(":INVENTORY_START_DATE_FROM", condition.InventoryDateFrom);
            }

            if (condition.InventoryDateTo != null)
            {
                query.Append(" AND TRUNC(INVENTORY_START_DATE) <= :INVENTORY_START_DATE_TO ");
                parameters.Add(":INVENTORY_START_DATE_TO", condition.InventoryDateTo);
            }

            // 棚卸状況
            if (!string.IsNullOrEmpty(condition.InventoryStatus))
            {
                if (condition.InventoryStatus == "1")
                {
                    query.Append(" AND INVENTORY_CONFIRM_FLAG = :INVENTORY_CONFIRM_FLAG ");
                    parameters.Add(":INVENTORY_CONFIRM_FLAG", 1);
                    query.Append(" AND DIFFERENCE_LIST_FLAG = :DIFFERENCE_LIST_FLAG ");
                    parameters.Add(":DIFFERENCE_LIST_FLAG", 0);
                }
                else if (condition.InventoryStatus == "2")
                {
                    query.Append(" AND INVENTORY_CONFIRM_FLAG = :INVENTORY_CONFIRM_FLAG ");
                    parameters.Add(":INVENTORY_CONFIRM_FLAG", 1);
                    query.Append(" AND DIFFERENCE_LIST_FLAG = :DIFFERENCE_LIST_FLAG ");
                    parameters.Add(":DIFFERENCE_LIST_FLAG", 1);
                }
                else if (condition.InventoryStatus == "3")
                {
                    query.Append(" AND INVENTORY_CONFIRM_FLAG = :INVENTORY_CONFIRM_FLAG ");
                    parameters.Add(":INVENTORY_CONFIRM_FLAG", 2);
                }
                else if (condition.InventoryStatus == "4")
                {
                    query.Append(" AND INVENTORY_CONFIRM_FLAG = :INVENTORY_CONFIRM_FLAG ");
                    parameters.Add(":INVENTORY_CONFIRM_FLAG", 3);
                }
            }

            // ロケーション
            if (!string.IsNullOrEmpty(condition.LocationCd))
            {
                query.Append(" AND LOCATION_CD LIKE :LOCATION_CD ");
                parameters.Add(":LOCATION_CD", condition.LocationCd + "%");
            }

            // SKU
            if (!string.IsNullOrEmpty(condition.ItemSkuId))
            {
                query.Append(" AND ITEM_SKU_ID LIKE :ITEM_SKU_ID ");
                parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId + "%");
            }

            query.Append(" ORDER BY INVENTORY_START_DATE DESC ");

            // 全レコード数を取得
            return MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters);
        }

        /// <summary>
        /// 棚卸Noデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectInventoryNoList(string InventoryClass, string InventoryName, string InventoryDateFrom, string InventoryDateTo, string InventoryStatus, string Location, string Sku, string CenterId)
        {
            DateTime? invDateFrom = null, invDateTo = null;

            if (!string.IsNullOrEmpty(InventoryDateFrom))
            {
                invDateFrom = DateTime.Parse(InventoryDateFrom);
            }

            if (!string.IsNullOrEmpty(InventoryDateTo))
            {
                invDateTo = DateTime.Parse(InventoryDateTo);
            }

            return GetSelectListInventoryNo(new Reference01SearchConditions() 
            {
                InventoryClass = InventoryClass,
                InventoryName = InventoryName,
                InventoryDateFrom = invDateFrom,
                InventoryDateTo = invDateTo,
                InventoryStatus = InventoryStatus,
                LocationCd = Location,
                ItemSkuId = Sku,
                CenterId = CenterId
            });
        }

        /// <summary>
        /// 格付データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListGrades()
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT GRADE_ID VALUE
                      ,GRADE_NAME TEXT
                      ,DISPLAY_ORDER
                  FROM M_GRADES
                 WHERE SHIPPER_ID = :SHIPPER_ID
                 ORDER BY
                       DISPLAY_ORDER
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

            // 全レコード数を取得
            return MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters);

        }

        /// <summary>
        /// 差異リスト更新
        /// </summary>
        /// <param name="References"></param>
        /// <returns></returns>
        public bool UpdateInvReference(long seq,string mark,string centerId)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (System.Data.Common.DbTransaction tran = MvcDbContext.Current.Database.Connection.BeginTransaction())
            {
                try
                {
                        if (mark == "Detail")
                        {

                            StringBuilder sql = new StringBuilder();

                            // 更新用
                            sql.Append(@"
                                  UPDATE T_INVENTORY_PLANS PLANS
                                     SET PLANS.UPDATE_DATE           = SYSTIMESTAMP,
                                         PLANS.UPDATE_USER_ID        = :UPDATE_USER_ID,
                                         PLANS.UPDATE_PROGRAM_NAME   = :UPDATE_PROGRAM_NAME,
                                        PLANS.UPDATE_COUNT           = PLANS.UPDATE_COUNT + 1,
                                        PLANS.DIFFERENCE_LIST_FLAG   = 1,
                                        PLANS.DIFFERENCE_LIST_DATE   = SYSDATE,
                                        PLANS.DIFFERENCE_LIST_USER_ID = :UPDATE_USER_ID
                                        WHERE PLANS.SHIPPER_ID       = :SHIPPER_ID
                                        AND PLANS.CENTER_ID          = :CENTER_ID
                                        AND PLANS.INVENTORY_NO IN (
                                          SELECT REF01.INVENTORY_NO
                                          FROM WW_INV_REFERENCE01 REF01
                                          WHERE REF01.SHIPPER_ID = :SHIPPER_ID
                                            AND REF01.SEQ = :SEQ
                                            AND REF01.IS_CHECK = 1
                                        )");
                       
                             parameters.Add(":UPDATE_USER_ID", Common.Profile.User.UserId);
                            parameters.Add(":UPDATE_PROGRAM_NAME", this.GetProgramId());
                            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                            parameters.Add(":CENTER_ID", centerId);
                            parameters.Add(":SEQ", seq);

                        MvcDbContext.Current.Database.Connection.Execute(sql.ToString(), parameters);
                     }else if (mark == "LocDetail")
                    {
                        StringBuilder sql = new StringBuilder();

                        // 更新用
                        sql.Append(@"
                                  UPDATE T_INVENTORY_PLANS PLANS
                                     SET PLANS.UPDATE_DATE           = SYSTIMESTAMP,
                                         PLANS.UPDATE_USER_ID        = :UPDATE_USER_ID,
                                         PLANS.UPDATE_PROGRAM_NAME   = :UPDATE_PROGRAM_NAME,
                                        PLANS.UPDATE_COUNT           = PLANS.UPDATE_COUNT + 1,
                                        PLANS.DIFFERENCE_LIST_FLAG   = 1,
                                        PLANS.DIFFERENCE_LIST_DATE   = SYSDATE,
                                        PLANS.DIFFERENCE_LIST_USER_ID = :UPDATE_USER_ID
                                        WHERE PLANS.SHIPPER_ID       = :SHIPPER_ID
                                        AND PLANS.CENTER_ID          = :CENTER_ID
                                        AND PLANS.INVENTORY_NO IN (
                                          SELECT REF02.INVENTORY_NO
                                          FROM WW_INV_REFERENCE02 REF02
                                          WHERE REF02.SHIPPER_ID = :SHIPPER_ID
                                            AND REF02.SEQ = :SEQ
                                            --AND REF02.IS_CHECK = 1
                                        )");

                        parameters.Add(":UPDATE_USER_ID", Common.Profile.User.UserId);
                        parameters.Add(":UPDATE_PROGRAM_NAME", this.GetProgramId());
                        parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                        parameters.Add(":CENTER_ID", centerId);
                        parameters.Add(":SEQ", seq);

                        MvcDbContext.Current.Database.Connection.Execute(sql.ToString(), parameters);
                    }
                    else if (mark == "SkuDetail")
                    {
                        StringBuilder sql = new StringBuilder();

                        // 更新用
                        sql.Append(@"
                                  UPDATE T_INVENTORY_PLANS PLANS
                                     SET PLANS.UPDATE_DATE           = SYSTIMESTAMP,
                                         PLANS.UPDATE_USER_ID        = :UPDATE_USER_ID,
                                         PLANS.UPDATE_PROGRAM_NAME   = :UPDATE_PROGRAM_NAME,
                                        PLANS.UPDATE_COUNT           = PLANS.UPDATE_COUNT + 1,
                                        PLANS.DIFFERENCE_LIST_FLAG   = 1,
                                        PLANS.DIFFERENCE_LIST_DATE   = SYSDATE,
                                        PLANS.DIFFERENCE_LIST_USER_ID = :UPDATE_USER_ID
                                        WHERE PLANS.SHIPPER_ID       = :SHIPPER_ID
                                        AND PLANS.CENTER_ID          = :CENTER_ID
                                        AND PLANS.INVENTORY_NO IN (
                                          SELECT REF03.INVENTORY_NO
                                          FROM WW_INV_REFERENCE03 REF03
                                          WHERE REF03.SHIPPER_ID = :SHIPPER_ID
                                            AND REF03.SEQ = :SEQ
                                            --AND REF03.IS_CHECK = 1
                                        )");

                        parameters.Add(":UPDATE_USER_ID", Common.Profile.User.UserId);
                        parameters.Add(":UPDATE_PROGRAM_NAME", this.GetProgramId());
                        parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                        parameters.Add(":CENTER_ID", centerId);
                        parameters.Add(":SEQ", seq);

                        MvcDbContext.Current.Database.Connection.Execute(sql.ToString(), parameters);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return false;
                }

                tran.Commit();
            }

            return true;
           
        }

        /// <summary>
        /// Get Program Id
        /// </summary>
        /// <returns></returns>
        private string GetProgramId()
        {
            // Get roote data
            var rooteData = System.Web.HttpContext.Current.Request.RequestContext.RouteData;
            if (rooteData != null)
            {
                // Get controller name, action name
                string controllerName = rooteData.Values != null && rooteData.Values.ContainsKey("controller") ? rooteData.Values["controller"].ToString() : string.Empty;
                string actionName = rooteData.Values != null && rooteData.Values.ContainsKey("action") ? rooteData.Values["action"].ToString() : string.Empty;

                // In the case of controller name is not empty
                if (!string.IsNullOrEmpty(controllerName))
                {
                    // Get area name
                    string areaName = rooteData.DataTokens != null && rooteData.DataTokens.ContainsKey("area") ? rooteData.DataTokens["area"].ToString() : string.Empty;

                    // Return program id
                    if (areaName == string.Empty)
                    {
                        return controllerName + "/" + actionName;
                    }
                    else
                    {
                        return areaName + "/" + controllerName + "/" + actionName;
                    }
                }
            }

            return string.Empty;
        }
    }
}