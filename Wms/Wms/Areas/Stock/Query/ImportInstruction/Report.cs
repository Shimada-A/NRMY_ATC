namespace Wms.Areas.Stock.Query.ImportInstruction
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Extensions.Classes;
    using Share.Helpers;
    using Wms.Areas.Stock.Models;
    using Wms.Areas.Stock.Resources;
    using Wms.Areas.Stock.ViewModels.ImportInstruction;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Stock.ViewModels.ImportInstruction.ImportInstructionSearchConditions;

    public class Report : BaseQuery
    {
        /// <summary>
        /// Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<ImportInstructionReport> ImportInstructionListing(ImportInstructionSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                    SELECT 
                            '' AS SORT_CLASS
                        ,   '' AS JAN
                        ,   '' AS SORT_CATEGORY_NAME
                        ,   '' AS TRANSFER_NO
                        ,   '' AS STOCK_QTY
                        ,   '' AS NOTE
                    FROM DUAL
                ");

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<ImportInstructionReport>(query.ToString(), parameters);
        }

        /// <summary>
        /// アップロードされたデータのImport
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        public void InsertWW_SORT_STOCK_INS(IEnumerable<ViewModels.ImportInstruction.ImportInstructionOutputReport> report, out string message, out long workId, string pSortInstructName, string pCenterId)
        {
            var dbContext = MvcDbContext.Current;
            var nullitems = report.Where(x => string.IsNullOrWhiteSpace(x.SortClass.ToString()) || string.IsNullOrWhiteSpace(x.Jan));
            workId = GetWorkId();
            if (nullitems.Any())
            {
                message = ImportInstructionResource.ERR_NOT_MATCH;
            }
            else
            {
                using (var trans = dbContext.Database.BeginTransaction())
                {
                    foreach (var u in report.Select((v, i) => new { v, i }))
                    {
                        if (u.v.StockQty.ToString().Length == 0)
                        {
                            u.v.StockQty = 0;
                        }
                        var wwSortStockInstructs02 = new Models.SortStockInstructs02
                        {
                            Seq = workId,
                            CenterId = pCenterId,
                            LineNo = u.i + 1,
                            SortInstructName = pSortInstructName,
                            SortClass = u.v.SortClass,
                            SortCategoryName = u.v.SortCategoryName,
                            TransferNo = u.v.TransferNo,
                            Jan = u.v.Jan,
                            StockQty = u.v.StockQty,
                            Note = u.v.Note

                        };
                        wwSortStockInstructs02.SetBaseInfoInsert();
                        dbContext.SortStockInstructs02s.Add(wwSortStockInstructs02);
                    }
                    dbContext.SaveChanges();
                    trans.Commit();
                }
                message = string.Empty;
            }
        }

        /// <summary>
        /// ワーク登録データチェック
        /// </summary>
        /// <param name="workId"></param>
        public string CheckWkData(long workId, string pCenterId)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            int ret;
            int errRow;
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                //JAN重複チェック
                parameters = new DynamicParameters();
                query = new StringBuilder();
                query.Append(@"
                UPDATE WW_SORT_STOCK_INSTRUCTS02 WSSI2
                SET
                        WSSI2.UPDATE_DATE = SYSTIMESTAMP
                    ,   WSSI2.UPDATE_USER_ID = :USER_ID
                    ,   WSSI2.UPDATE_PROGRAM_NAME = :PROGRAM_NAME
                    ,   WSSI2.UPDATE_COUNT = UPDATE_COUNT + 1
                    ,   WSSI2.MESSAGE = :MESSAGE
                WHERE
                        WSSI2.SHIPPER_ID = :SHIPPER_ID
                    AND WSSI2.CENTER_ID  = :CENTER_ID
                    AND WSSI2.SEQ  = :SEQ
                    AND EXISTS (
                            SELECT
                                    *
                            FROM
                                    WW_SORT_STOCK_INSTRUCTS02 WSSI
                            WHERE
                                    WSSI.SHIPPER_ID = WSSI2.SHIPPER_ID
                                AND WSSI.CENTER_ID  = WSSI2.CENTER_ID
                                AND WSSI.SEQ  = WSSI2.SEQ
                            GROUP BY
                                    WSSI.JAN
                            HAVING
                                    COUNT(WSSI.JAN) > 1
                            )
                ");

                parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                parameters.Add(":CENTER_ID", pCenterId);
                parameters.Add(":SEQ", workId);
                parameters.Add(":USER_ID", Profile.User.UserId);
                parameters.Add(":PROGRAM_NAME", "ImportInstruct");
                parameters.Add(":MESSAGE", ImportInstructionResource.ERR_DUPLICATE_JAN);

                try
                {
                    ret = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                    MvcDbContext.Current.SaveChanges();
                    if (ret > 0)
                    {
                        trans.Commit();

                        parameters = new DynamicParameters();
                        query = new StringBuilder();
                        query.Append(@"
                            SELECT
                                    MAX(WSSI.LINE_NO) AS LINE_NO
                            FROM
                                    WW_SORT_STOCK_INSTRUCTS02 WSSI
                            WHERE
                                    WSSI.SHIPPER_ID = :SHIPPER_ID
                                AND WSSI.CENTER_ID  = :CENTER_ID
                                AND WSSI.SEQ  = :SEQ
                            GROUP BY
                                    WSSI.JAN
                            HAVING
                                    COUNT(WSSI.JAN) > 1
                            ORDER BY
                                    MAX(WSSI.LINE_NO)
                        ");
                        parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                        parameters.Add(":CENTER_ID", pCenterId);
                        parameters.Add(":SEQ", workId);

                        var result = MvcDbContext.Current.Database.Connection.Query<SortStockInstructs02>(query.ToString(), parameters).FirstOrDefault();
                        errRow = int.Parse(result.LineNo.ToString());
                        return string.Format(ImportInstructionResource.ERR_DUPLICATE_JAN_ROW, errRow);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    trans.Rollback();
                    return Wms.Resources.MessageResource.ERROR_END;
                }

                //品番SKU存在チェック
                parameters = new DynamicParameters();
                query = new StringBuilder();
                query.Append(@"
                UPDATE WW_SORT_STOCK_INSTRUCTS02 WSSI
                SET
                        WSSI.UPDATE_DATE = SYSTIMESTAMP
                    ,   WSSI.UPDATE_USER_ID = :USER_ID
                    ,   WSSI.UPDATE_PROGRAM_NAME = :PROGRAM_NAME
                    ,   WSSI.UPDATE_COUNT = UPDATE_COUNT + 1
                    ,   WSSI.MESSAGE = :MESSAGE
                WHERE
                        WSSI.SHIPPER_ID = :SHIPPER_ID
                    AND WSSI.CENTER_ID  = :CENTER_ID
                    AND WSSI.SEQ  = :SEQ
                    AND NOT EXISTS(
                            SELECT
                                *
                            FROM
                                M_ITEM_SKU MIS
                            WHERE
                                    MIS.SHIPPER_ID = WSSI.SHIPPER_ID
                                AND MIS.JAN = WSSI.JAN
                    )
                ");

                parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                parameters.Add(":CENTER_ID", pCenterId);
                parameters.Add(":SEQ", workId);
                parameters.Add(":USER_ID", Profile.User.UserId);
                parameters.Add(":PROGRAM_NAME", "ImportInstruct");
                parameters.Add(":MESSAGE", ImportInstructionResource.ERR_NOTHING_M_ITEM_SKU);
                try
                {
                    ret = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                    MvcDbContext.Current.SaveChanges();
                    if (ret > 0)
                    {
                        trans.Commit();

                        parameters = new DynamicParameters();
                        query = new StringBuilder();
                        query.Append(@"
                            SELECT
                                    WSSI.LINE_NO
                            FROM WW_SORT_STOCK_INSTRUCTS02 WSSI
                            WHERE
                                    WSSI.SHIPPER_ID = :SHIPPER_ID
                                AND WSSI.CENTER_ID  = :CENTER_ID
                                AND WSSI.SEQ  = :SEQ
                                AND NOT EXISTS(
                                        SELECT
                                            *
                                        FROM
                                            M_ITEM_SKU MIS
                                        WHERE
                                                MIS.SHIPPER_ID = WSSI.SHIPPER_ID
                                            AND MIS.JAN = WSSI.JAN
                                )
                            ORDER BY
                                WSSI.LINE_NO
                            ");

                        parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                        parameters.Add(":CENTER_ID", pCenterId);
                        parameters.Add(":SEQ", workId);
                        var result = MvcDbContext.Current.Database.Connection.Query<SortStockInstructs02>(query.ToString(), parameters).FirstOrDefault();
                        errRow = int.Parse(result.LineNo.ToString());
                        return string.Format(ImportInstructionResource.ERR_NOTHING_M_ITEM_SKU_ROW, errRow);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    trans.Rollback();
                    return Wms.Resources.MessageResource.ERROR_END;
                }
                trans.Commit();
            }

            return "";
        }

        /// <summary>
        /// 在庫仕分カンバン(鑑)用データ
        /// </summary>
        /// <param name="search"></param>
        public IEnumerable<PrintStockSortKanban> GetStockSortKanban(ImportInstructionSearchConditions condition, int pSeq, string pSortInstrustNo)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                    SELECT 
                            SORT_INSTRUCT_NO
                        ,   SORT_INSTRUCT_NAME
                        ,   SORT_INSTRUCT_NO AS SORT_INSTRUCT_NO_BARCODE
                    FROM 
                            WW_SORT_STOCK_INSTRUCTS01
                    WHERE
                            SHIPPER_ID = :SHIPPER_ID
                        AND SEQ = :SEQ
                        AND CENTER_ID = :CENTER_ID
                        AND SORT_INSTRUCT_NO = :SORT_INSTRUCT_NO
                    ORDER BY
                            SORT_INSTRUCT_NO
                ");

            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SEQ", pSeq);
            parameters.Add(":SORT_INSTRUCT_NO", pSortInstrustNo);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<PrintStockSortKanban>(query.ToString(), parameters);
        }

        /// <summary>
        /// 在庫仕分カンバン(SKU別)用データ
        /// </summary>
        /// <param name="search"></param>
        public string InsStockSortKanbanSKU(long pSeq)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                    INSERT INTO WW_SORT_STOCK_INSTRUCTS03
                    (
                            MAKE_DATE
                        ,   MAKE_USER_ID
                        ,   MAKE_PROGRAM_NAME
                        ,   UPDATE_DATE
                        ,   UPDATE_USER_ID
                        ,   UPDATE_PROGRAM_NAME
                        ,   SHIPPER_ID
                        ,   SEQ
                        ,   LINE_NO
                        ,   LINE_NO_SEQ
                        ,   CENTER_ID
                        ,   SORT_INSTRUCT_NO
                        ,   SORT_INSTRUCT_NAME
                        ,   SORT_CLASS
                        ,   SORT_CATEGORY_NAME 
                        ,   TRANSFER_NO
                        ,   CATEGORY_NAME1
                        ,   JAN_1
                        ,   JAN_2
                        ,   ITEM_ID
                        ,   ITEM_COLOR
                        ,   ITEM_SIZE
                        ,   NOTE
                        ,   STOCK_QTY
                    )
                    SELECT 
                            SYSTIMESTAMP
                        ,   :USER_ID
                        ,   'ImportInstruct'
                        ,   SYSTIMESTAMP
                        ,   :USER_ID
                        ,   'ImportInstruct'
                        ,   :SHIPPER_ID
                        ,   :SEQ
                        ,   MAX(WSSI.LINE_NO)
                        ,   ROW_NUMBER() OVER(ORDER BY WSSI.SORT_INSTRUCT_NO, TSSI.SORT_CLASS, TSSI.TRANSFER_NO)
                        ,   MAX(WSSI.CENTER_ID)
                        ,   WSSI.SORT_INSTRUCT_NO
                        ,   MAX(WSSI.SORT_INSTRUCT_NAME) AS SORT_INSTRUCT_NAME
                        ,   TSSI.SORT_CLASS
                        ,   MAX(TSSI.SORT_CATEGORY_NAME) AS SORT_CATEGORY_NM
                        ,   TSSI.TRANSFER_NO
                        ,   MAX(MIC.CATEGORY_NAME1) AS BUNRUI_NM
                        ,   SUBSTR(TSSI.JAN,1,7) AS JAN1
                        ,   SUBSTR(TSSI.JAN,8,6) AS JAN2
                        ,   MAX(TSSI.ITEM_ID) AS ITEM_CD
                        ,   MAX(TSSI.ITEM_COLOR_ID) || ' ' || MAX(MC.ITEM_COLOR_NAME) AS ITEM_COLOR
                        ,   MAX(MIS.ITEM_SIZE_NAME) AS ITEM_SIZE
                        ,   MAX(TSSI.NOTE) AS NOTE
                        ,   SUM(TSSI.STOCK_QTY) AS NUM_STOCK
                    FROM 
                            WW_SORT_STOCK_INSTRUCTS01 WSSI
                    INNER JOIN
                            T_SORT_STOCK_INSTRUCTS TSSI
                    ON
                            WSSI.SHIPPER_ID = TSSI.SHIPPER_ID
                        AND WSSI.CENTER_ID = TSSI.CENTER_ID
                        AND WSSI.SORT_INSTRUCT_NO = TSSI.SORT_INSTRUCT_NO
                    LEFT OUTER JOIN
                            M_ITEM_SKU MIS
                    ON
                            TSSI.SHIPPER_ID = MIS.SHIPPER_ID
                        AND TSSI.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                        AND MIS.DELETE_FLAG = 0 
                    LEFT OUTER JOIN
                            M_COLORS MC
                    ON
                            TSSI.SHIPPER_ID = MC.SHIPPER_ID
                        AND TSSI.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                        AND MC.DELETE_FLAG = 0
                    LEFT OUTER JOIN
                            (SELECT
                                    SHIPPER_ID
                                ,   CATEGORY_ID1
                                ,   MAX(CATEGORY_NAME1) AS CATEGORY_NAME1
                            FROM
                                    M_ITEM_CATEGORIES4
                            WHERE
                                    SHIPPER_ID = :SHIPPER_ID
                                AND DELETE_FLAG = 0
                            GROUP BY
                                    SHIPPER_ID
                                ,   CATEGORY_ID1
                            ) MIC
                    ON
                            TSSI.SHIPPER_ID = MIC.SHIPPER_ID
                        AND TSSI.CATEGORY_ID1 = MIC.CATEGORY_ID1
                    WHERE
                            WSSI.SHIPPER_ID = :SHIPPER_ID
                        AND WSSI.SEQ = :SEQ
                        AND WSSI.IS_CHECK = 1
                        AND TSSI.SORT_CLASS = 1
                    GROUP BY
                            WSSI.SORT_INSTRUCT_NO
                        ,   TSSI.SORT_CLASS
                        ,   TSSI.TRANSFER_NO
                        ,   TSSI.JAN
                    ORDER BY
                            WSSI.SORT_INSTRUCT_NO
                        ,   TSSI.SORT_CLASS
                        ,   TSSI.TRANSFER_NO
                ");

            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":USER_ID", Profile.User.UserId);
            parameters.Add(":SEQ", pSeq);

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                    MvcDbContext.Current.SaveChanges();
                    trans.Commit();
                }
                catch (DbUpdateConcurrencyException)
                {
                    trans.Rollback();
                    return Wms.Resources.MessageResource.ERROR_END;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 在庫仕分カンバン(カテゴリー別)用データ
        /// </summary>
        /// <param name="search"></param>
        public string InsStockSortKanbanCategory(long pSeq)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                    INSERT INTO WW_SORT_STOCK_INSTRUCTS03
                    (
                            MAKE_DATE
                        ,   MAKE_USER_ID
                        ,   MAKE_PROGRAM_NAME
                        ,   UPDATE_DATE
                        ,   UPDATE_USER_ID
                        ,   UPDATE_PROGRAM_NAME
                        ,   SHIPPER_ID
                        ,   SEQ
                        ,   LINE_NO
                        ,   LINE_NO_SEQ
                        ,   CENTER_ID
                        ,   SORT_INSTRUCT_NO
                        ,   SORT_INSTRUCT_NAME
                        ,   SORT_CLASS
                        ,   SORT_CATEGORY_NAME 
                        ,   TRANSFER_NO
                        ,   NOTE
                        ,   STOCK_QTY
                    )
                    SELECT 
                            SYSTIMESTAMP
                        ,   :USER_ID
                        ,   'ImportInstruct'
                        ,   SYSTIMESTAMP
                        ,   :USER_ID
                        ,   'ImportInstruct'
                        ,   :SHIPPER_ID
                        ,   :SEQ
                        ,   MAX(WSSI.LINE_NO)
                        ,   ROW_NUMBER() OVER(ORDER BY WSSI.SORT_INSTRUCT_NO, TSSI.SORT_CLASS, TSSI.TRANSFER_NO)
                        ,   MAX(WSSI.CENTER_ID)
                        ,   WSSI.SORT_INSTRUCT_NO
                        ,   MAX(WSSI.SORT_INSTRUCT_NAME) AS SORT_INSTRUCT_NAME
                        ,   TSSI.SORT_CLASS
                        ,   MAX(TSSI.SORT_CATEGORY_NAME) AS SORT_CATEGORY_NM
                        ,   TSSI.TRANSFER_NO
                        ,   MAX(TSSI.NOTE) AS NOTE
                        ,   SUM(TSSI.STOCK_QTY) AS NUM_STOCK
                    FROM 
                            WW_SORT_STOCK_INSTRUCTS01 WSSI
                    INNER JOIN
                            T_SORT_STOCK_INSTRUCTS TSSI
                    ON
                            WSSI.SHIPPER_ID = TSSI.SHIPPER_ID
                        AND WSSI.CENTER_ID = TSSI.CENTER_ID
                        AND WSSI.SORT_INSTRUCT_NO = TSSI.SORT_INSTRUCT_NO
                    WHERE
                            WSSI.SHIPPER_ID = :SHIPPER_ID
                        AND WSSI.SEQ = :SEQ
                        AND WSSI.IS_CHECK = 1
                        AND TSSI.SORT_CLASS = 2
                    GROUP BY
                            WSSI.SORT_INSTRUCT_NO
                        ,   TSSI.SORT_CLASS
                        ,   TSSI.TRANSFER_NO
                    ORDER BY
                            WSSI.SORT_INSTRUCT_NO
                        ,   TSSI.SORT_CLASS
                        ,   TSSI.TRANSFER_NO
                ");

            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":USER_ID", Profile.User.UserId);
            parameters.Add(":SEQ", pSeq);

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                    MvcDbContext.Current.SaveChanges();
                    trans.Commit();
                }
                catch (DbUpdateConcurrencyException)
                {
                    trans.Rollback();
                    return Wms.Resources.MessageResource.ERROR_END;
                }
            }
            return string.Empty;
        }

        public IEnumerable<ImportInstructionReportRowForCsv> GetImportInstructionListingForCsv(ImportInstructionSearchConditions conditions)
        {
            var sql = $@"
                WITH
                    WORK_DATA01 AS (
                        SELECT
                                *
                        FROM
                                WW_SORT_STOCK_INSTRUCTS01
                        WHERE
                                SEQ = :SEQ
                            AND SHIPPER_ID = :SHIPPER_ID
                            AND IS_CHECK = 1
                    )
                    ,WORK_DATA03 AS (
                        SELECT
                                *
                        FROM
                                WW_SORT_STOCK_INSTRUCTS03
                        WHERE
                                SEQ = :SEQ
                            AND SHIPPER_ID = :SHIPPER_ID
                    )
                --鑑
                SELECT
                        1 AS REPORT_TYPE
                    ,   WK.CENTER_ID
                    ,   WK.SORT_INSTRUCT_NO
                    ,   WK.SORT_INSTRUCT_NAME
                    ,   NULL AS TRANSFER_NO
                    ,   NULL AS SORT_CATEGORY_NAME
                    ,   NULL AS CATEGORY_NAME1
                    ,   NULL AS ITEM_ID
                    ,   NULL AS ITEM_COLOR
                    ,   NULL AS ITEM_SIZE
                    ,   NULL AS JAN
                    ,   NULL AS STOCK_QTY
                    ,   NULL AS NOTE
                FROM
                        WORK_DATA01 WK

                --明細
                UNION ALL

                SELECT
                        CASE WHEN WK.SORT_CLASS = 1 THEN 3 ELSE 2 END AS REPORT_TYPE
                    ,   WK.CENTER_ID
                    ,   WK.SORT_INSTRUCT_NO
                    ,   WK.SORT_INSTRUCT_NAME
                    ,   WK.TRANSFER_NO
                    ,   WK.SORT_CATEGORY_NAME
                    ,   WK.CATEGORY_NAME1
                    ,   WK.ITEM_ID
                    ,   WK.ITEM_COLOR
                    ,   WK.ITEM_SIZE
                    ,   WK.JAN_1 || WK.JAN_2 AS JAN
                    ,   WK.STOCK_QTY
                    ,   WK.NOTE
                FROM
                        WORK_DATA03 WK

                ORDER BY
                        SORT_INSTRUCT_NO
                    ,   REPORT_TYPE
                    ,   TRANSFER_NO
                    ,   JAN
            
            ";

            return MvcDbContext.Current.Database.Connection.Query<ImportInstructionReportRowForCsv>(sql, 
                new {
                    SEQ = conditions.InstructionViewModels.First().Seq,
                    SHIPPER_ID = Profile.User.ShipperId
                });
        }
    }
}