using Dapper;
using Share.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Wms.Areas.Stock.ViewModels.ResearchReference;
using Wms.Common;
using Wms.Models;
using static Wms.Areas.Stock.ViewModels.ResearchReference.ResearchReferenceSearchConditions;

namespace Wms.Areas.Stock.Query.ResearchReference
{
    public static class ResearchReferenceQuery
    {
        private const string GenCenterId = "@@@";

        private const string RegisterDiviCd = "1";

        private const string GenDivCd1 = "REGIST_CLASS";

        private const string GenDivCd2 = "LIST_PRN_FLAG";

        private const string GenDivCd3 = "SUPPORT_STATUS";

        private const string GenRegisterDivCode = "1";

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
                Mvc.Common.AppError.PutLogREF(ex, "CanChangeCenter");
                throw ex;
            };

        }

        public static List<ResearchReferenceResultRow> GetResultRowList([Required] ResearchReferenceSearchConditions condition, bool meisaiFlg)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            var query = new StringBuilder();
            query.Append(GetSelecttPhrase());
            query.AppendLine(GetFromWherePhrase(condition));

            query.AppendLine(") SUB");
            //ダウンロード以外の場合、明細は表示しない
            if (!meisaiFlg)
            {
                query.AppendLine("WHERE SUB.SLIP_NO_ROW = 1");
            }

            query.AppendLine("OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");

            query.Replace("@ORDER_BY@", GetOrderBy(condition));

            return MvcDbContext.Current.Database.Connection.Query<ResearchReferenceResultRow>(query.ToString(), CreateParam(condition)).ToList();
        }

        public static List<ResearchReferenceResultRow> GetResultReportRowList([Required] ResearchReferenceSearchConditions condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            var query = new StringBuilder();
            query.Append(GetSelecttPhrase());
            query.AppendLine(GetFromWherePhrase(condition));

            query.AppendLine(") SUB");

            query.AppendLine("OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");

            query.Replace("@ORDER_BY@", "SUB.OCCURRED_DATE_TIME, SUB.SLIP_NO, SUB.SKU");

            return MvcDbContext.Current.Database.Connection.Query<ResearchReferenceResultRow>(query.ToString(), CreateParam(condition)).ToList();
        }

        public static string GetSelecttPhrase() {
            var query = new StringBuilder();
            query.AppendLine(@"SELECT 
                                     ROW_NUMBER() OVER (ORDER BY @ORDER_BY@) AS ROW_NUMBER,
                                     SUB.CENTER_ID AS CENTER_ID,
                                     SUB.OCCURRED_DATE_TIME AS OCCURRED_DATE_TIME,
                                     SUB.REGIST_CLASS AS REGIST_CLASS,
                                     SUB.REGIST_USER_ID AS REGIST_USER_ID,
                                     SUB.REGIST_USER_NAME AS REGIST_USER_NAME,
                                     SUB.BATCH_NO AS BATCH_NO,
                                     SUB.GAS_BATCH_NO AS GAS_BATCH_NO,
                                     SUB.FRONTAGE_NO AS FRONTAGE_NO,
                                     SUB.SLIP_NO AS SLIP_NO,
                                     SUB.LOCATION_CD AS LOCATION_CD,
                                     SUB.BOX_NO AS BOX_NO,
                                     SUB.SKU AS SKU,
                                     SUB.JAN AS JAN,
                                     SUB.ITEM_ID AS ITEM_ID,
                                     SUB.COLOR_ID AS COLOR_ID,
                                     SUB.SIZE_ID AS SIZE_ID,
                                     SUB.GRADE_ID AS GRADE_ID,
                                     SUB.SHIPPING_STORE_ID AS SHIPPING_STORE_ID,
                                     SUB.DIFF_QUANTITY AS DIFF_QUANTITY,
                                     SUB.LIST_PRINT_FLAG AS LIST_PRINT_FLAG,
                                     SUB.STATUS AS STATUS,
                                     SUB.INVOICE_NO AS INVOICE_NO,
                                     SUB.SLIP_SEQUENCE AS SLIP_SEQUENCE,
                                     SUB.RESEARCH_DATE_TIME AS RESEARCH_DATE_TIME,
                                     SUB.RESEARCH_USER_ID AS RESEARCH_USER_ID,
                                     SUB.RESEARCH_LOCATION_CD AS RESEARCH_LOCATION_CD,
                                     SUB.SLIP_NO_ROW AS SLIP_NO_ROW,
                                     SUB.RESEARCH_NOTE AS RESEARCH_NOTE,
                                     SUB.ITEM_NAME AS ITEM_NAME,
                                     SUB.COLOR_NAME AS COLOR_NAME,
                                     SUB.SIZE_NAME AS SIZE_NAME,
                                     SUB.VENDOR_NAME AS VENDOR_NAME,
                                     SUB.GRADE_NAME AS GRADE_NAME,
                                     SUB.SHIPPING_STORE_NAME AS SHIPPING_STORE_NAME,
                                     SUB.REGIST_CLASS_NAME AS REGIST_CLASS_NAME,
                                     SUB.LIST_PRINT_FLAG_NAME AS LIST_PRINT_FLAG_NAME,
                                     SUB.STATUS_NAME AS STATUS_NAME,
                                     SUB.RESEARCH_USER_NAME AS RESEARCH_USER_NAME
                                     FROM 
                                     (
                             ");

            query.AppendLine("SELECT");
            query.AppendLine("TO_DATE(TO_CHAR(TSRC.MAKE_DATE, 'YYYYMMDDHH24MISS'),'YYYY/MM/DD HH24:MI:SS') AS OCCURRED_DATE_TIME,");
            query.AppendLine("TSRC.CENTER_ID AS CENTER_ID,");
            query.AppendLine("TSRC.REGIST_CLASS AS REGIST_CLASS,");
            query.AppendLine("TSRC.REGIST_USER_ID AS REGIST_USER_ID,");
            query.AppendLine("REGIST_USER.USER_NAME AS REGIST_USER_NAME,");
            query.AppendLine("TSRC.BATCH_NO AS BATCH_NO,");
            query.AppendLine("TSRC.GAS_BATCH_NO AS GAS_BATCH_NO,");
            query.AppendLine("TSRC.FRONTAGE_NO AS FRONTAGE_NO,");
            query.AppendLine("TSRC.SLIP_NO AS SLIP_NO,");
            query.AppendLine("TSRC.LOCATION_CD AS LOCATION_CD,");
            query.AppendLine("TSRC.BOX_NO AS BOX_NO,");
            query.AppendLine("TSRC.ITEM_SKU_ID AS SKU,");
            query.AppendLine("TSRC.JAN AS JAN,");
            query.AppendLine("TSRC.ITEM_ID AS ITEM_ID,");
            query.AppendLine("TSRC.ITEM_COLOR_ID AS COLOR_ID,");
            query.AppendLine("TSRC.ITEM_SIZE_ID AS SIZE_ID,");
            query.AppendLine("TSRC.GRADE_ID,");
            query.AppendLine("TSRC.SHIP_TO_LOC_ID AS SHIPPING_STORE_ID,");
            query.AppendLine("TSRC.DIFF_QTY AS DIFF_QUANTITY,");
            query.AppendLine("TSRC.LIST_PRN_FLAG AS LIST_PRINT_FLAG,");
            query.AppendLine("TSRC.STATUS AS STATUS,");
            query.AppendLine("CASE WHEN TSRC.REGIST_CLASS = 4 THEN TSRC.SHIP_INSTRUCT_ID ELSE TSRC.INVOICE_NO END AS INVOICE_NO,");
            query.AppendLine("TSRCD.SLIP_SEQ AS SLIP_SEQUENCE,");
            query.AppendLine("TSRCD.RESEARCH_DATE AS RESEARCH_DATE_TIME,");
            query.AppendLine("TSRCD.RESEARCH_USER_ID AS RESEARCH_USER_ID,");
            query.AppendLine("TSRCD.LOCATION_CD AS RESEARCH_LOCATION_CD,");
            query.AppendLine("ROW_NUMBER() OVER (PARTITION BY TSRC.SLIP_NO ORDER BY TSRCD.SLIP_SEQ) AS SLIP_NO_ROW,");
            query.AppendLine("CASE COUNT(*) OVER (PARTITION BY TSRC.SLIP_NO ORDER BY TSRCD.SLIP_NO ) WHEN 1 THEN TSRCD.RESEARCH_NOTE");
            query.AppendLine("ELSE TSRCD.RESEARCH_NOTE || '　他あり'");
            query.AppendLine("END AS RESEARCH_NOTE,");
            query.AppendLine("MIS.ITEM_NAME AS ITEM_NAME,");
            query.AppendLine("MCO.ITEM_COLOR_NAME AS COLOR_NAME,");
            query.AppendLine("MIS.ITEM_SIZE_NAME AS SIZE_NAME,");
            query.AppendLine("MVE.VENDOR_NAME1 AS VENDOR_NAME,");
            query.AppendLine("MGR.GRADE_NAME AS GRADE_NAME,");
            query.AppendLine("MST.SHIP_TO_STORE_SHORT_NAME AS SHIPPING_STORE_NAME,");
            query.AppendLine("MGE1.GEN_NAME AS REGIST_CLASS_NAME,");
            query.AppendLine("MGE2.GEN_NAME AS LIST_PRINT_FLAG_NAME,");
            query.AppendLine("MGE3.GEN_NAME AS STATUS_NAME,");
            query.AppendLine("MGR.DISPLAY_ORDER AS DISPLAY_ORDER,");
            query.AppendLine("RESEARCH_USER.USER_NAME AS RESEARCH_USER_NAME");

            return query.ToString();
        }

        public static ResearchReferenceResultCount GetResultCount([Required] ResearchReferenceSearchConditions condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            var query = new StringBuilder();

            query.AppendLine("SELECT");
            query.AppendLine("COUNT(*) AS COUNT,");
            query.AppendLine("COUNT(DISTINCT(SUB.ITEM_SKU_ID)) AS COUNT_SKU");
            query.AppendLine("FROM(");
            query.AppendLine("SELECT");
            query.AppendLine("    MAX(TSRC.ITEM_SKU_ID) ITEM_SKU_ID");

            query.AppendLine(GetFromWherePhrase(condition));

            query.AppendLine("GROUP BY");
            query.AppendLine("TSRC.SLIP_NO");
            query.AppendLine(")SUB");
            return MvcDbContext.Current.Database.Connection.QueryFirst<ResearchReferenceResultCount>(query.ToString(), CreateParam(condition));
        }

        private static string GetFromWherePhrase(ResearchReferenceSearchConditions condition)
        {
            var query = new StringBuilder();

            query.AppendLine("FROM T_STOCK_RESEARCH_CHECK TSRC");
            query.AppendLine("LEFT OUTER JOIN T_STOCK_RESEARCH_CHECK_D TSRCD ON");
            query.AppendLine("TSRC.SHIPPER_ID = TSRCD.SHIPPER_ID AND");
            query.AppendLine("TSRC.CENTER_ID = TSRCD.CENTER_ID AND");
            query.AppendLine("TSRC.SLIP_NO = TSRCD.SLIP_NO");
            query.AppendLine("LEFT OUTER JOIN M_ITEM_SKU MIS ON");
            query.AppendLine("TSRC.SHIPPER_ID = MIS.SHIPPER_ID AND");
            query.AppendLine("TSRC.ITEM_SKU_ID = MIS.ITEM_SKU_ID");
            query.AppendLine("LEFT OUTER JOIN M_COLORS MCO ON");
            query.AppendLine("TSRC.SHIPPER_ID = MCO.SHIPPER_ID AND");
            query.AppendLine("TSRC.ITEM_COLOR_ID = MCO.ITEM_COLOR_ID");
            query.AppendLine("LEFT OUTER JOIN M_VENDORS MVE ON");
            query.AppendLine("MIS.SHIPPER_ID = MVE.SHIPPER_ID AND");
            query.AppendLine("MIS.MAIN_VENDOR_ID = MVE.VENDOR_ID");
            query.AppendLine("LEFT OUTER JOIN M_GRADES MGR ON");
            query.AppendLine("TSRC.SHIPPER_ID = MGR.SHIPPER_ID AND");
            query.AppendLine("TSRC.GRADE_ID = MGR.GRADE_ID");
            query.AppendLine("LEFT OUTER JOIN V_SHIP_TO_STORES MST ON");
            query.AppendLine("TSRC.SHIPPER_ID = MST.SHIPPER_ID AND");
            query.AppendLine("TSRC.SHIP_TO_LOC_ID = MST.SHIP_TO_STORE_ID");
            query.AppendLine("LEFT OUTER JOIN M_GENERALS MGE1 ON");
            query.AppendLine("TSRC.SHIPPER_ID = MGE1.SHIPPER_ID AND");
            query.AppendLine("TO_CHAR(TSRC.REGIST_CLASS) = MGE1.GEN_CD AND");
            query.AppendLine("MGE1.CENTER_ID =:GEN_CENTER_ID AND");
            query.AppendLine("MGE1.REGISTER_DIVI_CD = :REGISTER_DIVI_CD AND");
            query.AppendLine("MGE1.GEN_DIV_CD = :GEN_DIV_CD1");
            query.AppendLine("LEFT OUTER JOIN M_GENERALS MGE2 ON");
            query.AppendLine("TSRC.SHIPPER_ID = MGE2.SHIPPER_ID AND");
            query.AppendLine("TSRC.LIST_PRN_FLAG = MGE2.GEN_CD AND");
            query.AppendLine("MGE2.CENTER_ID =:GEN_CENTER_ID AND");
            query.AppendLine("MGE2.REGISTER_DIVI_CD = :REGISTER_DIVI_CD AND");
            query.AppendLine("MGE2.GEN_DIV_CD = :GEN_DIV_CD2");
            query.AppendLine("LEFT OUTER JOIN M_GENERALS MGE3 ON");
            query.AppendLine("TSRC.SHIPPER_ID = MGE3.SHIPPER_ID AND");
            query.AppendLine("TSRC.STATUS = MGE3.GEN_CD AND");
            query.AppendLine("MGE3.CENTER_ID =:GEN_CENTER_ID AND");
            query.AppendLine("MGE3.REGISTER_DIVI_CD = :REGISTER_DIVI_CD AND");
            query.AppendLine("MGE3.GEN_DIV_CD = :GEN_DIV_CD3");
            query.AppendLine("LEFT OUTER JOIN M_USERS REGIST_USER ON ");
            query.AppendLine("TSRC.SHIPPER_ID = REGIST_USER.SHIPPER_ID AND ");
            query.AppendLine("TSRC.REGIST_USER_ID = REGIST_USER.USER_ID ");
            query.AppendLine("LEFT OUTER JOIN M_USERS RESEARCH_USER ON ");
            query.AppendLine("TSRCD.SHIPPER_ID = RESEARCH_USER.SHIPPER_ID AND ");
            query.AppendLine("TSRCD.RESEARCH_USER_ID = RESEARCH_USER.USER_ID ");
            query.AppendLine("WHERE");
            query.AppendLine("TSRC.SHIPPER_ID = :SHIPPER_ID AND");

            //すべて選択済みまたは未選択の場合は条件を設定しない
            if (condition.StatusNot && !condition.StatusDuring && !condition.StatusComplete)
            {
                query.AppendLine("TO_NUMBER(TSRC.STATUS) = 0 AND");
            }
             else if (!condition.StatusNot && condition.StatusDuring && !condition.StatusComplete)
            {
                query.AppendLine("TO_NUMBER(TSRC.STATUS) = 1 AND");
            }
            else if (!condition.StatusNot && !condition.StatusDuring && condition.StatusComplete)
            {
                query.AppendLine("TO_NUMBER(TSRC.STATUS) = 9 AND");
            }
            else if (condition.StatusNot && condition.StatusDuring && !condition.StatusComplete)
            {
                query.AppendLine("TO_NUMBER(TSRC.STATUS) IN (0,1) AND");
            }
            else if (!condition.StatusNot && condition.StatusDuring && condition.StatusComplete)
            {
                query.AppendLine("TO_NUMBER(TSRC.STATUS) IN (1,9) AND");
            }
            else if (condition.StatusNot && !condition.StatusDuring && condition.StatusComplete)
            {
                query.AppendLine("TO_NUMBER(TSRC.STATUS) IN (0,9) AND");
            }

            AppendCondition(query, "TO_DATE(TO_CHAR(TSRC.MAKE_DATE, 'YYYYMMDDHH24MISS'),'YYYY/MM/DD HH24:MI:SS') >= :MAKE_DATE_FROM AND", condition.GetOccurredDateTimeFrom());
            AppendCondition(query, "TO_DATE(TO_CHAR(TSRC.MAKE_DATE, 'YYYYMMDDHH24MISS'),'YYYY/MM/DD HH24:MI:SS') <= :MAKE_DATE_TO AND", condition.GetGetOccurredDateTimeTo());
            AppendCondition(query, "TSRC.REGIST_CLASS = :REGIST_CLASS AND", condition.GetRegistClassInt());
            AppendCondition(query, "TSRC.LOCATION_CD = :LOCATION_CD AND", condition.LocationCd);
            AppendCondition(query, "TSRC.SHIP_TO_LOC_ID LIKE :SHIP_TO_LOC_ID AND", condition.ShippingStoreId);
            AppendCondition(query, "MST.SHIP_TO_STORE_SHORT_NAME LIKE :STORE_SHORT_NAME AND", condition.ShippingStoreName);
            AppendCondition(query, "TSRC.JAN LIKE :JAN AND", condition.Jan);
            AppendCondition(query, "TSRC.ITEM_SKU_ID LIKE :ITEM_SKU_ID AND", condition.Sku);
            AppendCondition(query, "TSRC.GRADE_ID = :GRADE_ID AND", condition.GradeId);
            AppendCondition(query, "TSRC.REGIST_USER_ID = :REGIST_USER_ID AND", condition.RegistUserId);
            AppendCondition(query, "TSRC.BATCH_NO = :BATCH_NO AND", condition.BatchNo);
            AppendCondition(query, "TSRC.INVOICE_NO = :INVOICE_NO AND", condition.InvoiceNo);
            AppendCondition(query, "TSRC.BOX_NO = :BOX_NO AND", condition.BoxNo);

            query.AppendLine("TSRC.CENTER_ID = :CENTER_ID");

            return query.ToString();
        }

        private static DynamicParameters CreateParam(ResearchReferenceSearchConditions condition)
        {
            var param = new DynamicParameters();

            param.AddDynamicParams(new
            {
                SHIPPER_ID = Profile.User.ShipperId,
                CENTER_ID = condition.CenterId,
                GEN_CENTER_ID = GenCenterId,
                REGISTER_DIVI_CD = RegisterDiviCd,
                GEN_DIV_CD1 = GenDivCd1,
                GEN_DIV_CD2 = GenDivCd2,
                GEN_DIV_CD3 = GenDivCd3,
                STATUS = condition.Status,
                MAKE_DATE_FROM = condition.GetOccurredDateTimeFrom(),
                MAKE_DATE_TO = condition.GetGetOccurredDateTimeTo(),
                REGIST_CLASS = condition.RegistClass,
                LOCATION_CD = condition.LocationCd,
                SHIP_TO_LOC_ID = AddPrefixMatch(condition.ShippingStoreId),
                STORE_SHORT_NAME = AddPrefixMatch(condition.ShippingStoreName),
                JAN = AddPrefixMatch(condition.Jan),
                ITEM_SKU_ID = AddPrefixMatch(condition.Sku),
                GRADE_ID = condition.GradeId,
                REGIST_USER_ID = condition.RegistUserId,
                BATCH_NO = condition.BatchNo,
                INVOICE_NO = condition.InvoiceNo,
                BOX_NO = condition.BoxNo,
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

        private static void AppendCondition(StringBuilder query, string phrase, int? condition)
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

        private static string GetOrderBy(ResearchReferenceSearchConditions condition)
        {
            const string SortOrderDate = "SUB.OCCURRED_DATE_TIME";
            const string SortOrderLocation = "SUB.LOCATION_CD";
            const string SortOrderSku = "SUB.SKU";
            const string SortOrderGrade = "SUB.DISPLAY_ORDER";
            const string SortSlipNo = "SUB.SLIP_NO";
            var orderBy = "{1} {0}, {2} {0}, {3} {0}, {4} {0}";
            var ascOrDesc = condition.AscDescSort == ResearchReferenceAscDescSort.Asc ? "ASC" : "DESC";

            if (condition.SortOrder == ResearchReferenceSortOrder.DateLocationSkuGrade)
            {
                return string.Format(orderBy, ascOrDesc, SortOrderDate, SortOrderLocation, SortOrderSku, SortOrderGrade);
            }
            else if (condition.SortOrder == ResearchReferenceSortOrder.DateSkuLocationGrade)
            {
                return string.Format(orderBy, ascOrDesc, SortOrderDate, SortOrderSku, SortOrderLocation, SortOrderGrade);
            }
            else if (condition.SortOrder == ResearchReferenceSortOrder.SlipNo)
            {
                return string.Format(orderBy, ascOrDesc, SortSlipNo,SortOrderDate, SortOrderLocation, SortOrderSku);
            }
            else
            {
                return string.Format(orderBy, ascOrDesc, SortOrderSku, SortOrderLocation, SortOrderGrade, SortOrderDate);
            }
        }


        public static List<ResearchReferenceCheckD> GetT_STOCK_REDEARCH_CHECK_D(ResearchReferenceSearchConditions condition, string slipNo)
        {
            DynamicParameters parameters = new DynamicParameters();
            var query = new StringBuilder();
            query.AppendLine(@"
            SELECT SLIP_NO
                 , SLIP_SEQ
                 , BATCH_NO
                 , ITEM_SKU_ID
                 , JAN
                 , ITEM_ID
                 , ITEM_COLOR_ID
                 , ITEM_SIZE_ID
                 , LOCATION_CD
                 , GRADE_ID
                 , QTY
                 , RESEARCH_NOTE
                 , RESEARCH_USER_ID
                 , USER_NAME AS RESEARCH_USER_NAME
                 , RESEARCH_DATE
                 , ASSORT_STOCK_OUT_QTY
                 , MG.GEN_NAME AS RESEARCH_CLASS_NAME
              FROM T_STOCK_RESEARCH_CHECK_D TSRCD
         LEFT JOIN M_GENERALS MG
                ON MG.SHIPPER_ID = TSRCD.SHIPPER_ID
               AND MG.GEN_DIV_CD = 'RESEARCH_CLASS'
               AND MG.GEN_CD = TO_CHAR(TSRCD.RESEARCH_CLASS)
         LEFT JOIN M_USERS MU
                ON TSRCD.SHIPPER_ID = MU.SHIPPER_ID
               AND TSRCD.RESEARCH_USER_ID = MU.USER_ID
             WHERE TSRCD.SHIPPER_ID = :SHIPPER_ID
               AND TSRCD.CENTER_ID = :CENTER_ID
               AND TSRCD.SLIP_NO = :SLIP_NO");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SLIP_NO", slipNo);
            try
            {
                return MvcDbContext.Current.Database.Connection.Query<ResearchReferenceCheckD>(query.ToString(), parameters).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 発行フラグ更新
        /// </summary>
        /// <returns></returns>
        public static string UpdateFlg(List<ResearchReferenceResultRow> result)
        {
            using (System.Data.Common.DbTransaction tran = MvcDbContext.Current.Database.Connection.BeginTransaction())
            {
                try
                {
                    //発行対象のデータ分ループしてフラグ更新
                    for (int i = 0; i < result.Count; i++)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        StringBuilder query = new StringBuilder();
                        query.Append(@"
                            UPDATE T_STOCK_RESEARCH_CHECK
                            SET
                                    UPDATE_DATE         = SYSTIMESTAMP
                                ,   UPDATE_USER_ID      = :UPDATE_USER_ID
                                ,   UPDATE_PROGRAM_NAME = :UPDATE_PROGRAM_NAME
                                ,   UPDATE_COUNT        = UPDATE_COUNT + 1
                                ,   LIST_PRN_FLAG       = 1
                            WHERE
                                    SHIPPER_ID = :SHIPPER_ID
                                AND CENTER_ID = :CENTER_ID
                                AND SLIP_NO = :SLIP_NO
                             ");
                        parameters.Add(":UPDATE_USER_ID", Common.Profile.User.UserId);
                        parameters.Add(":UPDATE_PROGRAM_NAME", "ResearchReference");
                        parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                        parameters.Add(":CENTER_ID", Common.Profile.User.CenterId);
                        parameters.Add(":SLIP_NO", result[i].SlipNo);

                        MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
                tran.Commit();
            }
            return null;
        }

        /// <summary>
        /// 在庫調査確認更新処理
        /// </summary>
        /// <param name="CenterId"></param>
        /// <param name="slipNo"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public static ProcedureStatus UpdateStatus(string CenterId, string slipNo)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_SLIP_NO", slipNo, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_RESEARCH_CHECK",
                param,
                commandType: CommandType.StoredProcedure);
            ProcedureStatus status = param.Get<ProcedureStatus>("OUT_STATUS");
            string message = param.Get<string>("OUT_MESSAGE");
            return status;
        }


    }

}