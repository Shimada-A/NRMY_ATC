namespace Wms.Areas.Ship.Query.UploadCaseInstruction
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Extensions.Classes;
    using Share.Helpers;
    using Wms.Areas.Ship.Models;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.UploadCaseInstruction;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.UploadCaseInstruction.UploadCaseInstructionSearchConditions;

    public class Report : BaseQuery
    {
        /// <summary>
        /// Excelに出力するデータを取得する。(ケース出荷)
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<UploadCaseInstructionReportCase> UploadCaseInstructionListingCase()
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                    SELECT 
                            '' AS SHIP_PLAN_DATE
                        ,   '' AS BOX_NO
                        ,   '' AS SHIP_TO_STORE_ID
                        ,   '' AS PRIORIT_ORDER
                    FROM DUAL
                ");

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<UploadCaseInstructionReportCase>(query.ToString(), parameters);
        }

        /// <summary>
        /// Excelに出力するデータを取得する。(JAN抜き取り)
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<UploadCaseInstructionReportJan> UploadCaseInstructionListingJan()
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                    SELECT 
                            '' AS SHIP_PLAN_DATE
                        ,   '' AS SHIP_TO_STORE_ID
                        ,   '' AS PRIORIT_ORDER
                        ,   '' AS JAN
                        ,   '' AS INSTRUCT_QTY
                    FROM DUAL
                ");
            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<UploadCaseInstructionReportJan>(query.ToString(), parameters);
        }

        /// <summary>
        ///直近取込エラーデータExcelに出力する出荷種別を取得する
        /// </summary>
        /// <param name="search"></param>
        public int GetShipKind(UploadCaseInstructionSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                    SELECT 
                            SHIP_KIND
                    FROM
                            WW_SHP_CASE_INSTRUCTION
                    WHERE
                            SHIPPER_ID = :SHIPPER_ID
                        AND CENTER_ID = :CENTER_ID
                        AND ERR_CLASS <> 0
                        AND SEQ = ( SELECT
                                        MAX(SEQ)
                                    FROM 
                                        WW_SHP_CASE_INSTRUCTION
                                    WHERE
                                        SHIPPER_ID = :SHIPPER_ID
                                    AND CENTER_ID = :CENTER_ID
                                    AND ERR_CLASS <> 0
                                   )
                    ORDER BY
                            LINE_NO
                ");
            parameters.Add(":CENTER_ID", condition.HidCenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<int>(query.ToString(), parameters).FirstOrDefault();
        }

        /// <summary>
        /// 直近取込エラーデータExcelに出力するデータを取得する。(ケース出荷)
        /// </summary>
        /// <param name="search"></param>
        public IEnumerable<UploadCaseErrReportCase> UploadCaseErrListingCase(UploadCaseInstructionSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                    SELECT 
                            TO_CHAR(MAKE_DATE,'YYYY/MM/DD HH24:MI:SS') AS MAKE_DATE
                        ,   MAKE_USER_ID
                        ,   SHIP_INSTRUCT_NAME
                        ,   LINE_NO
                        ,   TO_CHAR(SHIP_PLAN_DATE,'YYYY/MM/DD') AS SHIP_PLAN_DATE
                        ,   BOX_NO
                        ,   SHIP_TO_STORE_ID
                        ,   PRIORIT_ORDER
                        ,   ERR_MESSAGE
                    FROM
                            WW_SHP_CASE_INSTRUCTION
                    WHERE
                            SHIPPER_ID = :SHIPPER_ID
                        AND CENTER_ID = :CENTER_ID
                        AND ERR_CLASS <> 0
                        AND SEQ = ( SELECT
                                        MAX(SEQ)
                                    FROM 
                                        WW_SHP_CASE_INSTRUCTION
                                    WHERE
                                        SHIPPER_ID = :SHIPPER_ID
                                    AND CENTER_ID = :CENTER_ID
                                    AND ERR_CLASS <> 0
                                   )
                    ORDER BY
                            LINE_NO
                ");
            parameters.Add(":CENTER_ID", condition.HidCenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<UploadCaseErrReportCase>(query.ToString(), parameters);
        }

        /// <summary>
        /// 直近取込エラーデータExcelに出力するデータを取得する。(JAN抜き取り)
        /// </summary>
        /// <param name="search"></param>
        public IEnumerable<UploadCaseErrReportJan> UploadCaseErrListingJan(UploadCaseInstructionSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                    SELECT 
                            TO_CHAR(MAKE_DATE,'YYYY/MM/DD HH24:MI:SS') AS MAKE_DATE
                        ,   MAKE_USER_ID
                        ,   SHIP_INSTRUCT_NAME
                        ,   LINE_NO
                        ,   TO_CHAR(SHIP_PLAN_DATE,'YYYY/MM/DD') AS SHIP_PLAN_DATE
                        ,   SHIP_TO_STORE_ID
                        ,   PRIORIT_ORDER
                        ,   JAN
                        ,   INSTRUCT_QTY
                        ,   ERR_MESSAGE
                    FROM
                            WW_SHP_CASE_INSTRUCTION
                    WHERE
                            SHIPPER_ID = :SHIPPER_ID
                        AND CENTER_ID = :CENTER_ID
                        AND ERR_CLASS <> 0
                        AND SEQ = ( SELECT
                                        MAX(SEQ)
                                    FROM 
                                        WW_SHP_CASE_INSTRUCTION
                                    WHERE
                                        SHIPPER_ID = :SHIPPER_ID
                                    AND CENTER_ID = :CENTER_ID
                                    AND ERR_CLASS <> 0
                                   )
                    ORDER BY
                            LINE_NO
                ");
            parameters.Add(":CENTER_ID", condition.HidCenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<UploadCaseErrReportJan>(query.ToString(), parameters);
        }

        /// <summary>
        /// アップロードされたデータのImport
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        public void InsertWW_INPORT_CASE_INS(IEnumerable<ViewModels.UploadCaseInstruction.UploadCaseInstructionReportCase> report, out string message, out long workId, string pShipInstructName, string pCenterId)
        {
            var dbContext = MvcDbContext.Current;
            workId = GetWorkId();
            try
            {
                using (var trans = dbContext.Database.BeginTransaction())
                {
                    foreach (var u in report.Select((v, i) => new { v, i }))
                    {
                        var wwShpCaseInstruction = new Models.ShpCaseInstruction
                        {
                            Seq = workId,
                            CenterId = pCenterId,
                            LineNo = u.i + 1,
                            ShipInstructName = pShipInstructName,
                            ShipKind = 4,
                            ShipPlanDate = System.DateTime.Parse(u.v.ShipPlanDate),
                            BoxNo = u.v.BoxNo,
                            ShipToStoreId = u.v.ShipToStoreId,
                            PrioritOrder = int.Parse(u.v.PrioritOrder),
                            ErrClass = 0
                        };
                        wwShpCaseInstruction.SetBaseInfoInsert();
                        dbContext.ShpCaseInstructions.Add(wwShpCaseInstruction);
                    }
                    dbContext.SaveChanges();
                    trans.Commit();
                }
                message = string.Empty;
            }
            catch (Exception ex)
            {
                message = UploadCaseInstructionResource.ERR_IMP_DATA_TYPE;
            }
        }

        /// <summary>
        /// アップロードされたデータのImport
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        public void InsertWW_INPORT_JAN_INS(IEnumerable<ViewModels.UploadCaseInstruction.UploadCaseInstructionReportJan> report, out string message, out long workId, string pShipInstructName, string pCenterId)
        {
            var dbContext = MvcDbContext.Current;
            workId = GetWorkId();
            try
            {
                using (var trans = dbContext.Database.BeginTransaction())
                {
                    foreach (var u in report.Select((v, i) => new { v, i }))
                    {
                        var wwShpCaseInstruction = new Models.ShpCaseInstruction
                        {
                            Seq = workId,
                            CenterId = pCenterId,
                            LineNo = u.i + 1,
                            ShipInstructName = pShipInstructName,
                            ShipKind = 5,
                            ShipPlanDate = System.DateTime.Parse(u.v.ShipPlanDate),
                            ShipToStoreId = u.v.ShipToStoreId,
                            PrioritOrder = int.Parse(u.v.PrioritOrder),
                            Jan = u.v.Jan,
                            InstructQty = int.Parse(u.v.InstructQty),
                            ErrClass = 0
                        };
                        wwShpCaseInstruction.SetBaseInfoInsert();
                        dbContext.ShpCaseInstructions.Add(wwShpCaseInstruction);
                    }
                    dbContext.SaveChanges();
                    trans.Commit();
                }
                message = string.Empty;
            }
            catch (Exception ex)
            {
                message = UploadCaseInstructionResource.ERR_IMP_DATA_TYPE;
            }
        }
    }
}