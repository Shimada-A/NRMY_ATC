namespace Wms.Areas.Ship.Query.TransporterChng
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Common;
    using Share.Extensions.Classes;
    using Share.Models;
    using Wms.Areas.Ship.Models;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.TransporterChng;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.TransporterChng.TransporterChngSearchConditions;

    public class TransporterChngQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertShpTransporterChng(TransporterChngSearchConditions condition)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    // 1.ワークID採番
                    condition.Seq = new BaseQuery().GetWorkId();
                    condition.Page = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                        INSERT INTO WW_SHP_TRANSPORTER_CHNG (
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
                            ,   SHIP_PLAN_DATE
                            ,   INSTRUCT_CLASS_NAME
                            ,   EMERGENCY_CLASS_NAME
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   SHIP_TO_STORE_ID
                            ,   SHIP_TO_STORE_NAME
                            ,   TRANSPORTER_ID
                            ,   TRANSPORTER_NAME
                            ,   ALLOC_FLAG
                            ,   BATCH_NO
                            ,   BOX_NO
                            ,   UPDATE_COUNT_SHIP
                            ,   UPDATE_COUNT_PACKAGE
                            ,   PROCESS_CLASS
                        )
                        SELECT 
                              " + "SYSTIMESTAMP " + " AS MAKE_DATE" +
                              ", '" + Common.Profile.User.UserId + "' AS MAKE_USER_ID" +
                              ",'TransporterChng'" + " AS MAKE_PROGRAM_NAME" +
                              ",SYSTIMESTAMP " + "AS UPDATE_DATE" +
                              ", '" + Common.Profile.User.UserId + "' AS UPDATE_USER_ID" +
                              ",'TransporterChng'" + " AS UPDATE_PROGRAM_NAME" +
                              ",0" + " AS UPDATE_COUNT" +
                              "," + "'" + Common.Profile.User.ShipperId + "'" + " AS SHIPPER_ID" +
                              "," + condition.Seq + " AS SEQ");
                    query.Append(@"
                            ,   ROWNUM AS LINE_NO
                            ,   0 AS IS_CHECK
                            ,   TS.CENTER_ID
                            ,   TS.SHIP_PLAN_DATE
                            ,   MG1.GEN_NAME INSTRUCT_CLASS_NAME
                            ,   MG2.GEN_NAME EMERGENCY_CLASS_NAME
                            ,   TS.SHIP_INSTRUCT_ID
                            ,   TS.SHIP_INSTRUCT_SEQ
                            ,   TS.SHIP_TO_STORE_ID
                            ,   VSTS.SHIP_TO_STORE_NAME1 SHIP_TO_STORE_NAME ");

                    if (condition.AllocStatus == AllocStatusEnum.UnAlloc)
                    {
                        query.Append(@" 
                            ,   TS.TRANSPORTER_ID
                            ,   MT.TRANSPORTER_NAME 
                            ,   TS.ALLOC_FLAG
                            ,   NULL BATCH_NO
                            ,   NULL BOX_NO
                            ,   TS.UPDATE_COUNT UPDATE_COUNT_SHIP
                            ,   NULL UPDATE_COUNT_PACKAGE
                            ,   1 PROCESS_CLASS
                        ");
                    }
                    else
                    {
                        query.Append(@" 
                            ,   TSPI.TRANSPORTER_ID
                            ,   NVL(MT.TRANSPORTER_NAME, MT_SHP.TRANSPORTER_NAME) AS TRANSPORTER_NAME
                            ,   TS.ALLOC_FLAG
                            ,   TS.BATCH_NO
                            ,   TSPI.BOX_NO
                            ,   TS.UPDATE_COUNT UPDATE_COUNT_SHIP
                            ,   TSPI.UPDATE_COUNT UPDATE_COUNT_PACKAGE
                            ,   2 PROCESS_CLASS
                        ");
                    }
                    query.Append(" FROM T_SHIPS TS ");

                    if (condition.AllocStatus == AllocStatusEnum.Alloced)
                    {
                        query.Append(@" 
                            LEFT JOIN
                                    T_SHIP_PACKING_INFO TSPI
                            ON
                                    TS.SHIPPER_ID = TSPI.SHIPPER_ID
                                AND TS.CENTER_ID = TSPI.CENTER_ID
                                AND TS.SHIP_INSTRUCT_ID = TSPI.SHIP_INSTRUCT_ID
                                AND TS.SHIP_INSTRUCT_SEQ = TSPI.SHIP_INSTRUCT_SEQ
                        ");
                    }
                    query.Append(@" 
                        LEFT JOIN
                                M_GENERALS MG1
                        ON
                                TS.SHIPPER_ID = MG1.SHIPPER_ID
                            AND MG1.REGISTER_DIVI_CD = '1'
                            AND MG1.CENTER_ID = '@@@'
                            AND MG1.GEN_DIV_CD = 'INSTRUCT_CLASS'
                            AND TO_CHAR(TS.INSTRUCT_CLASS) = MG1.GEN_CD
                        LEFT JOIN
                                M_GENERALS MG2
                        ON
                                TS.SHIPPER_ID = MG2.SHIPPER_ID
                            AND MG2.REGISTER_DIVI_CD = '1'
                            AND MG2.CENTER_ID = '@@@'
                            AND MG2.GEN_DIV_CD = 'EMERGENCY_CLASS'
                            AND TO_CHAR(TS.EMERGENCY_CLASS) = MG2.GEN_CD
                        LEFT JOIN
                                V_SHIP_TO_STORES VSTS
                        ON
                                TS.SHIPPER_ID = VSTS.SHIPPER_ID
                            AND TS.SHIP_TO_STORE_ID  = VSTS.SHIP_TO_STORE_ID ");
                    if (condition.AllocStatus == AllocStatusEnum.UnAlloc)
                    {
                        query.Append(@" 
                            LEFT JOIN
                                    M_TRANSPORTERS MT
                            ON
                                    TS.SHIPPER_ID   = MT.SHIPPER_ID
                                AND TS.TRANSPORTER_ID = MT.TRANSPORTER_ID
                        ");
                    }
                    else
                    {
                        query.Append(@" 
                            LEFT JOIN
                                    M_TRANSPORTERS MT
                            ON
                                    TSPI.SHIPPER_ID = MT.SHIPPER_ID
                                AND TSPI.TRANSPORTER_ID = MT.TRANSPORTER_ID
                            LEFT JOIN
                                    M_TRANSPORTERS MT_SHP
                            ON
                                    TS.SHIPPER_ID   = MT_SHP.SHIPPER_ID
                                AND TS.TRANSPORTER_ID = MT_SHP.TRANSPORTER_ID
                        ");
                    }
                    query.Append(@"
                         WHERE TS.SHIPPER_ID = :SHIPPER_ID
                           AND TS.CENTER_ID = :CENTER_ID ");

                    if (condition.AllocStatus == AllocStatusEnum.UnAlloc)
                    {
                        query.Append(" AND TS.ALLOC_FLAG = 0 ");
                    }
                    else
                    {
                        query.Append(@" AND TS.ALLOC_FLAG = 1");
                        query.Append(@" AND TS.COMPLETE_FLAG = 0");
                        query.Append(@" AND NVL(TSPI.DELI_PRN_FLAG, 0) = 0 ");
                        query.Append(@" AND NVL(TSPI.KAKU_FLAG, 0) = 0");
                    }
                    parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                    parameters.Add(":CENTER_ID", condition.CenterId);

                    // Add search condition
                    // 受信日時(From-To)
                    if (condition.AllocDateFrom != null)
                    {
                        query.Append(" AND TO_CHAR(TS.MAKE_DATE,'yyyy/MM/dd hh24:mi:ss') >= :ALLOC_DATE_TIME_FROM ");
                        parameters.Add(":ALLOC_DATE_TIME_FROM", condition.AllocTimeFrom == null ?
                                                                string.Format("{0:yyyy/MM/dd}", condition.AllocDateFrom) + " 00:00:00" :
                                                                string.Format("{0:yyyy/MM/dd}", condition.AllocDateFrom) + " " + condition.AllocTimeFrom);
                    }

                    if (condition.AllocDateTo != null)
                    {
                        query.Append(" AND TO_CHAR(TS.MAKE_DATE,'yyyy/MM/dd hh24:mi:ss') <= :ALLOC_DATE_TIME_TO ");
                        parameters.Add(":ALLOC_DATE_TIME_TO", condition.AllocTimeTo == null ?
                                                                string.Format("{0:yyyy/MM/dd}", condition.AllocDateTo) + " 23:59:59" :
                                                                string.Format("{0:yyyy/MM/dd}", condition.AllocDateTo) + " " + condition.AllocTimeTo);
                    }

                    // 出荷予定日(From-To)
                    if (condition.ShipPlanDateFrom != null)
                    {
                        query.Append(" AND TS.SHIP_PLAN_DATE >= :SHIP_PLAN_DATE_FROM ");
                        parameters.Add(":SHIP_PLAN_DATE_FROM", condition.ShipPlanDateFrom);
                    }

                    if (condition.ShipPlanDateTo != null)
                    {
                        query.Append(" AND TS.SHIP_PLAN_DATE <= :SHIP_PLAN_DATE_TO ");
                        parameters.Add(":SHIP_PLAN_DATE_TO", condition.ShipPlanDateTo);
                    }

                    // 納品日
                    if (condition.DeliveryDate != null)
                    {
                        query.Append(" AND TS.DELIVERY_DATE = :DELIVERY_DATE ");
                        parameters.Add(":DELIVERY_DATE", condition.DeliveryDate);
                    }

                    // 出荷先区分
                    if (!string.IsNullOrEmpty(condition.ShipToClass))
                    {
                        query.Append(" AND TS.STORE_CLASS = :STORE_CLASS ");
                        parameters.Add(":STORE_CLASS", condition.ShipToClass);
                    }

                    // 出荷先
                    if (!string.IsNullOrEmpty(condition.ShipToId))
                    {
                        string[] shipToIds = (condition.ShipToId ?? condition.ShipToId).Split(',');
                        query.Append(" AND TS.SHIP_TO_STORE_ID IN :SHIP_TO_STORE_ID ");
                        parameters.Add(":SHIP_TO_STORE_ID", shipToIds);
                    }

                    // 配送業者
                    if (!string.IsNullOrEmpty(condition.TransporterId))
                    {
                        string[] transporterIds = (condition.TransporterId ?? condition.TransporterId).Split(',');
                        if(condition.AllocStatus == AllocStatusEnum.UnAlloc)
                        {
                            query.Append(" AND TS.TRANSPORTER_ID IN :TRANSPORTER_ID ");
                        }
                        else
                        {
                            query.Append(" AND NVL(TSPI.TRANSPORTER_ID, TS.TRANSPORTER_ID) IN :TRANSPORTER_ID ");
                        }
                        parameters.Add(":TRANSPORTER_ID", transporterIds);
                    }

                    // 配送エリア
                    if (!string.IsNullOrEmpty(condition.TransporterArea))
                    {
                        string[] transporterAreas = (condition.TransporterArea ?? condition.TransporterArea).Split(',');
                        query.Append(" AND TS.PREF_ID IN :PREF_ID ");
                        parameters.Add(":PREF_ID", transporterAreas);
                    }

                    // 出荷指示ID
                    if (!string.IsNullOrEmpty(condition.ShipInstructId))
                    {
                        query.Append(" AND TS.SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID ");
                        parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId);
                    }

                    // ケースNo
                    if (!string.IsNullOrEmpty(condition.BoxNo) && condition.AllocStatus == AllocStatusEnum.Alloced)
                    {
                        query.Append(" AND TSPI.BOX_NO = :BOX_NO ");
                        parameters.Add(":BOX_NO", condition.BoxNo);
                    }
                    query.Append(@" 
                        ORDER BY
                                TS.SHIP_INSTRUCT_ID
                            ,   TS.SHIP_INSTRUCT_SEQ
                    ");

                    // 2.検索・ワーク作成
                    var result = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }
                trans.Commit();
                return true;
            }
        }

        /// <summary>
        /// Get StockInquiry List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<TransporterChngResultRow> GetData(TransporterChngSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT *
                  FROM WW_SHP_TRANSPORTER_CHNG
                 WHERE SHIPPER_ID = :SHIPPER_ID
                   AND SEQ = :SEQ
                 ORDER BY
                        SHIP_INSTRUCT_ID
                    ,   SHIP_INSTRUCT_SEQ ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<TransporterChngResultRow>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var TransporterChngs = MvcDbContext.Current.Database.Connection.Query<TransporterChngResultRow>(query.ToString(), parameters);
            condition.SelectedCnt = MvcDbContext.Current.ShpTransporterChngs.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId && x.IsCheck).Count();

            // Excute paging
            return new StaticPagedList<TransporterChngResultRow>(TransporterChngs, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Update Work Table
        /// </summary>
        /// <param name="StockInquirys"></param>
        /// <returns></returns>
        public bool UpdateShpTransporterChng(IList<SelectedTransporterChngViewModel> ShpTransporterChngs)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in ShpTransporterChngs)
                {
                    // 在庫明細
                    var shpTransporterChng = MvcDbContext.Current.ShpTransporterChngs
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == u.Seq && m.LineNo == u.LineNo)
                                  .SingleOrDefault();

                    if (shpTransporterChng == null)
                    {
                        return false;
                    }

                    shpTransporterChng.SetBaseInfoUpdate();
                    shpTransporterChng.IsCheck = u.IsCheck;
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
        /// Update Work Table
        /// </summary>
        /// <param name="StockInquirys"></param>
        /// <returns></returns>
        public bool ShpTransporterChngAllChange(TransporterChngSearchConditions conditions, bool check)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query;
                    query = new StringBuilder(@"
                        UPDATE  WW_SHP_TRANSPORTER_CHNG
                        SET
                                UPDATE_DATE = :UPDATE_DATE
                            ,   UPDATE_USER_ID = :UPDATE_USER_ID
                            ,   UPDATE_PROGRAM_NAME = 'TransporterChng'
                            ,   UPDATE_COUNT = UPDATE_COUNT + 1
                            ,   IS_CHECK = :IS_CHECK
                        WHERE
                                SEQ = :SEQ
                            AND SHIPPER_ID = :SHIPPER_ID
                    ");
                    parameters.Add(":UPDATE_DATE", DateTimeOffset.Now);
                    parameters.Add(":UPDATE_USER_ID", Profile.User.UserId);
                    parameters.Add(":IS_CHECK", (check == true) ? 1 : 0);
                    parameters.Add(":SEQ", conditions.Seq);
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                    var result = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }
                trans.Commit();
            }

            conditions.SelectedCnt = MvcDbContext.Current.ShpTransporterChngs
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq && m.IsCheck).Count();
            return true;
        }

        /// <summary>
        /// 実績更新
        /// </summary>
        public void TransporterChange(TransporterChngSearchConditions searchConditions, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", searchConditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_WORK_ID", searchConditions.Seq, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_PROCESS_CLASS", searchConditions.AllocStatus, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_CHANGED_TRANSPORTER_ID", searchConditions.ChangedTransporterId, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_TRANSPORTER_CHNG1",
                param,
                commandType: CommandType.StoredProcedure); 
            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }

        /// <summary>
        /// 実績更新
        /// </summary>
        public void TransporterChange2(TransporterChngSearchConditions searchConditions, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", searchConditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_NO", searchConditions.CaseNo, DbType.String, ParameterDirection.Input);
            param.Add("IN_CHANGED_TRANSPORTER_ID", searchConditions.ChangedTransporterIdPrint, DbType.String, ParameterDirection.Input);
            param.Add("IN_SIZE", searchConditions.Size, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_TRANSPORTER_CHNG2",
                param,
                commandType: CommandType.StoredProcedure);
            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }

        /// <summary>
        /// 事業部データ取得
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

        /// <summary>
        /// サイズチェック
        /// </summary>
        public int CheckSize(string size)
        {
            return MvcDbContext.Current.Generals
                .Where(m => m.ShipperId == Profile.User.ShipperId 
                         && m.RegisterDiviCd == "1" 
                         && m.CenterId == "@@@" 
                         && m.GenDivCd == "YAMATO_BOX_SIZE" 
                         && m.GenCd == size)
                .Count();
        }
    }
}