namespace Wms.Areas.Returns.Query.PurchaseReturnReference
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Extensions.Classes;
    using Wms.Areas.Returns.Models;
    using Wms.Areas.Returns.Resources;
    using Wms.Areas.Returns.ViewModels.PurchaseReturnReference;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Returns.ViewModels.PurchaseReturnReference.PurchaseReturnReference01SearchConditions;

    public class PurchaseReturnReferenceQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertArrPurchaseReturnReference01(PurchaseReturnReference01SearchConditions condition)
        {
            // 1.ワークID採番
            condition.Seq = new BaseQuery().GetWorkId();
            condition.Page = 1;

            // 2.ワーク作成
            // 検索結果取得
            var result = AddMovTransRef01(condition);
            return result;
        }

        /// <summary>
        /// 移動入荷進捗照会ワーク01にデータを登録する
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        private bool AddMovTransRef01(PurchaseReturnReference01SearchConditions condition)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                    INSERT INTO WW_RET_PURCHASE_REFERENCE (
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
                        ,   CENTER_ID
                        ,   RETURN_ID
                        ,   RETURN_SEQ
                        ,   RETUEN_CLASS
                        ,   VENDOR_ID
                        ,   INVOICE_NO
                        ,   ARRIVE_DATE
                        ,   ITEM_SKU_ID
                        ,   JAN
                        ,   ITEM_ID
                        ,   ITEM_COLOR_ID
                        ,   ITEM_SIZE_ID
                        ,   RETURN_QTY
                        ,   NORMAL_BUYING_PRICE
                        ,   PURCHASE_BUYING_PRICE
                        ,   NORMAL_SELLING_PRICE_EX_TAX
                        ,   INPUT_USER_ID
                        ,   INPUT_USER_NAME
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
                        ,   ROWNUM
                        ,   :CENTER_ID
                        ,   TMR.RETURN_ID
                        ,   TMR.RETURN_SEQ
                        ,   TMR.RETUEN_CLASS
                        ,   TMR.VENDOR_ID
                        ,   TMR.INVOICE_NO
                        ,   TMR.ARRIVE_DATE
                        ,   TMR.ITEM_SKU_ID
                        ,   TMR.JAN
                        ,   TMR.ITEM_ID
                        ,   TMR.ITEM_COLOR_ID
                        ,   TMR.ITEM_SIZE_ID
                        ,   TMR.RETURN_QTY
                        ,   TMR.NORMAL_BUYING_PRICE
                        ,   TMR.PURCHASE_BUYING_PRICE
                        ,   TMR.NORMAL_SELLING_PRICE_EX_TAX
                        ,   TMR.INPUT_USER_ID
                        ,   TMR.INPUT_USER_NAME
                    FROM
                            T_MKRETURN_RESULTS TMR
                    INNER JOIN
                            M_ITEM_SKU MIS
                    ON 
                            TMR.SHIPPER_ID = MIS.SHIPPER_ID
                        AND TMR.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                    LEFT OUTER JOIN
                            M_BRANDS MB
                    ON
                            MIS.SHIPPER_ID = MB.SHIPPER_ID
                        AND MIS.BRAND_ID = MB.BRAND_ID
                    LEFT JOIN M_VENDORS MV
                    ON
                            TMR.SHIPPER_ID = MV.SHIPPER_ID
                        AND TMR.VENDOR_ID = MV.VENDOR_ID
                    WHERE 
                            TMR.SHIPPER_ID  = :SHIPPER_ID
                        AND TMR.CENTER_ID = :CENTER_ID
                ");
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                    parameters.Add(":CENTER_ID", condition.CenterId);
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":PROGRAM_NAME", "PurchaseReturnReference");
                    parameters.Add(":USER_ID", Common.Profile.User.UserId);

                    // 訂正区分
                    if (condition.RetuenClass != ReturnClasses.NotSelect)
                    {
                        query.AppendLine(@" AND TMR.RETUEN_CLASS = :RETUEN_CLASS ");
                        parameters.Add(":RETUEN_CLASS", condition.RetuenClass);
                    }

                    // 入荷予定開始日
                    if (condition.ArriveDateFrom != null)
                    {
                        query.Append(" AND TO_CHAR(TMR.ARRIVE_DATE,'YYYY/MM/DD') >= :ARRIVE_DATE_FROM ");
                        parameters.Add(":ARRIVE_DATE_FROM", condition.ArriveDateFrom);
                    }

                    // 入荷予定終了日
                    if (condition.ArriveDateTo != null)
                    {
                        query.Append(" AND TO_CHAR(TMR.ARRIVE_DATE,'YYYY/MM/DD') <= :ARRIVE_DATE_TO ");
                        parameters.Add(":ARRIVE_DATE_TO", condition.ArriveDateTo);
                    }

                    // 事業部
                    if (!string.IsNullOrEmpty(condition.DivisionCd))
                    {
                        query.AppendLine(@" AND MIS.DIVISION_ID = :DIVISION_ID ");
                        parameters.Add(":DIVISION_ID", condition.DivisionCd);
                    }

                    // 仕入先
                    if (!string.IsNullOrEmpty(condition.VendorId))
                    {
                        query.AppendLine(@" AND TMR.VENDOR_ID LIKE :VENDOR_ID ");
                        parameters.Add(":VENDOR_ID", condition.VendorId + "%");
                    }
                    if (!string.IsNullOrEmpty(condition.VendorName))
                    {
                        query.AppendLine(@" AND MV.VENDOR_NAME1 LIKE :VENDOR_NAME ");
                        parameters.Add(":VENDOR_NAME", condition.VendorName + "%");
                    }

                    // 返品伝票ID
                    if (!string.IsNullOrEmpty(condition.ReturnId))
                    {
                        query.AppendLine(@" AND TMR.RETURN_ID = :RETURN_ID ");
                        parameters.Add(":RETURN_ID", condition.ReturnId);
                    }

                    // 納品書番号
                    if (!string.IsNullOrEmpty(condition.InvoiceNo))
                    {
                        query.AppendLine(@" AND TMR.INVOICE_NO = :INVOICE_NO ");
                        parameters.Add(":INVOICE_NO", condition.InvoiceNo);
                    }

                    // 実績登録者
                    if (!string.IsNullOrEmpty(condition.InputUserId))
                    {
                        query.AppendLine(@" AND TMR.INPUT_USER_ID = :INPUT_USER_ID ");
                        parameters.Add(":INPUT_USER_ID", condition.InputUserId);
                    }

                    // ブランド
                    if (!string.IsNullOrEmpty(condition.BrandId))
                    {
                        query.AppendLine(@" AND MIS.BRAND_ID LIKE :BRAND_ID ");
                        parameters.Add(":BRAND_ID", condition.BrandId + "%");
                    }

                    // ブランド名
                    if (string.IsNullOrEmpty(condition.BrandId) && !string.IsNullOrEmpty(condition.BrandName))
                    {
                        query.AppendLine(@" AND MB.BRAND_NAME LIKE :BRAND_NAME ");
                        parameters.Add(":BRAND_NAME", condition.BrandName + "%");
                    }

                    // 分類
                    if (!string.IsNullOrEmpty(condition.CategoryId1))
                    {
                        query.AppendLine(@" AND MIS.CATEGORY_ID1 = :CATEGORY_ID1 ");
                        parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId2))
                    {
                        query.AppendLine(@" AND MIS.CATEGORY_ID2 = :CATEGORY_ID2 ");
                        parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId3))
                    {
                        query.AppendLine(@" AND MIS.CATEGORY_ID3 = :CATEGORY_ID3 ");
                        parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId4))
                    {
                        query.AppendLine(@" AND MIS.CATEGORY_ID4 = :CATEGORY_ID4 ");
                        parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
                    }

                    // 品番
                    if (!string.IsNullOrEmpty(condition.ItemId))
                    {
                        query.AppendLine(@" AND MIS.ITEM_ID LIKE :ITEM_ID ");
                        parameters.Add(":ITEM_ID", condition.ItemId + "%");
                    }

                    // JAN
                    if (!string.IsNullOrEmpty(condition.Jan))
                    {
                        query.AppendLine(@" AND MIS.JAN LIKE :JAN ");
                        parameters.Add(":JAN", condition.Jan + "%");
                    }

                    // SKU
                    if (!string.IsNullOrEmpty(condition.ItemSkuId))
                    {
                        query.AppendLine(@" AND MIS.ITEM_SKU_ID LIKE :ITEM_SKU_ID ");
                        parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId + "%");
                    }

                    query.AppendLine(@" ORDER BY
                                            TMR.RETURN_ID
                                        ,   TMR.RETURN_SEQ");

                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
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
        /// Get StockInquiry List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<PurchaseReturnReference01ResultRow> PurchaseReturnReference01GetData(PurchaseReturnReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            string order;
            switch (condition.SortKey)
            {
                case PurchaseReturnReference01SortKey.ReturnId:
                    switch (condition.Sort)
                    {
                        case PurchaseReturnReference01SearchConditions.AscDescSort.Desc:
                            order = "ORDER BY WW.RETURN_ID DESC ";
                            break;

                        default:
                            order = "ORDER BY WW.RETURN_ID ASC ";
                            break;
                    }

                    break;
                default:
                    switch (condition.Sort)
                    {
                        case PurchaseReturnReference01SearchConditions.AscDescSort.Desc:
                            order = "ORDER BY MAX(WW.VENDOR_ID) DESC,WW.RETURN_ID DESC ";
                            break;

                        default:
                            order = "ORDER BY MAX(WW.VENDOR_ID) ASC,WW.RETURN_ID ASC ";
                            break;
                    }

                    break;
            }

            StringBuilder query = new StringBuilder(@"
               SELECT
                        SUB.LINE_NO
                    ,   SUB.SHIPPER_ID
                    ,   SUB.SEQ
                    ,   SUB.CENTER_ID
                    ,   SUB.RETURN_ID
                    ,   SUB.RETUEN_CLASS
                    ,   SUB.RETURN_CLASS_NAME
                    ,   SUB.VENDOR_ID
                    ,   MV.VENDOR_NAME1 AS VENDOR_NAME
                    ,   SUB.INVOICE_NO
                    ,   SUB.ARRIVE_DATE
                    ,   SUB.INPUT_USER_ID
               FROM(
                SELECT
                        ROW_NUMBER() OVER ( PARTITION BY WW.SHIPPER_ID, WW.CENTER_ID, WW.RETURN_ID ");
            query.AppendLine(order);
            query.AppendLine(@"
                        )                       AS LINE_NO
                    ,   WW.SHIPPER_ID
                    ,   MAX(WW.SEQ)             AS SEQ
                    ,   WW.CENTER_ID
                    ,   WW.RETURN_ID
                    ,   MAX(WW.RETUEN_CLASS)    AS RETUEN_CLASS
                    ,   CASE MAX(WW.RETUEN_CLASS) WHEN 0 THEN '仕入先返品' ELSE '仕入訂正' END AS RETURN_CLASS_NAME
                    ,   MAX(WW.VENDOR_ID)       AS VENDOR_ID
                    ,   MAX(MV.VENDOR_NAME1)    AS VENDOR_NAME
                    ,   MAX(WW.INVOICE_NO)      AS INVOICE_NO
                    ,   MAX(TO_CHAR(WW.ARRIVE_DATE, 'YYYY/MM/DD'))     AS ARRIVE_DATE
                    ,   MAX(WW.INPUT_USER_ID)   AS INPUT_USER_ID
                FROM
                        WW_RET_PURCHASE_REFERENCE WW
                INNER JOIN M_ITEM_SKU MIS
                ON 
                        WW.SHIPPER_ID = MIS.SHIPPER_ID
                    AND WW.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                LEFT JOIN M_COLORS MC
                ON
                        MIS.SHIPPER_ID = MC.SHIPPER_ID
                    AND MIS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                LEFT JOIN M_SIZES MS
                ON
                        MIS.SHIPPER_ID = MS.SHIPPER_ID
                    AND MIS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                LEFT JOIN M_ITEM_CATEGORIES4 CTGRI
                ON
                        CTGRI.CATEGORY_ID1 = MIS.CATEGORY_ID1
                    AND CTGRI.CATEGORY_ID2 = MIS.CATEGORY_ID2
                    AND CTGRI.CATEGORY_ID3 = MIS.CATEGORY_ID3
                    AND CTGRI.CATEGORY_ID4 = MIS.CATEGORY_ID4
                    AND CTGRI.SHIPPER_ID = MIS.SHIPPER_ID
                LEFT JOIN M_VENDORS MV
                ON
                       WW.SHIPPER_ID = MV.SHIPPER_ID
                    AND WW.VENDOR_ID = MV.VENDOR_ID
                WHERE
                        WW.SHIPPER_ID = :SHIPPER_ID
                    AND WW.CENTER_ID = :CENTER_ID
                    AND WW.SEQ = :SEQ
                GROUP BY
                        WW.SHIPPER_ID
                    ,   WW.CENTER_ID
                    ,   WW.RETURN_ID
                ) SUB
                LEFT JOIN M_VENDORS MV
                ON
                        SUB.SHIPPER_ID = MV.SHIPPER_ID
                    AND SUB.VENDOR_ID = MV.VENDOR_ID
                ORDER BY
                        SUB.LINE_NO
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<PurchaseReturnReference01ResultRow>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var PurchaseReturnReference01s = MvcDbContext.Current.Database.Connection.Query<PurchaseReturnReference01ResultRow>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<PurchaseReturnReference01ResultRow>(PurchaseReturnReference01s, condition.Page, condition.PageSize, totalCount);
        }

         /// <summary>
        /// 仕入返品訂正照会ワーク02
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IList<PurchaseReturnReference02ResultRow> PurchaseReturnReference02GetData(PurchaseReturnReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT
                        ROW_NUMBER() OVER (ORDER BY
                                                WW.VENDOR_ID
                                            ,   MIS.CATEGORY_ID1
                                            ,   MIS.ITEM_ID)   AS LINE_NO
                    ,   WW.VENDOR_ID
                    ,   MV.VENDOR_NAME1    AS VENDOR_NAME
                    ,   CTGRI.CATEGORY_NAME1 AS CATEGORY_NAME
                    ,   WW.ITEM_ID
                    ,   MIS.ITEM_NAME
                    ,   WW.ITEM_COLOR_ID
                    ,   MC.ITEM_COLOR_NAME
                    ,   WW.ITEM_SIZE_ID
                    ,   MS.ITEM_SIZE_NAME
                    ,   WW.JAN
                    ,   WW.RETURN_QTY
                    ,   WW.NORMAL_BUYING_PRICE
                    ,   WW.PURCHASE_BUYING_PRICE
                    ,   WW.NORMAL_SELLING_PRICE_EX_TAX
                    ,   WW.INVOICE_NO
                    ,   WW.RETUEN_CLASS
                    ,   CASE WW.RETUEN_CLASS WHEN 0 THEN '仕入先返品' ELSE '仕入訂正' END AS RETURN_CLASS_NAME
                    ,   WW.SHIPPER_ID
                    ,   WW.CENTER_ID
                    ,   WW.RETURN_ID
                    ,   TO_CHAR(WW.ARRIVE_DATE, 'YYYY/MM/DD')  ARRIVE_DATE          --返品登録日
                    ,   WW.INPUT_USER_ID        --返品実績者ID
                    ,   WW.INPUT_USER_NAME      --返品実績者名
                FROM
                        WW_RET_PURCHASE_REFERENCE WW
                INNER JOIN M_ITEM_SKU MIS
                ON 
                        WW.SHIPPER_ID = MIS.SHIPPER_ID
                    AND WW.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                LEFT JOIN M_COLORS MC
                ON
                        MIS.SHIPPER_ID = MC.SHIPPER_ID
                    AND MIS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                LEFT JOIN M_SIZES MS
                ON
                        MIS.SHIPPER_ID = MS.SHIPPER_ID
                    AND MIS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                LEFT JOIN M_ITEM_CATEGORIES4 CTGRI
                ON
                        CTGRI.CATEGORY_ID1 = MIS.CATEGORY_ID1
                    AND CTGRI.CATEGORY_ID2 = MIS.CATEGORY_ID2
                    AND CTGRI.CATEGORY_ID3 = MIS.CATEGORY_ID3
                    AND CTGRI.CATEGORY_ID4 = MIS.CATEGORY_ID4
                    AND CTGRI.SHIPPER_ID = MIS.SHIPPER_ID
                LEFT JOIN M_VENDORS MV
                ON
                       WW.SHIPPER_ID = MV.SHIPPER_ID
                    AND WW.VENDOR_ID = MV.VENDOR_ID
                WHERE
                        WW.SHIPPER_ID = :SHIPPER_ID
                    AND WW.CENTER_ID = :CENTER_ID
                    AND WW.SEQ = :SEQ
                    AND WW.RETURN_ID = :RETURN_ID
                ORDER BY
                        WW.VENDOR_ID
                    ,   MIS.CATEGORY_ID1
                    ,   MIS.ITEM_ID
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SEQ", condition.Seq);
            parameters.Add(":RETURN_ID", condition.HidReturnId);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<PurchaseReturnReference02ResultRow>(query.ToString(), parameters).ToList();
        }

        /// <summary>
        /// 事業部データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListDivisions()
        {
            return MvcDbContext.Current.Divisions
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId)
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
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId1,
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
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId2,
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
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId
                && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1)
                && (categoryId2 == null ? 1 == 1 : m.CategoryId2 == categoryId2))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId3,
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
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId
                && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1)
                && (categoryId2 == null ? 1 == 1 : m.CategoryId2 == categoryId2)
                && (categoryId3 == null ? 1 == 1 : m.CategoryId3 == categoryId3))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId4,
                    Text = m.CategoryId4.ToString() + ":" + m.CategoryName4
                })
                .Distinct().OrderBy(m => m.Value);
        }
    }
}