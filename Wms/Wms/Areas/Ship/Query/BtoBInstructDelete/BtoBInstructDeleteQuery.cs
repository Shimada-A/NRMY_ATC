namespace Wms.Areas.Ship.Query.BtoBInstructDelete
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
    using Wms.Areas.Ship.ViewModels.BtoBInstructDelete;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.BtoBInstructDelete.BtoBInsDelDetailSearchConditions;
    using static Wms.Areas.Ship.ViewModels.BtoBInstructDelete.BtoBInstructDeleteSearchConditions;

    public class BtoBInstructDeleteQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertShpBtoBInstructDelete(BtoBInstructDeleteSearchConditions condition)
        {
            // ワーク登録(出荷指示一覧)
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query;

                    // 1.ワークID採番
                    condition.Seq = new BaseQuery().GetWorkId();
                    condition.Page = 1;

                    query = new StringBuilder(@" 
                        INSERT INTO WW_SHP_DEL_INSTRUCTION(
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
                            ,   EMERGENCY_CLASS_NAME
                            ,   SHIP_INSTRUCT_ID
                            ,   ITEM_SKU_QTY
                            ,   SHIP_TO_QTY
                            ,   INSTRUCT_QTY
                            ,   ALLOC_DATE
                            ,   INSTRUCT_CLASS
                        )
                        WITH
                            TARGET_SHIPS AS (
                                SELECT
                                        TS.SHIP_INSTRUCT_ID
                                    ,   TS.CENTER_ID
                                    ,   TS.SHIPPER_ID
                                FROM
                                        T_SHIPS TS
                                LEFT OUTER JOIN
                                        M_ITEM_SKU MIS
                                ON
                                        TS.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                                    AND TS.SHIPPER_ID = MIS.SHIPPER_ID
                                LEFT JOIN
                                        M_COLORS MC
                                ON
                                        TS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                                    AND TS.SHIPPER_ID   = MC.SHIPPER_ID
                                LEFT JOIN
                                        M_SIZES MS
                                ON
                                        TS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                                    AND TS.SHIPPER_ID   = MS.SHIPPER_ID
                                LEFT JOIN
                                        M_BRANDS MB
                                ON
                                        MIS.BRAND_ID = MB.BRAND_ID
                                    AND MIS.SHIPPER_ID = MB.SHIPPER_ID
                                LEFT JOIN
                                        M_VENDORS MV
                                ON
                                        MIS.MAIN_VENDOR_ID = MV.VENDOR_ID
                                    AND MIS.SHIPPER_ID   = MV.SHIPPER_ID
                                WHERE
                                        TS.SHIP_KIND IN (1, 2)
                                    AND TS.SHIPPER_ID = :SHIPPER_ID
                                    AND TS.INSTRUCT_CLASS = :INSTRUCT_CLASS
                    ");

                    // Add search condition
                    // センター
                    if (!string.IsNullOrEmpty(condition.CenterId))
                    {
                        query.Append(" AND TS.CENTER_ID = :CENTER_ID ");
                        parameters.Add(":CENTER_ID", condition.CenterId);
                    }

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
                        query.Append(" AND TS.TRANSPORTER_ID IN :TRANSPORTER_ID ");
                        parameters.Add(":TRANSPORTER_ID", transporterIds);
                    }

                    // 配送エリア
                    if (!string.IsNullOrEmpty(condition.TransporterArea))
                    {
                        string[] transporterAreas = (condition.TransporterArea ?? condition.TransporterArea).Split(',');
                        query.Append(" AND TS.PREF_ID IN :PREF_ID ");
                        parameters.Add(":PREF_ID", transporterAreas);
                    }

                    // 事業部
                    if (!string.IsNullOrEmpty(condition.DivisionId))
                    {
                        query.Append(" AND MIS.DIVISION_ID = :DIVISION_ID ");
                        parameters.Add(":DIVISION_ID", condition.DivisionId);
                    }

                    // ブランド
                    if (!string.IsNullOrEmpty(condition.BrandId))
                    {
                        query.Append(" AND MIS.BRAND_ID LIKE :BRAND_ID ");
                        parameters.Add(":BRAND_ID", condition.BrandId + "%");
                    }

                    // ブランド名
                    if (string.IsNullOrEmpty(condition.BrandId) && !string.IsNullOrEmpty(condition.BrandName))
                    {
                        query.Append(" AND MB.BRAND_NAME LIKE :BRAND_NAME ");
                        parameters.Add(":BRAND_NAME", condition.BrandName + "%");
                    }

                    // 代表仕入先
                    if (!string.IsNullOrEmpty(condition.MainVendorId))
                    {
                        query.Append(" AND MIS.MAIN_VENDOR_ID LIKE :MAIN_VENDOR_ID ");
                        parameters.Add(":MAIN_VENDOR_ID", condition.MainVendorId + "%");
                    }

                    // 代表仕入先名
                    if (string.IsNullOrEmpty(condition.MainVendorId) && !string.IsNullOrEmpty(condition.MainVendorName))
                    {
                        query.Append(" AND MV.VENDOR_NAME1 LIKE :VENDOR_NAME1 ");
                        parameters.Add(":VENDOR_NAME1", condition.MainVendorName + "%");
                    }

                    // 分類
                    if (!string.IsNullOrEmpty(condition.CategoryId1))
                    {
                        query.Append(" AND MIS.CATEGORY_ID1 = :CATEGORY_ID1 ");
                        parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId2))
                    {
                        query.Append(" AND MIS.CATEGORY_ID2 = :CATEGORY_ID2 ");
                        parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId3))
                    {
                        query.Append(" AND MIS.CATEGORY_ID3 = :CATEGORY_ID3 ");
                        parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId4))
                    {
                        query.Append(" AND MIS.CATEGORY_ID4 = :CATEGORY_ID4 ");
                        parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
                    }

                    // アイテムコード
                    if (!string.IsNullOrEmpty(condition.ItemCode))
                    {
                        query.Append(" AND MIS.ITEM_CODE = :ITEM_CODE ");
                        parameters.Add(":ITEM_CODE", condition.ItemCode);
                    }

                    // 品番
                    if (!string.IsNullOrEmpty(condition.ItemId))
                    {
                        query.Append(" AND TS.ITEM_ID LIKE :ITEM_ID ");
                        parameters.Add(":ITEM_ID", condition.ItemId + "%");
                    }

                    // JAN
                    if (!string.IsNullOrEmpty(condition.Jan))
                    {
                        query.Append(" AND TS.JAN LIKE :JAN ");
                        parameters.Add(":JAN", condition.Jan + "%");
                    }

                    // SKU
                    if (!string.IsNullOrEmpty(condition.ItemSkuId))
                    {
                        query.Append(" AND TS.ITEM_SKU_ID LIKE :ITEM_SKU_ID ");
                        parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId + "%");
                    }

                    // 出荷指示ID
                    if (!string.IsNullOrEmpty(condition.ShipInstructId))
                    {
                        query.Append(" AND TS.SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID ");
                        parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId);
                    }

                    // 緊急
                    if (!string.IsNullOrEmpty(condition.EmergencyClass))
                    {
                        query.Append(" AND TS.EMERGENCY_CLASS = :EMERGENCY_CLASS ");
                        parameters.Add(":EMERGENCY_CLASS", condition.EmergencyClass);
                    }
                    query.Append(@"
                                GROUP BY
                                        TS.SHIP_INSTRUCT_ID
                                    ,   TS.CENTER_ID
                                    ,   TS.SHIPPER_ID
                        )
                        SELECT
                                SYSTIMESTAMP
                            ,   :IN_USER_ID
                            ,   'BtoBInstructDelete'
                            ,   SYSTIMESTAMP
                            ,   :IN_USER_ID
                            ,   'BtoBInstructDelete'
                            ,   0
                            ,   :SHIPPER_ID
                            ,   :SEQ
                            ,   ROW_NUMBER() OVER(ORDER BY TS.SHIP_INSTRUCT_ID ASC)
                            ,   0
                            ,   TS.CENTER_ID
                            ,   MAX(TS.SHIP_PLAN_DATE) SHIP_PLAN_DATE
                            ,   MAX(MG.GEN_NAME) EMERGENCY_CLASS_NAME
                            ,   TS.SHIP_INSTRUCT_ID
                            ,   COUNT(DISTINCT TS.ITEM_SKU_ID) AS ITEM_SKU_QTY
                            ,   COUNT(DISTINCT TS.SHIP_TO_STORE_ID) AS SHIP_TO_QTY
                            ,   SUM(TS.INSTRUCT_QTY) AS INSTRUCT_QTY
                            ,   MAX(TS.MAKE_DATE) AS ALLOC_DATE
                            ,   MAX(TS.INSTRUCT_CLASS) AS INSTRUCT_CLASS
                        FROM
                                T_SHIPS TS
                        INNER JOIN
                                TARGET_SHIPS TGT
                        ON
                                TGT.SHIP_INSTRUCT_ID = TS.SHIP_INSTRUCT_ID
                            AND TGT.CENTER_ID = TS.CENTER_ID
                            AND TGT.SHIPPER_ID = TS.SHIPPER_ID
                        LEFT JOIN
                                M_GENERALS MG
                        ON
                                TS.SHIPPER_ID = MG.SHIPPER_ID
                            AND MG.REGISTER_DIVI_CD = '1'
                            AND MG.CENTER_ID = '@@@'
                            AND MG.GEN_DIV_CD = 'EMERGENCY_CLASS'
                            AND TO_CHAR(TS.EMERGENCY_CLASS) = MG.GEN_CD
                        WHERE
                                TS.SHIPPER_ID = :SHIPPER_ID
                        GROUP BY
                                TS.SHIP_INSTRUCT_ID
                            ,   TS.CENTER_ID
                            ,   TS.SHIPPER_ID
                        HAVING
                                MAX(TS.ALLOC_FLAG) = 0
                    ");
                    parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                    parameters.Add(":IN_USER_ID", Profile.User.UserId);
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":INSTRUCT_CLASS", (int)condition.InstructClass);

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
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public BtoBInsDelDetailViewModel GetShpBtoBInstructDeleteDeatil(BtoBInsDelDetailSearchConditions condition)
        {
            var vm = new BtoBInsDelDetailViewModel();
            vm.DetailSearchConditions = condition;
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                WITH
                    TARGET_SHIP_DATA AS (
                        SELECT
                                *
                        FROM
                                T_SHIPS
                        WHERE
                                SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                )
                SELECT
                        TS.SHIP_PLAN_DATE
                    ,   MG.GEN_NAME EMERGENCY_CLASS_NAME
                    ,   TS.SHIP_INSTRUCT_ID
                    ,   TS.SHIP_INSTRUCT_SEQ
                    ,   TS.ITEM_ID
                    ,   TS.ITEM_NAME
                    ,   TS.ITEM_COLOR_ID
                    ,   MC.ITEM_COLOR_NAME
                    ,   TS.ITEM_SIZE_ID
                    ,   MIS.ITEM_SIZE_NAME
                    ,   TS.JAN
                    ,   TS.SHIP_TO_STORE_ID
                    ,   MST.SHIP_TO_STORE_NAME1 SHIP_TO_STORE_NAME
                    ,   MG1.GEN_NAME SHIP_TO_STORE_CLASS_NAME
                    ,   MT.TRANSPORTER_NAME 
                    ,   TS.INSTRUCT_QTY
                    ,   TS.ITEM_SKU_ID
                    ,   TS.CENTER_ID
                FROM
                        TARGET_SHIP_DATA TS
                LEFT OUTER JOIN 
                        M_GENERALS MG
                ON
                        TS.SHIPPER_ID = MG.SHIPPER_ID
                    AND MG.REGISTER_DIVI_CD = '1'
                    AND MG.CENTER_ID = '@@@'
                    AND MG.GEN_DIV_CD = 'EMERGENCY_CLASS'
                    AND TO_CHAR(TS.EMERGENCY_CLASS) = MG.GEN_CD
                LEFT OUTER JOIN
                        M_COLORS MC
                ON
                        TS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                    AND TS.SHIPPER_ID   = MC.SHIPPER_ID
                LEFT OUTER JOIN
                        M_ITEM_SKU MIS
                ON
                        TS.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                    AND TS.SHIPPER_ID   = MIS.SHIPPER_ID
                LEFT OUTER JOIN
                        V_SHIP_TO_STORES MST
                ON
                        TS.SHIP_TO_STORE_ID = MST.SHIP_TO_STORE_ID
                    AND TS.SHIPPER_ID   = MST.SHIPPER_ID
                LEFT OUTER JOIN
                        M_GENERALS MG1
                ON
                        TS.SHIPPER_ID = MG1.SHIPPER_ID
                    AND MG1.REGISTER_DIVI_CD = '1'
                    AND MG1.CENTER_ID = '@@@'
                    AND MG1.GEN_DIV_CD = 'STORE_CLASS'
                    AND TS.STORE_CLASS = MG1.GEN_CD
                LEFT OUTER JOIN
                        M_TRANSPORTERS MT
                ON
                        TS.TRANSPORTER_ID = MT.TRANSPORTER_ID
                    AND TS.SHIPPER_ID   = MT.SHIPPER_ID
            ");
            switch (condition.DetailSortKey)
            {
                case BtoBInsDeleteDetailSortKey.SkuInstructIdDetailId:
                    switch (condition.Sort)
                    {
                        case AscDescSortDetail.Desc:
                            query.AppendLine(@" ORDER BY
                                                        TS.ITEM_SKU_ID DESC
                                                    ,   TS.SHIP_INSTRUCT_ID DESC
                                                    ,   TS.SHIP_INSTRUCT_SEQ DESC ");
                            break;

                        default:
                            query.AppendLine(@" ORDER BY
                                                        TS.ITEM_SKU_ID ASC
                                                    ,   TS.SHIP_INSTRUCT_ID ASC
                                                    ,   TS.SHIP_INSTRUCT_SEQ ASC ");
                            break;
                    }
                    break;

                case BtoBInsDeleteDetailSortKey.ShipIdSkuInstructIdDetailId:
                    switch (condition.Sort)
                    {
                        case AscDescSortDetail.Desc:
                            query.AppendLine(@" ORDER BY
                                                        TS.SHIP_TO_STORE_ID DESC
                                                    ,   TS.ITEM_SKU_ID DESC
                                                    ,   TS.SHIP_INSTRUCT_ID DESC
                                                    ,   TS.SHIP_INSTRUCT_SEQ DESC ");
                            break;

                        default:
                            query.AppendLine(@" ORDER BY
                                                        TS.SHIP_TO_STORE_ID ASC
                                                    ,   TS.ITEM_SKU_ID ASC
                                                    ,   TS.SHIP_INSTRUCT_ID ASC
                                                    ,   TS.SHIP_INSTRUCT_SEQ ASC ");
                            break;
                    }
                    break;
                default:
                    switch (condition.Sort)
                    {
                        case AscDescSortDetail.Desc:
                            query.AppendLine(@" ORDER BY
                                                        TS.SHIP_PLAN_DATE DESC
                                                    ,   TS.SHIP_INSTRUCT_ID DESC
                                                    ,   TS.SHIP_INSTRUCT_SEQ DESC ");
                            break;

                        default:
                            query.AppendLine(@" ORDER BY
                                                        TS.SHIP_PLAN_DATE ASC
                                                    ,   TS.SHIP_INSTRUCT_ID ASC
                                                    ,   TS.SHIP_INSTRUCT_SEQ ASC ");
                            break;
                    }

                    break;
            }

            parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            vm.DetailResults = new BtoBInsDelDetailResult
            {
                BtoBInsDelDetails = MvcDbContext.Current.Database.Connection.Query<BtoBInsDelDetailResultRow>(query.ToString(), parameters)
            };
            vm.DetailSearchConditions.InstructSeqSum = vm.DetailResults.BtoBInsDelDetails.Count();
            vm.DetailSearchConditions.InstructQtySum = vm.DetailResults.BtoBInsDelDetails.Select(x => x.InstructQty).Sum();
            vm.DetailSearchConditions.ItemSkuSum = vm.DetailResults.BtoBInsDelDetails.Select(x => x.ItemSkuId).Distinct().Count();

            return vm;
        }

        /// <summary>
        /// Get StockInquiry List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<BtoBInstructDeleteResultRow> GetData(BtoBInstructDeleteSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            // 出荷指示一覧
                StringBuilder query = new StringBuilder(@"
                    SELECT
                            *
                    FROM
                            WW_SHP_DEL_INSTRUCTION
                    WHERE
                            SHIPPER_ID = :SHIPPER_ID
                        AND SEQ = :SEQ
                ");
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":SEQ", condition.Seq);

                // 全レコード数を取得
                int totalCount = MvcDbContext.Current.Database.Connection.Query<BtoBInstructDeleteResultRow>(query.ToString(), parameters).Count();

                // Sort function
                switch (condition.SortKey)
                {
                    case BtoBInstructDeleteSortKey.InstructId:
                        switch (condition.Sort)
                        {
                            case AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY SHIP_INSTRUCT_ID DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY SHIP_INSTRUCT_ID ASC ");
                                break;
                        }

                        break;

                    default:
                        switch (condition.Sort)
                        {
                            case AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY SHIP_PLAN_DATE DESC,SHIP_INSTRUCT_ID DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY SHIP_PLAN_DATE ASC,SHIP_INSTRUCT_ID ASC ");
                                break;
                        }

                        break;
                }

                query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
                parameters.Add(":PAGE_SIZE", condition.PageSize);

                // Choose data corresponding on each page
                parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

                // Fill data to memory
                var BtoBInstructDeletes = MvcDbContext.Current.Database.Connection.Query<BtoBInstructDeleteResultRow>(query.ToString(), parameters);
                var shpBtoBInstructDeletes = MvcDbContext.Current.ShpDelInstructions.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId);
                condition.InstructIdSum = shpBtoBInstructDeletes.Select(x => x.ShipInstructId).Distinct().Count();
                condition.PlanQtySum = shpBtoBInstructDeletes.Select(x => x.InstructQty).Sum();
                condition.SelectedCnt = MvcDbContext.Current.ShpDelInstructions.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId && x.IsCheck).Count();

                // Excute paging
                return new StaticPagedList<BtoBInstructDeleteResultRow>(BtoBInstructDeletes, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Update Work Table
        /// </summary>
        /// <param name="StockInquirys"></param>
        /// <returns></returns>
        public bool UpdateShpBtoBInstructDelete(IList<SelectedBtoBInstructDeleteViewModel> ShpBtoBInstructDeletes)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in ShpBtoBInstructDeletes)
                {
                    // 在庫明細
                    var shpBtoBInstructDelete = MvcDbContext.Current.ShpDelInstructions
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == u.Seq && m.LineNo == u.LineNo)
                                  .SingleOrDefault();

                    if (shpBtoBInstructDelete == null)
                    {
                        return false;
                    }

                    shpBtoBInstructDelete.SetBaseInfoUpdate();
                    shpBtoBInstructDelete.IsCheck = u.IsCheck;
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
        public async Task<bool> UpdateShpBtoBInstructDeleteAsync(CurrentBagForCpuTask CurrentBagForCpuTask, IList<SelectedBtoBInstructDeleteViewModel> ShpBtoBInstructDeletes)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = CurrentBagForCpuTask.CurrentMvcDbContext.Database.BeginTransaction())
            {
                foreach (var u in ShpBtoBInstructDeletes)
                {
                    // 在庫明細
                    var shpBtoBInstructDelete = CurrentBagForCpuTask.CurrentMvcDbContext.ShpDelInstructions
                                  .Where(m => m.ShipperId == CurrentBagForCpuTask.ShipperId && m.Seq == u.Seq && m.LineNo == u.LineNo)
                                  .SingleOrDefault();

                    if (shpBtoBInstructDelete == null)
                    {
                        return false;
                    }

                    shpBtoBInstructDelete.SetBaseInfoUpdate();
                    shpBtoBInstructDelete.IsCheck = u.IsCheck;
                    try
                    {
                        CurrentBagForCpuTask.CurrentMvcDbContext.SaveChanges();
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
        public bool ShpBtoBInstructDeleteAllChange(BtoBInstructDeleteSearchConditions conditions, bool check)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in MvcDbContext.Current.ShpDelInstructions
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq))
                {
                    u.SetBaseInfoUpdate();
                    u.IsCheck = check;
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

            conditions.SelectedCnt = MvcDbContext.Current.ShpDelInstructions
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq && m.IsCheck).Count();
            return true;
        }

        /// <summary>
        /// 実績更新
        /// </summary>
        public void BtoBInstructDelete(BtoBInstructDeleteSearchConditions searchConditions, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", searchConditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_WORK_ID", searchConditions.Seq, DbType.Int32, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_BTOB_INSTRUCT_DELETE",
                param,
                commandType: CommandType.StoredProcedure);
            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }

        /// <summary>
        /// 事業部データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListDivisions()
        {
            return MvcDbContext.Current.Divisions
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.DivisionId,
                    Text = m.DivisionId + "：" + m.DivisionName
                })
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// 配送業者データ取得
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
        /// 分類1データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys1()
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId1.ToString(),
                    Text = m.CategoryId1.ToString() + "：" + m.CategoryName1
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類2データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys2(string categoryId1 = "")
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId2.ToString(),
                    Text = m.CategoryId2.ToString() + "：" + m.CategoryName2
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類3データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys3(string categoryId1 = "", string categoryId2 = "")
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId
                && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1)
                && (categoryId2 == null ? 1 == 1 : m.CategoryId2 == categoryId2))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId3.ToString(),
                    Text = m.CategoryId3.ToString() + "：" + m.CategoryName3
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類4データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys4(string categoryId1 = "", string categoryId2 = "", string categoryId3 = "")
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId
                && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1)
                && (categoryId2 == null ? 1 == 1 : m.CategoryId2 == categoryId2)
                && (categoryId3 == null ? 1 == 1 : m.CategoryId3 == categoryId3))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId4.ToString(),
                    Text = m.CategoryId4.ToString() + "：" + m.CategoryName4
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// アイテムコード取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListItems()
        {
            return MvcDbContext.Current.Items
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.ItemCode,
                    Text = m.ItemCode + "：" + m.ItemCodeName
                })
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// 引当進捗ワークデータ取得
        /// </summary>
        /// <returns>引当進捗ワークデータ</returns>
        public AllocStatus GetAllocStatus(long Seq)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@" select * from W_ALLOC_STATUS where SHIPPER_ID = :SHIPPER_ID and SEQ = :SEQ
                ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":SEQ", Seq);
            var allocStatus = MvcDbContext.Current.Database.Connection.Query<AllocStatus>(query.ToString(), parameters).FirstOrDefault();
            return allocStatus;
        }
    }
}