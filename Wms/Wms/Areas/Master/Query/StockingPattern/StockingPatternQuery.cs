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
    using Mvc.Common;
    using Oracle.ManagedDataAccess.Types;
    using PagedList;
    using Share.Extensions.Classes;
    using Wms.Areas.Master.ViewModels.StockingPattern;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.StockingPattern.StockingPatternSearchCondition;

    public partial class StockingPattern
    {
        /// <summary>
        /// Get StockingPattern List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<Detail> GetData(StockingPatternSearchCondition condition)
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
            var StockingPattern = MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters);
            
            // Excute paging
            return new StaticPagedList<Detail>(StockingPattern, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// 一覧表示のSQLを取得
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="query">SQL</param>
        /// <param name="parameters"></param>
        public static void GetQuery(StockingPatternSearchCondition condition, ref StringBuilder query, ref DynamicParameters parameters)
        {
            query.Append(@"
                SELECT
                         MSP.PATTERN_ID
                        ,MAX(MSP.PATTERN_NAME) AS PATTERN_NAME
                        ,MAX(MSP.UPDATE_COUNT) AS UPDATE_COUNT
                        
                FROM
                        M_STOCKING_PATTERN MSP
                WHERE
                        MSP.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Add search condition
            if (!string.IsNullOrEmpty(condition.PatternId))
            {
                query.Append(" AND MSP.PATTERN_ID = :PATTERN_ID ");
                parameters.Add(":PATTERN_ID", condition.PatternId);
            }

            if (!string.IsNullOrEmpty(condition.PatternName))
            {
                query.Append(" AND MSP.PATTERN_NAME LIKE :PATTERN_NAME ");
                parameters.Add(":PATTERN_NAME", "%" +condition.PatternName + "%");
            }

            //集約条件追加
            query.Append("GROUP BY MSP.PATTERN_ID ");

            // Sort function
            switch (condition.SortKey)
            {
                case StokingPatternSortKey.PatternName:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MAX(MSP.PATTERN_NAME) DESC,MSP.PATTERN_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MAX(MSP.PATTERN_NAME) ASC,MSP.PATTERN_ID ASC");
                            break;

                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MSP.PATTERN_ID DESC,MAX(MSP.PATTERN_NAME) DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MSP.PATTERN_ID ASC,MAX(MSP.PATTERN_NAME) ASC");
                            break;
                    }

                    break;
            }
        }

        /// <summary>
        /// Get ROWID
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public List<Detail> GetRowId(StockingPatternSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();

            //一覧表示のSQLを取得
            GetQuery(condition, ref query, ref parameters);

            var patternId = MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters).ToList();
            return patternId;
        }

        /// <summary>
        /// Get Delete By Id
        /// </summary>
        /// <param name = "stockingPattern, categoryTableItem" ></ param >
        /// < returns > stockingPattern </ returns >
        public Detail GetDeleteTargetById(Detail stockingPattern ,List <CategoryTableItem> categoryTableItem)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            List<string> categories = categoryTableItem.Select(m => m.CategoryId1).ToList();
            query.Append(@"
                SELECT
                         MSP.PATTERN_ID
                        ,MSP.CATEGORY_ID1
                FROM
                        M_STOCKING_PATTERN MSP
                LEFT JOIN
                        M_ITEM_CATEGORIES4 MIC4
                     ON MSP.SHIPPER_ID = :SHIPPER_ID
                    AND MSP.CATEGORY_ID1 = MIC4.CATEGORY_ID1
                WHERE
                        MSP.SHIPPER_ID = :SHIPPER_ID
                    AND MSP.PATTERN_ID = :PATTERN_ID
                    AND MSP.CATEGORY_ID1 IN :CATEGORY_ID1
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":PATTERN_ID", stockingPattern.PatternId);
            parameters.Add(":CATEGORY_ID1", categories);
            return MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters).FirstOrDefault();
        }
        /// <summary>
        /// Get Edit By Id
        /// </summary>
        /// <param name="patternId">patternId</param>
        /// <returns></returns>
        public Detail GetEditTargetById(string patternId) 
        {
            var parameters = new DynamicParameters();
            var query = new StringBuilder();

            query.Append(@"
                SELECT DISTINCT
                         MSP.PATTERN_ID
                        ,MSP.PATTERN_NAME
                        ,MSP.CATEGORY_ID1
                        ,MIC4.CATEGORY_NAME1
                        ,MSP.STOCKING_CLASS
                        ,MSP.SHIPPER_ID
                FROM
                        M_STOCKING_PATTERN MSP
                LEFT JOIN
                        M_ITEM_CATEGORIES4 MIC4
                     ON MIC4.SHIPPER_ID = MSP.SHIPPER_ID
                    AND MSP.CATEGORY_ID1 = MIC4.CATEGORY_ID1
                WHERE
                        MSP.PATTERN_ID = :PATTERN_ID
                    AND MSP.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":PATTERN_ID", patternId);
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

            return MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters).FirstOrDefault();
        }

        /// <summary>
        /// Delete StockingPatterns
        /// </summary>
        /// <param name="stockingPatternId">List record is deleted</param>
        /// <returns>List of rows error </returns>
        public bool Delete(List<string> stockingPatternId)
        {
            var parameters = new DynamicParameters();
            var query = new StringBuilder();

            query.Append(@" DELETE FROM M_STOCKING_PATTERN T  WHERE T.PATTERN_ID = :PATTERN_ID AND T.SHIPPER_ID = :SHIPPER_ID");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                
                foreach (var patternId in stockingPatternId)
                {
                    parameters.Add(":PATTERN_ID", patternId);

                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }

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

            return true;
        }

        /// <summary>
        /// Create new StockingPattern
        /// </summary>
        /// <param name="categoryTable"></param>
        /// <returns></returns>
        public bool Create(Detail resultRow , List<CategoryTableItem> categoryTable)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                INSERT INTO M_STOCKING_PATTERN (
                     MAKE_DATE          
                    ,MAKE_USER_ID       
                    ,MAKE_PROGRAM_NAME  
                    ,UPDATE_DATE        
                    ,UPDATE_USER_ID     
                    ,UPDATE_PROGRAM_NAME
                    ,UPDATE_COUNT       
                    ,SHIPPER_ID         
                    ,PATTERN_ID         
                    ,PATTERN_NAME       
                    ,CATEGORY_ID1       
                    ,STOCKING_CLASS     
                ) VALUES (
                     SYSDATE         
                    ,:USER_ID       
                    ,:PROGRAM_ID  
                    ,SYSDATE      
                    ,:USER_ID   
                    ,:PROGRAM_ID
                    ,0          
                    ,:SHIPPER_ID         
                    ,:PATTERN_ID         
                    ,:PATTERN_NAME       
                    ,:CATEGORY_ID1       
                    ,:STOCKING_CLASS     
                ) 
            ");
            parameters.Add(":USER_ID", Common.Profile.User.UserId);
            parameters.Add(":PROGRAM_ID", "InsertStockingPattern");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":PATTERN_ID", resultRow.PatternId);
            parameters.Add(":PATTERN_NAME", resultRow.PatternName);

            var dbContext = MvcDbContext.Current;
            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    for (int i = 0;i < categoryTable.Count; i++)
                    {
                        parameters.Add(":CATEGORY_ID1", categoryTable[i].CategoryId1);
                        parameters.Add(":STOCKING_CLASS", categoryTable[i].StockingClass);
                        MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                    }
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
        /// Update stockingPattern
        /// </summary>
        /// <param name="stockingPattern"></param>
        /// <returns>Update status</returns>
        public bool UpdateStockingPattern(Detail stockingPattern ,List< CategoryTableItem> categoryTable)
        {
            var dbContext = MvcDbContext.Current;


            foreach (var updateStockingClass in categoryTable)
            {

                var updatedStockingPattern =
                      MvcDbContext.Current.StockingPatterns
                      .Where(m => m.ShipperId == stockingPattern.ShipperId && m.PatternId == stockingPattern.PatternId && m.CategoryId1 == updateStockingClass.CategoryId1 && m.UpdateCount == updateStockingClass.UpdateCount)
                      .SingleOrDefault();

                // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合） 
                if (updatedStockingPattern == null)
                {
                    return false;
                }
                
                updatedStockingPattern.SetBaseInfoUpdate();
                updatedStockingPattern.PatternName = stockingPattern.PatternName;
                updatedStockingPattern.StockingClass = updateStockingClass.StockingClass;
            }

            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.SaveChanges();
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
        /// 分類1データ取得
        /// </summary>
        public List<CategoryTableItem> GetListCategory1(bool InsertFlag, string pattern)
        {

            var parameters = new DynamicParameters();
            var query = new StringBuilder();
            if (InsertFlag)
            {
                query.Append(@"
                    SELECT
                             MIC4.CATEGORY_ID1
                            ,MAX(MIC4.CATEGORY_NAME1) AS CATEGORY_NAME1
                    FROM
                            M_ITEM_CATEGORIES4 MIC4
                    WHERE
                            MIC4.SHIPPER_ID = :SHIPPER_ID
                    GROUP BY MIC4.CATEGORY_ID1
                    ORDER BY MIC4.CATEGORY_ID1
                ");
            }
            else
            {
                query.Append(@"
                    SELECT DISTINCT
                             MSP.CATEGORY_ID1
                            ,MIC4.CATEGORY_NAME1
                            ,MSP.STOCKING_CLASS
                            ,MSP.UPDATE_COUNT
                    FROM
                            M_STOCKING_PATTERN MSP
                    LEFT JOIN
                            M_ITEM_CATEGORIES4 MIC4
                         ON MIC4.SHIPPER_ID = MSP.SHIPPER_ID
                        AND MSP.CATEGORY_ID1 = MIC4.CATEGORY_ID1
                    WHERE
                            MSP.PATTERN_ID = :PATTERN_ID
                        AND MSP.SHIPPER_ID = :SHIPPER_ID
                    ORDER BY MSP.CATEGORY_ID1 ASC
                    ");
            parameters.Add(":PATTERN_ID", pattern);
            }
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);



            var categories = MvcDbContext.Current.Database.Connection.Query<CategoryTableItem>(query.ToString(),parameters);
            return categories.ToList();
        }

        /// <summary>
        /// 仕分方法のラジオボタンデータ取得
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetRadioButtonListStockingClass()
        {
            return MvcDbContext.Current.Generals
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.CenterId == "@@@" && m.RegisterDiviCd == "1" && m.GenDivCd == "STOCKING_PATTERN")
                .Select(m => new SelectListItem
                {
                    Value = m.GenCd,
                    Text = m.GenName
                })
                .Distinct().OrderBy(m => m.Value);
        }
    }
}