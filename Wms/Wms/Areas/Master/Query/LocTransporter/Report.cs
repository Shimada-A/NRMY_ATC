namespace Wms.Areas.Master.Query.LocTransporter
{
    using Dapper;
    using PagedList;
    using Share.Common.Resources;
    using Share.Reports.Import;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.Resources;
    using Wms.Areas.Master.ViewModels.LocTransporter;
    using Wms.Common;
    using Wms.Common.Resources;
    using Wms.Models;
    using Wms.Query;
    using Wms.Resources;

    public class Report : BaseQuery
    {
        /// <summary>
        /// Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、センター別店舗別配送業者マスタのデータを作る。</returns>
        public IEnumerable<ViewModels.LocTransporter.Report> Listing(LocTransporterSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();

            // 一覧のSQLを取得
            LocTransporter.GetQuery(condition, ref query, ref parameters);
            return MvcDbContext.Current.Database.Connection.Query<ViewModels.LocTransporter.Report>(query.ToString(), parameters).ToList();
        }

        /// <summary>
        /// アップロードされたデータのImport
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        public long InsertWwMasLocTransporters(IEnumerable<ViewModels.LocTransporter.Report> report)
        {
            var dbContext = MvcDbContext.Current;
            var workId = GetWorkId();
            var no = 0;
            using (var trans = dbContext.Database.BeginTransaction())
            {
                foreach (var u in report.Select((v, i) => new { v, i }))
                {
                    no = no + 1;

                    var LocTransporter = new Models.MasLocTransporter
                    {
                        Seq = workId,
                        No = no,
                        CenterId = u.v.CenterId,
                        ShipToStoreId = u.v.ShipToStoreId,
                        ShipToStoreClass = u.v.ShipToStoreClass.ToString(),
                        StartDate = u.v.StartDate.ToString(),
                        TransporterId = u.v.TransporterId,
                        LeadTimes = u.v.LeadTimes.ToString(),
                        TransporterIdMon = u.v.TransporterId,
                        TransporterIdTue = u.v.TransporterId,
                        TransporterIdWed = u.v.TransporterId,
                        TransporterIdThu = u.v.TransporterId,
                        TransporterIdFri = u.v.TransporterId,
                        TransporterIdSat = u.v.TransporterId,
                        TransporterIdSun = u.v.TransporterId,
                        TransporterIdHol = u.v.TransporterId,
                        LeadTimesMon = u.v.LeadTimes.ToString(),
                        LeadTimesTue = u.v.LeadTimes.ToString(),
                        LeadTimesWed = u.v.LeadTimes.ToString(),
                        LeadTimesThu = u.v.LeadTimes.ToString(),
                        LeadTimesFri = u.v.LeadTimes.ToString(),
                        LeadTimesSat = u.v.LeadTimes.ToString(),
                        LeadTimesSun = u.v.LeadTimes.ToString(),
                        LeadTimesHol = u.v.LeadTimes.ToString(),
                        ClientCd = u.v.ClientCd,
                        ControlId = u.v.ControlId,
                        ConsignorId = u.v.ConsignorId

                    };
                    LocTransporter.SetBaseInfoInsert();
                    dbContext.MasLocTransporters.Add(LocTransporter);
                    try
                    {
                        dbContext.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                    }
                }

                trans.Commit();
            }

            return workId;
        }

        /// <summary>
        /// アップロードされたデータのチェック
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        public bool UploadCheck(long workId, List<MasLocTransporter> report)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                ExcelReader<MasLocTransporter> er = new ExcelReader<MasLocTransporter>();
                foreach (var data in report.Select((v, i) => new { v, i }))
                {
                    // 倉庫マスタに存在しない出荷元が入力されている
                    if (!string.IsNullOrWhiteSpace(data.v.CenterId) && !MvcDbContext.Current.Warehouses.Where(x => x.ShipperId == Profile.User.ShipperId & x.CenterId == data.v.CenterId).Any())
                    {
                        data.v.ErrMsg = string.Format(MessagesResource.MasterNotExistsError, LocTransporterResource.ShipFromCenter);
                    }

                    // 出荷先ビューに存在しない出荷先が入力されている
                    else if (data.v.ErrMsg == null && !string.IsNullOrWhiteSpace(data.v.ShipToStoreId) && !MvcDbContext.Current.ShipToStores.Where(x => x.ShipperId == Profile.User.ShipperId & x.ShipToStoreId == data.v.ShipToStoreId).Any())
                    {
                        data.v.ErrMsg = string.Format(MessagesResource.MasterNotExistsError, LocTransporterResource.ShipToStore);
                    }

                    // 出荷元未入力
                    else if (data.v.ErrMsg == null && string.IsNullOrWhiteSpace(data.v.CenterId))
                    {
                        data.v.ErrMsg = string.Format(MessagesResource.Required, LocTransporterResource.ShipFromCenter);
                    }

                    // 出荷先未入力
                    else if (data.v.ErrMsg == null && string.IsNullOrWhiteSpace(data.v.ShipToStoreId))
                    {
                        data.v.ErrMsg = string.Format(MessagesResource.Required, LocTransporterResource.ShipToStore);
                    }

                    // 適用開始日未入力
                    else if (data.v.ErrMsg == null && string.IsNullOrWhiteSpace(data.v.StartDate))
                    {
                        data.v.ErrMsg = string.Format(MessagesResource.Required, LocTransporterResource.StartDate);
                    }

                    // 適用開始日の書式がYYYY / MM / DDではない
                    else if (data.v.ErrMsg == null && !er.CheckDateTime(0, data.v.StartDate))
                    {
                        data.v.ErrMsg = string.Format(MessagesResource.NotDateError, LocTransporterResource.StartDate);
                    }

                    // ファイル内重複チェック
                    data.v.ErrMsg = data.v.ErrMsg ?? CheckPrimaryKey(report, data.v);

                    //配送業者チェック
                    data.v.ErrMsg = data.v.ErrMsg ?? CheckTransporter(data.v.TransporterId, LocTransporterResource.TransporterId);
                    //data.v.ErrMsg = data.v.ErrMsg ?? CheckTransporter(data.v.TransporterIdMon, LocTransporterResource.TransporterIdMon);
                    //data.v.ErrMsg = data.v.ErrMsg ?? CheckTransporter(data.v.TransporterIdTue, LocTransporterResource.TransporterIdTue);
                    //data.v.ErrMsg = data.v.ErrMsg ?? CheckTransporter(data.v.TransporterIdWed, LocTransporterResource.TransporterIdWed);
                    //data.v.ErrMsg = data.v.ErrMsg ?? CheckTransporter(data.v.TransporterIdThu, LocTransporterResource.TransporterIdThu);
                    //data.v.ErrMsg = data.v.ErrMsg ?? CheckTransporter(data.v.TransporterIdFri, LocTransporterResource.TransporterIdFri);
                    //data.v.ErrMsg = data.v.ErrMsg ?? CheckTransporter(data.v.TransporterIdSat, LocTransporterResource.TransporterIdSat);
                    //data.v.ErrMsg = data.v.ErrMsg ?? CheckTransporter(data.v.TransporterIdSun, LocTransporterResource.TransporterIdSun);
                    //data.v.ErrMsg = data.v.ErrMsg ?? CheckTransporter(data.v.TransporterIdHol, LocTransporterResource.TransporterIdHol);
                    //リードタイムチェック
                    data.v.ErrMsg = data.v.ErrMsg ?? CheckLeadTimes(data.v.LeadTimes, LocTransporterResource.LeadTimes);
                    //data.v.ErrMsg = data.v.ErrMsg ?? CheckLeadTimes(data.v.LeadTimesMon, LocTransporterResource.LeadTimesMon);
                    //data.v.ErrMsg = data.v.ErrMsg ?? CheckLeadTimes(data.v.LeadTimesTue, LocTransporterResource.LeadTimesTue);
                    //data.v.ErrMsg = data.v.ErrMsg ?? CheckLeadTimes(data.v.LeadTimesWed, LocTransporterResource.LeadTimesWed);
                    //data.v.ErrMsg = data.v.ErrMsg ?? CheckLeadTimes(data.v.LeadTimesThu, LocTransporterResource.LeadTimesThu);
                    //data.v.ErrMsg = data.v.ErrMsg ?? CheckLeadTimes(data.v.LeadTimesFri, LocTransporterResource.LeadTimesFri);
                    //data.v.ErrMsg = data.v.ErrMsg ?? CheckLeadTimes(data.v.LeadTimesSat, LocTransporterResource.LeadTimesSat);
                    //data.v.ErrMsg = data.v.ErrMsg ?? CheckLeadTimes(data.v.LeadTimesSun, LocTransporterResource.LeadTimesSun);
                    //data.v.ErrMsg = data.v.ErrMsg ?? CheckLeadTimes(data.v.LeadTimesHol, LocTransporterResource.LeadTimesHol);

                    //佐川顧客コードチェック
                    data.v.ErrMsg = data.v.ErrMsg ?? CheckClientCd(data.v.CenterId, data.v.ClientCd, data.v.TransporterId);

                    //浪速管理IDチェック
                    data.v.ErrMsg = data.v.ErrMsg ?? CheckControlId(data.v.CenterId, data.v.ControlId, data.v.TransporterId);

                    //浪速仕分コードマスタチェック
                    data.v.ErrMsg = data.v.ErrMsg ?? CheckExistsNaniwaSorting(data.v.TransporterId, data.v.ShipToStoreId);

                    //荷送人(WS)マスタチェック
                    data.v.ErrMsg = data.v.ErrMsg ?? CheckConsignorId(data.v.CenterId,data.v.ConsignorId,data.v.TransporterId);

                    if (data.v.ErrMsg != null)
                    {
                        data.v.SetBaseInfoUpdate();
                        try
                        {
                            MvcDbContext.Current.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            return false;
                        }
                    }

                }
                trans.Commit();
            }

            return true;
        }

        /// <summary>
        /// チェック結果をワークテーブルへ更新
        /// </summary>
        /// <param name="report"></param>
        public void MergeLocTransporters(long workId)
        {
            var result = _dbContext.MasLocTransporters.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == workId && x.ErrMsg == null).ToList();

            var sql = @"MERGE INTO M_LOC_TRANSPORTERS T
                        USING(
                            SELECT
                                :SHIPPER_ID             SHIPPER_ID,
                                :CENTER_ID              CENTER_ID,
                                :SHIP_TO_STORE_ID       SHIP_TO_STORE_ID,
                                :START_DATE             START_DATE,
                                :TRANSPORTER_ID         TRANSPORTER_ID,
                                :LEAD_TIMES             LEAD_TIMES,
                                :TRANSPORTER_ID_MON     TRANSPORTER_ID_MON,
                                :TRANSPORTER_ID_TUE     TRANSPORTER_ID_TUE,
                                :TRANSPORTER_ID_WED     TRANSPORTER_ID_WED,
                                :TRANSPORTER_ID_THU     TRANSPORTER_ID_THU,
                                :TRANSPORTER_ID_FRI     TRANSPORTER_ID_FRI,
                                :TRANSPORTER_ID_SAT     TRANSPORTER_ID_SAT,
                                :TRANSPORTER_ID_SUN     TRANSPORTER_ID_SUN,
                                :TRANSPORTER_ID_HOL     TRANSPORTER_ID_HOL,
                                :LEAD_TIMES_MON         LEAD_TIMES_MON,
                                :LEAD_TIMES_TUE         LEAD_TIMES_TUE,
                                :LEAD_TIMES_WED         LEAD_TIMES_WED,
                                :LEAD_TIMES_THU         LEAD_TIMES_THU,
                                :LEAD_TIMES_FRI         LEAD_TIMES_FRI,
                                :LEAD_TIMES_SAT         LEAD_TIMES_SAT,
                                :LEAD_TIMES_SUN         LEAD_TIMES_SUN,
                                :LEAD_TIMES_HOL         LEAD_TIMES_HOL,
                                :CLIENT_CD              CLIENT_CD,
                                :CONTROL_ID             CONTROL_ID,
                                :CONSIGNOR_ID           CONSIGNOR_ID
                            FROM
                               DUAL
                        ) F
                        ON(
                                F.SHIPPER_ID = T.SHIPPER_ID
                            AND F.CENTER_ID = T.CENTER_ID
                            AND F.SHIP_TO_STORE_ID = T.SHIP_TO_STORE_ID
                            AND F.START_DATE = T.START_DATE
                        )
                        WHEN MATCHED THEN
                         UPDATE SET
                                T.UPDATE_DATE = SYSDATE,
                                T.UPDATE_USER_ID = :USER_ID,
                                T.UPDATE_PROGRAM_NAME = :PROGRAM_NAME,
                                T.UPDATE_COUNT = T.UPDATE_COUNT + 1,
                                T.TRANSPORTER_ID = F.TRANSPORTER_ID,
                                T.LEAD_TIMES = F.LEAD_TIMES,
                                T.TRANSPORTER_ID_MON = F.TRANSPORTER_ID_MON,
                                T.TRANSPORTER_ID_TUE = F.TRANSPORTER_ID_TUE,
                                T.TRANSPORTER_ID_WED = F.TRANSPORTER_ID_WED,
                                T.TRANSPORTER_ID_THU = F.TRANSPORTER_ID_THU,
                                T.TRANSPORTER_ID_FRI = F.TRANSPORTER_ID_FRI,
                                T.TRANSPORTER_ID_SAT = F.TRANSPORTER_ID_SAT,
                                T.TRANSPORTER_ID_SUN = F.TRANSPORTER_ID_SUN,
                                T.TRANSPORTER_ID_HOL = F.TRANSPORTER_ID_HOL,
                                T.LEAD_TIMES_MON = F.LEAD_TIMES_MON,
                                T.LEAD_TIMES_TUE = F.LEAD_TIMES_TUE,
                                T.LEAD_TIMES_WED = F.LEAD_TIMES_WED,
                                T.LEAD_TIMES_THU = F.LEAD_TIMES_THU,
                                T.LEAD_TIMES_FRI = F.LEAD_TIMES_FRI,
                                T.LEAD_TIMES_SAT = F.LEAD_TIMES_SAT,
                                T.LEAD_TIMES_SUN = F.LEAD_TIMES_SUN,
                                T.LEAD_TIMES_HOL = F.LEAD_TIMES_HOL,
                                T.CLIENT_CD      = F.CLIENT_CD,
                                T.CONTROL_ID     = F.CONTROL_ID,
                                T.CONSIGNOR_ID     = F.CONSIGNOR_ID
                        WHEN NOT MATCHED THEN
                            INSERT (
                                T.MAKE_DATE,
                                T.MAKE_USER_ID,
                                T.MAKE_PROGRAM_NAME,
                                T.UPDATE_DATE,
                                T.UPDATE_USER_ID,
                                T.UPDATE_PROGRAM_NAME,
                                T.UPDATE_COUNT,
                                T.SHIPPER_ID,
                                T.CENTER_ID,
                                T.SHIP_TO_STORE_ID,
                                T.START_DATE,
                                T.TRANSPORTER_ID,
                                T.LEAD_TIMES,
                                T.TRANSPORTER_ID_MON,
                                T.TRANSPORTER_ID_TUE,
                                T.TRANSPORTER_ID_WED,
                                T.TRANSPORTER_ID_THU,
                                T.TRANSPORTER_ID_FRI,
                                T.TRANSPORTER_ID_SAT,
                                T.TRANSPORTER_ID_SUN,
                                T.TRANSPORTER_ID_HOL,
                                T.LEAD_TIMES_MON,
                                T.LEAD_TIMES_TUE,
                                T.LEAD_TIMES_WED,
                                T.LEAD_TIMES_THU,
                                T.LEAD_TIMES_FRI,
                                T.LEAD_TIMES_SAT,
                                T.LEAD_TIMES_SUN,
                                T.LEAD_TIMES_HOL,
                                T.CLIENT_CD,
                                T.CONTROL_ID,
                                T.CONSIGNOR_ID
                            )VALUES (
                                SYSDATE,
                                :USER_ID,
                                :PROGRAM_NAME,
                                SYSDATE,
                                :USER_ID,
                                :PROGRAM_NAME,
                                0,
                                F.SHIPPER_ID,
                                F.CENTER_ID,
                                F.SHIP_TO_STORE_ID,
                                F.START_DATE,
                                F.TRANSPORTER_ID,
                                F.LEAD_TIMES,
                                F.TRANSPORTER_ID_MON,
                                F.TRANSPORTER_ID_TUE,
                                F.TRANSPORTER_ID_WED,
                                F.TRANSPORTER_ID_THU,
                                F.TRANSPORTER_ID_FRI,
                                F.TRANSPORTER_ID_SAT,
                                F.TRANSPORTER_ID_SUN,
                                F.TRANSPORTER_ID_HOL,
                                F.LEAD_TIMES_MON,
                                F.LEAD_TIMES_TUE,
                                F.LEAD_TIMES_WED,
                                F.LEAD_TIMES_THU,
                                F.LEAD_TIMES_FRI,
                                F.LEAD_TIMES_SAT,
                                F.LEAD_TIMES_SUN,
                                F.LEAD_TIMES_HOL,
                                F.CLIENT_CD,
                                F.CONTROL_ID,
                                F.CONSIGNOR_ID
                            )
                    ";

            var parameters = new DynamicParameters();

            foreach (var v in result)
            {
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":CENTER_ID", v.CenterId);
                parameters.Add(":SHIP_TO_STORE_CLASS", v.ShipToStoreClass);
                parameters.Add(":SHIP_TO_STORE_ID", v.ShipToStoreId);
                parameters.Add(":START_DATE", v.StartDate);
                parameters.Add(":TRANSPORTER_ID", v.TransporterId);
                parameters.Add(":LEAD_TIMES", v.LeadTimes);
                parameters.Add(":TRANSPORTER_ID_MON", v.TransporterIdMon);
                parameters.Add(":TRANSPORTER_ID_TUE", v.TransporterIdTue);
                parameters.Add(":TRANSPORTER_ID_WED", v.TransporterIdWed);
                parameters.Add(":TRANSPORTER_ID_THU", v.TransporterIdThu);
                parameters.Add(":TRANSPORTER_ID_FRI", v.TransporterIdFri);
                parameters.Add(":TRANSPORTER_ID_SAT", v.TransporterIdSat);
                parameters.Add(":TRANSPORTER_ID_SUN", v.TransporterIdSun);
                parameters.Add(":TRANSPORTER_ID_HOL", v.TransporterIdHol);
                parameters.Add(":LEAD_TIMES_MON", v.LeadTimesMon);
                parameters.Add(":LEAD_TIMES_TUE", v.LeadTimesTue);
                parameters.Add(":LEAD_TIMES_WED", v.LeadTimesWed);
                parameters.Add(":LEAD_TIMES_THU", v.LeadTimesThu);
                parameters.Add(":LEAD_TIMES_FRI", v.LeadTimesFri);
                parameters.Add(":LEAD_TIMES_SAT", v.LeadTimesSat);
                parameters.Add(":LEAD_TIMES_SUN", v.LeadTimesSun);
                parameters.Add(":LEAD_TIMES_HOL", v.LeadTimesHol);
                parameters.Add(":CLIENT_CD", v.ClientCd);
                parameters.Add(":CONTROL_ID", v.ControlId);
                parameters.Add(":CONSIGNOR_ID", v.ConsignorId);
                parameters.Add(":USER_ID", Common.Profile.User.UserId);
                parameters.Add(":PROGRAM_NAME", nameof(MenuResource.W_MAS_Transporter01));
                _dbContext.Database.Connection.Execute(sql, parameters);
            }
        }

        /// <summary>
        /// エラー情報取得
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<ViewModels.LocTransporter.IndexResultRow> GetReportErrList(UploadCondition conditions)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine(" SELECT ");
            query.AppendLine("         WW.SEQ ");
            query.AppendLine("     ,   WW.NO ");
            query.AppendLine("     ,   WW.SHIPPER_ID ");
            query.AppendLine("     ,   WW.CENTER_ID ");
            query.AppendLine("     ,   CENTER.CENTER_NAME1 AS CENTER_NAME ");
            query.AppendLine("     ,   WW.SHIP_TO_STORE_CLASS ");
            query.AppendLine("     ,   WW.SHIP_TO_STORE_ID ");
            query.AppendLine("     ,   STORES.SHIP_TO_STORE_NAME1 AS SHIP_TO_STORE_NAME ");
            query.AppendLine("     ,   STORES.SHIP_TO_PREF_ID AS PREF_ID ");
            query.AppendLine("     ,   PREFS.PREF_NAME ");
            query.AppendLine("     ,   WW.START_DATE ");
            query.AppendLine("     ,   WW.TRANSPORTER_ID ");
            query.AppendLine("     ,   WW.LEAD_TIMES ");
            query.AppendLine("     ,   WW.TRANSPORTER_ID_SUN ");
            query.AppendLine("     ,   WW.TRANSPORTER_ID_MON ");
            query.AppendLine("     ,   WW.TRANSPORTER_ID_TUE ");
            query.AppendLine("     ,   WW.TRANSPORTER_ID_WED ");
            query.AppendLine("     ,   WW.TRANSPORTER_ID_THU ");
            query.AppendLine("     ,   WW.TRANSPORTER_ID_FRI ");
            query.AppendLine("     ,   WW.TRANSPORTER_ID_SAT ");
            query.AppendLine("     ,   WW.TRANSPORTER_ID_HOL ");
            query.AppendLine("     ,   TRANSPORTER.TRANSPORTER_NAME ");
            query.AppendLine("     ,   TRANSPORTER_SUN.TRANSPORTER_NAME AS TRANSPORTER_NAME_SUN ");
            query.AppendLine("     ,   TRANSPORTER_MON.TRANSPORTER_NAME AS TRANSPORTER_NAME_MON ");
            query.AppendLine("     ,   TRANSPORTER_TUE.TRANSPORTER_NAME AS TRANSPORTER_NAME_TUE ");
            query.AppendLine("     ,   TRANSPORTER_WED.TRANSPORTER_NAME AS TRANSPORTER_NAME_WED ");
            query.AppendLine("     ,   TRANSPORTER_THU.TRANSPORTER_NAME AS TRANSPORTER_NAME_THU ");
            query.AppendLine("     ,   TRANSPORTER_FRI.TRANSPORTER_NAME AS TRANSPORTER_NAME_FRI ");
            query.AppendLine("     ,   TRANSPORTER_SAT.TRANSPORTER_NAME AS TRANSPORTER_NAME_SAT ");
            query.AppendLine("     ,   TRANSPORTER_HOL.TRANSPORTER_NAME AS TRANSPORTER_NAME_HOL ");
            query.AppendLine("     ,   WW.LEAD_TIMES_SUN ");
            query.AppendLine("     ,   WW.LEAD_TIMES_MON ");
            query.AppendLine("     ,   WW.LEAD_TIMES_TUE ");
            query.AppendLine("     ,   WW.LEAD_TIMES_WED ");
            query.AppendLine("     ,   WW.LEAD_TIMES_THU ");
            query.AppendLine("     ,   WW.LEAD_TIMES_FRI ");
            query.AppendLine("     ,   WW.LEAD_TIMES_SAT ");
            query.AppendLine("     ,   WW.LEAD_TIMES_HOL ");
            query.AppendLine("     ,   WW.CLIENT_CD");
            query.AppendLine("     ,   WW.CONTROL_ID");
            query.AppendLine("     ,   WW.CONSIGNOR_ID");
            query.AppendLine("     ,   WW.ERR_MSG ");
            query.AppendLine(" FROM ");
            query.AppendLine("         WW_MAS_LOC_TRANSPORTERS WW ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_CENTERS CENTER ");
            query.AppendLine(" ON ");
            query.AppendLine("         WW.SHIPPER_ID = CENTER.SHIPPER_ID ");
            query.AppendLine("     AND WW.CENTER_ID = CENTER.CENTER_ID  ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         V_SHIP_TO_STORES STORES ");
            query.AppendLine(" ON ");
            query.AppendLine("         WW.SHIPPER_ID = STORES.SHIPPER_ID ");
            query.AppendLine("     AND WW.SHIP_TO_STORE_ID = STORES.SHIP_TO_STORE_ID ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_PREFS PREFS ");
            query.AppendLine(" ON ");
            query.AppendLine("         PREFS.SHIPPER_ID = STORES.SHIPPER_ID ");
            query.AppendLine("     AND PREFS.PREF_ID = STORES.SHIP_TO_PREF_ID ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_TRANSPORTERS TRANSPORTER ");
            query.AppendLine(" ON ");
            query.AppendLine("         WW.SHIPPER_ID = TRANSPORTER.SHIPPER_ID ");
            query.AppendLine("     AND WW.TRANSPORTER_ID = TRANSPORTER.TRANSPORTER_ID ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_TRANSPORTERS TRANSPORTER_SUN ");
            query.AppendLine(" ON ");
            query.AppendLine("         WW.SHIPPER_ID = TRANSPORTER_SUN.SHIPPER_ID ");
            query.AppendLine("     AND WW.TRANSPORTER_ID_SUN = TRANSPORTER_SUN.TRANSPORTER_ID ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_TRANSPORTERS TRANSPORTER_MON ");
            query.AppendLine(" ON ");
            query.AppendLine("         WW.SHIPPER_ID = TRANSPORTER_MON.SHIPPER_ID ");
            query.AppendLine("     AND WW.TRANSPORTER_ID_MON = TRANSPORTER_MON.TRANSPORTER_ID ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_TRANSPORTERS TRANSPORTER_TUE ");
            query.AppendLine(" ON ");
            query.AppendLine("         WW.SHIPPER_ID = TRANSPORTER_TUE.SHIPPER_ID ");
            query.AppendLine("     AND WW.TRANSPORTER_ID_TUE = TRANSPORTER_TUE.TRANSPORTER_ID ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_TRANSPORTERS TRANSPORTER_WED ");
            query.AppendLine(" ON ");
            query.AppendLine("         WW.SHIPPER_ID = TRANSPORTER_WED.SHIPPER_ID ");
            query.AppendLine("     AND WW.TRANSPORTER_ID_WED = TRANSPORTER_WED.TRANSPORTER_ID ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_TRANSPORTERS TRANSPORTER_THU ");
            query.AppendLine(" ON ");
            query.AppendLine("         WW.SHIPPER_ID = TRANSPORTER_THU.SHIPPER_ID ");
            query.AppendLine("     AND WW.TRANSPORTER_ID_THU = TRANSPORTER_THU.TRANSPORTER_ID ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_TRANSPORTERS TRANSPORTER_FRI ");
            query.AppendLine(" ON ");
            query.AppendLine("         WW.SHIPPER_ID = TRANSPORTER_FRI.SHIPPER_ID ");
            query.AppendLine("     AND WW.TRANSPORTER_ID_FRI = TRANSPORTER_FRI.TRANSPORTER_ID ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_TRANSPORTERS TRANSPORTER_SAT ");
            query.AppendLine(" ON ");
            query.AppendLine("         WW.SHIPPER_ID = TRANSPORTER_SAT.SHIPPER_ID ");
            query.AppendLine("     AND WW.TRANSPORTER_ID_SAT = TRANSPORTER_SAT.TRANSPORTER_ID ");
            query.AppendLine(" LEFT JOIN ");
            query.AppendLine("         M_TRANSPORTERS TRANSPORTER_HOL ");
            query.AppendLine(" ON ");
            query.AppendLine("         WW.SHIPPER_ID = TRANSPORTER_HOL.SHIPPER_ID  ");
            query.AppendLine("     AND WW.TRANSPORTER_ID_HOL = TRANSPORTER_HOL.TRANSPORTER_ID ");
            query.AppendLine(" WHERE ");
            query.AppendLine("         WW.SHIPPER_ID = :SHIPPER_ID ");
            query.AppendLine("     AND WW.SEQ = :SEQ ");
            query.AppendLine(" ORDER BY ");
            query.AppendLine("         WW.NO ASC ");

            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":SEQ", conditions.WorkId);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<IndexResultRow>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", conditions.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (conditions.Page - 1) * conditions.PageSize });

            // Fill data to memory
            var results = MvcDbContext.Current.Database.Connection.Query<ViewModels.LocTransporter.IndexResultRow>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<ViewModels.LocTransporter.IndexResultRow>(results, conditions.Page, conditions.PageSize, totalCount);
        }

        /// <summary>
        /// 配送業者チェック
        /// </summary>
        /// <param name="transporterId"></param>
        /// <param name="resource"></param>
        /// <returns></returns>

        private string CheckTransporter(string transporterId, string resource)
        {
            string errMsg = null;
            //配送業者未入力
            if (string.IsNullOrWhiteSpace(transporterId))
            {
                errMsg = string.Format(MessagesResource.Required, resource);
            }
            //配送業者マスタに存在しない配送業者が入力されている
            else if (!string.IsNullOrWhiteSpace(transporterId) && !MvcDbContext.Current.Transporters.Where(x => x.ShipperId == Profile.User.ShipperId & x.TransporterId == transporterId).Any())
            {
                errMsg = string.Format(MessagesResource.MasterNotExistsError, resource);
            }
            return errMsg;
        }

        /// <summary>
        /// リードタイムチェック
        /// </summary>
        /// <param name="transporterId"></param>
        /// <param name="resource"></param>
        /// <returns></returns>

        private string CheckLeadTimes(string leadTimes, string resource)
        {
            ExcelReader<MasLocTransporter> er = new ExcelReader<MasLocTransporter>();
            string errMsg = null;
            //リードタイム未入力
            if (string.IsNullOrWhiteSpace(leadTimes))
            {
                errMsg = string.Format(MessagesResource.Required, resource);
            }
            else if (!er.CheckNumber(leadTimes, true) || Int32.Parse(leadTimes) < 0)
            {
                errMsg = string.Format(MessagesResource.NotIntError, resource);
            }
            else if (leadTimes.Trim().Length > 2)
            {
                errMsg = string.Format(MessagesResource.MaxLength, resource, "2");
            }
            return errMsg;
        }

        /// <summary>
        /// 佐川顧客コードチェック
        /// </summary>
        /// <param name="clientCd"></param>
        /// <param name="transPorterId"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        private string CheckClientCd(string centerId, string clientCd, string transPorterId)
        {
            //配送業者が佐川の場合のみ必須項目チェック
            if (transPorterId == "02" && string.IsNullOrWhiteSpace(clientCd))
            {
                return string.Format(MessagesResource.Required, LocTransporterResource.ClientCdSagawa);
            }
            //佐川荷送人マスタチェック
            if (!string.IsNullOrWhiteSpace(clientCd) && !MvcDbContext.Current.ConsignorsSagawas.Where(m => m.ShipperId == Profile.User.ShipperId && m.CenterId == centerId && m.ClientCd == clientCd).Any())
            {
                return string.Format(MessagesResource.MasterNotExistsError, LocTransporterResource.ClientCdSagawa);
            }
            return null;
        }

        /// <summary>
        /// 浪速管理IDチェック
        /// </summary>
        /// <param name="clientCd"></param>
        /// <param name="transPorterId"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        private string CheckControlId(string centerId, string controlId, string transPorterId)
        {
            //配送業者が浪速運送の場合のみ必須項目チェック
            if (transPorterId == "03" && string.IsNullOrWhiteSpace(controlId))
            {
                return string.Format(MessagesResource.Required, LocTransporterResource.ClientCdNaniwa);
            }
            //荷送人マスタ(浪速)チェック
            if (!string.IsNullOrWhiteSpace(controlId) && !MvcDbContext.Current.ConsignorsNaniwa.Where(m => m.ShipperId == Profile.User.ShipperId && m.CenterId == centerId && m.ControlId == controlId).Any())
            {
                return string.Format(MessagesResource.MasterNotExistsError, LocTransporterResource.ClientCdNaniwa);
            }
            return null;
        }

        private string CheckExistsNaniwaSorting(string transPorterId, string storeId)
        {
            //浪速仕分コードマスタ存在チェック　※配送業者が浪速運送の場合のみ
            if (transPorterId == "03" && !MvcDbContext.Current.NaniwaSorting.Where(x => x.ShipperId == Profile.User.ShipperId && x.StoreId == storeId).Any())
            {
                return string.Format(MessageResource.ERR_LOC_TRANSPORTER_NANIWA);
            }
            return null;
        }

        /// <summary>
        /// 荷送人コード 存在チェック(ワールドサプライ)
        /// </summary>
        /// <param name="centerId"></param>
        /// <param name="ConsignorId"></param>
        /// <param name="transPorterId"></param>
        /// <returns></returns>
        private string CheckConsignorId(string centerId, string ConsignorId, string transPorterId)
        {
            //配送業者がワールドサプライの場合のみ必須項目チェック
            if (transPorterId == "08" && string.IsNullOrWhiteSpace(ConsignorId))
            {
                return string.Format(MessagesResource.Required, LocTransporterResource.ClientCdWs);
            }
            //荷送人マスタ(WS)チェック
            if (!string.IsNullOrWhiteSpace(ConsignorId) && !MvcDbContext.Current.ConsignorsWorld.Where(m => m.ShipperId == Profile.User.ShipperId && m.CenterId == centerId && m.ConsignorId == ConsignorId).Any())
            {
                return string.Format(MessagesResource.MasterNotExistsError, LocTransporterResource.ClientCdWs);
            }
            return null;
        }

        /// <summary>
        /// ファイル内重複チェック
        /// </summary>
        /// <param name="report"></param>
        /// <param name="data"></param>
        /// <returns></returns>

        private string CheckPrimaryKey(List<MasLocTransporter> report, MasLocTransporter data)
        {
            if (report.Where(x => x.CenterId == data.CenterId
                               && x.ShipToStoreId == data.ShipToStoreId
                               && x.StartDate == data.StartDate).Count() > 1)
            {
                return string.Format(MessageResource.ErrorUploadUniqueConstraintViolated, LocTransporterResource.ShipFromCenter + "－" + LocTransporterResource.ShipToStore + "－" + LocTransporterResource.StartDate);
            }

            return null;
        }
    }
}