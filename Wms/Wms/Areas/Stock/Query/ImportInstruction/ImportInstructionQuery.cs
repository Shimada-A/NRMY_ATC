namespace Wms.Areas.Stock.Query.ImportInstruction
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using Wms.Areas.Stock.Models;
    using PagedList;
    using Share.Common;
    using Share.Extensions.Classes;
    using Share.Helpers;
    using Wms.Areas.Stock.Resources;
    using Wms.Areas.Stock.ViewModels.ImportInstruction;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Stock.ViewModels.ImportInstruction.ImportInstructionSearchConditions;
    using Wms.Areas.Master.Models;
    using System;

    public class ImportInstructionQuery : BaseQuery
    {
        /// <summary>
        /// ワークテーブル作成
        /// </summary>
        public bool InsertSortStockWk(ImportInstructionSearchConditions condition)
        {
            long workId = GetWorkId();
            condition.Seq = workId;

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                        INSERT INTO WW_SORT_STOCK_INSTRUCTS01 (
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
                            ,   SORT_INSTRUCT_NO
                            ,   SORT_INSTRUCT_NAME
                            ,   SORT_IMPORT_DATE
                            ,   SKU_SORT_QTY
                            ,   CATEGORY_SORT_QTY
                        )
                        SELECT
                                SYSTIMESTAMP
                            ,   :USER_ID
                            ,   :PROGRAM_NAME
                            ,   SYSTIMESTAMP
                            ,   :USER_ID
                            ,   :PROGRAM_NAME
                            ,   0
                            ,   TSSI.SHIPPER_ID
                            ,   :SEQ
                            ,   ROW_NUMBER() OVER(ORDER BY TSSI.SHIPPER_ID , TSSI.CENTER_ID , TSSI.SORT_INSTRUCT_NO)
                            ,   0
                            ,   TSSI.CENTER_ID
                            ,   TSSI.SORT_INSTRUCT_NO
                            ,   TSSI.SORT_INSTRUCT_NAME
                            ,   TSSI.SORT_IMPORT_DATE
                            ,   COUNT(DISTINCT CASE WHEN TSSI.SORT_CLASS = 1 THEN TSSI.TRANSFER_NO ELSE NULL END) AS SKU_SORT_QTY
                            ,   COUNT(DISTINCT CASE WHEN TSSI.SORT_CLASS = 2 THEN TSSI.TRANSFER_NO ELSE NULL END) AS CATEGORY_SORT_QTY
                        FROM
                                T_SORT_STOCK_INSTRUCTS TSSI
                        LEFT OUTER JOIN
                                (SELECT
                                        SHIPPER_ID
                                    ,   CENTER_ID
                                    ,   SORT_INSTRUCT_NO
                                FROM
                                        T_SORT_STOCK_RESULTS
                                WHERE
                                        SHIPPER_ID = :SHIPPER_ID
                                    AND CENTER_ID = :CENTER_ID
                                GROUP BY
                                        SHIPPER_ID
                                    ,   CENTER_ID
                                    ,   SORT_INSTRUCT_NO
                                )TSSR
                        ON
                                TSSI.SHIPPER_ID = TSSR.SHIPPER_ID
                            AND TSSI.CENTER_ID = TSSR.CENTER_ID
                            AND TSSI.SORT_INSTRUCT_NO = TSSR.SORT_INSTRUCT_NO
                        WHERE
                                TSSI.SHIPPER_ID = :SHIPPER_ID
                            AND TSSI.CENTER_ID = :CENTER_ID
                            AND TSSI.SORT_IMPORT_DATE >= ADD_MONTHS(SYSDATE,-12)
                        GROUP BY
                                TSSI.SHIPPER_ID
                            ,   TSSI.CENTER_ID
                            ,   TSSI.SORT_INSTRUCT_NO
                            ,   TSSI.SORT_INSTRUCT_NAME
                            ,   TSSI.SORT_IMPORT_DATE
                        ORDER BY
                                TSSI.SHIPPER_ID
                            ,   TSSI.CENTER_ID
                            ,   TSSI.SORT_INSTRUCT_NO
                    ");
                    parameters.Add(":USER_ID", Common.Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "InsertSortStockWk");
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                    parameters.Add(":CENTER_ID", condition.CenterId);

                    //ワークテーブルから削除する。
                    var sortStockInstructs01s = MvcDbContext.Current.SortStockInstructs01s.Where(x => x.ShipperId == Profile.User.ShipperId && x.CenterId == condition.CenterId);
                    if (sortStockInstructs01s.Any())
                    {
                        MvcDbContext.Current.SortStockInstructs01s.RemoveRange(sortStockInstructs01s);
                        MvcDbContext.Current.SaveChanges();
                    }

                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (Exception ex) //デバッグ用
                {
                    Mvc.Common.AppError.PutLogREF(ex, "InsertSortStockWk");
                    trans.Rollback();
                    return false;
                }
                trans.Commit();
            }
            return true;
        }

        /// <summary>
        /// ワークテーブルデータ取得
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<ImportInstructionResultRow> GetSortStockWkData(ImportInstructionSearchConditions condition)
        {
            string order;
            // Sort function
            switch (condition.Key)
            {
                case SortKey.SortInstructNameNo:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            order = "ORDER BY WSSI.SORT_INSTRUCT_NAME DESC,WSSI.SORT_INSTRUCT_NO DESC";
                            break;

                        default:
                            order = "ORDER BY WSSI.SORT_INSTRUCT_NAME ASC,WSSI.SORT_INSTRUCT_NO ASC";
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            order = "ORDER BY WSSI.SORT_INSTRUCT_NO DESC";
                            break;

                        default:
                            order = "ORDER BY WSSI.SORT_INSTRUCT_NO ASC";
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
                        ) AS NO
                    ,   WSSI.SORT_INSTRUCT_NO
                    ,   WSSI.SORT_INSTRUCT_NAME
                    ,   WSSI.SORT_IMPORT_DATE
                    ,   WSSI.CATEGORY_SORT_QTY
                    ,   WSSI.SKU_SORT_QTY
                    ,   WSSI.UPDATE_COUNT
                    ,   WSSI.IS_CHECK
                    ,   WSSI.SEQ
                    ,   CASE WHEN TSSR.SORT_INSTRUCT_NO IS NULL THEN 0 ELSE 1 END RESULT_FLG
                FROM
                        WW_SORT_STOCK_INSTRUCTS01 WSSI
                LEFT OUTER JOIN
                        (SELECT
                                SHIPPER_ID
                            ,   CENTER_ID
                            ,   SORT_INSTRUCT_NO
                        FROM
                                T_SORT_STOCK_RESULTS
                        WHERE
                                SHIPPER_ID = :SHIPPER_ID
                            AND CENTER_ID = :CENTER_ID
                        GROUP BY
                                SHIPPER_ID
                            ,   CENTER_ID
                            ,   SORT_INSTRUCT_NO
                        )TSSR
                ON
                        WSSI.SHIPPER_ID = TSSR.SHIPPER_ID
                    AND WSSI.CENTER_ID = TSSR.CENTER_ID
                    AND WSSI.SORT_INSTRUCT_NO = TSSR.SORT_INSTRUCT_NO
                WHERE
                        WSSI.SHIPPER_ID = :SHIPPER_ID
                    AND WSSI.CENTER_ID  = :CENTER_ID
                    AND WSSI.SEQ = :SEQ
            ");

            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SEQ", condition.Seq);


            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<ImportInstructionResultRow>(query.ToString(), parameters).Count();

            query.AppendLine(order);

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var ImportInstructions = MvcDbContext.Current.Database.Connection.Query<ImportInstructionResultRow>(query.ToString(), parameters);

            condition.SelectedCnt = MvcDbContext.Current.SortStockInstructs01s
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.CenterId == condition.CenterId && m.IsCheck && m.Seq == condition.Seq).Count();
            //選択中実績済数取得
            query = new StringBuilder(@"
                        SELECT
                                COUNT(1)
                        FROM
                                WW_SORT_STOCK_INSTRUCTS01 WW
                        INNER JOIN
                                (SELECT
                                        SHIPPER_ID
                                    ,   CENTER_ID
                                    ,   SORT_INSTRUCT_NO
                                FROM
                                        T_SORT_STOCK_RESULTS
                                WHERE
                                        SHIPPER_ID = :SHIPPER_ID
                                    AND CENTER_ID = :CENTER_ID
                                GROUP BY
                                        SHIPPER_ID
                                    ,   CENTER_ID
                                    ,   SORT_INSTRUCT_NO
                                ) TS
                        ON
                                WW.SHIPPER_ID = TS.SHIPPER_ID
                            AND WW.CENTER_ID = TS.CENTER_ID
                            AND WW.SORT_INSTRUCT_NO  = TS.SORT_INSTRUCT_NO
                        WHERE
                                WW.SHIPPER_ID = :SHIPPER_ID
                            AND WW.CENTER_ID = :CENTER_ID
                            AND WW.SEQ = :SEQ
                            AND WW.IS_CHECK = 1
            ");
            parameters = new DynamicParameters();
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SEQ", condition.Seq);
            int resultCnt = MvcDbContext.Current.Database.Connection.Query<int>(query.ToString(), parameters).FirstOrDefault();
            condition.ResultCnt = resultCnt;

            // Excute paging
            return new StaticPagedList<ImportInstructionResultRow>(ImportInstructions, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// //ワークテーブル選択行更新
        /// </summary>
        public bool UpdateWkData(IList<SelectedImportInstructionViewModel> InstructionViewModel, string pCenterId)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in InstructionViewModel)
                {
                    // 在庫明細
                    var importInstructionChngs = MvcDbContext.Current.SortStockInstructs01s
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.CenterId == pCenterId && m.Seq == u.Seq && m.SortInstructNo == u.SortInstructNo)
                                  .SingleOrDefault();

                    if (importInstructionChngs == null)
                    {
                        continue;
                    }

                    importInstructionChngs.SetBaseInfoUpdate();
                    importInstructionChngs.IsCheck = u.IsCheck;
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
        /// //ワークテーブル全行更新
        /// </summary>
        public bool WkDataAllChange(ImportInstructionSearchConditions conditions, bool check)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in MvcDbContext.Current.SortStockInstructs01s
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.CenterId == conditions.CenterId && m.Seq == conditions.Seq))
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
            conditions.SelectedCnt = MvcDbContext.Current.SortStockInstructs01s
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.CenterId == conditions.CenterId && m.IsCheck && m.Seq == conditions.Seq).Count();
            return true;
        }

        /// <summary>
        //指示テーブル削除
        /// </summary>
        public string DeleteWkData(IList<SelectedImportInstructionViewModel> InstructionViewModel, string pCenterId)
        {
            var parameters = new DynamicParameters();
            var deleteSortStockInstructs01 = new StringBuilder();
            var retMsg = string.Empty;

            deleteSortStockInstructs01.Append(@"
                DELETE FROM
                        T_SORT_STOCK_INSTRUCTS
                 WHERE
                        SHIPPER_ID = :SHIPPER_ID
                    AND CENTER_ID = :CENTER_ID
                    AND SORT_INSTRUCT_NO = :SORT_INSTRUCT_NO
            ");

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                // KEY
                foreach (var u in InstructionViewModel)
                {
                    if (u.IsCheck)
                    {
                        //在庫仕分実績検索
                        var cntResult = MvcDbContext.Current.SortStockResults.Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.CenterId == pCenterId && m.SortInstructNo == u.SortInstructNo).Count();
                        //実績に存在した場合エラー
                        if (cntResult > 0)
                        {
                            retMsg = retMsg + u.SortInstructNo + ",";

                        }
                        else
                        {
                            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                            parameters.Add(":CENTER_ID", pCenterId);
                            parameters.Add(":SORT_INSTRUCT_NO", u.SortInstructNo);
                            MvcDbContext.Current.Database.Connection.Execute(deleteSortStockInstructs01.ToString(), parameters);

                            try
                            {
                                MvcDbContext.Current.SaveChanges();
                            }
                            catch (DbUpdateConcurrencyException)
                            {
                                return Wms.Resources.MessageResource.ERROR_END;
                            }
                        }
                    }
                }
                if (retMsg == string.Empty)
                {
                    trans.Commit();
                    retMsg = "";
                }
                else
                {
                    retMsg = ImportInstructionResource.ERR_NOT_DELETE + "(" + retMsg.TrimEnd(',') + ")";
                    trans.Rollback();
                }
            }

            return retMsg;
        }

        /// <summary>
        /// 実績更新
        /// </summary>
        public void InsertSortStockIns(ImportInstructionSearchConditions searchConditions, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", searchConditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_WORK_ID", searchConditions.Seq, DbType.Int32, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
            "SP_W_SORT_STOCK_INSTRUCTS",
            param,
            commandType: CommandType.StoredProcedure);
            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }
    }
}