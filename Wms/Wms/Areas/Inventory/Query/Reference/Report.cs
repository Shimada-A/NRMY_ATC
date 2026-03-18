namespace Wms.Areas.Inventory.Query.Reference
{
    using System.Collections.Generic;
    using System.Text;
    using Dapper;
    using Wms.Areas.Inventory.Resources;
    using Wms.Areas.Inventory.ViewModels.Reference;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Inventory.ViewModels.Reference.Reference01SearchConditions;
    using static Wms.Areas.Inventory.ViewModels.Reference.Reference02SearchConditions;

    public class Report : BaseQuery
    {
        /// <summary>
        /// 棚卸進捗照会（棚卸No）に出力するデータを取得する。
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<ReferenceReport> Reference01Listing(Reference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT TIP.CENTER_ID
                      ,to_char(TIP.INVENTORY_START_DATE, 'yyyy/mm/dd') INVENTORY_START_DATE
                      ,TIP.INVENTORY_NO
                      ,TIP.INVENTORY_NAME
                      ,TIP.INVENTORY_SEQ
                      ,ML.LOCSEC_1 AREA
                      ,ML.LOCSEC_2 INVENTORY_ROW
                      ,TIP.LOCATION_CD
                      ,MG.GRADE_NAME GRADE
                      ,CASE WHEN TIP.BOX_NO = ' ' THEN '2' ELSE '1' END CASE_ID
                      ,CASE WHEN TIP.BOX_NO = ' ' THEN '" + ReferenceResource.Rose + @"'
                                                  ELSE '" + ReferenceResource.Cases + @"' END CASE_NAME
                      ,TIP.BOX_NO
                      ,MIS.DIVISION_ID
                      ,MD.DIVISION_NAME
                      ,MIS.CATEGORY_ID1
                      ,MIC.CATEGORY_NAME1
                      ,MIS.CATEGORY_ID2
                      ,MIC.CATEGORY_NAME2
                      ,MIS.CATEGORY_ID3
                      ,MIC.CATEGORY_NAME3
                      ,MIS.CATEGORY_ID4
                      ,MIC.CATEGORY_NAME4
                      ,TIP.ITEM_ID
                      ,TIP.ITEM_NAME
                      ,TIP.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,TIP.ITEM_SIZE_ID
                      ,MIS.ITEM_SIZE_NAME
                      ,TIP.JAN
                      ,TIP.STOCK_QTY_START
                      ,CASE WHEN TIR.RESULT_QTY = 0 THEN NULL ELSE TIR.RESULT_QTY END RESULT_QTY
                      ,TIR.RESULT_QTY - TIP.STOCK_QTY_START DIFFERENCE_QTY
                      ,TIR.COUNT_SEQ
                      ,TIR.MAKE_USER_ID USER_ID
                      ,MU.USER_NAME
                  FROM 
                        T_INVENTORY_PLANS TIP
                    INNER JOIN
                        M_LOCATIONS ML
                    ON
                            ML.SHIPPER_ID = TIP.SHIPPER_ID
                        AND ML.CENTER_ID = TIP.CENTER_ID
                        AND ML.LOCATION_CD = TIP.LOCATION_CD
                    INNER JOIN
                       WW_INV_REFERENCE01 WW
                    ON
                            TIP.SHIPPER_ID = WW.SHIPPER_ID
                        AND TIP.CENTER_ID = WW.CENTER_ID
                        AND TIP.INVENTORY_NO = WW.INVENTORY_NO
                        AND WW.SEQ = :SEQ
                        AND WW.IS_CHECK = 1
                  LEFT JOIN
                       T_INVENTORY_RESULTS TIR
                    ON TIP.SHIPPER_ID = TIR.SHIPPER_ID
                   AND TIP.CENTER_ID = TIR.CENTER_ID
                   AND TIP.INVENTORY_NO = TIR.INVENTORY_NO
                   AND TIP.INVENTORY_SEQ = TIR.INVENTORY_SEQ
                   AND TIR.LAST_COUNT_FLAG = 1
                  LEFT JOIN
                       M_GRADES MG
                    ON TIP.SHIPPER_ID = MG.SHIPPER_ID
                   AND TIP.GRADE_ID = MG.GRADE_ID
                  LEFT JOIN
                       M_ITEM_SKU MIS
                    ON TIP.SHIPPER_ID = MIS.SHIPPER_ID
                   AND TIP.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                  LEFT JOIN
                       M_DIVISIONS MD
                    ON MIS.SHIPPER_ID = MD.SHIPPER_ID
                   AND MIS.DIVISION_ID = MD.DIVISION_ID
                  LEFT JOIN
                       M_ITEM_CATEGORIES4 MIC
                    ON MIS.SHIPPER_ID = MIC.SHIPPER_ID
                   AND MIS.CATEGORY_ID1 = MIC.CATEGORY_ID1
                   AND MIS.CATEGORY_ID2 = MIC.CATEGORY_ID2
                   AND MIS.CATEGORY_ID3 = MIC.CATEGORY_ID3
                   AND MIS.CATEGORY_ID4 = MIC.CATEGORY_ID4
                  LEFT JOIN
                       M_COLORS MC
                    ON TIP.SHIPPER_ID = MC.SHIPPER_ID
                   AND TIP.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                  LEFT JOIN
                       M_SIZES MS
                    ON TIP.SHIPPER_ID = MS.SHIPPER_ID
                   AND TIP.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                  LEFT JOIN
                       M_USERS MU
                    ON TIR.SHIPPER_ID = MU.SHIPPER_ID
                   AND TIR.MAKE_USER_ID = MU.USER_ID
                 WHERE TIP.SHIPPER_ID = :SHIPPER_ID
                 ORDER BY 
                       TIP.CENTER_ID
                      ,TIP.INVENTORY_START_DATE
                      ,TIP.INVENTORY_NO
                      ,TIP.INVENTORY_NAME
                      ,TIP.INVENTORY_SEQ
                      ,TIP.LOCATION_CD
                      ,MG.GRADE_NAME
                      ,TIP.BOX_NO
                      ,MIS.DIVISION_ID
                      ,MD.DIVISION_NAME
                      ,MIS.CATEGORY_ID1
                      ,MIC.CATEGORY_NAME1
                      ,MIS.CATEGORY_ID2
                      ,MIC.CATEGORY_NAME2
                      ,MIS.CATEGORY_ID3
                      ,MIC.CATEGORY_NAME3
                      ,MIS.CATEGORY_ID4
                      ,MIC.CATEGORY_NAME4
                      ,TIP.ITEM_ID
                      ,TIP.ITEM_NAME
                      ,TIP.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,TIP.ITEM_SIZE_ID
                      ,MIS.ITEM_SIZE_NAME
                      ,TIP.JAN
                      ,TIP.STOCK_QTY_START
                      ,TIR.RESULT_QTY
                      ,TIR.COUNT_SEQ
                      ,TIR.MAKE_USER_ID
                      ,MU.USER_NAME");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<ReferenceReport>(query.ToString(), parameters);
        }

        /// <summary>
        /// 棚卸進捗照会（ロケ別）に出力するデータを取得する。
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<ReferenceReport> Reference02Listing(Reference02SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT TIP.CENTER_ID
                      ,to_char(TIP.INVENTORY_START_DATE, 'yyyy/mm/dd') INVENTORY_START_DATE
                      ,TIP.INVENTORY_NO
                      ,TIP.INVENTORY_NAME
                      ,TIP.INVENTORY_SEQ
                      ,ML.LOCSEC_1 AREA
                      ,ML.LOCSEC_2 INVENTORY_ROW
                      ,TIP.LOCATION_CD
                      ,MG.GRADE_NAME GRADE
                      ,CASE WHEN TIP.BOX_NO = ' ' THEN '2' ELSE '1' END CASE_ID
                      ,CASE WHEN TIP.BOX_NO = ' ' THEN '" + ReferenceResource.Rose + @"'
                                                  ELSE '" + ReferenceResource.Cases + @"' END CASE_NAME
                      ,TIP.BOX_NO
                      ,MIS.DIVISION_ID
                      ,MD.DIVISION_NAME
                      ,MIS.CATEGORY_ID1
                      ,MIC.CATEGORY_NAME1
                      ,MIS.CATEGORY_ID2
                      ,MIC.CATEGORY_NAME2
                      ,MIS.CATEGORY_ID3
                      ,MIC.CATEGORY_NAME3
                      ,MIS.CATEGORY_ID4
                      ,MIC.CATEGORY_NAME4
                      ,TIP.ITEM_ID
                      ,TIP.ITEM_NAME
                      ,TIP.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,TIP.ITEM_SIZE_ID
                      ,MIS.ITEM_SIZE_NAME
                      ,TIP.JAN
                      ,TIP.STOCK_QTY_START
                      ,CASE WHEN TIR.RESULT_QTY = 0 THEN NULL ELSE TIR.RESULT_QTY END RESULT_QTY
                      ,TIR.RESULT_QTY - TIP.STOCK_QTY_START DIFFERENCE_QTY
                      ,TIR.COUNT_SEQ
                      ,TIR.MAKE_USER_ID USER_ID
                      ,MU.USER_NAME
                  FROM 
                        T_INVENTORY_PLANS TIP
                    INNER JOIN
                        M_LOCATIONS ML
                    ON
                            ML.SHIPPER_ID = TIP.SHIPPER_ID
                        AND ML.CENTER_ID = TIP.CENTER_ID
                        AND ML.LOCATION_CD = TIP.LOCATION_CD
                    INNER JOIN
                       WW_INV_REFERENCE02 WW
                    ON
                            TIP.SHIPPER_ID = WW.SHIPPER_ID
                        AND TIP.CENTER_ID = WW.CENTER_ID
                        AND TIP.INVENTORY_NO = WW.INVENTORY_NO
                        AND TIP.LOCATION_CD = WW.LOCATION_CD
                        AND TIP.CASE_CLASS = WW.CASE_CLASS
                        AND TIP.GRADE_ID = WW.GRADE_ID
                        AND WW.SEQ = :SEQ
                  LEFT JOIN
                       T_INVENTORY_RESULTS TIR
                    ON TIP.SHIPPER_ID = TIR.SHIPPER_ID
                   AND TIP.CENTER_ID = TIR.CENTER_ID
                   AND TIP.INVENTORY_NO = TIR.INVENTORY_NO
                   AND TIP.INVENTORY_SEQ = TIR.INVENTORY_SEQ
                   AND TIR.LAST_COUNT_FLAG = 1
                  LEFT JOIN
                       M_GRADES MG
                    ON TIP.SHIPPER_ID = MG.SHIPPER_ID
                   AND TIP.GRADE_ID = MG.GRADE_ID
                  LEFT JOIN
                       M_ITEM_SKU MIS
                    ON TIP.SHIPPER_ID = MIS.SHIPPER_ID
                   AND TIP.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                  LEFT JOIN
                       M_DIVISIONS MD
                    ON MIS.SHIPPER_ID = MD.SHIPPER_ID
                   AND MIS.DIVISION_ID = MD.DIVISION_ID
                  LEFT JOIN
                       M_ITEM_CATEGORIES4 MIC
                    ON MIS.SHIPPER_ID = MIC.SHIPPER_ID
                   AND MIS.CATEGORY_ID1 = MIC.CATEGORY_ID1
                   AND MIS.CATEGORY_ID2 = MIC.CATEGORY_ID2
                   AND MIS.CATEGORY_ID3 = MIC.CATEGORY_ID3
                   AND MIS.CATEGORY_ID4 = MIC.CATEGORY_ID4
                  LEFT JOIN
                       M_COLORS MC
                    ON TIP.SHIPPER_ID = MC.SHIPPER_ID
                   AND TIP.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                  LEFT JOIN
                       M_SIZES MS
                    ON TIP.SHIPPER_ID = MS.SHIPPER_ID
                   AND TIP.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                  LEFT JOIN
                       M_USERS MU
                    ON TIR.SHIPPER_ID = MU.SHIPPER_ID
                   AND TIR.MAKE_USER_ID = MU.USER_ID

                 WHERE TIP.SHIPPER_ID = :SHIPPER_ID
                 ORDER BY 
                       TIP.CENTER_ID
                      ,TIP.INVENTORY_START_DATE
                      ,TIP.INVENTORY_NO
                      ,TIP.INVENTORY_NAME
                      ,TIP.INVENTORY_SEQ
                      ,TIP.LOCATION_CD
                      ,MG.GRADE_NAME
                      ,TIP.BOX_NO
                      ,MIS.DIVISION_ID
                      ,MD.DIVISION_NAME
                      ,MIS.CATEGORY_ID1
                      ,MIC.CATEGORY_NAME1
                      ,MIS.CATEGORY_ID2
                      ,MIC.CATEGORY_NAME2
                      ,MIS.CATEGORY_ID3
                      ,MIC.CATEGORY_NAME3
                      ,MIS.CATEGORY_ID4
                      ,MIC.CATEGORY_NAME4
                      ,TIP.ITEM_ID
                      ,TIP.ITEM_NAME
                      ,TIP.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,TIP.ITEM_SIZE_ID
                      ,MIS.ITEM_SIZE_NAME
                      ,TIP.JAN
                      ,TIP.STOCK_QTY_START
                      ,TIR.RESULT_QTY
                      ,TIR.COUNT_SEQ
                      ,TIR.MAKE_USER_ID
                      ,MU.USER_NAME");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq2);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<ReferenceReport>(query.ToString(), parameters);
        }

        /// <summary>
        /// 棚卸進捗照会（SKU明細）に出力するデータを取得する。
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<ReferenceReport> Reference03Listing(Reference02SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT TIP.CENTER_ID
                      ,to_char(TIP.INVENTORY_START_DATE, 'yyyy/mm/dd') INVENTORY_START_DATE
                      ,TIP.INVENTORY_NO
                      ,TIP.INVENTORY_NAME
                      ,TIP.INVENTORY_SEQ
                      ,ML.LOCSEC_1 AREA
                      ,ML.LOCSEC_2 INVENTORY_ROW
                      ,TIP.LOCATION_CD
                      ,MG.GRADE_NAME GRADE
                      ,CASE WHEN TIP.BOX_NO = ' ' THEN '2' ELSE '1' END CASE_ID
                      ,CASE WHEN TIP.BOX_NO = ' ' THEN '" + ReferenceResource.Rose + @"'
                                                  ELSE '" + ReferenceResource.Cases + @"' END CASE_NAME
                      ,TIP.BOX_NO
                      ,MIS.DIVISION_ID
                      ,MD.DIVISION_NAME
                      ,MIS.CATEGORY_ID1
                      ,MIC.CATEGORY_NAME1
                      ,MIS.CATEGORY_ID2
                      ,MIC.CATEGORY_NAME2
                      ,MIS.CATEGORY_ID3
                      ,MIC.CATEGORY_NAME3
                      ,MIS.CATEGORY_ID4
                      ,MIC.CATEGORY_NAME4
                      ,TIP.ITEM_ID
                      ,TIP.ITEM_NAME
                      ,TIP.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,TIP.ITEM_SIZE_ID
                      ,MIS.ITEM_SIZE_NAME
                      ,TIP.JAN
                      ,TIP.STOCK_QTY_START
                      ,CASE WHEN TIR.RESULT_QTY = 0 THEN NULL ELSE TIR.RESULT_QTY END RESULT_QTY
                      ,TIR.RESULT_QTY - TIP.STOCK_QTY_START DIFFERENCE_QTY
                      ,TIR.COUNT_SEQ
                      ,TIR.MAKE_USER_ID USER_ID
                      ,MU.USER_NAME
                  FROM
                        T_INVENTORY_PLANS TIP
                    INNER JOIN
                        M_LOCATIONS ML
                    ON
                            ML.SHIPPER_ID = TIP.SHIPPER_ID
                        AND ML.CENTER_ID = TIP.CENTER_ID
                        AND ML.LOCATION_CD = TIP.LOCATION_CD
                    INNER JOIN
                        WW_INV_REFERENCE03 WW
                    ON
                            TIP.SHIPPER_ID = WW.SHIPPER_ID
                        AND TIP.CENTER_ID = WW.CENTER_ID
                        AND TIP.INVENTORY_NO = WW.INVENTORY_NO
                        AND TIP.LOCATION_CD = WW.LOCATION_CD
                        AND TIP.ITEM_SKU_ID = WW.ITEM_SKU_ID
                        AND TIP.JAN = WW.JAN
                        AND TIP.CASE_CLASS = WW.CASE_CLASS
                        AND TIP.GRADE_ID = WW.GRADE_ID
                        AND TIP.BOX_NO = WW.BOX_NO
                        AND WW.SEQ = :SEQ
                  LEFT JOIN
                       T_INVENTORY_RESULTS TIR
                    ON TIP.SHIPPER_ID = TIR.SHIPPER_ID
                   AND TIP.CENTER_ID = TIR.CENTER_ID
                   AND TIP.INVENTORY_NO = TIR.INVENTORY_NO
                   AND TIP.INVENTORY_SEQ = TIR.INVENTORY_SEQ
                   AND TIR.LAST_COUNT_FLAG = 1
                  LEFT JOIN
                       M_GRADES MG
                    ON TIP.SHIPPER_ID = MG.SHIPPER_ID
                   AND TIP.GRADE_ID = MG.GRADE_ID
                  LEFT JOIN
                       M_ITEM_SKU MIS
                    ON TIP.SHIPPER_ID = MIS.SHIPPER_ID
                   AND TIP.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                  LEFT JOIN
                       M_DIVISIONS MD
                    ON MIS.SHIPPER_ID = MD.SHIPPER_ID
                   AND MIS.DIVISION_ID = MD.DIVISION_ID
                  LEFT JOIN
                       M_ITEM_CATEGORIES4 MIC
                    ON MIS.SHIPPER_ID = MIC.SHIPPER_ID
                   AND MIS.CATEGORY_ID1 = MIC.CATEGORY_ID1
                   AND MIS.CATEGORY_ID2 = MIC.CATEGORY_ID2
                   AND MIS.CATEGORY_ID3 = MIC.CATEGORY_ID3
                   AND MIS.CATEGORY_ID4 = MIC.CATEGORY_ID4
                  LEFT JOIN
                       M_COLORS MC
                    ON TIP.SHIPPER_ID = MC.SHIPPER_ID
                   AND TIP.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                  LEFT JOIN
                       M_SIZES MS
                    ON TIP.SHIPPER_ID = MS.SHIPPER_ID
                   AND TIP.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                  LEFT JOIN
                       M_USERS MU
                    ON TIR.SHIPPER_ID = MU.SHIPPER_ID
                   AND TIR.MAKE_USER_ID = MU.USER_ID

                 WHERE TIP.SHIPPER_ID = :SHIPPER_ID
                 ORDER BY 
                       TIP.CENTER_ID
                      ,TIP.INVENTORY_START_DATE
                      ,TIP.INVENTORY_NO
                      ,TIP.INVENTORY_NAME
                      ,TIP.INVENTORY_SEQ
                      ,TIP.LOCATION_CD
                      ,MG.GRADE_NAME
                      ,TIP.BOX_NO
                      ,MIS.DIVISION_ID
                      ,MD.DIVISION_NAME
                      ,MIS.CATEGORY_ID1
                      ,MIC.CATEGORY_NAME1
                      ,MIS.CATEGORY_ID2
                      ,MIC.CATEGORY_NAME2
                      ,MIS.CATEGORY_ID3
                      ,MIC.CATEGORY_NAME3
                      ,MIS.CATEGORY_ID4
                      ,MIC.CATEGORY_NAME4
                      ,TIP.ITEM_ID
                      ,TIP.ITEM_NAME
                      ,TIP.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,TIP.ITEM_SIZE_ID
                      ,MIS.ITEM_SIZE_NAME
                      ,TIP.JAN
                      ,TIP.STOCK_QTY_START
                      ,TIR.RESULT_QTY
                      ,TIR.COUNT_SEQ
                      ,TIR.MAKE_USER_ID
                      ,MU.USER_NAME");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq3);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<ReferenceReport>(query.ToString(), parameters);
        }

        /// <summary>
        /// 棚卸進捗照会（棚卸No）に出力するデータを取得する。
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<ReferenceReportForCsv> Reference01ListingForCsv(Reference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT TIP.CENTER_ID || '　' || MCE.CENTER_SHORT_NAME CENTER
                      ,to_char(TIP.INVENTORY_START_DATE, 'yyyy/mm/dd') INVENTORY_START_DATE
                      ,ML.LOCSEC_1 AREA
                      ,ML.LOCSEC_2 INVENTORY_ROW
                      ,TIP.INVENTORY_NO
                      ,TIP.INVENTORY_NAME
                      ,:USER_ID USER_ID
                      ,TIP.GRADE_ID ID
                      ,TIP.LOCATION_CD
                      ,TIP.BOX_NO
                      ,TIP.ITEM_ID
                      ,TIP.ITEM_NAME
                      ,TIP.ITEM_COLOR_ID
                      ,MCO.ITEM_COLOR_NAME
                      ,TIP.ITEM_SIZE_ID
                      ,MIS.ITEM_SIZE_NAME
                      ,TIP.JAN
                      ,TIP.STOCK_QTY_START
                      ,TIR.RESULT_QTY
                      ,CASE WHEN TIR.RESULT_QTY - TIP.STOCK_QTY_START >0 THEN '+'||(TIR.RESULT_QTY - TIP.STOCK_QTY_START) 
                       ELSE TO_CHAR(TIR.RESULT_QTY - TIP.STOCK_QTY_START) END DIFFERENCE_QTY
                      ,CASE WHEN TIP.BOX_NO =' ' AND TIR.MAKE_USER_ID IS NOT NULL THEN MUS.USER_NAME 
                       ELSE RESULT_MUS.USER_NAME END USER_NAME
                      ,TIR.COUNT_SEQ
                  FROM T_INVENTORY_PLANS TIP
                    INNER JOIN
                            M_LOCATIONS ML
                    ON
                            ML.SHIPPER_ID = TIP.SHIPPER_ID
                        AND ML.CENTER_ID = TIP.CENTER_ID
                        AND ML.LOCATION_CD = TIP.LOCATION_CD
                    INNER JOIN 
                       WW_INV_REFERENCE01 WW
                    ON
                            TIP.SHIPPER_ID = WW.SHIPPER_ID
                        AND TIP.CENTER_ID = WW.CENTER_ID
                        AND TIP.INVENTORY_NO = WW.INVENTORY_NO
                        AND WW.SEQ = :SEQ
                        AND WW.IS_CHECK = 1
                  LEFT OUTER JOIN (
                     SELECT SHIPPER_ID
                           ,CENTER_ID
                           ,INVENTORY_NO
                           ,LOCATION_CD
                           ,BOX_NO
                           ,MAX(MAKE_USER_ID) AS MAKE_USER_ID
                     FROM T_INVENTORY_RESULTS
                     WHERE LAST_COUNT_FLAG = 1
                       AND BOX_NO <> ' '
                     GROUP BY SHIPPER_ID
                           ,CENTER_ID
                           ,INVENTORY_NO
                           ,LOCATION_CD
                           ,BOX_NO
                  ) RESULTS
                    ON TIP.SHIPPER_ID = RESULTS.SHIPPER_ID
                   AND TIP.CENTER_ID = RESULTS.CENTER_ID
                   AND TIP.INVENTORY_NO = RESULTS.INVENTORY_NO
                   AND TIP.LOCATION_CD = RESULTS.LOCATION_CD
                   AND TIP.BOX_NO = RESULTS.BOX_NO
                   LEFT JOIN
                       M_USERS RESULT_MUS
                    ON RESULTS.SHIPPER_ID = RESULT_MUS.SHIPPER_ID
                   AND RESULTS.MAKE_USER_ID = RESULT_MUS.USER_ID
                  LEFT JOIN
                       T_INVENTORY_RESULTS TIR
                    ON TIP.SHIPPER_ID = TIR.SHIPPER_ID
                   AND TIP.CENTER_ID = TIR.CENTER_ID
                   AND TIP.INVENTORY_NO = TIR.INVENTORY_NO
                   AND TIP.INVENTORY_SEQ = TIR.INVENTORY_SEQ
                   --AND TIP.STOCK_QTY_START <> TIR.RESULT_QTY
                   AND TIR.LAST_COUNT_FLAG = 1
                  LEFT JOIN
                       M_CENTERS MCE
                    ON TIP.SHIPPER_ID = MCE.SHIPPER_ID
                   AND TIP.CENTER_ID = MCE.CENTER_ID
                  LEFT JOIN
                       M_USERS MUS
                    ON TIR.SHIPPER_ID = MUS.SHIPPER_ID
                   AND TIR.MAKE_USER_ID = MUS.USER_ID
                  LEFT JOIN
                       M_COLORS MCO
                    ON TIP.SHIPPER_ID = MCO.SHIPPER_ID
                   AND TIP.ITEM_COLOR_ID = MCO.ITEM_COLOR_ID
                  LEFT JOIN
                       M_ITEM_SKU MIS
                    ON TIP.SHIPPER_ID = MIS.SHIPPER_ID
                   AND TIP.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                 WHERE TIP.SHIPPER_ID = :SHIPPER_ID
                   AND TIP.STOCK_QTY_START <> NVL(TIR.RESULT_QTY,0)
                 ORDER BY 
                       TIP.INVENTORY_NO
                      ,TIP.LOCATION_CD
                      ,TIP.CASE_CLASS
                      ,TIP.BOX_NO
                      ,TIP.ITEM_ID
                      ,TIP.ITEM_COLOR_ID
                      ,TIP.ITEM_SIZE_ID
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":USER_ID", Common.Profile.User.UserId);
            parameters.Add(":SEQ", condition.Seq);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<ReferenceReportForCsv>(query.ToString(), parameters);
        }

        /// <summary>
        /// 棚卸進捗照会（ロケ別）に出力するデータを取得する。
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<ReferenceReportForCsv> Reference02ListingForCsv(Reference02SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT TIP.CENTER_ID || '　' || MCE.CENTER_SHORT_NAME CENTER
                      ,to_char(TIP.INVENTORY_START_DATE, 'yyyy/mm/dd') INVENTORY_START_DATE
                      ,ML.LOCSEC_1 AREA
                      ,ML.LOCSEC_2 INVENTORY_ROW
                      ,TIP.INVENTORY_NO
                      ,TIP.INVENTORY_NAME
                      ,:USER_ID USER_ID
                      ,TIP.GRADE_ID
                      ,TIP.LOCATION_CD
                      ,TIP.BOX_NO
                      ,TIP.ITEM_ID
                      ,TIP.ITEM_NAME
                      ,TIP.ITEM_COLOR_ID
                      ,MCO.ITEM_COLOR_NAME
                      ,TIP.ITEM_SIZE_ID
                      ,MIS.ITEM_SIZE_NAME
                      ,TIP.JAN
                      ,TIP.STOCK_QTY_START
                      ,TIR.RESULT_QTY
                      ,CASE WHEN TIR.RESULT_QTY - TIP.STOCK_QTY_START >0 THEN '+'||(TIR.RESULT_QTY - TIP.STOCK_QTY_START) 
                       ELSE TO_CHAR(TIR.RESULT_QTY - TIP.STOCK_QTY_START) END DIFFERENCE_QTY
                      ,CASE WHEN TIP.BOX_NO =' ' AND TIR.MAKE_USER_ID IS NOT NULL THEN MUS.USER_NAME 
                       ELSE RESULT_MUS.USER_NAME END USER_NAME
                      ,TIR.COUNT_SEQ
                  FROM T_INVENTORY_PLANS TIP
                    INNER JOIN
                        M_LOCATIONS ML
                    ON
                            ML.SHIPPER_ID = TIP.SHIPPER_ID
                        AND ML.CENTER_ID = TIP.CENTER_ID
                        AND ML.LOCATION_CD = TIP.LOCATION_CD
                    INNER JOIN
                        WW_INV_REFERENCE02 WW
                    ON
                            TIP.SHIPPER_ID = WW.SHIPPER_ID
                        AND TIP.CENTER_ID = WW.CENTER_ID
                        AND TIP.INVENTORY_NO = WW.INVENTORY_NO
                        AND TIP.LOCATION_CD = WW.LOCATION_CD
                        AND TIP.CASE_CLASS = WW.CASE_CLASS
                        AND TIP.GRADE_ID = WW.GRADE_ID
                        AND WW.SEQ = :SEQ
                   LEFT OUTER JOIN (
                     SELECT SHIPPER_ID
                           ,CENTER_ID
                           ,INVENTORY_NO
                           ,LOCATION_CD
                           ,BOX_NO
                           ,MAX(MAKE_USER_ID) AS MAKE_USER_ID
                     FROM T_INVENTORY_RESULTS
                     WHERE LAST_COUNT_FLAG = 1
                       AND BOX_NO <> ' '
                     GROUP BY SHIPPER_ID
                           ,CENTER_ID
                           ,INVENTORY_NO
                           ,LOCATION_CD
                           ,BOX_NO
                  ) RESULTS
                    ON TIP.SHIPPER_ID = RESULTS.SHIPPER_ID
                   AND TIP.CENTER_ID = RESULTS.CENTER_ID
                   AND TIP.INVENTORY_NO = RESULTS.INVENTORY_NO
                   AND TIP.LOCATION_CD = RESULTS.LOCATION_CD
                   AND TIP.BOX_NO = RESULTS.BOX_NO
                   LEFT JOIN
                       M_USERS RESULT_MUS
                    ON RESULTS.SHIPPER_ID = RESULT_MUS.SHIPPER_ID
                   AND RESULTS.MAKE_USER_ID = RESULT_MUS.USER_ID
                  LEFT JOIN
                       T_INVENTORY_RESULTS TIR
                    ON TIP.SHIPPER_ID = TIR.SHIPPER_ID
                   AND TIP.CENTER_ID = TIR.CENTER_ID
                   AND TIP.INVENTORY_NO = TIR.INVENTORY_NO
                   AND TIP.INVENTORY_SEQ = TIR.INVENTORY_SEQ
                   --AND TIP.STOCK_QTY_START <> TIR.RESULT_QTY
                   AND TIR.LAST_COUNT_FLAG = 1
                  LEFT JOIN
                       M_CENTERS MCE
                    ON TIP.SHIPPER_ID = MCE.SHIPPER_ID
                   AND TIP.CENTER_ID = MCE.CENTER_ID
                  LEFT JOIN
                       M_USERS MUS
                    ON TIR.SHIPPER_ID = MUS.SHIPPER_ID
                   AND TIR.MAKE_USER_ID = MUS.USER_ID
                  LEFT JOIN
                       M_COLORS MCO
                    ON TIP.SHIPPER_ID = MCO.SHIPPER_ID
                   AND TIP.ITEM_COLOR_ID = MCO.ITEM_COLOR_ID
                  LEFT JOIN
                       M_ITEM_SKU MIS
                    ON TIP.SHIPPER_ID = MIS.SHIPPER_ID
                   AND TIP.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                 WHERE TIP.SHIPPER_ID = :SHIPPER_ID
                   AND TIP.STOCK_QTY_START <> NVL(TIR.RESULT_QTY,0)
                 ORDER BY 
                       TIP.INVENTORY_NO
                      ,TIP.LOCATION_CD
                      ,TIP.CASE_CLASS
                      ,TIP.BOX_NO
                      ,TIP.ITEM_ID
                      ,TIP.ITEM_COLOR_ID
                      ,TIP.ITEM_SIZE_ID
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":USER_ID", Common.Profile.User.UserId);
            parameters.Add(":SEQ", condition.Seq2);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<ReferenceReportForCsv>(query.ToString(), parameters);
        }

        /// <summary>
        /// 棚卸進捗照会（SKU明細）に出力するデータを取得する。
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<ReferenceReportForCsv> Reference03ListingForCsv(Reference02SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT TIP.CENTER_ID || '　' || MCE.CENTER_SHORT_NAME CENTER
                      ,to_char(TIP.INVENTORY_START_DATE, 'yyyy/mm/dd') INVENTORY_START_DATE
                      ,ML.LOCSEC_1 AREA
                      ,ML.LOCSEC_2 INVENTORY_ROW
                      ,TIP.INVENTORY_NO
                      ,TIP.INVENTORY_NAME
                      ,:USER_ID USER_ID
                      ,TIP.GRADE_ID ID
                      ,TIP.LOCATION_CD
                      ,TIP.BOX_NO
                      ,TIP.ITEM_ID
                      ,TIP.ITEM_NAME
                      ,TIP.ITEM_COLOR_ID
                      ,MCO.ITEM_COLOR_NAME
                      ,TIP.ITEM_SIZE_ID
                      ,MIS.ITEM_SIZE_NAME
                      ,TIP.JAN
                      ,TIP.STOCK_QTY_START
                      ,TIR.RESULT_QTY
                      ,CASE WHEN TIR.RESULT_QTY - TIP.STOCK_QTY_START >0 THEN '+'||(TIR.RESULT_QTY - TIP.STOCK_QTY_START) 
                       ELSE TO_CHAR(TIR.RESULT_QTY - TIP.STOCK_QTY_START) END DIFFERENCE_QTY
                      ,CASE WHEN TIP.BOX_NO =' ' AND TIR.MAKE_USER_ID IS NOT NULL THEN MUS.USER_NAME 
                       ELSE RESULT_MUS.USER_NAME END USER_NAME
                      ,TIR.COUNT_SEQ
                  FROM T_INVENTORY_PLANS TIP
                    INNER JOIN
                        M_LOCATIONS ML
                    ON
                            ML.SHIPPER_ID = TIP.SHIPPER_ID
                        AND ML.CENTER_ID = TIP.CENTER_ID
                        AND ML.LOCATION_CD = TIP.LOCATION_CD
                    INNER JOIN
                        WW_INV_REFERENCE03 WW
                    ON
                            TIP.SHIPPER_ID = WW.SHIPPER_ID
                        AND TIP.CENTER_ID = WW.CENTER_ID
                        AND TIP.INVENTORY_NO = WW.INVENTORY_NO
                        AND TIP.LOCATION_CD = WW.LOCATION_CD
                        AND TIP.ITEM_SKU_ID = WW.ITEM_SKU_ID
                        AND TIP.JAN = WW.JAN
                        AND TIP.CASE_CLASS = WW.CASE_CLASS
                        AND TIP.GRADE_ID = WW.GRADE_ID
                        AND TIP.BOX_NO = WW.BOX_NO
                        AND WW.SEQ = :SEQ
                   LEFT OUTER JOIN (
                     SELECT SHIPPER_ID
                           ,CENTER_ID
                           ,INVENTORY_NO
                           ,LOCATION_CD
                           ,BOX_NO
                           ,MAX(MAKE_USER_ID) AS MAKE_USER_ID
                     FROM T_INVENTORY_RESULTS
                     WHERE LAST_COUNT_FLAG = 1
                       AND BOX_NO <> ' '
                     GROUP BY SHIPPER_ID
                           ,CENTER_ID
                           ,INVENTORY_NO
                           ,LOCATION_CD
                           ,BOX_NO
                  ) RESULTS
                    ON TIP.SHIPPER_ID = RESULTS.SHIPPER_ID
                   AND TIP.CENTER_ID = RESULTS.CENTER_ID
                   AND TIP.INVENTORY_NO = RESULTS.INVENTORY_NO
                   AND TIP.LOCATION_CD = RESULTS.LOCATION_CD
                   AND TIP.BOX_NO = RESULTS.BOX_NO
                   LEFT JOIN
                       M_USERS RESULT_MUS
                    ON RESULTS.SHIPPER_ID = RESULT_MUS.SHIPPER_ID
                   AND RESULTS.MAKE_USER_ID = RESULT_MUS.USER_ID
                  LEFT JOIN
                       T_INVENTORY_RESULTS TIR
                    ON TIP.SHIPPER_ID = TIR.SHIPPER_ID
                   AND TIP.CENTER_ID = TIR.CENTER_ID
                   AND TIP.INVENTORY_NO = TIR.INVENTORY_NO
                   AND TIP.INVENTORY_SEQ = TIR.INVENTORY_SEQ
                   --AND TIP.STOCK_QTY_START <> TIR.RESULT_QTY
                   AND TIR.LAST_COUNT_FLAG = 1
                  LEFT JOIN
                       M_CENTERS MCE
                    ON TIP.SHIPPER_ID = MCE.SHIPPER_ID
                   AND TIP.CENTER_ID = MCE.CENTER_ID
                  LEFT JOIN
                       M_USERS MUS
                    ON TIR.SHIPPER_ID = MUS.SHIPPER_ID
                   AND TIR.MAKE_USER_ID = MUS.USER_ID
                  LEFT JOIN
                       M_COLORS MCO
                    ON TIP.SHIPPER_ID = MCO.SHIPPER_ID
                   AND TIP.ITEM_COLOR_ID = MCO.ITEM_COLOR_ID
                  LEFT JOIN
                       M_ITEM_SKU MIS
                    ON TIP.SHIPPER_ID = MIS.SHIPPER_ID
                   AND TIP.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                 WHERE TIP.SHIPPER_ID = :SHIPPER_ID
                   AND TIP.STOCK_QTY_START <> NVL(TIR.RESULT_QTY,0)
                 ORDER BY 
                       TIP.INVENTORY_NO
                      ,TIP.LOCATION_CD
                      ,TIP.CASE_CLASS
                      ,TIP.BOX_NO
                      ,TIP.ITEM_ID
                      ,TIP.ITEM_COLOR_ID
                      ,TIP.ITEM_SIZE_ID
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":USER_ID", Common.Profile.User.UserId);
            parameters.Add(":SEQ", condition.Seq3);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<ReferenceReportForCsv>(query.ToString(), parameters);
        }

    }
}