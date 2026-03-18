namespace Wms.Areas.Ship.Query.BtoBInstructionReference
{
    using System.Collections.Generic;
    using System.Text;
    using Dapper;
    using Wms.Areas.Ship.ViewModels.BtoBInstructionReference;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.BtoBInstructionReference.BtoBInstructionReference01SearchConditions;

    public class Report : BaseQuery
    {
        /// <summary>
        /// BtoB出荷指示照会に出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、引当エラー(商品別)のデータを作る。</returns>
        public IEnumerable<BtoBInstructionReferenceReport> BtoBInstructionReferenceListing(BtoBInstructionReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
                StringBuilder query = new StringBuilder(@"
                SELECT *
                  FROM WW_SHP_BTO_B_INSTRUCTION_REFERENCE
                 WHERE SHIPPER_ID = :SHIPPER_ID
                   AND SEQ = :SEQ ");
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":SEQ", condition.Seq);
                // Sort function
                switch (condition.SortKey)
                {
                    case BtoBInstructionReference01SortKey.SkuInstructId:
                        switch (condition.Sort)
                        {
                            case AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY ITEM_SKU_ID DESC,SHIP_INSTRUCT_ID DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY ITEM_SKU_ID ASC,SHIP_INSTRUCT_ID ASC ");
                                break;
                        }

                        break;

                    default:
                        switch (condition.Sort)
                        {
                            case AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY SHIP_PLAN_DATE DESC,SHIP_INSTRUCT_ID DESC,ITEM_SKU_ID DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY SHIP_PLAN_DATE ASC,SHIP_INSTRUCT_ID ASC,ITEM_SKU_ID ASC ");
                                break;
                        }

                        break;
                }

                // Fill data to memory
                return MvcDbContext.Current.Database.Connection.Query<BtoBInstructionReferenceReport>(query.ToString(), parameters);
        }

        /// <summary>
        /// BtoB出荷指示照会(明細別)に出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、引当エラー(商品別)のデータを作る。</returns>
        public IEnumerable<BtoBInstructionReferenceDetailReport> BtoBInstructionReferenceDetailListing(BtoBInstructionReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            if (condition.ShipAllocStatusOld)
            {
                query.AppendLine(@"
                    WITH
                        SELECTED_SHIPS AS (
                            SELECT
                                    SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   CENTER_ID
                                ,   SHIPPER_ID
                                ,   COMPLETE_DATE
                                ,   SLIP_DATE
                                ,   SALES_CLASS
                                ,   OFF_RATE
                            FROM
                                    T_SHIPS
                            WHERE
                                    SHIPPER_ID = :SHIPPER_ID
                            UNION
                            SELECT
                                    SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   CENTER_ID
                                ,   SHIPPER_ID
                                ,   COMPLETE_DATE
                                ,   SLIP_DATE
                                ,   SALES_CLASS
                                ,   OFF_RATE
                            FROM
                                    A_SHIPS
                            WHERE
                                    SHIPPER_ID = :SHIPPER_ID
                    )
                ");
            } else {
                query.AppendLine(@"
                    WITH
                        SELECTED_SHIPS AS (
                            SELECT
                                    SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   CENTER_ID
                                ,   SHIPPER_ID
                                ,   COMPLETE_DATE
                                ,   SLIP_DATE
                                ,   SALES_CLASS
                                ,   OFF_RATE
                            FROM
                                    T_SHIPS
                            WHERE
                                    SHIPPER_ID = :SHIPPER_ID
                    )
                ");
            }
            query.AppendLine(@"
                SELECT DISTINCT
                        WW.*
                    ,   CTGR.CATEGORY_NAME1 AS CATEGORY_NAME
                    ,   TO_CHAR(SHIP.COMPLETE_DATE, 'YYYY/MM/DD') AS COMPLETE_DATE
                    ,   SHIP.SALES_CLASS
                    ,   SHIP.OFF_RATE
                    ,   TO_CHAR(SHIP.SLIP_DATE,'YYYY/MM/DD') AS SLIP_DATE
                FROM
                        WW_SHP_BTO_B_INSTRUCTION_REFERENCE WW
                LEFT OUTER JOIN
                        M_ITEM_SKU ITEM
                ON
                        ITEM.ITEM_SKU_ID = WW.ITEM_SKU_ID
                    AND ITEM.SHIPPER_ID = WW.SHIPPER_ID
                LEFT OUTER JOIN
                        M_ITEM_CATEGORIES4 CTGR
                ON
                        CTGR.CATEGORY_ID1 = ITEM.CATEGORY_ID1
                    AND CTGR.SHIPPER_ID = ITEM.SHIPPER_ID
                LEFT OUTER JOIN
                        SELECTED_SHIPS SHIP
                ON
                        SHIP.SHIP_INSTRUCT_ID = WW.SHIP_INSTRUCT_ID
                    AND SHIP.SHIP_INSTRUCT_SEQ = WW.SHIP_INSTRUCT_SEQ
                    AND SHIP.CENTER_ID = WW.CENTER_ID
                    AND SHIP.SHIPPER_ID = WW.SHIPPER_ID
                WHERE
                        WW.SHIPPER_ID = :SHIPPER_ID
                    AND WW.SEQ = :SEQ
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // Sort function
            switch (condition.DetailSortKey)
            {
                case BtoBInstructionReferenceDetailSortKey.ShipToStoreInstructIdSeq:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WW.SHIP_TO_STORE_ID DESC, WW.SHIP_INSTRUCT_ID DESC, WW.SHIP_INSTRUCT_SEQ DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WW.SHIP_TO_STORE_ID ASC, WW.SHIP_INSTRUCT_ID ASC, WW.SHIP_INSTRUCT_SEQ ASC ");
                            break;
                    }

                    break;

                case BtoBInstructionReferenceDetailSortKey.SkuInstructIdSeq:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WW.ITEM_SKU_ID DESC, WW.SHIP_INSTRUCT_ID DESC, WW.SHIP_INSTRUCT_SEQ DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WW.ITEM_SKU_ID ASC, WW.SHIP_INSTRUCT_ID ASC, WW.SHIP_INSTRUCT_SEQ ASC ");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WW.SHIP_PLAN_DATE DESC, WW.SHIP_INSTRUCT_ID DESC, WW.SHIP_INSTRUCT_SEQ DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WW.SHIP_PLAN_DATE ASC, WW.SHIP_INSTRUCT_ID ASC, WW.SHIP_INSTRUCT_SEQ ASC ");
                            break;
                    }

                    break;
            }

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<BtoBInstructionReferenceDetailReport>(query.ToString(), parameters);
        }
    }
}