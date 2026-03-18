namespace Wms.Areas.Ship.Query.EcAllocation
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Common;
    using Share.Extensions.Classes;
    using Share.Models;
    using Wms.Areas.Ship.Models;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.EcAllocation;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.EcAllocation.EcAllocationSearchConditions;

    public class EcAllocationQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertShpEcAllocation(EcAllocationSearchConditions condition)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query;

                    // 1.ワークID採番
                    condition.Seq = new BaseQuery().GetWorkId();
                    condition.Page = 1;

                    query = new StringBuilder(@"
                        INSERT INTO WW_SHP_EC_ALLOCATION (
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
                            ,   SHIP_REQUEST_DATE
                            ,   SHIP_INSTRUCT_ID
                            ,   ORDER_DATE
                            ,   TRANSPORTER_ID
                            ,   TRANSPORTER_NAME
                            ,   ARRIVE_REQUEST_DATE
                            ,   DATA_DATE
                            ,   EC_SHIP_CLASS
                            ,   ORDER_QTY
                            ,   ALLOC_FLAG
                            ,   ALLOC_QTY
                            ,   ITEM_SKU_QTY
                        )
                        WITH
                            TARGET_ECSHIP_DATA AS (
                                SELECT
                                        TE.SHIP_INSTRUCT_ID
                                    ,   TE.CENTER_ID
                                    ,   TE.SHIPPER_ID
                                FROM
                                        T_ECSHIPS TE
                                WHERE
                                        TE.BATCH_NO = ' '
                                    AND TE.CANCEL_FLAG = 0
                                    AND TE.DELI_SHIWAKE_CD <> ' '
                                    AND TE.KAKU_FLAG = 0
                                    AND TE.AFT_ALLOC_CANCEL_FLAG = 0
                                    AND TE.AFT_ALLOC_UP_FLAG = 0
                                    AND TE.SHIPPER_ID = :SHIPPER_ID
                                    AND TE.CENTER_ID = :CENTER_ID
                    ");
                    // 引当状況指定が
                    if (!string.IsNullOrEmpty(condition.AllocationState))
                    {
                        if (condition.AllocationState == "2")
                        {
                            query.Append(" AND TE.ALLOC_FLAG = 0 ");
                        }
                        else if (condition.AllocationState == "3")
                        {
                            query.Append(" AND TE.ALLOC_FLAG = 1 ");
                        }
                    }

                    // 受信日時(From-To)
                    if (condition.AllocDateFrom != null)
                    {
                        query.Append(" AND TO_CHAR(TE.MAKE_DATE,'yyyy/MM/dd hh24:mi:ss') >= :ALLOC_DATE_TIME_FROM ");
                        parameters.Add(":ALLOC_DATE_TIME_FROM", condition.AllocTimeFrom == null ?
                                                                string.Format("{0:yyyy/MM/dd}", condition.AllocDateFrom) + " 00:00:00" :
                                                                string.Format("{0:yyyy/MM/dd}", condition.AllocDateFrom) + " " + condition.AllocTimeFrom);
                    }

                    if (condition.AllocDateTo != null)
                    {
                        query.Append(" AND TO_CHAR(TE.MAKE_DATE,'yyyy/MM/dd hh24:mi:ss') <= :ALLOC_DATE_TIME_TO ");
                        parameters.Add(":ALLOC_DATE_TIME_TO", condition.AllocTimeTo == null ?
                                                                string.Format("{0:yyyy/MM/dd}", condition.AllocDateTo) + " 23:59:59" :
                                                                string.Format("{0:yyyy/MM/dd}", condition.AllocDateTo) + " " + condition.AllocTimeTo);
                    }
                    if (!condition.AddConditions)
                    {
                        // 出荷日
                        if (condition.ShipDateBeforeToday && !condition.ShipDateAfterToday)
                        {
                            query.Append(" AND TE.SHIP_REQUEST_DATE <= :SHIP_REQUEST_DATE ");
                            parameters.Add(":SHIP_REQUEST_DATE", DateTimeOffset.Now);
                        }
                        if (!condition.ShipDateBeforeToday && condition.ShipDateAfterToday)
                        {
                            query.Append(" AND TE.SHIP_REQUEST_DATE > :SHIP_REQUEST_DATE ");
                            parameters.Add(":SHIP_REQUEST_DATE", DateTimeOffset.Now);
                        }
                    }
                    else
                    {
                        // 出荷予定日(From-To)
                        if (condition.ShipPlanDateFrom != null)
                        {
                            query.Append(" AND TE.SHIP_REQUEST_DATE >= :SHIP_REQUEST_DATE_FROM ");
                            parameters.Add(":SHIP_REQUEST_DATE_FROM", condition.ShipPlanDateFrom);
                        }

                        if (condition.ShipPlanDateTo != null)
                        {
                            query.Append(" AND TE.SHIP_REQUEST_DATE <= :SHIP_REQUEST_DATE_TO ");
                            parameters.Add(":SHIP_REQUEST_DATE_TO", condition.ShipPlanDateTo);
                        }
                        var flag = false;
                        query.Append(" AND ( ");
                        // EC出荷形態
                        if (condition.Single)
                        {
                            query.Append(" TE.EC_SHIP_CLASS = 1 ");
                            flag = true;
                        }

                        if (condition.Order && flag)
                        {
                            query.Append(" OR TE.EC_SHIP_CLASS = 2 ");
                        }
                        else if (condition.Order && !flag)
                        {
                            query.Append(" TE.EC_SHIP_CLASS = 2 ");
                            flag = true;
                        }

                        if (condition.Gas && flag)
                        {
                            query.Append(" OR TE.EC_SHIP_CLASS = 3 ");
                        }
                        else if (condition.Gas && !flag)
                        {
                            query.Append(" TE.EC_SHIP_CLASS = 3 ");
                            flag = true;
                        }
                        if (flag)
                        {
                            query.Append(" ) ");
                        }
                        else
                        {
                            query.Append(" 1 = 1) ");
                        }

                        // 配送業者
                        if (!string.IsNullOrEmpty(condition.TransporterId))
                        {
                            query.Append(" AND TE.TRANSPORTER_ID = :TRANSPORTER_ID ");
                            parameters.Add(":TRANSPORTER_ID", condition.TransporterId);
                        }

                        // 配送指定日
                        switch (condition.TransporterDateClass)
                        {
                            case TransporterDateClasses.NoDesignatedDate:
                                query.Append(" AND TE.ARRIVE_REQUEST_DATE IS NULL ");
                                break;
                            case TransporterDateClasses.WithDesignatedDate:
                                query.Append(" AND TE.ARRIVE_REQUEST_DATE IS NOT NULL ");
                                if (condition.TransporterDate != null)
                                {
                                    query.Append(" AND TE.ARRIVE_REQUEST_DATE = :ARRIVE_REQUEST_DATE ");
                                    parameters.Add(":ARRIVE_REQUEST_DATE", condition.TransporterDate);
                                }
                                break;
                            default:
                                break;
                        }

                        // 配送エリア
                        if (!string.IsNullOrEmpty(condition.TransporterArea))
                        {
                            string[] transporterAreas = (condition.TransporterArea ?? condition.TransporterArea).Split(',');
                            query.Append(" AND TE.PREF_ID IN :PREF_ID ");
                            parameters.Add(":PREF_ID", transporterAreas);
                        }

                        // 注文番号
                        if (!string.IsNullOrEmpty(condition.ShipInstructId))
                        {
                            query.Append(" AND TE.SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID ");
                            parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId);
                        }

                        // 品番
                        if (!string.IsNullOrEmpty(condition.ItemId))
                        {
                            query.Append(" AND TE.ITEM_ID LIKE :ITEM_ID ");
                            parameters.Add(":ITEM_ID", condition.ItemId + "%");
                        }

                        // JAN
                        if (!string.IsNullOrEmpty(condition.Jan))
                        {
                            query.Append(" AND TE.JAN LIKE :JAN ");
                            parameters.Add(":JAN", condition.Jan + "%");
                        }

                        // SKU
                        if (!string.IsNullOrEmpty(condition.ItemSkuId))
                        {
                            query.Append(" AND TE.ITEM_SKU_ID LIKE :ITEM_SKU_ID ");
                            parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId + "%");
                        }
                    }
                    query.Append(@"
                                GROUP BY
                                        TE.SHIP_INSTRUCT_ID
                                    ,   TE.CENTER_ID
                                    ,   TE.SHIPPER_ID
                        )
                        ,   SELECTED_ECSHIP_DATA AS (
                                SELECT
                                        MAX(TE.SHIP_REQUEST_DATE) SHIP_REQUEST_DATE
                                    ,   TE.SHIP_INSTRUCT_ID
                                    ,   MAX(TE.ORDER_DATE) ORDER_DATE
                                    ,   MAX(TE.TRANSPORTER_ID) TRANSPORTER_ID
                                    ,   MAX(TE.ARRIVE_REQUEST_DATE) ARRIVE_REQUEST_DATE
                                    ,   MAX(TE.MAKE_DATE) DATA_DATE
                                    ,   SUM(TE.ORDER_QTY) ORDER_QTY
                                    ,   MAX(TE.ALLOC_FLAG) ALLOC_FLAG
                                    ,   SUM(TE.ALLOC_QTY) ALLOC_QTY
                                    ,   COUNT(DISTINCT(TE.ITEM_SKU_ID)) ITEM_SKU_QTY
                                    ,   TE.CENTER_ID
                                    ,   TE.SHIPPER_ID
                                    ,   MAX(TE.EC_SHIP_CLASS) EC_SHIP_CLASS
                                FROM
                                        T_ECSHIPS TE
                                INNER JOIN
                                        TARGET_ECSHIP_DATA TGT
                                ON
                                        TE.SHIP_INSTRUCT_ID = TGT.SHIP_INSTRUCT_ID
                                    AND TE.CENTER_ID = TGT.CENTER_ID
                                    AND TE.SHIPPER_ID = TGT.SHIPPER_ID
                                GROUP BY
                                        TE.SHIP_INSTRUCT_ID
                                    ,   TE.CENTER_ID
                                    ,   TE.SHIPPER_ID
                        )
                        SELECT
                                :MAKE_DATE
                            ,   :MAKE_USER_ID
                            ,   'EcAllocation'
                            ,   :MAKE_DATE
                            ,   :MAKE_USER_ID
                            ,   'EcAllocation'
                            ,   0
                            ,   :SHIPPER_ID
                            ,   :SEQ
                            ,   ROWNUM
                            ,   0
                            ,   EC.CENTER_ID
                            ,   EC.SHIP_REQUEST_DATE
                            ,   EC.SHIP_INSTRUCT_ID
                            ,   EC.ORDER_DATE
                            ,   EC.TRANSPORTER_ID
                            ,   MT.TRANSPORTER_NAME
                            ,   EC.ARRIVE_REQUEST_DATE
                            ,   EC.DATA_DATE
                            ,   CASE
                                    WHEN EC.EC_SHIP_CLASS = 1 THEN 'シングル'
                                    WHEN EC.EC_SHIP_CLASS = 2 THEN 'オーダー'
                                    WHEN EC.EC_SHIP_CLASS = 3 THEN 'GAS'
                                    ELSE ''
                                END EC_SHIP_CLASS
                            ,   EC.ORDER_QTY
                            ,   EC.ALLOC_FLAG
                            ,   EC.ALLOC_QTY
                            ,   EC.ITEM_SKU_QTY
                        FROM
                                SELECTED_ECSHIP_DATA EC
                        INNER JOIN
                                M_TRANSPORTERS MT
                        ON
                                EC.TRANSPORTER_ID = MT.TRANSPORTER_ID
                            AND EC.SHIPPER_ID   = MT.SHIPPER_ID
                    ");
                    parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                    parameters.Add(":CENTER_ID", condition.CenterId);
                    parameters.Add(":MAKE_DATE", DateTimeOffset.Now);
                    parameters.Add(":MAKE_USER_ID", Profile.User.UserId);
                    parameters.Add(":SEQ", condition.Seq);

                    // 2.検索・ワーク作成
                    var result = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }
                trans.Commit();
            }

            StringBuilder sku_query;
            DynamicParameters sku_parameters = new DynamicParameters();
            sku_query = new StringBuilder(@"
                WITH
                    SELECTED_WK_DATA AS (
                        SELECT
                                SHIP_INSTRUCT_ID
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                        FROM
                                WW_SHP_EC_ALLOCATION
                        WHERE
                                SEQ = :SEQ
                            AND SHIPPER_ID = :SHIPPER_ID
                )
                SELECT
                        COUNT(DISTINCT EC.ITEM_SKU_ID)
                FROM
                        T_ECSHIPS EC
                INNER JOIN
                        SELECTED_WK_DATA WK
                ON
                        EC.SHIP_INSTRUCT_ID = WK.SHIP_INSTRUCT_ID
                    AND EC.CENTER_ID = WK.CENTER_ID
                    AND EC.SHIPPER_ID = WK.SHIPPER_ID
            ");
            sku_parameters.Add(":SEQ", condition.Seq);
            sku_parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            condition.ItemSkuSum = MvcDbContext.Current.Database.Connection.Query<int>(sku_query.ToString(), sku_parameters).FirstOrDefault();

            return true;
        }
        /// <summary>
        /// Get StockInquiry List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<EcAllocationResultRow> GetData(EcAllocationSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                SELECT *
                  FROM WW_SHP_EC_ALLOCATION
                 WHERE SHIPPER_ID = :SHIPPER_ID
                   AND SEQ = :SEQ ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<EcAllocationResultRow>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case EcAllocationSortKey.OrderNo:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY SHIP_INSTRUCT_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY SHIP_INSTRUCT_ID ASC ");
                            break;
                    }

                    break;

                case EcAllocationSortKey.OrderQty:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"
                                ORDER BY
                                        ORDER_QTY DESC
                                    ,   SHIP_REQUEST_DATE DESC
                                    ,   ORDER_DATE DESC
                                    ,   SHIP_INSTRUCT_ID DESC ");
                            break;
                        default:
                            query.AppendLine(@"
                                ORDER BY 
                                        ORDER_QTY ASC 
                                    ,   SHIP_REQUEST_DATE ASC
                                    ,   ORDER_DATE ASC
                                    ,   SHIP_INSTRUCT_ID ASC ");
                            break;
                    }
                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY SHIP_REQUEST_DATE DESC,ORDER_DATE DESC,SHIP_INSTRUCT_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY SHIP_REQUEST_DATE ASC,ORDER_DATE ASC,SHIP_INSTRUCT_ID ASC ");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var ecAllocations = MvcDbContext.Current.Database.Connection.Query<EcAllocationResultRow>(query.ToString(), parameters);
            var shpEcAllocations = MvcDbContext.Current.ShpEcAllocations.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId);
            condition.OrderSum = shpEcAllocations.Select(x => x.ShipInstructId).Distinct().Count();
            condition.InstructQtySum = shpEcAllocations.Select(x => x.OrderQty).Sum();
            condition.SelectedCnt = MvcDbContext.Current.ShpEcAllocations.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId && x.IsCheck).Count();
            condition.SelectedErrCnt = MvcDbContext.Current.ShpEcAllocations.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId && x.AllocFlag != 0 && x.IsCheck).Count();
            // Excute paging
            return new StaticPagedList<EcAllocationResultRow>(ecAllocations, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Update Work Table
        /// </summary>
        /// <param name="StockInquirys"></param>
        /// <returns></returns>
        public bool UpdateShpEcAllocation(IList<SelectedEcAllocationViewModel> ShpEcAllocations)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in ShpEcAllocations)
                {
                    // 在庫明細
                    var shpEcAllocation = MvcDbContext.Current.ShpEcAllocations
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == u.Seq && m.LineNo == u.LineNo)
                                  .SingleOrDefault();

                    if (shpEcAllocation == null)
                    {
                        return false;
                    }

                    shpEcAllocation.SetBaseInfoUpdate();
                    shpEcAllocation.IsCheck = u.IsCheck;
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
        /// 実績更新
        /// </summary>
        public void AllocationUpdate(EcAllocationSearchConditions searchConditions)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", searchConditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_BATCH_NAME", searchConditions.AllocationName, DbType.String, ParameterDirection.Input);
            param.Add("IN_WORK_ID", searchConditions.Seq, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_EC_ALLOC_KIND", searchConditions.AllocationState, DbType.Int32, ParameterDirection.Input);
            if (searchConditions.OrderBatchNo || searchConditions.AllOrderBatchNo)
            {
                param.Add("IN_EC_ONE_BATCH_CLASS", 2, DbType.Int32, ParameterDirection.Input);
            }
            else if (searchConditions.BatchNoInUnit)
            {
                param.Add("IN_EC_ONE_BATCH_CLASS", 1, DbType.Int32, ParameterDirection.Input);
            }
            else
            {
                param.Add("IN_EC_ONE_BATCH_CLASS", 0, DbType.Int32, ParameterDirection.Input);
            }
            if (searchConditions.All)
            {
                param.Add("IN_DO_ORDER_PIC", 1, DbType.Int32, ParameterDirection.Input);
            }
            else
            {
                param.Add("IN_DO_ORDER_PIC", 0, DbType.Int32, ParameterDirection.Input);
            }

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_ECAllOC_MAIN",
                param,
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 実績更新
        /// </summary>
        public void AllocationRelieve(EcAllocationSearchConditions searchConditions, out ProcedureStatus status, out string message)
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
                "SP_W_SHP_ECAllOC_SAVE_CANCEL",
                param,
                commandType: CommandType.StoredProcedure);
            status = param.Get<ProcedureStatus>("OUT_STATUS");
            if (status == ProcedureStatus.Success)
            {
                message = EcAllocationResource.SUC_RELIEVE;
            }
            else
            {
                message = param.Get<string>("OUT_MESSAGE");
            }
        }

        /// <summary>
        /// Update Work Table
        /// </summary>
        /// <param name="StockInquirys"></param>
        /// <returns></returns>
        public bool ShpEcAllocationAllChange(EcAllocationSearchConditions conditions, bool check)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query;
                    query = new StringBuilder(@"
                        UPDATE  WW_SHP_EC_ALLOCATION
                        SET
                                UPDATE_DATE = :UPDATE_DATE
                            ,   UPDATE_USER_ID = :UPDATE_USER_ID
                            ,   UPDATE_PROGRAM_NAME = 'EcAllocation'
                            ,   UPDATE_COUNT = UPDATE_COUNT + 1
                            ,   IS_CHECK = :IS_CHECK
                        WHERE
                                SEQ = :SEQ
                            AND SHIPPER_ID = :SHIPPER_ID
                    ");
                    parameters.Add(":UPDATE_DATE", DateTimeOffset.Now);
                    parameters.Add(":UPDATE_USER_ID", Profile.User.UserId);
                    parameters.Add(":IS_CHECK", (check == true) ? 1 : 0);
                    parameters.Add(":SEQ", conditions.Seq);
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                    var result = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }
                trans.Commit();
            }

            conditions.SelectedCnt = MvcDbContext.Current.ShpEcAllocations
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq && m.IsCheck).Count();
            conditions.SelectedErrCnt = MvcDbContext.Current.ShpEcAllocations
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq && m.AllocFlag != 0 && m.IsCheck).Count();
            return true;
        }

        /// <summary>
        /// 事業部データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListTransporters()
        {
            return MvcDbContext.Current.Transporters
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.TransporterId.ToString(),
                    Text = m.TransporterName
                })
                .OrderBy(m => m.Value);
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
        /// Get Detail Data
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public EcAllocationDetailViewModel GetDetailData(long Seq, long LineNo)
        {
            var shpEcAllocation = MvcDbContext.Current.ShpEcAllocations
                .Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == Seq && x.LineNo == LineNo).FirstOrDefault();
            // 明細ヘッダ
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT
                        T.DEST_PREF_NAME
                    ,   MT.TRANSPORTER_NAME
                    ,   T.DELI_SHIWAKE_CD
                    ,   MG.GEN_NAME EC_SHIP_CLASS_NAME
                    ,   T.ARRIVE_REQUEST_DATE
                    ,   MG1.GEN_NAME ARRIVE_REQUEST_TIME
                    ,   T.ITEM_SKU_QTY_SUM
                    ,   T.ORDER_QTY_SUM
                FROM (
                    SELECT
                            MAX(TE.DEST_PREF_NAME) DEST_PREF_NAME
                        ,   MAX(TE.TRANSPORTER_ID) TRANSPORTER_ID
                        ,   MAX(TE.DELI_SHIWAKE_CD) DELI_SHIWAKE_CD
                        ,   MAX(TE.EC_SHIP_CLASS) EC_SHIP_CLASS
                        ,   MAX(TE.ARRIVE_REQUEST_DATE) ARRIVE_REQUEST_DATE
                        ,   MAX(TE.ARRIVE_REQUEST_TIME) ARRIVE_REQUEST_TIME
                        ,   COUNT(DISTINCT(TE.ITEM_SKU_ID)) ITEM_SKU_QTY_SUM
                        ,   SUM(TE.ORDER_QTY) ORDER_QTY_SUM
                        ,   TE.CENTER_ID
                        ,   TE.SHIPPER_ID
                    FROM
                            T_ECSHIPS TE
                    WHERE
                            TE.BATCH_NO = ' '
                        AND TE.CANCEL_FLAG = 0
                        AND TE.DELI_SHIWAKE_CD <> ' '
                        AND TE.KAKU_FLAG = 0
                        AND TE.AFT_ALLOC_CANCEL_FLAG = 0
                        AND TE.AFT_ALLOC_UP_FLAG = 0
                        AND TE.SHIPPER_ID = :SHIPPER_ID
                        AND TE.CENTER_ID = :CENTER_ID
                        AND TE.SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                    GROUP BY
                            TE.SHIP_INSTRUCT_ID
                        ,   TE.CENTER_ID
                        ,   TE.SHIPPER_ID
                ) T
                LEFT OUTER JOIN
                        M_GENERALS MG
                ON
                        T.SHIPPER_ID = MG.SHIPPER_ID
                    AND MG.REGISTER_DIVI_CD = '1'
                    AND MG.CENTER_ID = '@@@'
                    AND MG.GEN_DIV_CD = 'EC_SHIP_CLASS'
                    AND TO_CHAR(T.EC_SHIP_CLASS) = MG.GEN_CD
                LEFT OUTER JOIN
                        M_GENERALS MG1
                ON
                        T.SHIPPER_ID = MG1.SHIPPER_ID
                    AND MG1.REGISTER_DIVI_CD = '1'
                    AND MG1.CENTER_ID = '@@@'
                    AND MG1.GEN_DIV_CD = 'SAGAWA_ARRIVE_REQUEST_TIME'
                    AND T.ARRIVE_REQUEST_TIME = MG1.GEN_CD
                LEFT OUTER JOIN
                        M_TRANSPORTERS MT
                ON
                        T.SHIPPER_ID   = MT.SHIPPER_ID
                    AND T.TRANSPORTER_ID = MT.TRANSPORTER_ID");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", shpEcAllocation.CenterId);
            parameters.Add(":SHIP_INSTRUCT_ID", shpEcAllocation.ShipInstructId);

            EcAllocationDetailViewModel vm = new EcAllocationDetailViewModel();
            vm.DetailHead = new EcAllocationDetailHead();
            vm.DetailHead = MvcDbContext.Current.Database.Connection.Query<EcAllocationDetailHead>(query.ToString(), parameters).FirstOrDefault();
            if (shpEcAllocation != null)
            {
                vm.DetailHead.ShipRequestDate = shpEcAllocation.ShipRequestDate;
                vm.DetailHead.DataDate = shpEcAllocation.DataDate;
                vm.DetailHead.ShipInstructId = shpEcAllocation.ShipInstructId;
                vm.DetailHead.OrderDate = shpEcAllocation.OrderDate;
            }

            // 明細
            parameters = new DynamicParameters();
            query = new StringBuilder(@"
                    SELECT TE.SHIP_INSTRUCT_SEQ
                          ,TE.ITEM_ID
                          ,TE.ITEM_NAME
                          ,TE.ITEM_COLOR_ID
                          ,MC.ITEM_COLOR_NAME
                          ,TE.ITEM_SIZE_ID
                          ,MS.ITEM_SIZE_NAME
                          ,TE.JAN
                          ,TE.ORDER_QTY
                      FROM T_ECSHIPS TE
                     INNER JOIN M_COLORS MC
                        ON TE.SHIPPER_ID   = MC.SHIPPER_ID
                       AND TE.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                     INNER JOIN M_SIZES MS
                        ON TE.SHIPPER_ID   = MS.SHIPPER_ID
                       AND TE.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                     WHERE TE.BATCH_NO = ' '
                       AND TE.CANCEL_FLAG = 0
                       AND TE.DELI_SHIWAKE_CD <> ' '
                       AND TE.KAKU_FLAG = 0
                       AND TE.AFT_ALLOC_CANCEL_FLAG = 0
                       AND TE.AFT_ALLOC_UP_FLAG = 0
                       AND TE.SHIPPER_ID = :SHIPPER_ID
                       AND TE.CENTER_ID = :CENTER_ID
                       AND TE.SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", shpEcAllocation.CenterId);
            parameters.Add(":SHIP_INSTRUCT_ID", shpEcAllocation.ShipInstructId);
            vm.EcAllocationDetails = MvcDbContext.Current.Database.Connection.Query<EcAllocationDetailResultRow>(query.ToString(), parameters);
            return vm;
        }
    }
}