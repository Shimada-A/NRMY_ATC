namespace Wms.Areas.Inventory.Query.Input
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Common;
    using Share.Extensions.Classes;
    using Share.Helpers;
    using Wms.Areas.Inventory.Models;
    using Wms.Areas.Inventory.Resources;
    using Wms.Areas.Inventory.ViewModels.Input;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Inventory.ViewModels.Input.InputSearchConditions;

    public class InputQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertInvInput(InputSearchConditions condition)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    // 1.ワークID採番
                    condition.Seq = new BaseQuery().GetWorkId();
                    condition.Page = condition.SearchType == SearchTypes.Search ? 1 : condition.Page;

                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                        INSERT INTO WW_INV_INPUT01(
                               MAKE_DATE
                              ,MAKE_USER_ID
                              ,MAKE_PROGRAM_NAME
                              ,UPDATE_DATE
                              ,UPDATE_USER_ID
                              ,UPDATE_PROGRAM_NAME
                              ,UPDATE_COUNT
                              ,SHIPPER_ID
                              ,SEQ
                              ,CENTER_ID
                              ,INVENTORY_NO
                              ,INVENTORY_SEQ
                              ,INVENTORY_START_DATE
                              ,INVENTORY_CLASS
                              ,INVENTORY_NAME
                              ,ITEM_SKU_ID
                              ,ITEM_NAME
                              ,JAN
                              ,ITEM_ID
                              ,ITEM_COLOR_ID
                              ,ITEM_SIZE_ID
                              ,LOCATION_CD
                              ,CASE_CLASS
                              ,GRADE_ID
                              ,BOX_NO
                              ,INVOICE_NO
                              ,STOCK_QTY_START
                              ,INVENTORY_CONFIRM_FLAG
                              ,INVENTORY_CONFIRM_DATE
                              ,INVENTORY_CONFIRM_SEQ
                              ,STOCK_FLAG
                              ,SIMPLE_INVENTORY_FLAG
                              ,DIFFERENCE_LIST_FLAG
                              ,DIFFERENCE_LIST_DATE
                              ,DIFFERENCE_LIST_USER_ID
                              ,RESULT_QTY_BEFORE
                              ,RESULT_QTY
                              ,ADD_FLAG
                            )
                        SELECT
                              " + "SYSTIMESTAMP " + " AS MAKE_DATE" +
                              ", '" + Common.Profile.User.UserId + "' AS MAKE_USER_ID" +
                              ",'InventoryInput'" + " AS MAKE_PROGRAM_NAME" +
                              ",SYSTIMESTAMP " + "AS UPDATE_DATE" +
                              ", '" + Common.Profile.User.UserId + "' AS UPDATE_USER_ID" +
                              ",'InventoryInput'" + " AS UPDATE_PROGRAM_NAME" +
                              ",0" + " AS UPDATE_COUNT" +
                              "," + "'" + Common.Profile.User.ShipperId + "'" + " AS SHIPPER_ID" +
                              "," + condition.Seq + " AS SEQ");
                    query.Append(@"
                              ,TIP.CENTER_ID
                              ,TIP.INVENTORY_NO
                              ,TIP.INVENTORY_SEQ
                              ,TIP.INVENTORY_START_DATE
                              ,TIP.INVENTORY_CLASS
                              ,TIP.INVENTORY_NAME
                              ,TIP.ITEM_SKU_ID
                              ,TIP.ITEM_NAME
                              ,TIP.JAN
                              ,TIP.ITEM_ID
                              ,TIP.ITEM_COLOR_ID
                              ,TIP.ITEM_SIZE_ID
                              ,TIP.LOCATION_CD
                              ,TIP.CASE_CLASS
                              ,TIP.GRADE_ID
                              ,TIP.BOX_NO
                              ,TIP.INVOICE_NO
                              ,TIP.STOCK_QTY_START
                              ,TIP.INVENTORY_CONFIRM_FLAG
                              ,TIP.INVENTORY_CONFIRM_DATE
                              ,TIP.INVENTORY_CONFIRM_SEQ
                              ,TIP.STOCK_FLAG
                              ,TIP.SIMPLE_INVENTORY_FLAG
                              ,TIP.DIFFERENCE_LIST_FLAG
                              ,TIP.DIFFERENCE_LIST_DATE
                              ,TIP.DIFFERENCE_LIST_USER_ID
                              ,TIR.RESULT_QTY RESULT_QTY_BEFORE
                              ,TIR.RESULT_QTY
                              ,0
                          FROM T_INVENTORY_PLANS TIP
                          LEFT JOIN T_INVENTORY_RESULTS TIR
                            ON TIP.SHIPPER_ID = TIR.SHIPPER_ID
                           AND TIP.CENTER_ID = TIR.CENTER_ID
                           AND TIP.INVENTORY_NO = TIR.INVENTORY_NO
                           AND TIP.INVENTORY_SEQ = TIR.INVENTORY_SEQ
                           AND TIR.LAST_COUNT_FLAG = 1
                         INNER JOIN M_LOCATIONS ML
                            ON TIP.SHIPPER_ID = ML.SHIPPER_ID
                           AND TIP.CENTER_ID = ML.CENTER_ID
                           AND TIP.LOCATION_CD = ML.LOCATION_CD
                         WHERE TIP.SHIPPER_ID = :SHIPPER_ID
                           AND TIP.INVENTORY_CONFIRM_FLAG <> 3
                     ");
                    parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

                    // Add search condition
                    // センター
                    if (!string.IsNullOrEmpty(condition.CenterId))
                    {
                        query.Append(" AND TIP.CENTER_ID = :CENTER_ID ");
                        parameters.Add(":CENTER_ID", condition.CenterId);
                    }
                    // 棚卸No
                    if (!string.IsNullOrEmpty(condition.InventoryNo))
                    {
                        query.Append(" AND TIP.INVENTORY_NO = :INVENTORY_NO ");
                        parameters.Add(":INVENTORY_NO", condition.InventoryNo);
                    }
                    // ロケーション
                    if (!string.IsNullOrEmpty(condition.LocationCd))
                    {
                        query.Append(" AND TIP.LOCATION_CD LIKE :LOCATION_CD ");
                        parameters.Add(":LOCATION_CD", condition.LocationCd + "%");
                    }
                    // ケースNo
                    if (!string.IsNullOrEmpty(condition.BoxNo))
                    {
                        query.Append(" AND TIP.BOX_NO LIKE :BOX_NO ");
                        parameters.Add(":BOX_NO", condition.BoxNo + "%");
                    }
                    // SKU
                    if (!string.IsNullOrEmpty(condition.ItemSkuId))
                    {
                        query.Append(" AND TIP.ITEM_SKU_ID LIKE :ITEM_SKU_ID ");
                        parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId + "%");
                    }



                    // 2.検索・ワーク作成
                    var result = MvcDbContext.Current.Database.Connection.Query<InvInput01>(query.ToString(), parameters);

                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }

                //3.ワークテーブルから削除する。
                var invInput01s = MvcDbContext.Current.InvInput01s.Where(x => x.Seq != condition.Seq 
                                                                      && x.ShipperId == Profile.User.ShipperId 
                                                                      && x.MakeUserId == Profile.User.UserId);
                if (invInput01s.Any())
                {
                    MvcDbContext.Current.InvInput01s.RemoveRange(invInput01s);
                    MvcDbContext.Current.SaveChanges();
                }

                trans.Commit();
            }

            return true;
        }

        /// <summary>
        /// Get StockInquiry List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<InputResultRow> GetData(InputSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            //「棚卸実績入力ワーク01」を検索する
            StringBuilder query = new StringBuilder(@"
                SELECT WW.SEQ
                      ,WW.CENTER_ID
                      ,WW.INVENTORY_NO
                      ,WW.INVENTORY_SEQ
                      ,WW.INVENTORY_START_DATE
                      ,WW.INVENTORY_CLASS
                      ,WW.INVENTORY_NAME
                      ,WW.ITEM_SKU_ID
                      ,WW.LOCATION_CD
                      ,WW.CASE_CLASS
                      ,CASE WHEN WW.BOX_NO = ' ' THEN '" + InputResource.BaraCom + @"' ELSE '" + InputResource.CaseCom + @"' END CASE_CLASS_NAME
                      ,WW.GRADE_ID
                      ,WW.BOX_NO
                      ,WW.ITEM_ID
                      ,WW.ITEM_NAME
                      ,WW.ITEM_COLOR_ID
                      ,WW.ITEM_SIZE_ID
                      ,MC.ITEM_COLOR_NAME
                      ,MIS.ITEM_SIZE_NAME
                      ,WW.JAN
                      ,WW.STOCK_QTY_START
                      ,WW.STOCK_FLAG
                      ,WW.SIMPLE_INVENTORY_FLAG
                      ,WW.RESULT_QTY_BEFORE
                      ,WW.RESULT_QTY
                      ,CASE WHEN WW.RESULT_QTY IS NULL THEN NULL 
                        WHEN NVL(WW.RESULT_QTY,0) - NVL(WW.STOCK_QTY_START,0) = 0 THEN NULL
                        ELSE NVL(WW.RESULT_QTY,0) - NVL(WW.STOCK_QTY_START,0) 
                        END DIFFERENCE_QTY
                      ,WW.ADD_FLAG
                      ,WW.RESULT_QTY RESULT_QTY_HID
                  FROM WW_INV_INPUT01 WW
                 INNER JOIN M_COLORS MC
                    ON WW.SHIPPER_ID = MC.SHIPPER_ID
                   AND WW.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                 LEFT JOIN M_ITEM_SKU MIS
                    ON WW.SHIPPER_ID = MIS.SHIPPER_ID
                   AND WW.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                 WHERE WW.SHIPPER_ID = :SHIPPER_ID
                   AND WW.SEQ = :SEQ
                 ORDER BY WW.LOCATION_CD
                         ,WW.ITEM_SKU_ID
                         ,WW.BOX_NO
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<InputResultRow>(query.ToString(), parameters).Count();
            condition.TotalCount = totalCount;

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var Inputs = MvcDbContext.Current.Database.Connection.Query<InputResultRow>(query.ToString(), parameters);

            //ケースエリアとバラエリア・荷姿混在エリアの判定を行う
            query = new StringBuilder(@"
                SELECT CASE WHEN MAX(ML.CASE_CLASS) = 1 AND MIN(ML.CASE_CLASS) = 1 THEN 1
                            WHEN MAX(ML.CASE_CLASS) = 2 AND MIN(ML.CASE_CLASS) = 2 THEN 2
                            ELSE 3 END
                  FROM M_LOCATIONS ML
                 WHERE ML.SHIPPER_ID = :SHIPPER_ID
                   AND ML.CENTER_ID = :CENTER_ID
                   AND ML.INVENTORY_NO = :INVENTORY_NO
            ");

            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":INVENTORY_NO", condition.InventoryNo);

            // 全レコード数を取得
            condition.AreaType = MvcDbContext.Current.Database.Connection.QuerySingle<AreaTypes>(query.ToString(), parameters);
            // Excute paging
            return new StaticPagedList<InputResultRow>(Inputs, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool UpdateInvInput(InputViewModel vm)
        {

            var dbContext = MvcDbContext.Current;
            if (vm.SearchConditions.Seq == 0) { vm.SearchConditions.Seq = new BaseQuery().GetWorkId(); }
            using (var trans = dbContext.Database.BeginTransaction())
            {
                foreach (var cModel in vm.Results.InputList.Where(x=>!x.AddFlag).Select((v, i) => new { v, i }))
                {
                    InvInput01 invInput01 = dbContext.InvInput01s.Find(cModel.v.Seq, cModel.v.CenterId, cModel.v.InventoryNo, cModel.v.InventorySeq, Profile.User.ShipperId);

                    invInput01.SetBaseInfoUpdate();

                    invInput01.ResultQty = cModel.v.ResultQty;

                    try
                    {
                        dbContext.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return false;
                    }
                }
                //ケースエリアとバラエリア・荷姿混在エリアの判定を行う

                DynamicParameters parameters = new DynamicParameters();
                StringBuilder query = new StringBuilder(@"
                    SELECT
                            NVL(MAX(TIP.INVENTORY_SEQ), 0) AS INVENTORY_SEQ
                        ,   MAX(LOC.INVENTORY_NAME) AS INVENTORY_NAME
                        ,   NVL(MAX(TIP.INVENTORY_CLASS), 2) AS INVENTORY_CLASS
                        ,   MAX(LOC.INVENTORY_START_DATE) AS INVENTORY_START_DATE
                        ,   NVL(MAX(TIP.SIMPLE_INVENTORY_FLAG), 0) AS SIMPLE_INVENTORY_FLAG
                    FROM
                            M_LOCATIONS LOC
                    LEFT OUTER JOIN
                            T_INVENTORY_PLANS TIP
                    ON
                            LOC.INVENTORY_NO = TIP.INVENTORY_NO
                        AND LOC.LOCATION_CD = TIP.LOCATION_CD
                        AND LOC.CENTER_ID = TIP.CENTER_ID
                        AND LOC.SHIPPER_ID = TIP.SHIPPER_ID
                    WHERE
                            LOC.SHIPPER_ID = :SHIPPER_ID
                        AND LOC.CENTER_ID = :CENTER_ID
                        AND LOC.INVENTORY_NO = :INVENTORY_NO
                ");
                parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                parameters.Add(":CENTER_ID", vm.SearchConditions.CenterId);
                parameters.Add(":INVENTORY_NO", vm.SearchConditions.InventoryNo);

                // 全レコード数を取得
                var inventory = MvcDbContext.Current.Database.Connection.Query(query.ToString(), parameters).FirstOrDefault();
                var inventorySeq = inventory.INVENTORY_SEQ;
                var inventoryName = inventory.INVENTORY_NAME;
                var inventoryClass = inventory.INVENTORY_CLASS;
                var inventoryStartDate = inventory.INVENTORY_START_DATE;
                var inventorySimpleInventoryFlag = inventory.SIMPLE_INVENTORY_FLAG;
                var maxInventorySeq = 0;
                var invInput01List = dbContext.InvInput01s.Where(x => x.Seq == vm.SearchConditions.Seq && x.CenterId == vm.SearchConditions.CenterId && x.InventoryNo == vm.SearchConditions.InventoryNo && x.ShipperId == Profile.User.ShipperId).ToList();
                if (invInput01List.Count() != 0)
                {
                    maxInventorySeq = invInput01List.Select(x => x.InventorySeq).Max();
                }                
                inventorySeq = maxInventorySeq > inventorySeq ? maxInventorySeq : inventorySeq;
                foreach (var cModel in vm.Results.InputList.Where(x => x.AddFlag && x.InventorySeq != 0 && x.Seq != 0 && x.Jan != null && x.LocationCd != null).Select((v, i) => new { v, i }))
                {
                    InvInput01 invInput01 = dbContext.InvInput01s.Find(cModel.v.Seq, cModel.v.CenterId, cModel.v.InventoryNo, cModel.v.InventorySeq, Profile.User.ShipperId);

                    invInput01.SetBaseInfoUpdate();
                    invInput01.LocationCd = cModel.v.LocationCd;
                    if (!string.IsNullOrWhiteSpace(cModel.v.BoxNo))
                    {
                        invInput01.BoxNo = cModel.v.BoxNo;
                    }
                    else
                    {
                        invInput01.BoxNo = " ";
                    }
                    invInput01.Jan = cModel.v.Jan;
                    invInput01.ItemSkuId = cModel.v.ItemSkuId;
                    invInput01.ItemId = cModel.v.ItemId;
                    invInput01.ItemName = cModel.v.ItemName;
                    invInput01.ItemColorId = cModel.v.ItemColorId;
                    invInput01.ItemSizeId = cModel.v.ItemSizeId;
                    invInput01.ResultQty = cModel.v.ResultQty;
                    try
                    {
                        dbContext.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return false;
                    }
                }
                foreach (var cModel in vm.Results.InputList.Where(x =>x.AddFlag && x.InventorySeq==0 && x.Seq==0 && x.Jan != null && x.LocationCd != null).Select((v, i) => new { v, i }))
                {
                    cModel.v.Seq = vm.SearchConditions.Seq;
                    cModel.v.CenterId = vm.SearchConditions.CenterId;
                    cModel.v.InventoryNo = vm.SearchConditions.InventoryNo;
                    cModel.v.InventorySeq = int.Parse(inventorySeq.ToString()) + cModel.i + 1;
                    cModel.v.InventoryStartDate = inventoryStartDate;
                    cModel.v.InventoryClass = int.Parse(inventoryClass.ToString());
                    cModel.v.InventoryName = inventoryName;
                    cModel.v.StockFlag = false;
                    cModel.v.SimpleInventoryFlag = Convert.ToBoolean(inventorySimpleInventoryFlag);
                    cModel.v.AddFlag = true;
                    var invInput01 = new InvInput01();
                        invInput01.SetBaseInfoInsert();
                        invInput01.Seq = vm.SearchConditions.Seq;
                        invInput01.CenterId = vm.SearchConditions.CenterId;
                        invInput01.InventoryNo = vm.SearchConditions.InventoryNo;
                        invInput01.InventorySeq = int.Parse(inventorySeq.ToString()) + cModel.i + 1;
                        invInput01.InventoryStartDate = inventoryStartDate;
                        invInput01.InventoryClass = int.Parse(inventoryClass.ToString());
                        invInput01.InventoryName = inventoryName;
                        invInput01.ItemSkuId = cModel.v.ItemSkuId;
                        invInput01.ItemName = cModel.v.ItemName;
                        invInput01.Jan = cModel.v.Jan;
                        invInput01.ItemId = cModel.v.ItemId;
                        invInput01.ItemColorId = cModel.v.ItemColorId;
                        invInput01.ItemSizeId = cModel.v.ItemSizeId;
                        invInput01.LocationCd = cModel.v.LocationCd;
                        invInput01.CaseClass = cModel.v.CaseClass;
                        invInput01.GradeId = cModel.v.GradeId;
                    if (!string.IsNullOrWhiteSpace(cModel.v.BoxNo))
                    {
                        invInput01.BoxNo = cModel.v.BoxNo;
                    }
                    else
                    {
                        invInput01.BoxNo = " ";
                    }
                    invInput01.StockQtyStart = cModel.v.StockQtyStart;
                        invInput01.StockFlag = false;
                        invInput01.SimpleInventoryFlag = int.Parse(inventorySimpleInventoryFlag.ToString());
                        invInput01.ResultQtyBefore = cModel.v.ResultQtyBefore;
                        invInput01.ResultQty = cModel.v.ResultQty;
                        invInput01.AddFlag = 1;
                        MvcDbContext.Current.InvInput01s.Add(invInput01);
                        try
                        {
                            MvcDbContext.Current.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        return false;
                    }
                    catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                        {
                            return false;
                        }
                }
                trans.Commit();
            }
            vm.SearchConditions.TotalCount = dbContext.InvInput01s.Where(x => x.Seq == vm.SearchConditions.Seq && x.CenterId == vm.SearchConditions.CenterId && x.InventoryNo == vm.SearchConditions.InventoryNo && x.ShipperId == Profile.User.ShipperId).Count();
            return true;
        }

        /// <summary>
        /// 実績更新
        /// </summary>
        public void ResultUpdate(InputViewModel vm, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", vm.SearchConditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_SEQ", vm.SearchConditions.Seq, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_INVENTORY_NO", vm.SearchConditions.InventoryNo, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_INV_INPUT",
                param,
                commandType: CommandType.StoredProcedure);

            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }

        /// <summary>
        /// 棚卸No取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetInventoryNoList(InputSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT
                        LOC.INVENTORY_NO VALUE
                    ,   LOC.INVENTORY_NO || ':' || MAX(LOC.INVENTORY_NAME) TEXT
                FROM
                        M_LOCATIONS LOC
                LEFT OUTER JOIN
                        T_INVENTORY_PLANS INV
                ON
                        LOC.INVENTORY_NO = INV.INVENTORY_NO
                    AND LOC.LOCATION_CD = INV.LOCATION_CD
                    AND LOC.CENTER_ID = INV.CENTER_ID
                    AND LOC.SHIPPER_ID = INV.SHIPPER_ID
                WHERE
                        LOC.SHIPPER_ID = :SHIPPER_ID
                    AND LOC.CENTER_ID = :CENTER_ID
                    AND LOC.INVENTORY_CONFIRM_FLAG <> 3
                    AND NOT (LOC.INVENTORY_CONFIRM_FLAG = 0 AND INV.INVENTORY_NO IS NULL)
                 GROUP BY
                        LOC.SHIPPER_ID
                    ,   LOC.INVENTORY_NO
                 ORDER BY
                        LOC.INVENTORY_NO ASC
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);

            // 全レコード数を取得
            return MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters);
        }

        /// <summary>
        /// 商品取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public void GetItemInfo(string jan, out ItemInfo item)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT MIS.ITEM_SKU_ID
                      ,MIS.ITEM_ID
                      ,MIS.ITEM_NAME
                      ,MIS.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,MIS.ITEM_SIZE_ID
                      ,MIS.ITEM_SIZE_NAME
                  FROM M_ITEM_SKU MIS
                  LEFT JOIN M_COLORS MC
                    ON MIS.SHIPPER_ID   = MC.SHIPPER_ID
                   AND MIS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                  LEFT JOIN M_SIZES MS
                    ON MIS.SHIPPER_ID   = MS.SHIPPER_ID
                   AND MIS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                 WHERE MIS.SHIPPER_ID = :SHIPPER_ID
                   AND MIS.JAN = :JAN
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":JAN", jan);

            // 全レコード数を取得
            item = MvcDbContext.Current.Database.Connection.Query<ItemInfo>(query.ToString(), parameters).FirstOrDefault();
        }

        /// <summary>
        /// ロケーションチェック
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public void CheckLocation(string locationCd, string centerId, string inventoryNo,int lineClass,out string gradeId,out string message)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT GRADE_ID
                  FROM M_LOCATIONS
                 WHERE SHIPPER_ID = :SHIPPER_ID
                   AND CENTER_ID = :CENTER_ID
                   AND LOCATION_CD = :LOCATION_CD
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", centerId);
            parameters.Add(":LOCATION_CD", locationCd);
            gradeId = MvcDbContext.Current.Database.Connection.Query<string>(query.ToString(), parameters).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(gradeId))
            {
                message = InputResource.ErrLocationNotExist;
                return;
            }
            query = new StringBuilder(@"
                SELECT COUNT(0)
                  FROM M_LOCATIONS
                 WHERE SHIPPER_ID = :SHIPPER_ID
                   AND CENTER_ID = :CENTER_ID
                   AND LOCATION_CD = :LOCATION_CD
                   AND INVENTORY_NO = :INVENTORY_NO
                   AND ROWNUM = 1
            ");
            parameters.Add(":INVENTORY_NO", inventoryNo);
            if(MvcDbContext.Current.Database.Connection.Query<int>(query.ToString(), parameters).FirstOrDefault() == 0)
            {
                message = InputResource.ErrOutInventory;
                return;
            }
            query = new StringBuilder(@"
                SELECT MLC.CASE_CLASS
                  FROM M_LOCATIONS ML
                 INNER JOIN M_LOCATION_CLASSES MLC
                    ON ML.SHIPPER_ID = MLC.SHIPPER_ID
                   AND ML.LOCATION_CLASS = MLC.LOCATION_CLASS
                 WHERE ML.SHIPPER_ID = :SHIPPER_ID
                   AND ML.CENTER_ID = :CENTER_ID
                   AND ML.LOCATION_CD = :LOCATION_CD
            ");
            var caseClass = MvcDbContext.Current.Database.Connection.Query<int>(query.ToString(), parameters).FirstOrDefault();
            if((lineClass == 1 && caseClass == 2) || (lineClass == 2 && caseClass == 1))
            {
                message = InputResource.ErrDiffCaseClass;
                return;
            }
            message = string.Empty;
        }

        /// <summary>
        /// 棚卸Noデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectInventoryNoList(string CenterId)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT DISTINCT
                        LOC.INVENTORY_NO VALUE
                    ,   LOC.INVENTORY_NO || ':' || LOC.INVENTORY_NAME TEXT
                FROM
                        M_LOCATIONS LOC
                LEFT OUTER JOIN
                        T_INVENTORY_PLANS INV
                ON
                        LOC.INVENTORY_NO = INV.INVENTORY_NO
                    AND LOC.LOCATION_CD = INV.LOCATION_CD
                    AND LOC.CENTER_ID = INV.CENTER_ID
                    AND LOC.SHIPPER_ID = INV.SHIPPER_ID
                WHERE
                        LOC.SHIPPER_ID = :SHIPPER_ID
                    AND LOC.INVENTORY_CONFIRM_FLAG <> 3
                    AND NOT (LOC.INVENTORY_CONFIRM_FLAG = 0 AND INV.INVENTORY_NO IS NULL)
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

            // センター
            if (!string.IsNullOrEmpty(CenterId))
            {
                query.Append(" AND LOC.CENTER_ID = :CENTER_ID ");
                parameters.Add(":CENTER_ID", CenterId);
            }

            query.Append(" ORDER BY LOC.INVENTORY_NO ");

            // 全レコード数を取得
            return MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters);
        }
    }
}