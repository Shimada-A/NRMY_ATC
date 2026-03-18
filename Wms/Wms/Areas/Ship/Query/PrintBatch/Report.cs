namespace Wms.Areas.Ship.Query.PrintBatch
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
    using Share.Extensions.Classes;
    using Wms.Areas.Ship.Models;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.PrintBatch;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.PrintBatch.PrintBatchSearchConditions;

    public class Report
    {
        /// <summary>
        /// バッチカンバン
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IEnumerable<BatchBoard> GetBatchBoard(PrintBatchSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT TAI.CENTER_ID || '　' || MCE.CENTER_NAME1 CENTER
                      ,TO_CHAR(TAI.ALLOC_DATE,'yyyy/MM/dd') ALLOC_DATE
                      ,TAI.ALLOC_NO BATCH_NO
                      ,TAI.ALLOC_NO BATCH_NO_BARCODE
                      ,CASE WHEN TAI.SHIP_KIND = 2 THEN '" + PrintBatchResource.Dc + @"'
                            WHEN TAI.SHIP_KIND = 3 THEN '" + PrintBatchResource.Ec + @"'
                            WHEN TAI.SHIP_KIND = 4 THEN '" + PrintBatchResource.CaseShip + @"'
                            WHEN TAI.SHIP_KIND = 5 THEN '" + PrintBatchResource.JanBoard + @"'
                            ELSE NULL END SHIP_KIND_NAME
                      ,TAI.BATCH_NAME
                      ,SYSDATE PRINT_DATE
                      ,:USER_ID || '　' || MU.USER_NAME PRINT_USER
                  FROM T_ALLOC_INFO TAI
                 INNER JOIN M_CENTERS MCE
                    ON TAI.SHIPPER_ID   = MCE.SHIPPER_ID
                   AND TAI.CENTER_ID = MCE.CENTER_ID
                 INNER JOIN M_USERS MU
                    ON TAI.SHIPPER_ID   = MU.SHIPPER_ID
                   AND :USER_ID = MU.USER_ID
                  WHERE TAI.SHIPPER_ID = :SHIPPER_ID
                    AND TAI.CENTER_ID = :CENTER_ID
                    AND TAI.ALLOC_GROUP_NO = :ALLOC_GROUP_NO
                  ORDER BY TAI.ALLOC_NO ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":ALLOC_GROUP_NO", condition.AllocGroupNo);
            parameters.Add(":USER_ID", Profile.User.UserId);
            return MvcDbContext.Current.Database.Connection.Query<BatchBoard>(query.ToString(), parameters);
        }

        /// <summary>
        /// トータルピッキングリスト
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IEnumerable<TotalPicking> GetTotalPicking(PrintBatchSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT TP.CENTER_ID || '　' || MCE.CENTER_NAME1 CENTER
                      ,TP.BATCH_NO
                      ,TP.BATCH_NO BATCH_NO_BARCODE
                      ,TAI.BATCH_NAME
                      ,TP.LOCSEC_1
                      ,TP.PICKING_GROUP_NO || '　' || MG.GEN_NAME PICKING_GROUP_NO
                      ,SYSDATE PRINT_DATE
                      ,:USER_ID || '　' || MU.USER_NAME PRINT_USER
                      ,TP.LOCATION_CD LOCATION_CD_BARCODE
                      ,TP.LOCSEC_1 || '_' || TP.PICKING_GROUP_NO AREA_EC
                      ,TP.LOCATION_CD 
                      ,CASE WHEN TP.CASE_CLASS = 1 THEN '" + PrintBatchResource.Case + @"'
                            WHEN TP.CASE_CLASS = 2 THEN '" + PrintBatchResource.Bara + @"'
                            ELSE NULL END CASE_CLASS
                      ,TP.ITEM_SKU_ID
                      ,TP.ITEM_ID
                      ,TP.ITEM_NAME
                      ,TP.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,TP.ITEM_SIZE_ID
                      ,MIS.ITEM_SIZE_NAME
                      ,TP.HIKI_QTY
                      ,TP.PIC_LOC_NO
                      ,CASE WHEN TP2.CASE_CNT > 1 AND TP.CASE_CLASS = 1 THEN '" + PrintBatchResource.HaveBara + @"'
                            WHEN TP2.CASE_CNT > 1 AND TP.CASE_CLASS = 2 THEN '" + PrintBatchResource.HaveCase + @"'
                            ELSE NULL END ASSORTMENT
                      ,TP.JAN
                      ," + (condition.chkJan ? "1" :"0") + @" JAN_FLAG
                      ,MIS.NOVELTY_NAME
                      ,DENSE_RANK() OVER(
                            PARTITION BY
                                     TP.LOCSEC_1
                                    ,TP.PICKING_GROUP_NO
                            ORDER BY
                                     TP.LOCSEC_1
                                    ,TP.PICKING_GROUP_NO
                                    ,TP.LOCATION_CD
                                    ,TP.ITEM_SKU_ID 
                      ) AS LIST_SEQ
                  FROM T_PIC TP
                 INNER JOIN M_COLORS MC
                    ON TP.SHIPPER_ID   = MC.SHIPPER_ID
                   AND TP.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                 INNER JOIN M_ITEM_SKU MIS
                    ON TP.SHIPPER_ID  = MIS.SHIPPER_ID
                   AND TP.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                 INNER JOIN M_CENTERS MCE
                    ON TP.SHIPPER_ID   = MCE.SHIPPER_ID
                   AND TP.CENTER_ID = MCE.CENTER_ID
                 INNER JOIN M_USERS MU
                    ON TP.SHIPPER_ID   = MU.SHIPPER_ID
                   AND :USER_ID = MU.USER_ID
                  LEFT JOIN T_ALLOC_INFO TAI
                    ON TP.BATCH_NO = TAI.ALLOC_NO
                   AND TP.SHIPPER_ID = TAI.SHIPPER_ID
                   AND TP.CENTER_ID = TAI.CENTER_ID
                  LEFT JOIN M_ITEM_SKU MIS
                    ON TP.SHIPPER_ID = MIS.SHIPPER_ID
                   AND TP.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                  LEFT JOIN M_GENERALS MG
                    ON TP.SHIPPER_ID = MG.SHIPPER_ID
                   AND MG.REGISTER_DIVI_CD = '1'
                   AND MG.CENTER_ID = TP.CENTER_ID
                   AND MG.GEN_DIV_CD = 'PICKING_GROUP_NAME'
                   AND TO_CHAR(TP.PICKING_GROUP_NO) = MG.GEN_CD
                  LEFT JOIN (
                             SELECT SHIPPER_ID
                                   ,CENTER_ID
                                   ,BATCH_NO
                                   ,ITEM_SKU_ID
                                   ,COUNT(DISTINCT(CASE_CLASS)) CASE_CNT
                               FROM T_PIC
                              GROUP BY SHIPPER_ID
                                      ,CENTER_ID
                                      ,BATCH_NO
                                      ,ITEM_SKU_ID) TP2
                    ON TP.BATCH_NO = TP2.BATCH_NO
                   AND TP.ITEM_SKU_ID = TP2.ITEM_SKU_ID
                   AND TP.CENTER_ID = TP2.CENTER_ID
                   AND TP.SHIPPER_ID = TP2.SHIPPER_ID
                  LEFT JOIN (
                             SELECT SHIPPER_ID
                                   ,CENTER_ID
                                   ,BATCH_NO
                                   ,ITEM_SKU_ID
                                   ,COUNT(DISTINCT(LOCATION_CD)) LOCATION_CNT
                               FROM T_PIC
                              WHERE PICK_KIND = 1
                                AND CASE_CLASS = 1
                              GROUP BY SHIPPER_ID
                                      ,CENTER_ID
                                      ,BATCH_NO
                                      ,ITEM_SKU_ID) TP3
                    ON TP.BATCH_NO = TP3.BATCH_NO
                   AND TP.ITEM_SKU_ID = TP3.ITEM_SKU_ID
                   AND TP.CENTER_ID = TP3.CENTER_ID
                   AND TP.SHIPPER_ID = TP3.SHIPPER_ID
                   AND TP.CASE_CLASS = 1
                 WHERE TP.SHIPPER_ID = :SHIPPER_ID
                   AND TP.CENTER_ID = :CENTER_ID
                   AND TP.BATCH_NO = :BATCH_NO
                   AND TP.PICK_KIND = 1
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":BATCH_NO", condition.AllocGroupNo);
            parameters.Add(":USER_ID", Profile.User.UserId);
            // ロケ開始、終了
            if (!string.IsNullOrEmpty(condition.LocationCdFrom))
            {
                query.Append(" AND TP.LOCATION_CD >= :LOCATION_CD_FROM ");
                parameters.Add(":LOCATION_CD_FROM", condition.LocationCdFrom);
            }
            if (!string.IsNullOrEmpty(condition.LocationCdTo))
            {
                query.Append(" AND TP.LOCATION_CD <= :LOCATION_CD_TO ");
                parameters.Add(":LOCATION_CD_TO", condition.LocationCdTo);
            }

            // 品番
            if (!string.IsNullOrEmpty(condition.ItemId))
            {
                query.Append(" AND TP.ITEM_ID = :ITEM_ID ");
                parameters.Add(":ITEM_ID", condition.ItemId);
            }

            // JAN
            if (!string.IsNullOrEmpty(condition.Jan))
            {
                query.Append(" AND TP.JAN = :JAN ");
                parameters.Add(":JAN", condition.Jan);
            }

            // SKU
            if (!string.IsNullOrEmpty(condition.ItemSkuId))
            {
                query.Append(" AND TP.ITEM_SKU_ID = :ITEM_SKU_ID ");
                parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId);
            }

            // 未ピックのみ、ピック中のみ
            if (condition.OnlyUnpicked && condition.OnlyPicDuring)
            {
                query.Append(" AND TP.PIC_STATUS IN (0,1) ");
            }
            else if (condition.OnlyUnpicked && !condition.OnlyPicDuring)
            {
                query.Append(" AND TP.PIC_STATUS = 0 ");
            }
            else if (!condition.OnlyUnpicked && condition.OnlyPicDuring)
            {
                query.Append(" AND TP.PIC_STATUS = 1 ");
            }

            query.Append(@"ORDER BY
                                     TP.LOCSEC_1
                                    ,TP.PICKING_GROUP_NO
                                    ,TP.LOCATION_CD
                                    ,TP.ITEM_SKU_ID ");

            // 複数ロケデータを持つデータ
            StringBuilder queryOthers = new StringBuilder(@"
                SELECT DISTINCT(TP.LOCATION_CD) LOCATION_CD
                  FROM T_PIC TP
                 WHERE TP.SHIPPER_ID = :SHIPPER_ID
                   AND TP.CENTER_ID = :CENTER_ID
                   AND TP.BATCH_NO = :BATCH_NO
                   AND TP.ITEM_SKU_ID = :ITEM_SKU_ID
                   AND TP.LOCATION_CD <> :LOCATION_CD
                   AND TP.PICK_KIND = 1
                   AND TP.CASE_CLASS = 1
                 ORDER BY TP.LOCATION_CD
            ");
            var result = MvcDbContext.Current.Database.Connection.Query<TotalPickingDetailRow>(query.ToString(), parameters);
            var totalPickingList = new List<TotalPicking>();
            foreach (var cModel in result.Select((v, i) => new { v, i }))
            {
                var row1 = new TotalPicking();
                row1.Center = cModel.v.Center;
                row1.BatchNo = cModel.v.BatchNo;
                row1.BatchNoBarcode = cModel.v.BatchNoBarcode;
                row1.BatchName = cModel.v.BatchName;
                row1.Locsec_1 = cModel.v.Locsec_1;
                row1.PickingGroupNo = cModel.v.PickingGroupNo;
                row1.PrintDate = cModel.v.PrintDate;
                row1.PrintUser = cModel.v.PrintUser;
                row1.LocationCdBarcode = cModel.v.LocationCdBarcode;
                row1.AreaEc = cModel.v.AreaEc;
                row1.LocationCd = cModel.v.LocationCd;
                row1.CaseClass = cModel.v.CaseClass;
                row1.ItemId = cModel.v.ItemId;
                row1.ItemName = cModel.v.ItemName;
                row1.ItemColorId = cModel.v.ItemColorId;
                row1.ItemColorName = cModel.v.ItemColorName;
                row1.ItemSizeId = cModel.v.ItemSizeId;
                row1.ItemSizeName = cModel.v.ItemSizeName;
                row1.Jan = cModel.v.Jan;
                row1.HikiQty = cModel.v.HikiQty;
                row1.PicLocNo = cModel.v.PicLocNo;
                row1.Assortment = cModel.v.Assortment;
                row1.JanFlag = cModel.v.JanFlag;
                row1.NoveltyName = cModel.v.NoveltyName;
                row1.ListSeq = cModel.v.ListSeq;
                totalPickingList.Add(row1);

                // 複数ロケデータを持つデータ
                if (cModel.v.CaseClass == PrintBatchResource.Case)
                {
                    var parameterOthers = new DynamicParameters();
                    parameterOthers.Add(":SHIPPER_ID", Profile.User.ShipperId);
                    parameterOthers.Add(":CENTER_ID", condition.CenterId);
                    parameterOthers.Add(":BATCH_NO", condition.AllocGroupNo);
                    parameterOthers.Add(":ITEM_SKU_ID", cModel.v.ItemSkuId);
                    parameterOthers.Add(":LOCATION_CD", cModel.v.LocationCd);
                    var locations = MvcDbContext.Current.Database.Connection.Query<string>(queryOthers.ToString(), parameterOthers).ToList();

                    foreach (var otherlocation in locations)
                    {
                        var row = new TotalPicking();
                        row.Center = cModel.v.Center;
                        row.BatchNo = cModel.v.BatchNo;
                        row.BatchNoBarcode = cModel.v.BatchNoBarcode;
                        row.BatchName = cModel.v.BatchName;
                        row.Locsec_1 = cModel.v.Locsec_1;
                        row.PickingGroupNo = cModel.v.PickingGroupNo;
                        row.PrintDate = cModel.v.PrintDate;
                        row.PrintUser = cModel.v.PrintUser;
                        row.LocationCdBarcode = cModel.v.LocationCdBarcode;
                        row.AreaEc = cModel.v.AreaEc;
                        row.LocationCd = cModel.v.LocationCd;
                        row.CaseClass = PrintBatchResource.Case;
                        row.LocationCd2 = "(" + otherlocation + ")";
                        row.CaseClass2 = PrintBatchResource.Case;
                        row.ItemId = cModel.v.ItemId;
                        row.ItemName = cModel.v.ItemName;
                        row.ItemColorId = cModel.v.ItemColorId;
                        row.ItemColorName = cModel.v.ItemColorName;
                        row.ItemSizeId = cModel.v.ItemSizeId;
                        row.ItemSizeName = cModel.v.ItemSizeName;
                        row.Jan = cModel.v.Jan;
                        row.HikiQty = cModel.v.HikiQty;
                        row.PicLocNo = cModel.v.PicLocNo;
                        row.Assortment = cModel.v.Assortment;
                        row.JanFlag = cModel.v.JanFlag;
                        row.NoveltyName = cModel.v.NoveltyName;
                        row.ListSeq = cModel.v.ListSeq;
                        totalPickingList.Add(row);
                    }
                }
            }

            return totalPickingList.AsEnumerable();
        }

        /// <summary>
        /// ECバッチ単位ピッキングリスト
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IEnumerable<EcBatchPicking> GetEcBatchPicking(PrintBatchSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT TP.CENTER_ID || '　' || MCE.CENTER_NAME1 CENTER
                      ,TP.BATCH_NO
                      ,TP.BATCH_NO BATCH_NO_BARCODE
                      ,TAI.BATCH_NAME
                      ,CASE WHEN TAI.EC_ONE_BATCH_CLASS = 1 THEN '" + PrintBatchResource.REcOneBatchClass1 + @"'
                            WHEN TAI.EC_ONE_BATCH_CLASS = 2 THEN '" + PrintBatchResource.REcOneBatchClass2 + @"'
                            ELSE NULL END EC_ONE_BATCH_CLASS
                      ,TAI.EC_ONE_BATCH_ID
                      ,SYSDATE PRINT_DATE
                      ,:USER_ID || '　' || MU.USER_NAME PRINT_USER
                      ,TAI.ALLOC_GROUP_NO
                      ,TP.LOCATION_CD 
                      ,CASE WHEN TP.CASE_CLASS = 1 THEN '" + PrintBatchResource.Case + @"'
                            WHEN TP.CASE_CLASS = 2 THEN '" + PrintBatchResource.Bara + @"'
                            ELSE NULL END CASE_CLASS
                      ,TP.ITEM_SKU_ID
                      ,TP.ITEM_ID
                      ,TP.ITEM_NAME
                      ,TP.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,TP.ITEM_SIZE_ID
                      ,MIS.ITEM_SIZE_NAME
                      ,SUBSTR(TP.JAN,1,7) JAN1
                      ,SUBSTR(TP.JAN,8,6) JAN2
                      ,TP.HIKI_QTY
                      ,SUBSTR(TP.JAN,1,12) JAN
                  FROM T_PIC TP
                 INNER JOIN M_COLORS MC
                    ON TP.SHIPPER_ID   = MC.SHIPPER_ID
                   AND TP.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                 INNER JOIN M_ITEM_SKU MIS
                    ON TP.SHIPPER_ID   = MIS.SHIPPER_ID
                   AND TP.ITEM_SKU_ID  = MIS.ITEM_SKU_ID
                 INNER JOIN M_CENTERS MCE
                    ON TP.SHIPPER_ID   = MCE.SHIPPER_ID
                   AND TP.CENTER_ID = MCE.CENTER_ID
                 INNER JOIN M_USERS MU
                    ON TP.SHIPPER_ID   = MU.SHIPPER_ID
                   AND :USER_ID = MU.USER_ID
                  LEFT JOIN T_ALLOC_INFO TAI
                    ON TP.BATCH_NO = TAI.ALLOC_NO
                   AND TP.SHIPPER_ID = TAI.SHIPPER_ID
                   AND TP.CENTER_ID = TAI.CENTER_ID
                  LEFT JOIN (
                             SELECT SHIPPER_ID
                                   ,CENTER_ID
                                   ,BATCH_NO
                                   ,ITEM_SKU_ID
                                   ,COUNT(DISTINCT(LOCATION_CD)) LOCATION_CNT
                               FROM T_PIC
                              WHERE PICK_KIND = 2
                                AND CASE_CLASS = 1
                              GROUP BY SHIPPER_ID
                                      ,CENTER_ID
                                      ,BATCH_NO
                                      ,ITEM_SKU_ID) TP2
                    ON TP.BATCH_NO = TP2.BATCH_NO
                   AND TP.ITEM_SKU_ID = TP2.ITEM_SKU_ID
                   AND TP.CENTER_ID = TP2.CENTER_ID
                   AND TP.SHIPPER_ID = TP2.SHIPPER_ID
                   AND TP.CASE_CLASS = 1
                 WHERE TP.SHIPPER_ID = :SHIPPER_ID
                   AND TP.CENTER_ID = :CENTER_ID
                   AND TP.PICK_KIND = 2
                   AND TAI.ALLOC_GROUP_NO = :ALLOC_GROUP_NO
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":ALLOC_GROUP_NO", condition.AllocGroupNo);
            parameters.Add(":USER_ID", Profile.User.UserId);
            // ロケ開始、終了
            if (!string.IsNullOrEmpty(condition.LocationCdFrom))
            {
                query.Append(" AND TP.LOCATION_CD >= :LOCATION_CD_FROM ");
                parameters.Add(":LOCATION_CD_FROM", condition.LocationCdFrom);
            }
            if (!string.IsNullOrEmpty(condition.LocationCdTo))
            {
                query.Append(" AND TP.LOCATION_CD <= :LOCATION_CD_TO ");
                parameters.Add(":LOCATION_CD_TO", condition.LocationCdTo);
            }

            // 品番
            if (!string.IsNullOrEmpty(condition.ItemId))
            {
                query.Append(" AND TP.ITEM_ID = :ITEM_ID ");
                parameters.Add(":ITEM_ID", condition.ItemId);
            }

            // JAN
            if (!string.IsNullOrEmpty(condition.Jan))
            {
                query.Append(" AND TP.JAN = :JAN ");
                parameters.Add(":JAN", condition.Jan);
            }

            // SKU
            if (!string.IsNullOrEmpty(condition.ItemSkuId))
            {
                query.Append(" AND TP.ITEM_SKU_ID = :ITEM_SKU_ID ");
                parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId);
            }

            // 注文番号
            if (!string.IsNullOrEmpty(condition.BatchNo))
            {
                query.Append(" AND TAI.EC_ONE_BATCH_ID = :EC_ONE_BATCH_ID ");
                parameters.Add(":EC_ONE_BATCH_ID", condition.BatchNo);
            }

            // 未ピックのみ、ピック中のみ
            if (condition.OnlyUnpicked && condition.OnlyPicDuring)
            {
                query.Append(" AND TP.PIC_STATUS IN (0,1) ");
            }
            else if (condition.OnlyUnpicked && !condition.OnlyPicDuring)
            {
                query.Append(" AND TP.PIC_STATUS = 0 ");
            }
            else if (!condition.OnlyUnpicked && condition.OnlyPicDuring)
            {
                query.Append(" AND TP.PIC_STATUS = 1 ");
            }

            query.Append(@" ORDER BY TP.BATCH_NO
                                    ,TAI.EC_ONE_BATCH_ID
                                    ,TP.CASE_CLASS DESC
                                    ,TP.LOCATION_CD ASC
                                    ,CASE WHEN TP2.LOCATION_CNT IS NOT NULL AND TP2.LOCATION_CNT > 1 THEN 1 ELSE 0 END
                                    ,TP.ITEM_SKU_ID ");

            // 複数ロケデータを持つデータ
            StringBuilder queryOthers = new StringBuilder(@"
                SELECT DISTINCT(TP.LOCATION_CD) LOCATION_CD
                  FROM T_PIC TP
                  LEFT JOIN T_ALLOC_INFO TAI
                    ON TP.BATCH_NO = TAI.ALLOC_NO
                   AND TP.SHIPPER_ID = TAI.SHIPPER_ID
                   AND TP.CENTER_ID = TAI.CENTER_ID
                 WHERE TP.SHIPPER_ID = :SHIPPER_ID
                   AND TP.CENTER_ID = :CENTER_ID
                   AND TAI.ALLOC_GROUP_NO = :ALLOC_GROUP_NO
                   AND TP.ITEM_SKU_ID = :ITEM_SKU_ID
                   AND TP.LOCATION_CD <> :LOCATION_CD
                   AND TP.BATCH_NO   = :BATCH_NO
                   AND TP.PICK_KIND = 2
                   AND TP.CASE_CLASS = :CASE_CLASS
                 ORDER BY TP.LOCATION_CD
            ");
            var result = MvcDbContext.Current.Database.Connection.Query<EcBatchPickingDetailRow>(query.ToString(), parameters);
            var ecBatchPickingList = new List<EcBatchPicking>();
            foreach (var cModel in result.Select((v, i) => new { v, i }))
            {
                var row1 = new EcBatchPicking();
                row1.Center = cModel.v.Center;
                row1.BatchNo = cModel.v.BatchNo;
                row1.BatchNoBarcode = cModel.v.BatchNoBarcode;
                row1.BatchName = cModel.v.BatchName;
                row1.EcOneBatchClass = cModel.v.EcOneBatchClass;
                row1.EcOneBatchId = cModel.v.EcOneBatchId;
                row1.PrintDate = cModel.v.PrintDate;
                row1.PrintUser = cModel.v.PrintUser;
                row1.AllocGroupNo = cModel.v.AllocGroupNo;
                row1.LocationCd = cModel.v.LocationCd;
                row1.CaseClass = cModel.v.CaseClass;
                row1.ItemId = cModel.v.ItemId;
                row1.ItemName = cModel.v.ItemName;
                row1.ItemColorId = cModel.v.ItemColorId;
                row1.ItemColorName = cModel.v.ItemColorName;
                row1.ItemSizeId = cModel.v.ItemSizeId;
                row1.ItemSizeName = cModel.v.ItemSizeName;
                row1.Jan1 = cModel.v.Jan1;
                row1.Jan2 = cModel.v.Jan2;
                row1.Jan = cModel.v.Jan;
                row1.HikiQty = cModel.v.HikiQty;
                ecBatchPickingList.Add(row1);

                // 複数ロケデータを持つデータ
                if (cModel.v.CaseClass == PrintBatchResource.Case)
                {
                    var parameterOthers = new DynamicParameters();
                    parameterOthers.Add(":SHIPPER_ID", Profile.User.ShipperId);
                    parameterOthers.Add(":CENTER_ID", condition.CenterId);
                    parameterOthers.Add(":ALLOC_GROUP_NO", condition.AllocGroupNo);
                    parameterOthers.Add(":ITEM_SKU_ID", cModel.v.ItemSkuId);
                    parameterOthers.Add(":LOCATION_CD", cModel.v.LocationCd);
                    parameterOthers.Add(":CASE_CLASS", 1);
                    parameterOthers.Add(":BATCH_NO", cModel.v.BatchNo);
                    var locations = MvcDbContext.Current.Database.Connection.Query<string>(queryOthers.ToString(), parameterOthers).ToList();

                    foreach (var otherlocation in locations)
                    {
                        var row = new EcBatchPicking();
                        row.Center = cModel.v.Center;
                        row.BatchNo = cModel.v.BatchNo;
                        row.BatchNoBarcode = cModel.v.BatchNoBarcode;
                        row.BatchName = cModel.v.BatchName;
                        row.EcOneBatchClass = cModel.v.EcOneBatchClass;
                        row.EcOneBatchId = cModel.v.EcOneBatchId;
                        row.PrintDate = cModel.v.PrintDate;
                        row.PrintUser = cModel.v.PrintUser;
                        row.AllocGroupNo = cModel.v.AllocGroupNo;
                        row.LocationCd = cModel.v.LocationCd;
                        row.CaseClass = PrintBatchResource.Case;
                        row.LocationCd2 = "(" + otherlocation + ")";
                        row.CaseClass2 = PrintBatchResource.Case;
                        ecBatchPickingList.Add(row);
                    }
                }
                else if (cModel.v.CaseClass == PrintBatchResource.Bara)
                {
                    var parameterOthers = new DynamicParameters();
                    parameterOthers.Add(":SHIPPER_ID", Profile.User.ShipperId);
                    parameterOthers.Add(":CENTER_ID", condition.CenterId);
                    parameterOthers.Add(":ALLOC_GROUP_NO", condition.AllocGroupNo);
                    parameterOthers.Add(":ITEM_SKU_ID", cModel.v.ItemSkuId);
                    parameterOthers.Add(":LOCATION_CD", cModel.v.LocationCd);
                    parameterOthers.Add(":CASE_CLASS", 2);
                    parameterOthers.Add(":BATCH_NO", cModel.v.BatchNo);
                    var locations = MvcDbContext.Current.Database.Connection.Query<string>(queryOthers.ToString(), parameterOthers).ToList();

                    foreach (var otherlocation in locations)
                    {
                        var row = new EcBatchPicking();
                        row.Center = cModel.v.Center;
                        row.BatchNo = cModel.v.BatchNo;
                        row.BatchNoBarcode = cModel.v.BatchNoBarcode;
                        row.BatchName = cModel.v.BatchName;
                        row.EcOneBatchClass = cModel.v.EcOneBatchClass;
                        row.EcOneBatchId = cModel.v.EcOneBatchId;
                        row.PrintDate = cModel.v.PrintDate;
                        row.PrintUser = cModel.v.PrintUser;
                        row.AllocGroupNo = cModel.v.AllocGroupNo;
                        row.LocationCd = cModel.v.LocationCd;
                        row.CaseClass = PrintBatchResource.Bara;
                        row.LocationCd2 = "(" + otherlocation + ")";
                        row.CaseClass2 = PrintBatchResource.Bara;
                        ecBatchPickingList.Add(row);
                    }
                }
            }
            return ecBatchPickingList.AsEnumerable();
        }

        /// <summary>
        /// ECユニット仕分　カンバン
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IEnumerable<EcUnitBoard> GetEcUnitBoard(PrintBatchSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT T.* FROM (
                SELECT TE.CENTER_ID || '　' || MCE.CENTER_NAME1 CENTER
                      ,:USER_ID || '　' || MU.USER_NAME PRINT_USER
                      ,TE.BATCH_NO
                      ,TE.BATCH_NO BATCH_NO_BARCODE
                      ,TAI.BATCH_NAME
                      ,TE.EC_SHIP_CLASS
                      ,CASE WHEN TE.EC_SHIP_CLASS = 1 THEN '" + PrintBatchResource.EcShipClass1 + @"'
                            WHEN TE.EC_SHIP_CLASS = 2 THEN '" + PrintBatchResource.EcShipClass2 + @"' || ' ' || TE.EC_SHIP_SEQ
                            WHEN TE.EC_SHIP_CLASS = 3 THEN '" + PrintBatchResource.EcShipClass3 + @"' || ' ' || TE.EC_SHIP_SEQ
                            ELSE NULL END EC_SHIP_CLASS_NAME
                      ,TE.EC_UNIT_ID EC_UNIT_ID_BARCODE
                      ,CASE WHEN TE.EC_SHIP_CLASS = 1 THEN '" + PrintBatchResource.EcUnitName1 + @"' || TE.EC_UNIT_ID
                            WHEN TE.EC_SHIP_CLASS = 2 THEN '" + PrintBatchResource.EcUnitName2 + @"' || TE.EC_UNIT_ID
                            WHEN TE.EC_SHIP_CLASS = 3 THEN '" + PrintBatchResource.EcUnitName3 + @"' || TE.EC_UNIT_ID
                            ELSE NULL END EC_UNIT_NAME
                      ,CASE WHEN TE.EC_SHIP_CLASS = 1 THEN '1'
                            WHEN TE.EC_SHIP_CLASS = 2 THEN '2' || LPAD(TE.EC_SHIP_SEQ,9,'0')
                            WHEN TE.EC_SHIP_CLASS = 3 THEN '3' || LPAD(TE.EC_SHIP_SEQ,9,'0')
                            ELSE NULL END EC_SHIP_CLASS_NAME_ORDER
                  FROM T_ECUNITBORD TE
                 INNER JOIN M_CENTERS MCE
                    ON TE.SHIPPER_ID   = MCE.SHIPPER_ID
                   AND TE.CENTER_ID = MCE.CENTER_ID
                 INNER JOIN M_USERS MU
                    ON TE.SHIPPER_ID   = MU.SHIPPER_ID
                   AND :USER_ID = MU.USER_ID
                  LEFT JOIN T_ALLOC_INFO TAI
                    ON TE.SHIPPER_ID = TAI.SHIPPER_ID
                   AND TE.CENTER_ID = TAI.CENTER_ID
                   AND TE.BATCH_NO = TAI.ALLOC_NO
                  WHERE TE.SHIPPER_ID = :SHIPPER_ID
                    AND TE.CENTER_ID = :CENTER_ID
                    AND TE.BATCH_NO = :ALLOC_GROUP_NO) T
                  GROUP BY T.CENTER,T.PRINT_USER,T.BATCH_NO,T.BATCH_NO_BARCODE,T.BATCH_NAME,T.EC_SHIP_CLASS,T.EC_SHIP_CLASS_NAME,T.EC_UNIT_ID_BARCODE,T.EC_UNIT_NAME ,T.EC_SHIP_CLASS_NAME_ORDER
                  ORDER BY T.EC_SHIP_CLASS 
                          ,T.EC_SHIP_CLASS_NAME_ORDER ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":ALLOC_GROUP_NO", condition.AllocGroupNo);
            parameters.Add(":USER_ID", Profile.User.UserId);
            return MvcDbContext.Current.Database.Connection.Query<EcUnitBoard>(query.ToString(), parameters);
        }

        /// <summary>
        /// GASカンバン
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IEnumerable<EcGASBoard> GetEcGASBoard(PrintBatchSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT TE.CENTER_ID || '　' || MCE.CENTER_NAME1 CENTER
                      ,:USER_ID || '　' || MU.USER_NAME PRINT_USER
                      ,TE.BATCH_NO
                      ,TE.BATCH_NO BATCH_NO_BARCODE
                      ,TAI.BATCH_NAME
                      ,TE.GAS_BATCH_NO
                      ,TE.GAS_BATCH_NO GAS_BATCH_NO_BARCODE
                  FROM T_ECGASBORD TE
                 INNER JOIN M_CENTERS MCE
                    ON TE.SHIPPER_ID   = MCE.SHIPPER_ID
                   AND TE.CENTER_ID = MCE.CENTER_ID
                 INNER JOIN M_USERS MU
                    ON TE.SHIPPER_ID   = MU.SHIPPER_ID
                   AND :USER_ID = MU.USER_ID
                  LEFT JOIN T_ALLOC_INFO TAI
                    ON TE.SHIPPER_ID = TAI.SHIPPER_ID
                   AND TE.CENTER_ID = TAI.CENTER_ID
                   AND TE.BATCH_NO = TAI.ALLOC_NO
                  WHERE TE.SHIPPER_ID = :SHIPPER_ID
                    AND TE.CENTER_ID = :CENTER_ID
                    AND TE.ALLOC_GROUP_NO = :ALLOC_GROUP_NO
                  ORDER BY TE.BATCH_NO,TE.GAS_BATCH_NO ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":ALLOC_GROUP_NO", condition.AllocGroupNo);
            parameters.Add(":USER_ID", Profile.User.UserId);
            return MvcDbContext.Current.Database.Connection.Query<EcGASBoard>(query.ToString(), parameters);
        }

        /// <summary>
        /// ケース出荷　ケースピッキングリスト
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IEnumerable<CaseShipPicking> GetCaseShipPicking(PrintBatchSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                WITH
                    SELECTED_SHIPS AS (
                        SELECT DISTINCT
                                BATCH_NO
                            ,   BOX_NO
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   SHIP_TO_STORE_ID
                            ,   TRANSPORTER_ID
                        FROM
                                T_SHIPS
                        WHERE
                                SHIP_KIND = 4
                            AND BATCH_NO = :BATCH_NO
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                )
                SELECT
                        T.CENTER
                    ,   T.BATCH_NO
                    ,   MAX(T.BATCH_NAME) BATCH_NAME
                    ,   T.LOCSEC_1
                    ,   MAX(T.BATCH_NO_BARCODE) BATCH_NO_BARCODE
                    ,   MAX(T.PRINT_DATE) PRINT_DATE
                    ,   MAX(T.PRINT_USER) PRINT_USER
                    ,   MAX(T.LOCATION_CD_BARCODE) LOCATION_CD_BARCODE
                    ,   T.LOCATION_CD
                    ,   T.BOX_NO
                    ,   SUM(NVL(T.STOCK_QTY,0)) STOCK_QTY
                    ,   MAX(T.SHIP_TO_STORE_ID) AS SHIP_TO_STORE_ID
                    ,   MAX(T.SHIP_TO_STORE_NAME1) AS SHIP_TO_STORE_NAME
                    ,   MAX(T.TRANSPORTER_NAME) AS TRANSPORTER_NAME
                    ,   DENSE_RANK() OVER(
                            PARTITION BY
                                        T.LOCSEC_1
                            ORDER BY
                                        T.LOCSEC_1
                                    ,   T.LOCATION_CD
                        ) AS LIST_SEQ
                    ," + (condition.chkJan ? "1" :"0") + @" JAN_FLAG
                    ,   MAX(FRONTAGE_NO) AS FRONTAGE_NO
                FROM (
                    SELECT
                            TP.CENTER_ID || '　' || MCE.CENTER_NAME1 CENTER
                        ,   TP.BATCH_NO
                        ,   TAI.BATCH_NAME
                        ,   TP.LOCSEC_1
                        ,   TP.BATCH_NO BATCH_NO_BARCODE
                        ,   SYSDATE PRINT_DATE
                        ,   :USER_ID || '　' || MU.USER_NAME PRINT_USER
                        ,   TP.LOCATION_CD LOCATION_CD_BARCODE
                        ,   TP.LOCATION_CD 
                        ,   TP.BOX_NO
                        ,   TPS.STOCK_QTY
                        ,   SHIP.SHIP_TO_STORE_ID
                        ,   VS.SHIP_TO_STORE_NAME1
                        ,   MT.TRANSPORTER_NAME
                        ,   SF_GET_MAGUCHI_NO(TP.SHIPPER_ID,TP.CENTER_ID,SHIP.SHIP_TO_STORE_ID,MIS.BRAND_ID) AS FRONTAGE_NO
                    FROM
                            T_PIC TP
                    INNER JOIN
                            SELECTED_SHIPS SHIP
                    ON
                            TP.BATCH_NO = SHIP.BATCH_NO
                        AND TP.BOX_NO = SHIP.BOX_NO
                        AND TP.CENTER_ID = SHIP.CENTER_ID
                        AND TP.SHIPPER_ID = SHIP.SHIPPER_ID
                    INNER JOIN
                            M_CENTERS MCE
                    ON
                            TP.SHIPPER_ID   = MCE.SHIPPER_ID
                        AND TP.CENTER_ID = MCE.CENTER_ID
                    INNER JOIN
                            M_USERS MU
                    ON
                            TP.SHIPPER_ID   = MU.SHIPPER_ID
                        AND :USER_ID = MU.USER_ID
                    LEFT JOIN
                            T_ALLOC_INFO TAI
                    ON
                            TP.BATCH_NO = TAI.ALLOC_NO
                        AND TP.SHIPPER_ID = TAI.SHIPPER_ID
                        AND TP.CENTER_ID = TAI.CENTER_ID
                    LEFT JOIN
                            T_PACKAGE_STOCKS TPS
                    ON
                            TP.BOX_NO = TPS.BOX_NO
                        AND TP.SHIPPER_ID = TPS.SHIPPER_ID
                        AND TP.CENTER_ID = TPS.CENTER_ID
                    LEFT JOIN
                            V_SHIP_TO_STORES VS
                    ON
                            TP.SHIPPER_ID = VS.SHIPPER_ID
                        AND SHIP.SHIP_TO_STORE_ID = VS.SHIP_TO_STORE_ID
                    LEFT JOIN
                            M_TRANSPORTERS MT
                    ON
                            SHIP.SHIPPER_ID = MT.SHIPPER_ID
                        AND SHIP.TRANSPORTER_ID = MT.TRANSPORTER_ID
                    LEFT JOIN
                            M_ITEM_SKU MIS
                    ON
                            TP.SHIPPER_ID = MIS.SHIPPER_ID
                        AND TP.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                    WHERE
                            TP.SHIPPER_ID = :SHIPPER_ID
                        AND TP.CENTER_ID = :CENTER_ID
                        AND TP.BATCH_NO = :BATCH_NO
                        AND TP.PICK_KIND = 3
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":BATCH_NO", condition.AllocGroupNo);
            parameters.Add(":USER_ID", Profile.User.UserId);
            // ロケ開始、終了
            if (!string.IsNullOrEmpty(condition.LocationCdFrom))
            {
                query.Append(" AND TP.LOCATION_CD >= :LOCATION_CD_FROM ");
                parameters.Add(":LOCATION_CD_FROM", condition.LocationCdFrom);
            }
            if (!string.IsNullOrEmpty(condition.LocationCdTo))
            {
                query.Append(" AND TP.LOCATION_CD <= :LOCATION_CD_TO ");
                parameters.Add(":LOCATION_CD_TO", condition.LocationCdTo);
            }

            // 未ピックのみ、ピック中のみ
            if (condition.OnlyUnpicked && condition.OnlyPicDuring)
            {
                query.Append(" AND TP.PIC_STATUS IN (0,1) ");
            }
            else if (condition.OnlyUnpicked && !condition.OnlyPicDuring)
            {
                query.Append(" AND TP.PIC_STATUS = 0 ");
            }
            else if (!condition.OnlyUnpicked && condition.OnlyPicDuring)
            {
                query.Append(" AND TP.PIC_STATUS = 1 ");
            }
            query.Append(@" ) T
                GROUP BY T.CENTER,T.BATCH_NO,T.LOCSEC_1,T.LOCATION_CD,T.BOX_NO
                ORDER BY T.LOCSEC_1,T.LOCATION_CD,T.BOX_NO");

            return MvcDbContext.Current.Database.Connection.Query<CaseShipPicking>(query.ToString(), parameters);

        }

        /// <summary>
        /// ケース出荷　JAN抜き取りピッキングリスト
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IEnumerable<CaseJanPicking> GetCaseJanPicking(PrintBatchSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT TP.CENTER_ID || '　' || MCE.CENTER_NAME1 CENTER
                      ,TP.BATCH_NO
                      ,TAI.BATCH_NAME
                      ,TP.LOCSEC_1
                      ,TP.BATCH_NO BATCH_NO_BARCODE
                      ,SYSDATE PRINT_DATE
                      ,:USER_ID || '　' || MU.USER_NAME PRINT_USER
                      ,TP.LOCATION_CD LOCATION_CD_BARCODE
                      ,TP.LOCATION_CD 
                      ,TP.BOX_NO
                      ,TP.ITEM_ID
                      ,TP.ITEM_NAME
                      ,TP.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,TP.ITEM_SIZE_ID
                      ,MIS.ITEM_SIZE_NAME
                      ,TP.HIKI_QTY
                      ,TP.JAN
                      ,DENSE_RANK() OVER(
                            PARTITION BY
                                        TP.LOCSEC_1
                            ORDER BY
                                        TP.LOCSEC_1
                                    ,   TP.LOCATION_CD
                        ) AS LIST_SEQ
                      ," + (condition.chkJan ? "1" : "0") + @" AS JAN_FLAG
                  FROM T_PIC TP
                 INNER JOIN M_COLORS MC
                    ON TP.SHIPPER_ID   = MC.SHIPPER_ID
                   AND TP.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                 INNER JOIN M_ITEM_SKU MIS
                    ON TP.SHIPPER_ID  = MIS.SHIPPER_ID
                   AND TP.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                 INNER JOIN M_CENTERS MCE
                    ON TP.SHIPPER_ID   = MCE.SHIPPER_ID
                   AND TP.CENTER_ID = MCE.CENTER_ID
                 INNER JOIN M_USERS MU
                    ON TP.SHIPPER_ID   = MU.SHIPPER_ID
                   AND :USER_ID = MU.USER_ID
                  LEFT JOIN T_ALLOC_INFO TAI
                     ON TP.BATCH_NO = TAI.ALLOC_NO
                    AND TP.SHIPPER_ID = TAI.SHIPPER_ID
                    AND TP.CENTER_ID = TAI.CENTER_ID
                  WHERE TP.SHIPPER_ID = :SHIPPER_ID
                    AND TP.CENTER_ID = :CENTER_ID
                    AND TP.BATCH_NO = :BATCH_NO
                    AND TP.PICK_KIND = 4
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":BATCH_NO", condition.AllocGroupNo);
            parameters.Add(":USER_ID", Profile.User.UserId);
            // ロケ開始、終了
            if (!string.IsNullOrEmpty(condition.LocationCdFrom))
            {
                query.Append(" AND TP.LOCATION_CD >= :LOCATION_CD_FROM ");
                parameters.Add(":LOCATION_CD_FROM", condition.LocationCdFrom);
            }
            if (!string.IsNullOrEmpty(condition.LocationCdTo))
            {
                query.Append(" AND TP.LOCATION_CD <= :LOCATION_CD_TO ");
                parameters.Add(":LOCATION_CD_TO", condition.LocationCdTo);
            }

            // 品番
            if (!string.IsNullOrEmpty(condition.ItemId))
            {
                query.Append(" AND TP.ITEM_ID = :ITEM_ID ");
                parameters.Add(":ITEM_ID", condition.ItemId);
            }

            // JAN
            if (!string.IsNullOrEmpty(condition.Jan))
            {
                query.Append(" AND TP.JAN = :JAN ");
                parameters.Add(":JAN", condition.Jan);
            }

            // SKU
            if (!string.IsNullOrEmpty(condition.ItemSkuId))
            {
                query.Append(" AND TP.ITEM_SKU_ID = :ITEM_SKU_ID ");
                parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId);
            }

            // 未ピックのみ、ピック中のみ
            if (condition.OnlyUnpicked && condition.OnlyPicDuring)
            {
                query.Append(" AND TP.PIC_STATUS IN (0,1) ");
            }
            else if (condition.OnlyUnpicked && !condition.OnlyPicDuring)
            {
                query.Append(" AND TP.PIC_STATUS = 0 ");
            }
            else if (!condition.OnlyUnpicked && condition.OnlyPicDuring)
            {
                query.Append(" AND TP.PIC_STATUS = 1 ");
            }

            query.Append("ORDER BY TP.LOCSEC_1,TP.LOCATION_CD,TP.BOX_NO,TP.ITEM_SKU_ID");

            return MvcDbContext.Current.Database.Connection.Query<CaseJanPicking>(query.ToString(), parameters);
        }

        /// <summary>
        /// 摘取用カンバン
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IEnumerable<PickBoard> GetPickBoard(PrintBatchSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT 
                        T.CENTER
                    ,   T.PRINT_USER
                    ,   T.BATCH_NO
                    ,   TAI.BATCH_NAME
                    ,   T.PIC_LOC_NO
                    ,   T.JAN
                    ,   T.HIKI_QTY
                    ,   T.ITEM_ID
                    ,   T.ITEM_NAME
                    ,   T.ITEM_COLOR_ID || '　' || MC.ITEM_COLOR_NAME ITEM_COLOR
                    ,   T.ITEM_SIZE_ID || '　' || MIS.ITEM_SIZE_NAME ITEM_SIZE
                FROM(
                    SELECT 
                            MAX(TP.CENTER_ID || '　' || MCE.CENTER_NAME1) CENTER
                        ,   MAX(:USER_ID || '　' || MU.USER_NAME) PRINT_USER
                        ,   MAX(TP.BATCH_NO) BATCH_NO
                        ,   TP.PIC_LOC_NO
                        ,   MAX(TP.JAN) JAN
                        ,   SUM(NVL(TP.HIKI_QTY,0)) HIKI_QTY
                        ,   MAX(TP.ITEM_ID) ITEM_ID
                        ,   MAX(TP.ITEM_NAME) ITEM_NAME
                        ,   MAX(TP.ITEM_COLOR_ID) ITEM_COLOR_ID
                        ,   MAX(TP.ITEM_SIZE_ID) ITEM_SIZE_ID
                        ,   MAX(TP.ITEM_SKU_ID) ITEM_SKU_ID
                    FROM
                            T_PIC TP
                    INNER JOIN 
                            M_CENTERS MCE
                        ON 
                            TP.SHIPPER_ID   = MCE.SHIPPER_ID
                        AND TP.CENTER_ID = MCE.CENTER_ID
                    INNER JOIN 
                            M_USERS MU
                        ON 
                            TP.SHIPPER_ID   = MU.SHIPPER_ID
                        AND :USER_ID = MU.USER_ID
                    WHERE 
                            TP.SHIPPER_ID = :SHIPPER_ID
                        AND TP.CENTER_ID = :CENTER_ID
                        AND TP.BATCH_NO = :BATCH_NO
                        AND TP.PICK_KIND = 1
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":BATCH_NO", condition.AllocGroupNo);
            parameters.Add(":USER_ID", Profile.User.UserId);

            // 品番
            if (!string.IsNullOrEmpty(condition.ItemId))
            {
                query.Append(" AND TP.ITEM_ID = :ITEM_ID ");
                parameters.Add(":ITEM_ID", condition.ItemId);
            }

            // JAN
            if (!string.IsNullOrEmpty(condition.Jan))
            {
                query.Append(" AND TP.JAN = :JAN ");
                parameters.Add(":JAN", condition.Jan);
            }

            // SKU
            if (!string.IsNullOrEmpty(condition.ItemSkuId))
            {
                query.Append(" AND TP.ITEM_SKU_ID = :ITEM_SKU_ID ");
                parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId);
            }
            query.Append(@" GROUP BY TP.PIC_LOC_NO) T
                INNER JOIN 
                        M_COLORS MC
                    ON
                        MC.SHIPPER_ID   = :SHIPPER_ID
                    AND T.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                INNER JOIN 
                        M_ITEM_SKU MIS
                    ON 
                        MIS.SHIPPER_ID = :SHIPPER_ID
                    AND T.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                LEFT JOIN 
                        T_ALLOC_INFO TAI
                    ON  T.BATCH_NO = TAI.ALLOC_NO
                    AND TAI.SHIPPER_ID = :SHIPPER_ID
                    AND TAI.CENTER_ID = :CENTER_ID
                ORDER BY 
                        T.PIC_LOC_NO  ");
            return MvcDbContext.Current.Database.Connection.Query<PickBoard>(query.ToString(), parameters);
        }

        /// <summary>
        /// 店別ピッキングリスト
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<StorePicking> GetStorePicking(PrintBatchSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT
                        PIC.CENTER_ID || ' ' || CENTER.CENTER_NAME1 CENTER
                    ,   :USER_ID || ' ' || MU.USER_NAME PRINT_USER
                    ,   PIC.BATCH_NO
                    ,   TAI.BATCH_NAME
                    ,   PIC.LOCATION_CD
                    ,   CASE
                            WHEN ML.CASE_CLASS = 1 THEN '" + PrintBatchResource.Case + @"'
                            WHEN ML.CASE_CLASS = 2 THEN '" + PrintBatchResource.Bara + @"'
                            ELSE NULL 
                        END CASE_CLASS
                    ,   PIC.ITEM_COLOR_ID
                    ,   COLOR.ITEM_COLOR_NAME
                    ,   PIC.ITEM_SIZE_ID
                    ,   MIS.ITEM_SIZE_NAME
                    ,   PIC.SHIP_TO_STORE_ID
                    ,   VS.SHIP_TO_STORE_NAME1 SHIP_TO_STORE_NAME
                    ,   PIC.ITEM_ID
                    ,   PIC.ITEM_NAME
                    ,   PIC.JAN
                    ,   PIC.HIKI_QTY
                    ,   MIS.NOVELTY_NAME
                    ,   " + (condition.chkJan ? "1" : "0") + @" JAN_FLAG
                FROM
                        T_ORDER_PIC PIC
                INNER JOIN
                        M_USERS MU
                    ON
                        PIC.SHIPPER_ID = MU.SHIPPER_ID
                    AND MU.USER_ID = :USER_ID
                INNER JOIN
                        T_ALLOC_INFO TAI
                    ON
                        PIC.SHIPPER_ID = TAI.SHIPPER_ID
                    AND PIC.CENTER_ID = TAI.CENTER_ID
                    AND PIC.BATCH_NO = TAI.ALLOC_NO
                LEFT JOIN
                        M_ITEM_SKU MIS
                    ON
                        PIC.SHIPPER_ID = MIS.SHIPPER_ID
                    AND PIC.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                LEFT JOIN
                        M_COLORS COLOR
                    ON
                        PIC.SHIPPER_ID = COLOR.SHIPPER_ID
                    AND PIC.ITEM_COLOR_ID = COLOR.ITEM_COLOR_ID
                LEFT JOIN
                        M_CENTERS CENTER
                    ON
                        PIC.SHIPPER_ID = CENTER.SHIPPER_ID
                    AND PIC.CENTER_ID = CENTER.CENTER_ID
                LEFT JOIN
                        M_LOCATIONS ML
                    ON
                        PIC.SHIPPER_ID = ML.SHIPPER_ID
                    AND PIC.CENTER_ID = ML.CENTER_ID
                    AND PIC.LOCATION_CD = ML.LOCATION_CD
                LEFT JOIN
                        V_SHIP_TO_STORES VS
                    ON
                        PIC.SHIPPER_ID = VS.SHIPPER_ID
                    AND PIC.SHIP_TO_STORE_ID = VS.SHIP_TO_STORE_ID
                WHERE
                        PIC.SHIPPER_ID = :SHIPPER_ID
                    AND TAI.ALLOC_GROUP_NO = :ALLOC_GROUP_NO
                    AND PIC.CENTER_ID = :CENTER_ID
                    AND PIC.PICK_KIND = 5
            ");

            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":USER_ID", Profile.User.UserId);
            parameters.Add(":ALLOC_GROUP_NO", condition.AllocGroupNo);

            // ロケ開始、終了
            if (!string.IsNullOrEmpty(condition.LocationCdFrom))
            {
                query.Append(" AND PIC.LOCATION_CD >= :LOCATION_CD_FROM ");
                parameters.Add(":LOCATION_CD_FROM", condition.LocationCdFrom);
            }
            if (!string.IsNullOrEmpty(condition.LocationCdTo))
            {
                query.Append(" AND PIC.LOCATION_CD <= :LOCATION_CD_TO ");
                parameters.Add(":LOCATION_CD_TO", condition.LocationCdTo);
            }

            // 品番
            if (!string.IsNullOrEmpty(condition.ItemId))
            {
                query.Append(" AND PIC.ITEM_ID = :ITEM_ID ");
                parameters.Add(":ITEM_ID", condition.ItemId);
            }

            // JAN
            if (!string.IsNullOrEmpty(condition.Jan))
            {
                query.Append(" AND PIC.JAN = :JAN ");
                parameters.Add(":JAN", condition.Jan);
            }

            // SKU
            if (!string.IsNullOrEmpty(condition.ItemSkuId))
            {
                query.Append(" AND PIC.ITEM_SKU_ID = :ITEM_SKU_ID ");
                parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId);
            }

            // 注文番号
            if (!string.IsNullOrEmpty(condition.BatchNo))
            {
                query.Append(" AND PIC.BATCH_NO = :BATCH_NO ");
                parameters.Add(":BATCH_NO", condition.BatchNo);
            }

            // 未ピックのみ、ピック中のみ
            if (condition.OnlyUnpicked && condition.OnlyPicDuring)
            {
                query.Append(" AND PIC.PIC_STATUS IN (0,1) ");
            }
            else if (condition.OnlyUnpicked && !condition.OnlyPicDuring)
            {
                query.Append(" AND PIC.PIC_STATUS = 0 ");
            }
            else if (!condition.OnlyUnpicked && condition.OnlyPicDuring)
            {
                query.Append(" AND PIC.PIC_STATUS = 1 ");
            }

            query.Append(@"
                ORDER BY
                        PIC.BATCH_NO
                    ,   PIC.STORE_PIC_ORDER
                    ,   PIC.PIC_ORDER
            ");

            return MvcDbContext.Current.Database.Connection.Query<StorePicking>(query.ToString(), parameters);
        }

        /// <summary>
        /// Get List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IEnumerable<PrintBatchReport> GetPrintBatchReport(PrintBatchSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT TAI.CENTER_ID || '　' || MCE.CENTER_NAME1 CENTER
                      ,TO_CHAR(TAI.ALLOC_DATE,'yyyy/MM/dd') ALLOC_DATE
                      ,SYSDATE PRINT_DATE
                      ,:USER_ID || '　' || MU.USER_NAME PRINT_USER
                      ,TAI.ALLOC_NO BATCH_NO
                      ,TAI.BATCH_NAME
                  FROM T_ALLOC_INFO TAI
                 INNER JOIN M_CENTERS MCE
                    ON TAI.SHIPPER_ID   = MCE.SHIPPER_ID
                   AND TAI.CENTER_ID = MCE.CENTER_ID
                 INNER JOIN M_USERS MU
                    ON TAI.SHIPPER_ID   = MU.SHIPPER_ID
                   AND :USER_ID = MU.USER_ID
                  WHERE TAI.SHIPPER_ID = :SHIPPER_ID
                    AND TAI.CENTER_ID = :CENTER_ID ");
            if (condition.PrintFlag == "Ec")
            {
                query.Append(" AND TAI.SHIP_KIND = 3 ");
            }
            else if (condition.PrintFlag == "Dc")
            {
                query.Append(" AND TAI.SHIP_KIND = 2 ");
            }
            else
            {
                query.Append(" AND (TAI.SHIP_KIND = 4 OR TAI.SHIP_KIND = 5) ");
            }
            query.Append(" ORDER BY TAI.ALLOC_DATE,TAI.ALLOC_NO ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":USER_ID", Profile.User.UserId);
            return MvcDbContext.Current.Database.Connection.Query<PrintBatchReport>(query.ToString(), parameters);
        }
    }
}