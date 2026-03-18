namespace Wms.Areas.Master.ViewModels.SyukkasakiTransporterSearchModal
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.SyukkasakiTransporterSearchModal.SyukkasakiTransporterSearchCondition;

    public partial class SyukkasakiTransporterViewModel
    {
        /// <summary>
        /// 検索結果取得
        /// </summary>
        /// <param name="condition">画面検索条件</param>
        /// <param name="pageSize">最大ページサイズ</param>
        /// <returns>検索結果</returns>
        public IPagedList<SyukkasakiTransporterViewModel> Listing(SyukkasakiTransporterSearchCondition conditions, int pageSize)
        {
            DynamicParameters parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                WITH
                    TARGET_LOC_TRANSPORTERS AS(
                        SELECT
                                SHIPPER_ID
                            ,   CENTER_ID
                            ,   SHIP_TO_STORE_ID
                            ,   MAX(START_DATE) AS NEW_START_DATE
                        FROM
                                M_LOC_TRANSPORTERS
                        WHERE
                                START_DATE <= TRUNC(SYSDATE)
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                        GROUP BY
                                SHIP_TO_STORE_ID
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                    )
                SELECT
                        VS.SHIP_TO_STORE_CLASS
                    ,   ML.SHIP_TO_STORE_ID
                    ,   VS.SHIP_TO_STORE_NAME1 SHIP_TO_STORE_NAME
                    ,   MP.PREF_NAME
                    ,   ML.TRANSPORTER_ID
                    ,   ML.TRANSPORTER_ID_MON
                    ,   ML.TRANSPORTER_ID_TUE
                    ,   ML.TRANSPORTER_ID_WED
                    ,   ML.TRANSPORTER_ID_THU
                    ,   ML.TRANSPORTER_ID_FRI
                    ,   ML.TRANSPORTER_ID_SAT
                    ,   ML.TRANSPORTER_ID_SUN
                    ,   ML.TRANSPORTER_ID_HOL
                    ,   (SELECT MT.TRANSPORTER_SHORT_NAME FROM M_TRANSPORTERS MT WHERE MT.SHIPPER_ID = ML.SHIPPER_ID AND MT.TRANSPORTER_ID = ML.TRANSPORTER_ID ) TRANSPORTER_NAME
                    ,   (SELECT MT.TRANSPORTER_SHORT_NAME FROM M_TRANSPORTERS MT WHERE MT.SHIPPER_ID = ML.SHIPPER_ID AND MT.TRANSPORTER_ID = ML.TRANSPORTER_ID_MON ) TRANSPORTER_NAME_MON
                    ,   (SELECT MT.TRANSPORTER_SHORT_NAME FROM M_TRANSPORTERS MT WHERE MT.SHIPPER_ID = ML.SHIPPER_ID AND MT.TRANSPORTER_ID = ML.TRANSPORTER_ID_TUE ) TRANSPORTER_NAME_TUE
                    ,   (SELECT MT.TRANSPORTER_SHORT_NAME FROM M_TRANSPORTERS MT WHERE MT.SHIPPER_ID = ML.SHIPPER_ID AND MT.TRANSPORTER_ID = ML.TRANSPORTER_ID_WED ) TRANSPORTER_NAME_WED
                    ,   (SELECT MT.TRANSPORTER_SHORT_NAME FROM M_TRANSPORTERS MT WHERE MT.SHIPPER_ID = ML.SHIPPER_ID AND MT.TRANSPORTER_ID = ML.TRANSPORTER_ID_THU ) TRANSPORTER_NAME_THU
                    ,   (SELECT MT.TRANSPORTER_SHORT_NAME FROM M_TRANSPORTERS MT WHERE MT.SHIPPER_ID = ML.SHIPPER_ID AND MT.TRANSPORTER_ID = ML.TRANSPORTER_ID_FRI ) TRANSPORTER_NAME_FRI
                    ,   (SELECT MT.TRANSPORTER_SHORT_NAME FROM M_TRANSPORTERS MT WHERE MT.SHIPPER_ID = ML.SHIPPER_ID AND MT.TRANSPORTER_ID = ML.TRANSPORTER_ID_SAT ) TRANSPORTER_NAME_SAT
                    ,   (SELECT MT.TRANSPORTER_SHORT_NAME FROM M_TRANSPORTERS MT WHERE MT.SHIPPER_ID = ML.SHIPPER_ID AND MT.TRANSPORTER_ID = ML.TRANSPORTER_ID_SUN ) TRANSPORTER_NAME_SUN
                    ,   (SELECT MT.TRANSPORTER_SHORT_NAME FROM M_TRANSPORTERS MT WHERE MT.SHIPPER_ID = ML.SHIPPER_ID AND MT.TRANSPORTER_ID = ML.TRANSPORTER_ID_HOL ) TRANSPORTER_NAME_HOL
                FROM
                        M_LOC_TRANSPORTERS ML
                INNER JOIN
                        TARGET_LOC_TRANSPORTERS TGT
                ON
                        TGT.SHIP_TO_STORE_ID = ML.SHIP_TO_STORE_ID
                    AND TGT.NEW_START_DATE = ML.START_DATE
                    AND TGT.CENTER_ID = ML.CENTER_ID
                    AND TGT.SHIPPER_ID = ML.SHIPPER_ID
                INNER JOIN
                        V_SHIP_TO_STORES VS
                ON
                        VS.SHIPPER_ID = ML.SHIPPER_ID
                    AND VS.SHIP_TO_STORE_ID = ML.SHIP_TO_STORE_ID
            ");
            if (!conditions.IsCenterOnly)
            {
                sql.AppendLine($@"
                    INNER JOIN (
                        SELECT DISTINCT
                                SHIPPER_ID
                            ,   CENTER_ID
                            ,   STORE_ID
                        FROM
                                M_SHIP_FRONTAGE
                        WHERE
                                SHIPPER_ID = :SHIPPER_ID
                            AND CENTER_ID = :CENTER_ID
                            {((!string.IsNullOrEmpty(conditions.BrandId)) ? "AND BRAND_ID = :BRAND_ID" : null)}
                    ) MSF
                    ON
                            MSF.CENTER_ID = ML.CENTER_ID
                        AND MSF.SHIPPER_ID = ML.SHIPPER_ID
                        AND MSF.STORE_ID = ML.SHIP_TO_STORE_ID
                ");
            }
            sql.AppendLine(@"
                LEFT JOIN
                        M_PREFS MP
                ON
                        MP.PREF_ID = VS.SHIP_TO_PREF_ID
                    AND MP.SHIPPER_ID = VS.SHIPPER_ID
                WHERE
                        1 = 1
            ");
            parameters.Add(":CENTER_ID", conditions.CenterId);
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":BRAND_ID", conditions.BrandId);

            if (!string.IsNullOrEmpty(conditions.StoreClass))
            {
                sql.AppendLine(" AND VS.SHIP_TO_STORE_CLASS = :SHIP_TO_STORE_CLASS ");
                parameters.Add(":SHIP_TO_STORE_CLASS", conditions.StoreClass);
            }

            if (!string.IsNullOrEmpty(conditions.StoreRanks))
            {
                sql.AppendLine(" AND VS.SHIP_TO_RANK_ID = :STORE_RANK_ID ");
                switch (conditions.StoreRanks)
                {
                    case "S":
                        parameters.Add(":STORE_RANK_ID", "1");
                        break;
                    case "A":
                        parameters.Add(":STORE_RANK_ID", "2");
                        break;
                    case "B":
                        parameters.Add(":STORE_RANK_ID", "3");
                        break;
                    case "C":
                        parameters.Add(":STORE_RANK_ID", "4");
                        break;
                    case "D":
                        parameters.Add(":STORE_RANK_ID", "5");
                        break;
                    default:
                        parameters.Add(":STORE_RANK_ID", "6");
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(conditions.ShipToStoreId))
            {
                sql.AppendLine(" AND ML.SHIP_TO_STORE_ID LIKE :SHIP_TO_STORE_ID ");
                parameters.Add(":SHIP_TO_STORE_ID", "%" + conditions.ShipToStoreId + "%");
            }

            if (string.IsNullOrWhiteSpace(conditions.ShipToStoreId) && !string.IsNullOrWhiteSpace(conditions.ShipToStoreName))
            {
                sql.AppendLine(" AND VS.SHIP_TO_STORE_NAME1 LIKE :SHIP_TO_STORE_NAME1 ");
                parameters.Add(":SHIP_TO_STORE_NAME1", "%" + conditions.ShipToStoreName + "%");
            }

            if (conditions.AreaItem.Where(x => x.IsCheck).Any())
            {
                sql.AppendLine(" AND EXISTS (SELECT 'X' FROM M_DELIAREA_GROUP D WHERE D.DELIAREA_GROUP_ID IN :AREAS AND D.PREF_ID = VS.SHIP_TO_PREF_ID )");
                parameters.Add(":AREAS", conditions.AreaItem.Where(x => x.IsCheck).Select(x => x.AreaId).ToArray());
            }

            if (!string.IsNullOrWhiteSpace(conditions.TransporterId))
            {
                sql.AppendLine(" AND ML.TRANSPORTER_ID LIKE :TRANSPORTER_ID ");
                parameters.Add(":TRANSPORTER_ID", conditions.TransporterId);
            }

            if (conditions.StoreOutletsClass == StoreOutletsClasses.NotOutlets)
            {
                sql.AppendLine(" AND NOT EXISTS (SELECT 1 FROM M_GENERALS SUB WHERE SUB.SHIPPER_ID = :SHIPPER_ID AND SUB.CENTER_ID  = '@@@' AND SUB.GEN_DIV_CD = 'EVENT_STORE_CODE' AND SUB.REGISTER_DIVI_CD ＝ '1' AND SUB.GEN_CD = ML.SHIP_TO_STORE_ID )");
            }
            else if (conditions.StoreOutletsClass == StoreOutletsClasses.OnlyOutlets)
            {
                sql.AppendLine(" AND EXISTS (SELECT 1 FROM M_GENERALS SUB WHERE SUB.SHIPPER_ID = :SHIPPER_ID AND SUB.CENTER_ID  = '@@@' AND SUB.GEN_DIV_CD = 'EVENT_STORE_CODE' AND SUB.REGISTER_DIVI_CD ＝ '1' AND SUB.GEN_CD = ML.SHIP_TO_STORE_ID )");
            }

            sql.AppendLine("AND VS.DELETE_FLAG = 0");
            // 全レコード数を取得
            var totalCount = MvcDbContext.Current.Database.Connection.Query<SyukkasakiTransporterViewModel>(sql.ToString(), parameters).Count();

            switch (conditions.SortKey)
            {
                case StoreSortKey.ShipToStoreId:
                    sql.AppendLine(" ORDER BY ");
                    switch (conditions.OrderKey)
                    {
                        case AscDescSort.Desc:
                            sql.AppendLine("    ML.SHIP_TO_STORE_ID DESC, VS.SHIP_TO_STORE_NAME1 DESC ");
                            break;
                        default:
                            sql.AppendLine("    ML.SHIP_TO_STORE_ID ASC, VS.SHIP_TO_STORE_NAME1 ASC ");
                            break;
                    }

                    break;
                default:
                    sql.AppendLine(" ORDER BY ");
                    switch (conditions.OrderKey)
                    {
                        case AscDescSort.Desc:
                            sql.AppendLine("    VS.SHIP_TO_STORE_NAME1 DESC, ML.SHIP_TO_STORE_ID DESC ");
                            break;
                        default:
                            sql.AppendLine("    VS.SHIP_TO_STORE_NAME1 ASC, ML.SHIP_TO_STORE_ID ASC ");
                            break;
                    }

                    break;
            }

            sql.AppendLine("OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");

            parameters.AddDynamicParams(new { OFFSET = (conditions.Page - 1) * pageSize });
            parameters.AddDynamicParams(new { PAGE_SIZE = pageSize });

            var SyukkasakiTransporter = MvcDbContext.Current.Database.Connection.Query<SyukkasakiTransporterViewModel>(sql.ToString(), parameters).ToList();

            return new StaticPagedList<SyukkasakiTransporterViewModel>(SyukkasakiTransporter, (int)conditions.Page, pageSize, totalCount);
        }

        /// <summary>
        /// エリアList
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetAreaList(bool selected)
        {
            string gen_div_cd = "STORE_SELECT_DIALOG_AREA_G";
            return MvcDbContext.Current.Generals.Where(d => d.ShipperId == Profile.User.ShipperId && d.CenterId == Profile.User.CenterId && d.GenDivCd == gen_div_cd && d.RegisterDiviCd == "1")
                .Select(m => new SelectListItem
                {
                    Value = m.GenCd.ToString(),
                    Text = m.GenName,
                    Selected = selected
                }).Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 配送業者データ取得
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
    }
}