namespace Wms.Areas.Ship.Query.PrintEcInvoice
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Dapper;
    using Wms.Areas.Ship.ViewModels.PrintEcInvoice;
    using Wms.Common;
    using Wms.Models;

    public class Report
    {

        /// <summary>
        /// 納品書発行CSVデータ取得
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<PrintNouhinEc> GetPrintNouhinEcs(PrintEcInvoiceConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
                WITH
                    SELECTED_ECSHIPS_DATA AS (
                        SELECT
                                CENTER_ID 
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   DEST_FAMILY_NAME 
                            ,   DEST_FIRST_NAME
                            ,   ORDER_DATE
                            ,   PAYMENT_METHOD
                            ,   SHIP_REQUEST_DATE
                            ,   DEST_ZIP
                            ,   DEST_PREF_NAME
                            ,   DEST_CITY_NAME 
                            ,   DEST_ADDRESS1 
                            ,   DEST_ADDRESS2
                            ,   DEST_ADDRESS3
                            ,   DEST_TEL
                            ,   ARRIVE_REQUEST_DATE
                            ,   ITEM_ID
                            ,   ITEM_NAME
                            ,   ITEM_COLOR_ID
                            ,   ITEM_SIZE_ID
                            ,   JAN
                            ,   SALE_UNIT_PRICE
                            ,   ORDER_QTY
                            ,   ITEM_AMOUNT
                            ,   TOTAL_ITEM_AMOUNT
                            ,   CARRIAGE_AMOUNT
                            ,   COMMISSION_AMOUNT
                            ,   POINT_DISCOUNT_AMOUNT
                            ,   TOTAL_CLAIM_AMOUNT
                            ,   CAMPAIGN_DISCOUNT_AMOUNT
                        FROM
                                T_ECSHIPS
                        WHERE
                                SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                )
            ");

            if (condition.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                    , SELECTED_AECSHIPS_DATA AS(
                        SELECT
                                CENTER_ID 
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   DEST_FAMILY_NAME 
                            ,   DEST_FIRST_NAME
                            ,   ORDER_DATE
                            ,   PAYMENT_METHOD
                            ,   SHIP_REQUEST_DATE
                            ,   DEST_ZIP
                            ,   DEST_PREF_NAME
                            ,   DEST_CITY_NAME 
                            ,   DEST_ADDRESS1 
                            ,   DEST_ADDRESS2
                            ,   DEST_ADDRESS3
                            ,   DEST_TEL
                            ,   ARRIVE_REQUEST_DATE
                            ,   ITEM_ID
                            ,   ITEM_NAME
                            ,   ITEM_COLOR_ID
                            ,   ITEM_SIZE_ID
                            ,   JAN
                            ,   SALE_UNIT_PRICE
                            ,   ORDER_QTY
                            ,   ITEM_AMOUNT
                            ,   TOTAL_ITEM_AMOUNT
                            ,   CARRIAGE_AMOUNT
                            ,   COMMISSION_AMOUNT
                            ,   POINT_DISCOUNT_AMOUNT
                            ,   TOTAL_CLAIM_AMOUNT
                            ,   CAMPAIGN_DISCOUNT_AMOUNT
                        FROM
                                A_ECSHIPS
                        WHERE
                                SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                    )
                    , SELECTED_ALL_ECSHIPS_DATA AS(
                            SELECT * FROM SELECTED_ECSHIPS_DATA
                            UNION
                            SELECT * FROM SELECTED_AECSHIPS_DATA
                    )
                ");
            }
            query.AppendLine(@"
                ,   GROUP_SUM_ORDER_QTY AS (
                        SELECT
                                SHIP_INSTRUCT_ID
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   SUM(ORDER_QTY) AS SUM_ORDER_QTY
                        FROM

            ");
            if (condition.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                        SELECTED_ALL_ECSHIPS_DATA 
                ");
            }
            else
            {
                query.AppendLine(@"
                        SELECTED_ECSHIPS_DATA 
                ");
            }
            query.AppendLine(@"
                        GROUP BY
                                SHIP_INSTRUCT_ID
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                )
                ,   TARGET_GENERAL_DATA AS (
                        SELECT
                                GEN_DIV_CD
                            ,   GEN_CD
                            ,   GEN_NAME
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   REGISTER_DIVI_CD
                        FROM
                                M_GENERALS
                        WHERE
                                REGISTER_DIVI_CD = '1'
                            AND GEN_DIV_CD IN ('EC_OKAIAGE_STORE_NAME','EC_OKAIAGE_SHIPPER','EC_OKAIAGE_MESSAGE1','EC_OKAIAGE_MESSAGE2','EC_OKAIAGE_MESSAGE3')
                            AND SHIPPER_ID = :SHIPPER_ID
                )
                SELECT
                        ECSHIP.DEST_FAMILY_NAME || '　' || ECSHIP.DEST_FIRST_NAME || '　様' AS DEST_NAME
                    ,   ECSHIP.SHIP_INSTRUCT_ID AS ORDER_NO
                    ,   TO_CHAR(ECSHIP.ORDER_DATE, 'YYYY""年""MM""月""DD""日""') AS ORDER_DATE
                    ,   ECSHIP.PAYMENT_METHOD
                    ,   GEN_STORE1.GEN_NAME AS EC_OKAIAGE_STORE_NAME
                    ,   TO_CHAR(ECSHIP.SHIP_REQUEST_DATE, 'YYYY""年""MM""月""DD""日""') AS SHIP_DATE
                    ,   GRP.SUM_ORDER_QTY AS SUM_ORDER_QTY
                    ,   ECSHIP.DEST_ZIP
                    ,   ECSHIP.DEST_PREF_NAME || ECSHIP.DEST_CITY_NAME || ECSHIP.DEST_ADDRESS1 || ECSHIP.DEST_ADDRESS2 || ECSHIP.DEST_ADDRESS3 AS DEST_ADDRESS
                    ,   ECSHIP.DEST_TEL
                    ,   TO_CHAR(ECSHIP.ARRIVE_REQUEST_DATE, 'YYYY""年""MM""月""DD""日""') AS ARRIVE_REQUEST_DATE
                    ,   GEN_STORE1.GEN_NAME AS EC_OKAIAGE_STORE_NAME1
                    ,   GEN_STORE2.GEN_NAME AS EC_OKAIAGE_STORE_NAME2
                    ,   GEN_SHIP1.GEN_NAME AS EC_OKAIAGE_SHIPPER1
                    ,   GEN_SHIP2.GEN_NAME AS EC_OKAIAGE_SHIPPER2
                    ,   GEN_SHIP3.GEN_NAME AS EC_OKAIAGE_SHIPPER3
                    ,   ECSHIP.ITEM_ID
                    ,   COLOR.ITEM_COLOR_NAME AS COLOR_NAME
                    ,   SIZES.ITEM_SIZE_NAME AS SIZE_NAME
                    ,   ECSHIP.ITEM_NAME
                    ,   ECSHIP.JAN
                    ,   ECSHIP.SALE_UNIT_PRICE AS TANKA
                    ,   ECSHIP.ORDER_QTY
                    ,   ECSHIP.ITEM_AMOUNT
                    ,   ECSHIP.TOTAL_ITEM_AMOUNT
                    ,   ECSHIP.CARRIAGE_AMOUNT
                    ,   ECSHIP.COMMISSION_AMOUNT
                    ,   0 - NVL(ECSHIP.POINT_DISCOUNT_AMOUNT,0)  AS POINT_DISCOUNT_AMOUNT
                    ,   ECSHIP.TOTAL_CLAIM_AMOUNT
                    ,   ECSHIP.SHIP_INSTRUCT_ID AS ORDER_BARCODE
                    ,   GEN_MES11.GEN_NAME AS EC_OKAIAGE_MESSAGE11
                    ,   GEN_MES12.GEN_NAME AS EC_OKAIAGE_MESSAGE12
                    ,   GEN_MES13.GEN_NAME AS EC_OKAIAGE_MESSAGE13
                    ,   GEN_MES21.GEN_NAME AS EC_OKAIAGE_MESSAGE21
                    ,   GEN_MES22.GEN_NAME AS EC_OKAIAGE_MESSAGE22
                    ,   GEN_MES23.GEN_NAME AS EC_OKAIAGE_MESSAGE23
                    ,   GEN_MES24.GEN_NAME AS EC_OKAIAGE_MESSAGE24
                    ,   GEN_MES25.GEN_NAME AS EC_OKAIAGE_MESSAGE25
                    ,   GEN_MES26.GEN_NAME AS EC_OKAIAGE_MESSAGE26
                    ,   GEN_MES27.GEN_NAME AS EC_OKAIAGE_MESSAGE27
                    ,   GEN_MES28.GEN_NAME AS EC_OKAIAGE_MESSAGE28
                    ,   GEN_MES29.GEN_NAME AS EC_OKAIAGE_MESSAGE29
                    ,   GEN_MES210.GEN_NAME AS EC_OKAIAGE_MESSAGE210
                    ,   GEN_MES211.GEN_NAME AS EC_OKAIAGE_MESSAGE211
                    ,   GEN_MES212.GEN_NAME AS EC_OKAIAGE_MESSAGE212
                    ,   GEN_MES213.GEN_NAME AS EC_OKAIAGE_MESSAGE213
                    ,   GEN_MES214.GEN_NAME AS EC_OKAIAGE_MESSAGE214
                    ,   GEN_MES215.GEN_NAME AS EC_OKAIAGE_MESSAGE215
                    ,   GEN_MES216.GEN_NAME AS EC_OKAIAGE_MESSAGE216
                    ,   GEN_MES217.GEN_NAME AS EC_OKAIAGE_MESSAGE217
                    ,   GEN_MES218.GEN_NAME AS EC_OKAIAGE_MESSAGE218
                    ,   GEN_MES219.GEN_NAME AS EC_OKAIAGE_MESSAGE219
                    ,   GEN_MES220.GEN_NAME AS EC_OKAIAGE_MESSAGE220
                    ,   GEN_MES221.GEN_NAME AS EC_OKAIAGE_MESSAGE221
                    ,   GEN_MES222.GEN_NAME AS EC_OKAIAGE_MESSAGE222
                    ,   GEN_MES223.GEN_NAME AS EC_OKAIAGE_MESSAGE223
                    ,   GEN_MES224.GEN_NAME AS EC_OKAIAGE_MESSAGE224
                    ,   GEN_MES225.GEN_NAME AS EC_OKAIAGE_MESSAGE225
                    ,   GEN_MES226.GEN_NAME AS EC_OKAIAGE_MESSAGE226
                    ,   GEN_MES227.GEN_NAME AS EC_OKAIAGE_MESSAGE227
                    ,   GEN_MES31.GEN_NAME AS EC_OKAIAGE_MESSAGE31
                    ,   GEN_MES32.GEN_NAME AS EC_OKAIAGE_MESSAGE32
                    ,   0 - NVL(ECSHIP.CAMPAIGN_DISCOUNT_AMOUNT,0) AS CAMPAIGN_DISCOUNT_AMOUNT
                  FROM
            ");
            if (condition.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                        SELECTED_ALL_ECSHIPS_DATA ECSHIP
                ");
            }
            else
            {
                query.AppendLine(@"
                        SELECTED_ECSHIPS_DATA ECSHIP 
                ");
            }
            query.AppendLine(@"

                LEFT OUTER JOIN
                        GROUP_SUM_ORDER_QTY GRP
                ON
                        GRP.SHIP_INSTRUCT_ID = ECSHIP.SHIP_INSTRUCT_ID
                    AND GRP.CENTER_ID = ECSHIP.CENTER_ID
                    AND GRP.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        M_COLORS COLOR
                ON
                        COLOR.ITEM_COLOR_ID = ECSHIP.ITEM_COLOR_ID
                    AND COLOR.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        M_SIZES SIZES
                ON
                        SIZES.ITEM_SIZE_ID = ECSHIP.ITEM_SIZE_ID
                    AND SIZES.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_STORE1
                ON
                        GEN_STORE1.REGISTER_DIVI_CD = '1'
                    AND GEN_STORE1.GEN_DIV_CD = 'EC_OKAIAGE_STORE_NAME'
                    AND GEN_STORE1.GEN_CD = '1'
                    AND GEN_STORE1.CENTER_ID = '@@@'
                    AND GEN_STORE1.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_STORE2
                ON
                        GEN_STORE2.REGISTER_DIVI_CD = '1'
                    AND GEN_STORE2.GEN_DIV_CD = 'EC_OKAIAGE_STORE_NAME'
                    AND GEN_STORE2.GEN_CD = '2'
                    AND GEN_STORE2.CENTER_ID = '@@@'
                    AND GEN_STORE2.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_SHIP1
                ON
                        GEN_SHIP1.REGISTER_DIVI_CD = '1'
                    AND GEN_SHIP1.GEN_DIV_CD = 'EC_OKAIAGE_SHIPPER'
                    AND GEN_SHIP1.GEN_CD = '1'
                    AND GEN_SHIP1.CENTER_ID = ECSHIP.CENTER_ID
                    AND GEN_SHIP1.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_SHIP2
                ON
                        GEN_SHIP2.REGISTER_DIVI_CD = '1'
                    AND GEN_SHIP2.GEN_DIV_CD = 'EC_OKAIAGE_SHIPPER'
                    AND GEN_SHIP2.GEN_CD = '2'
                    AND GEN_SHIP2.CENTER_ID = ECSHIP.CENTER_ID
                    AND GEN_SHIP2.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_SHIP3
                ON
                        GEN_SHIP3.REGISTER_DIVI_CD = '1'
                    AND GEN_SHIP3.GEN_DIV_CD = 'EC_OKAIAGE_SHIPPER'
                    AND GEN_SHIP3.GEN_CD = '3'
                    AND GEN_SHIP3.CENTER_ID = ECSHIP.CENTER_ID
                    AND GEN_SHIP3.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES11
                ON
                        GEN_MES11.REGISTER_DIVI_CD = '1'
                    AND GEN_MES11.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE1'
                    AND GEN_MES11.GEN_CD = '1'
                    AND GEN_MES11.CENTER_ID = '@@@'
                    AND GEN_MES11.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES12
                ON
                        GEN_MES12.REGISTER_DIVI_CD = '1'
                    AND GEN_MES12.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE1'
                    AND GEN_MES12.GEN_CD = '2'
                    AND GEN_MES12.CENTER_ID = '@@@'
                    AND GEN_MES12.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES13
                ON
                        GEN_MES13.REGISTER_DIVI_CD = '1'
                    AND GEN_MES13.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE1'
                    AND GEN_MES13.GEN_CD = '3'
                    AND GEN_MES13.CENTER_ID = '@@@'
                    AND GEN_MES13.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES21
                ON
                        GEN_MES21.REGISTER_DIVI_CD = '1'
                    AND GEN_MES21.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES21.GEN_CD = '1'
                    AND GEN_MES21.CENTER_ID = '@@@'
                    AND GEN_MES21.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES22
                ON
                        GEN_MES22.REGISTER_DIVI_CD = '1'
                    AND GEN_MES22.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES22.GEN_CD = '2'
                    AND GEN_MES22.CENTER_ID = '@@@'
                    AND GEN_MES22.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES23
                ON
                        GEN_MES23.REGISTER_DIVI_CD = '1'
                    AND GEN_MES23.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES23.GEN_CD = '3'
                    AND GEN_MES23.CENTER_ID = '@@@'
                    AND GEN_MES23.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES24
                ON
                        GEN_MES24.REGISTER_DIVI_CD = '1'
                    AND GEN_MES24.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES24.GEN_CD = '4'
                    AND GEN_MES24.CENTER_ID = '@@@'
                    AND GEN_MES24.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES25
                ON
                        GEN_MES25.REGISTER_DIVI_CD = '1'
                    AND GEN_MES25.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES25.GEN_CD = '5'
                    AND GEN_MES25.CENTER_ID = '@@@'
                    AND GEN_MES25.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES26
                ON
                        GEN_MES26.REGISTER_DIVI_CD = '1'
                    AND GEN_MES26.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES26.GEN_CD = '6'
                    AND GEN_MES26.CENTER_ID = '@@@'
                    AND GEN_MES26.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES27
                ON
                        GEN_MES27.REGISTER_DIVI_CD = '1'
                    AND GEN_MES27.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES27.GEN_CD = '7'
                    AND GEN_MES27.CENTER_ID = '@@@'
                    AND GEN_MES27.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES28
                ON
                        GEN_MES28.REGISTER_DIVI_CD = '1'
                    AND GEN_MES28.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES28.GEN_CD = '8'
                    AND GEN_MES28.CENTER_ID = '@@@'
                    AND GEN_MES28.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES29
                ON
                        GEN_MES29.REGISTER_DIVI_CD = '1'
                    AND GEN_MES29.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES29.GEN_CD = '9'
                    AND GEN_MES29.CENTER_ID = '@@@'
                    AND GEN_MES28.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES210
                ON
                        GEN_MES210.REGISTER_DIVI_CD = '1'
                    AND GEN_MES210.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES210.GEN_CD = '10'
                    AND GEN_MES210.CENTER_ID = '@@@'
                    AND GEN_MES210.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES211
                ON
                        GEN_MES211.REGISTER_DIVI_CD = '1'
                    AND GEN_MES211.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES211.GEN_CD = '11'
                    AND GEN_MES211.CENTER_ID = '@@@'
                    AND GEN_MES211.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES212
                ON
                        GEN_MES212.REGISTER_DIVI_CD = '1'
                    AND GEN_MES212.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES212.GEN_CD = '12'
                    AND GEN_MES212.CENTER_ID = '@@@'
                    AND GEN_MES212.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES213
                ON
                        GEN_MES213.REGISTER_DIVI_CD = '1'
                    AND GEN_MES213.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES213.GEN_CD = '13'
                    AND GEN_MES213.CENTER_ID = '@@@'
                    AND GEN_MES213.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES214
                ON
                        GEN_MES214.REGISTER_DIVI_CD = '1'
                    AND GEN_MES214.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES214.GEN_CD = '14'
                    AND GEN_MES214.CENTER_ID = '@@@'
                    AND GEN_MES214.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES215
                ON
                        GEN_MES215.REGISTER_DIVI_CD = '1'
                    AND GEN_MES215.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES215.GEN_CD = '15'
                    AND GEN_MES215.CENTER_ID = '@@@'
                    AND GEN_MES215.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES216
                ON
                        GEN_MES216.REGISTER_DIVI_CD = '1'
                    AND GEN_MES216.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES216.GEN_CD = '16'
                    AND GEN_MES216.CENTER_ID = '@@@'
                    AND GEN_MES216.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES217
                ON
                        GEN_MES217.REGISTER_DIVI_CD = '1'
                    AND GEN_MES217.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES217.GEN_CD = '17'
                    AND GEN_MES217.CENTER_ID = '@@@'
                    AND GEN_MES217.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES218
                ON
                        GEN_MES218.REGISTER_DIVI_CD = '1'
                    AND GEN_MES218.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES218.GEN_CD = '18'
                    AND GEN_MES218.CENTER_ID = '@@@'
                    AND GEN_MES218.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES219
                ON
                        GEN_MES219.REGISTER_DIVI_CD = '1'
                    AND GEN_MES219.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES219.GEN_CD = '19'
                    AND GEN_MES219.CENTER_ID = '@@@'
                    AND GEN_MES219.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES220
                ON
                        GEN_MES220.REGISTER_DIVI_CD = '1'
                    AND GEN_MES220.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES220.GEN_CD = '20'
                    AND GEN_MES220.CENTER_ID = '@@@'
                    AND GEN_MES220.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES221
                ON
                        GEN_MES221.REGISTER_DIVI_CD = '1'
                    AND GEN_MES221.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES221.GEN_CD = '21'
                    AND GEN_MES221.CENTER_ID = '@@@'
                    AND GEN_MES221.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES222
                ON
                        GEN_MES222.REGISTER_DIVI_CD = '1'
                    AND GEN_MES222.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES222.GEN_CD = '22'
                    AND GEN_MES222.CENTER_ID = '@@@'
                    AND GEN_MES222.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES223
                ON
                        GEN_MES223.REGISTER_DIVI_CD = '1'
                    AND GEN_MES223.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES223.GEN_CD = '23'
                    AND GEN_MES223.CENTER_ID = '@@@'
                    AND GEN_MES223.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES224
                ON
                        GEN_MES224.REGISTER_DIVI_CD = '1'
                    AND GEN_MES224.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES224.GEN_CD = '24'
                    AND GEN_MES224.CENTER_ID = '@@@'
                    AND GEN_MES224.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES225
                ON
                        GEN_MES225.REGISTER_DIVI_CD = '1'
                    AND GEN_MES225.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES225.GEN_CD = '25'
                    AND GEN_MES225.CENTER_ID = '@@@'
                    AND GEN_MES225.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES226
                ON
                        GEN_MES226.REGISTER_DIVI_CD = '1'
                    AND GEN_MES226.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES226.GEN_CD = '26'
                    AND GEN_MES226.CENTER_ID = '@@@'
                    AND GEN_MES226.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES227
                ON
                        GEN_MES227.REGISTER_DIVI_CD = '1'
                    AND GEN_MES227.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE2'
                    AND GEN_MES227.GEN_CD = '27'
                    AND GEN_MES227.CENTER_ID = '@@@'
                    AND GEN_MES227.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES31
                ON
                        GEN_MES31.REGISTER_DIVI_CD = '1'
                    AND GEN_MES31.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE3'
                    AND GEN_MES31.GEN_CD = '1'
                    AND GEN_MES31.CENTER_ID = '@@@'
                    AND GEN_MES31.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES32
                ON
                        GEN_MES32.REGISTER_DIVI_CD = '1'
                    AND GEN_MES32.GEN_DIV_CD = 'EC_OKAIAGE_MESSAGE3'
                    AND GEN_MES32.GEN_CD = '2'
                    AND GEN_MES32.CENTER_ID = '@@@'
                    AND GEN_MES32.SHIPPER_ID = ECSHIP.SHIPPER_ID
                ORDER BY
                        ECSHIP.SHIP_INSTRUCT_SEQ
            ");
            parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            return MvcDbContext.Current.Database.Connection.Query<PrintNouhinEc>(query.ToString(), parameters);
        }


        /// <summary>
        ///  楽天納品書発行CSVデータ取得
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<PrintNouhinEcRakuten> GetPrintNouhinEcRakutens(PrintEcInvoiceConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
               WITH
                    SELECTED_ECSHIPS_DATA AS (
                        SELECT
                                CENTER_ID 
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   DEST_FAMILY_NAME 
                            ,   DEST_FIRST_NAME
                            ,   ORDER_DATE
                            ,   CLIENT_FAMILY_NAME 
                            ,   CLIENT_FIRST_NAME
                            ,   PAYMENT_METHOD
                            ,   SHIP_REQUEST_DATE
                            ,   DEST_ZIP
                            ,   DEST_PREF_NAME
                            ,   DEST_CITY_NAME 
                            ,   DEST_ADDRESS1 
                            ,   DEST_ADDRESS2
                            ,   DEST_ADDRESS3
                            ,   DEST_TEL
                            ,   ARRIVE_REQUEST_DATE
                            ,   TAX_RATE
                            ,   MAX(TAX_RATE) OVER (PARTITION BY SHIP_INSTRUCT_ID) AS MAX_TAX
                            ,   ITEM_ID
                            ,   ITEM_NAME
                            ,   ITEM_COLOR_ID
                            ,   ITEM_SIZE_ID
                            ,   JAN
                            ,   SALE_UNIT_PRICE
                            ,   ORDER_QTY
                            ,   ITEM_AMOUNT
                            ,   TOTAL_TAX_AMOUNT
                            ,   TOTAL_ITEM_AMOUNT
                            ,   CARRIAGE_AMOUNT
                            ,   COMMISSION_AMOUNT AS CASH_ON_DELIVERY_AMOUNT
                            ,   POINT_DISCOUNT_AMOUNT
                            ,   COUPON_DISCOUNT_AMOUNT
                            ,   TOTAL_CLAIM_AMOUNT
                        FROM
                                T_ECSHIPS
                        WHERE 1=1
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                            AND SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                )
            ");

            if (condition.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                    , SELECTED_AECSHIPS_DATA AS(
                        SELECT
                                CENTER_ID 
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   DEST_FAMILY_NAME 
                            ,   DEST_FIRST_NAME
                            ,   ORDER_DATE
                            ,   CLIENT_FAMILY_NAME 
                            ,   CLIENT_FIRST_NAME
                            ,   PAYMENT_METHOD
                            ,   SHIP_REQUEST_DATE
                            ,   DEST_ZIP
                            ,   DEST_PREF_NAME
                            ,   DEST_CITY_NAME 
                            ,   DEST_ADDRESS1 
                            ,   DEST_ADDRESS2
                            ,   DEST_ADDRESS3
                            ,   DEST_TEL
                            ,   ARRIVE_REQUEST_DATE
                            ,   TAX_RATE
                            ,   MAX(TAX_RATE) OVER (PARTITION BY SHIP_INSTRUCT_ID) AS MAX_TAX
                            ,   ITEM_ID
                            ,   ITEM_NAME
                            ,   ITEM_COLOR_ID
                            ,   ITEM_SIZE_ID
                            ,   JAN
                            ,   SALE_UNIT_PRICE
                            ,   ORDER_QTY
                            ,   ITEM_AMOUNT
                            ,   TOTAL_TAX_AMOUNT
                            ,   TOTAL_ITEM_AMOUNT
                            ,   CARRIAGE_AMOUNT
                            ,   COMMISSION_AMOUNT AS CASH_ON_DELIVERY_AMOUNT
                            ,   POINT_DISCOUNT_AMOUNT
                            ,   COUPON_DISCOUNT_AMOUNT
                            ,   TOTAL_CLAIM_AMOUNT
                        FROM
                                A_ECSHIPS
                        WHERE
                                SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                    )
                    , SELECTED_ALL_ECSHIPS_DATA AS(
                            SELECT * FROM SELECTED_ECSHIPS_DATA
                            UNION
                            SELECT * FROM SELECTED_AECSHIPS_DATA
                    )
                ");
            }

            query.AppendLine(@"
                ,   TARGET_GENERAL_DATA AS (
                        SELECT
                                GEN_DIV_CD
                            ,   GEN_CD
                            ,   GEN_NAME
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   REGISTER_DIVI_CD
                        FROM
                                M_GENERALS
                        WHERE
                                REGISTER_DIVI_CD = '1'
                            AND GEN_DIV_CD IN ('EC_RAKUTEN_OKAIAGE_SHIPPER','EC_RAKUTEN_OKAIAGE_MESSAGE1')
                            AND SHIPPER_ID = :SHIPPER_ID
                )
                SELECT
                      ECSHIP.DEST_FAMILY_NAME   || '　' || ECSHIP.DEST_FIRST_NAME || '　様' AS DEST_NAME
                    , ECSHIP.CLIENT_FAMILY_NAME || '　' || ECSHIP.CLIENT_FIRST_NAME || '　様' AS CLIENT_NAME
                    , GEN_SHIP1.GEN_NAME AS EC_RAKUTEN_OKAIAGE_SHIPPER1
                    , GEN_SHIP2.GEN_NAME AS EC_RAKUTEN_OKAIAGE_SHIPPER2
                    , GEN_SHIP3.GEN_NAME AS EC_RAKUTEN_OKAIAGE_SHIPPER3
                    , GEN_SHIP4.GEN_NAME AS EC_RAKUTEN_OKAIAGE_SHIPPER4
                    , GEN_SHIP5.GEN_NAME AS EC_RAKUTEN_OKAIAGE_SHIPPER5
                    , GEN_SHIP6.GEN_NAME AS EC_RAKUTEN_OKAIAGE_SHIPPER6
                    , GEN_SHIP7.GEN_NAME AS EC_RAKUTEN_OKAIAGE_SHIPPER7

                    , ECSHIP.SHIP_INSTRUCT_ID AS ORDER_NO
                    , TO_CHAR(ECSHIP.ORDER_DATE, 'YYYY""年""MM""月""DD""日"" HH24:MI:SS') AS ORDER_DATE
                    , ECSHIP.PAYMENT_METHOD
                    , TO_CHAR(SYSDATE, 'YYYY""年""MM""月""DD""日""') AS PRINT_DATE
                    , ECSHIP.ITEM_ID
                    , 'カラー：' || COLOR.ITEM_COLOR_NAME AS COLOR_NAME
                    , 'サイズ：' || SIZES.ITEM_SIZE_NAME AS SIZE_NAME
                    , ECSHIP.ITEM_NAME
                    , ECSHIP.JAN
                    , ECSHIP.TAX_RATE || '%' AS TAX_RATE
                    , ECSHIP.ORDER_QTY
                    , ECSHIP.SALE_UNIT_PRICE AS TANKA
                    , ECSHIP.ITEM_AMOUNT                                              --金額
                    , ECSHIP.TOTAL_ITEM_AMOUNT                                        --小計
                    , 0 AS TOTAL_TAX                                                  --消費税
                    , ECSHIP.CARRIAGE_AMOUNT
                    , ECSHIP.CASH_ON_DELIVERY_AMOUNT                                  --手数料(代引/決済)
                    , (ECSHIP.POINT_DISCOUNT_AMOUNT  * - 1) AS POINT_DISCOUNT_AMOUNT
                    , (ECSHIP.COUPON_DISCOUNT_AMOUNT * - 1) AS COUPON_DISCOUNT_AMOUNT
                    , ECSHIP.TOTAL_CLAIM_AMOUNT AS TOTAL_CLAIM_AMOUNT                 --代金総額
                    , ECSHIP.TOTAL_CLAIM_AMOUNT AS TOTAL_TAX_AMOUNT
                    , ECSHIP.MAX_TAX || '%対象(税込)' AS TOTAL_TAX_AMOUNT_TITLE
                    , GEN_MES11.GEN_NAME AS EC_RAKUTEN_OKAIAGE_MESSAGE11
                    , GEN_MES12.GEN_NAME AS EC_RAKUTEN_OKAIAGE_MESSAGE12
                    , GEN_MES13.GEN_NAME AS EC_RAKUTEN_OKAIAGE_MESSAGE13

                FROM

            ");
            if (condition.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                        SELECTED_ALL_ECSHIPS_DATA ECSHIP
                ");
            }
            else
            {
                query.AppendLine(@"
                        SELECTED_ECSHIPS_DATA ECSHIP 
                ");
            }

            query.AppendLine(@"

                LEFT OUTER JOIN
                        M_COLORS COLOR
                ON
                        COLOR.ITEM_COLOR_ID = ECSHIP.ITEM_COLOR_ID
                    AND COLOR.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        M_SIZES SIZES
                ON
                        SIZES.ITEM_SIZE_ID = ECSHIP.ITEM_SIZE_ID
                    AND SIZES.SHIPPER_ID = ECSHIP.SHIPPER_ID

                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_SHIP1
                ON
                        GEN_SHIP1.REGISTER_DIVI_CD = '1'
                    AND GEN_SHIP1.GEN_DIV_CD = 'EC_RAKUTEN_OKAIAGE_SHIPPER'
                    AND GEN_SHIP1.GEN_CD = '1'
                    AND GEN_SHIP1.CENTER_ID = ECSHIP.CENTER_ID
                    AND GEN_SHIP1.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_SHIP2
                ON
                        GEN_SHIP2.REGISTER_DIVI_CD = '1'
                    AND GEN_SHIP2.GEN_DIV_CD = 'EC_RAKUTEN_OKAIAGE_SHIPPER'
                    AND GEN_SHIP2.GEN_CD = '2'
                    AND GEN_SHIP2.CENTER_ID = ECSHIP.CENTER_ID
                    AND GEN_SHIP2.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_SHIP3
                ON
                        GEN_SHIP3.REGISTER_DIVI_CD = '1'
                    AND GEN_SHIP3.GEN_DIV_CD = 'EC_RAKUTEN_OKAIAGE_SHIPPER'
                    AND GEN_SHIP3.GEN_CD = '3'
                    AND GEN_SHIP3.CENTER_ID = ECSHIP.CENTER_ID
                    AND GEN_SHIP3.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN

                        TARGET_GENERAL_DATA GEN_SHIP4
                ON
                        GEN_SHIP4.REGISTER_DIVI_CD = '1'
                    AND GEN_SHIP4.GEN_DIV_CD = 'EC_RAKUTEN_OKAIAGE_SHIPPER'
                    AND GEN_SHIP4.GEN_CD = '4'
                    AND GEN_SHIP4.CENTER_ID = ECSHIP.CENTER_ID
                    AND GEN_SHIP4.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_SHIP5
                ON
                        GEN_SHIP5.REGISTER_DIVI_CD = '1'
                    AND GEN_SHIP5.GEN_DIV_CD = 'EC_RAKUTEN_OKAIAGE_SHIPPER'
                    AND GEN_SHIP5.GEN_CD = '5'
                    AND GEN_SHIP5.CENTER_ID = ECSHIP.CENTER_ID
                    AND GEN_SHIP5.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_SHIP6
                ON
                        GEN_SHIP6.REGISTER_DIVI_CD = '1'
                    AND GEN_SHIP6.GEN_DIV_CD = 'EC_RAKUTEN_OKAIAGE_SHIPPER'
                    AND GEN_SHIP6.GEN_CD = '6'
                    AND GEN_SHIP6.CENTER_ID = ECSHIP.CENTER_ID
                    AND GEN_SHIP6.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_SHIP7
                ON
                        GEN_SHIP7.REGISTER_DIVI_CD = '1'
                    AND GEN_SHIP7.GEN_DIV_CD = 'EC_RAKUTEN_OKAIAGE_SHIPPER'
                    AND GEN_SHIP7.GEN_CD = '7'
                    AND GEN_SHIP7.CENTER_ID = ECSHIP.CENTER_ID
                    AND GEN_SHIP7.SHIPPER_ID = ECSHIP.SHIPPER_ID

                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES11
                ON
                        GEN_MES11.REGISTER_DIVI_CD = '1'
                    AND GEN_MES11.GEN_DIV_CD = 'EC_RAKUTEN_OKAIAGE_MESSAGE1'
                    AND GEN_MES11.GEN_CD = '1'
                    AND GEN_MES11.CENTER_ID = '@@@'
                    AND GEN_MES11.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES12
                ON
                        GEN_MES12.REGISTER_DIVI_CD = '1'
                    AND GEN_MES12.GEN_DIV_CD = 'EC_RAKUTEN_OKAIAGE_MESSAGE1'
                    AND GEN_MES12.GEN_CD = '2'
                    AND GEN_MES12.CENTER_ID = '@@@'
                    AND GEN_MES12.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES13
                ON
                        GEN_MES13.REGISTER_DIVI_CD = '1'
                    AND GEN_MES13.GEN_DIV_CD = 'EC_RAKUTEN_OKAIAGE_MESSAGE1'
                    AND GEN_MES13.GEN_CD = '3'
                    AND GEN_MES13.CENTER_ID = '@@@'
                    AND GEN_MES13.SHIPPER_ID = ECSHIP.SHIPPER_ID

                ORDER BY
                        ECSHIP.SHIP_INSTRUCT_SEQ
            ");
            parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            return MvcDbContext.Current.Database.Connection.Query<PrintNouhinEcRakuten>(query.ToString(), parameters);
        }

        /// <summary>
        /// WaKsnap納品書発行CSVデータ取得
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<PrintNouhinEcWaKsnap> GetPrintNouhinEcWaKsnaps(PrintEcInvoiceConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
                WITH
                    SELECTED_ECSHIPS_DATA AS (
                        SELECT
                                CENTER_ID 
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   DEST_FAMILY_NAME 
                            ,   DEST_FIRST_NAME
                            ,   ORDER_DATE
                            ,   PAYMENT_METHOD
                            ,   SHIP_REQUEST_DATE
                            ,   DEST_ZIP
                            ,   DEST_PREF_NAME
                            ,   DEST_CITY_NAME 
                            ,   DEST_ADDRESS1 
                            ,   DEST_ADDRESS2
                            ,   DEST_ADDRESS3
                            ,   DEST_TEL
                            ,   ARRIVE_REQUEST_DATE
                            ,   ITEM_ID
                            ,   ITEM_NAME
                            ,   ITEM_COLOR_ID
                            ,   ITEM_SIZE_ID
                            ,   JAN
                            ,   SALE_UNIT_PRICE
                            ,   ORDER_QTY
                            ,   ITEM_AMOUNT
                            ,   TOTAL_ITEM_AMOUNT
                            ,   CARRIAGE_AMOUNT
                            ,   COMMISSION_AMOUNT
                            ,   POINT_DISCOUNT_AMOUNT
                            ,   TOTAL_CLAIM_AMOUNT
                        FROM
                                T_ECSHIPS
                        WHERE
                                SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                )
            ");

            if (condition.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                    , SELECTED_AECSHIPS_DATA AS(
                        SELECT
                                CENTER_ID 
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   DEST_FAMILY_NAME 
                            ,   DEST_FIRST_NAME
                            ,   ORDER_DATE
                            ,   PAYMENT_METHOD
                            ,   SHIP_REQUEST_DATE
                            ,   DEST_ZIP
                            ,   DEST_PREF_NAME
                            ,   DEST_CITY_NAME 
                            ,   DEST_ADDRESS1 
                            ,   DEST_ADDRESS2
                            ,   DEST_ADDRESS3
                            ,   DEST_TEL
                            ,   ARRIVE_REQUEST_DATE
                            ,   ITEM_ID
                            ,   ITEM_NAME
                            ,   ITEM_COLOR_ID
                            ,   ITEM_SIZE_ID
                            ,   JAN
                            ,   SALE_UNIT_PRICE
                            ,   ORDER_QTY
                            ,   ITEM_AMOUNT
                            ,   TOTAL_ITEM_AMOUNT
                            ,   CARRIAGE_AMOUNT
                            ,   COMMISSION_AMOUNT
                            ,   POINT_DISCOUNT_AMOUNT
                            ,   TOTAL_CLAIM_AMOUNT
                        FROM
                                A_ECSHIPS
                        WHERE
                                SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                    )
                    , SELECTED_ALL_ECSHIPS_DATA AS(
                            SELECT * FROM SELECTED_ECSHIPS_DATA
                            UNION
                            SELECT * FROM SELECTED_AECSHIPS_DATA
                    )
                ");
            }
            query.AppendLine(@"
                ,   GROUP_SUM_ORDER_QTY AS (
                        SELECT
                                SHIP_INSTRUCT_ID
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   SUM(ORDER_QTY) AS SUM_ORDER_QTY
                        FROM

            ");
            if (condition.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                        SELECTED_ALL_ECSHIPS_DATA 
                ");
            }
            else
            {
                query.AppendLine(@"
                        SELECTED_ECSHIPS_DATA 
                ");
            }
            query.AppendLine(@"
                        GROUP BY
                                SHIP_INSTRUCT_ID
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                )
                ,   TARGET_GENERAL_DATA AS (
                        SELECT
                                GEN_DIV_CD
                            ,   GEN_CD
                            ,   GEN_NAME
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   REGISTER_DIVI_CD
                        FROM
                                M_GENERALS
                        WHERE
                                REGISTER_DIVI_CD = '1'
                            AND GEN_DIV_CD IN ('EC_OKAIAGE_WAKSNAP_STORE_NAME','EC_OKAIAGE_WAKSNAP_SHIPPER','EC_OKAIAGE_WAKSNAP_MESSAGE1','EC_OKAIAGE_WAKSNAP_MESSAGE2','EC_OKAIAGE_WAKSNAP_MESSAGE3')
                            AND SHIPPER_ID = :SHIPPER_ID
                )
                SELECT
                        ECSHIP.DEST_FAMILY_NAME || '　' || ECSHIP.DEST_FIRST_NAME || '　様' AS DEST_NAME
                    ,   ECSHIP.SHIP_INSTRUCT_ID AS ORDER_NO
                    ,   TO_CHAR(ECSHIP.ORDER_DATE, 'YYYY""年""MM""月""DD""日""') AS ORDER_DATE
                    ,   ECSHIP.PAYMENT_METHOD
                    ,   GEN_STORE1.GEN_NAME                                             AS EC_OKAIAGE_STORE_NAME
                    ,   TO_CHAR(ECSHIP.SHIP_REQUEST_DATE, 'YYYY""年""MM""月""DD""日""') AS SHIP_DATE
                    ,   GRP.SUM_ORDER_QTY AS SUM_ORDER_QTY
                    ,   ECSHIP.DEST_ZIP
                    ,   ECSHIP.DEST_PREF_NAME || ECSHIP.DEST_CITY_NAME || ECSHIP.DEST_ADDRESS1 || ECSHIP.DEST_ADDRESS2 || ECSHIP.DEST_ADDRESS3 AS DEST_ADDRESS
                    ,   ECSHIP.DEST_TEL
                    ,   TO_CHAR(ECSHIP.ARRIVE_REQUEST_DATE, 'YYYY""年""MM""月""DD""日""') AS ARRIVE_REQUEST_DATE
                    ,   GEN_STORE1.GEN_NAME    AS EC_OKAIAGE_STORE_NAME1
                    ,   GEN_STORE2.GEN_NAME    AS EC_OKAIAGE_STORE_NAME2
                    ,   GEN_SHIP1.GEN_NAME     AS EC_OKAIAGE_SHIPPER1
                    ,   GEN_SHIP2.GEN_NAME     AS EC_OKAIAGE_SHIPPER2
                    ,   GEN_SHIP3.GEN_NAME     AS EC_OKAIAGE_SHIPPER3
                    ,   ECSHIP.ITEM_ID
                    ,   COLOR.ITEM_COLOR_NAME  AS COLOR_NAME
                    ,   SIZES.ITEM_SIZE_NAME   AS SIZE_NAME
                    ,   ECSHIP.ITEM_NAME
                    ,   ECSHIP.JAN
                    ,   ECSHIP.SALE_UNIT_PRICE AS TANKA
                    ,   ECSHIP.ORDER_QTY
                    ,   ECSHIP.ITEM_AMOUNT
                    ,   ECSHIP.TOTAL_ITEM_AMOUNT
                    ,   ECSHIP.CARRIAGE_AMOUNT
                    ,   ECSHIP.COMMISSION_AMOUNT
                    ,   ECSHIP.POINT_DISCOUNT_AMOUNT
                    ,   ECSHIP.TOTAL_CLAIM_AMOUNT
                    ,   ECSHIP.SHIP_INSTRUCT_ID AS ORDER_BARCODE
                    ,   GEN_MES11.GEN_NAME      AS EC_OKAIAGE_MESSAGE11
                    ,   GEN_MES12.GEN_NAME      AS EC_OKAIAGE_MESSAGE12
                    ,   GEN_MES13.GEN_NAME      AS EC_OKAIAGE_MESSAGE13
                    ,   GEN_MES21.GEN_NAME      AS EC_OKAIAGE_MESSAGE21
                    ,   GEN_MES22.GEN_NAME      AS EC_OKAIAGE_MESSAGE22
                    ,   GEN_MES23.GEN_NAME      AS EC_OKAIAGE_MESSAGE23
                    ,   GEN_MES24.GEN_NAME      AS EC_OKAIAGE_MESSAGE24
                    ,   GEN_MES25.GEN_NAME      AS EC_OKAIAGE_MESSAGE25
                    ,   GEN_MES26.GEN_NAME      AS EC_OKAIAGE_MESSAGE26
                    ,   GEN_MES27.GEN_NAME      AS EC_OKAIAGE_MESSAGE27
                    ,   GEN_MES28.GEN_NAME      AS EC_OKAIAGE_MESSAGE28
                    ,   GEN_MES29.GEN_NAME      AS EC_OKAIAGE_MESSAGE29
                    ,   GEN_MES210.GEN_NAME     AS EC_OKAIAGE_MESSAGE210
                    ,   GEN_MES211.GEN_NAME     AS EC_OKAIAGE_MESSAGE211
                    ,   GEN_MES212.GEN_NAME     AS EC_OKAIAGE_MESSAGE212
                    ,   GEN_MES213.GEN_NAME     AS EC_OKAIAGE_MESSAGE213
                    ,   GEN_MES214.GEN_NAME     AS EC_OKAIAGE_MESSAGE214
                    ,   GEN_MES215.GEN_NAME     AS EC_OKAIAGE_MESSAGE215
                    ,   GEN_MES216.GEN_NAME     AS EC_OKAIAGE_MESSAGE216
                    ,   GEN_MES217.GEN_NAME     AS EC_OKAIAGE_MESSAGE217
                    ,   GEN_MES218.GEN_NAME     AS EC_OKAIAGE_MESSAGE218
                    ,   GEN_MES219.GEN_NAME     AS EC_OKAIAGE_MESSAGE219
                    ,   GEN_MES220.GEN_NAME     AS EC_OKAIAGE_MESSAGE220
                    ,   GEN_MES221.GEN_NAME     AS EC_OKAIAGE_MESSAGE221
                    ,   GEN_MES222.GEN_NAME     AS EC_OKAIAGE_MESSAGE222
                    ,   GEN_MES223.GEN_NAME     AS EC_OKAIAGE_MESSAGE223
                    ,   GEN_MES224.GEN_NAME     AS EC_OKAIAGE_MESSAGE224
                    ,   GEN_MES225.GEN_NAME     AS EC_OKAIAGE_MESSAGE225
                    ,   GEN_MES226.GEN_NAME     AS EC_OKAIAGE_MESSAGE226
                    ,   GEN_MES227.GEN_NAME     AS EC_OKAIAGE_MESSAGE227
                    ,   GEN_MES228.GEN_NAME     AS EC_OKAIAGE_MESSAGE228
                    ,   GEN_MES229.GEN_NAME     AS EC_OKAIAGE_MESSAGE229
                    ,   GEN_MES230.GEN_NAME     AS EC_OKAIAGE_MESSAGE230
                    ,   GEN_MES31.GEN_NAME      AS EC_OKAIAGE_MESSAGE31
                    ,   GEN_MES32.GEN_NAME      AS EC_OKAIAGE_MESSAGE32
                FROM
            ");
            if (condition.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                        SELECTED_ALL_ECSHIPS_DATA ECSHIP
                ");
            }
            else
            {
                query.AppendLine(@"
                        SELECTED_ECSHIPS_DATA ECSHIP 
                ");
            }
            query.AppendLine(@"

                LEFT OUTER JOIN
                        GROUP_SUM_ORDER_QTY GRP
                ON
                        GRP.SHIP_INSTRUCT_ID = ECSHIP.SHIP_INSTRUCT_ID
                    AND GRP.CENTER_ID = ECSHIP.CENTER_ID
                    AND GRP.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        M_COLORS COLOR
                ON
                        COLOR.ITEM_COLOR_ID = ECSHIP.ITEM_COLOR_ID
                    AND COLOR.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        M_SIZES SIZES
                ON
                        SIZES.ITEM_SIZE_ID = ECSHIP.ITEM_SIZE_ID
                    AND SIZES.SHIPPER_ID = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_STORE1
                ON
                        GEN_STORE1.REGISTER_DIVI_CD = '1'
                    AND GEN_STORE1.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_STORE_NAME'
                    AND GEN_STORE1.GEN_CD           = '1'
                    AND GEN_STORE1.CENTER_ID        = '@@@'
                    AND GEN_STORE1.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_STORE2
                ON
                        GEN_STORE2.REGISTER_DIVI_CD = '1'
                    AND GEN_STORE2.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_STORE_NAME'
                    AND GEN_STORE2.GEN_CD           = '2'
                    AND GEN_STORE2.CENTER_ID        = '@@@'
                    AND GEN_STORE2.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_SHIP1
                ON
                        GEN_SHIP1.REGISTER_DIVI_CD = '1'
                    AND GEN_SHIP1.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_SHIPPER'
                    AND GEN_SHIP1.GEN_CD           = '1'
                    AND GEN_SHIP1.CENTER_ID        = ECSHIP.CENTER_ID
                    AND GEN_SHIP1.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_SHIP2
                ON
                        GEN_SHIP2.REGISTER_DIVI_CD = '1'
                    AND GEN_SHIP2.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_SHIPPER'
                    AND GEN_SHIP2.GEN_CD           = '2'
                    AND GEN_SHIP2.CENTER_ID        = ECSHIP.CENTER_ID
                    AND GEN_SHIP2.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_SHIP3
                ON
                        GEN_SHIP3.REGISTER_DIVI_CD = '1'
                    AND GEN_SHIP3.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_SHIPPER'
                    AND GEN_SHIP3.GEN_CD           = '3'
                    AND GEN_SHIP3.CENTER_ID        = ECSHIP.CENTER_ID
                    AND GEN_SHIP3.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES11
                ON
                        GEN_MES11.REGISTER_DIVI_CD = '1'
                    AND GEN_MES11.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE1'
                    AND GEN_MES11.GEN_CD           = '1'
                    AND GEN_MES11.CENTER_ID        = '@@@'
                    AND GEN_MES11.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES12
                ON
                        GEN_MES12.REGISTER_DIVI_CD = '1'
                    AND GEN_MES12.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE1'
                    AND GEN_MES12.GEN_CD           = '2'
                    AND GEN_MES12.CENTER_ID        = '@@@'
                    AND GEN_MES12.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES13
                ON
                        GEN_MES13.REGISTER_DIVI_CD = '1'
                    AND GEN_MES13.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE1'
                    AND GEN_MES13.GEN_CD           = '3'
                    AND GEN_MES13.CENTER_ID        = '@@@'
                    AND GEN_MES13.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES21
                ON
                        GEN_MES21.REGISTER_DIVI_CD = '1'
                    AND GEN_MES21.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES21.GEN_CD           = '1'
                    AND GEN_MES21.CENTER_ID        = '@@@'
                    AND GEN_MES21.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES22
                ON
                        GEN_MES22.REGISTER_DIVI_CD = '1'
                    AND GEN_MES22.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES22.GEN_CD           = '2'
                    AND GEN_MES22.CENTER_ID        = '@@@'
                    AND GEN_MES22.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES23
                ON
                        GEN_MES23.REGISTER_DIVI_CD = '1'
                    AND GEN_MES23.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES23.GEN_CD           = '3'
                    AND GEN_MES23.CENTER_ID        = '@@@'
                    AND GEN_MES23.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES24
                ON
                        GEN_MES24.REGISTER_DIVI_CD = '1'
                    AND GEN_MES24.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES24.GEN_CD           = '4'
                    AND GEN_MES24.CENTER_ID        = '@@@'
                    AND GEN_MES24.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES25
                ON
                        GEN_MES25.REGISTER_DIVI_CD = '1'
                    AND GEN_MES25.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES25.GEN_CD           = '5'
                    AND GEN_MES25.CENTER_ID        = '@@@'
                    AND GEN_MES25.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES26
                ON
                        GEN_MES26.REGISTER_DIVI_CD = '1'
                    AND GEN_MES26.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES26.GEN_CD           = '6'
                    AND GEN_MES26.CENTER_ID        = '@@@'
                    AND GEN_MES26.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES27
                ON
                        GEN_MES27.REGISTER_DIVI_CD = '1'
                    AND GEN_MES27.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES27.GEN_CD           = '7'
                    AND GEN_MES27.CENTER_ID        = '@@@'
                    AND GEN_MES27.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES28
                ON
                        GEN_MES28.REGISTER_DIVI_CD = '1'
                    AND GEN_MES28.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES28.GEN_CD           = '8'
                    AND GEN_MES28.CENTER_ID        = '@@@'
                    AND GEN_MES28.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES29
                ON
                        GEN_MES29.REGISTER_DIVI_CD = '1'
                    AND GEN_MES29.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES29.GEN_CD           = '9'
                    AND GEN_MES29.CENTER_ID        = '@@@'
                    AND GEN_MES28.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES210
                ON
                        GEN_MES210.REGISTER_DIVI_CD = '1'
                    AND GEN_MES210.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES210.GEN_CD           = '10'
                    AND GEN_MES210.CENTER_ID        = '@@@'
                    AND GEN_MES210.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES211
                ON
                        GEN_MES211.REGISTER_DIVI_CD = '1'
                    AND GEN_MES211.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES211.GEN_CD           = '11'
                    AND GEN_MES211.CENTER_ID        = '@@@'
                    AND GEN_MES211.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES212
                ON
                        GEN_MES212.REGISTER_DIVI_CD = '1'
                    AND GEN_MES212.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES212.GEN_CD           = '12'
                    AND GEN_MES212.CENTER_ID        = '@@@'
                    AND GEN_MES212.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES213
                ON
                        GEN_MES213.REGISTER_DIVI_CD = '1'
                    AND GEN_MES213.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES213.GEN_CD           = '13'
                    AND GEN_MES213.CENTER_ID        = '@@@'
                    AND GEN_MES213.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES214
                ON
                        GEN_MES214.REGISTER_DIVI_CD = '1'
                    AND GEN_MES214.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES214.GEN_CD           = '14'
                    AND GEN_MES214.CENTER_ID        = '@@@'
                    AND GEN_MES214.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES215
                ON
                        GEN_MES215.REGISTER_DIVI_CD = '1'
                    AND GEN_MES215.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES215.GEN_CD           = '15'
                    AND GEN_MES215.CENTER_ID        = '@@@'
                    AND GEN_MES215.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES216
                ON
                        GEN_MES216.REGISTER_DIVI_CD = '1'
                    AND GEN_MES216.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES216.GEN_CD           = '16'
                    AND GEN_MES216.CENTER_ID        = '@@@'
                    AND GEN_MES216.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES217
                ON
                        GEN_MES217.REGISTER_DIVI_CD = '1'
                    AND GEN_MES217.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES217.GEN_CD           = '17'
                    AND GEN_MES217.CENTER_ID        = '@@@'
                    AND GEN_MES217.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES218
                ON
                        GEN_MES218.REGISTER_DIVI_CD = '1'
                    AND GEN_MES218.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES218.GEN_CD           = '18'
                    AND GEN_MES218.CENTER_ID        = '@@@'
                    AND GEN_MES218.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES219
                ON
                        GEN_MES219.REGISTER_DIVI_CD = '1'
                    AND GEN_MES219.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES219.GEN_CD           = '19'
                    AND GEN_MES219.CENTER_ID        = '@@@'
                    AND GEN_MES219.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES220
                ON
                        GEN_MES220.REGISTER_DIVI_CD = '1'
                    AND GEN_MES220.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES220.GEN_CD           = '20'
                    AND GEN_MES220.CENTER_ID        = '@@@'
                    AND GEN_MES220.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES221
                ON
                        GEN_MES221.REGISTER_DIVI_CD = '1'
                    AND GEN_MES221.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES221.GEN_CD           = '21'
                    AND GEN_MES221.CENTER_ID        = '@@@'
                    AND GEN_MES221.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES222
                ON
                        GEN_MES222.REGISTER_DIVI_CD = '1'
                    AND GEN_MES222.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES222.GEN_CD           = '22'
                    AND GEN_MES222.CENTER_ID        = '@@@'
                    AND GEN_MES222.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES223
                ON
                        GEN_MES223.REGISTER_DIVI_CD = '1'
                    AND GEN_MES223.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES223.GEN_CD           = '23'
                    AND GEN_MES223.CENTER_ID        = '@@@'
                    AND GEN_MES223.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES224
                ON
                        GEN_MES224.REGISTER_DIVI_CD = '1'
                    AND GEN_MES224.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES224.GEN_CD           = '24'
                    AND GEN_MES224.CENTER_ID        = '@@@'
                    AND GEN_MES224.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES225
                ON
                        GEN_MES225.REGISTER_DIVI_CD = '1'
                    AND GEN_MES225.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES225.GEN_CD           = '25'
                    AND GEN_MES225.CENTER_ID        = '@@@'
                    AND GEN_MES225.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES226
                ON
                        GEN_MES226.REGISTER_DIVI_CD = '1'
                    AND GEN_MES226.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES226.GEN_CD           = '26'
                    AND GEN_MES226.CENTER_ID        = '@@@'
                    AND GEN_MES226.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES227
                ON
                        GEN_MES227.REGISTER_DIVI_CD = '1'
                    AND GEN_MES227.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES227.GEN_CD           = '27'
                    AND GEN_MES227.CENTER_ID        = '@@@'
                    AND GEN_MES227.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES228
                ON
                        GEN_MES228.REGISTER_DIVI_CD = '1'
                    AND GEN_MES228.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES228.GEN_CD           = '28'
                    AND GEN_MES228.CENTER_ID        = '@@@'
                    AND GEN_MES228.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES229
                ON
                        GEN_MES229.REGISTER_DIVI_CD = '1'
                    AND GEN_MES229.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES229.GEN_CD           = '29'
                    AND GEN_MES229.CENTER_ID        = '@@@'
                    AND GEN_MES229.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES230
                ON
                        GEN_MES230.REGISTER_DIVI_CD = '1'
                    AND GEN_MES230.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE2'
                    AND GEN_MES230.GEN_CD           = '30'
                    AND GEN_MES230.CENTER_ID        = '@@@'
                    AND GEN_MES230.SHIPPER_ID       = ECSHIP.SHIPPER_ID

                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES31
                ON
                        GEN_MES31.REGISTER_DIVI_CD = '1'
                    AND GEN_MES31.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE3'
                    AND GEN_MES31.GEN_CD           = '1'
                    AND GEN_MES31.CENTER_ID        = '@@@'
                    AND GEN_MES31.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        TARGET_GENERAL_DATA GEN_MES32
                ON
                        GEN_MES32.REGISTER_DIVI_CD = '1'
                    AND GEN_MES32.GEN_DIV_CD       = 'EC_OKAIAGE_WAKSNAP_MESSAGE3'
                    AND GEN_MES32.GEN_CD           = '2'
                    AND GEN_MES32.CENTER_ID        = '@@@'
                    AND GEN_MES32.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                ORDER BY
                        ECSHIP.SHIP_INSTRUCT_SEQ
            ");
            parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            return MvcDbContext.Current.Database.Connection.Query<PrintNouhinEcWaKsnap>(query.ToString(), parameters);
        }

        /// <summary>
        /// 佐川送り状CSVデータ取得
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<ViewModels.PrintInvoice.PrintInvoiceSagawa> GetPrintInvoiceSagawas(PrintEcInvoiceConditions condition)
        {
            #region コメントアウト
            // DynamicParameters parameters = new DynamicParameters();
            // StringBuilder query = new StringBuilder();
            // query.AppendLine(@"
            //     WITH
            //         SELECTED_PACKING_DATA AS (
            //             SELECT
            //                     CENTER_ID
            //                 ,   SHIPPER_ID
            //                 ,   DELI_NO
            //                 ,   TRIM(DELI_SHIWAKE_CD) AS DELI_SHIWAKE_CD
            //                 ,   UNIT_CNT
            //                 ,   SHIP_INSTRUCT_ID
            //             FROM
            //                     T_SHIP_PACKING_INFO
            //             WHERE
            //                     EC_FLAG = 1
            //                 AND SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
            //                 AND CENTER_ID = :CENTER_ID
            //                 AND SHIPPER_ID = :SHIPPER_ID
            //                 AND ROWNUM = 1
            //     )

            // ");

            // if (condition.ChkOldData == true)  //過去分含む
            // {
            //     query.AppendLine(@"
            //         , SELECTED_APACKING_DATA AS(
            //             SELECT
            //                     CENTER_ID
            //                 ,   SHIPPER_ID
            //                 ,   DELI_NO
            //                 ,   TRIM(DELI_SHIWAKE_CD) AS DELI_SHIWAKE_CD
            //                 ,   UNIT_CNT
            //                 ,   SHIP_INSTRUCT_ID
            //             FROM
            //                     A_SHIP_PACKING_INFO
            //             WHERE
            //                     EC_FLAG = 1
            //                 AND SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
            //                 AND CENTER_ID = :CENTER_ID
            //                 AND SHIPPER_ID = :SHIPPER_ID
            //                 AND ROWNUM = 1
            //         )
            //         , SELECTED_ALL_PACKING_DATA AS(
            //                 SELECT * FROM SELECTED_PACKING_DATA
            //                 UNION
            //                 SELECT * FROM SELECTED_APACKING_DATA
            //         )
            //     ");
            // }

            // query.AppendLine(@"
            //     ,   SELECTED_ECSHIP_DATA AS (
            //             SELECT
            //                     CENTER_ID 
            //                 ,   SHIPPER_ID
            //                 ,   SHIP_INSTRUCT_ID
            //                 ,   ARRIVE_REQUEST_DATE
            //                 ,   ARRIVE_REQUEST_TIME
            //                 ,   DEST_ZIP
            //                 ,   DEST_TEL
            //                 ,   DEST_PREF_NAME
            //                 ,   DEST_CITY_NAME
            //                 ,   DEST_ADDRESS1
            //                 ,   DEST_ADDRESS2
            //                 ,   DEST_FAMILY_NAME
            //                 ,   DEST_FIRST_NAME
            //                 ,   DAIBIKI_FLAG
            //                 ,   TOTAL_CLAIM_AMOUNT
            //              FROM
            //                     T_ECSHIPS
            //             WHERE
            //                     SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
            //                 AND CENTER_ID = :CENTER_ID
            //                 AND SHIPPER_ID = :SHIPPER_ID
            //                 AND ROWNUM = 1
            //     )

            //");

            // if (condition.ChkOldData == true)  //過去分含む
            // {
            //     query.AppendLine(@"
            //         , SELECTED_AECSHIP_DATA AS(
            //             SELECT
            //                     CENTER_ID 
            //                 ,   SHIPPER_ID
            //                 ,   SHIP_INSTRUCT_ID
            //                 ,   ARRIVE_REQUEST_DATE
            //                 ,   ARRIVE_REQUEST_TIME
            //                 ,   DEST_ZIP
            //                 ,   DEST_TEL
            //                 ,   DEST_PREF_NAME
            //                 ,   DEST_CITY_NAME
            //                 ,   DEST_ADDRESS1
            //                 ,   DEST_ADDRESS2
            //                 ,   DEST_FAMILY_NAME
            //                 ,   DEST_FIRST_NAME
            //                 ,   DAIBIKI_FLAG
            //                 ,   TOTAL_CLAIM_AMOUNT
            //             FROM
            //                     A_ECSHIPS
            //             WHERE
            //                     SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
            //                 AND CENTER_ID = :CENTER_ID
            //                 AND SHIPPER_ID = :SHIPPER_ID
            //                 AND ROWNUM = 1
            //         )
            //         , SELECTED_ALL_ECSHIPS_DATA AS(
            //                 SELECT * FROM SELECTED_ECSHIP_DATA
            //                 UNION
            //                 SELECT * FROM SELECTED_AECSHIP_DATA
            //         )
            //     ");
            // }

            // query.AppendLine(@"

            //     SELECT
            //             1 AS ROW_NO
            //         ,   LPAD(PACK.DELI_SHIWAKE_CD, 7 ,'$') AS DELI_SHIWAKE_CD
            //         ,   SUBSTR(PACK.DELI_SHIWAKE_CD, 1, LENGTH(PACK.DELI_SHIWAKE_CD) - 3) AS OFFICE_CD
            //         ,   SUBSTR(PACK.DELI_SHIWAKE_CD,-3) AS LOCAL_CD
            //         ,   TO_CHAR(SYSDATE, 'YYYYMMDD') AS PRINT_DATE
            //         ,   PACK.UNIT_CNT AS UNIT
            //         ,   '陸便' AS BIN_TYPE
            //         ,   TO_CHAR(ECSHIP.ARRIVE_REQUEST_DATE, 'MMDD') AS ARRIVE_REQUEST_DATE
            //         ,   CASE ECSHIP.ARRIVE_REQUEST_TIME WHEN N'00' THEN N'' ELSE ECSHIP.ARRIVE_REQUEST_TIME END AS ARRIVE_REQUEST_TIME
            //         ,   CASE ECSHIP.ARRIVE_REQUEST_TIME WHEN N'00' THEN N'' ELSE GEN.GEN_NAME END AS ARRIVE_REQUEST_TIME_NAME
            //         ,   ECSHIP.DEST_ZIP AS DEST_ZIP
            //         ,   ECSHIP.DEST_TEL AS DEST_TEL
            //         ,   SUBSTR(SF_GET_FULL_WIDTH_STRING(ECSHIP.DEST_PREF_NAME || ECSHIP.DEST_CITY_NAME), 1, 18) AS DEST_ADDRESS1
            //         ,   SUBSTR(SF_GET_FULL_WIDTH_STRING(ECSHIP.DEST_PREF_NAME || ECSHIP.DEST_CITY_NAME), 19) || ECSHIP.DEST_ADDRESS1 || ECSHIP.DEST_ADDRESS2 AS DEST_ADDRESS2
            //         ,   SUBSTR(SF_GET_FULL_WIDTH_STRING(ECSHIP.DEST_PREF_NAME || ECSHIP.DEST_CITY_NAME), 19) || ECSHIP.DEST_ADDRESS1 || ECSHIP.DEST_ADDRESS2 AS DEST_ADDRESS3
            //         ,   ECSHIP.DEST_FAMILY_NAME || ECSHIP.DEST_FIRST_NAME AS DEST_NAME
            //         ,   CONS.CONSIGNOR_ADDRESS1 AS CONSIGNOR_ADDRESS1
            //         ,   CONS.CONSIGNOR_ADDRESS2 AS CONSIGNOR_ADDRESS2
            //         ,   CONS.CONSIGNOR_NAME AS CONSIGNOR_NAME
            //         ,   CONS.CONSIGNOR_TEL AS CONSIGNOR_TEL
            //         ,   CONS.CLIENT_CD_OYA || CONS.CLIENT_CD_EDA || CONS.CLIENT_CD_C_D AS CLIENT_CD
            //         ,   PACK.DELI_NO AS DELI_NO
            //         ,   CONS.SALES_OFFICE_NAME AS SALE_OFFICE_NAME
            //         ,   CONS.SALES_OFFICE_TEL AS SALE_OFFICE_TEL
            //         ,   CONS.SALES_OFFICE_FAX AS SALE_OFFICE_FAX
            //         ,   '*' AS HANDLING_PRECAUTIONS_DESP
            //         ,   CASE 
            //                 WHEN TRIM(ECSHIP.ARRIVE_REQUEST_DATE) IS NULL THEN '*'
            //                 ELSE ''
            //             END AS ARRIVE_REQUEST_DATE_DESP
            //         ,   CASE 
            //                 WHEN ECSHIP.DAIBIKI_FLAG = 0 THEN '*'
            //                 ELSE ''
            //             END AS SPECIFY_DESP
            //         ,   ECSHIP.DAIBIKI_FLAG AS DAIBIKI_FLAG
            //         ,   CASE 
            //                 WHEN ECSHIP.DAIBIKI_FLAG = 1 THEN TO_CHAR(ECSHIP.TOTAL_CLAIM_AMOUNT)
            //                 ELSE ''
            //             END AS CLAIM_AMOUNT
            //         ,   CASE 
            //                 WHEN ECSHIP.DAIBIKI_FLAG = 1 THEN '0'
            //                 ELSE ''
            //             END AS TAX_AMOUNT
            //         ,   CASE 
            //                 WHEN ECSHIP.DAIBIKI_FLAG = 1 THEN '830046'
            //                 ELSE '830011'
            //             END AS CLASS_CD
            //         ,   CASE 
            //                 WHEN ECSHIP.DAIBIKI_FLAG = 1 THEN ''
            //                 ELSE '*'
            //             END AS TAX_AMOUNT_DESP
            //         ,   CASE 
            //                 WHEN ECSHIP.DAIBIKI_FLAG = 1 THEN ''
            //                 ELSE '*'
            //             END AS DAIKO_DESP
            //         ,   'd' || PACK.DELI_NO || 'd' AS DELI_NO_BARCODE
            //         ,   '' AS NUMBER_DESP
            //         ,   CASE 
            //                 WHEN ECSHIP.DAIBIKI_FLAG = 1 THEN '0'
            //                 ELSE '1'
            //             END AS DESP_FLAG
            //         ,   'd' || PACK.DELI_NO || 'd' AS DELI_NO_BARCODE_NIHUDA
            //     FROM

            // ");
            // if (condition.ChkOldData == true)  //過去分含む
            // {
            //     query.AppendLine(@"
            //             SELECTED_ALL_PACKING_DATA PACK
            //     ");
            // }
            // else
            // {
            //     query.AppendLine(@"
            //             SELECTED_PACKING_DATA PACK 
            //     ");
            // }
            // query.AppendLine(@"

            //     INNER JOIN

            // ");
            // if (condition.ChkOldData == true)  //過去分含む
            // {
            //     query.AppendLine(@"
            //             SELECTED_ALL_ECSHIPS_DATA ECSHIP
            //     ");
            // }
            // else
            // {
            //     query.AppendLine(@"
            //             SELECTED_ECSHIP_DATA ECSHIP 
            //     ");
            // }
            // query.AppendLine(@"

            //     ON
            //             ECSHIP.SHIP_INSTRUCT_ID = PACK.SHIP_INSTRUCT_ID
            //         AND ECSHIP.CENTER_ID = PACK.CENTER_ID
            //         AND ECSHIP.SHIPPER_ID = PACK.SHIPPER_ID
            //     INNER JOIN
            //             M_CONSIGNORS CONS
            //     ON
            //             CONS.CENTER_ID = PACK.CENTER_ID
            //         AND CONS.CONSIGNOR_TYPE_ID = '05'
            //         AND CONS.SHIPPER_ID = PACK.SHIPPER_ID
            //     LEFT OUTER JOIN
            //             M_GENERALS GEN
            //     ON
            //             GEN.REGISTER_DIVI_CD = '1'
            //         AND GEN.GEN_DIV_CD = 'SAGAWA_ARRIVE_REQUEST_TIME'
            //         AND GEN.GEN_CD = ECSHIP.ARRIVE_REQUEST_TIME
            //         AND GEN.CENTER_ID = '@@@'
            //         AND GEN.SHIPPER_ID = ECSHIP.SHIPPER_ID
            // ");
            // parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId);
            // parameters.Add(":CENTER_ID", condition.CenterId);
            // parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // var target_ec_data = MvcDbContext.Current.Database.Connection.Query<ViewModels.PrintInvoice.PrintInvoiceSagawa>(query.ToString(), parameters);
            // var print_sagawa_data = new List<ViewModels.PrintInvoice.PrintInvoiceSagawa>();
            // var first_row_data = target_ec_data.FirstOrDefault();

            // print_sagawa_data.Add(first_row_data);

            // if (first_row_data.Unit > 1)
            // {
            //     for (int i = 1; i <= first_row_data.Unit - 1; i++)
            //     {
            //         var row = new ViewModels.PrintInvoice.PrintInvoiceSagawa();
            //         row.RowNo = i + 1;
            //         row.DeliShiwakeCd = first_row_data.DeliShiwakeCd;
            //         row.OfficeCd = first_row_data.OfficeCd;
            //         row.LocalCd = first_row_data.LocalCd;
            //         row.PrintDate = first_row_data.PrintDate;
            //         row.Unit = first_row_data.Unit;
            //         row.BinType = first_row_data.BinType;
            //         row.ArriveRequestDate = first_row_data.ArriveRequestDate;
            //         row.ArriveRequestTime = first_row_data.ArriveRequestTime;
            //         row.ArriveRequestTimeName = first_row_data.ArriveRequestTimeName;
            //         row.DestZip = first_row_data.DestZip;
            //         row.DestTel = first_row_data.DestTel;
            //         row.DestAddress1 = first_row_data.DestAddress1;
            //         row.DestAddress2 = first_row_data.DestAddress2;
            //         row.DestAddress3 = "";
            //         row.DestName = first_row_data.DestName;
            //         row.ConsignorAddress1 = first_row_data.ConsignorAddress1;
            //         row.ConsignorAddress2 = first_row_data.ConsignorAddress2;
            //         row.ConsignorName = first_row_data.ConsignorName;
            //         row.ConsignorTel = first_row_data.ConsignorTel;
            //         row.ClientCd = first_row_data.ClientCd;
            //         row.DeliNo = first_row_data.DeliNo;
            //         row.SaleOfficeName = first_row_data.SaleOfficeName;
            //         row.SaleOfficeTel = first_row_data.SaleOfficeTel;
            //         row.SaleOfficeFax = first_row_data.SaleOfficeFax;
            //         row.HandlingPrecautionsDesp = first_row_data.HandlingPrecautionsDesp;
            //         row.ArriveRequestDateDesp = first_row_data.ArriveRequestDateDesp;
            //         row.SpecifyDesp = first_row_data.SpecifyDesp;
            //         row.DaibikiFlag = first_row_data.DaibikiFlag;
            //         row.ClaimAmount = null;
            //         row.TaxAmount = null;
            //         row.ClassCd = "";
            //         row.TaxAmountDesp = "*";
            //         row.DaikoDesp = "*";
            //         row.DeliNoBarcode = "";
            //         row.NumberDesp = (i + 1).ToString() + "/" + first_row_data.Unit.ToString();
            //         row.DespFlag = "1";
            //         row.DeliNoBarcodeNihuda = first_row_data.DeliNoBarcodeNihuda;
            //         print_sagawa_data.Add(row);
            //     };
            // };

            // return print_sagawa_data.AsEnumerable();

            return new List<ViewModels.PrintInvoice.PrintInvoiceSagawa>();
            #endregion
        }

        /// <summary>
        ///出力する帳票種別を取得する
        /// </summary>
        /// <param name="search"></param>
        public int GetPrintKind(PrintEcInvoiceConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                WITH
                    SELECTED_ECSHIPS_DATA AS (
                        SELECT
                                CENTER_ID 
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   EC_CLASS
                        FROM
                                T_ECSHIPS
                        WHERE
                                SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                )
            ");

            if (condition.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                    , SELECTED_AECSHIPS_DATA AS(
                        SELECT
                                CENTER_ID 
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   EC_CLASS
                        FROM
                                A_ECSHIPS
                        WHERE
                                SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                    )
                    , SELECTED_ALL_ECSHIPS_DATA AS(
                            SELECT * FROM SELECTED_ECSHIPS_DATA
                            UNION
                            SELECT * FROM SELECTED_AECSHIPS_DATA
                    )
                ");
            }
            query.AppendLine(@"

                        SELECT
                                EC_CLASS
                        FROM
            ");
            if (condition.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                        SELECTED_ALL_ECSHIPS_DATA 
                ");
            }
            else
            {
                query.AppendLine(@"
                        SELECTED_ECSHIPS_DATA  
                ");
            }
            //query.AppendLine(@"
            //   ");

            parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);


            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<int>(query.ToString(), parameters).FirstOrDefault();
        }


        /// <summary>
        /// ヤマト送り状CSVデータ取得EC
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<ViewModels.PrintInvoice.PrintInvoiceYamato> GetPrintInvoiceYamatos(PrintEcInvoiceConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
                WITH
                    SELECTED_PACKING_DATA AS (
                        SELECT
                                CENTER_ID
                            ,   SHIPPER_ID
                            ,   DELI_NO
                            ,   TRIM(DELI_SHIWAKE_CD) AS DELI_SHIWAKE_CD
                            ,   UNIT_CNT
                            ,   SHIP_INSTRUCT_ID
                        FROM
                                T_SHIP_PACKING_INFO
                        WHERE
                                EC_FLAG = 1
                            AND SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                            AND ROWNUM = 1
                )

            ");

            if (condition.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                    , SELECTED_APACKING_DATA AS(
                        SELECT
                                CENTER_ID
                            ,   SHIPPER_ID
                            ,   DELI_NO
                            ,   TRIM(DELI_SHIWAKE_CD) AS DELI_SHIWAKE_CD
                            ,   UNIT_CNT
                            ,   SHIP_INSTRUCT_ID
                        FROM
                                A_SHIP_PACKING_INFO
                        WHERE
                                EC_FLAG = 1
                            AND SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
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
                ,SELECTED_YAMATO_DATA AS (
                        SELECT
                                CENTER_ID
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   DELI_NO
                            ,   BOX_SIZE
                            ,   UNIT_SEQ
                            ,   UNIT_CNT
                        FROM
                                T_SHIP_DELI_YAMATO
                        WHERE
                                EC_FLAG          = 1
                            AND SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND CENTER_ID        = :CENTER_ID
                            AND SHIPPER_ID       = :SHIPPER_ID

                )
            ");
            if (condition.ChkOldData == true)
            {
                query.AppendLine(@"
                    , SELECTED_AYAMATO_DATA AS(
                            SELECT
                                CENTER_ID
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   DELI_NO
                            ,   BOX_SIZE
                            ,   UNIT_SEQ
                            ,   UNIT_CNT
                            FROM
                                    A_SHIP_DELI_YAMATO
                            WHERE
                                    EC_FLAG          = 1
                                AND SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                                AND CENTER_ID        = :CENTER_ID
                                AND SHIPPER_ID       = :SHIPPER_ID
                    )
                    , SELECTED_ALL_YAMATO_DATA AS(
                            SELECT * FROM SELECTED_YAMATO_DATA
                            UNION
                            SELECT * FROM SELECTED_AYAMATO_DATA
                    )
                ");
            }

            query.AppendLine(@"
                ,   SELECTED_ECSHIP_DATA AS (
                        SELECT
                                CENTER_ID 
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   ARRIVE_REQUEST_DATE
                            ,   ARRIVE_REQUEST_TIME
                            ,   DEST_ZIP
                            ,   DEST_TEL
                            ,   DEST_PREF_NAME
                            ,   DEST_CITY_NAME
                            ,   DEST_ADDRESS1
                            ,   DEST_ADDRESS2
                            ,   DEST_FAMILY_NAME
                            ,   DEST_FIRST_NAME
                            ,   DAIBIKI_FLAG
                            ,   TOTAL_CLAIM_AMOUNT
                            ,   DELI_SHIWAKE_CD
                         FROM
                                T_ECSHIPS
                        WHERE
                                SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND CENTER_ID        = :CENTER_ID
                            AND SHIPPER_ID       = :SHIPPER_ID
                            AND ROWNUM           = 1
                )

           ");

            if (condition.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                    , SELECTED_AECSHIP_DATA AS(
                        SELECT
                                CENTER_ID 
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   ARRIVE_REQUEST_DATE
                            ,   ARRIVE_REQUEST_TIME
                            ,   DEST_ZIP
                            ,   DEST_TEL
                            ,   DEST_PREF_NAME
                            ,   DEST_CITY_NAME
                            ,   DEST_ADDRESS1
                            ,   DEST_ADDRESS2
                            ,   DEST_FAMILY_NAME
                            ,   DEST_FIRST_NAME
                            ,   DAIBIKI_FLAG
                            ,   TOTAL_CLAIM_AMOUNT
                            ,   DELI_SHIWAKE_CD
                        FROM
                                A_ECSHIPS
                        WHERE
                                SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                            AND ROWNUM = 1
                    )
                    , SELECTED_ALL_ECSHIPS_DATA AS(
                            SELECT * FROM SELECTED_ECSHIP_DATA
                            UNION
                            SELECT * FROM SELECTED_AECSHIP_DATA
                    )
                ");
            }
            query.AppendLine(@"
                SELECT
                        '0' || PACK.DELI_SHIWAKE_CD || '0'                   AS DELI_SHIWAKE_BARCODE
                    ,   PACK.DELI_SHIWAKE_CD                                 AS DELI_SHIWAKE_CD
                    ,   ECSHIP.DEST_ZIP                                        AS STORE_ZIP
                    ,   ECSHIP.DEST_TEL                                        AS STORE_TEL
                    ,   DELIYAMATO.DELI_NO                                     AS DELI_NO
                    ,   TO_CHAR(SYSDATE, 'YYMMDD')                             AS PRINT_DATE
                    ,   SUBSTRB(TO_CHAR(ECSHIP.ARRIVE_REQUEST_DATE,'YYYYMMDD'),5,2) || '   ' || SUBSTRB(TO_CHAR(ECSHIP.ARRIVE_REQUEST_DATE,'YYYYMMDD'),7,2)  AS ARRIVE_REQUEST_DATE
                    ,   CASE WHEN TRIM(ECSHIP.ARRIVE_REQUEST_TIME) IS NULL THEN N'指定なし' WHEN TRIM(ECSHIP.ARRIVE_REQUEST_TIME) = N'00' THEN N'指定なし' ELSE  GEN_TIME.GEN_NAME END   AS ARRIVE_REQUEST_TIME
                    ,   SUBSTR(SF_GET_FULL_WIDTH_STRING(ECSHIP.DEST_PREF_NAME || ECSHIP.DEST_CITY_NAME), 1, 18)  AS DEST_ADDRESS1
                    ,   SUBSTR(SUBSTR(SF_GET_FULL_WIDTH_STRING(ECSHIP.DEST_PREF_NAME || ECSHIP.DEST_CITY_NAME), 19) || SF_GET_FULL_WIDTH_STRING(ECSHIP.DEST_ADDRESS1 || ECSHIP.DEST_ADDRESS2 ), 1, 18)  AS DEST_ADDRESS2
                    ,   SUBSTR(SUBSTR(SF_GET_FULL_WIDTH_STRING(ECSHIP.DEST_PREF_NAME || ECSHIP.DEST_CITY_NAME), 19) || SF_GET_FULL_WIDTH_STRING(ECSHIP.DEST_ADDRESS1 || ECSHIP.DEST_ADDRESS2 ), 19, 18) AS DEST_ADDRESS3
                    ,   ECSHIP.DEST_FAMILY_NAME || ECSHIP.DEST_FIRST_NAME                                                                                                                               AS DEST_NAME
                    ,   SUBSTR(SUBSTR(SF_GET_FULL_WIDTH_STRING(ECSHIP.DEST_PREF_NAME || ECSHIP.DEST_CITY_NAME), 19) || SF_GET_FULL_WIDTH_STRING(ECSHIP.DEST_ADDRESS1 || ECSHIP.DEST_ADDRESS2 ), 37, 18) AS DEST_BUMON1
                    ,   SUBSTR(SUBSTR(SF_GET_FULL_WIDTH_STRING(ECSHIP.DEST_PREF_NAME || ECSHIP.DEST_CITY_NAME), 19) || SF_GET_FULL_WIDTH_STRING(ECSHIP.DEST_ADDRESS1 || ECSHIP.DEST_ADDRESS2 ), 55, 18) AS DEST_BUMON2
                    ,   CONS.CONSIGNOR_ZIP                                     AS CONSIGNOR_ZIP
                    ,   CONS.CONSIGNOR_TEL                                     AS CONSIGNOR_TEL
                    ,   CONS.CONSIGNOR_ADDRESS1                                AS CONSIGNOR_ADDRESS1
                    ,   CONS.CONSIGNOR_ADDRESS2                                AS CONSIGNOR_ADDRESS2
                    ,   ''                                                     AS CONSIGNOR_ADDRESS3
                    ,   CONS.CONSIGNOR_NAME                                    AS CONSIGNOR_NAME
                    ,   '衣類品等'                                             AS ITEM_NAME1
                    ,   ''                                                     AS ITEM_NAME2
                    ,   DELIYAMATO.UNIT_SEQ                                    AS UNIT
                    ,   DELIYAMATO.UNIT_CNT                                    AS ALL_UNIT
                    ,   '水濡厳禁'                                             AS HANDLING1
                    ,   ''                                                     AS HANDLING2
                    ,   ''                                                     AS ARTICLE
                    ,   GEN.GEN_NAME                                           AS BOX_SIZE
                    ,   CONS.SHIP_STORE_CD                                     AS SHIP_STORE_CD
                    ,   TO_CHAR(SYSDATE + 30, 'YYYY""年""MM""月""DD""日迄""')  AS DELI_USE_DATE
                    ,   CONS.DELIVERY_COMPANY_TEL                              AS DELIVERY_COMPANY_TEL
                    ,   DELIYAMATO.DELI_NO                                     AS DELI_NO_BARCODE
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

            ");

            if (condition.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                        SELECTED_ALL_ECSHIPS_DATA ECSHIP
                ");
            }
            else
            {
                query.AppendLine(@"
                        SELECTED_ECSHIP_DATA ECSHIP 
                ");
            }

            query.AppendLine(@"
                ON
                        PACK.SHIP_INSTRUCT_ID = ECSHIP.SHIP_INSTRUCT_ID
                    AND PACK.CENTER_ID        = ECSHIP.CENTER_ID
                    AND PACK.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                ");

            query.AppendLine(@"
                          INNER JOIN
            ");
            if (condition.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                        SELECTED_ALL_YAMATO_DATA  DELIYAMATO
                ");
            }
            else
            {
                query.AppendLine(@"
                        SELECTED_YAMATO_DATA      DELIYAMATO 
                ");
            }
            query.AppendLine(@"
                ON
                        DELIYAMATO.SHIP_INSTRUCT_ID = ECSHIP.SHIP_INSTRUCT_ID
                    AND DELIYAMATO.CENTER_ID        = ECSHIP.CENTER_ID
                    AND DELIYAMATO.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                INNER JOIN
                        M_CONSIGNORS CONS
                ON
                        CONS.CENTER_ID         = ECSHIP.CENTER_ID
                    AND CONS.CONSIGNOR_TYPE_ID = '07'
                    AND CONS.SHIPPER_ID        = ECSHIP.SHIPPER_ID
                LEFT OUTER JOIN
                        M_GENERALS GEN
                ON
                        GEN.REGISTER_DIVI_CD = '1'
                    AND GEN.GEN_DIV_CD       = 'YAMATO_BOX_SIZE'
                    AND GEN.GEN_CD           = DELIYAMATO.BOX_SIZE
                    AND GEN.CENTER_ID        = '@@@'
                    AND GEN.SHIPPER_ID       = DELIYAMATO.SHIPPER_ID
               LEFT OUTER JOIN
                        M_GENERALS GEN_TIME
                ON
                        GEN_TIME.REGISTER_DIVI_CD = '1'
                    AND GEN_TIME.GEN_DIV_CD = 'YAMATO_ARRIVE_REQUEST_TIME'
                    AND GEN_TIME.GEN_CD = ECSHIP.ARRIVE_REQUEST_TIME
                    AND GEN_TIME.CENTER_ID = '@@@'
                    AND GEN_TIME.SHIPPER_ID = ECSHIP.SHIPPER_ID
                ORDER BY  DELIYAMATO.UNIT_SEQ
            ");
            parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            return MvcDbContext.Current.Database.Connection.Query<ViewModels.PrintInvoice.PrintInvoiceYamato>(query.ToString(), parameters);
        }

        /// <summary>
        /// ネコポス送り状CSVデータ取得EC
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<ViewModels.PrintInvoice.PrintInvoiceNekoposu> GetPrintInvoiceNekoposu(PrintEcInvoiceConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
                WITH
                    SELECTED_PACKING_DATA AS (
                        SELECT
                                CENTER_ID
                            ,   SHIPPER_ID
                            ,   DELI_NO
                            ,   TRIM(DELI_SHIWAKE_CD) AS DELI_SHIWAKE_CD
                            ,   UNIT_CNT
                            ,   SHIP_INSTRUCT_ID
                        FROM
                                T_SHIP_PACKING_INFO
                        WHERE
                                EC_FLAG = 1
                            AND SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                            AND ROWNUM = 1
                )

            ");

            if (condition.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                    , SELECTED_APACKING_DATA AS(
                        SELECT
                                CENTER_ID
                            ,   SHIPPER_ID
                            ,   DELI_NO
                            ,   TRIM(DELI_SHIWAKE_CD) AS DELI_SHIWAKE_CD
                            ,   UNIT_CNT
                            ,   SHIP_INSTRUCT_ID
                        FROM
                                A_SHIP_PACKING_INFO
                        WHERE
                                EC_FLAG = 1
                            AND SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
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
                ,  SELECTED_YAMATO_DATA AS (
                        SELECT
                                CENTER_ID
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   DELI_NO
                            ,   BOX_SIZE
                            ,   UNIT_SEQ
                            ,   UNIT_CNT
                        FROM
                                T_SHIP_DELI_YAMATO
                        WHERE
                                EC_FLAG          = 1
                            AND SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND CENTER_ID        = :CENTER_ID
                            AND SHIPPER_ID       = :SHIPPER_ID

                )
            ");
            if (condition.ChkOldData == true)
            {
                query.AppendLine(@"
                    , SELECTED_AYAMATO_DATA AS(
                            SELECT
                                CENTER_ID
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   DELI_NO
                            ,   BOX_SIZE
                            ,   UNIT_SEQ
                            ,   UNIT_CNT
                            FROM
                                    A_SHIP_DELI_YAMATO
                            WHERE
                                    EC_FLAG          = 1
                                AND SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                                AND CENTER_ID        = :CENTER_ID
                                AND SHIPPER_ID       = :SHIPPER_ID
                    )
                    , SELECTED_ALL_YAMATO_DATA AS(
                            SELECT * FROM SELECTED_YAMATO_DATA
                            UNION
                            SELECT * FROM SELECTED_AYAMATO_DATA
                    )
                ");
            }

            query.AppendLine(@"
                ,   SELECTED_ECSHIP_DATA AS (
                        SELECT
                                CENTER_ID 
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   ARRIVE_REQUEST_DATE
                            ,   ARRIVE_REQUEST_TIME
                            ,   DEST_ZIP
                            ,   DEST_TEL
                            ,   DEST_PREF_NAME
                            ,   DEST_CITY_NAME
                            ,   DEST_ADDRESS1
                            ,   DEST_ADDRESS2
                            ,   DEST_FAMILY_NAME
                            ,   DEST_FIRST_NAME
                            ,   DAIBIKI_FLAG
                            ,   TOTAL_CLAIM_AMOUNT
                            ,   DELI_SHIWAKE_CD
                         FROM
                                T_ECSHIPS
                        WHERE
                                SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND CENTER_ID        = :CENTER_ID
                            AND SHIPPER_ID       = :SHIPPER_ID
                            AND ROWNUM           = 1
                )

           ");

            if (condition.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                    , SELECTED_AECSHIP_DATA AS(
                        SELECT
                                CENTER_ID 
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   ARRIVE_REQUEST_DATE
                            ,   ARRIVE_REQUEST_TIME
                            ,   DEST_ZIP
                            ,   DEST_TEL
                            ,   DEST_PREF_NAME
                            ,   DEST_CITY_NAME
                            ,   DEST_ADDRESS1
                            ,   DEST_ADDRESS2
                            ,   DEST_FAMILY_NAME
                            ,   DEST_FIRST_NAME
                            ,   DAIBIKI_FLAG
                            ,   TOTAL_CLAIM_AMOUNT
                            ,   DELI_SHIWAKE_CD
                        FROM
                                A_ECSHIPS
                        WHERE
                                SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                            AND ROWNUM = 1
                    )
                    , SELECTED_ALL_ECSHIPS_DATA AS(
                            SELECT * FROM SELECTED_ECSHIP_DATA
                            UNION
                            SELECT * FROM SELECTED_AECSHIP_DATA
                    )
                ");
            }
            query.AppendLine(@"
                SELECT
                        '0' || PACK.DELI_SHIWAKE_CD || '0'                     AS DELI_SHIWAKE_BARCODE
                    ,   PACK.DELI_SHIWAKE_CD                                   AS DELI_SHIWAKE_CD
                    ,   ECSHIP.DEST_ZIP                                        AS DEST_ZIP
                    ,   ECSHIP.DEST_PREF_NAME || ECSHIP.DEST_CITY_NAME         AS DEST_ADDRESS1
                    ,   ECSHIP.DEST_ADDRESS1                                   AS DEST_ADDRESS2
                    ,   ECSHIP.DEST_ADDRESS2                                   AS DEST_ADDRESS3
                    ,   ''                                                     AS DEST_BUMON1
                    ,   ''                                                     AS DEST_BUMON2
                    ,   ECSHIP.DEST_FAMILY_NAME || ECSHIP.DEST_FIRST_NAME      AS DEST_NAME
                    ,   DELIYAMATO.DELI_NO                                     AS DELI_NO
                    ,   TO_CHAR(SYSDATE, 'YYYYMMDD')                           AS PRINT_DATE
                    ,   CONS.CONSIGNOR_ZIP                                     AS CONSIGNOR_ZIP
                    ,   CONS.CONSIGNOR_ADDRESS1                                AS CONSIGNOR_ADDRESS1
                    ,   CONS.CONSIGNOR_ADDRESS2                                AS CONSIGNOR_ADDRESS2
                    ,   ''                                                     AS CONSIGNOR_ADDRESS3
                    ,   CONS.CONSIGNOR_NAME                                    AS CONSIGNOR_NAME
                    ,   '衣類品等'                                             AS ITEM_NAME1
                    ,   ''                                                     AS ITEM_NAME2
                    ,   '水濡厳禁'                                             AS HANDLING1
                    ,   ''                                                     AS HANDLING2
                    ,   ''                                                     AS ARTICLE
                    ,   CONS.SHIP_STORE_CD                                     AS SHIP_STORE_CD
                    ,   TO_CHAR(SYSDATE + 30, 'YYYY""年""MM""月""DD""日迄""')  AS DELI_USE_DATE
                    ,   CONS.DELIVERY_COMPANY_TEL                              AS DELIVERY_COMPANY_TEL
                    ,   DELIYAMATO.DELI_NO                                     AS DELI_NO_BARCODE
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

            ");
            if (condition.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                        SELECTED_ALL_ECSHIPS_DATA ECSHIP
                ");
            }
            else
            {
                query.AppendLine(@"
                        SELECTED_ECSHIP_DATA ECSHIP 
                ");
            }

            query.AppendLine(@"
                ON
                        PACK.SHIP_INSTRUCT_ID = ECSHIP.SHIP_INSTRUCT_ID
                    AND PACK.CENTER_ID        = ECSHIP.CENTER_ID
                    AND PACK.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                ");
            query.AppendLine(@"
                INNER JOIN
            ");
            if (condition.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                        SELECTED_ALL_YAMATO_DATA  DELIYAMATO
                ");
            }
            else
            {
                query.AppendLine(@"
                        SELECTED_YAMATO_DATA      DELIYAMATO 
                ");
            }
            query.AppendLine(@"
                ON
                        DELIYAMATO.SHIP_INSTRUCT_ID = ECSHIP.SHIP_INSTRUCT_ID
                    AND DELIYAMATO.CENTER_ID        = ECSHIP.CENTER_ID
                    AND DELIYAMATO.SHIPPER_ID       = ECSHIP.SHIPPER_ID
                INNER JOIN
                        M_CONSIGNORS CONS
                ON
                        CONS.CENTER_ID         = ECSHIP.CENTER_ID
                    AND CONS.CONSIGNOR_TYPE_ID = '06'
                    AND CONS.SHIPPER_ID        = ECSHIP.SHIPPER_ID
                ORDER BY  DELIYAMATO.UNIT_SEQ
            ");
            parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            return MvcDbContext.Current.Database.Connection.Query<ViewModels.PrintInvoice.PrintInvoiceNekoposu>(query.ToString(), parameters);
        }

    }
}