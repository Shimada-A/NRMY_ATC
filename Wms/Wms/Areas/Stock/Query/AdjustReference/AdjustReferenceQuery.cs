using Dapper;
using Microsoft.Owin.Security.Provider;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Wms.Areas.Stock.ViewModels.AdjustReference;
using Wms.Common;
using Wms.Models;
using static Wms.Areas.Stock.ViewModels.AdjustReference.AdjustReferenceSearchConditions;

namespace Wms.Areas.Stock.Query.AdjustReference
{
    public static class AdjustReferenceQuery
    {
        private const string GenCenterId = "@@@";

        private const string GenRegisterDivCode = "1";

        private const string GenDivCodeStockAdjustReason = "STOCK_ADJUST_REASON";

        private const string GenDivCodeCenterChangeLevel = "CENTER_CHANGE_LEVEL";

        public static bool CanChangeCenter()
        {
            try
            {
                var permissionLevel = ((int)Profile.User.PermissionLevel).ToString();
                var genList = MvcDbContext.Current.Generals.Where(
                    gen => gen.ShipperId == Profile.User.ShipperId &&
                    gen.CenterId == GenCenterId &&
                    gen.RegisterDiviCd == GenRegisterDivCode &&
                    gen.GenDivCd == GenDivCodeCenterChangeLevel &&
                    gen.GenCd == permissionLevel);

                return genList != null && genList.ToList().Count > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            };

        }

        public static IEnumerable<SelectListItem> GetSelectListStockAdjustReason()
        {
            return MvcDbContext.Current.Generals.Where(
                gen => gen.ShipperId == Profile.User.ShipperId &&
                gen.CenterId == GenCenterId &&
                gen.RegisterDiviCd == GenRegisterDivCode &&
                gen.GenDivCd == GenDivCodeStockAdjustReason).Select(
                gen => new SelectListItem
                {
                    Value = gen.GenCd,
                    Text = gen.GenCd + ":" + gen.GenName
                }).OrderBy(item => item.Value);
        }

        public static List<AdjustReferenceResultRow> GetResultRowList([Required] AdjustReferenceSearchConditions condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            var query = new StringBuilder();

            query.AppendLine("SELECT");
            query.AppendLine("ROW_NUMBER() OVER (ORDER BY @ORDER_BY@) AS ROW_NUMBER,");
            query.AppendLine("TSA.SLIP_NO AS ADJUST_NUMBER,");
            query.AppendLine("TSA.ITEM_SKU_ID AS SKU,");
            query.AppendLine("TSA.JAN AS JAN,");
            query.AppendLine("TSA.ITEM_ID AS ITEM_ID,");
            query.AppendLine("TSA.ITEM_COLOR_ID AS COLOR_ID,");
            query.AppendLine("TSA.ITEM_SIZE_ID AS SIZE_ID,");
            query.AppendLine("TSA.NOTE AS NOTE,");
            query.AppendLine("TSA.ADJUST_DATE AS ADJUST_DATE,");
            query.AppendLine("TSA.LOCATION_CD_FROM AS LOCATION_CD_FROM,");
            query.AppendLine("TSA.LOCATION_CD_TO AS LOCATION_CD_TO,");
            query.AppendLine("TSA.BOX_NO_FROM AS BOX_NO_FROM,");
            query.AppendLine("TSA.BOX_NO_TO AS BOX_NO_TO,");
            query.AppendLine("TSA.ADJUST_QTY_TO AS ADJUST_QUANTITY_TO,");
            query.AppendLine("TSA.ADJUST_REASON_CD AS ADJUST_REASON_CD,");
            query.AppendLine("TSA.UPDATE_USER_ID AS UPDATE_USER_ID,");
            query.AppendLine("MIS.CATEGORY_ID1 AS CATEGORY_ID1,");
            query.AppendLine("MIS.ITEM_SHORT_NAME AS ITEM_NAME,");
            query.AppendLine("MIC.CATEGORY_NAME1 AS CATEGORY_NAME1,");
            query.AppendLine("MCO.ITEM_COLOR_NAME AS COLOR_NAME,");
            query.AppendLine("MIS.ITEM_SIZE_NAME AS SIZE_NAME,");
            query.AppendLine("MGR1.GRADE_NAME AS GRADE_NAME_FROM,");
            query.AppendLine("MGR2.GRADE_NAME AS GRADE_NAME_TO,");
            query.AppendLine("MUS.USER_NAME AS USER_NAME");

            query.AppendLine(GetFromWherePhrase(condition));

            query.AppendLine("OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");

            query.Replace("@ORDER_BY@", GetOrderBy(condition));

            var ret = MvcDbContext.Current.Database.Connection.Query<AdjustReferenceResultRow>(query.ToString(), CreateParam(condition)).ToList();
            var reasonList = GetSelectListStockAdjustReason().ToList();

            foreach (var row in ret)
            {
                var reason = reasonList.Find((val) => val.Value == row.AdjustReasonCd.ToString());

                row.AdjustReasonName = reason != null ? reason.Text : row.AdjustReasonCd.ToString();
            }

            return ret;
        }

        public static int CountResultRowList([Required] AdjustReferenceSearchConditions condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            var query = new StringBuilder();

            query.AppendLine("SELECT");
            query.AppendLine("COUNT(*)");

            query.AppendLine(GetFromWherePhrase(condition));

            return MvcDbContext.Current.Database.Connection.ExecuteScalar<int>(query.ToString(), CreateParam(condition));
        }

        private static string GetFromWherePhrase(AdjustReferenceSearchConditions condition)
        {
            var query = new StringBuilder();

            query.AppendLine("FROM");
            query.AppendLine("T_STOCK_ADJUST TSA");
            query.AppendLine("LEFT OUTER JOIN M_ITEM_SKU MIS ON");
            query.AppendLine("TSA.SHIPPER_ID = MIS.SHIPPER_ID AND");
            query.AppendLine("TSA.ITEM_SKU_ID = MIS.ITEM_SKU_ID");
            query.AppendLine("LEFT OUTER JOIN M_ITEM_CATEGORIES4 MIC ON");
            query.AppendLine("MIS.SHIPPER_ID = MIC.SHIPPER_ID AND");
            query.AppendLine("MIS.CATEGORY_ID1 = MIC.CATEGORY_ID1 AND");
            query.AppendLine("MIS.CATEGORY_ID2 = MIC.CATEGORY_ID2 AND");
            query.AppendLine("MIS.CATEGORY_ID3 = MIC.CATEGORY_ID3 AND");
            query.AppendLine("MIS.CATEGORY_ID4 = MIC.CATEGORY_ID4");
            query.AppendLine("LEFT OUTER JOIN M_COLORS MCO ON");
            query.AppendLine("TSA.SHIPPER_ID = MCO.SHIPPER_ID AND");
            query.AppendLine("TSA.ITEM_COLOR_ID = MCO.ITEM_COLOR_ID");
            query.AppendLine("LEFT OUTER JOIN M_GRADES MGR1 ON");
            query.AppendLine("TSA.SHIPPER_ID = MGR1.SHIPPER_ID AND");
            query.AppendLine("TSA.GRADE_ID_FROM = MGR1.GRADE_ID");
            query.AppendLine("LEFT OUTER JOIN M_GRADES MGR2 ON");
            query.AppendLine("TSA.SHIPPER_ID = MGR2.SHIPPER_ID AND");
            query.AppendLine("TSA.GRADE_ID_TO = MGR2.GRADE_ID");
            query.AppendLine("LEFT OUTER JOIN M_BRANDS MBR ON");
            query.AppendLine("MIS.SHIPPER_ID = MBR.SHIPPER_ID AND");
            query.AppendLine("MIS.BRAND_ID = MBR.BRAND_ID");
            query.AppendLine("LEFT OUTER JOIN M_VENDORS MVE ON");
            query.AppendLine("MIS.SHIPPER_ID = MVE.SHIPPER_ID AND");
            query.AppendLine("MIS.MAIN_VENDOR_ID = MVE.VENDOR_ID");
            query.AppendLine("LEFT OUTER JOIN M_USERS MUS ON");
            query.AppendLine("MUS.SHIPPER_ID = TSA.SHIPPER_ID AND");
            query.AppendLine("MUS.USER_ID = TSA.UPDATE_USER_ID");
            query.AppendLine("WHERE");
            query.AppendLine("TSA.SHIPPER_ID = :SHIPPER_ID AND");

            AppendCondition(query, "TSA.ADJUST_REASON_CD = :ADJUST_REASON_CD AND", condition.AdjustReasonCd);
            AppendCondition(query, "TRUNC(TSA.ADJUST_DATE, 'DD') >= :ADJUST_DATE_FROM AND", condition.StockAdjustDateFrom);
            AppendCondition(query, "TRUNC(TSA.ADJUST_DATE, 'DD') <= :ADJUST_DATE_TO AND", condition.StockAdjustDateTo);
            AppendCondition(query, "TSA.SLIP_NO >= :SLIP_NO_FROM AND", condition.StockAdjustNumberFrom);
            AppendCondition(query, "TSA.SLIP_NO <= :SLIP_NO_TO AND", condition.StockAdjustNumberTo);
            AppendCondition(query, "TSA.JAN LIKE :JAN AND", condition.Jan);
            AppendCondition(query, "TSA.ITEM_ID LIKE :ITEM_ID AND", condition.ItemId);
            AppendCondition(query, "MCO.ITEM_COLOR_ID LIKE :ITEM_COLOR_ID AND", condition.ColorId);
            AppendCondition(query, "MCO.ITEM_COLOR_NAME LIKE :ITEM_COLOR_NAME AND", condition.ColorName);
            AppendCondition(query, "MIS.ITEM_SIZE_ID LIKE :ITEM_SIZE_ID AND", condition.SizeId);
            AppendCondition(query, "MIS.ITEM_SIZE_NAME LIKE :ITEM_SIZE_NAME AND", condition.SizeName);
            AppendCondition(query, "TSA.ITEM_SKU_ID LIKE :ITEM_SKU_ID AND", condition.Sku);
            AppendCondition(query, "MIS.DIVISION_ID = :DIVISION_ID AND", condition.DivisionId);
            AppendCondition(query, "MBR.BRAND_ID = :BRAND_ID AND", condition.BrandId);
            AppendCondition(query, "MBR.BRAND_NAME LIKE :BRAND_NAME AND", condition.BrandName);
            AppendCondition(query, "MVE.VENDOR_ID LIKE :VENDOR_ID AND", condition.VendorId);
            AppendCondition(query, "MVE.VENDOR_NAME1 LIKE :VENDOR_NAME1 AND", condition.VendorName);
            AppendCondition(query, "MIS.CATEGORY_ID1 = :CATEGORY_ID1 AND", condition.CategoryId1);
            AppendCondition(query, "MIS.CATEGORY_ID2 = :CATEGORY_ID2 AND", condition.CategoryId2);
            AppendCondition(query, "MIS.CATEGORY_ID3 = :CATEGORY_ID3 AND", condition.CategoryId3);
            AppendCondition(query, "MIS.CATEGORY_ID4 = :CATEGORY_ID4 AND", condition.CategoryId4);
            AppendCondition(query, "TSA.LOCATION_CD_FROM = :LOCATION_CD_FROM AND", condition.LocationCdFrom);
            AppendCondition(query, "TSA.LOCATION_CD_TO = :LOCATION_CD_TO AND", condition.LocationCdTo);
            AppendCondition(query, "MIS.ITEM_CODE = :ITEM_CODE AND", condition.ItemCode);

            query.AppendLine("TSA.CENTER_ID = :CENTER_ID");

            return query.ToString();
        }

        private static DynamicParameters CreateParam(AdjustReferenceSearchConditions condition)
        {
            var param = new DynamicParameters();

            param.AddDynamicParams(new
            {
                SHIPPER_ID = Profile.User.ShipperId,
                CENTER_ID = condition.CenterId,
                ADJUST_REASON_CD = condition.AdjustReasonCd,
                ADJUST_DATE_FROM = condition.StockAdjustDateFrom,
                ADJUST_DATE_TO = condition.StockAdjustDateTo,
                SLIP_NO_FROM = condition.StockAdjustNumberFrom,
                SLIP_NO_TO = condition.StockAdjustNumberTo,
                JAN = AddPrefixMatch(condition.Jan),
                ITEM_ID = AddPrefixMatch(condition.ItemId),
                ITEM_COLOR_ID = AddPrefixMatch(condition.ColorId),
                ITEM_COLOR_NAME = AddPrefixMatch(condition.ColorName),
                ITEM_SIZE_ID = AddPrefixMatch(condition.SizeId),
                ITEM_SIZE_NAME = AddPrefixMatch(condition.SizeName),
                ITEM_SKU_ID = AddPrefixMatch(condition.Sku),
                DIVISION_ID = condition.DivisionId,
                BRAND_ID = condition.BrandId,
                BRAND_NAME = AddPrefixMatch(condition.BrandName),
                VENDOR_ID = AddPrefixMatch(condition.VendorId),
                VENDOR_NAME1 = AddPrefixMatch(condition.VendorName),
                CATEGORY_ID1 = condition.CategoryId1,
                CATEGORY_ID2 = condition.CategoryId2,
                CATEGORY_ID3 = condition.CategoryId3,
                CATEGORY_ID4 = condition.CategoryId4,
                ITEM_CODE = condition.ItemCode,
                LOCATION_CD_FROM = condition.LocationCdFrom,
                LOCATION_CD_TO = condition.LocationCdTo,
                OFFSET = condition.GetOffset(),
                PAGE_SIZE = condition.PageSize,
            });

            return param;
        }

        private static void AppendCondition(StringBuilder query, string phrase, string condition)
        {
            if (string.IsNullOrEmpty(condition))
            {
                return;
            }

            query.AppendLine(phrase);
        }

        private static void AppendCondition(StringBuilder query, string phrase, DateTime? condition)
        {
            if (condition == null)
            {
                return;
            }

            query.AppendLine(phrase);
        }

        private static string AddPrefixMatch(string val)
        {
            return string.Format("{0}%", val);
        }

        private static string GetOrderBy(AdjustReferenceSearchConditions condition)
        {
            const string SortOrderDate = "TSA.ADJUST_DATE";
            const string SortOrderNumber = "TSA.SLIP_NO";
            const string SortOrderSku = "TSA.ITEM_SKU_ID";
            var orderBy = "{1} {0}, {2} {0}, {3} {0}";
            var ascOrDesc = condition.AscDescSort == AdjustReferenceAscDescSort.Asc ? "ASC" : "DESC";

            if (condition.SortOrder == AdjustReferenceSortOrder.DateNumberSku)
            {
                return string.Format(orderBy, ascOrDesc, SortOrderDate, SortOrderNumber, SortOrderSku);
            }
            else if (condition.SortOrder == AdjustReferenceSortOrder.DateSkuNumber)
            {
                return string.Format(orderBy, ascOrDesc, SortOrderDate, SortOrderSku, SortOrderNumber);
            }
            else
            {
                return string.Format(orderBy, ascOrDesc, SortOrderSku, SortOrderDate, SortOrderNumber);
            }
        }
    }
}