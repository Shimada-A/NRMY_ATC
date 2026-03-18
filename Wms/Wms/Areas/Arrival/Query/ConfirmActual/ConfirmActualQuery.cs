namespace Wms.Areas.Arrival.Query.ConfirmActual
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
    using Wms.Areas.Arrival.Models;
    using Wms.Areas.Arrival.Resources;
    using Wms.Areas.Arrival.ViewModels.ConfirmActual;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Arrival.ViewModels.ConfirmActual.ConfirmActualSearchConditions;

    public class ConfirmActualQuery
    {

        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertArrConfirmActual01(ConfirmActualSearchConditions condition)
        {
            condition.Seq = new BaseQuery().GetWorkId();
            condition.Page = 1;

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                        INSERT INTO WW_ARR_CON_ACT01 (
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
                            ,   ARRIVE_PLAN_DATE
                            ,   ARRIVE_DATE
                            ,   VENDOR_ID
                            ,   VENDOR_NAME
                            ,   INVOICE_NO
                            ,   PO_ID
                            ,   CATEGORY_ID1
                            ,   CATEGORY_NAME1
                            ,   ITEM_ID
                            ,   ITEM_NAME
                            ,   ITEM_SKU_ID
                            ,   SKU_QTY
                            ,   PLAN_BOX_NO_QTY
                            ,   RESULT_BOX_NO_QTY
                            ,   ARRIVE_PLAN_QTY
                            ,   RESULT_QTY
                            ,   ARRIVAL_STATUS
                            ,   ARRIVAL_STATUS_NAME
                            ,   RESULT_UPDATE_COUNT
                        )
                        WITH
                            ARRIVAL_STATUS AS (
                                SELECT
                                        SHIPPER_ID
                                    ,   GEN_CD
                                    ,   GEN_NAME 
                                FROM
                                        M_GENERALS GENERAL
                                WHERE
                                        GENERAL.CENTER_ID = '@@@'
                                    AND GENERAL.REGISTER_DIVI_CD = '1'
                                    AND GENERAL.GEN_DIV_CD  = 'ARRIVAL_STATUS'
                        )
                        ,   PACKING_ARRIVE_RESULT AS (
                                SELECT
                                        TPAR.SHIPPER_ID
                                    ,   TPAR.CENTER_ID
                                    ,   TPAR.INVOICE_NO
                                    ,   COUNT(DISTINCT TRIM(TPAR.BOX_NO)) RESULT_BOX_NO_QTY
                                FROM
                                        T_PACKING_ARRIVE_RESULTS TPAR
                                GROUP BY
                                        TPAR.SHIPPER_ID
                                    ,   TPAR.CENTER_ID
                                    ,   TPAR.INVOICE_NO
                        )
                        ,   PACKING_ARRIVE_PLAN AS (
                                SELECT
                                        TPAP.SHIPPER_ID
                                    ,   TPAP.CENTER_ID
                                    ,   TPAP.INVOICE_NO
                                    ,   COUNT(DISTINCT TRIM(TPAP.BOX_NO)) PLAN_BOX_NO_QTY
                                FROM
                                        T_PACKING_ARRIVE_PLANS TPAP
                                GROUP BY
                                        TPAP.SHIPPER_ID
                                    ,   TPAP.CENTER_ID
                                    ,   TPAP.INVOICE_NO
                        )
                        SELECT
                                SYSTIMESTAMP
                            ,   :USER_ID
                            ,   :PROGRAM_NAME
                            ,   SYSTIMESTAMP
                            ,   :USER_ID
                            ,   :PROGRAM_NAME
                            ,   0
                            ,   TAR.SHIPPER_ID
                            ,   :SEQ
                            ,   ROW_NUMBER() OVER(ORDER BY TAR.SHIPPER_ID)
                            ,   0
                            ,   TAR.CENTER_ID
                            ,   MAX(TAP.ARRIVE_PLAN_DATE) AS ARRIVE_PLAN_DATE
                            ,   MAX(TAR.ARRIVE_DATE) AS ARRIVE_DATE
                            ,   MAX(TAR.VENDOR_ID) AS VENDOR_ID
                            ,   MAX(MV.VENDOR_NAME1) AS VENDOR_NAME
                            ,   TAR.INVOICE_NO
                            ,   MAX(TAP.PO_ID) AS PO_ID
                            ,   MAX(MIC.CATEGORY_ID1) AS CATEGORY_ID1
                            ,   MAX(MIC.CATEGORY_NAME1) AS CATEGORY_NAME1
                            ,   MAX(TAR.ITEM_ID) AS ITEM_ID
                            ,   MAX(MIS.ITEM_NAME) AS ITEM_NAME
                            ,   MAX(MIS.ITEM_SKU_ID) AS ITEM_SKU_ID
                            ,   COUNT(TAR.ITEM_SKU_ID) AS SKU_QTY
                            ,   MAX(PAP.PLAN_BOX_NO_QTY) AS PLAN_BOX_NO_QTY
                            ,   MAX(PAR.RESULT_BOX_NO_QTY) AS RESULT_BOX_NO_QTY
                            ,   SUM(TAP.ARRIVE_PLAN_QTY) AS ARRIVE_PLAN_QTY
                            ,   SUM(TAR.RESULT_QTY) AS RESULT_QTY
                            ,   MAX(TAR.ARRIVAL_STATUS) AS ARRIVAL_STATUS
                            ,   MAX(ARRIVAL_STATUS.GEN_NAME) AS ARRIVAL_STATUS_NAME
                            ,   SUM(TAR.UPDATE_COUNT) AS RESULT_UPDATE_COUNT
                        FROM
                                T_ARRIVE_RESULTS TAR
                        LEFT JOIN
                                PACKING_ARRIVE_RESULT PAR
                        ON
                                PAR.SHIPPER_ID = TAR.SHIPPER_ID
                            AND PAR.CENTER_ID = TAR.CENTER_ID
                            AND PAR.INVOICE_NO = TAR.INVOICE_NO
                        LEFT JOIN
                                T_ARRIVE_PLANS TAP
                        ON
                                TAP.SHIPPER_ID = TAR.SHIPPER_ID
                            AND TAP.CENTER_ID = TAR.CENTER_ID
                            AND TAP.INVOICE_NO = TAR.INVOICE_NO
                            AND TAP.INVOICE_SEQ = TAR.INVOICE_SEQ
                        LEFT JOIN
                                PACKING_ARRIVE_PLAN PAP
                        ON
                                PAP.SHIPPER_ID = TAP.SHIPPER_ID
                            AND PAP.CENTER_ID = TAP.CENTER_ID
                            AND PAP.INVOICE_NO = TAP.INVOICE_NO
                        LEFT JOIN
                                M_VENDORS MV
                        ON
                                MV.SHIPPER_ID = TAR.SHIPPER_ID
                            AND MV.VENDOR_ID = TAR.VENDOR_ID
                        LEFT JOIN
                                M_ITEM_SKU MIS
                        ON
                                MIS.SHIPPER_ID = TAR.SHIPPER_ID
                            AND MIS.ITEM_SKU_ID = TAR.ITEM_SKU_ID
                        LEFT JOIN
                                M_DIVISIONS MD
                        ON
                                MD.SHIPPER_ID = MIS.SHIPPER_ID
                            AND MD.DIVISION_ID = MIS.DIVISION_ID
                        LEFT JOIN
                                M_ITEM_CATEGORIES4 MIC
                        ON
                                MIC.SHIPPER_ID = MIS.SHIPPER_ID
                            AND MIC.CATEGORY_ID1 = MIS.CATEGORY_ID1
                            AND MIC.CATEGORY_ID2 = MIS.CATEGORY_ID2
                            AND MIC.CATEGORY_ID3 = MIS.CATEGORY_ID3
                            AND MIC.CATEGORY_ID4 = MIS.CATEGORY_ID4
                        LEFT JOIN
                                M_BRANDS MB
                        ON
                                MB.SHIPPER_ID = MIS.SHIPPER_ID
                            AND MB.BRAND_ID = MIS.BRAND_ID
                        LEFT JOIN
                                ARRIVAL_STATUS
                        ON
                                ARRIVAL_STATUS.SHIPPER_ID = TAR.SHIPPER_ID
                            AND ARRIVAL_STATUS.GEN_CD = TO_CHAR(TAR.ARRIVAL_STATUS)
                        WHERE
                                TAR.SHIPPER_ID = :SHIPPER_ID
                    ");
                    parameters.Add(":USER_ID", Common.Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "InsertArrConfirmActual01");
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

                    // Add search condition
                    // センター
                    if (!string.IsNullOrEmpty(condition.CenterId))
                    {
                        query.Append(" AND TAR.CENTER_ID = :CENTER_ID ");
                        parameters.Add(":CENTER_ID", condition.CenterId);
                    }

                    // 入荷予定開始日
                    if (condition.ArrivePlanDateFrom != null)
                    {
                        query.Append(" AND TAP.ARRIVE_PLAN_DATE >= :ARRIVE_PLAN_DATE_FROM ");
                        parameters.Add(":ARRIVE_PLAN_DATE_FROM", condition.ArrivePlanDateFrom);
                    }

                    // 入荷予定終了日
                    if (condition.ArrivePlanDateTo != null)
                    {
                        query.Append(" AND TAP.ARRIVE_PLAN_DATE <= :ARRIVE_PLAN_DATE_TO ");
                        parameters.Add(":ARRIVE_PLAN_DATE_TO", condition.ArrivePlanDateTo);
                    }

                    // 入荷日
                    if (condition.ArriveDate != ConfirmActualResource.None && condition.ArriveDate != null)
                    {
                        query.Append(" AND TO_CHAR(TAR.ARRIVE_DATE,'YYYY/MM/DD') = :ARRIVE_DATE ");
                        parameters.Add(":ARRIVE_DATE", condition.ArriveDate);
                    }

                    // 事業部
                    if (!string.IsNullOrEmpty(condition.DivisionCd))
                    {
                        query.Append(" AND MD.DIVISION_ID = :DIVISION_ID ");
                        parameters.Add(":DIVISION_ID", condition.DivisionCd);
                    }

                    // ブランド
                    if (!string.IsNullOrEmpty(condition.BrandId))
                    {
                        query.Append(" AND MIS.BRAND_ID LIKE :BRAND_ID ");
                        parameters.Add(":BRAND_ID", condition.BrandId + "%");
                    }

                    // ブランド名
                    if (string.IsNullOrEmpty(condition.BrandId) && !string.IsNullOrEmpty(condition.BrandName))
                    {
                        query.Append(" AND MB.BRAND_NAME LIKE :BRAND_NAME ");
                        parameters.Add(":BRAND_NAME", condition.BrandName + "%");
                    }

                    // 代表仕入先
                    if (!string.IsNullOrEmpty(condition.VendorId))
                    {
                        query.Append(" AND TAR.VENDOR_ID LIKE :VENDOR_ID ");
                        parameters.Add(":VENDOR_ID", condition.VendorId + "%");
                    }

                    // 代表仕入先名
                    if (string.IsNullOrEmpty(condition.VendorId) && !string.IsNullOrEmpty(condition.VendorName))
                    {
                        query.Append(" AND MV.VENDOR_NAME1 LIKE :VENDOR_NAME1 ");
                        parameters.Add(":VENDOR_NAME1", condition.VendorName + "%");
                    }

                    // 分類1
                    if (!string.IsNullOrEmpty(condition.CategoryId1))
                    {
                        query.Append(" AND MIS.CATEGORY_ID1 = :CATEGORY_ID1 ");
                        parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
                    }

                    // 分類2
                    if (!string.IsNullOrEmpty(condition.CategoryId2))
                    {
                        query.Append(" AND MIS.CATEGORY_ID2 = :CATEGORY_ID2 ");
                        parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
                    }

                    // 分類3
                    if (!string.IsNullOrEmpty(condition.CategoryId3))
                    {
                        query.Append(" AND MIS.CATEGORY_ID3 = :CATEGORY_ID3 ");
                        parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
                    }

                    // 分類4
                    if (!string.IsNullOrEmpty(condition.CategoryId4))
                    {
                        query.Append(" AND MIS.CATEGORY_ID4 = :CATEGORY_ID4 ");
                        parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
                    }

                    // 状況
                    if (!string.IsNullOrEmpty(condition.ArrivalStatus))
                    {
                        query.Append(" AND TO_CHAR(TAR.ARRIVAL_STATUS) = :ARRIVAL_STATUS ");
                        parameters.Add(":ARRIVAL_STATUS", condition.ArrivalStatus);
                    }

                    // 納品書番号
                    if (!string.IsNullOrEmpty(condition.InvoiceNo))
                    {
                        query.Append(" AND TAR.INVOICE_NO LIKE :INVOICE_NO ");
                        parameters.Add(":INVOICE_NO", condition.InvoiceNo + "%");
                    }

                    // 発注ID
                    if (!string.IsNullOrEmpty(condition.PoId))
                    {
                        query.Append(" AND TAP.PO_ID LIKE :PO_ID ");
                        parameters.Add(":PO_ID", condition.PoId + "%");
                    }

                    // 品番
                    if (!string.IsNullOrEmpty(condition.ItemId))
                    {
                        query.Append(" AND TAR.ITEM_ID LIKE :ITEM_ID ");
                        parameters.Add(":ITEM_ID", condition.ItemId + "%");
                    }

                    query.AppendLine(@"
                        GROUP BY
                                TAR.INVOICE_NO
                            ,   TAR.CENTER_ID
                            ,   TAR.SHIPPER_ID
                    ");
                    
                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (Exception ex) //デバッグ用
                {
                    Mvc.Common.AppError.PutLogREF(ex, "InsertArrInputPurchase01");
                    trans.Rollback();
                    return false;
                }
                trans.Commit();
            }
            return true;
        }

        /// <summary>
        /// Get UnshelvedReference List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<ConfirmActualResultRow> GetData(ConfirmActualSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            // 在庫明細
            query.Append(@"
                     SELECT *
                       FROM WW_ARR_CON_ACT01
	                  WHERE SHIPPER_ID = :SHIPPER_ID
                        AND SEQ = :SEQ
                ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<ConfirmActualResultRow>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case ArrivalSortKey.ArrivePlanDateVendorIdInvoiceNo:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE DESC,VENDOR_ID DESC,INVOICE_NO DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE ASC,VENDOR_ID ASC,INVOICE_NO ASC ");
                            break;
                    }

                    break;

                case ArrivalSortKey.VendorIdInvoiceNoArrivePlanDate:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY VENDOR_ID DESC,INVOICE_NO DESC,ARRIVE_PLAN_DATE DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY VENDOR_ID ASC,INVOICE_NO ASC,ARRIVE_PLAN_DATE ASC ");
                            break;
                    }

                    break;

                case ArrivalSortKey.ArrivePlanDateItemId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE DESC,ITEM_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE ASC,ITEM_ID ASC ");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY ITEM_ID DESC,ARRIVE_PLAN_DATE DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY ITEM_ID ASC,ARRIVE_PLAN_DATE ASC ");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var ConfirmActual = MvcDbContext.Current.Database.Connection.Query<ConfirmActualResultRow>(query.ToString(), parameters);
            //var arrConAct01s = MvcDbContext.Current.ArrConAct01s.Where(x => x.Seq == condition.Seq && x.ShipperId == Common.Profile.User.ShipperId);
            condition.ItemSkuSum = GetItemSkuSum(condition);
            condition.ArrivePlanQtySum = GetArrivePlanQtySum(condition);
            condition.ResultQtySum = GetResultQtySum(condition);
            condition.ArrivePlanSlipQtySum = GetArrivePlanSlipQtySum(condition);
            condition.ResultSlipQtySum = GetResultSlipQtySum(condition);
            condition.SelectedCnt = MvcDbContext.Current.ArrConAct01s.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId && x.IsCheck).Count();

            // Excute paging
            return new StaticPagedList<ConfirmActualResultRow>(ConfirmActual, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// SKU数取得
        /// </summary>
        /// <returns>SKU数</returns>
        public int GetItemSkuSum(ConfirmActualSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();

            query.Append(@"
                    SELECT
                            SUM(SKU_QTY) ITEM_SKU_SUM
                    FROM
                            WW_ARR_CON_ACT01
                    WHERE
                            SHIPPER_ID = :SHIPPER_ID
                        AND SEQ = :SEQ
                ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // Fill data to memory
            var itemSkuSum = MvcDbContext.Current.Database.Connection.Query<ConfirmActualSum>(query.ToString(), parameters).ToList();
            int itemSku = itemSkuSum[0].ItemSkuSum;

            return itemSku;
        }

        /// <summary>
        /// 予定数合計取得
        /// </summary>
        /// <returns>予定数合計</returns>
        public int GetArrivePlanQtySum(ConfirmActualSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();

            query.Append(@"
                    SELECT
                            SUM(ARRIVE_PLAN_QTY) ARRIVE_PLAN_QTY_SUM
                    FROM
                            WW_ARR_CON_ACT01
                    WHERE
                            SHIPPER_ID = :SHIPPER_ID
                        AND SEQ = :SEQ
                ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // Fill data to memory
            var ArrivePlanQtySum = MvcDbContext.Current.Database.Connection.Query<ConfirmActualSum>(query.ToString(), parameters).ToList();
            int ArrivePlanQty = ArrivePlanQtySum[0].ArrivePlanQtySum;

            return ArrivePlanQty;
        }

        /// <summary>
        /// 実績数合計取得
        /// </summary>
        /// <returns>実績数合計</returns>
        public int GetResultQtySum(ConfirmActualSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();

            query.Append(@"
                    SELECT
                            SUM(RESULT_QTY) RESULT_QTY_SUM
                    FROM
                            WW_ARR_CON_ACT01
                    WHERE
                            SHIPPER_ID = :SHIPPER_ID
                        AND SEQ = :SEQ
                ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // Fill data to memory
            var ResultQtySum = MvcDbContext.Current.Database.Connection.Query<ConfirmActualSum>(query.ToString(), parameters).ToList();
            int ResultQty = ResultQtySum[0].ResultQtySum;

            return ResultQty;
        }

        /// <summary>
        /// 予定伝票数取得
        /// </summary>
        /// <returns>予定伝票数</returns>
        public int GetArrivePlanSlipQtySum(ConfirmActualSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();

            query.Append(@"
                    SELECT
                            COUNT(DISTINCT TAP.INVOICE_NO) ARRIVE_PLAN_SLIP_QTY_SUM
                    FROM
                            WW_ARR_CON_ACT01 WW
                    INNER JOIN
                            T_ARRIVE_PLANS TAP
                    ON
                            WW.SHIPPER_ID = TAP.SHIPPER_ID
                        AND WW.INVOICE_NO = TAP.INVOICE_NO
                    WHERE
                            TAP.SHIPPER_ID = :SHIPPER_ID
                        AND WW.SEQ = :SEQ
                ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // Fill data to memory
            var ArrivePlanSlipQtySum = MvcDbContext.Current.Database.Connection.Query<ConfirmActualSum>(query.ToString(), parameters).ToList();
            int ArrivePlanSlipQty = ArrivePlanSlipQtySum[0].ArrivePlanSlipQtySum;

            return ArrivePlanSlipQty;
        }

        /// <summary>
        /// 実績伝票数取得
        /// </summary>
        /// <returns>実績伝票数</returns>
        public int GetResultSlipQtySum(ConfirmActualSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();

            query.Append(@"
                    SELECT
                            COUNT(DISTINCT TAR.INVOICE_NO) RESULT_SLIP_QTY_SUM
                    FROM
                            WW_ARR_CON_ACT01 WW
                    INNER JOIN
                            T_ARRIVE_RESULTS TAR
                    ON
                            WW.SHIPPER_ID = TAR.SHIPPER_ID
                        AND WW.INVOICE_NO = TAR.INVOICE_NO
                    WHERE
                            TAR.SHIPPER_ID = :SHIPPER_ID
                        AND WW.SEQ = :SEQ
                ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // Fill data to memory
            var ResultSlipQtySum = MvcDbContext.Current.Database.Connection.Query<ConfirmActualSum>(query.ToString(), parameters).ToList();
            int ResultSlipQty = ResultSlipQtySum[0].ResultSlipQtySum;

            return ResultSlipQty;
        }

        /// <summary>
        /// Update Work Table
        /// </summary>
        /// <param name="confirmActual"></param>
        /// <returns></returns>
        public bool UpdateIsCheck(IList<SelectedConfirmActualViewModel> ConfirmActuals)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in ConfirmActuals)
                {
                    // 在庫明細
                    var ConfirmActual = MvcDbContext.Current.ArrConAct01s
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == u.Seq && m.LineNo == u.LineNo)
                                  .SingleOrDefault();

                    if (ConfirmActual == null)
                    {
                        return false;
                    }

                    ConfirmActual.SetBaseInfoUpdate();
                    ConfirmActual.IsCheck = u.IsCheck;
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
        /// Update ConfirmActual 実績解除処理
        /// </summary>
        /// <param name="confirmActual"></param>
        /// <returns>Update status</returns>
        public string UpdateConfirmedArrival(ConfirmActualSearchConditions searchConditions)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", searchConditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_SEQ", searchConditions.Seq, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_SHORI_KBN", 2, DbType.Int32, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute("SP_W_ARR_CONFIRM_ACTUAL", param, commandType: CommandType.StoredProcedure);
            if (param.Get<ProcedureStatus>("OUT_STATUS") != ProcedureStatus.Success)
            {
                return param.Get<string>("OUT_MESSAGE");
            }

            return null;
        }

        /// <summary>
        /// Update ConfirmActual 実績確定処理
        /// </summary>
        /// <param name="confirmActual"></param>
        /// <returns>Update status</returns>
        public string UpdateArrived(ConfirmActualSearchConditions searchConditions)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", searchConditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_SEQ", searchConditions.Seq, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_SHORI_KBN", 1, DbType.Int32, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute("SP_W_ARR_CONFIRM_ACTUAL", param, commandType: CommandType.StoredProcedure);
            if (param.Get<ProcedureStatus>("OUT_STATUS") != ProcedureStatus.Success)
            {
                return param.Get<string>("OUT_MESSAGE");
            }

            return null;
        }

        /// <summary>
        /// Check Work Table
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public bool ArrivalAllChange(ConfirmActualSearchConditions conditions, bool check)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in MvcDbContext.Current.ArrConAct01s.Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq))
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

            conditions.SelectedCnt = MvcDbContext.Current.ArrConAct01s
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq && m.IsCheck).Count();
            return true;
        }

        /// <summary>
        /// 状況データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListArrivalStatus()
        {
            return MvcDbContext.Current.Generals
                .Where(m => m.ShipperId == Profile.User.ShipperId && m.CenterId == "@@@" && m.RegisterDiviCd == "1" && m.GenDivCd == "ARRIVAL_STATUS" && (m.GenCd == "3" || m.GenCd == "4"))
                .Select(m => new SelectListItem
                {
                    Value = m.GenCd.ToString(),
                    Text = m.GenName
                })
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// 事業部データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListDivisions()
        {
            return MvcDbContext.Current.Divisions
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.DivisionId,
                    Text = m.DivisionId + ":" + m.DivisionName
                })
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類1データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys1()
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId1.ToString(),
                    Text = m.CategoryId1.ToString() + ":" + m.CategoryName1
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類2データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys2(string categoryId1 = "")
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId2.ToString(),
                    Text = m.CategoryId2.ToString() + ":" + m.CategoryName2
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類3データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys3(string categoryId1 = "", string categoryId2 = "")
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId
                && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1)
                && (categoryId2 == null ? 1 == 1 : m.CategoryId2 == categoryId2))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId3.ToString(),
                    Text = m.CategoryId3.ToString() + ":" + m.CategoryName3
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類4データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys4(string categoryId1 = "", string categoryId2 = "", string categoryId3 = "")
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId
                && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1)
                && (categoryId2 == null ? 1 == 1 : m.CategoryId2 == categoryId2)
                && (categoryId3 == null ? 1 == 1 : m.CategoryId3 == categoryId3))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId4.ToString(),
                    Text = m.CategoryId4.ToString() + ":" + m.CategoryName4
                })
                .Distinct().OrderBy(m => m.Value);
        }

    }
}