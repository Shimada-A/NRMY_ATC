namespace Wms.Areas.Returns.Query.EcReference
{
    using System.Collections.Generic;
    using System.Text;
    using Dapper;
    using Wms.Areas.Returns.ViewModels.EcReference;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Returns.ViewModels.EcReference.EcReference01SearchConditions;

    public class Report : BaseQuery
    {
        public IEnumerable<EcReferenceReport> EcReferenceListing(EcReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder();
            query.Append(@"
                       SELECT
                              T_ECRET.ARRIVE_DATE
                            , T_ECRET.SHIP_INSTRUCT_ID
                            , A_ECSHP.KAKU_DATE AS KAKU_DATE
                            , T_ECRET.SHIP_INSTRUCT_SEQ
                            , T_ECRET.RETURN_ID
                            , T_ECRET.RETURN_SEQ AS RETURN_SEQ
                            , CTGRI.CATEGORY_NAME1
                            , T_ECRET.ITEM_ID
                            , MIS.ITEM_NAME
                            , T_ECRET.ITEM_COLOR_ID
                            , MC.ITEM_COLOR_NAME
                            , T_ECRET.ITEM_SIZE_ID
                            , MS.ITEM_SIZE_NAME
                            , T_ECRET.JAN
                            , A_ECSHP.RESULT_QTY
                            , T_ECRET.RETURN_QTY AS RETURN_QTY
                         FROM T_ECRETURN_RESULTS T_ECRET
                   INNER JOIN A_ECSHIPS A_ECSHP
                           ON A_ECSHP.SHIPPER_ID = T_ECRET.SHIPPER_ID
                          AND A_ECSHP.CENTER_ID = T_ECRET.CENTER_ID
                          AND A_ECSHP.SHIP_INSTRUCT_ID = T_ECRET.SHIP_INSTRUCT_ID
                          AND A_ECSHP.SHIP_INSTRUCT_SEQ = T_ECRET.SHIP_INSTRUCT_SEQ
                    LEFT JOIN M_ITEM_SKU MIS
                           ON T_ECRET.SHIPPER_ID = MIS.SHIPPER_ID
                          AND A_ECSHP.ITEM_SKU_ID = MIS.ITEM_SKU_ID
              LEFT OUTER JOIN M_BRANDS MB
                           ON MIS.BRAND_ID = MB.BRAND_ID
                          AND MIS.SHIPPER_ID = MB.SHIPPER_ID
              LEFT OUTER JOIN M_COLORS MC
                           ON T_ECRET.SHIPPER_ID = MC.SHIPPER_ID
                          AND T_ECRET.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
              LEFT OUTER JOIN M_SIZES MS
                           ON T_ECRET.SHIPPER_ID = MS.SHIPPER_ID
                          AND T_ECRET.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
              LEFT OUTER JOIN M_ITEM_CATEGORIES4 CTGRI
                           ON CTGRI.CATEGORY_ID1 = MIS.CATEGORY_ID1
                          AND CTGRI.CATEGORY_ID2 = MIS.CATEGORY_ID2
                          AND CTGRI.CATEGORY_ID3 = MIS.CATEGORY_ID3
                          AND CTGRI.CATEGORY_ID4 = MIS.CATEGORY_ID4
                          AND CTGRI.SHIPPER_ID = MIS.SHIPPER_ID
                        WHERE A_ECSHP.CENTER_ID = :CENTER_ID
                          AND A_ECSHP.SHIPPER_ID = :SHIPPER_ID");
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
                     ORDER BY  T_ECRET.ARRIVE_DATE
                             , A_ECSHP.SHIP_INSTRUCT_ID
                             , T_ECRET.RETURN_ID
                             , T_ECRET.RETURN_SEQ
                         ");
            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<EcReferenceReport>(query.ToString(), parameters);
        }
    }
}