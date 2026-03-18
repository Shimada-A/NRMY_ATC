namespace Wms.Areas.Master.ViewModels.SyukkasakiSearchModal
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Helpers;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.SyukkasakiSearchModal.SyukkasakiSearchCondition;

    public class SyukkasakiModalQuery
    {
        /// <summary>
        /// 検索結果取得
        /// </summary>
        /// <param name="condition">画面検索条件</param>
        /// <param name="pageSize">最大ページサイズ</param>
        /// <returns>検索結果</returns>
        public IPagedList<SyukkasakiViewModel> Listing(SyukkasakiSearchCondition conditions, int pageSize)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", Profile.User.CenterId);

            string SHIP_TO_STORE_ID = string.Empty;
            if (!string.IsNullOrEmpty(conditions.StoreId))
            {
                SHIP_TO_STORE_ID = " AND T.SHIP_TO_STORE_ID LIKE :SHIP_TO_STORE_ID || '%' ";
                parameters.Add(":SHIP_TO_STORE_ID", conditions.StoreId);
            }

            string SHIP_TO_STORE_NAME1 = string.Empty;
            if (!string.IsNullOrEmpty(conditions.StoreName))
            {
                SHIP_TO_STORE_NAME1 = " AND T.SHIP_TO_STORE_NAME1 LIKE '%' || :SHIP_TO_STORE_NAME1 || '%' ";
                parameters.Add(":SHIP_TO_STORE_NAME1", conditions.StoreName);
            }

            string SHIP_TO_STORE_CLASS = string.Empty;
            if (!string.IsNullOrEmpty(conditions.StoreClass))
            {
                SHIP_TO_STORE_CLASS = " AND T.SHIP_TO_STORE_CLASS = :SHIP_TO_STORE_CLASS ";
                parameters.Add(":SHIP_TO_STORE_CLASS", conditions.StoreClass);
            }

            string areas = string.Empty;
            string AREA_ID = string.Empty;

            if (conditions.AreaItem.Where(x => x.IsCheck).Any())
            {
                AREA_ID = $" AND EXISTS (SELECT 'X' FROM M_DELIAREA_GROUP D WHERE D.DELIAREA_GROUP_ID IN :AREAS AND D.PREF_ID = T.SHIP_TO_PREF_ID )";
                parameters.Add(":AREAS", conditions.AreaItem.Where(x => x.IsCheck).Select(x => x.AreaId).ToArray());
            }

            StringBuilder query = new StringBuilder();
            query.AppendLine(" SELECT T.SHIP_TO_STORE_ID SHIP_TO_STORE_ID,");
            query.AppendLine("        T.SHIP_TO_STORE_NAME1 SHIP_TO_STORE_NAME,");
            query.AppendLine("        T.SHIP_TO_STORE_CLASS SHIP_TO_STORE_CLASS,");
            query.AppendLine("        M.PREF_NAME PREF_NAME");
            query.AppendLine("   FROM V_SHIP_TO_STORES T");
            query.AppendLine("   LEFT JOIN M_PREFS M ON T.SHIP_TO_PREF_ID = M.PREF_ID AND M.SHIPPER_ID = T.SHIPPER_ID");
            query.AppendLine("  WHERE T.SHIPPER_ID = :SHIPPER_ID");
            query.AppendLine("    AND ( ");
            query.AppendLine("          T.SHIP_TO_CLOSE_DATE IS NULL ");
            query.AppendLine("       OR T.SHIP_TO_CLOSE_DATE > TO_DATE(TO_CHAR(SYSDATE, 'YYYY/MM/DD')) ");
            query.AppendLine("    ) ");
            query.AppendLine("  " + SHIP_TO_STORE_ID);
            query.AppendLine("  " + SHIP_TO_STORE_NAME1);
            query.AppendLine("  " + SHIP_TO_STORE_CLASS);
            query.AppendLine("  " + AREA_ID);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<SyukkasakiViewModel>(query.ToString(), parameters).Count();

            string strSort = " ASC ";
            if (conditions.Sort == AscDescSort.Desc)
            {
                strSort = " DESC ";
            }

            string strSortKey = string.Empty;
            switch (conditions.SortKey)
            {
                case StoreSortKey.ShipToStoreId:
                    strSortKey = " T.SHIP_TO_STORE_ID ";
                    break;
                case StoreSortKey.AeraId:
                    strSortKey = " T.SHIP_TO_PREF_ID ";
                    break;
                default:
                    break;
            }

            query.AppendLine("  ORDER BY " + strSortKey + strSort);
            query.AppendLine("  OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", pageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (conditions.Page - 1) * pageSize });

            // Fill data to memory
            List<SyukkasakiViewModel> rs = MvcDbContext.Current.Database.Connection.Query<SyukkasakiViewModel>(query.ToString(), parameters).ToList();
            var storeIds = string.IsNullOrWhiteSpace(conditions.TempStoreId) ? null : conditions.TempStoreId.Split(',');
            rs = rs.Select(x => { x.IsCheck = storeIds == null ? false : (storeIds.Contains(x.SHIP_TO_STORE_ID) ? true : false); return x; }).ToList();
            // Excute paging
            return new StaticPagedList<SyukkasakiViewModel>(rs, (int)conditions.Page, pageSize, totalCount);
        }

        public List<string> GetPKey(SyukkasakiSearchCondition conditions)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", Profile.User.CenterId);

            string SHIP_TO_STORE_ID = string.Empty;
            if (!string.IsNullOrEmpty(conditions.StoreId))
            {
                SHIP_TO_STORE_ID = " AND T.SHIP_TO_STORE_ID LIKE :SHIP_TO_STORE_ID || '%' ";
                parameters.Add(":SHIP_TO_STORE_ID", conditions.StoreId);
            }

            string SHIP_TO_STORE_NAME1 = string.Empty;
            if (!string.IsNullOrEmpty(conditions.StoreName))
            {
                SHIP_TO_STORE_NAME1 = " AND T.SHIP_TO_STORE_NAME1 LIKE :SHIP_TO_STORE_NAME1 || '%' ";
                parameters.Add(":SHIP_TO_STORE_NAME1", conditions.StoreName);
            }

            string SHIP_TO_STORE_CLASS = string.Empty;
            if (!string.IsNullOrEmpty(conditions.StoreClass))
            {
                SHIP_TO_STORE_CLASS = " AND T.SHIP_TO_STORE_CLASS = :SHIP_TO_STORE_CLASS ";
                parameters.Add(":SHIP_TO_STORE_CLASS", conditions.StoreClass);
            }

            string areas = string.Empty;
            string AREA_ID = string.Empty;
            int a_count = 0;
            if (conditions.AreaItem != null)
            {
                foreach (var item in conditions.AreaItem)
                {
                    if (item.IsCheck)
                    {
                        if (a_count == 0)
                        {
                            areas = item.AreaId;
                        }
                        else
                        {
                            areas += ", " + item.AreaId;
                        }

                        a_count++;
                    }
                }
            }

            if (a_count > 0)
            {
                AREA_ID = $" AND EXISTS (SELECT 'X' FROM M_DELIAREA_GROUP D WHERE D.DELIAREA_GROUP_ID IN ({areas}) AND D.PREF_ID = T.SHIP_TO_PREF_ID )";
            }

            string strSort = " ASC ";
            if (conditions.Sort == AscDescSort.Desc)
            {
                strSort = " DESC ";
            }

            string strSortKey = string.Empty;
            switch (conditions.SortKey)
            {
                case StoreSortKey.ShipToStoreId:
                    strSortKey = " T.SHIP_TO_STORE_ID ";
                    break;
                case StoreSortKey.AeraId:
                    strSortKey = " T.SHIP_TO_PREF_ID ";
                    break;
                default:
                    break;
            }

            StringBuilder query = new StringBuilder();

            query.AppendLine("SELECT  T.SHIP_TO_STORE_ID || T.SHIP_TO_STORE_CLASS  FROM V_SHIP_TO_STORES T ");
            query.AppendLine("  WHERE T.SHIPPER_ID = :SHIPPER_ID");
            query.AppendLine("    AND ( ");
            query.AppendLine("          T.SHIP_TO_CLOSE_DATE IS NULL ");
            query.AppendLine("       OR T.SHIP_TO_CLOSE_DATE > TO_DATE(TO_CHAR(SYSDATE, 'YYYY/MM/DD')) ");
            query.AppendLine("    ) ");
            query.AppendLine("  " + SHIP_TO_STORE_ID);
            query.AppendLine("  " + SHIP_TO_STORE_NAME1);
            query.AppendLine("  " + SHIP_TO_STORE_CLASS);
            query.AppendLine("  " + AREA_ID);
            query.AppendLine("  ORDER BY " + strSortKey + strSort);

            return MvcDbContext.Current.Database.Connection.Query<string>(query.ToString(), parameters).ToList();

        }

        /// <summary>
        /// エリアList
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetAreaList()
        {
            string gen_div_cd = "STORE_SELECT_DIALOG_AREA_G";
            return MvcDbContext.Current.Generals.Where(d => d.ShipperId == Profile.User.ShipperId && d.CenterId == Profile.User.CenterId && d.GenDivCd == gen_div_cd && d.RegisterDiviCd == "1")
                .Select(m => new SelectListItem
                {
                    Value = m.GenCd.ToString(),
                    Text = m.GenName,
                }).Distinct().OrderBy(m => m.Value);
        }
    }
}