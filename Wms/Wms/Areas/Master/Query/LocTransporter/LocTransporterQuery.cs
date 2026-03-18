namespace Wms.Areas.Master.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
    using PagedList;
    using Wms.Areas.Master.ViewModels.LocTransporter;
    using Wms.Areas.Ship.Resources;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.LocTransporter.LocTransporterSearchCondition;

    public partial class LocTransporter
    {
        /// <summary>
        /// 一覧画面 一覧取得
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<SearchItem> GetData(LocTransporterSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();

            // 一覧のSQLを取得
            GetQuery(condition, ref query, ref parameters);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<SearchItem>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each pageTransporterId
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            List<SearchItem> LocTransporters = MvcDbContext.Current.Database.Connection.Query<SearchItem>(query.ToString(), parameters).ToList();

            // Excute paging
            return new StaticPagedList<SearchItem>(LocTransporters, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// 一覧画面 一覧のROWIDを取得する
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<SearchItem> GetRowId(LocTransporterSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();

            // 一覧のSQLを取得
            GetQuery(condition, ref query, ref parameters);
            return MvcDbContext.Current.Database.Connection.Query<SearchItem>(query.ToString(), parameters).ToList();
        }

        /// <summary>
        /// 一覧画面で選択された行のデータを取得する
        /// </summary>
        /// <param name="rowids"></param>
        /// <returns></returns>
        public List<SearchItem> GetRows(List<string> rowids, bool isNewDate)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            var condition = new LocTransporterSearchCondition()
            {
                IsNewDate = isNewDate
            };

            // 一覧のSQLを取得
            GetQuery(condition, ref query, ref parameters);

            return MvcDbContext.Current.Database.Connection.Query<SearchItem>(query.ToString(), parameters).Where(s => rowids.Contains(s.RowId)).ToList();
        }

        /// <summary>
        /// SELECT文取得
        /// </summary>
        /// <param name="isNewDate">出荷先ごとの最新のみ表示する場合true</param>
        /// <returns>SELECT文</returns>
        public static void GetQuery(LocTransporterSearchCondition condition, ref StringBuilder query, ref DynamicParameters parameters)
        {
            if (condition.IsNewDate)
            {
                query.AppendLine(" WITH ");
                query.AppendLine("     MAX_START_DATE AS ( ");
                query.AppendLine("         SELECT ");
                query.AppendLine("                 SHIPPER_ID ");
                query.AppendLine("             ,   CENTER_ID ");
                query.AppendLine("             ,   SHIP_TO_STORE_ID ");
                query.AppendLine("             ,   MAX(START_DATE) AS START_DATE ");
                query.AppendLine("         FROM ");
                query.AppendLine("                 M_LOC_TRANSPORTERS ");
                query.AppendLine("         WHERE ");
                query.AppendLine("                 START_DATE <= SYSDATE ");
                query.AppendLine("         GROUP BY ");
                query.AppendLine("                 SHIPPER_ID ");
                query.AppendLine("             ,   CENTER_ID ");
                query.AppendLine("             ,   SHIP_TO_STORE_ID ");
                query.AppendLine(" ) ");
            }

            query.AppendLine(" SELECT ");
            query.AppendLine("         LOC_TRANSPORTER.SHIPPER_ID ");
            query.AppendLine("     ,   LOC_TRANSPORTER.CENTER_ID ");
            query.AppendLine("     ,   CENTER.CENTER_NAME1 AS CENTER_NAME ");
            query.AppendLine("     ,   LOC_TRANSPORTER.SHIP_TO_STORE_CLASS ");
            query.AppendLine("     ,   LOC_TRANSPORTER.SHIP_TO_STORE_ID ");
            query.AppendLine("     ,   STORES.SHIP_TO_STORE_NAME1 AS SHIP_TO_STORE_NAME ");
            query.AppendLine("     ,   STORES.SHIP_TO_PREF_ID AS PREF_ID ");
            query.AppendLine("     ,   PREFS.PREF_NAME ");
            query.AppendLine("     ,   TO_CHAR(LOC_TRANSPORTER.START_DATE,'YYYY/MM/DD') AS START_DATE ");
            query.AppendLine("     ,   LOC_TRANSPORTER.TRANSPORTER_ID ");
            query.AppendLine("     ,   LOC_TRANSPORTER.LEAD_TIMES ");
            query.AppendLine("     ,   LOC_TRANSPORTER.TRANSPORTER_ID_SUN ");
            query.AppendLine("     ,   LOC_TRANSPORTER.TRANSPORTER_ID_MON ");
            query.AppendLine("     ,   LOC_TRANSPORTER.TRANSPORTER_ID_TUE ");
            query.AppendLine("     ,   LOC_TRANSPORTER.TRANSPORTER_ID_WED ");
            query.AppendLine("     ,   LOC_TRANSPORTER.TRANSPORTER_ID_THU ");
            query.AppendLine("     ,   LOC_TRANSPORTER.TRANSPORTER_ID_FRI ");
            query.AppendLine("     ,   LOC_TRANSPORTER.TRANSPORTER_ID_SAT ");
            query.AppendLine("     ,   LOC_TRANSPORTER.TRANSPORTER_ID_HOL ");
            query.AppendLine("     ,   TRANSPORTER.TRANSPORTER_NAME ");
            query.AppendLine("     ,   TRANSPORTER_SUN.TRANSPORTER_NAME AS TRANSPORTER_NAME_SUN ");
            query.AppendLine("     ,   TRANSPORTER_MON.TRANSPORTER_NAME AS TRANSPORTER_NAME_MON ");
            query.AppendLine("     ,   TRANSPORTER_TUE.TRANSPORTER_NAME AS TRANSPORTER_NAME_TUE ");
            query.AppendLine("     ,   TRANSPORTER_WED.TRANSPORTER_NAME AS TRANSPORTER_NAME_WED ");
            query.AppendLine("     ,   TRANSPORTER_THU.TRANSPORTER_NAME AS TRANSPORTER_NAME_THU ");
            query.AppendLine("     ,   TRANSPORTER_FRI.TRANSPORTER_NAME AS TRANSPORTER_NAME_FRI ");
            query.AppendLine("     ,   TRANSPORTER_SAT.TRANSPORTER_NAME AS TRANSPORTER_NAME_SAT ");
            query.AppendLine("     ,   TRANSPORTER_HOL.TRANSPORTER_NAME AS TRANSPORTER_NAME_HOL ");
            query.AppendLine("     ,   LOC_TRANSPORTER.LEAD_TIMES_SUN ");
            query.AppendLine("     ,   LOC_TRANSPORTER.LEAD_TIMES_MON ");
            query.AppendLine("     ,   LOC_TRANSPORTER.LEAD_TIMES_TUE ");
            query.AppendLine("     ,   LOC_TRANSPORTER.LEAD_TIMES_WED ");
            query.AppendLine("     ,   LOC_TRANSPORTER.LEAD_TIMES_THU ");
            query.AppendLine("     ,   LOC_TRANSPORTER.LEAD_TIMES_FRI ");
            query.AppendLine("     ,   LOC_TRANSPORTER.LEAD_TIMES_SAT ");
            query.AppendLine("     ,   LOC_TRANSPORTER.LEAD_TIMES_HOL ");
            query.AppendLine("     ,   LOC_TRANSPORTER.ROWID AS ROW_ID ");
            query.AppendLine("     ,   LOC_TRANSPORTER.UPDATE_COUNT ");
            query.AppendLine("     ,   LOC_TRANSPORTER.CLIENT_CD ");
            query.AppendLine("     ,   CASE");
            query.AppendLine("             WHEN LOC_TRANSPORTER.CLIENT_CD IS NULL THEN NULL");
            query.AppendLine("             ELSE SUBSTR(LOC_TRANSPORTER.CLIENT_CD, 1, 8) || '-' || SUBSTR(LOC_TRANSPORTER.CLIENT_CD, 9, 3)");
            query.AppendLine("         END AS DISP_CLIENT_CD");
            query.AppendLine("     ,   MCS.CONSIGNOR_NAME2 AS CONSIGNOR_NAME ");
            query.AppendLine("     ,   LOC_TRANSPORTER.CONTROL_ID ");
            query.AppendLine("     ,   LOC_TRANSPORTER.CONTROL_ID AS DISP_CONTROL_ID ");
            query.AppendLine("     ,   MCN.CONSIGNOR_NAME2 AS NANIWA_CONSIGNOR_NAME ");
            query.AppendLine("     ,   LOC_TRANSPORTER.CONSIGNOR_ID ");
            query.AppendLine("     ,   LOC_TRANSPORTER.CONSIGNOR_ID AS DISP_CONSIGNOR_ID ");
            query.AppendLine("     ,   MCW.CONSIGNOR_NAME2 AS WS_CONSIGNOR_NAME ");

            query.AppendLine(" FROM ");
            query.AppendLine("         M_LOC_TRANSPORTERS LOC_TRANSPORTER ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_CENTERS CENTER ");
            query.AppendLine(" ON ");
            query.AppendLine("         LOC_TRANSPORTER.SHIPPER_ID = CENTER.SHIPPER_ID  ");
            query.AppendLine("     AND LOC_TRANSPORTER.CENTER_ID = CENTER.CENTER_ID  ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         V_SHIP_TO_STORES STORES ");
            query.AppendLine(" ON ");
            query.AppendLine("         LOC_TRANSPORTER.SHIPPER_ID = STORES.SHIPPER_ID  ");
            query.AppendLine("     AND LOC_TRANSPORTER.SHIP_TO_STORE_ID = STORES.SHIP_TO_STORE_ID ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_PREFS PREFS ");
            query.AppendLine(" ON ");
            query.AppendLine("         PREFS.SHIPPER_ID = STORES.SHIPPER_ID ");
            query.AppendLine("     AND PREFS.PREF_ID = STORES.SHIP_TO_PREF_ID ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_TRANSPORTERS TRANSPORTER ");
            query.AppendLine(" ON ");
            query.AppendLine("         LOC_TRANSPORTER.SHIPPER_ID = TRANSPORTER.SHIPPER_ID  ");
            query.AppendLine("     AND LOC_TRANSPORTER.TRANSPORTER_ID = TRANSPORTER.TRANSPORTER_ID  ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_TRANSPORTERS TRANSPORTER_SUN ");
            query.AppendLine(" ON ");
            query.AppendLine("         LOC_TRANSPORTER.SHIPPER_ID = TRANSPORTER_SUN.SHIPPER_ID  ");
            query.AppendLine("     AND LOC_TRANSPORTER.TRANSPORTER_ID_SUN = TRANSPORTER_SUN.TRANSPORTER_ID  ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_TRANSPORTERS TRANSPORTER_MON ");
            query.AppendLine(" ON ");
            query.AppendLine("         LOC_TRANSPORTER.SHIPPER_ID = TRANSPORTER_MON.SHIPPER_ID  ");
            query.AppendLine("     AND LOC_TRANSPORTER.TRANSPORTER_ID_MON = TRANSPORTER_MON.TRANSPORTER_ID  ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_TRANSPORTERS TRANSPORTER_TUE ");
            query.AppendLine(" ON ");
            query.AppendLine("         LOC_TRANSPORTER.SHIPPER_ID = TRANSPORTER_TUE.SHIPPER_ID  ");
            query.AppendLine("     AND LOC_TRANSPORTER.TRANSPORTER_ID_TUE = TRANSPORTER_TUE.TRANSPORTER_ID  ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_TRANSPORTERS TRANSPORTER_WED ");
            query.AppendLine(" ON ");
            query.AppendLine("         LOC_TRANSPORTER.SHIPPER_ID = TRANSPORTER_WED.SHIPPER_ID  ");
            query.AppendLine("     AND LOC_TRANSPORTER.TRANSPORTER_ID_WED = TRANSPORTER_WED.TRANSPORTER_ID  ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_TRANSPORTERS TRANSPORTER_THU ");
            query.AppendLine(" ON ");
            query.AppendLine("         LOC_TRANSPORTER.SHIPPER_ID = TRANSPORTER_THU.SHIPPER_ID  ");
            query.AppendLine("     AND LOC_TRANSPORTER.TRANSPORTER_ID_THU = TRANSPORTER_THU.TRANSPORTER_ID  ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_TRANSPORTERS TRANSPORTER_FRI ");
            query.AppendLine(" ON ");
            query.AppendLine("         LOC_TRANSPORTER.SHIPPER_ID = TRANSPORTER_FRI.SHIPPER_ID  ");
            query.AppendLine("     AND LOC_TRANSPORTER.TRANSPORTER_ID_FRI = TRANSPORTER_FRI.TRANSPORTER_ID  ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_TRANSPORTERS TRANSPORTER_SAT ");
            query.AppendLine(" ON ");
            query.AppendLine("         LOC_TRANSPORTER.SHIPPER_ID = TRANSPORTER_SAT.SHIPPER_ID  ");
            query.AppendLine("     AND LOC_TRANSPORTER.TRANSPORTER_ID_SAT = TRANSPORTER_SAT.TRANSPORTER_ID  ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_TRANSPORTERS TRANSPORTER_HOL ");
            query.AppendLine(" ON ");
            query.AppendLine("         LOC_TRANSPORTER.SHIPPER_ID = TRANSPORTER_HOL.SHIPPER_ID  ");
            query.AppendLine("     AND LOC_TRANSPORTER.TRANSPORTER_ID_HOL = TRANSPORTER_HOL.TRANSPORTER_ID  ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_CONSIGNORS_SAGAWA MCS  ");
            query.AppendLine(" ON ");
            query.AppendLine("         LOC_TRANSPORTER.SHIPPER_ID = MCS.SHIPPER_ID  ");
            query.AppendLine("     AND LOC_TRANSPORTER.CENTER_ID = MCS.CENTER_ID ");
            query.AppendLine("     AND LOC_TRANSPORTER.CLIENT_CD = MCS.CLIENT_CD  ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_CONSIGNORS_NANIWA MCN  ");
            query.AppendLine(" ON ");
            query.AppendLine("         LOC_TRANSPORTER.SHIPPER_ID = MCN.SHIPPER_ID  ");
            query.AppendLine("     AND LOC_TRANSPORTER.CENTER_ID = MCN.CENTER_ID ");
            query.AppendLine("     AND LOC_TRANSPORTER.CONTROL_ID = MCN.CONTROL_ID  ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_CONSIGNORS_WORLD MCW ");
            query.AppendLine(" ON ");
            query.AppendLine("         LOC_TRANSPORTER.SHIPPER_ID = MCW.SHIPPER_ID ");
            query.AppendLine("     AND LOC_TRANSPORTER.CENTER_ID = MCW.CENTER_ID ");
            query.AppendLine("     AND LOC_TRANSPORTER.CONSIGNOR_ID = MCW.CONSIGNOR_ID  ");

            if (condition.IsNewDate)
            {
                query.AppendLine(" INNER JOIN ");
                query.AppendLine("         MAX_START_DATE");
                query.AppendLine(" ON ");
                query.AppendLine("         LOC_TRANSPORTER.SHIPPER_ID = MAX_START_DATE.SHIPPER_ID ");
                query.AppendLine("     AND LOC_TRANSPORTER.CENTER_ID = MAX_START_DATE.CENTER_ID ");
                query.AppendLine("     AND LOC_TRANSPORTER.SHIP_TO_STORE_ID = MAX_START_DATE.SHIP_TO_STORE_ID ");
                query.AppendLine("     AND LOC_TRANSPORTER.START_DATE = MAX_START_DATE.START_DATE ");
            }

            query.AppendLine(" WHERE ");
            query.AppendLine("         LOC_TRANSPORTER.SHIPPER_ID = :SHIPPER_ID ");

            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            if (!string.IsNullOrEmpty(condition.CenterId))
            {
                query.AppendLine("     AND LOC_TRANSPORTER.CENTER_ID = :CENTER_ID ");
                parameters.Add(":CENTER_ID", condition.CenterId);
            }

            if (!string.IsNullOrEmpty(condition.ShipToStoreId))
            {
                query.AppendLine("     AND LOC_TRANSPORTER.SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID ");
                parameters.Add(":SHIP_TO_STORE_ID", condition.ShipToStoreId);
            }

            if (!string.IsNullOrEmpty(condition.ShipToStoreName))
            {
                query.AppendLine("     AND STORES.SHIP_TO_STORE_NAME1 LIKE :SHIP_TO_STORE_NAME");
                parameters.Add(":SHIP_TO_STORE_NAME", condition.ShipToStoreName + "%");
            }

            if (!string.IsNullOrEmpty(condition.TransporterId))
            {
                query.AppendLine("     AND LOC_TRANSPORTER.TRANSPORTER_ID = :TRANSPORTER_ID ");
                parameters.Add(":TRANSPORTER_ID", condition.TransporterId);
            }

            if (!string.IsNullOrEmpty(condition.ShipToStoreClass))
            {
                query.AppendLine("     AND LOC_TRANSPORTER.SHIP_TO_STORE_CLASS = :SHIP_TO_STORE_CLASS ");
                parameters.Add(":SHIP_TO_STORE_CLASS", condition.ShipToStoreClass);
            }

            if (condition.AreaItem != null && condition.AreaItem.Where(x => x.IsCheck).Any())
            {
                query.AppendLine("     AND EXISTS ( ");
                query.AppendLine("             SELECT ");
                query.AppendLine("                     'X' ");
                query.AppendLine("             FROM ");
                query.AppendLine("                     M_DELIAREA_GROUP AREA_GROUP ");
                query.AppendLine("             WHERE ");
                query.AppendLine("                     AREA_GROUP.SHIPPER_ID = LOC_TRANSPORTER.SHIPPER_ID ");
                query.AppendLine("                 AND AREA_GROUP.CENTER_ID = LOC_TRANSPORTER.CENTER_ID ");
                query.AppendLine("                 AND AREA_GROUP.DELIAREA_GROUP_ID IN :AREA ");
                query.AppendLine("                 AND AREA_GROUP.PREF_ID = STORES.SHIP_TO_PREF_ID ");
                query.AppendLine("         ) ");
                parameters.Add(":AREA", condition.AreaItem.Where(x => x.IsCheck).Select(x => x.AreaId).ToArray());
            }

            string strSort = " ASC ";
            if (condition.Sort == AscDescSort.Desc)
            {
                strSort = " DESC ";
            }

            query.AppendLine(" ORDER BY ");
            switch (condition.SortKey)
            {
                case LocTransporterSortKey.SKey1:
                    query.AppendLine("         LOC_TRANSPORTER.CENTER_ID " + strSort);
                    query.AppendLine("     ,   LOC_TRANSPORTER.SHIP_TO_STORE_ID " + strSort);
                    query.AppendLine("     ,   LOC_TRANSPORTER.START_DATE " + strSort);
                    break;
                case LocTransporterSortKey.SKey2:
                    query.AppendLine("         LOC_TRANSPORTER.CENTER_ID " + strSort);
                    query.AppendLine("     ,   LOC_TRANSPORTER.START_DATE " + strSort);
                    query.AppendLine("     ,   LOC_TRANSPORTER.SHIP_TO_STORE_ID " + strSort);
                    break;
                default:
                    break;
            }

        }

        #region dropdownlist get

        /// <summary>
        /// 配送業者マスタList
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetTransportersList()
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
        /// エリアList
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetAreaList(bool selected)
        {
            string gen_div_cd = "DELI_AREA_SELECT_DIALOG_AREA_G";
            return MvcDbContext.Current.Generals
                .Where(d => d.ShipperId == Profile.User.ShipperId
                    && d.CenterId == Profile.User.CenterId
                    && d.GenDivCd == gen_div_cd
                    && d.RegisterDiviCd == "1")
                .Select(m => new SelectListItem
                {
                    Value = m.GenCd.ToString(),
                    Text = m.GenName,
                    Selected = selected
                }).Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 佐川顧客コードドロップダウンリスト生成
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetSagawaClientList(string centerId)
        {

            var query = new StringBuilder();
            var parameters = new DynamicParameters();

            query.Append(@"
                    SELECT
                            MCS.CLIENT_CD AS VALUE
                        ,   SUBSTR(MCS.CLIENT_CD,1,8) || '-' || SUBSTR(MCS.CLIENT_CD,9,3) || '：' || MCS.CONSIGNOR_NAME2 AS TEXT
                    FROM
                            M_CONSIGNORS_SAGAWA MCS
                    WHERE
                            MCS.SHIPPER_ID = :SHIPPER_ID
                        AND MCS.CENTER_ID = :CENTER_ID
                    ORDER BY
                            VALUE
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", centerId);

            var sagawaSelect = MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters);

            return sagawaSelect.ToList();
        }

        /// <summary>
        /// 浪速顧客コードドロップダウンリスト生成
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetNaniwaControlList(string centerId)
        {

            var query = new StringBuilder();
            var parameters = new DynamicParameters();

            query.Append(@"
                    SELECT
                            MCN.CONTROL_ID AS VALUE
                        ,   MCN.CONTROL_ID || '：' || MCN.CONSIGNOR_NAME2 AS TEXT
                    FROM
                            M_CONSIGNORS_NANIWA MCN
                    WHERE
                            MCN.SHIPPER_ID = :SHIPPER_ID
                        AND MCN.CENTER_ID = :CENTER_ID
                    ORDER BY
                            VALUE
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", centerId);

            var naniwaSelect = MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters);

            return naniwaSelect.ToList();
        }

        /// <summary>
        /// WS顧客コードドロップダウンリスト生成
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetWsConsignorIdList(string centerId)
        {

            var query = new StringBuilder();
            var parameters = new DynamicParameters();

            query.Append(@"
                    SELECT
                            MCW.CONSIGNOR_ID AS VALUE
                        ,   MCW.CONSIGNOR_ID || '：' || MCW.CONSIGNOR_NAME2 AS TEXT
                    FROM
                            M_CONSIGNORS_WORLD MCW
                    WHERE
                            MCW.SHIPPER_ID = :SHIPPER_ID
                        AND MCW.CENTER_ID = :CENTER_ID
                    ORDER BY
                            VALUE
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", centerId);

            var wsList = MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters);

            return wsList.ToList();
        }
        #endregion

        /// <summary>
        /// 保存する（新規登録画面、追加画面）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool LocTransporterAdd(IList<LocTransporter> LocTransporters)
        {
            var dbContext = MvcDbContext.Current;
            using (var trans = dbContext.Database.BeginTransaction())
            {
                if (LocTransporters != null)
                {
                    foreach (var item in LocTransporters)
                    {
                        var upd_model = dbContext.LocTransporters
                                            .Where(m => m.CenterId == item.CenterId
                                            && m.ShipToStoreId == item.ShipToStoreId
                                            && m.StartDate == item.StartDate
                                            && m.ShipperId == Profile.User.ShipperId
                                            && m.UpdateCount == item.UpdateCount).SingleOrDefault();

                        if (upd_model == null)
                        {
                            item.SetBaseInfoInsert();
                            item.TransporterIdMon = item.TransporterId;
                            item.TransporterIdTue = item.TransporterId;
                            item.TransporterIdWed = item.TransporterId;
                            item.TransporterIdThu = item.TransporterId;
                            item.TransporterIdFri = item.TransporterId;
                            item.TransporterIdSat = item.TransporterId;
                            item.TransporterIdSun = item.TransporterId;
                            item.TransporterIdHol = item.TransporterId;
                            item.LeadTimesMon = item.LeadTimes;
                            item.LeadTimesTue = item.LeadTimes;
                            item.LeadTimesWed = item.LeadTimes;
                            item.LeadTimesThu = item.LeadTimes;
                            item.LeadTimesFri = item.LeadTimes;
                            item.LeadTimesSat = item.LeadTimes;
                            item.LeadTimesSun = item.LeadTimes;
                            item.LeadTimesHol = item.LeadTimes;
                            dbContext.LocTransporters.Add(item);
                        }
                        else
                        {
                            upd_model.SetBaseInfoUpdate();
                            upd_model.TransporterId = item.TransporterId;
                            upd_model.LeadTimes = item.LeadTimes;
                            upd_model.TransporterIdMon = item.TransporterId;
                            upd_model.TransporterIdTue = item.TransporterId;
                            upd_model.TransporterIdWed = item.TransporterId;
                            upd_model.TransporterIdThu = item.TransporterId;
                            upd_model.TransporterIdFri = item.TransporterId;
                            upd_model.TransporterIdSat = item.TransporterId;
                            upd_model.TransporterIdSun = item.TransporterId;
                            upd_model.TransporterIdHol = item.TransporterId;
                            upd_model.LeadTimesMon = item.LeadTimes;
                            upd_model.LeadTimesTue = item.LeadTimes;
                            upd_model.LeadTimesWed = item.LeadTimes;
                            upd_model.LeadTimesThu = item.LeadTimes;
                            upd_model.LeadTimesFri = item.LeadTimes;
                            upd_model.LeadTimesSat = item.LeadTimes;
                            upd_model.LeadTimesSun = item.LeadTimes;
                            upd_model.LeadTimesHol = item.LeadTimes;
                            upd_model.ClientCd = item.ClientCd;
                            upd_model.ControlId = item.ControlId;
                            upd_model.ConsignorId = item.ConsignorId;

                        }
                    }

                    try
                    {
                        dbContext.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        return false;
                    }

                    trans.Commit();
                }
            }

            return true;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool LocTransporterUpd(SearchItem item)
        {
            var dbContext = MvcDbContext.Current;
            using (var trans = dbContext.Database.BeginTransaction())
            {
                var upd_model = dbContext.LocTransporters
                                    .Where(m => m.CenterId == item.CenterId
                                    && m.ShipToStoreId == item.ShipToStoreId
                                    && m.StartDate == item.StartDate
                                    && m.ShipperId == Profile.User.ShipperId
                                    && m.UpdateCount == item.UpdateCount).SingleOrDefault();

                // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合）
                if (upd_model == null)
                {
                    return false;
                }

                StringBuilder sqlUpd = new StringBuilder();
                DynamicParameters parameters = new DynamicParameters();

                sqlUpd.AppendLine(" UPDATE ");
                sqlUpd.AppendLine("         M_LOC_TRANSPORTERS ");
                sqlUpd.AppendLine(" SET ");
                sqlUpd.AppendLine("         UPDATE_DATE = SYSTIMESTAMP ");
                sqlUpd.AppendLine("     ,   UPDATE_USER_ID = :UPDATE_USER_ID ");
                sqlUpd.AppendLine("     ,   UPDATE_PROGRAM_NAME = :UPDATE_PROGRAM_NAME ");
                sqlUpd.AppendLine("     ,   UPDATE_COUNT = UPDATE_COUNT + 1 ");
                sqlUpd.AppendLine("     ,   TRANSPORTER_ID = :TRANSPORTER_ID");
                sqlUpd.AppendLine("     ,   LEAD_TIMES = :LEAD_TIMES ");
                sqlUpd.AppendLine("     ,   TRANSPORTER_ID_MON = :TRANSPORTER_ID ");
                sqlUpd.AppendLine("     ,   TRANSPORTER_ID_TUE = :TRANSPORTER_ID ");
                sqlUpd.AppendLine("     ,   TRANSPORTER_ID_WED = :TRANSPORTER_ID ");
                sqlUpd.AppendLine("     ,   TRANSPORTER_ID_THU = :TRANSPORTER_ID ");
                sqlUpd.AppendLine("     ,   TRANSPORTER_ID_FRI = :TRANSPORTER_ID ");
                sqlUpd.AppendLine("     ,   TRANSPORTER_ID_SAT = :TRANSPORTER_ID ");
                sqlUpd.AppendLine("     ,   TRANSPORTER_ID_SUN = :TRANSPORTER_ID ");
                sqlUpd.AppendLine("     ,   TRANSPORTER_ID_HOL = :TRANSPORTER_ID ");
                sqlUpd.AppendLine("     ,   LEAD_TIMES_MON = :LEAD_TIMES ");
                sqlUpd.AppendLine("     ,   LEAD_TIMES_TUE = :LEAD_TIMES ");
                sqlUpd.AppendLine("     ,   LEAD_TIMES_WED = :LEAD_TIMES ");
                sqlUpd.AppendLine("     ,   LEAD_TIMES_THU = :LEAD_TIMES ");
                sqlUpd.AppendLine("     ,   LEAD_TIMES_FRI = :LEAD_TIMES ");
                sqlUpd.AppendLine("     ,   LEAD_TIMES_SAT = :LEAD_TIMES ");
                sqlUpd.AppendLine("     ,   LEAD_TIMES_SUN = :LEAD_TIMES ");
                sqlUpd.AppendLine("     ,   LEAD_TIMES_HOL = :LEAD_TIMES ");
                sqlUpd.AppendLine("     ,   CLIENT_CD = :CLIENT_CD");
                sqlUpd.AppendLine("     ,   CONTROL_ID = :CONTROL_ID");
                sqlUpd.AppendLine("     ,   CONSIGNOR_ID = :CONSIGNOR_ID");

                sqlUpd.AppendLine(" WHERE ");
                sqlUpd.AppendLine("         SHIPPER_ID = :SHIPPER_ID ");
                sqlUpd.AppendLine("     AND CENTER_ID = :CENTER_ID ");
                sqlUpd.AppendLine("     AND SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID ");
                sqlUpd.AppendLine("     AND START_DATE = :START_DATE ");

                parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                parameters.Add(":CENTER_ID", item.CenterId);
                parameters.Add(":UPDATE_USER_ID", Profile.User.UserId);
                parameters.Add(":UPDATE_PROGRAM_NAME", "Master/LocTransporter/Update");
                parameters.Add(":SHIP_TO_STORE_ID", item.ShipToStoreId);
                parameters.Add(":START_DATE", item.StartDate);
                parameters.Add(":TRANSPORTER_ID", item.TransporterId);
                parameters.Add(":LEAD_TIMES", item.LeadTimes);
                //parameters.Add(":TRANSPORTER_ID_MON", item.TransporterIdMon);
                //parameters.Add(":TRANSPORTER_ID_TUE", item.TransporterIdTue);
                //parameters.Add(":TRANSPORTER_ID_WED", item.TransporterIdWed);
                //parameters.Add(":TRANSPORTER_ID_THU", item.TransporterIdThu);
                //parameters.Add(":TRANSPORTER_ID_FRI", item.TransporterIdFri);
                //parameters.Add(":TRANSPORTER_ID_SAT", item.TransporterIdSat);
                //parameters.Add(":TRANSPORTER_ID_SUN", item.TransporterIdSun);
                //parameters.Add(":TRANSPORTER_ID_HOL", item.TransporterIdHol);
                //parameters.Add(":LEAD_TIMES_MON", item.LeadTimesMon);
                //parameters.Add(":LEAD_TIMES_TUE", item.LeadTimesTue);
                //parameters.Add(":LEAD_TIMES_WED", item.LeadTimesWed);
                //parameters.Add(":LEAD_TIMES_THU", item.LeadTimesThu);
                //parameters.Add(":LEAD_TIMES_FRI", item.LeadTimesFri);
                //parameters.Add(":LEAD_TIMES_SAT", item.LeadTimesSat);
                //parameters.Add(":LEAD_TIMES_SUN", item.LeadTimesSun);
                //parameters.Add(":LEAD_TIMES_HOL", item.LeadTimesHol);
                parameters.Add(":CLIENT_CD", item.ClientCd);
                parameters.Add(":CONTROL_ID", item.ControlId);
                parameters.Add(":CONSIGNOR_ID", item.ConsignorId);

                try
                {
                    dbContext.Database.Connection.Execute(sqlUpd.ToString(), parameters, trans.UnderlyingTransaction);
                }
                catch (DbUpdateException)
                {
                    trans.Rollback();
                    return false;
                }

                trans.Commit();
            }

            return true;
        }

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="LocTransporters"></param>
        /// <returns></returns>
        public bool LocTransporterDel(IList<LocTransporter> LocTransporters)
        {
            var dbContext = MvcDbContext.Current;
            using (var trans = dbContext.Database.BeginTransaction())
            {
                foreach (var item in LocTransporters)
                {
                    var del_model = dbContext.LocTransporters
                                        .Where(m => m.CenterId == item.CenterId
                                        && m.ShipToStoreId == item.ShipToStoreId
                                        && m.StartDate == item.StartDate
                                        && m.ShipperId == Profile.User.ShipperId)
                                        .SingleOrDefault();
                    if (del_model != null)
                    {
                        dbContext.LocTransporters.Remove(del_model);
                    }
                }

                try
                {
                    dbContext.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    return false;
                }

                trans.Commit();
            }

            return true;
        }

        /// <summary>
        /// 対象の行のデータを取得
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IList<LocTransporter> GetTargetById(SearchItem item)
        {
            return MvcDbContext.Current.LocTransporters
                   .Where(m => m.ShipperId == Profile.User.ShipperId
                            && m.ShipToStoreId == item.ShipToStoreId
                            && m.StartDate == item.StartDate
                            && m.CenterId == item.CenterId).ToList();
        }

        public IEnumerable<SelectListItem> GetStoreClassList()
        {
            return MvcDbContext.Current.Generals
                .Where(m => m.ShipperId == Profile.User.ShipperId
                         && m.CenterId == "@@@"
                         && m.RegisterDiviCd == "1"
                         && m.GenDivCd == "STORE_CLASS")
                .Select(v => new SelectListItem
                {
                    Value = v.GenCd.ToString(),
                    Text = v.GenName
                }).Distinct().OrderBy(v => v.Value);
        }


        /// <summary>
        /// 配送業者が03：浪速のとき 浪速仕分コード設定がない店舗の場合はエラー
        /// </summary>
        /// <param name="shipToStoreId"></param>
        /// <returns></returns>
        public bool CheckNaniwaSorting(string shipToStoreId)
        {
            return MvcDbContext.Current.NaniwaSorting
                .Where(m => m.ShipperId == Profile.User.ShipperId
                && m.StoreId == shipToStoreId).Any();
        }
    }
}