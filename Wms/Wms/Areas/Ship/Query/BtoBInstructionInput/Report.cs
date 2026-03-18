namespace Wms.Areas.Ship.Query.BtoBInstructionInput
{
    using System.Collections.Generic;
    using System.Text;
    using Dapper;
    using Wms.Areas.Ship.ViewModels.BtoBInstructionInput;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.BtoBInstructionInput.BtoBInstructionInput01SearchConditions;

    public class Report : BaseQuery
    {
        /// <summary>
        /// BtoB出荷実績入力に出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、引当エラー(商品別)のデータを作る。</returns>
        public IEnumerable<BtoBInstructionInputReport> BtoBInstructionInputListing(BtoBInstructionInput01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT *
                  FROM WW_SHP_BTO_B_RESULT_INPUT
                 WHERE SHIPPER_ID = :SHIPPER_ID
                   AND SEQ = :SEQ ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);
            // Sort function
            switch (condition.SortKey)
            {
                case BtoBInstructionInput01SortKey.SkuInstructId:
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
            return MvcDbContext.Current.Database.Connection.Query<BtoBInstructionInputReport>(query.ToString(), parameters);
        }

        /// <summary>
        /// BtoB出荷指示照会(明細別)に出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、引当エラー(商品別)のデータを作る。</returns>
        public IEnumerable<BtoBInstructionInputDetailReport> BtoBInstructionInputDetailListing(BtoBInstructionInput01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT *
                  FROM WW_SHP_BTO_B_RESULT_INPUT
                 WHERE SHIPPER_ID = :SHIPPER_ID
                   AND SEQ = :SEQ ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // Sort function
            switch (condition.DetailSortKey)
            {
                case BtoBInstructionInputDetailSortKey.ShipToStoreInstructIdSeq:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY SHIP_TO_STORE_ID DESC,SHIP_INSTRUCT_ID DESC,SHIP_INSTRUCT_SEQ DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY SHIP_TO_STORE_ID ASC,SHIP_INSTRUCT_ID ASC,SHIP_INSTRUCT_SEQ ASC ");
                            break;
                    }

                    break;

                case BtoBInstructionInputDetailSortKey.SkuInstructIdSeq:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY ITEM_SKU_ID DESC,SHIP_INSTRUCT_ID DESC,SHIP_INSTRUCT_SEQ DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY ITEM_SKU_ID ASC,SHIP_INSTRUCT_ID ASC,SHIP_INSTRUCT_SEQ ASC ");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY SHIP_PLAN_DATE DESC,SHIP_INSTRUCT_ID DESC,SHIP_INSTRUCT_SEQ DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY SHIP_PLAN_DATE ASC,SHIP_INSTRUCT_ID ASC,SHIP_INSTRUCT_SEQ ASC ");
                            break;
                    }

                    break;
            }

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<BtoBInstructionInputDetailReport>(query.ToString(), parameters);
        }
    }
}