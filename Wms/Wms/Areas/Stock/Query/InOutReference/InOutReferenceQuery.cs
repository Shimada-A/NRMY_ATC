using Dapper;
using PagedList;
using Share.Extensions.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Wms.Areas.Stock.Resources;
using Wms.Areas.Stock.ViewModels.InOutReference;
using Wms.Common;
using Wms.Models;

namespace Wms.Areas.Stock.Query.InOutReference
{
    public class InOutReferenceQuery
    {
        #region ロケーション区分データ取得
        /// <summary>
        /// ロケーション区分データ取得
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetSelectListLocationClasses()
        {
            return MvcDbContext.Current.LocationClasses
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.LocationClass,
                    Text = m.LocationName
                })
                .Distinct()
                .OrderBy(m => m.Value);
        }
        #endregion ロケーション区分データ取得

        #region 画面表示データ検索
        /// <summary>
        /// 画面表示データ（在庫明細）取得
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public IPagedList<InOutReferenceResultRow> GetStockData(InOutReferenceSearchConditions conditions)
        {
            var sql = $@"
                SELECT 
                        STK.CENTER_ID
                    ,   STK.INS_DATE AS MOVE_DATE
                    ,   STK.OPERATION_ID
                    ,   STK.UPDATE_PROGRAM_NAME
                    ,   STK.OPERATION_CLASS
                    ,   STK.UPDATE_USER_ID AS OPERATION_USER_ID
                    ,   MUS.USER_NAME AS OPERATION_USER_NAME
                    ,   STK.LOCATION_CD AS LOCATION_CODE
                    ,   MLOC.LOCATION_CLASS
                    ,   MLOCCL.LOCATION_NAME AS LOCATION_CLASS_NAME
                    ,   MSKU.CATEGORY_ID1
                    ,   MCATE1.CATEGORY_NAME1
                    ,   STK.ITEM_ID
                    ,   MSKU.ITEM_NAME
                    ,   STK.ITEM_COLOR_ID
                    ,   MCOL.ITEM_COLOR_NAME
                    ,   STK.ITEM_SIZE_ID
                    ,   MSKU.ITEM_SIZE_NAME
                    ,   STK.JAN
                    ,   STK.GRADE_ID
                    ,   MGRA.GRADE_NAME
                    ,   STK.STOCK_QTY
                    ,   STK.ALLOC_QTY
                    ,   STK.UPDATE_COUNT
                FROM
                        TL_STOCKS STK
                LEFT OUTER JOIN
                        M_USERS MUS
                ON
                        MUS.USER_ID = STK.UPDATE_USER_ID
                    AND MUS.SHIPPER_ID = STK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_LOCATIONS MLOC
                ON
                        MLOC.LOCATION_CD = STK.LOCATION_CD
                    AND MLOC.CENTER_ID = STK.CENTER_ID
                    AND MLOC.SHIPPER_ID = STK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_LOCATION_CLASSES MLOCCL
                ON
                        MLOCCL.LOCATION_CLASS = MLOC.LOCATION_CLASS
                    AND MLOCCL.SHIPPER_ID = MLOC.SHIPPER_ID
                LEFT OUTER JOIN
                        M_ITEM_SKU MSKU
                ON
                        MSKU.ITEM_SKU_ID = STK.ITEM_SKU_ID
                    AND MSKU.SHIPPER_ID = STK.SHIPPER_ID
                LEFT OUTER JOIN (
                    SELECT 
                            SHIPPER_ID
                        ,   CATEGORY_ID1
                        ,   MAX(CATEGORY_NAME1) AS CATEGORY_NAME1
                    FROM
                            M_ITEM_CATEGORIES4
                    WHERE
                            SHIPPER_ID = :SHIPPER_ID
                    GROUP BY 
                            SHIPPER_ID
                        ,   CATEGORY_ID1
                ) MCATE1
                ON
                        MCATE1.CATEGORY_ID1 = MSKU.CATEGORY_ID1
                    AND MCATE1.SHIPPER_ID = MSKU.SHIPPER_ID
                LEFT OUTER JOIN
                        M_COLORS MCOL
                ON
                        MCOL.ITEM_COLOR_ID = STK.ITEM_COLOR_ID
                    AND MCOL.SHIPPER_ID = STK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_SIZES MSIZ
                ON
                        MSIZ.ITEM_SIZE_ID = STK.ITEM_SIZE_ID
                    AND MSIZ.SHIPPER_ID = STK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_GRADES MGRA
                ON
                        MGRA.GRADE_ID = STK.GRADE_ID
                    AND MGRA.SHIPPER_ID = STK.SHIPPER_ID
                WHERE
                        STK.SHIPPER_ID = :SHIPPER_ID
                        {GetSqlConditions(conditions)}
                ORDER BY
                        {GetSqlStockOrder(conditions)}
            ";

            var parameters = new DynamicParameters();

            parameters.AddDynamicParams(
                new
                {
                    SHIPPER_ID = Profile.User.ShipperId,
                    CENTER_ID = conditions.CenterId,
                    MOVE_DATE_FROM = conditions.MoveDateFrom,
                    MOVE_DATE_TO = conditions.MoveDateTo,
                    JAN = conditions.Jan,
                    LOCATION_CLASS = conditions.LocationClass,
                    ITEM_ID = conditions.ItemId,
                    LOCATION_CD = conditions.LocationCode,
                    ITEM_SKU_ID = conditions.ItemSkuId,
                    BOX_NO = conditions.BoxNo
                });

            var totalCount = 0;
            var details = MvcDbContext.Current.Database.Connection.FetchWithRecordCountQuery<InOutReferenceResultRow>(sql, parameters, conditions.PageNumber, conditions.PageSize, out totalCount).ToList();

            foreach (var detail in details)
            {
                string operationName = string.Empty;

                switch (detail.UpdateProgramName)
                {
                    case "SP_W_SHP_MODIFYTCINSTRUCTION":
                        operationName = "TC出荷指示修正";
                        break;
                    case "SP_W_SHP_CASEALLOC_CANCEL":
                        operationName = "ケース出荷引当解除";
                        break;
                    case "SP_W_SHP_CASE_INSTRUCTION":
                        operationName = "ケース出荷指示取込";
                        break;
                    case "SP_W_SHP_AUTO_TCALLOC":
                        operationName = "TC自動引当";
                        break;
                    case "SP_W_RTN_STOCK_REG":
                        operationName = "在庫登録処理(EC返品受付)";
                        break;
                    case "SP_W_INV_CONFIRM":
                        operationName = "棚卸開始";
                        break;
                    case "SP_H_STK_SORT_REG":
                        operationName = "在庫仕分実績登録";
                        break;
                    case "SP_DAILY_STOCK_WITHDRAWAL":
                        operationName = "日次処理(在庫引落し)";
                        break;
                    case "PK_W_MOV_INPUTTRANSFER02.UPDATE_RESULT":
                        operationName = "移動入荷実績更新";
                        break;
                    case "PK_W_H_ARR_SORT.UpdateSortStatusFromWork":
                        operationName = "仕分入荷計上";
                        break;
                    case "PK_W_H_ARR_SORT.SortTCDC":
                        operationName = "仕分入荷計上";
                        break;
                    case "PK_W_H_ARR_SORT.FinishDeliverySku":
                        operationName = "仕分入荷計上";
                        break;
                    case "PK_W_H_ARR_SORT.NumberingBoxNum":
                        operationName = "仕分入荷計上";
                        break;
                    case "PK_W_ARR_INPUTPURCHASE02.MERGE_PACKAGE_STOCK":
                        operationName = "入荷実績入力";
                        break;
                    case "PK_W_ARR_INPUTPURCHASE02.UPDATE_RESULT":
                        operationName = "入荷実績入力";
                        break;
                    case "PK_STK_ADJUSTMENT.SP_MERGE_PACKAGE_STOCKS":
                        operationName = "在庫調整";
                        break;
                    case "PK_LOC_MOVE_ASSORTPIC.PicAssortBoxUpdateStock":
                        operationName = "ピック完了登録";
                        break;
                    case "PK_LOC_MOVE_ASSORTPIC.PicAssortSkuUpdateStock":
                        operationName = "ピック完了登録";
                        break;
                    case "PK_LOC_MOVE.UpdateStock":
                        operationName = "在庫移動";
                        break;
                    case "PK_H_STOCK_SORTS.SET_RESULT":
                        operationName = "仕分先ケース在庫移動";
                        break;
                    case "PK_H_MOV_TRANSFERINSPECTION.SET_RESULT":
                        operationName = "拠点間移動入荷検品";
                        break;
                    case "PK_H_MOV_SIMPLEINSPECTION.SET_RESULT":
                        operationName = "外装検品";
                        break;
                    case "PK_H_MOV_COM_SORT_INSPECTION.SET_RESULT":
                        operationName = "強制仕分検品";
                        break;
                    case "PK_H_MOV_SORTINSPECTION.SET_RESULT":
                        operationName = "仕分検品";
                        break;
                    case "PK_H_MOV_ALLINSPECTION.SET_RESULT":
                        operationName = "全検品";
                        break;
                    case "PK_H_MOV_COM_ALL_INSPECTION.SET_RESULT":
                        operationName = "強制全検品";
                        break;
                    case "PK_H_CREATE_CASE.CreateBox":
                        operationName = "ケース作成";
                        break;
                    case "SP_W_RET_PURCHASE_CORRECTION_UPD":
                        operationName = "仕入訂正確定";
                        break;
                    case "SP_W_RET_PURCHASE_RETURNS_UPD":
                        operationName = "";
                        break;
                    case "仕入返品確定":
                        operationName = "";
                        break;
                    case "SP_W_SHP_TCALLOC_SUB":
                        operationName = "TC引当";
                        break;
                    case "SP_W_SHP_ECALLOC_SAVE_CANCEL":
                        operationName = "EC引当保持分解除";
                        break;
                    case "SP_W_SHP_ECALLOC_SAVE_CAN_SUB":
                        operationName = "EC引当保持分解除";
                        break;
                    case "SP_W_SHP_ECALLOC_MAIN":
                        operationName = "EC引当";
                        break;
                    case "SP_W_SHP_ECALLOC_CANCEL":
                        operationName = "EC引当解除";
                        break;
                    case "SP_W_SHP_DCALLOC_MAIN":
                        operationName = "DC引当";
                        break;
                    case "SP_W_SHP_DCALLOC_CANCEL":
                        operationName = "DC引当解除";
                        break;
                    case "PK_W_ARR_INPUTPURCHASE02.MERGE_STOCK":
                        operationName = "入荷実績入力";
                        break;
                    case "PK_STK_ADJUSTMENT.SP_MERGE_STOCKS":
                        operationName = "在庫調整";
                        break;
                    case "SP_W_MOV_INPUTTRANSFER01":
                        operationName = "移動入荷実績入力";
                        break;
                    case "SP_H_SHP_DIRECT_ALLOC_HOKAN":
                        operationName = "直接出荷引当";
                        break;
                    case "SP_IFRMK_ARRIVE_RESULTS.SP_MERGE_T_STOCKS":
                        operationName = "RFID入荷実績計上";
                        break;
                    case "SP_IFRMK_TRANSFER_RESULTS.SP_MERGE_T_STOCKS":
                        operationName = "RFID移動入荷実績計上";
                        break;
                    case "SP_H_SHP_DIRECT_ALLOC":
                        operationName = "直接出荷引当";
                        break;

                    default:
                        operationName = "";
                        break;
                }

                detail.OperationName = operationName;
            }

            return new StaticPagedList<InOutReferenceResultRow>(details, conditions.PageNumber, conditions.PageSize, totalCount);
        }

        /// <summary>
        /// 画面表示データ（ケース明細）取得
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public IPagedList<InOutReferenceResultRow> GetPackageStockData(InOutReferenceSearchConditions conditions)
        {
            var sql = $@"
                SELECT 
                        STK.CENTER_ID
                    ,   STK.INS_DATE AS MOVE_DATE
                    ,   STK.OPERATION_ID
                    ,   STK.UPDATE_PROGRAM_NAME
                    ,   STK.OPERATION_CLASS
                    ,   STK.UPDATE_USER_ID AS OPERATION_USER_ID
                    ,   MUS.USER_NAME AS OPERATION_USER_NAME
                    ,   STK.LOCATION_CD AS LOCATION_CODE
                    ,   MLOC.LOCATION_CLASS
                    ,   MLOCCL.LOCATION_NAME AS LOCATION_CLASS_NAME
                    ,   STK.BOX_NO
                    ,   MSKU.CATEGORY_ID1
                    ,   MCATE1.CATEGORY_NAME1
                    ,   STK.ITEM_ID
                    ,   MSKU.ITEM_NAME
                    ,   STK.ITEM_COLOR_ID
                    ,   MCOL.ITEM_COLOR_NAME
                    ,   STK.ITEM_SIZE_ID
                    ,   MSKU.ITEM_SIZE_NAME
                    ,   STK.JAN
                    ,   STK.GRADE_ID
                    ,   MGRA.GRADE_NAME
                    ,   STK.INVOICE_NO
                    ,   STK.STOCK_QTY
                    ,   STK.ALLOC_QTY
                    ,   STK.UPDATE_COUNT
                    ,   STK.SORT_STATUS
                FROM
                        TL_PACKAGE_STOCKS STK
                LEFT OUTER JOIN
                        M_USERS MUS
                ON
                        MUS.USER_ID = STK.UPDATE_USER_ID
                    AND MUS.SHIPPER_ID = STK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_LOCATIONS MLOC
                ON
                        MLOC.LOCATION_CD = STK.LOCATION_CD
                    AND MLOC.CENTER_ID = STK.CENTER_ID
                    AND MLOC.SHIPPER_ID = STK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_LOCATION_CLASSES MLOCCL
                ON
                        MLOCCL.LOCATION_CLASS = MLOC.LOCATION_CLASS
                    AND MLOCCL.SHIPPER_ID = MLOC.SHIPPER_ID
                LEFT OUTER JOIN
                        M_ITEM_SKU MSKU
                ON
                        MSKU.ITEM_SKU_ID = STK.ITEM_SKU_ID
                    AND MSKU.SHIPPER_ID = STK.SHIPPER_ID
                LEFT OUTER JOIN (
                    SELECT 
                            SHIPPER_ID
                        ,   CATEGORY_ID1
                        ,   MAX(CATEGORY_NAME1) AS CATEGORY_NAME1
                    FROM
                            M_ITEM_CATEGORIES4
                    WHERE
                            SHIPPER_ID = :SHIPPER_ID
                    GROUP BY 
                            SHIPPER_ID
                        ,   CATEGORY_ID1
                ) MCATE1
                ON
                        MCATE1.CATEGORY_ID1 = MSKU.CATEGORY_ID1
                    AND MCATE1.SHIPPER_ID = MSKU.SHIPPER_ID
                LEFT OUTER JOIN
                        M_COLORS MCOL
                ON
                        MCOL.ITEM_COLOR_ID = STK.ITEM_COLOR_ID
                    AND MCOL.SHIPPER_ID = STK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_SIZES MSIZ
                ON
                        MSIZ.ITEM_SIZE_ID = STK.ITEM_SIZE_ID
                    AND MSIZ.SHIPPER_ID = STK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_GRADES MGRA
                ON
                        MGRA.GRADE_ID = STK.GRADE_ID
                    AND MGRA.SHIPPER_ID = STK.SHIPPER_ID
                WHERE
                        STK.SHIPPER_ID = :SHIPPER_ID
                        {GetSqlConditions(conditions)}
                ORDER BY
                        {GetSqlPackageStockOrder(conditions)}
            ";

            var parameters = new DynamicParameters();

            parameters.AddDynamicParams(
                new
                {
                    SHIPPER_ID = Profile.User.ShipperId,
                    CENTER_ID = conditions.CenterId,
                    MOVE_DATE_FROM = conditions.MoveDateFrom,
                    MOVE_DATE_TO = conditions.MoveDateTo,
                    JAN = conditions.Jan,
                    LOCATION_CLASS = conditions.LocationClass,
                    ITEM_ID = conditions.ItemId,
                    LOCATION_CD = conditions.LocationCode,
                    ITEM_SKU_ID = conditions.ItemSkuId,
                    BOX_NO = conditions.BoxNo
                });

            var totalCount = 0;
            var details = MvcDbContext.Current.Database.Connection.FetchWithRecordCountQuery<InOutReferenceResultRow>(sql, parameters, conditions.PageNumber, conditions.PageSize, out totalCount).ToList();

            foreach (var detail in details)
            {
                string operationName = string.Empty;

                switch (detail.UpdateProgramName)
                {
                    case "SP_W_SHP_MODIFYTCINSTRUCTION":
                        operationName = "TC出荷指示修正";
                        break;
                    case "SP_W_SHP_CASEALLOC_CANCEL":
                        operationName = "ケース出荷引当解除";
                        break;
                    case "SP_W_SHP_CASE_INSTRUCTION":
                        operationName = "ケース出荷指示取込";
                        break;
                    case "SP_W_SHP_AUTO_TCALLOC":
                        operationName = "TC自動引当";
                        break;
                    case "SP_W_RTN_STOCK_REG":
                        operationName = "在庫登録処理(EC返品受付)";
                        break;
                    case "SP_W_INV_CONFIRM":
                        operationName = "棚卸開始";
                        break;
                    case "SP_H_STK_SORT_REG":
                        operationName = "在庫仕分実績登録";
                        break;
                    case "SP_DAILY_STOCK_WITHDRAWAL":
                        operationName = "日次処理(在庫引落し)";
                        break;
                    case "PK_W_MOV_INPUTTRANSFER02.UPDATE_RESULT":
                        operationName = "移動入荷実績更新";
                        break;
                    case "PK_W_H_ARR_SORT.UpdateSortStatusFromWork":
                        operationName = "仕分入荷計上";
                        break;
                    case "PK_W_H_ARR_SORT.SortTCDC":
                        operationName = "仕分入荷計上";
                        break;
                    case "PK_W_H_ARR_SORT.FinishDeliverySku":
                        operationName = "仕分入荷計上";
                        break;
                    case "PK_W_H_ARR_SORT.NumberingBoxNum":
                        operationName = "仕分入荷計上";
                        break;
                    case "PK_W_ARR_INPUTPURCHASE02.MERGE_PACKAGE_STOCK":
                        operationName = "入荷実績入力";
                        break;
                    case "PK_W_ARR_INPUTPURCHASE02.UPDATE_RESULT":
                        operationName = "入荷実績入力";
                        break;
                    case "PK_STK_ADJUSTMENT.SP_MERGE_PACKAGE_STOCKS":
                        operationName = "在庫調整";
                        break;
                    case "PK_LOC_MOVE_ASSORTPIC.PicAssortBoxUpdateStock":
                        operationName = "ピック完了登録";
                        break;
                    case "PK_LOC_MOVE_ASSORTPIC.PicAssortSkuUpdateStock":
                        operationName = "ピック完了登録";
                        break;
                    case "PK_LOC_MOVE.UpdateStock":
                        operationName = "在庫移動";
                        break;
                    case "PK_H_STOCK_SORTS.SET_RESULT":
                        operationName = "仕分先ケース在庫移動";
                        break;
                    case "PK_H_MOV_TRANSFERINSPECTION.SET_RESULT":
                        operationName = "拠点間移動入荷検品";
                        break;
                    case "PK_H_MOV_SIMPLEINSPECTION.SET_RESULT":
                        operationName = "外装検品";
                        break;
                    case "PK_H_MOV_COM_SORT_INSPECTION.SET_RESULT":
                        operationName = "強制仕分検品";
                        break;
                    case "PK_H_MOV_SORTINSPECTION.SET_RESULT":
                        operationName = "仕分検品";
                        break;
                    case "PK_H_MOV_ALLINSPECTION.SET_RESULT":
                        operationName = "全検品";
                        break;
                    case "PK_H_MOV_COM_ALL_INSPECTION.SET_RESULT":
                        operationName = "強制全検品";
                        break;
                    case "PK_H_CREATE_CASE.CreateBox":
                        operationName = "ケース作成";
                        break;
                    case "SP_W_RET_PURCHASE_CORRECTION_UPD":
                        operationName = "仕入訂正確定";
                        break;
                    case "SP_W_RET_PURCHASE_RETURNS_UPD":
                        operationName = "";
                        break;
                    case "仕入返品確定":
                        operationName = "";
                        break;
                    case "SP_W_SHP_TCALLOC_SUB":
                        operationName = "TC引当";
                        break;
                    case "SP_W_SHP_ECALLOC_SAVE_CANCEL":
                        operationName = "EC引当保持分解除";
                        break;
                    case "SP_W_SHP_ECALLOC_SAVE_CAN_SUB":
                        operationName = "EC引当保持分解除";
                        break;
                    case "SP_W_SHP_ECALLOC_MAIN":
                        operationName = "EC引当";
                        break;
                    case "SP_W_SHP_ECALLOC_CANCEL":
                        operationName = "EC引当解除";
                        break;
                    case "SP_W_SHP_DCALLOC_MAIN":
                        operationName = "DC引当";
                        break;
                    case "SP_W_SHP_DCALLOC_CANCEL":
                        operationName = "DC引当解除";
                        break;
                    case "PK_W_ARR_INPUTPURCHASE02.MERGE_STOCK":
                        operationName = "入荷実績入力";
                        break;
                    case "PK_STK_ADJUSTMENT.SP_MERGE_STOCKS":
                        operationName = "在庫調整";
                        break;
                    default:
                        operationName = "";
                        break;
                }

                detail.OperationName = operationName;
            }

            return new StaticPagedList<InOutReferenceResultRow>(details, conditions.PageNumber, conditions.PageSize, totalCount);
        }

        /// <summary>
        /// Where句を取得
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        private string GetSqlConditions(InOutReferenceSearchConditions conditions)
        {
            var sql = new StringBuilder();

            if (!string.IsNullOrEmpty(conditions.CenterId))
            {
                sql.AppendLine(" AND STK.CENTER_ID = :CENTER_ID ");
            }

            if (conditions.MoveDateFrom != null)
            {
                sql.AppendLine(" AND TRUNC(STK.INS_DATE) >= :MOVE_DATE_FROM ");
            }

            if (conditions.MoveDateTo != null)
            {
                sql.AppendLine(" AND TRUNC(STK.INS_DATE) <= :MOVE_DATE_TO ");
            }

            if (!string.IsNullOrEmpty(conditions.Jan))
            {
                sql.AppendLine(" AND STK.JAN LIKE :JAN || '%' ");
            }

            if (!string.IsNullOrEmpty(conditions.LocationClass))
            {
                sql.AppendLine(" AND MLOC.LOCATION_CLASS = :LOCATION_CLASS ");
            }

            if (!string.IsNullOrEmpty(conditions.ItemId))
            {
                sql.AppendLine(" AND STK.ITEM_ID LIKE :ITEM_ID || '%' ");
            }

            if (!string.IsNullOrEmpty(conditions.LocationCode))
            {
                sql.AppendLine(" AND STK.LOCATION_CD = :LOCATION_CD ");
            }

            if (!string.IsNullOrEmpty(conditions.ItemSkuId))
            {
                sql.AppendLine(" AND STK.ITEM_SKU_ID LIKE :ITEM_SKU_ID || '%' ");
            }

            if (!string.IsNullOrEmpty(conditions.BoxNo))
            {
                sql.AppendLine(" AND STK.BOX_NO = :BOX_NO ");
            }

            if (conditions.NotZeroDisp)
            {
                sql.AppendLine(" AND (      STK.STOCK_QTY <> 0");
                sql.AppendLine("        OR  STK.ALLOC_QTY <> 0 ");
                sql.AppendLine("        OR  STK.OPERATION_CLASS <> 2 )");
            }
            return sql.ToString();
        }

        /// <summary>
        /// OrderBy句を取得（在庫明細）
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        private string GetSqlStockOrder(InOutReferenceSearchConditions conditions)
        {
            var sort = (conditions.Sort == InOutReferenceSearchConditions.AscDescSort.Desc) ? "DESC" : "ASC";

            var sql = new StringBuilder();

            switch (conditions.SortKey)
            {
                case InOutReferenceSearchConditions.StockSortKey.Location:
                    sql.AppendLine($"     STK.LOCATION_CD {sort} ");
                    sql.AppendLine($" ,   STK.ITEM_SKU_ID {sort} ");
                    sql.AppendLine($" ,   STK.OPERATION_ID {sort} ");
                    break;

                case InOutReferenceSearchConditions.StockSortKey.ItemSku:
                    sql.AppendLine($"     STK.ITEM_SKU_ID {sort} ");
                    sql.AppendLine($" ,   STK.LOCATION_CD {sort} ");
                    sql.AppendLine($" ,   STK.OPERATION_ID {sort} ");
                    break;

                case InOutReferenceSearchConditions.StockSortKey.Operation:
                    sql.AppendLine($"     STK.OPERATION_ID {sort} ");
                    break;

                default:
                    sql.AppendLine($"     STK.LOCATION_CD {sort} ");
                    sql.AppendLine($" ,   STK.ITEM_SKU_ID {sort} ");
                    sql.AppendLine($" ,   STK.OPERATION_ID {sort} ");
                    break;
            }

            return sql.ToString();
        }

        /// <summary>
        /// OrderBy句を取得（ケース明細）
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        private string GetSqlPackageStockOrder(InOutReferenceSearchConditions conditions)
        {
            var sort = (conditions.Sort == InOutReferenceSearchConditions.AscDescSort.Desc) ? "DESC" : "ASC";

            var sql = new StringBuilder();

            switch (conditions.PackageSortKey)
            {
                case InOutReferenceSearchConditions.PackageStockSortKey.Location:
                    sql.AppendLine($"     STK.LOCATION_CD {sort} ");
                    sql.AppendLine($" ,   STK.ITEM_SKU_ID {sort} ");
                    sql.AppendLine($" ,   STK.BOX_NO {sort} ");
                    sql.AppendLine($" ,   STK.OPERATION_ID {sort} ");
                    break;

                case InOutReferenceSearchConditions.PackageStockSortKey.ItemSku:
                    sql.AppendLine($"     STK.ITEM_SKU_ID {sort} ");
                    sql.AppendLine($" ,   STK.BOX_NO {sort} ");
                    sql.AppendLine($" ,   STK.LOCATION_CD {sort} ");
                    sql.AppendLine($" ,   STK.OPERATION_ID {sort} ");
                    break;

                case InOutReferenceSearchConditions.PackageStockSortKey.Box:
                    sql.AppendLine($"     STK.BOX_NO {sort} ");
                    sql.AppendLine($" ,   STK.ITEM_SKU_ID {sort} ");
                    sql.AppendLine($" ,   STK.LOCATION_CD {sort} ");
                    sql.AppendLine($" ,   STK.OPERATION_ID {sort} ");
                    break;

                case InOutReferenceSearchConditions.PackageStockSortKey.Operation:
                    sql.AppendLine($"     STK.OPERATION_ID {sort} ");
                    break;

                default:
                    sql.AppendLine($"     STK.LOCATION_CD {sort} ");
                    sql.AppendLine($" ,   STK.ITEM_SKU_ID {sort} ");
                    sql.AppendLine($" ,   STK.BOX_NO {sort} ");
                    sql.AppendLine($" ,   STK.OPERATION_ID {sort} ");
                    break;
            }

            return sql.ToString();
        }
        #endregion 画面表示データ検索

        #region ダウンロードデータ検索
        /// <summary>
        /// ダウンロードデータ（在庫明細）を取得
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public IEnumerable<InOutReferenceStockReportRow> GetStockReportData(InOutReferenceSearchConditions conditions)
        {
            var sql = $@"
                SELECT 
                        STK.CENTER_ID
                    ,   STK.INS_DATE AS MOVE_DATE
                    ,   STK.OPERATION_ID
                    ,   STK.UPDATE_PROGRAM_NAME AS OPERATION_NAME
                    ,   STK.OPERATION_CLASS
                    ,   STK.UPDATE_USER_ID AS OPERATION_USER_ID
                    ,   MUS.USER_NAME AS OPERATION_USER_NAME
                    ,   STK.LOCATION_CD AS LOCATION_CODE
                    ,   MLOC.LOCATION_CLASS
                    ,   MLOCCL.LOCATION_NAME AS LOCATION_CLASS_NAME
                    ,   MSKU.CATEGORY_ID1
                    ,   MCATE1.CATEGORY_NAME1
                    ,   STK.ITEM_ID
                    ,   MSKU.ITEM_NAME
                    ,   STK.ITEM_COLOR_ID
                    ,   MCOL.ITEM_COLOR_NAME
                    ,   STK.ITEM_SIZE_ID
                    ,   MSKU.ITEM_SIZE_NAME
                    ,   STK.JAN
                    ,   STK.GRADE_ID
                    ,   MGRA.GRADE_NAME
                    ,   STK.STOCK_QTY
                    ,   STK.ALLOC_QTY
                    ,   STK.UPDATE_COUNT
                FROM
                        TL_STOCKS STK
                LEFT OUTER JOIN
                        M_USERS MUS
                ON
                        MUS.USER_ID = STK.UPDATE_USER_ID
                    AND MUS.SHIPPER_ID = STK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_LOCATIONS MLOC
                ON
                        MLOC.LOCATION_CD = STK.LOCATION_CD
                    AND MLOC.CENTER_ID = STK.CENTER_ID
                    AND MLOC.SHIPPER_ID = STK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_LOCATION_CLASSES MLOCCL
                ON
                        MLOCCL.LOCATION_CLASS = MLOC.LOCATION_CLASS
                    AND MLOCCL.SHIPPER_ID = MLOC.SHIPPER_ID
                LEFT OUTER JOIN
                        M_ITEM_SKU MSKU
                ON
                        MSKU.ITEM_SKU_ID = STK.ITEM_SKU_ID
                    AND MSKU.SHIPPER_ID = STK.SHIPPER_ID
                LEFT OUTER JOIN (
                    SELECT 
                            SHIPPER_ID
                        ,   CATEGORY_ID1
                        ,   MAX(CATEGORY_NAME1) AS CATEGORY_NAME1
                    FROM
                            M_ITEM_CATEGORIES4
                    WHERE
                            SHIPPER_ID = :SHIPPER_ID
                    GROUP BY 
                            SHIPPER_ID
                        ,   CATEGORY_ID1
                ) MCATE1
                ON
                        MCATE1.CATEGORY_ID1 = MSKU.CATEGORY_ID1
                    AND MCATE1.SHIPPER_ID = MSKU.SHIPPER_ID
                LEFT OUTER JOIN
                        M_COLORS MCOL
                ON
                        MCOL.ITEM_COLOR_ID = STK.ITEM_COLOR_ID
                    AND MCOL.SHIPPER_ID = STK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_SIZES MSIZ
                ON
                        MSIZ.ITEM_SIZE_ID = STK.ITEM_SIZE_ID
                    AND MSIZ.SHIPPER_ID = STK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_GRADES MGRA
                ON
                        MGRA.GRADE_ID = STK.GRADE_ID
                    AND MGRA.SHIPPER_ID = STK.SHIPPER_ID
                WHERE
                        STK.SHIPPER_ID = :SHIPPER_ID
                        {GetSqlConditions(conditions)}
                ORDER BY
                        {GetSqlStockOrder(conditions)}
            ";

            var parameters = new DynamicParameters();

            parameters.AddDynamicParams(
                new
                {
                    SHIPPER_ID = Profile.User.ShipperId,
                    CENTER_ID = conditions.CenterId,
                    MOVE_DATE_FROM = conditions.MoveDateFrom,
                    MOVE_DATE_TO = conditions.MoveDateTo,
                    JAN = conditions.Jan,
                    LOCATION_CLASS = conditions.LocationClass,
                    ITEM_ID = conditions.ItemId,
                    LOCATION_CD = conditions.LocationCode,
                    ITEM_SKU_ID = conditions.ItemSkuId,
                    BOX_NO = conditions.BoxNo
                });

            var details = MvcDbContext.Current.Database.Connection.Query<InOutReferenceStockReportRow>(sql, parameters).ToList();

            foreach (var detail in details)
            {
                string operationName = string.Empty;

                switch (detail.OperationName)
                {
                    case "SP_W_SHP_MODIFYTCINSTRUCTION":
                        operationName = "TC出荷指示修正";
                        break;
                    case "SP_W_SHP_CASEALLOC_CANCEL":
                        operationName = "ケース出荷引当解除";
                        break;
                    case "SP_W_SHP_CASE_INSTRUCTION":
                        operationName = "ケース出荷指示取込";
                        break;
                    case "SP_W_SHP_AUTO_TCALLOC":
                        operationName = "TC自動引当";
                        break;
                    case "SP_W_RTN_STOCK_REG":
                        operationName = "在庫登録処理(EC返品受付)";
                        break;
                    case "SP_W_INV_CONFIRM":
                        operationName = "棚卸開始";
                        break;
                    case "SP_H_STK_SORT_REG":
                        operationName = "在庫仕分実績登録";
                        break;
                    case "SP_DAILY_STOCK_WITHDRAWAL":
                        operationName = "日次処理(在庫引落し)";
                        break;
                    case "PK_W_MOV_INPUTTRANSFER02.UPDATE_RESULT":
                        operationName = "移動入荷実績更新";
                        break;
                    case "PK_W_H_ARR_SORT.UpdateSortStatusFromWork":
                        operationName = "仕分入荷計上";
                        break;
                    case "PK_W_H_ARR_SORT.SortTCDC":
                        operationName = "仕分入荷計上";
                        break;
                    case "PK_W_H_ARR_SORT.FinishDeliverySku":
                        operationName = "仕分入荷計上";
                        break;
                    case "PK_W_H_ARR_SORT.NumberingBoxNum":
                        operationName = "仕分入荷計上";
                        break;
                    case "PK_W_ARR_INPUTPURCHASE02.MERGE_PACKAGE_STOCK":
                        operationName = "入荷実績入力";
                        break;
                    case "PK_W_ARR_INPUTPURCHASE02.UPDATE_RESULT":
                        operationName = "入荷実績入力";
                        break;
                    case "PK_STK_ADJUSTMENT.SP_MERGE_PACKAGE_STOCKS":
                        operationName = "在庫調整";
                        break;
                    case "PK_LOC_MOVE_ASSORTPIC.PicAssortBoxUpdateStock":
                        operationName = "ピック完了登録";
                        break;
                    case "PK_LOC_MOVE_ASSORTPIC.PicAssortSkuUpdateStock":
                        operationName = "ピック完了登録";
                        break;
                    case "PK_LOC_MOVE.UpdateStock":
                        operationName = "在庫移動";
                        break;
                    case "PK_H_STOCK_SORTS.SET_RESULT":
                        operationName = "仕分先ケース在庫移動";
                        break;
                    case "PK_H_MOV_TRANSFERINSPECTION.SET_RESULT":
                        operationName = "拠点間移動入荷検品";
                        break;
                    case "PK_H_MOV_SIMPLEINSPECTION.SET_RESULT":
                        operationName = "外装検品";
                        break;
                    case "PK_H_MOV_COM_SORT_INSPECTION.SET_RESULT":
                        operationName = "強制仕分検品";
                        break;
                    case "PK_H_MOV_SORTINSPECTION.SET_RESULT":
                        operationName = "仕分検品";
                        break;
                    case "PK_H_MOV_ALLINSPECTION.SET_RESULT":
                        operationName = "全検品";
                        break;
                    case "PK_H_MOV_COM_ALL_INSPECTION.SET_RESULT":
                        operationName = "強制全検品";
                        break;
                    case "PK_H_CREATE_CASE.CreateBox":
                        operationName = "ケース作成";
                        break;
                    case "SP_W_RET_PURCHASE_CORRECTION_UPD":
                        operationName = "仕入訂正確定";
                        break;
                    case "SP_W_RET_PURCHASE_RETURNS_UPD":
                        operationName = "";
                        break;
                    case "仕入返品確定":
                        operationName = "";
                        break;
                    case "SP_W_SHP_TCALLOC_SUB":
                        operationName = "TC引当";
                        break;
                    case "SP_W_SHP_ECALLOC_SAVE_CANCEL":
                        operationName = "EC引当保持分解除";
                        break;
                    case "SP_W_SHP_ECALLOC_SAVE_CAN_SUB":
                        operationName = "EC引当保持分解除";
                        break;
                    case "SP_W_SHP_ECALLOC_MAIN":
                        operationName = "EC引当";
                        break;
                    case "SP_W_SHP_ECALLOC_CANCEL":
                        operationName = "EC引当解除";
                        break;
                    case "SP_W_SHP_DCALLOC_MAIN":
                        operationName = "DC引当";
                        break;
                    case "SP_W_SHP_DCALLOC_CANCEL":
                        operationName = "DC引当解除";
                        break;
                    case "PK_W_ARR_INPUTPURCHASE02.MERGE_STOCK":
                        operationName = "入荷実績入力";
                        break;
                    case "PK_STK_ADJUSTMENT.SP_MERGE_STOCKS":
                        operationName = "在庫調整";
                        break;
                    default:
                        operationName = "";
                        break;
                }

                detail.OperationName = operationName;
            }

            return details.AsEnumerable();
        }

        /// <summary>
        /// ダウンロードデータ（ケース明細）を取得
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public IEnumerable<InOutReferencePackageStockReportRow> GetPackageStockReportData(InOutReferenceSearchConditions conditions)
        {
            var sql = $@"
                SELECT 
                        STK.CENTER_ID
                    ,   STK.INS_DATE AS MOVE_DATE
                    ,   STK.OPERATION_ID
                    ,   STK.UPDATE_PROGRAM_NAME AS OPERATION_NAME
                    ,   STK.OPERATION_CLASS
                    ,   STK.UPDATE_USER_ID AS OPERATION_USER_ID
                    ,   MUS.USER_NAME AS OPERATION_USER_NAME
                    ,   STK.LOCATION_CD AS LOCATION_CODE
                    ,   MLOC.LOCATION_CLASS
                    ,   MLOCCL.LOCATION_NAME AS LOCATION_CLASS_NAME
                    ,   STK.BOX_NO
                    ,   MSKU.CATEGORY_ID1
                    ,   MCATE1.CATEGORY_NAME1
                    ,   STK.ITEM_ID
                    ,   MSKU.ITEM_NAME
                    ,   STK.ITEM_COLOR_ID
                    ,   MCOL.ITEM_COLOR_NAME
                    ,   STK.ITEM_SIZE_ID
                    ,   MSKU.ITEM_SIZE_NAME
                    ,   STK.JAN
                    ,   STK.GRADE_ID
                    ,   MGRA.GRADE_NAME
                    ,   STK.INVOICE_NO
                    ,   STK.STOCK_QTY
                    ,   STK.ALLOC_QTY
                    ,   STK.UPDATE_COUNT
                    ,   TO_CHAR(STK.SORT_STATUS) AS SORT_STATUS
                FROM
                        TL_PACKAGE_STOCKS STK
                LEFT OUTER JOIN
                        M_USERS MUS
                ON
                        MUS.USER_ID = STK.UPDATE_USER_ID
                    AND MUS.SHIPPER_ID = STK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_LOCATIONS MLOC
                ON
                        MLOC.LOCATION_CD = STK.LOCATION_CD
                    AND MLOC.CENTER_ID = STK.CENTER_ID
                    AND MLOC.SHIPPER_ID = STK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_LOCATION_CLASSES MLOCCL
                ON
                        MLOCCL.LOCATION_CLASS = MLOC.LOCATION_CLASS
                    AND MLOCCL.SHIPPER_ID = MLOC.SHIPPER_ID
                LEFT OUTER JOIN
                        M_ITEM_SKU MSKU
                ON
                        MSKU.ITEM_SKU_ID = STK.ITEM_SKU_ID
                    AND MSKU.SHIPPER_ID = STK.SHIPPER_ID
                LEFT OUTER JOIN (
                    SELECT 
                            SHIPPER_ID
                        ,   CATEGORY_ID1
                        ,   MAX(CATEGORY_NAME1) AS CATEGORY_NAME1
                    FROM
                            M_ITEM_CATEGORIES4
                    WHERE
                            SHIPPER_ID = :SHIPPER_ID
                    GROUP BY 
                            SHIPPER_ID
                        ,   CATEGORY_ID1
                ) MCATE1
                ON
                        MCATE1.CATEGORY_ID1 = MSKU.CATEGORY_ID1
                    AND MCATE1.SHIPPER_ID = MSKU.SHIPPER_ID
                LEFT OUTER JOIN
                        M_COLORS MCOL
                ON
                        MCOL.ITEM_COLOR_ID = STK.ITEM_COLOR_ID
                    AND MCOL.SHIPPER_ID = STK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_SIZES MSIZ
                ON
                        MSIZ.ITEM_SIZE_ID = STK.ITEM_SIZE_ID
                    AND MSIZ.SHIPPER_ID = STK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_GRADES MGRA
                ON
                        MGRA.GRADE_ID = STK.GRADE_ID
                    AND MGRA.SHIPPER_ID = STK.SHIPPER_ID
                WHERE
                        STK.SHIPPER_ID = :SHIPPER_ID
                        {GetSqlConditions(conditions)}
                ORDER BY
                        {GetSqlPackageStockOrder(conditions)}
            ";

            var parameters = new DynamicParameters();

            parameters.AddDynamicParams(
                new
                {
                    SHIPPER_ID = Profile.User.ShipperId,
                    CENTER_ID = conditions.CenterId,
                    MOVE_DATE_FROM = conditions.MoveDateFrom,
                    MOVE_DATE_TO = conditions.MoveDateTo,
                    JAN = conditions.Jan,
                    LOCATION_CLASS = conditions.LocationClass,
                    ITEM_ID = conditions.ItemId,
                    LOCATION_CD = conditions.LocationCode,
                    ITEM_SKU_ID = conditions.ItemSkuId,
                    BOX_NO = conditions.BoxNo
                });

            var details = MvcDbContext.Current.Database.Connection.Query<InOutReferencePackageStockReportRow>(sql, parameters).ToList();

            foreach (var detail in details)
            {
                string operationName = string.Empty;
                string sortStatus = string.Empty;

                switch (detail.OperationName)
                {
                    case "SP_W_SHP_MODIFYTCINSTRUCTION":
                        operationName = "TC出荷指示修正";
                        break;
                    case "SP_W_SHP_CASEALLOC_CANCEL":
                        operationName = "ケース出荷引当解除";
                        break;
                    case "SP_W_SHP_CASE_INSTRUCTION":
                        operationName = "ケース出荷指示取込";
                        break;
                    case "SP_W_SHP_AUTO_TCALLOC":
                        operationName = "TC自動引当";
                        break;
                    case "SP_W_RTN_STOCK_REG":
                        operationName = "在庫登録処理(EC返品受付)";
                        break;
                    case "SP_W_INV_CONFIRM":
                        operationName = "棚卸開始";
                        break;
                    case "SP_H_STK_SORT_REG":
                        operationName = "在庫仕分実績登録";
                        break;
                    case "SP_DAILY_STOCK_WITHDRAWAL":
                        operationName = "日次処理(在庫引落し)";
                        break;
                    case "PK_W_MOV_INPUTTRANSFER02.UPDATE_RESULT":
                        operationName = "移動入荷実績更新";
                        break;
                    case "PK_W_H_ARR_SORT.UpdateSortStatusFromWork":
                        operationName = "仕分入荷計上";
                        break;
                    case "PK_W_H_ARR_SORT.SortTCDC":
                        operationName = "仕分入荷計上";
                        break;
                    case "PK_W_H_ARR_SORT.FinishDeliverySku":
                        operationName = "仕分入荷計上";
                        break;
                    case "PK_W_H_ARR_SORT.NumberingBoxNum":
                        operationName = "仕分入荷計上";
                        break;
                    case "PK_W_ARR_INPUTPURCHASE02.MERGE_PACKAGE_STOCK":
                        operationName = "入荷実績入力";
                        break;
                    case "PK_W_ARR_INPUTPURCHASE02.UPDATE_RESULT":
                        operationName = "入荷実績入力";
                        break;
                    case "PK_STK_ADJUSTMENT.SP_MERGE_PACKAGE_STOCKS":
                        operationName = "在庫調整";
                        break;
                    case "PK_LOC_MOVE_ASSORTPIC.PicAssortBoxUpdateStock":
                        operationName = "ピック完了登録";
                        break;
                    case "PK_LOC_MOVE_ASSORTPIC.PicAssortSkuUpdateStock":
                        operationName = "ピック完了登録";
                        break;
                    case "PK_LOC_MOVE.UpdateStock":
                        operationName = "在庫移動";
                        break;
                    case "PK_H_STOCK_SORTS.SET_RESULT":
                        operationName = "仕分先ケース在庫移動";
                        break;
                    case "PK_H_MOV_TRANSFERINSPECTION.SET_RESULT":
                        operationName = "拠点間移動入荷検品";
                        break;
                    case "PK_H_MOV_SIMPLEINSPECTION.SET_RESULT":
                        operationName = "外装検品";
                        break;
                    case "PK_H_MOV_COM_SORT_INSPECTION.SET_RESULT":
                        operationName = "強制仕分検品";
                        break;
                    case "PK_H_MOV_SORTINSPECTION.SET_RESULT":
                        operationName = "仕分検品";
                        break;
                    case "PK_H_MOV_ALLINSPECTION.SET_RESULT":
                        operationName = "全検品";
                        break;
                    case "PK_H_MOV_COM_ALL_INSPECTION.SET_RESULT":
                        operationName = "強制全検品";
                        break;
                    case "PK_H_CREATE_CASE.CreateBox":
                        operationName = "ケース作成";
                        break;
                    case "SP_W_RET_PURCHASE_CORRECTION_UPD":
                        operationName = "仕入訂正確定";
                        break;
                    case "SP_W_RET_PURCHASE_RETURNS_UPD":
                        operationName = "";
                        break;
                    case "仕入返品確定":
                        operationName = "";
                        break;
                    case "SP_W_SHP_TCALLOC_SUB":
                        operationName = "TC引当";
                        break;
                    case "SP_W_SHP_ECALLOC_SAVE_CANCEL":
                        operationName = "EC引当保持分解除";
                        break;
                    case "SP_W_SHP_ECALLOC_SAVE_CAN_SUB":
                        operationName = "EC引当保持分解除";
                        break;
                    case "SP_W_SHP_ECALLOC_MAIN":
                        operationName = "EC引当";
                        break;
                    case "SP_W_SHP_ECALLOC_CANCEL":
                        operationName = "EC引当解除";
                        break;
                    case "SP_W_SHP_DCALLOC_MAIN":
                        operationName = "DC引当";
                        break;
                    case "SP_W_SHP_DCALLOC_CANCEL":
                        operationName = "DC引当解除";
                        break;
                    case "PK_W_ARR_INPUTPURCHASE02.MERGE_STOCK":
                        operationName = "入荷実績入力";
                        break;
                    case "PK_STK_ADJUSTMENT.SP_MERGE_STOCKS":
                        operationName = "在庫調整";
                        break;
                    default:
                        operationName = "";
                        break;
                }

                switch (detail.SortStatus)
                {
                    case "1":
                        sortStatus = InOutReferenceResource.TCCorrectWait;
                        break;
                    case "2":
                        sortStatus = InOutReferenceResource.TCCreateWait;
                        break;
                    case "3":
                        sortStatus = InOutReferenceResource.TCPossible;
                        break;
                    default:
                        sortStatus = "";
                        break;
                }

                detail.OperationName = operationName;
                detail.SortStatus = sortStatus;
            }

            return details.AsEnumerable();
        }
        #endregion ダウンロードデータ検索
    }
}