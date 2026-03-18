namespace Wms.Areas.Stock.Query.LocMove
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
    using Wms.Areas.Stock.ViewModels.LocMove;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Stock.ViewModels.LocMove.LocMoveSearchConditions;

    public class LocMoveQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertStkLocMove(LocMoveSearchConditions condition)
        {
            condition.Seq = new BaseQuery().GetWorkId();
            condition.Page = 1;

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();

                    var whereQuery = new StringBuilder();

                    // Add search condition
                    // センター
                    if (!string.IsNullOrEmpty(condition.CenterId))
                    {
                        whereQuery.AppendLine(" AND TS.CENTER_ID = :CENTER_ID ");
                        parameters.Add(":CENTER_ID", condition.CenterId);
                    }

                    // ロケーション区分
                    if (!string.IsNullOrEmpty(condition.LocationClass))
                    {
                        whereQuery.AppendLine(" AND ML.LOCATION_CLASS = :LOCATION_CLASS ");
                        parameters.Add(":LOCATION_CLASS", condition.LocationClass);
                    }

                    // エリア
                    if (!string.IsNullOrEmpty(condition.Locsec1))
                    {
                        whereQuery.AppendLine(" AND ML.LOCSEC_1 LIKE :AREA_CD ");
                        parameters.Add(":AREA_CD", condition.Locsec1 + "%");
                    }

                    // ロケーションFrom
                    if (!string.IsNullOrEmpty(condition.LocationCdFrom))
                    {
                        whereQuery.AppendLine(" AND TS.LOCATION_CD >= :LOCATION_CD_FROM ");
                        parameters.Add(":LOCATION_CD_FROM", condition.LocationCdFrom);
                    }

                    // ロケーションTo
                    if (!string.IsNullOrEmpty(condition.LocationCdTo))
                    {
                        whereQuery.AppendLine(" AND TS.LOCATION_CD <= :LOCATION_CD_TO ");
                        parameters.Add(":LOCATION_CD_TO", condition.LocationCdTo);
                    }

                    // 格付
                    if (!string.IsNullOrEmpty(condition.GradeId))
                    {
                        whereQuery.AppendLine(" AND ML.GRADE_ID = :GRADE_ID ");
                        parameters.Add(":GRADE_ID", condition.GradeId);
                    }

                    StringBuilder query = new StringBuilder($@"
                        INSERT INTO WW_STK_MOVE (
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
                            ,IS_CHECK
                            ,LOCATION_CD
                            ,GRADE_ID
                            ,CASE_QTY
                            ,SKU_QTY
                            ,STOCK_QTY
                            ,ALLOC_QTY
                            ,NOT_ALLOC_QTY
                        )
                        WITH
                            STOCK_DATA AS (
                                SELECT
                                        TS.SHIPPER_ID
                                    ,   TS.CENTER_ID
                                    ,   TS.LOCATION_CD
                                    ,   MAX(ML.GRADE_ID)                   AS GRADE_ID
                                    ,   COUNT(DISTINCT(TS.ITEM_SKU_ID))    AS SKU_QTY
                                    ,   SUM(TS.STOCK_QTY)                  AS STOCK_QTY
                                    ,   SUM(TS.ALLOC_QTY)                  AS ALLOC_QTY
                                    ,   SUM(TS.STOCK_QTY - TS.ALLOC_QTY)   AS NOT_ALLOC_QTY
                                FROM
                                        T_STOCKS TS
                                INNER JOIN
                                        M_LOCATIONS ML
                                ON
                                        ML.SHIPPER_ID = TS.SHIPPER_ID
                                    AND ML.CENTER_ID = TS.CENTER_ID
                                    AND ML.LOCATION_CD = TS.LOCATION_CD
                                WHERE
                                        TS.SHIPPER_ID = :SHIPPER_ID
                                    {whereQuery}
                                    AND TS.STOCK_QTY > 0
                                    AND ML.LOCATION_CLASS NOT IN ('11', '12', '16', '22')
                                GROUP BY
                                        TS.SHIPPER_ID
                                    ,   TS.CENTER_ID
                                    ,   TS.LOCATION_CD
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
                            ,   ROW_NUMBER() OVER(ORDER BY SUB.LOCATION_CD)
                            ,   0
                            ,   SUB.LOCATION_CD
                            ,   MAX(SUB.GRADE_ID)
                            ,   COUNT(DISTINCT(TPS.BOX_NO))
                            ,   MAX(SUB.SKU_QTY)
                            ,   MAX(SUB.STOCK_QTY)
                            ,   MAX(SUB.ALLOC_QTY)
                            ,   MAX(SUB.NOT_ALLOC_QTY)
                        FROM
                                STOCK_DATA SUB
                        LEFT OUTER JOIN
                                T_PACKAGE_STOCKS TPS
                        ON
                                TPS.SHIPPER_ID  = SUB.SHIPPER_ID
                            AND TPS.CENTER_ID   = SUB.CENTER_ID
                            AND TPS.LOCATION_CD = SUB.LOCATION_CD
                            AND TPS.BOX_NO <> ' '
                        WHERE 1 = 1
                            {(condition.AllocQtyZero? "AND SUB.ALLOC_QTY = 0" : null)}
                        GROUP BY
                                SUB.SHIPPER_ID
                            ,   SUB.CENTER_ID
                            ,   SUB.LOCATION_CD
                    ");

                    parameters.Add(":USER_ID", Common.Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "InsertArrLocMove");
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (Exception ex) //デバッグ用
                {
                    Mvc.Common.AppError.PutLogREF(ex, "InsertArrLocMove");
                    trans.Rollback();
                    return false;
                }
                trans.Commit();
            }
            return true;
        }

        /// <summary>
        /// Get StockInquiry List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<LocMoveResultRow> LocMoveGetData(LocMoveSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                SELECT 
                	 WSM.*
                	,MG.GRADE_NAME
                FROM
                	WW_STK_MOVE WSM
                INNER JOIN
                	M_GRADES MG
                ON	WSM.SHIPPER_ID = MG.SHIPPER_ID
                AND WSM.GRADE_ID = MG.GRADE_ID
                WHERE
                	WSM.SHIPPER_ID = :SHIPPER_ID
                AND WSM.SEQ = :SEQ
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<LocMoveResultRow>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case LocMoveSort.Loc:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WSM.LOCATION_CD DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WSM.LOCATION_CD ASC ");
                            break;
                    }

                    break;
                case LocMoveSort.GradeLoc:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WSM.GRADE_ID DESC, WSM.LOCATION_CD DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WSM.GRADE_ID ASC, WSM.LOCATION_CD ASC ");
                            break;
                    }

                    break;
                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WSM.LOCATION_CD DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WSM.LOCATION_CD ASC ");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var locMoves = MvcDbContext.Current.Database.Connection.Query<LocMoveResultRow>(query.ToString(), parameters);
            var stkLocMoves = MvcDbContext.Current.StkLocMoves.Where(x => x.Seq == condition.Seq && x.ShipperId == Common.Profile.User.ShipperId);
            condition.LocationCount = stkLocMoves.Select(x => x.LocationCd).Count();
            condition.ItemSkuQtySum = stkLocMoves.Select(x => x.SkuQty).Sum();
            condition.StockQtySum = stkLocMoves.Select(x => x.StockQty).Sum();
            condition.AllocQtySum = stkLocMoves.Select(x => x.AllocQty).Sum();
            condition.NotAllocQtySum = stkLocMoves.Select(x => x.NotAllocQty).Sum();
            condition.SelectedCount = MvcDbContext.Current.StkLocMoves.Where(x => x.Seq == condition.Seq && x.ShipperId == Common.Profile.User.ShipperId && x.IsCheck).Count();

            // Excute paging
            return new StaticPagedList<LocMoveResultRow>(locMoves, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Update Work Table
        /// </summary>
        /// <param name="References"></param>
        /// <returns></returns>
        public bool StkStockAllChange(LocMoveSearchConditions conditions, bool check)
        {
            var dbContext = MvcDbContext.Current.Database.Connection;

            using (System.Data.Common.DbTransaction tran = dbContext.BeginTransaction())
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    DynamicParameters parameters = new DynamicParameters();

                    // 更新用
                    sql.Append(@"
                        UPDATE
                                WW_STK_MOVE
                        SET
                                UPDATE_DATE = SYSTIMESTAMP
                            ,   UPDATE_USER_ID = :USER_ID
                            ,   UPDATE_PROGRAM_NAME = :PROGRAM_NAME
                            ,   UPDATE_COUNT = UPDATE_COUNT + 1
                            ,   IS_CHECK = :IS_CHECK
                        WHERE
                                SHIPPER_ID = :SHIPPER_ID
                            AND SEQ = :SEQ
                            AND ALLOC_QTY = 0
                    ");

                    parameters.Add(":USER_ID", Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "StkStockAllChange");
                    parameters.Add(":IS_CHECK", check ? 1 : 0);
                    parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                    parameters.Add(":SEQ", conditions.Seq);

                    dbContext.Execute(sql.ToString(), parameters);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return false;
                }

                tran.Commit();
            }

            conditions.SelectedCount = MvcDbContext.Current.StkLocMoves
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq && m.IsCheck).Count();
            return true;
        }

        /// <summary>
        /// Update Work Table
        /// </summary>
        /// <param name="References"></param>
        /// <returns></returns>
        public bool UpdateStkLocMove(IList<SelectedLocMoveViewModel> References)
        {
            var sql = $@"
                UPDATE WW_STK_MOVE
                SET
                        UPDATE_DATE = SYSDATE
                    ,   UPDATE_USER_ID = :USER_ID
                    ,   UPDATE_PROGRAM_NAME = :PROGRAM_NAME
                    ,   UPDATE_COUNT = UPDATE_COUNT + 1
                    ,   IS_CHECK = :IS_CHECK
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                    AND SEQ = :SEQ
                    AND LOCATION_CD = :LOCATION_CD
            ";

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in References)
                {
                    try
                    {
                        MvcDbContext.Current.Database.Connection.Execute(sql,
                            new
                            {
                                USER_ID = Profile.User.UserId,
                                PROGRAM_NAME = BaseModel.GetProgramId(),
                                IS_CHECK = u.IsCheck ? 1 : 0,
                                SHIPPER_ID = Profile.User.ShipperId,
                                SEQ = u.Seq,
                                LOCATION_CD = u.LocationCd
                            });
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        trans.Rollback();
                        return false;
                    }
                }

                trans.Commit();
            }

            return true;
        }

        /// <summary>
        /// 実績更新
        /// </summary>
        public void UpdateLocMove(LocMoveSearchConditions searchConditions, out ProcedureStatus status, out string message)
        {
            var dbContext = MvcDbContext.Current;
            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
                    param.Add("IN_CENTER_ID", searchConditions.CenterId, DbType.String, ParameterDirection.Input);
                    param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
                    param.Add("IN_SEQ", searchConditions.Seq, DbType.Int64, ParameterDirection.Input);
                    param.Add("IN_LOCATION_CD", searchConditions.LocationCdMove, DbType.String, ParameterDirection.Input);
                    param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                    var db = MvcDbContext.Current.Database;

                    db.Connection.Execute(
                        "SP_STK_MOVE_LOCATION_ALL_MOVE_TO",
                        param,
                        commandType: CommandType.StoredProcedure);

                    status = param.Get<ProcedureStatus>("OUT_STATUS");
                    message = param.Get<string>("OUT_MESSAGE");

                    if (status == ProcedureStatus.Success)
                    {
                        trans.Commit();
                    }
                    else
                    {
                        trans.Rollback();
                    }
                }
                catch (Exception ex) //デバッグ用
                {
                    Mvc.Common.AppError.PutLogREF(ex, "UpdateLocMove");
                    trans.Rollback();
                    status = ProcedureStatus.Error;
                    message = "error";
                }
            }
        }

        /// <summary>
        /// ロケーション区分データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListLocationClasses()
        {
            var notInLocationClass = new List<string>() {"11", "12", "16", "22"};

            return MvcDbContext.Current.LocationClasses
                .Where(m => (m.ShipperId == Common.Profile.User.ShipperId) && !(notInLocationClass.Contains(m.LocationClass)))
                .Select(m => new SelectListItem
                {
                    Value = m.LocationClass,
                    Text = m.LocationName
                })
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// 格付データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListGrades()
        {
            return MvcDbContext.Current.Grades
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.GradeId,
                    Text = m.GradeName
                })
                .OrderBy(m => m.Value);
        }
    }
}