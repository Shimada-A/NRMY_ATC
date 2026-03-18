namespace Wms.Areas.Master.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.UI.WebControls;
    using Dapper;
    using Glimpse.Core.Extensions;
    using Microsoft.Ajax.Utilities;
    using Mvc.Common;
    using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
    using Oracle.ManagedDataAccess.Types;
    using PagedList;
    using Share.Extensions.Classes;
    using StackExchange.Redis;
    using Wms.Areas.Master.ViewModels.Operation;
    using Wms.Areas.Ship.ViewModels.PrintEcInvoice;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.Operation.OperationSearchCondition;

    public partial class Operation
    {
        /// <summary>
        /// Get Operation List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<Detail> GetData(OperationSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();

            //一覧表示のSQLを取得
            GetQuery(condition, ref query, ref parameters);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var Operation = MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<Detail>(Operation, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// 一覧表示のSQLを取得
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="query">SQL</param>
        /// <param name="parameters"></param>
        public static void GetQuery(OperationSearchCondition condition, ref StringBuilder query, ref DynamicParameters parameters)
        {
            query.Append(@"
                SELECT
                        MO.OPERATION_ID
                    ,   MO.OPERATION_NAME
                    ,   MO.CATEGORY_NAME
                    ,   MO.UPDATE_COUNT
                FROM
                        M_OPERATIONS MO
                WHERE
                        MO.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Add search condition
            if (!string.IsNullOrEmpty(condition.OperationId))
            {
                query.Append(" AND MO.OPERATION_ID = :OPERATION_ID ");
                parameters.Add(":OPERATION_ID", condition.OperationId);
            }

            if (!string.IsNullOrEmpty(condition.OperationName))
            {
                query.Append(" AND MO.OPERATION_NAME LIKE :OPERATION_NAME ");
                parameters.Add(":OPERATION_NAME", "%" + condition.OperationName + "%");
            }

            if (!string.IsNullOrEmpty(condition.CategoryName))
            {
                query.Append(" AND MO.CATEGORY_NAME = :CATEGORY_NAME ");
                parameters.Add(":CATEGORY_NAME", condition.CategoryName);
            }

            query.Append("AND WMS_OPERATION_FLAG = '0'");


            // Sort function
            switch (condition.SortKey)
            {
                case OperationSortKey.OperationId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MO.OPERATION_ID DESC,MO.CATEGORY_NAME DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MO.OPERATION_ID ASC,MO.CATEGORY_NAME ASC");
                            break;

                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MO.CATEGORY_NAME DESC,MO.OPERATION_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MO.CATEGORY_NAME ASC,MO.OPERATION_ID ASC");
                            break;
                    }

                    break;
            }
        }

        /// <summary>
        /// Get OperationID
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public List<Detail> GetOperationId(OperationSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();

            //一覧表示のSQLを取得
            GetQuery(condition, ref query, ref parameters);

            var operationId = MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters).ToList();
            return operationId;
        }

        /// <summary>
        /// Get Delete By Id(新規作成時の重複チェック用)
        /// </summary>
        /// <param name="operations" ></ param >
        /// < returns > operation </ returns >
        public Detail GetTargetById(Detail operations)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                        MO.OPERATION_ID
                FROM
                        M_OPERATIONS MO
                WHERE
                        MO.SHIPPER_ID = :SHIPPER_ID
                    AND MO.OPERATION_ID = :OPERATION_ID
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":OPERATION_ID", operations.OperationId);
            return MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters).FirstOrDefault();
        }

        /// <summary>
        /// Get Edit By Id
        /// </summary>
        /// <param name="operationId">patternId</param>
        /// <returns></returns>
        public Detail GetEditTargetById(string operationId)
        {
            var parameters = new DynamicParameters();
            var query = new StringBuilder();

            query.Append(@"
                SELECT DISTINCT
                        MO.OPERATION_ID
                    ,   MO.OPERATION_NAME
                    ,   MO.QTY_ZERO_FLAG
                    ,   MON.OPERATION_NOTE
                    ,   MON.SEQ
                    ,   MO.SHIPPER_ID
                    ,   MO.UPDATE_COUNT
                    ,   MO.CATEGORY_NAME
                FROM
                        M_OPERATIONS MO
                LEFT JOIN
                        M_OPERATION_NOTES MON
                    ON 
                        MON.SHIPPER_ID = MO.SHIPPER_ID
                    AND MON.OPERATION_ID = MO.OPERATION_ID
                WHERE
                        MO.OPERATION_ID = :OPERATION_ID
                    AND MO.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":OPERATION_ID", operationId);
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

            return MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters).FirstOrDefault();
        }

        /// <summary>
        /// Delete Operations
        /// </summary>
        /// <param name="operationIds">List record is deleted</param>
        /// <returns>List of operations error </returns>
        public bool Delete(List<string> operationIds)
        {
            var parameters = new DynamicParameters();
            var query = new StringBuilder();
            var queryNote = new StringBuilder();

            query.Append(@" DELETE FROM M_OPERATIONS MO  WHERE MO.OPERATION_ID = :OPERATION_ID AND MO.SHIPPER_ID = :SHIPPER_ID");
            DeleteNotes(ref queryNote);
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    foreach (var operationId in operationIds)
                    {
                        parameters.Add(":OPERATION_ID", operationId);
                        MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                        MvcDbContext.Current.Database.Connection.Execute(queryNote.ToString(), parameters);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return false;
                }

                trans.Commit();
            }

            return true;
        }


        /// <summary>
        ///　作業備考削除のSQL(更新時は一時削除)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public void DeleteNotes(ref StringBuilder query)
        {

            query.Append(@" DELETE FROM M_OPERATION_NOTES MON WHERE MON.OPERATION_ID = :OPERATION_ID AND MON.SHIPPER_ID = :SHIPPER_ID");

        }
        /// <summary>
        /// 詳細画面の更新/新規登録押下時備考登録処理のSQL
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public void CreateNotes(ref StringBuilder query)
        {
            query.Append(@"
                INSERT INTO M_OPERATION_NOTES (
                        MAKE_DATE
                    ,   MAKE_USER_ID
                    ,   MAKE_PROGRAM_NAME
                    ,   UPDATE_DATE
                    ,   UPDATE_USER_ID
                    ,   UPDATE_PROGRAM_NAME
                    ,   UPDATE_COUNT
                    ,   SHIPPER_ID
                    ,   OPERATION_ID
                    ,   SEQ
                    ,   OPERATION_NOTE
                    ) 
                VALUES (
                        SYSDATE
                    ,   :USER_ID
                    ,   :PROGRAM_ID
                    ,   SYSDATE
                    ,   :USER_ID
                    ,   :PROGRAM_ID
                    ,   0          
                    ,   :SHIPPER_ID
                    ,   :OPERATION_ID
                    ,   :SEQ
                    ,   :OPERATION_NOTE
                )"
            );
        }


        /// <summary>
        /// Create new Operation and OperationNotes
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="operationNote"></param>
        /// <returns></returns>
        public bool Create(Detail operation, List<string> operationNote)
        {
            var dbContext = MvcDbContext.Current;
            var newOperation = new Operations();
            newOperation.SetBaseInfoInsert();
            newOperation.OperationId = operation.OperationId;
            newOperation.OperationName = operation.OperationName;
            newOperation.CategoryName = operation.CategoryName;
            newOperation.QtyZeroFlag = operation.QtyZeroFlag;
            dbContext.Operations.Add(newOperation);

            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (operationNote != null)
                    {
                        for (int i = 0; i < operationNote.Count; i++)
                        {
                            DynamicParameters parameters = new DynamicParameters();
                            StringBuilder query = new StringBuilder();
                            //備考欄登録のSQLを取得
                            CreateNotes(ref query);

                            parameters.Add(":OPERATION_NOTE", operationNote[i]);
                            parameters.Add(":SEQ", i + 1);
                            parameters.Add(":USER_ID", Common.Profile.User.UserId);
                            parameters.Add(":PROGRAM_ID", "InsertOperationNotes");
                            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                            parameters.Add(":OPERATION_ID", operation.OperationId);

                            MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                        }
                    }

                    dbContext.SaveChanges();
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    AppError.PutLog(ex);
                    return false;
                }
                catch (Exception ex)
                {
                    AppError.PutLog(ex);
                    return false;
                }

                trans.Commit();
            }

            return true;
        }

        /// <summary>
        /// Update Operation and OperationNotes
        /// </summary>
        /// <param name="operation"></param>
        /// <returns>Update status</returns>
        public bool UpdateOperation(Detail operation, List<string> operationNote)
        {
            var dbContext = MvcDbContext.Current;

            var updatedOperation =
                  MvcDbContext.Current.Operations
                  .Where(m => m.ShipperId == operation.ShipperId && m.OperationId == operation.OperationId && m.UpdateCount == operation.UpdateCount)
                  .SingleOrDefault();

            // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合） 
            if (updatedOperation == null)
            {
                return false;
            }

            updatedOperation.SetBaseInfoUpdate();
            updatedOperation.OperationName = operation.OperationName;
            updatedOperation.QtyZeroFlag = operation.QtyZeroFlag;
            updatedOperation.CategoryName = operation.CategoryName;

            DynamicParameters parametersDeleteNotes = new DynamicParameters();
            StringBuilder queryDeleteNotes = new StringBuilder();

            //備考マスタ削除のSQLを取得
            DeleteNotes(ref queryDeleteNotes);
            parametersDeleteNotes.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parametersDeleteNotes.Add(":OPERATION_ID", operation.OperationId);


            // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合） 
            if (updatedOperation == null)
            {
                return false;
            }

            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    MvcDbContext.Current.Database.Connection.Execute(queryDeleteNotes.ToString(), parametersDeleteNotes);
                    dbContext.SaveChanges();

                    if (operationNote != null)
                    {
                        for (int i = 0; i < operationNote.Count; i++)
                        {
                            StringBuilder queryCreateNotes = new StringBuilder();
                            DynamicParameters parametersCreateNotes = new DynamicParameters();

                            //備考欄登録のSQLを取得
                            CreateNotes(ref queryCreateNotes);

                            parametersCreateNotes.Add(":OPERATION_NOTE", operationNote[i]);
                            parametersCreateNotes.Add(":SEQ", i + 1);
                            parametersCreateNotes.Add(":USER_ID", Common.Profile.User.UserId);
                            parametersCreateNotes.Add(":PROGRAM_ID", "UpdateOperationNotes");
                            parametersCreateNotes.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                            parametersCreateNotes.Add(":OPERATION_ID", operation.OperationId);

                            MvcDbContext.Current.Database.Connection.Execute(queryCreateNotes.ToString(), parametersCreateNotes);

                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return false;
                }
                trans.Commit();
            }

            return true;
        }

        /// <summary>
        /// カテゴリ名データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategoryName()
        {
            var parameters = new DynamicParameters();
            var query = new StringBuilder();

            query.Append(@"
                SELECT 
                        MO.CATEGORY_NAME AS VALUE
                    ,   MO.CATEGORY_NAME AS TEXT
                FROM 
                        M_OPERATIONS MO
                WHERE 
                        MO.SHIPPER_ID = :SHIPPER_ID
                GROUP BY
                        MO.CATEGORY_NAME
                ORDER BY 
                        MIN(OPERATION_ID)
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);


            var categoryNames = MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters);

            return categoryNames.ToList();
        }

        /// <summary>
        /// 作業備考マスタから備考取得
        /// </summary>
        /// <param name="operations"></param>
        /// <returns>OperationNote</returns>
        public List<OperationNoteItem> OperationNotes(string operations)
        {
            return MvcDbContext.Current.OperationNotes
                .Where(m => m.ShipperId == Profile.User.ShipperId && m.OperationId == operations)
                .Select(m => new OperationNoteItem
                {
                    Seq = m.Seq,
                    OperationNote = m.OperationNote.ToString(),
                })
                .Distinct().OrderBy(m => m.Seq).ToList();
        }
    }
}