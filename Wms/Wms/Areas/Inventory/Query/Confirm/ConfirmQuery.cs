namespace Wms.Areas.Inventory.Query.Confirm
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
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Inventory.ViewModels.Confirm.ConfirmSearchConditions;

    public class ConfirmQuery
    {

        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertInvConfirmSc01(ConfirmSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT ML.CENTER_ID
                      ,ML.INVENTORY_NO
                      ,ML.INVENTORY_START_DATE
                      ,MIN(NVL(IP.INVENTORY_CLASS, 2)) INVENTORY_CLASS
                      ,ML.INVENTORY_NAME
                      ,ML.INVENTORY_CONFIRM_FLAG
                      ,ML.INVENTORY_CONFIRM_DATE
                  FROM
                        M_LOCATIONS ML
                    LEFT OUTER JOIN
                        T_INVENTORY_PLANS IP
                    ON
                            IP.SHIPPER_ID = ML.SHIPPER_ID
                        AND IP.CENTER_ID = ML.CENTER_ID
                        AND IP.LOCATION_CD = ML.LOCATION_CD
                        AND IP.INVENTORY_NO = ML.INVENTORY_NO
                 WHERE
                        ML.SHIPPER_ID = :SHIPPER_ID
                    AND ML.INVENTORY_NO IS NOT NULL
                    AND ML.INVENTORY_CONFIRM_FLAG = 2
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

            // Add search condition
            // センター
            if (!string.IsNullOrEmpty(condition.CenterId))
            {
                query.Append(" AND ML.CENTER_ID = :CENTER_ID ");
                parameters.Add(":CENTER_ID", condition.CenterId);
            }
            // 棚卸No
            if (!string.IsNullOrEmpty(condition.InventoryNo))
            {
                query.Append(" AND ML.INVENTORY_NO = :INVENTORY_NO ");
                parameters.Add(":INVENTORY_NO", condition.InventoryNo);
            }
            // 棚卸開始日
            if (condition.InventoryDateFrom != null)
            {
                query.Append(" AND TRUNC(ML.INVENTORY_START_DATE) >= :INVENTORY_START_DATE_FROM ");
                parameters.Add(":INVENTORY_START_DATE_FROM", condition.InventoryDateFrom);
            }

            if (condition.InventoryDateTo != null)
            {
                query.Append(" AND TRUNC(ML.INVENTORY_START_DATE) <= :INVENTORY_START_DATE_TO ");
                parameters.Add(":INVENTORY_START_DATE_TO", condition.InventoryDateTo);
            }

            query.Append(" GROUP BY ML.CENTER_ID, ML.INVENTORY_NO, ML.INVENTORY_START_DATE, ML.INVENTORY_NAME, ML.INVENTORY_CONFIRM_FLAG, ML.INVENTORY_CONFIRM_DATE ");

            // 1.ワークID採番
            condition.Seq = new BaseQuery().GetWorkId();
            condition.Page = 1;
            // 2.検索・ワーク作成
            var result = MvcDbContext.Current.Database.Connection.Query<InventoryConfirm>(query.ToString(), parameters);
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var row in result.Select((v, i) => new { v, i }))
                {
                    var inventoryConfirm = new InventoryConfirm();
                    inventoryConfirm.SetBaseInfoInsert();
                    inventoryConfirm.Seq = condition.Seq;
                    inventoryConfirm.LineNo = row.i + 1;
                    inventoryConfirm.IsCheck = false;
                    inventoryConfirm.CenterId = row.v.CenterId;
                    inventoryConfirm.InventoryNo = row.v.InventoryNo;
                    inventoryConfirm.InventoryStartDate = row.v.InventoryStartDate;
                    inventoryConfirm.InventoryClass = row.v.InventoryClass;
                    inventoryConfirm.InventoryName = row.v.InventoryName;
                    inventoryConfirm.InventoryConfirmFlag = row.v.InventoryConfirmFlag;
                    inventoryConfirm.InventoryConfirmDate = row.v.InventoryConfirmDate;
                    MvcDbContext.Current.InventoryConfirms.Add(inventoryConfirm);
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

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<ConfirmResultRow> GetData(ConfirmSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                SELECT W.INVENTORY_NO
                      ,W.INVENTORY_START_DATE
                      ,W.INVENTORY_CLASS
                      ,W.INVENTORY_NAME
                      ,CASE WHEN W.INVENTORY_CONFIRM_FLAG = 1 THEN '" + ConfirmResource.Undecided + @"'
                            WHEN W.INVENTORY_CONFIRM_FLAG = 2 THEN '" + ConfirmResource.ProvisionalDecision + @"' END INVENTORY_CONFIRM_FLAG
                      ,W.INVENTORY_CONFIRM_DATE
                      ,W.SEQ
                      ,W.LINE_NO
                      ,W.IS_CHECK
                      ,CASE
                        WHEN
                            (SELECT CASE
                                        WHEN T.INVENTORY_NO IS NULL THEN 0 ELSE 1 END INVENTORY_NO
                                    FROM
                                        T_INVENTORY_RESULTS T
                                    WHERE
                                            W.SHIPPER_ID = T.SHIPPER_ID
                                        AND W.INVENTORY_NO = T.INVENTORY_NO
                                        AND W.CENTER_ID = T.CENTER_ID
                                        AND ROWNUM = 1
                            ) = 1 THEN 1
                        WHEN
                            (SELECT CASE
                                    WHEN ML.INVENTORY_NO IS NOT NULL 
                                        AND IP.INVENTORY_NO IS NULL THEN 1 ELSE 0 END INVENTORY_NO
                                    FROM
                                        M_LOCATIONS ML
                                    LEFT OUTER JOIN
                                        T_INVENTORY_PLANS IP
                                    ON
                                            ML.SHIPPER_ID = IP.SHIPPER_ID
                                        AND ML.CENTER_ID = IP.CENTER_ID
                                        AND ML.LOCATION_CD = IP.LOCATION_CD
                                        AND ML.INVENTORY_NO = IP.INVENTORY_NO
                                    WHERE
                                            W.SHIPPER_ID = ML.SHIPPER_ID
                                        AND W.INVENTORY_NO = ML.INVENTORY_NO
                                        AND W.CENTER_ID = ML.CENTER_ID
                                        AND ROWNUM = 1
                            ) = 1 THEN 1

                        ELSE 0 END RESULTS_DATA_FLAG
                 FROM  WW_INV_CONFIRM_SC01 W
                 WHERE W.SHIPPER_ID = :SHIPPER_ID
                   AND W.SEQ = :SEQ
              ORDER BY W.INVENTORY_NO
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<ConfirmResultRow>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var stockInquirys = MvcDbContext.Current.Database.Connection.Query<ConfirmResultRow>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<ConfirmResultRow>(stockInquirys, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// 本確定
        /// </summary>
        public void InventoryConfirm(string centerId, string inventoryNo, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", centerId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_INVENTORY_NO", inventoryNo, DbType.String, ParameterDirection.Input);
            param.Add("IN_CONFIRM_FLAG", 3, DbType.Int32, ParameterDirection.Input);
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
        /// もう一度本確定
        /// </summary>
        public void InventoryAgainConfirm(string centerId, string inventoryNo, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", centerId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_INVENTORY_NO", inventoryNo, DbType.String, ParameterDirection.Input);
            param.Add("IN_CONFIRM_FLAG", 3, DbType.Int32, ParameterDirection.Input);
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
        /// Update Work Table
        /// </summary>
        /// <param name="References"></param>
        /// <returns></returns>
        public bool UpdateInvConfirm(long seq, long lineNo)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {

                //
                var reference = MvcDbContext.Current.InventoryConfirms
                                .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == seq && m.LineNo == lineNo)
                                .SingleOrDefault();

                if (reference == null)
                {
                    return false;
                }

                reference.SetBaseInfoUpdate();
                reference.IsCheck = true;
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
        /// 棚卸状況データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListInventoryConfirmFlag()
        {
            return MvcDbContext.Current.Generals
                .Where(m => m.ShipperId == Profile.User.ShipperId && m.RegisterDiviCd == "1" && m.GenDivCd == "INVENTORY_STATUS" && m.GenCd != "4" && m.GenCd != "1")
                .Select(m => new SelectListItem
                {
                    Value = m.GenCd.ToString(),
                    Text = m.GenName,
                })
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// 棚卸Noデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListInventoryNo(ConfirmSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT DISTINCT ML.INVENTORY_NO VALUE
                      ,ML.INVENTORY_NO || ':' || ML.INVENTORY_NAME TEXT
                      ,ML.INVENTORY_START_DATE
                  FROM
                        M_LOCATIONS ML
                 WHERE
                        ML.SHIPPER_ID = :SHIPPER_ID
                    AND ML.INVENTORY_NO IS NOT NULL
                    AND ML.INVENTORY_CONFIRM_FLAG = 2
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

            // センター
            if (!string.IsNullOrEmpty(condition.CenterId))
            {
                query.Append(" AND ML.CENTER_ID = :CENTER_ID ");
                parameters.Add(":CENTER_ID", condition.CenterId);
            }

            // 棚卸開始日
            if (condition.InventoryDateFrom != null)
            {
                query.Append(" AND TRUNC(ML.INVENTORY_START_DATE) >= :INVENTORY_START_DATE_FROM ");
                parameters.Add(":INVENTORY_START_DATE_FROM", condition.InventoryDateFrom);
            }

            if (condition.InventoryDateTo != null)
            {
                query.Append(" AND TRUNC(ML.INVENTORY_START_DATE) <= :INVENTORY_START_DATE_TO ");
                parameters.Add(":INVENTORY_START_DATE_TO", condition.InventoryDateTo);
            }

            query.Append(" ORDER BY ML.INVENTORY_START_DATE ");

            // 全レコード数を取得
            return MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters);
        }

        /// <summary>
        /// 棚卸Noデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetInventoryNoList(string InventoryDateFrom, string InventoryDateTo, string InventoryStatus, string CenterId)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT DISTINCT ML.INVENTORY_NO VALUE
                      ,ML.INVENTORY_NO || ':' || ML.INVENTORY_NAME TEXT
                      ,ML.INVENTORY_START_DATE
                  FROM
                        M_LOCATIONS ML
                 WHERE
                        ML.SHIPPER_ID = :SHIPPER_ID
                    AND ML.INVENTORY_NO IS NOT NULL
                    AND ML.INVENTORY_CONFIRM_FLAG = 2
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

            // センター
            if (!string.IsNullOrEmpty(CenterId))
            {
                query.Append(" AND ML.CENTER_ID = :CENTER_ID ");
                parameters.Add(":CENTER_ID", CenterId);
            }

            // 棚卸開始日
            if (InventoryDateFrom != null && InventoryDateFrom != "")
            {
                query.Append(" AND TRUNC(ML.INVENTORY_START_DATE) >= :INVENTORY_START_DATE_FROM ");
                parameters.Add(":INVENTORY_START_DATE_FROM", InventoryDateFrom);
            }

            if (InventoryDateTo != null && InventoryDateTo != "")
            {
                query.Append(" AND TRUNC(ML.INVENTORY_START_DATE) <= :INVENTORY_START_DATE_TO ");
                parameters.Add(":INVENTORY_START_DATE_TO", InventoryDateTo);
            }

            query.Append(" ORDER BY ML.INVENTORY_START_DATE ");

            // 全レコード数を取得
            return MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters);
        }
    }
}