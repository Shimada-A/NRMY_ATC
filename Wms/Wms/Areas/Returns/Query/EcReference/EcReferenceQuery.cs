namespace Wms.Areas.Returns.Query.EcReference
{
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
    using Wms.Areas.Returns.Models;
    using Wms.Areas.Returns.Resources;
    using Wms.Areas.Returns.ViewModels.EcReference;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Returns.ViewModels.EcReference.EcReference01SearchConditions;
    using static Wms.Areas.Returns.ViewModels.EcReference.EcReference02SearchConditions;

    public class EcReferenceQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertRetEcReference01(EcReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT
                        A_ECSHP.CENTER_ID
                    ,   MAX(T_ECRET.ARRIVE_DATE) AS ARRIVE_DATE
                    ,   A_ECSHP.SHIP_INSTRUCT_ID
                    ,   MAX(A_ECSHP.KAKU_DATE) AS KAKU_DATE
                    ,   T_ECRET.RETURN_ID
                    ,   SUM(T_ECRET.RETURN_QTY) AS RETURN_QTY_SUM
                FROM
                        T_ECRETURN_RESULTS T_ECRET
                INNER JOIN
                        A_ECSHIPS A_ECSHP
                ON
                        A_ECSHP.SHIPPER_ID = T_ECRET.SHIPPER_ID
                    AND A_ECSHP.CENTER_ID = T_ECRET.CENTER_ID
                    AND A_ECSHP.SHIP_INSTRUCT_ID = T_ECRET.SHIP_INSTRUCT_ID
                    AND A_ECSHP.SHIP_INSTRUCT_SEQ = T_ECRET.SHIP_INSTRUCT_SEQ
                LEFT JOIN
                        M_ITEM_SKU MIS
                ON
                        T_ECRET.SHIPPER_ID = MIS.SHIPPER_ID
                    AND A_ECSHP.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                LEFT OUTER JOIN
                        M_BRANDS MB
                ON
                        MIS.BRAND_ID = MB.BRAND_ID
                    AND MIS.SHIPPER_ID = MB.SHIPPER_ID
                WHERE
                        A_ECSHP.CENTER_ID = :CENTER_ID
                    AND A_ECSHP.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);

            // 事業部
            if (!string.IsNullOrEmpty(condition.DivisionId))
            {
                query.Append(" AND MIS.DIVISION_ID = :DIVISION_ID ");
                parameters.Add(":DIVISION_ID", condition.DivisionId);
            }

            // 返品登録日(From-To)
            if (condition.ArriveDateFrom != null)
            {
                query.Append(" AND TRUNC(T_ECRET.ARRIVE_DATE) >= :ARRIVE_DATE_FROM ");
                parameters.Add(":ARRIVE_DATE_FROM", condition.ArriveDateFrom);
            }

            if (condition.ArriveDateTo != null)
            {
                query.Append(" AND TRUNC(T_ECRET.ARRIVE_DATE) <= :ARRIVE_DATE_TO ");
                parameters.Add(":ARRIVE_DATE_TO", condition.ArriveDateTo);
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

            // EC注文番号
            if (!string.IsNullOrEmpty(condition.ShipInstructId))
            {
                query.Append(" AND T_ECRET.SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID ");
                parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId);
            }

            // 分類
            if (!string.IsNullOrEmpty(condition.CategoryId1))
            {
                query.Append(" AND MIS.CATEGORY_ID1 = :CATEGORY_ID1 ");
                parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId2))
            {
                query.Append(" AND MIS.CATEGORY_ID2 = :CATEGORY_ID2 ");
                parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId3))
            {
                query.Append(" AND MIS.CATEGORY_ID3 = :CATEGORY_ID3 ");
                parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId4))
            {
                query.Append(" AND MIS.CATEGORY_ID4 = :CATEGORY_ID4 ");
                parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
            }

            // 返品伝票ID
            if (!string.IsNullOrEmpty(condition.ReturnId))
            {
                query.Append(" AND T_ECRET.RETURN_ID = :RETURN_ID ");
                parameters.Add(":RETURN_ID", condition.ReturnId);
            }

            // 品番
            if (!string.IsNullOrEmpty(condition.ItemId))
            {
                query.Append(" AND T_ECRET.ITEM_ID LIKE :ITEM_ID ");
                parameters.Add(":ITEM_ID", condition.ItemId + "%");
            }

            // JAN
            if (!string.IsNullOrEmpty(condition.Jan))
            {
                query.Append(" AND T_ECRET.JAN LIKE :JAN ");
                parameters.Add(":JAN", condition.Jan + "%");
            }

            // SKU
            if (!string.IsNullOrEmpty(condition.ItemSkuId))
            {
                query.Append(" AND T_ECRET.ITEM_SKU_ID LIKE :ITEM_SKU_ID ");
                parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId + "%");
            }
            query.Append(@"
            GROUP BY
                    A_ECSHP.CENTER_ID
                  , A_ECSHP.SHIP_INSTRUCT_ID
                  , T_ECRET.RETURN_ID");

            // 1.ワークID採番
            var seq = new BaseQuery().GetWorkId();

            // 2.検索・ワーク作成
            var result = MvcDbContext.Current.Database.Connection.Query<RetEcReference>(query.ToString(), parameters);
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var row in result.Select((v, i) => new { v, i }))
                {
                    var RetEcReference = new RetEcReference();
                    RetEcReference.SetBaseInfoInsert();
                    RetEcReference.Seq = seq;
                    RetEcReference.LineNo = row.i + 1;
                    RetEcReference.CenterId = row.v.CenterId;
                    RetEcReference.ArriveDate = row.v.ArriveDate;
                    RetEcReference.KakuDate = row.v.KakuDate;
                    RetEcReference.ShipInstructId = row.v.ShipInstructId;
                    RetEcReference.ReturnId = row.v.ReturnId;
                    RetEcReference.ReturnQtySum = row.v.ReturnQtySum;
                    MvcDbContext.Current.RetEcReference01s.Add(RetEcReference);
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
                condition.Seq = seq;
                condition.Page = 1;
            }
            return true;
        }

        /// <summary>
        /// Get StockInquiry List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<EcReference01ResultRow> EcReference01GetData(EcReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                SELECT *
                  FROM WW_RET_EC_REFERENCE01
                 WHERE SHIPPER_ID = :SHIPPER_ID
                   AND SEQ = :SEQ ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<EcReference01ResultRow>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case DataSortKey.ArriveShpIns:
                    switch (condition.Sort)
                    {
                        case EcReference01SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY ARRIVE_DATE DESC, SHIP_INSTRUCT_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY ARRIVE_DATE ASC, SHIP_INSTRUCT_ID ASC ");
                            break;
                    }

                    break;
                default:
                    switch (condition.Sort)
                    {
                        case EcReference01SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY SHIP_INSTRUCT_ID DESC, ARRIVE_DATE DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY SHIP_INSTRUCT_ID ASC, ARRIVE_DATE ASC ");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);
            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var EcReference01s = MvcDbContext.Current.Database.Connection.Query<EcReference01ResultRow>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<EcReference01ResultRow>(EcReference01s, condition.Page, condition.PageSize, totalCount);
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
                .Where(m => m.ShipperId == Profile.User.ShipperId && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1))
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
                .Where(m => m.ShipperId == Profile.User.ShipperId
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
                .Where(m => m.ShipperId == Profile.User.ShipperId
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

        /// <summary>
        /// Get StockInquiry List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public List<EcReference02ResultRow> EcReference02GetData(EcReference02SearchConditions condition)
        {
            var EcReference01 = MvcDbContext.Current.RetEcReference01s.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == condition.Seq && x.LineNo == condition.LineNo).FirstOrDefault();
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                SELECT
                        T_ECRET.CENTER_ID
                    ,   T_ECRET.ARRIVE_DATE
                    ,   T_ECRET.RELATED_ORDER_NO
                    ,   A_ECSHP.KAKU_DATE
                    ,   T_ECRET.SHIP_INSTRUCT_ID
                    ,   T_ECRET.RETURN_ID
                    ,   MIS.CATEGORY_ID1
                    ,   CTGRI.CATEGORY_NAME1
                    ,   T_ECRET.ITEM_ID
                    ,   MIS.ITEM_NAME
                    ,   T_ECRET.ITEM_COLOR_ID
                    ,   MC.ITEM_COLOR_NAME
                    ,   T_ECRET.ITEM_SIZE_ID
                    ,   MS.ITEM_SIZE_NAME
                    ,   T_ECRET.JAN
                    ,   A_ECSHP.RESULT_QTY
                    ,   T_ECRET.RETURN_QTY
                FROM
                        T_ECRETURN_RESULTS T_ECRET
                INNER JOIN
                        A_ECSHIPS A_ECSHP
                ON
                        A_ECSHP.SHIP_INSTRUCT_ID = T_ECRET.SHIP_INSTRUCT_ID
                    AND A_ECSHP.SHIP_INSTRUCT_SEQ = T_ECRET.SHIP_INSTRUCT_SEQ
                    AND A_ECSHP.CENTER_ID = T_ECRET.CENTER_ID
                    AND A_ECSHP.SHIPPER_ID = T_ECRET.SHIPPER_ID
                LEFT JOIN
                        M_ITEM_SKU MIS
                ON
                        T_ECRET.SHIPPER_ID = MIS.SHIPPER_ID
                    AND T_ECRET.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                LEFT JOIN
                        M_COLORS MC
                ON
                        T_ECRET.SHIPPER_ID = MC.SHIPPER_ID
                    AND T_ECRET.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                LEFT JOIN
                        M_SIZES MS
                ON
                        T_ECRET.SHIPPER_ID = MS.SHIPPER_ID
                    AND T_ECRET.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                LEFT OUTER JOIN
                        M_ITEM_CATEGORIES4 CTGRI
                ON
                        CTGRI.CATEGORY_ID1 = MIS.CATEGORY_ID1
                    AND CTGRI.CATEGORY_ID2 = MIS.CATEGORY_ID2
                    AND CTGRI.CATEGORY_ID3 = MIS.CATEGORY_ID3
                    AND CTGRI.CATEGORY_ID4 = MIS.CATEGORY_ID4
                    AND CTGRI.SHIPPER_ID = MIS.SHIPPER_ID
                WHERE
                        T_ECRET.SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                    AND T_ECRET.CENTER_ID = :CENTER_ID
                    AND T_ECRET.SHIPPER_ID = :SHIPPER_ID
                    AND T_ECRET.RETURN_ID = :RETURN_ID ");

            parameters.Add(":SHIP_INSTRUCT_ID", EcReference01.ShipInstructId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":RETURN_ID", EcReference01.ReturnId);
            var vm = new EcReference02ViewModel();
            vm.SearchConditions = MvcDbContext.Current.Database.Connection.Query<EcReference02SearchConditions>(query.ToString(), parameters).FirstOrDefault();
            // Fill data to memory
            var EcReference02s = MvcDbContext.Current.Database.Connection.Query<EcReference02ResultRow>(query.ToString(), parameters);

            // Excute paging
            return new List<EcReference02ResultRow>(EcReference02s);
        }
    }
}