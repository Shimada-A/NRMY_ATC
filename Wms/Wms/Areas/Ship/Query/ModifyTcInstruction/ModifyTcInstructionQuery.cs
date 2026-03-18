namespace Wms.Areas.Ship.Query.ModifyTcInstruction
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Ship.Models;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.JanSearchModal;
    using Wms.Areas.Ship.ViewModels.ModifyTcInstruction;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using Wms.Resources;
    using static Wms.Areas.Ship.ViewModels.ModifyTcInstruction.ModifyTcInstructionSearchConditions;

    public class ModifyTcInstructionQuery
    {
        /// <summary>
        /// Get List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public string MakeData(ModifyTcInstructionSearchConditions condition)
        {
            string message = string.Empty;
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT TPS.INVOICE_NO INVOICE_NO
                      ,MAX(TPS.SORT_STATUS) SORT_STATUS
                      ,SUM(TPS.STOCK_QTY) STOCK_QTY
                  FROM T_PACKAGE_STOCKS TPS
                 WHERE TPS.LOCATION_CD = (SELECT GEN_NAME
                                            FROM M_GENERALS
                                           WHERE SHIPPER_ID = :SHIPPER_ID
                                             AND (CENTER_ID = '@@@' OR CENTER_ID = :CENTER_ID)
                                             AND REGISTER_DIVI_CD = '1'
                                             AND GEN_DIV_CD = 'FIXED_LOCATION_CD'
                                             AND GEN_CD = 'NYK-TC1')
                   AND TPS.JAN LIKE :JAN
                   AND TPS.SHIPPER_ID = :SHIPPER_ID
                   AND TPS.CENTER_ID = :CENTER_ID
            ");
            parameters.Add(":JAN", condition.Jan + "%");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            if (!string.IsNullOrEmpty(condition.InvoiceNo))
            {
                query.Append(" AND TPS.INVOICE_NO = :INVOICE_NO ");
                parameters.Add(":INVOICE_NO", condition.InvoiceNo);
            }
            query.Append(" GROUP BY TPS.INVOICE_NO ");
            var tPackageStocks = MvcDbContext.Current.Database.Connection.Query(query.ToString(), parameters);
            if (condition.EditFlag)
            {
                if (!tPackageStocks.Any())
                {
                    message = MessagesResource.MSG_NOT_FOUND;
                    return message;
                }
                else if (tPackageStocks.Count() > 1)
                {
                    message = ModifyTcInstructionResource.ERR_MULTIPLE_INVOICE;
                    return message;
                }
                else if (tPackageStocks.FirstOrDefault().SORT_STATUS == 0)
                {
                    condition.EditFlag = false;
                    message = ModifyTcInstructionResource.ERR_CANT_EDIT1;
                    return message;
                }
                else if (tPackageStocks.FirstOrDefault().SORT_STATUS == 2
                      || tPackageStocks.FirstOrDefault().SORT_STATUS == 3)
                {
                    condition.EditFlag = false;
                    message = ModifyTcInstructionResource.ERR_CANT_EDIT2;
                    return message;
                }
                else
                {
                    condition.EditFlag = true;
                }
            }

            //同一納品書番号内複数JANエラーチェック
            query.Append(" ,TPS.JAN ");
            tPackageStocks = MvcDbContext.Current.Database.Connection.Query(query.ToString(), parameters);
            if (condition.EditFlag)
            {
                if (!tPackageStocks.Any())
                {
                    message = MessagesResource.MSG_NOT_FOUND;
                    return message;
                }
                else if (tPackageStocks.Count() > 1)
                {
                    message = ModifyTcInstructionResource.ERR_MULTIPLE_JAN;
                    return message;
                }
                else
                {
                    condition.EditFlag = true;
                }
            }

            message = string.Empty;
            condition.ArriveResultQtySum = int.Parse(tPackageStocks.FirstOrDefault().STOCK_QTY.ToString());
            parameters = new DynamicParameters();
            query = new StringBuilder();
            query.Append(@"
                SELECT TS.ITEM_ID
                      ,TS.ITEM_NAME
                      ,TS.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,TS.ITEM_SIZE_ID
                      ,MS.ITEM_SIZE_NAME
                      ,TS.SHIP_PLAN_DATE
                      ,CASE WHEN TS.EMERGENCY_CLASS = 1 THEN '" + ModifyTcInstructionResource.EmergencyClass + @"'
                            ELSE '' END EMERGENCY_CLASS_NAME
                      ,TS.SHIP_INSTRUCT_ID
                      ,TS.SHIP_INSTRUCT_SEQ
                      ,TS.SHIP_TO_STORE_ID
                      ,VSTS.SHIP_TO_STORE_NAME1 SHIP_TO_STORE_NAME
                      ,TS.PRIORITY_ORDER
                      ,CASE WHEN VSTS.STOCK_OUT_DISABLE_FLAG = 1 THEN '" + ModifyTcInstructionResource.StockOut + @"'
                            ELSE '' END STOCK_OUT_STORE
                      ,MSF.LANE_NO
                      ,MSF.FRONTAGE_NO
                      ,TS.MAKE_DATE ALLOC_UP_DATE
                      ,TS.INSTRUCT_QTY
                      ,TS.MIN_INSTRUCT_QTY
                      ,TS.WMS_INSTRUCT_QTY
                  FROM T_SHIPS TS
                 INNER JOIN V_SHIP_TO_STORES VSTS
                    ON TS.SHIPPER_ID = VSTS.SHIPPER_ID
                   AND TS.SHIP_TO_STORE_ID  = VSTS.SHIP_TO_STORE_ID
                 INNER JOIN M_SHIP_FRONTAGE MSF
                    ON TS.SHIPPER_ID = MSF.SHIPPER_ID
                   AND TS.CENTER_ID = MSF.CENTER_ID
                   AND TS.SHIP_TO_STORE_ID = MSF.STORE_ID
                 INNER JOIN M_COLORS MC
                    ON TS.SHIPPER_ID   = MC.SHIPPER_ID
                   AND TS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                 INNER JOIN M_SIZES MS
                    ON TS.SHIPPER_ID   = MS.SHIPPER_ID
                   AND TS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                 WHERE TS.SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                   AND TS.SHIP_KIND = 1
                   AND TS.JAN LIKE :JAN
                   AND TS.CENTER_ID = :CENTER_ID
                   AND TS.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIP_INSTRUCT_ID", tPackageStocks.FirstOrDefault().INVOICE_NO);
            parameters.Add(":JAN", condition.Jan + "%");
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // 1.ワークID採番
            condition.Seq = new BaseQuery().GetWorkId();
            condition.Page = 1;

            // 2.検索・ワーク作成
            var result = MvcDbContext.Current.Database.Connection.Query<ShpModTcInstruction>(query.ToString(), parameters);
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var row in result.Select((v, i) => new { v, i }))
                {
                    var shpModTcInstruction = new ShpModTcInstruction();
                    shpModTcInstruction.SetBaseInfoInsert();
                    shpModTcInstruction.Seq = condition.Seq;
                    shpModTcInstruction.LineNo = row.i + 1;
                    shpModTcInstruction.CenterId = condition.CenterId;
                    shpModTcInstruction.ItemId = row.v.ItemId;
                    shpModTcInstruction.ItemName = row.v.ItemName;
                    shpModTcInstruction.ItemColorId = row.v.ItemColorId;
                    shpModTcInstruction.ItemColorName = row.v.ItemColorName;
                    shpModTcInstruction.ItemSizeId = row.v.ItemSizeId;
                    shpModTcInstruction.ItemSizeName = row.v.ItemSizeName;
                    shpModTcInstruction.ShipPlanDate = row.v.ShipPlanDate;
                    shpModTcInstruction.EmergencyClassName = row.v.EmergencyClassName;
                    shpModTcInstruction.ShipInstructId = row.v.ShipInstructId;
                    shpModTcInstruction.ShipInstructSeq = row.v.ShipInstructSeq;
                    shpModTcInstruction.ShipToStoreId = row.v.ShipToStoreId;
                    shpModTcInstruction.ShipToStoreName = row.v.ShipToStoreName;
                    shpModTcInstruction.PriorityOrder = row.v.PriorityOrder;
                    shpModTcInstruction.StockOutStore = row.v.StockOutStore;
                    shpModTcInstruction.LaneNo = row.v.LaneNo;
                    shpModTcInstruction.FrontageNo = row.v.FrontageNo;
                    shpModTcInstruction.AllocUpDate = row.v.AllocUpDate;
                    shpModTcInstruction.InstructQty = row.v.InstructQty;
                    shpModTcInstruction.MinInstructQty = row.v.MinInstructQty;
                    shpModTcInstruction.WmsInstructQty = row.v.WmsInstructQty;
                    MvcDbContext.Current.ShpModTcInstructions.Add(shpModTcInstruction);
                    try
                    {
                        MvcDbContext.Current.SaveChanges();
                    }
                    catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                    {
                        return ex.ToString();
                    }
                }

                trans.Commit();
            }
            query = new StringBuilder();
            query.Append(@"
                SELECT TS.ITEM_SKU_ID
                  FROM T_SHIPS TS
                 WHERE TS.SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                   AND TS.SHIP_KIND = 1
                   AND TS.JAN LIKE :JAN
                   AND TS.CENTER_ID = :CENTER_ID
                   AND TS.SHIPPER_ID = :SHIPPER_ID
            ");
            condition.ItemSkuId = MvcDbContext.Current.Database.Connection.Query<string>(query.ToString(), parameters).First();
            condition.InvoiceNoHidden = tPackageStocks.FirstOrDefault().INVOICE_NO;
            return message;
        }

        /// <summary>
        /// Get List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<ModifyTcInstructionResultRow> GetData(ModifyTcInstructionSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT *
                  FROM WW_SHP_MOD_TC_INSTRUCTION WSMTI
                 WHERE WSMTI.SEQ = :SEQ
                   AND WSMTI.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SEQ", condition.Seq);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<ModifyTcInstructionResultRow>(query.ToString(), parameters).Count();
            var all = MvcDbContext.Current.Database.Connection.Query<ModifyTcInstructionResultRow>(query.ToString(), parameters);

            // Sort function
            switch (condition.SortKey)
            {
                case ModifyTcInstructionSortKey.LaneNoFrontageNo:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WSMTI.LANE_NO DESC,WSMTI.FRONTAGE_NO DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WSMTI.LANE_NO ASC,WSMTI.FRONTAGE_NO ASC ");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WSMTI.PRIORITY_ORDER DESC,WSMTI.SHIP_TO_STORE_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WSMTI.PRIORITY_ORDER ASC,WSMTI.SHIP_TO_STORE_ID ASC ");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var ModifyTcInstructions = MvcDbContext.Current.Database.Connection.Query<ModifyTcInstructionResultRow>(query.ToString(), parameters);
            condition.ArrivePlanQtySum = all.Select(x => x.InstructQty).Sum();
            condition.WmsInstructQtySum = all.Select(x => x.WmsInstructQty).Sum();
            condition.WmsInstructQtySumOtherPage = condition.WmsInstructQtySum - ModifyTcInstructions.Select(x => x.WmsInstructQty).Sum();
            // Excute paging
            return new StaticPagedList<ModifyTcInstructionResultRow>(ModifyTcInstructions, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Update Work Table
        /// </summary>
        /// <param name="StockInquirys"></param>
        /// <returns></returns>
        public bool UpdateShpModTcInstruction(IList<ShpModTcInstruction> ModifyTcInstructions)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in ModifyTcInstructions)
                {
                    // 在庫明細
                    var shpDcAllocation = MvcDbContext.Current.ShpModTcInstructions
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == u.Seq && m.LineNo == u.LineNo)
                                  .SingleOrDefault();

                    if (shpDcAllocation == null)
                    {
                        return false;
                    }

                    shpDcAllocation.SetBaseInfoUpdate();
                    shpDcAllocation.WmsInstructQty = u.WmsInstructQty;
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
        /// 一括設定
        /// </summary>
        /// <param name="StockInquirys"></param>
        /// <returns></returns>
        public bool UpdateAllShpModTcInstruction(ModifyTcInstructionSearchConditions conditions)
        {
            DynamicParameters parameters = new DynamicParameters();
            var arriveResultQty = conditions.ArriveResultQtySum <= conditions.ArrivePlanQtySum ? conditions.ArriveResultQtySum : conditions.ArrivePlanQtySum;

            // 優先順位順の場合
            if (conditions.Orders == Order.PriorityOrder)
            {
                var modifyTcInstructions = MvcDbContext.Current.ShpModTcInstructions.Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq).OrderBy(x => x.StockOutStore).ThenBy(x => x.PriorityOrder).ThenBy(x => x.ShipToStoreId);
                using (var trans = MvcDbContext.Current.Database.BeginTransaction())
                {
                    foreach (var u in modifyTcInstructions)
                    {
                        // 在庫明細
                        var shpDcAllocation = MvcDbContext.Current.ShpModTcInstructions
                                      .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == u.Seq && m.LineNo == u.LineNo)
                                      .SingleOrDefault();

                        if (shpDcAllocation == null)
                        {
                            return false;
                        }

                        shpDcAllocation.SetBaseInfoUpdate();
                        if (arriveResultQty == 0)
                        {
                            shpDcAllocation.WmsInstructQty = 0;
                        }
                        else if (arriveResultQty - u.InstructQty < 0)
                        {
                            shpDcAllocation.WmsInstructQty = arriveResultQty;
                            arriveResultQty = 0;
                        }
                        else
                        {
                            shpDcAllocation.WmsInstructQty = u.InstructQty;
                            arriveResultQty = arriveResultQty - u.InstructQty;
                        }
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
            }
            // 均等の場合
            else
            {
                var outStores = MvcDbContext.Current.ShpModTcInstructions.Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq && m.StockOutStore != null).OrderBy(x => x.PriorityOrder).ThenBy(x => x.ShipToStoreId).ToList();
                var inStores = MvcDbContext.Current.ShpModTcInstructions.Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq && m.StockOutStore == null).OrderBy(x => x.PriorityOrder).ThenBy(x => x.ShipToStoreId).ToList();
                outStores.ForEach(a => a.WmsInstructQty = null);
                inStores.ForEach(a => a.WmsInstructQty = null);
                using (var trans = MvcDbContext.Current.Database.BeginTransaction())
                {
                    if (outStores.Any())
                    {
                        if (arriveResultQty >= outStores.Select(x => x.InstructQty).Sum())
                        {
                            foreach (var u in outStores)
                            {
                                // 在庫明細
                                var shpDcAllocation = MvcDbContext.Current.ShpModTcInstructions
                                              .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == u.Seq && m.LineNo == u.LineNo)
                                              .SingleOrDefault();

                                if (shpDcAllocation == null)
                                {
                                    return false;
                                }

                                shpDcAllocation.SetBaseInfoUpdate();
                                shpDcAllocation.WmsInstructQty = u.InstructQty;
                                arriveResultQty = arriveResultQty - u.InstructQty;
                                try
                                {
                                    MvcDbContext.Current.SaveChanges();
                                }
                                catch (DbUpdateConcurrencyException)
                                {
                                    return false;
                                }
                            }
                            if (inStores.Any())
                            {
                                var loopcnt = Math.Ceiling(decimal.Parse((arriveResultQty == null ? 0 : arriveResultQty).ToString()) / inStores.Count);
                                for (int i = 0; i < loopcnt; i++)
                                {
                                    for (int j = 0; j < inStores.Count; j++)
                                    {
                                        if(arriveResultQty - 1 >= 0)
                                        {
                                            if ((inStores[j].WmsInstructQty == null ? 0 : inStores[j].WmsInstructQty) < (inStores[j].InstructQty == null ? 0 : inStores[j].InstructQty))
                                            {
                                                inStores[j].WmsInstructQty = inStores[j].WmsInstructQty == null ? 0 + 1 : inStores[j].WmsInstructQty + 1;
                                                arriveResultQty = arriveResultQty - 1;
                                            }
                                        }
                                    }
                                    if ((i == loopcnt - 1) && (arriveResultQty > 0))
                                    {
                                        i = i - 1;
                                    }
                                }
                                foreach (var u in inStores.Select((v, i) => new { v, i }))
                                {
                                    // 在庫明細
                                    var shpDcAllocation = MvcDbContext.Current.ShpModTcInstructions
                                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == u.v.Seq && m.LineNo == u.v.LineNo)
                                                  .SingleOrDefault();

                                    if (shpDcAllocation == null)
                                    {
                                        return false;
                                    }

                                    shpDcAllocation.SetBaseInfoUpdate();
                                    shpDcAllocation.WmsInstructQty = u.v.WmsInstructQty;
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
                        }
                        else
                        {
                            var loopcnt = Math.Ceiling(decimal.Parse((arriveResultQty == null ? 0 : arriveResultQty).ToString()) / outStores.Count);
                            for (int i = 0; i < loopcnt; i++)
                            {
                                for (int j = 0; j < outStores.Count; j++)
                                {
                                    if (arriveResultQty - 1 >= 0)
                                    {
                                        if ((outStores[j].WmsInstructQty == null ? 0 : outStores[j].WmsInstructQty) < (outStores[j].InstructQty == null ? 0 : outStores[j].InstructQty))
                                        {
                                            outStores[j].WmsInstructQty = outStores[j].WmsInstructQty == null ? 0 + 1 : outStores[j].WmsInstructQty + 1;
                                            arriveResultQty = arriveResultQty - 1;
                                        }
                                    }
                                }
                                if ((i == loopcnt - 1) && (arriveResultQty > 0))
                                {
                                    i = i - 1;
                                }
                            }
                            foreach (var u in outStores.Select((v, i) => new { v, i }))
                            {
                                // 在庫明細
                                var shpDcAllocation = MvcDbContext.Current.ShpModTcInstructions
                                              .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == u.v.Seq && m.LineNo == u.v.LineNo)
                                              .SingleOrDefault();

                                if (shpDcAllocation == null)
                                {
                                    return false;
                                }

                                shpDcAllocation.SetBaseInfoUpdate();
                                shpDcAllocation.WmsInstructQty = u.v.WmsInstructQty;
                                try
                                {
                                    MvcDbContext.Current.SaveChanges();
                                }
                                catch (DbUpdateConcurrencyException)
                                {
                                    return false;
                                }
                            }
                            if (inStores.Any())
                            {
                                foreach (var u in inStores.Select((v, i) => new { v, i }))
                                {
                                    // 在庫明細
                                    var shpDcAllocation = MvcDbContext.Current.ShpModTcInstructions
                                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == u.v.Seq && m.LineNo == u.v.LineNo)
                                                  .SingleOrDefault();

                                    if (shpDcAllocation == null)
                                    {
                                        return false;
                                    }

                                    shpDcAllocation.SetBaseInfoUpdate();
                                    shpDcAllocation.WmsInstructQty = 0;
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
                        }
                    }
                    else
                    {
                        if (inStores.Any())
                        {
                            var loopcnt = Math.Ceiling(decimal.Parse((arriveResultQty == null ? 0 : arriveResultQty).ToString()) / inStores.Count);
                            for (int i = 0; i < loopcnt; i++)
                            {
                                for (int j = 0; j < inStores.Count; j++)
                                {
                                    if (arriveResultQty - 1 >= 0)
                                    {
                                        if ((inStores[j].WmsInstructQty == null ? 0 : inStores[j].WmsInstructQty) < (inStores[j].InstructQty == null ? 0 : inStores[j].InstructQty))
                                        {
                                            inStores[j].WmsInstructQty = inStores[j].WmsInstructQty == null ? 0 + 1 : inStores[j].WmsInstructQty + 1;
                                            arriveResultQty = arriveResultQty - 1;
                                        }
                                    }
                                    else
                                    {
                                        inStores[j].WmsInstructQty = 0;
                                    }
                                }
                                if ((i == loopcnt - 1) && (arriveResultQty > 0))
                                {
                                    i = i - 1;
                                }
                            }
                            foreach (var u in inStores.Select((v, i) => new { v, i }))
                            {
                                // 在庫明細
                                var shpDcAllocation = MvcDbContext.Current.ShpModTcInstructions
                                              .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == u.v.Seq && m.LineNo == u.v.LineNo)
                                              .SingleOrDefault();

                                if (shpDcAllocation == null)
                                {
                                    return false;
                                }

                                shpDcAllocation.SetBaseInfoUpdate();
                                shpDcAllocation.WmsInstructQty = u.v.WmsInstructQty;
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
                    }

                    trans.Commit();
                }
            }

            return true;
        }

        /// <summary>
        /// 実績更新
        /// </summary>
        public string CheckMinInstructQty(ModifyTcInstructionSearchConditions conditions)
        {
            var modifyTcInstructions = MvcDbContext.Current.ShpModTcInstructions.Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq).ToList();
            foreach (var u in modifyTcInstructions)
            {
                if ((u.MinInstructQty > u.WmsInstructQty) ||(!string.IsNullOrEmpty(u.StockOutStore) && u.InstructQty > u.WmsInstructQty))
                {
                    return ModifyTcInstructionResource.MinInstructQtyMin;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 実績更新
        /// </summary>
        public void ModifyTcInstruction(ModifyTcInstructionSearchConditions conditions, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", conditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_ITEM_SKU_ID", conditions.ItemSkuId, DbType.String, ParameterDirection.Input);
            param.Add("IN_INVOICE_NO", conditions.InvoiceNoHidden, DbType.String, ParameterDirection.Input);
            param.Add("IN_WORK_ID", conditions.Seq, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_MODIFYTCINSTRUCTION",
                param,
                commandType: CommandType.StoredProcedure);
            status = param.Get<ProcedureStatus>("OUT_STATUS");
            if (status == ProcedureStatus.Success)
            {
                message = ModifyTcInstructionResource.SUC_UPDATE;
            }
            else
            {
                message = param.Get<string>("OUT_MESSAGE");
            }
        }

        /// <summary>
        /// JANモーダル検索結果取得
        /// </summary>
        /// <returns>検索結果</returns>
        public List<JanViewModel> ListingJan(ModifyTcInstructionSearchConditions searchCondition, ref int TotalItemCount)
        {
            DynamicParameters parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT
                        MIS.ITEM_ID AS ITEM_ID
                    ,   MIS.ITEM_NAME AS ITEM_NAME
                    ,   MIS.ITEM_SKU_ID AS ITEM_SKU_ID
                    ,   MIS.ITEM_COLOR_ID AS ITEM_COLOR_ID
                    ,   MIS.ITEM_SIZE_ID AS ITEM_SIZE_ID
                    ,   MIS.JAN AS JAN
                    ,   MC.ITEM_COLOR_NAME AS ITEM_COLOR_NAME
                    ,   MS.ITEM_SIZE_NAME AS ITEM_SIZE_NAME
                    ,   TPS.INVOICE_NO
                    ,   TO_CHAR(TPS.INVOICE_END_DATE,'YYYY/MM/DD HH24:MI:SS') AS INVOICE_END_DATE
                    ,   TPS.INVOICE_USER_ID AS INVOICE_USER_ID
                    ,   MU.USER_NAME AS INVOICE_USER_NAME
                    ,   MV.VENDOR_ID AS VENDOR_ID
                    ,   MV.VENDOR_NAME1 AS VENDOR_NAME1
                    ,   TPS.STOCK_QTY AS STOCK_QTY
                FROM
                        T_PACKAGE_STOCKS TPS
                INNER JOIN
                        M_ITEM_SKU MIS
                ON
                        TPS.SHIPPER_ID = MIS.SHIPPER_ID
                    AND TPS.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                LEFT JOIN M_COLORS MC
                ON
                        MIS.SHIPPER_ID = MC.SHIPPER_ID
                    AND MIS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                LEFT JOIN M_SIZES MS
                ON
                        MIS.SHIPPER_ID = MS.SHIPPER_ID
                    AND MIS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                LEFT OUTER JOIN
                        (SELECT
                                SHIPPER_ID
                            ,   CENTER_ID
                            ,   INVOICE_NO
                            ,   MAX(VENDOR_ID) AS VENDOR_ID
                        FROM
                                T_ARRIVE_PLANS
                        WHERE
                                SHIPPER_ID = :SHIPPER_ID
                            AND CENTER_ID = :CENTER_ID
                        GROUP BY
                                SHIPPER_ID
                            ,   CENTER_ID
                            ,   INVOICE_NO
                        ) TAR
                ON
                        TAR.SHIPPER_ID = TPS.SHIPPER_ID
                    AND TAR.CENTER_ID = TPS.CENTER_ID
                    AND TAR.INVOICE_NO = TPS.INVOICE_NO
                LEFT OUTER JOIN
                        M_VENDORS MV
                ON 
                        TAR.SHIPPER_ID = MV.SHIPPER_ID
                    AND TAR.VENDOR_ID = MV.VENDOR_ID
                LEFT JOIN
                        M_USERS MU
                ON
                        TPS.SHIPPER_ID = MU.SHIPPER_ID
                    AND TPS.INVOICE_USER_ID = MU.USER_ID
                WHERE
                        TPS.SHIPPER_ID = :SHIPPER_ID
                    AND TPS.CENTER_ID = :CENTER_ID
                    AND TPS.SORT_STATUS = 1
                ORDER BY
                        TPS.INVOICE_NO
                    ,   MV.VENDOR_ID
                    ,   MIS.ITEM_SKU_ID

            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", searchCondition.CenterId);


            // 全レコード数を取得
            var totalCount = MvcDbContext.Current.Database.Connection.Query<JanViewModel>(sql.ToString(), parameters).Count();
            TotalItemCount = totalCount;

            return MvcDbContext.Current.Database.Connection.Query<JanViewModel>(sql.ToString(), parameters).ToList();

        }
    }
}