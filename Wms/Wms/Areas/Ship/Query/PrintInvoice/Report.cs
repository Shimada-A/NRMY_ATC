namespace Wms.Areas.Ship.Query.PrintInvoice
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Dapper;
    using Wms.Areas.Ship.ViewModels.PrintInvoice;
    using Wms.Common;
    using Wms.Models;

    public class Report
    {

        /// <summary>
        /// 納品書発行CSVデータ取得
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<PrintNouhinBtoB> GetPrintNouhinBtoBs(PrintInvoiceConditions condition)
        {
            string packingQuery;
            packingQuery = @"
                    SELECT
                            SHIPPER_ID
                        ,   CENTER_ID
                        ,   SHIP_INSTRUCT_ID
                        ,   SHIP_INSTRUCT_SEQ
                        ,   BATCH_NO
                        ,   NOUHIN_NO
                        ,   SHIP_TO_STORE_ID
                        ,   KAKU_DATE
                        ,   KAKU_USER_ID
                        ,   DELI_DATE
                        ,   ITEM_SKU_ID
                        ,   ITEM_ID
                        ,   ITEM_COLOR_ID
                        ,   RESULT_QTY
                        ,   SHIP_TO_STORE_CLASS
                        ,   NOUHIN_PRN_FLAG
                        ,   BOX_NO
                    FROM
                            T_SHIP_PACKING_INFO
                    WHERE
                            EC_FLAG = 0
                        AND BOX_NO = :BOX_NO
                        AND CENTER_ID = :CENTER_ID
                        AND SHIPPER_ID = :SHIPPER_ID
                ";

            if (condition.ChkOldData)
            {
                packingQuery += @"
                    UNION ALL

                    SELECT
                            SHIPPER_ID
                        ,   CENTER_ID
                        ,   SHIP_INSTRUCT_ID
                        ,   SHIP_INSTRUCT_SEQ
                        ,   BATCH_NO
                        ,   NOUHIN_NO
                        ,   SHIP_TO_STORE_ID
                        ,   KAKU_DATE
                        ,   KAKU_USER_ID
                        ,   DELI_DATE
                        ,   ITEM_SKU_ID
                        ,   ITEM_ID
                        ,   ITEM_COLOR_ID
                        ,   RESULT_QTY
                        ,   SHIP_TO_STORE_CLASS
                        ,   NOUHIN_PRN_FLAG
                        ,   BOX_NO
                    FROM
                            A_SHIP_PACKING_INFO
                    WHERE
                            EC_FLAG = 0
                        AND BOX_NO = :BOX_NO
                        AND CENTER_ID = :CENTER_ID
                        AND SHIPPER_ID = :SHIPPER_ID
                ";
            }

            string shipQuery;
            shipQuery = @"
                    SELECT
                            SPD.SHIPPER_ID
                        ,   SPD.CENTER_ID
                        ,   SPD.BATCH_NO
                        ,   SPD.NOUHIN_NO
                        ,   TS.SALES_CLASS
                        ,   TS.OFF_RATE
                        ,   TS.DELIVERY_NOTE
                    FROM
                            SHIP_PACKING_DATA SPD
                    INNER JOIN
                            T_SHIPS TS
                    ON
                            TS.SHIP_INSTRUCT_ID = SPD.SHIP_INSTRUCT_ID
                        AND TS.SHIP_INSTRUCT_SEQ = SPD.SHIP_INSTRUCT_SEQ
                        AND TS.CENTER_ID = SPD.CENTER_ID
                        AND TS.SHIPPER_ID = SPD.SHIPPER_ID
                ";

            if (condition.ChkOldData)
            {
                shipQuery += @"
                    UNION ALL

                    SELECT
                            SPD.SHIPPER_ID
                        ,   SPD.CENTER_ID
                        ,   SPD.BATCH_NO
                        ,   SPD.NOUHIN_NO
                        ,   TS.SALES_CLASS
                        ,   TS.OFF_RATE
                        ,   TS.DELIVERY_NOTE
                    FROM
                            SHIP_PACKING_DATA SPD
                    INNER JOIN
                            A_SHIPS TS
                    ON
                            TS.SHIP_INSTRUCT_ID = SPD.SHIP_INSTRUCT_ID
                        AND TS.SHIP_INSTRUCT_SEQ = SPD.SHIP_INSTRUCT_SEQ
                        AND TS.CENTER_ID = SPD.CENTER_ID
                        AND TS.SHIPPER_ID = SPD.SHIPPER_ID
                ";
            }

            string allocQuery;
            allocQuery = @"
                    SELECT
                            SDB.*
                        ,   TAI.BATCH_NAME
                    FROM
                            SHIP_DATA_BASE SDB
                    INNER JOIN
                            T_ALLOC_INFO TAI
                    ON
                            TAI.ALLOC_NO = SDB.BATCH_NO
                        AND TAI.CENTER_ID = SDB.CENTER_ID
                        AND TAI.SHIPPER_ID = SDB.SHIPPER_ID
                ";

            if (condition.ChkOldData)
            {
                allocQuery += @"
                    UNION ALL

                    SELECT
                            SDB.*
                        ,   TAI.BATCH_NAME
                    FROM
                            SHIP_DATA_BASE SDB
                    INNER JOIN
                            A_ALLOC_INFO TAI
                    ON
                            TAI.ALLOC_NO = SDB.BATCH_NO
                        AND TAI.CENTER_ID = SDB.CENTER_ID
                        AND TAI.SHIPPER_ID = SDB.SHIPPER_ID
                ";
            }

            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine($@"
                WITH
                        GENERALS_DATA AS (
                            SELECT
                                    *
                            FROM
                                    M_GENERALS
                            WHERE
                                    REGISTER_DIVI_CD = '1'
                                AND GEN_DIV_CD IN ('SALE_BASE_CLASS', 'SALE_CLASS', 'TAX_RATE')
                                AND CENTER_ID = '@@@'
                                AND SHIPPER_ID = :SHIPPER_ID
                        )
                    ,   COMPANY_DATA AS (
                            SELECT
                                    MAX(CASE WHEN GEN_CD = 'COMPANY_NAME' THEN GEN_NAME ELSE NULL END) AS COMPANY_NAME
                                ,   MAX(CASE WHEN GEN_CD = 'DIVISION_NAME' THEN GEN_NAME ELSE NULL END) AS COMPANY_DIVISION_NAME
                                ,   MAX(CASE WHEN GEN_CD = 'ADDRESS' THEN GEN_NAME ELSE NULL END) AS COMPANY_ADDRESS
                                ,   MAX(CASE WHEN GEN_CD = 'TEL_FAX' THEN GEN_NAME ELSE NULL END) AS COMPANY_TEL_FAX
                                ,   MAX(CASE WHEN GEN_CD = 'CHECKING_ACCOUNT' THEN GEN_NAME ELSE NULL END) AS COMPANY_CHECKING_ACCOUNT
                            FROM
                                    M_GENERALS
                            WHERE
                                    REGISTER_DIVI_CD = '1'
                                AND GEN_DIV_CD = 'DELIVERY_SLIP_COMPANY'
                                AND CENTER_ID = '@@@'
                                AND SHIPPER_ID = :SHIPPER_ID
                        )
                    ,   SHIP_PACKING_DATA AS ({packingQuery})
                    ,   SHIP_PACKING_SKU_DATA AS (
                            SELECT
                                    SPD.SHIPPER_ID
                                ,   SPD.CENTER_ID
                                ,   SPD.NOUHIN_NO
                                ,   SPD.ITEM_SKU_ID
                                ,   MAX(SPD.SHIP_TO_STORE_ID) AS SHIP_TO_STORE_ID
                                ,   MAX(SPD.KAKU_DATE) AS KAKU_DATE
                                ,   MAX(SPD.KAKU_USER_ID) AS KAKU_USER_ID
                                ,   MAX(SPD.DELI_DATE) AS DELI_DATE
                                ,   MAX(SPD.ITEM_ID) AS ITEM_ID
                                ,   MAX(SPD.ITEM_COLOR_ID) AS ITEM_COLOR_ID
                                ,   SUM(SPD.RESULT_QTY) AS RESULT_QTY
                                ,   MAX(SPD.SHIP_TO_STORE_CLASS) AS SHIP_TO_STORE_CLASS
                                ,   MAX(SPD.NOUHIN_PRN_FLAG) AS NOUHIN_PRN_FLAG
                                ,   MAX(SPD.BOX_NO) AS BOX_NO
                            FROM
                                    SHIP_PACKING_DATA SPD
                            GROUP BY
                                    SPD.SHIPPER_ID
                                ,   SPD.CENTER_ID
                                ,   SPD.NOUHIN_NO
                                ,   SPD.ITEM_SKU_ID
                        )
                    ,   MASTER_SKU_DATA AS (
                            SELECT 
                                    SPSD.SHIPPER_ID
                                ,   SPSD.CENTER_ID
                                ,   SPSD.NOUHIN_NO
                                ,   MIS.ITEM_SKU_ID
                                ,   MAX(SPSD.SHIP_TO_STORE_ID) AS SHIP_TO_STORE_ID
                                ,   MAX(SPSD.KAKU_DATE) AS KAKU_DATE
                                ,   MAX(SPSD.KAKU_USER_ID) AS KAKU_USER_ID
                                ,   MAX(SPSD.DELI_DATE) AS DELI_DATE
                                ,   MAX(MIS.ITEM_ID) AS ITEM_ID
                                ,   MAX(MIS.ITEM_NAME) AS ITEM_NAME
                                ,   MAX(MIS.ITEM_COLOR_ID) AS ITEM_COLOR_ID
                                ,   MAX(MIS.ITEM_SIZE_NAME) AS ITEM_SIZE_NAME
                                ,   MAX(MIS.NORMAL_SELLING_PRICE_EX_TAX) AS NORMAL_SELLING_PRICE
                                ,   ROW_NUMBER() OVER (PARTITION BY 
                                                                MAX(MIS.ITEM_ID)
                                                            ,   MAX(MIS.ITEM_COLOR_ID)
                                                       ORDER BY 
                                                                MAX(MIS.ITEM_SIZE_DISPLAY_ORDER)
                                                            ,   MAX(MIS.ITEM_SIZE_ID)
                                                ) AS ITEM_SIZE_ORDER
                                ,   MAX(MIS.BRAND_ID) AS BRAND_ID
                                ,   MAX(SPSD.BOX_NO)  AS BOX_NO
                            FROM
                                    SHIP_PACKING_SKU_DATA SPSD
                            INNER JOIN
                                    M_ITEM_SKU MIS
                            ON
                                    MIS.ITEM_ID = SPSD.ITEM_ID
                                AND MIS.ITEM_COLOR_ID = SPSD.ITEM_COLOR_ID
                                AND MIS.SHIPPER_ID = SPSD.SHIPPER_ID
                                AND MIS.DELETE_FLAG = 0
                            GROUP BY
                                    SPSD.SHIPPER_ID
                                ,   SPSD.CENTER_ID
                                ,   SPSD.NOUHIN_NO
                                ,   MIS.ITEM_SKU_ID
                        )
                    ,   SHIP_DETAIL_DATA AS (
                            SELECT
                                    MSD.SHIPPER_ID
                                ,   MSD.CENTER_ID
                                ,   MSD.NOUHIN_NO
                                ,   MAX(MSD.SHIP_TO_STORE_ID) AS SHIP_TO_STORE_ID
                                ,   MAX(MSD.KAKU_DATE) AS KAKU_DATE
                                ,   MAX(MSD.KAKU_USER_ID) AS KAKU_USER_ID
                                ,   MAX(MSD.DELI_DATE) AS DELI_DATE
                                ,   MSD.ITEM_ID
                                ,   MAX(MSD.ITEM_NAME) AS ITEM_NAME
                                ,   MSD.ITEM_COLOR_ID
                                ,   LISTAGG(MSD.ITEM_SIZE_NAME || ';') WITHIN GROUP (ORDER BY MSD.ITEM_SIZE_ORDER) AS ITEM_SIZE_NAME
                                ,   LISTAGG(SPSD.RESULT_QTY || ';') WITHIN GROUP (ORDER BY MSD.ITEM_SIZE_ORDER) AS SIZE_RESULT_QTY
                                ,   SUM(SPSD.RESULT_QTY) AS COLOR_RESULT_QTY
                                ,   MAX(MSD.NORMAL_SELLING_PRICE) AS NORMAL_SELLING_PRICE
                                ,   SUM(SPSD.RESULT_QTY) * MAX(MSD.NORMAL_SELLING_PRICE) AS COLOR_AMOUNT
                                ,   SUM(SUM(SPSD.RESULT_QTY)) OVER (PARTITION BY MSD.NOUHIN_NO) AS TOTAL_RESULT_QTY
                                ,   SUM(SUM(SPSD.RESULT_QTY) * MAX(MSD.NORMAL_SELLING_PRICE)) OVER (PARTITION BY MSD.NOUHIN_NO) AS TOTAL_AMOUNT
                                ,   MAX(SPSD.SHIP_TO_STORE_CLASS) AS SHIP_TO_STORE_CLASS
                                ,   MAX(SPSD.NOUHIN_PRN_FLAG) AS NOUHIN_PRN_FLAG
                                ,   MAX(MSD.BRAND_ID) AS BRAND_ID
                                ,   MAX(MSD.BOX_NO) AS BOX_NO
                            FROM
                                    MASTER_SKU_DATA MSD
                            LEFT OUTER JOIN
                                    SHIP_PACKING_SKU_DATA SPSD
                            ON
                                    SPSD.ITEM_SKU_ID = MSD.ITEM_SKU_ID
                                AND SPSD.NOUHIN_NO = MSD.NOUHIN_NO
                                AND SPSD.CENTER_ID = MSD.CENTER_ID
                                AND SPSD.SHIPPER_ID = MSD.SHIPPER_ID
                            GROUP BY
                                    MSD.SHIPPER_ID
                                ,   MSD.CENTER_ID
                                ,   MSD.NOUHIN_NO
                                ,   MSD.ITEM_ID
                                ,   MSD.ITEM_COLOR_ID
                                ,   TRUNC((MSD.ITEM_SIZE_ORDER - 1) / 10)
                        )
                    ,   SHIP_DATA_BASE AS ({shipQuery})
                    ,   SHIP_DATA AS ({allocQuery})
                    ,   SHIP_ADD_DATA AS (
                            SELECT
                                    SHP.SHIPPER_ID
                                ,   SHP.CENTER_ID
                                ,   SHP.NOUHIN_NO
                                ,   LISTAGG(CASE WHEN SHP.RN_SALES_CLASS = 1 THEN GEN.GEN_NAME ELSE NULL END, '$') WITHIN GROUP (ORDER BY SHP.SALES_CLASS) AS SALES_CLASS_NAME
                                ,   MAX(SHP.OFF_RATE) || '%' || MAX(CASE WHEN SHP.COUNT_OFF_RATE > 1 THEN '他' ELSE NULL END) AS OFF_RATE
                                ,   LISTAGG(CASE WHEN SHP.RN_DELIVERY_NOTE = 1 THEN SHP.DELIVERY_NOTE ELSE NULL END, ' ') WITHIN GROUP (ORDER BY SHP.DELIVERY_NOTE) AS DELIVERY_NOTE
                                ,   LISTAGG(CASE WHEN SHP.RN_BATCH_NAME = 1 THEN SHP.BATCH_NAME ELSE NULL END, ' ') WITHIN GROUP (ORDER BY SHP.BATCH_NO) AS BATCH_NAME
                            FROM (
                                SELECT
                                        SHP.*
                                    ,   ROW_NUMBER() OVER (PARTITION BY SHP.SHIPPER_ID, SHP.CENTER_ID, SHP.NOUHIN_NO, SHP.SALES_CLASS ORDER BY SHP.SALES_CLASS) AS RN_SALES_CLASS
                                    ,   COUNT(DISTINCT(SHP.OFF_RATE)) OVER (PARTITION BY SHP.SHIPPER_ID, SHP.CENTER_ID, SHP.NOUHIN_NO) AS COUNT_OFF_RATE
                                    ,   ROW_NUMBER() OVER (PARTITION BY SHP.SHIPPER_ID, SHP.CENTER_ID, SHP.NOUHIN_NO, SHP.DELIVERY_NOTE ORDER BY SHP.DELIVERY_NOTE) AS RN_DELIVERY_NOTE
                                    ,   ROW_NUMBER() OVER (PARTITION BY SHP.SHIPPER_ID, SHP.CENTER_ID, SHP.NOUHIN_NO, SHP.BATCH_NAME ORDER BY SHP.BATCH_NO) AS RN_BATCH_NAME
                                FROM
                                        SHIP_DATA SHP
                            ) SHP
                            LEFT OUTER JOIN
                                    GENERALS_DATA GEN
                            ON
                                    GEN.GEN_DIV_CD = 'SALE_CLASS'
                                AND GEN.GEN_CD = SHP.SALES_CLASS
                            GROUP BY
                                    SHP.SHIPPER_ID
                                ,   SHP.CENTER_ID
                                ,   SHP.NOUHIN_NO
                        )
                    ,   TAX_RATE_DATA AS (
                            SELECT
                                    TO_NUMBER(GD.GEN_NAME) AS TAX_RATE
                            FROM
                                    GENERALS_DATA GD
                            CROSS JOIN (
                                SELECT
                                        MAX(GEN_CD) AS GEN_CD
                                FROM
                                        GENERALS_DATA
                                WHERE
                                        GEN_DIV_CD = 'TAX_RATE'
                                    AND TO_DATE(GEN_CD, 'YYYY/MM/DD') <= TRUNC(SYSDATE)
                            ) GCD
                            WHERE
                                    GD.GEN_DIV_CD = 'TAX_RATE'
                                AND GD.GEN_CD = GCD.GEN_CD
                        )
                SELECT
                        SDD.NOUHIN_NO
                    ,   VSTS.SHIP_TO_ZIP
                    ,   VSTS.SHIP_TO_PREF_NAME || VSTS.SHIP_TO_CITY_NAME || VSTS.SHIP_TO_ADDRESS1 AS SHIP_TO_ADDRESS1
                    ,   VSTS.SHIP_TO_ADDRESS2
                    ,   VSTS.SHIP_TO_ADDRESS3
                    ,   VSTS.INVOICE_STORE_NAME || 
                            CASE 
                                WHEN MC.BRAND_WORK_CLASS = '1' THEN ' ' || MB.BRAND_SHORT_NAME 
                                ELSE NULL 
                            END AS SHIP_TO_STORE_NAME
                    ,   SDD.SHIP_TO_STORE_ID
                    ,   SDD.KAKU_DATE
                    ,   SDD.DELI_DATE
                    ,   CASE
                            WHEN SDD.NOUHIN_PRN_FLAG = 9 AND SDD.SHIP_TO_STORE_CLASS = 8 THEN
                                N'倉庫移動'
                            ELSE
                                GEN_SBC.GEN_NAME
                        END AS SALE_BASE_CLASS_NAME
                    ,   MC.CENTER_NAME1 || '　' || SDD.CENTER_ID AS CENTER_NAME
                    ,   MU.USER_NAME AS KAKU_USER_NAME
                    ,   SAD.SALES_CLASS_NAME
                    ,   SAD.OFF_RATE
                    ,   GEN_COM.COMPANY_NAME
                    ,   GEN_COM.COMPANY_DIVISION_NAME
                    ,   GEN_COM.COMPANY_ADDRESS
                    ,   GEN_COM.COMPANY_TEL_FAX
                    ,   GEN_COM.COMPANY_CHECKING_ACCOUNT
                    ,   SDD.ITEM_ID
                    ,   SDD.ITEM_NAME
                    ,   SDD.ITEM_COLOR_ID
                    ,   MCO.ITEM_COLOR_NAME
                    ,   SDD.ITEM_SIZE_NAME
                    ,   SDD.SIZE_RESULT_QTY
                    ,   SDD.COLOR_RESULT_QTY
                    ,   SDD.NORMAL_SELLING_PRICE
                    ,   SDD.COLOR_AMOUNT
                    ,   SAD.DELIVERY_NOTE
                    ,   SAD.BATCH_NAME
                    ,   SDD.TOTAL_RESULT_QTY
                    ,   SDD.TOTAL_AMOUNT
                    ,   CASE
                            WHEN MS.ROUND_CLASS = '2' THEN CEIL(SDD.TOTAL_AMOUNT * (TRD.TAX_RATE / 100))
                            WHEN MS.ROUND_CLASS = '5' THEN TRUNC(SDD.TOTAL_AMOUNT * (TRD.TAX_RATE / 100))
                            ELSE ROUND(SDD.TOTAL_AMOUNT * (TRD.TAX_RATE / 100))
                        END AS TAX_AMOUNT
                    ,   CASE
                            WHEN SDD.NOUHIN_PRN_FLAG = 9 AND SDD.SHIP_TO_STORE_CLASS = 8 THEN
                                '倉庫移動明細書'
                            ELSE
                                '納品伝票'
                        END AS TITLE
                    ,   SDD.BOX_NO
                FROM
                        SHIP_DETAIL_DATA SDD
                INNER JOIN
                        SHIP_ADD_DATA SAD
                ON
                        SAD.SHIPPER_ID = SDD.SHIPPER_ID
                    AND SAD.CENTER_ID = SDD.CENTER_ID
                    AND SAD.NOUHIN_NO = SDD.NOUHIN_NO
                INNER JOIN
                        V_SHIP_TO_STORES VSTS
                ON
                        VSTS.SHIP_TO_STORE_ID = SDD.SHIP_TO_STORE_ID
                    AND VSTS.SHIPPER_ID = SDD.SHIPPER_ID
                LEFT OUTER JOIN
                        M_STORES MS
                ON
                        MS.STORE_ID = SDD.SHIP_TO_STORE_ID
                    AND MS.SHIPPER_ID = SDD.SHIPPER_ID
                LEFT OUTER JOIN
                        GENERALS_DATA GEN_SBC
                ON
                        GEN_SBC.GEN_DIV_CD = 'SALE_BASE_CLASS'
                    AND GEN_SBC.GEN_CD = MS.SALE_BASE_CLASS
                LEFT OUTER JOIN
                        M_CENTERS MC
                ON
                        MC.CENTER_ID = SDD.CENTER_ID
                    AND MC.SHIPPER_ID = SDD.SHIPPER_ID
                LEFT OUTER JOIN
                        M_USERS MU
                ON
                        MU.USER_ID = SDD.KAKU_USER_ID
                    AND MU.SHIPPER_ID = SDD.SHIPPER_ID
                CROSS JOIN
                        COMPANY_DATA GEN_COM
                LEFT OUTER JOIN
                        M_COLORS MCO
                ON
                        MCO.ITEM_COLOR_ID = SDD.ITEM_COLOR_ID
                    AND MCO.SHIPPER_ID = SDD.SHIPPER_ID
                LEFT OUTER JOIN
                        M_BRANDS MB
                ON
                        MB.BRAND_ID = SDD.BRAND_ID
                    AND MB.SHIPPER_ID = SDD.SHIPPER_ID
                CROSS JOIN
                        TAX_RATE_DATA TRD
                ORDER BY
                        SDD.NOUHIN_NO
                    ,   SDD.ITEM_ID
                    ,   SDD.ITEM_COLOR_ID
            ");
            parameters.Add(":BOX_NO", condition.BoxNo);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            return MvcDbContext.Current.Database.Connection.Query<PrintNouhinBtoB>(query.ToString(), parameters);
        }

        /// <summary>
        /// 浪速送り状CSVデータ取得
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<PrintInvoiceNaniwa> GetPrintInvoiceNaniwas(PrintInvoiceConditions condition)
        {
            string packingQuery;
            packingQuery = $@"
                    SELECT
                            TSPI.BOX_NO
                        ,   TSPI.CENTER_ID
                        ,   TSPI.SHIPPER_ID
                        ,   TSPI.SHIP_TO_STORE_ID
                        ,   TSPI.DELI_NO
                        ,   TSPI.DELI_NO2
                        ,   TSPI.NOUHIN_NO
                        ,   TSPI.KAKU_DATE
                        ,   TSPI.CLIENT_CD
                        ,   TSPI.DELI_PRN_USER_ID
                        ,   MIS.BRAND_ID
                    FROM
                            T_SHIP_PACKING_INFO TSPI
                    LEFT OUTER JOIN
                            M_ITEM_SKU MIS
                    ON
                            MIS.ITEM_SKU_ID = TSPI.ITEM_SKU_ID
                        AND MIS.SHIPPER_ID = TSPI.SHIPPER_ID
                    WHERE
                            TSPI.EC_FLAG = 0
                        AND TSPI.BOX_NO = :BOX_NO
                        AND TSPI.CENTER_ID = :CENTER_ID
                        AND TSPI.SHIPPER_ID = :SHIPPER_ID
                        AND ROWNUM = 1
            ";

            if (condition.ChkOldData)
            {
                packingQuery += $@"
                    UNION

                    SELECT
                            TSPI.BOX_NO
                        ,   TSPI.CENTER_ID
                        ,   TSPI.SHIPPER_ID
                        ,   TSPI.SHIP_TO_STORE_ID
                        ,   TSPI.DELI_NO
                        ,   TSPI.DELI_NO2
                        ,   TSPI.NOUHIN_NO
                        ,   TSPI.KAKU_DATE
                        ,   TSPI.CLIENT_CD
                        ,   TSPI.DELI_PRN_USER_ID
                        ,   MIS.BRAND_ID
                    FROM
                            A_SHIP_PACKING_INFO TSPI
                    LEFT OUTER JOIN
                            M_ITEM_SKU MIS
                    ON
                            MIS.ITEM_SKU_ID = TSPI.ITEM_SKU_ID
                        AND MIS.SHIPPER_ID = TSPI.SHIPPER_ID
                    WHERE
                            TSPI.EC_FLAG = 0
                        AND TSPI.BOX_NO = :BOX_NO
                        AND TSPI.CENTER_ID = :CENTER_ID
                        AND TSPI.SHIPPER_ID = :SHIPPER_ID
                        AND ROWNUM = 1
                ";
            }

            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine($@"
                WITH
                        SELECTED_PACKING_DATA AS ({packingQuery})
                    ,   GENERALS_DATA AS (
                            SELECT
                                    *
                            FROM
                                    M_GENERALS
                            WHERE
                                    REGISTER_DIVI_CD = '1'
                                AND GEN_DIV_CD IN ('STORE_CLASS', 'NANIWA_DELI_CENTER')
                                AND CENTER_ID = '@@@'
                                AND SHIPPER_ID = :SHIPPER_ID
                        )
                SELECT
                        SOTRE.SHIP_TO_PREF_NAME || SHIP_TO_CITY_NAME || SHIP_TO_ADDRESS1 AS STORE_ADDRESS1
                    ,   SOTRE.SHIP_TO_ADDRESS2 || SOTRE.SHIP_TO_ADDRESS3 AS STORE_ADDRESS2
                    ,   PACK.SHIP_TO_STORE_ID AS SHIP_TO_STORE_ID
                    ,   SOTRE.INVOICE_STORE_NAME || 
                            CASE 
                                WHEN MC.BRAND_WORK_CLASS = '1' THEN ' ' || MB.BRAND_SHORT_NAME 
                                ELSE NULL 
                            END AS STORE_NAME
                    ,   SOTRE.SHIP_TO_TEL AS STORE_TEL
                    ,   GEN_NC.GEN_NAME AS NANIWA_DELI_CENTER
                    ,   CONS.CONSIGNOR_ADDRESS1 AS CONSIGNOR_ADDRESS1
                    ,   CONS.CONSIGNOR_ADDRESS2 AS CONSIGNOR_ADDRESS2
                    ,   CONS.CONSIGNOR_NAME1 AS CONSIGNOR_NAME1
                    ,   CONS.CONSIGNOR_NAME2 AS CONSIGNOR_NAME2
                    ,   CONS.CONSIGNOR_TEL AS CONSIGNOR_TEL
                    ,   PACK.DELI_NO2 AS DELI_NO2
                    ,   PACK.DELI_NO AS DELI_NO
                    ,   TO_CHAR(PACK.KAKU_DATE, 'YYYY/MM/DD') AS PRINT_DATE
                    ,   PACK.NOUHIN_NO AS GOODS_NAME1
                    ,   PACK.BOX_NO AS GOODS_NAME2
                    ,   CASE
                            WHEN SOTRE.SHIP_TO_STORE_CLASS IN ('2', '3', '5') THEN GEN_SC.GEN_NAME
                            ELSE NULL
                        END AS TRANSACTION_FORM
                FROM
                        SELECTED_PACKING_DATA PACK
                INNER JOIN
                        M_USERS MU
                ON
                        MU.USER_ID = PACK.DELI_PRN_USER_ID
                    AND MU.SHIPPER_ID = PACK.SHIPPER_ID
                INNER JOIN
                        M_CONSIGNORS_NANIWA CONS
                ON
                        CONS.CONTROL_ID = PACK.CLIENT_CD
                    AND CONS.CENTER_ID = MU.CENTER_ID
                    AND CONS.SHIPPER_ID = PACK.SHIPPER_ID
                INNER JOIN
                        M_CENTERS MC
                ON
                        MC.CENTER_ID = PACK.CENTER_ID
                    AND MC.SHIPPER_ID = PACK.SHIPPER_ID
                LEFT OUTER JOIN
                        V_SHIP_TO_STORES SOTRE
                ON
                        SOTRE.SHIP_TO_STORE_ID = PACK.SHIP_TO_STORE_ID
                    AND SOTRE.SHIPPER_ID = PACK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_BRANDS MB
                ON
                        MB.BRAND_ID = PACK.BRAND_ID
                    AND MB.SHIPPER_ID = PACK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_NANIWA_SORTING SORTING
                ON
                        SORTING.STORE_ID = PACK.SHIP_TO_STORE_ID
                    AND SORTING.SHIPPER_ID = PACK.SHIPPER_ID
                LEFT OUTER JOIN
                        GENERALS_DATA GEN_NC
                ON
                        GEN_NC.GEN_DIV_CD = 'NANIWA_DELI_CENTER'
                    AND GEN_NC.GEN_CD = SORTING.NANIWA_DELI_CENTER_CD
                    AND GEN_NC.SHIPPER_ID = SORTING.SHIPPER_ID
                LEFT OUTER JOIN
                        GENERALS_DATA GEN_SC
                ON
                        GEN_SC.GEN_DIV_CD = 'STORE_CLASS'
                    AND GEN_SC.GEN_CD = SOTRE.SHIP_TO_STORE_CLASS
                    AND GEN_SC.SHIPPER_ID = SOTRE.SHIPPER_ID
            ");
            parameters.Add(":BOX_NO", condition.BoxNo);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            return MvcDbContext.Current.Database.Connection.Query<PrintInvoiceNaniwa>(query.ToString(), parameters);
        }

        /// <summary>
        /// 佐川送り状CSVデータ取得
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<PrintInvoiceSagawa> GetPrintInvoiceSagawas(PrintInvoiceConditions condition)
        {
            string packingQuery;
            packingQuery = $@"
                    SELECT
                            TSPI.BOX_NO
                        ,   TSPI.CENTER_ID
                        ,   TSPI.SHIPPER_ID
                        ,   TSPI.SHIP_TO_STORE_ID
                        ,   TSPI.DELI_NO
                        ,   TRIM(TSPI.DELI_SHIWAKE_CD) AS DELI_SHIWAKE_CD
                        ,   TSPI.KAKU_DATE
                        ,   TSPI.NOUHIN_NO
                        ,   TSPI.CLIENT_CD
                        ,   TSPI.DELI_PRN_USER_ID
                        ,   MIS.BRAND_ID
                    FROM
                            T_SHIP_PACKING_INFO TSPI
                    LEFT OUTER JOIN
                            M_ITEM_SKU MIS
                    ON
                            MIS.ITEM_SKU_ID = TSPI.ITEM_SKU_ID
                        AND MIS.SHIPPER_ID = TSPI.SHIPPER_ID
                    WHERE
                            TSPI.EC_FLAG = 0
                        AND TSPI.BOX_NO = :BOX_NO
                        AND TSPI.CENTER_ID = :CENTER_ID
                        AND TSPI.SHIPPER_ID = :SHIPPER_ID
                        AND ROWNUM = 1
            ";

            if (condition.ChkOldData)
            {
                packingQuery += $@"
                    UNION

                    SELECT
                            TSPI.BOX_NO
                        ,   TSPI.CENTER_ID
                        ,   TSPI.SHIPPER_ID
                        ,   TSPI.SHIP_TO_STORE_ID
                        ,   TSPI.DELI_NO
                        ,   TRIM(TSPI.DELI_SHIWAKE_CD) AS DELI_SHIWAKE_CD
                        ,   TSPI.KAKU_DATE
                        ,   TSPI.NOUHIN_NO
                        ,   TSPI.CLIENT_CD
                        ,   TSPI.DELI_PRN_USER_ID
                        ,   MIS.BRAND_ID
                    FROM
                            A_SHIP_PACKING_INFO TSPI
                    LEFT OUTER JOIN
                            M_ITEM_SKU MIS
                    ON
                            MIS.ITEM_SKU_ID = TSPI.ITEM_SKU_ID
                        AND MIS.SHIPPER_ID = TSPI.SHIPPER_ID
                    WHERE
                            TSPI.EC_FLAG = 0
                        AND TSPI.BOX_NO = :BOX_NO
                        AND TSPI.CENTER_ID = :CENTER_ID
                        AND TSPI.SHIPPER_ID = :SHIPPER_ID
                        AND ROWNUM = 1
                ";
            }
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine($@"
                WITH
                    SELECTED_PACKING_DATA AS ({packingQuery})
                SELECT
                        LPAD(PACK.DELI_SHIWAKE_CD, 7 ,'$') AS DELI_SHIWAKE_CD
                    ,   SUBSTR(PACK.DELI_SHIWAKE_CD, 1, LENGTH(PACK.DELI_SHIWAKE_CD) - 3) AS OFFICE_CD
                    ,   SUBSTR(PACK.DELI_SHIWAKE_CD, -3) AS LOCAL_CD
                    ,   PACK.KAKU_DATE AS PRINT_DATE
                    ,   1 AS UNIT_CNT
                    ,   '陸便' AS BIN_TYPE
                    ,   SOTRE.SHIP_TO_ZIP
                    ,   SOTRE.SHIP_TO_TEL
                    ,   SOTRE.SHIP_TO_PREF_NAME
                    ,   SOTRE.SHIP_TO_CITY_NAME
                    ,   SOTRE.SHIP_TO_ADDRESS1
                    ,   SOTRE.SHIP_TO_ADDRESS2
                    ,   SOTRE.SHIP_TO_ADDRESS3
                    ,   SOTRE.INVOICE_STORE_NAME || 
                            CASE 
                                WHEN MC.BRAND_WORK_CLASS = '1' THEN ' ' || MB.BRAND_SHORT_NAME 
                                ELSE NULL 
                            END AS SHIP_TO_STORE_NAME
                    ,   CONS.CONSIGNOR_ADDRESS1
                    ,   CONS.CONSIGNOR_ADDRESS2
                    ,   CONS.CONSIGNOR_NAME1
                    ,   CONS.CONSIGNOR_NAME2
                    ,   CONS.CONSIGNOR_TEL
                    ,   CONS.CLIENT_CD
                    ,   PACK.DELI_NO
                    ,   CONS.SALES_OFFICE_NAME
                    ,   CONS.SALES_OFFICE_TEL
                    ,   CONS.SALES_OFFICE_FAX
                    ,   1 AS PRINT_COPIES
                    ,   0 AS DAIBIKI_FLAG
                    ,   '箱類' AS PACKING_TYPE
                    ,   '衣料品' AS GOODS_NAME1
                    ,   PACK.NOUHIN_NO AS GOODS_NAME2
                    ,   '830011' AS CLASS_CD
                    ,   PACK.BOX_NO AS GOODS_NAME3
                FROM
                        SELECTED_PACKING_DATA PACK
                INNER JOIN
                        M_USERS MU
                ON
                        MU.USER_ID = PACK.DELI_PRN_USER_ID
                    AND MU.SHIPPER_ID = PACK.SHIPPER_ID
                INNER JOIN
                        M_CONSIGNORS_SAGAWA CONS
                ON
                        CONS.CLIENT_CD = PACK.CLIENT_CD
                    AND CONS.CENTER_ID = MU.CENTER_ID
                    AND CONS.SHIPPER_ID = PACK.SHIPPER_ID
                INNER JOIN
                        M_CENTERS MC
                ON
                        MC.CENTER_ID = PACK.CENTER_ID
                    AND MC.SHIPPER_ID = PACK.SHIPPER_ID
                LEFT OUTER JOIN
                        V_SHIP_TO_STORES SOTRE
                ON
                        SOTRE.SHIP_TO_STORE_ID = PACK.SHIP_TO_STORE_ID
                    AND SOTRE.SHIPPER_ID = PACK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_BRANDS MB
                ON
                        MB.BRAND_ID = PACK.BRAND_ID
                    AND MB.SHIPPER_ID = PACK.SHIPPER_ID
            ");
            parameters.Add(":BOX_NO", condition.BoxNo);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            return MvcDbContext.Current.Database.Connection.Query<PrintInvoiceSagawa>(query.ToString(), parameters);
        }

        /// <summary>
        /// 東京納品代行送り状CSVデータ取得
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<PrintInvoiceTounou> GetPrintInvoiceTounous(PrintInvoiceConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
                WITH
                    SELECTED_PACKING_DATA AS (
                        SELECT
                                BOX_NO
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   SHIP_TO_STORE_ID
                            ,   DELI_NO
                            ,   TO_CHAR(SYSDATE, 'FMMM / DD') AS SHIP_DATE
                            ,   DELI_DATE AS NOUHIN_DATE
                        FROM
                                T_SHIP_PACKING_INFO
                        WHERE
                                EC_FLAG = 0
                            AND BOX_NO = :BOX_NO
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                            AND ROWNUM = 1
                )
                SELECT
                        DIV.DIVISION_NAME || SOTRE.STORE_NAME1 AS STORE_NAME
                    ,   SOTRE.STORE_PREF_NAME || SOTRE.STORE_CITY_NAME || SOTRE.STORE_ADDRESS1 || SOTRE.STORE_ADDRESS2 AS STORE_ADDRESS
                    ,   SOTRE.STORE_TEL AS STORE_TEL
                    ,   PACK.SHIP_TO_STORE_ID AS SHIP_TO_STORE_ID
                    ,   CONS.CONSIGNOR_ADDRESS1 || CONS.CONSIGNOR_ADDRESS2 AS CONSIGNOR_ADDRESS
                    ,   CONS.CONSIGNOR_NAME AS CONSIGNOR_NAME
                    ,   CONS.CONSIGNOR_TEL AS CONSIGNOR_TEL
                    ,   PACK.SHIP_DATE AS SHIP_DATE
                    ,   TO_CHAR(PACK.NOUHIN_DATE, 'FMMM') AS NOUHIN_MONTH
                    ,   TO_CHAR(PACK.NOUHIN_DATE, 'FMDD') AS NOUHIN_DATE
                    ,   PACK.DELI_NO || '001' AS UNIT_BARCODE
                FROM
                        SELECTED_PACKING_DATA PACK
                INNER JOIN
                        M_CONSIGNORS CONS
                ON
                        CONS.CENTER_ID = PACK.CENTER_ID
                    AND CONS.CONSIGNOR_TYPE_ID = '04'
                    AND CONS.SHIPPER_ID = PACK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_STORES SOTRE
                ON
                        SOTRE.STORE_ID = PACK.SHIP_TO_STORE_ID
                    AND SOTRE.SHIPPER_ID = PACK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_DIVISIONS DIV
                ON
                        DIV.DIVISION_ID = SOTRE.DIVISION_ID
                    AND DIV.SHIPPER_ID = SOTRE.SHIPPER_ID
            ");
            parameters.Add(":BOX_NO", condition.BoxNo);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            return MvcDbContext.Current.Database.Connection.Query<PrintInvoiceTounou>(query.ToString(), parameters);
        }

        /// <summary>
        /// ヤマト送り状CSVデータ取得
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<PrintInvoiceYamato> GetPrintInvoiceYamatos(PrintInvoiceConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
                WITH
                    SELECTED_PACKING_DATA AS (
                        SELECT
                                BOX_NO
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   SHIP_TO_STORE_ID
                            ,   DELI_NO
                            ,   TRIM(DELI_SHIWAKE_CD) AS DELI_SHIWAKE_CD
                            ,   BOX_SIZE
                            ,   SYSDATE AS NOW_DATE
                        FROM
                                T_SHIP_PACKING_INFO
                        WHERE
                                EC_FLAG = 0
                            AND BOX_NO = :BOX_NO
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                            AND ROWNUM = 1
                )
            ");
            if (condition.ChkOldData == true)
            {
                query.AppendLine(@"
                    , SELECTED_APACKING_DATA AS(
                            SELECT
                                    BOX_NO
                                ,   CENTER_ID
                                ,   SHIPPER_ID
                                ,   SHIP_TO_STORE_ID
                                ,   DELI_NO
                                ,   TRIM(DELI_SHIWAKE_CD) AS DELI_SHIWAKE_CD
                                ,   BOX_SIZE
                                ,   SYSDATE AS NOW_DATE
                            FROM
                                    A_SHIP_PACKING_INFO
                            WHERE
                                    EC_FLAG = 0
                                AND BOX_NO = :BOX_NO
                                AND CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND ROWNUM = 1
                    )
                    , SELECTED_ALL_PACKING_DATA AS(
                            SELECT * FROM SELECTED_PACKING_DATA
                            UNION
                            SELECT * FROM SELECTED_APACKING_DATA
                    )
                ");
            }
            query.AppendLine(@"
                SELECT
                        '0' || PACK.DELI_SHIWAKE_CD || '0' AS DELI_SHIWAKE_BARCODE
                    ,   PACK.DELI_SHIWAKE_CD AS DELI_SHIWAKE_CD
                    ,   SOTRE.SHIP_TO_ZIP AS STORE_ZIP
                    ,   SOTRE.SHIP_TO_TEL AS STORE_TEL
                    ,   PACK.DELI_NO AS DELI_NO
                    ,   TO_CHAR(NOW_DATE, 'YYMMDD') AS PRINT_DATE
                    ,   '' AS ARRIVE_REQUEST_DATE
                    ,   '指定なし' AS ARRIVE_REQUEST_TIME
                    ,   SOTRE.SHIP_TO_PREF_NAME || SOTRE.SHIP_TO_CITY_NAME AS DEST_ADDRESS1
                    ,   SOTRE.SHIP_TO_ADDRESS1 AS DEST_ADDRESS2
                    ,   SOTRE.SHIP_TO_ADDRESS2 AS DEST_ADDRESS3
                    ,   SOTRE.SHIP_TO_STORE_NAME1 AS DEST_NAME
                    ,   '' AS DEST_BUMON1
                    ,   TO_CHAR(SOTRE.SHIP_TO_STORE_ID) AS DEST_BUMON2
                    ,   CONS.CONSIGNOR_ZIP AS CONSIGNOR_ZIP
                    ,   CONS.CONSIGNOR_TEL AS CONSIGNOR_TEL
                    ,   CONS.CONSIGNOR_ADDRESS1 AS CONSIGNOR_ADDRESS1
                    ,   CONS.CONSIGNOR_ADDRESS2 AS CONSIGNOR_ADDRESS2
                    ,   '' AS CONSIGNOR_ADDRESS3
                    ,   CONS.CONSIGNOR_NAME AS CONSIGNOR_NAME
                    ,   '衣類品等' AS ITEM_NAME1
                    ,   '' AS ITEM_NAME2
                    ,   '1' AS UNIT
                    ,   '1' AS ALL_UNIT
                    ,   '水濡厳禁' AS HANDLING1
                    ,   '' AS HANDLING2
                    ,   '' AS ARTICLE
                    ,   GEN.GEN_NAME AS BOX_SIZE
                    ,   CONS.SHIP_STORE_CD AS SHIP_STORE_CD
                    ,   TO_CHAR(NOW_DATE + 30, 'YYYY""年""MM""月""DD""日迄""') AS DELI_USE_DATE
                    ,   CONS.DELIVERY_COMPANY_TEL AS DELIVERY_COMPANY_TEL
                    ,   PACK.DELI_NO AS DELI_NO_BARCODE
                FROM
            ");
            if (condition.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                        SELECTED_ALL_PACKING_DATA PACK
                ");
            }
            else
            {
                query.AppendLine(@"
                        SELECTED_PACKING_DATA PACK
                ");
            }
            query.AppendLine(@"
                INNER JOIN
                        M_CONSIGNORS CONS
                ON
                        CONS.CENTER_ID = PACK.CENTER_ID
                    AND CONS.CONSIGNOR_TYPE_ID = '01'
                    AND CONS.SHIPPER_ID = PACK.SHIPPER_ID
                LEFT OUTER JOIN
                        V_SHIP_TO_STORES SOTRE
                ON
                        SOTRE.SHIP_TO_STORE_ID = PACK.SHIP_TO_STORE_ID
                    AND SOTRE.SHIPPER_ID = PACK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_GENERALS GEN
                ON
                        GEN.REGISTER_DIVI_CD = '1'
                    AND GEN.GEN_DIV_CD = 'YAMATO_BOX_SIZE'
                    AND GEN.GEN_CD = PACK.BOX_SIZE
                    AND GEN.CENTER_ID = '@@@'
                    AND GEN.SHIPPER_ID = PACK.SHIPPER_ID
            ");
            parameters.Add(":BOX_NO", condition.BoxNo);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            return MvcDbContext.Current.Database.Connection.Query<PrintInvoiceYamato>(query.ToString(), parameters);
        }

        /// <summary>
        /// ワールドサプライ送り状CSVデータ取得
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<PrintInvoiceWorldSupply> GetPrintInvoiceWorldSupply(PrintInvoiceConditions condition)
        {
            string packingQuery;
            packingQuery = $@"
                    SELECT
                            TSPI.BOX_NO
                        ,   TSPI.CENTER_ID
                        ,   TSPI.SHIPPER_ID
                        ,   MAX(TSPI.SHIP_TO_STORE_ID) AS SHIP_TO_STORE_ID
                        ,   MAX(TSPI.DELI_NO) AS DELI_NO
                        ,   MAX(TSPI.NOUHIN_NO) AS NOUHIN_NO
                        ,   MAX(TSPI.KAKU_DATE) AS KAKU_DATE
                        ,   MAX(TSPI.CLIENT_CD) AS CLIENT_CD
                        ,   MAX(TSPI.DELI_PRN_USER_ID) AS DELI_PRN_USER_ID
                        ,   MAX(MIS.BRAND_ID) AS BRAND_ID
                        ,   COUNT(DISTINCT(TSPI.NOUHIN_NO)) AS NOUHIN_COUNT
                    FROM
                            T_SHIP_PACKING_INFO TSPI
                    LEFT OUTER JOIN
                            M_ITEM_SKU MIS
                    ON
                            MIS.ITEM_SKU_ID = TSPI.ITEM_SKU_ID
                        AND MIS.SHIPPER_ID = TSPI.SHIPPER_ID
                    WHERE
                            TSPI.EC_FLAG = 0
                        AND TSPI.BOX_NO = :BOX_NO
                        AND TSPI.CENTER_ID = :CENTER_ID
                        AND TSPI.SHIPPER_ID = :SHIPPER_ID
                    GROUP BY
                            TSPI.BOX_NO
                        ,   TSPI.CENTER_ID
                        ,   TSPI.SHIPPER_ID
            ";

            if (condition.ChkOldData)
            {
                packingQuery += $@"
                    UNION

                    SELECT
                            TSPI.BOX_NO
                        ,   TSPI.CENTER_ID
                        ,   TSPI.SHIPPER_ID
                        ,   MAX(TSPI.SHIP_TO_STORE_ID) AS SHIP_TO_STORE_ID
                        ,   MAX(TSPI.DELI_NO) AS DELI_NO
                        ,   MAX(TSPI.NOUHIN_NO) AS NOUHIN_NO
                        ,   MAX(TSPI.KAKU_DATE) AS KAKU_DATE
                        ,   MAX(TSPI.CLIENT_CD) AS CLIENT_CD
                        ,   MAX(TSPI.DELI_PRN_USER_ID) AS DELI_PRN_USER_ID
                        ,   MAX(MIS.BRAND_ID) AS BRAND_ID
                        ,   COUNT(DISTINCT(TSPI.NOUHIN_NO)) AS NOUHIN_COUNT
                    FROM
                            A_SHIP_PACKING_INFO TSPI
                    LEFT OUTER JOIN
                            M_ITEM_SKU MIS
                    ON
                            MIS.ITEM_SKU_ID = TSPI.ITEM_SKU_ID
                        AND MIS.SHIPPER_ID = TSPI.SHIPPER_ID
                    WHERE
                            TSPI.EC_FLAG = 0
                        AND TSPI.BOX_NO = :BOX_NO
                        AND TSPI.CENTER_ID = :CENTER_ID
                        AND TSPI.SHIPPER_ID = :SHIPPER_ID
                    GROUP BY
                            TSPI.BOX_NO
                        ,   TSPI.CENTER_ID
                        ,   TSPI.SHIPPER_ID
                ";
            }

            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine($@"
                WITH
                        SELECTED_PACKING_DATA AS ({packingQuery})
                SELECT
                        CASE WHEN STORE.SALE_BASE_CLASS = '2' THEN '買取' ELSE '消化' END AS DELIVERY_CLASS
                    ,   TO_CHAR(PACK.KAKU_DATE, 'YYYYMMDD') AS PRINT_DATE
                    ,   STORE.INVOICE_STORE_NAME || 
                            CASE 
                                WHEN MC.BRAND_WORK_CLASS = '1' THEN ' ' || MB.BRAND_SHORT_NAME 
                                ELSE NULL 
                            END AS STORE_NAME
                    ,   STORE.SHIP_TO_ZIP AS STORE_ZIP
                    ,   STORE.SHIP_TO_TEL AS STORE_TEL
                    ,   STORE.SHIP_TO_PREF_NAME || STORE.SHIP_TO_CITY_NAME || STORE.SHIP_TO_ADDRESS1 || STORE.SHIP_TO_ADDRESS2 || STORE.SHIP_TO_ADDRESS3 AS STORE_ADDRESS
                    ,   PACK.NOUHIN_COUNT AS SLIP_COUNT
                    ,   CONS.CONSIGNOR_NAME1 || CONS.CONSIGNOR_NAME2 AS CONSIGNOR_NAME
                    ,   CONS.CONSIGNOR_ZIP AS CONSIGNOR_ZIP
                    ,   CONS.CONSIGNOR_TEL AS CONSIGNOR_TEL
                    ,   CONS.CONSIGNOR_ADDRESS1 || CONS.CONSIGNOR_ADDRESS2 AS CONSIGNOR_ADDRESS
                    ,   PACK.DELI_NO AS DELI_NO
                    ,   PACK.NOUHIN_NO AS ARTICLE1
                    ,   PACK.BOX_NO AS ARTICLE2
                    ,   0 AS TAG_CLASS
                FROM
                        SELECTED_PACKING_DATA PACK
                INNER JOIN
                        M_USERS MU
                ON
                        MU.USER_ID = PACK.DELI_PRN_USER_ID
                    AND MU.SHIPPER_ID = PACK.SHIPPER_ID
                INNER JOIN
                        M_CONSIGNORS_WORLD CONS
                ON
                        CONS.CONSIGNOR_ID = PACK.CLIENT_CD
                    AND CONS.CENTER_ID = MU.CENTER_ID
                    AND CONS.SHIPPER_ID = PACK.SHIPPER_ID
                INNER JOIN
                        M_CENTERS MC
                ON
                        MC.CENTER_ID = PACK.CENTER_ID
                    AND MC.SHIPPER_ID = PACK.SHIPPER_ID
                LEFT OUTER JOIN
                        V_SHIP_TO_STORES STORE
                ON
                        STORE.SHIP_TO_STORE_ID = PACK.SHIP_TO_STORE_ID
                    AND STORE.SHIPPER_ID = PACK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_BRANDS MB
                ON
                        MB.BRAND_ID = PACK.BRAND_ID
                    AND MB.SHIPPER_ID = PACK.SHIPPER_ID
            ");

            parameters.Add(":BOX_NO", condition.BoxNo);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            return MvcDbContext.Current.Database.Connection.Query<PrintInvoiceWorldSupply>(query.ToString(), parameters);
        }
    }
}