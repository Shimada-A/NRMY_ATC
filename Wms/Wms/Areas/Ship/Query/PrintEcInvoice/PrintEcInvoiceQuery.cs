namespace Wms.Areas.Ship.Query.PrintEcInvoice
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
    using Wms.Areas.Ship.ViewModels.PrintEcInvoice;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.PrintEcInvoice.PrintEcInvoiceConditions;

    public class PrintEcInvoiceQuery
    {
        /// <summary>
        /// シングル新規発行時処理
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public void PrintSingleMain(PrintEcInvoiceConditions conditions, out int status, out string message)
        {
            status = 0;
            message = "";
            var errstatus = 0;
            CheckErrorSingle(conditions, out errstatus);
            if(errstatus != 0)
            {
                status = errstatus;
                switch (errstatus)
                {
                    case 2:
                        message = Resources.PrintEcInvoiceResource.ErrorNotEcShip;
                        break;
                    case 3:
                        message = Resources.PrintEcInvoiceResource.ErrorPic;
                        break;
                    case 4:
                        message = Wms.Resources.MessageResource.ERR_DEFECT;
                        break;
                    case 50:
                        UpdatePackingInfoSingle(conditions, 1, out status, out message);
                        if(status == 0)
                        {
                            message = Resources.PrintEcInvoiceResource.ErrorCancel;
                            status = 50;
                        }
                        else
                        {
                            //フラグ更新処理でエラーとなった場合
                            message = Wms.Resources.MessageResource.ERR_DEFECT;
                            status = 4;
                        }
                        break;
                    case 51:
                        UpdatePackingInfoSingle(conditions, 1, out status, out message);
                        if (status == 0)
                        {
                            message = Resources.PrintEcInvoiceResource.ErrorChange;
                            status = 51;
                        }
                        else
                        {
                            //フラグ更新処理でエラーとなった場合
                            message = Wms.Resources.MessageResource.ERR_DEFECT;
                            status = 4;
                        }
                        break;
                    case 6:
                        message = Resources.PrintEcInvoiceResource.ErrorOutPut;
                        break;
                    case 7:
                        message = string.Format(Share.Common.Resources.MessagesResource.Required, "ヤマトの場合箱サイズ");
                        break;
                    case 8:
                        message = Resources.PrintEcInvoiceResource.ErrorBoxSize;
                        break;

                }
            }
            else
            {
                UpdatePackingInfoSingle(conditions, 0, out status, out message);
            }
        }

        /// <summary>
        /// シングル新規発行エラーチェック処理
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="errstatus"></param>
        public void CheckErrorSingle(PrintEcInvoiceConditions conditions, out int errstatus)
        {

            IList<EcunitSort> ec_unit_sort;
            ec_unit_sort = MvcDbContext.Current.EcunitSorts
                .Where(m => m.BatchNo == conditions.BatchNo
                        && m.CenterId == conditions.CenterId
                        && m.ShipperId == Profile.User.ShipperId
                        && m.Jan == conditions.Jan
                        && m.EcShipClass == 1)
                .ToList();
            if (ec_unit_sort.Count == 0)
            {
                errstatus = 2;
                return;
            };

            EcunitSort target_single_data;
            target_single_data = ec_unit_sort
                .Where(m => m.SingleDeliSheetFlag == false)
                .OrderBy(m => m.EcShipOrder)
                .FirstOrDefault();
            if(target_single_data == null)
            {
                errstatus = 3;
                return;
            }

            Ecship target_ec_ship;
            target_ec_ship = MvcDbContext.Current.Ecships
                .Where(m => m.BatchNo == conditions.BatchNo
                         && m.CenterId == conditions.CenterId
                         && m.ShipperId == Profile.User.ShipperId
                         && m.ShipInstructId == target_single_data.ShipInstructId
                         && m.ItemSkuId == target_single_data.ItemSkuId
                         && m.Jan == target_single_data.Jan)
                .FirstOrDefault();
            if(target_ec_ship == null)
            {
                errstatus = 2;
                return;
            }
            if(target_ec_ship.AftAllocCancelFlag == 1)
            {
                conditions.ErrShipInstructId = target_ec_ship.ShipInstructId;
                conditions.ShipInstructId = target_ec_ship.ShipInstructId;
                conditions.ItemSkuId = target_ec_ship.ItemSkuId;
                errstatus = 50;
                return;
            }
            if(target_ec_ship.AftAllocUpFlag == 1)
            {
                conditions.ErrShipInstructId = target_ec_ship.ShipInstructId;
                conditions.ShipInstructId = target_ec_ship.ShipInstructId;
                conditions.ItemSkuId = target_ec_ship.ItemSkuId;
                errstatus = 51;
                return;
            }

            var check_packing_info = MvcDbContext.Current.ShipPackingInfoes
                .Where(m => m.ShipInstructId == target_ec_ship.ShipInstructId
                         && m.ShipInstructSeq == target_ec_ship.ShipInstructSeq
                         && m.CenterId == conditions.CenterId
                         && m.ShipperId == Profile.User.ShipperId
                         && m.EcFlag == true)
                .Select(m => m.ShipInstructId)
                .ToList();
            if(check_packing_info.Count > 0)
            {
                errstatus = 6;
                return;
            }

            //ヤマトの場合、箱サイズ必須
            if (target_ec_ship.TransporterId == "01")
            {
                if (conditions.PrnClass == PrintEcInvoiceConditions.PrnClasses.New && conditions.BoxSize1 == "")
                {
                    errstatus = 7;   //ヤマトの場合箱サイズは必須です
                    return;
                }
                if (conditions.BoxSize1 != "" && CheckBoxSize(conditions.BoxSize1) == false)
                {
                    errstatus = 8;   //ヤマト箱サイズが不正です
                    return;
                };

            }

            conditions.ShipInstructId = target_ec_ship.ShipInstructId;
            conditions.ShipInstructSeq = target_ec_ship.ShipInstructSeq;
            conditions.ItemSkuId = target_ec_ship.ItemSkuId;
            conditions.TransporterId = target_ec_ship.TransporterId;
            errstatus = 0;
        }

        /// <summary>
        /// シングル新規発行前処理
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public void UpdatePackingInfoSingle(PrintEcInvoiceConditions conditions, int cancel_flag, out int status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", conditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_BATCH_NO", conditions.BatchNo, DbType.String, ParameterDirection.Input);
            param.Add("IN_SHIP_INSTRUCT_ID", conditions.ShipInstructId, DbType.String, ParameterDirection.Input);
            param.Add("IN_SHIP_INSTRUCT_SEQ", conditions.ShipInstructSeq, DbType.String, ParameterDirection.Input);
            param.Add("IN_ITEM_SKU_ID", conditions.ItemSkuId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CANCEL_FLAG", cancel_flag, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE", conditions.BoxSize1, DbType.String, ParameterDirection.Input);
            param.Add("IN_TRANSPORTER_ID", conditions.TransporterId, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_DELIVERYNOTE_INVOICE01",
                param,
                commandType: CommandType.StoredProcedure);

            status = param.Get<int>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }

        /// <summary>
        /// Gas/オーダー 新規発行処理
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public void PrintGasOrderMain(PrintEcInvoiceConditions conditions, out int status, out string message)
        {
            status = 0;
            message = "";
            var errstatus = 0;
            if (conditions.EcShipClass == EcShipClasses.Orders)
            {
                CheckErrorOrder(conditions, out errstatus);
            }
            else
            {
                CheckErrorGas(conditions, out errstatus);
            }
            if (errstatus != 0)
            {
                status = errstatus;
                switch (errstatus)
                {
                    case 7:
                        message = Wms.Resources.MessageResource.ERR_DEFECT;
                        break;
                    case 8:
                        message = Resources.PrintEcInvoiceResource.ErrorOutPut;
                        break;
                    case 9:
                        message = Resources.PrintEcInvoiceResource.ErrorNotEcShips;
                        break;
                    case 100:
                        message = Resources.PrintEcInvoiceResource.ErrorCancel;
                        break;
                    case 101:
                        message = Resources.PrintEcInvoiceResource.ErrorChange;
                        break;
                    case 180:
                        message = Resources.PrintEcInvoiceResource.ErrorMessage;
                        break;
                    case 181:
                        message = Resources.PrintEcInvoiceResource.ErrorKenpin;
                        break;
                    case 182:
                        message = Resources.PrintInvoiceResource.ErrorPicComfirm;
                        break;
                    case 11:
                        message = Resources.PrintEcInvoiceResource.ErrorNotBoxNo;
                        break;
                    case 12:
                        message = Resources.PrintEcInvoiceResource.ErrorStockOut;
                        break;
                    case 13:
                        message = Resources.PrintEcInvoiceResource.ErrorResultGas;
                        break;
                    case 1401:
                        message = string.Format(Share.Common.Resources.MessagesResource.Required, "ヤマトの場合箱サイズ(1)");
                        break;
                    case 1402:
                        message = string.Format(Share.Common.Resources.MessagesResource.Required, "ヤマトの場合箱サイズ(2)");
                        break;
                    case 1403:
                        message = string.Format(Share.Common.Resources.MessagesResource.Required, "ヤマトの場合箱サイズ(3)");
                        break;
                    case 1404:
                        message = string.Format(Share.Common.Resources.MessagesResource.Required, "ヤマトの場合箱サイズ(4)");
                        break;
                    case 1405:
                        message = string.Format(Share.Common.Resources.MessagesResource.Required, "ヤマトの場合箱サイズ(5)");
                        break;
                    case 1406:
                        message = string.Format(Share.Common.Resources.MessagesResource.Required, "ヤマトの場合箱サイズ(6)");
                        break;
                    case 1407:
                        message = string.Format(Share.Common.Resources.MessagesResource.Required, "ヤマトの場合箱サイズ(7)");
                        break;
                    case 1408:
                        message = string.Format(Share.Common.Resources.MessagesResource.Required, "ヤマトの場合箱サイズ(8)");
                        break;
                    case 1409:
                        message = string.Format(Share.Common.Resources.MessagesResource.Required, "ヤマトの場合箱サイズ(9)");
                        break;
                    case 1410:
                        message = string.Format(Share.Common.Resources.MessagesResource.Required, "ヤマトの場合箱サイズ(10)");
                        break;
                    case 1501:
                        message =  Resources.PrintEcInvoiceResource.ErrorBoxSize + "（箱サイズ1)"  ;
                        break;
                    case 1502:
                        message = Resources.PrintEcInvoiceResource.ErrorBoxSize + "（箱サイズ2)";
                        break;
                    case 1503:
                        message = Resources.PrintEcInvoiceResource.ErrorBoxSize + "（箱サイズ3)";
                        break;
                    case 1504:
                        message = Resources.PrintEcInvoiceResource.ErrorBoxSize + "（箱サイズ4)";
                        break;
                    case 1505:
                        message = Resources.PrintEcInvoiceResource.ErrorBoxSize + "（箱サイズ5)";
                        break;
                    case 1506:
                        message = Resources.PrintEcInvoiceResource.ErrorBoxSize + "（箱サイズ6)";
                        break;
                    case 1507:
                        message = Resources.PrintEcInvoiceResource.ErrorBoxSize + "（箱サイズ7)";
                        break;
                    case 1508:
                        message = Resources.PrintEcInvoiceResource.ErrorBoxSize + "（箱サイズ8)";
                        break;
                    case 1509:
                        message = Resources.PrintEcInvoiceResource.ErrorBoxSize + "（箱サイズ9)";
                        break;
                    case 1510:
                        message = Resources.PrintEcInvoiceResource.ErrorBoxSize + "（箱サイズ10)";
                        break;
                }
            }
            else
            {
                UpdatePackingInfoGasOrder(conditions, out status, out message);
            }
        }

        /// <summary>
        /// オーダー 新規発行エラーチェック処理
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="errstatus"></param>
        public void CheckErrorOrder(PrintEcInvoiceConditions conditions, out int errstatus)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
                WITH
                    TARGET_PACKING_INFO AS (
                        SELECT
                                MAX(SHIP_INSTRUCT_ID) AS SHIP_INSTRUCT_ID
                            ,   MAX(BATCH_NO) AS BATCH_NO
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                        FROM
                                T_SHIP_PACKING_INFO
                        WHERE
                                BOX_NO = :BOX_NO
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                            AND EC_FLAG = 1
                        GROUP BY
                                BOX_NO
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                )
                SELECT
                        PACK.SHIP_INSTRUCT_ID
                    ,   MAX(PACK.BATCH_NO) AS BATCH_NO
                    ,   MAX(PACK.NOUHIN_PRN_FLAG) AS NOUHIN_PRN_FLAG
                    ,   MAX(PACK.DELI_PRN_FLAG) AS DELI_PRN_FLAG
                    ,   MIN(PACK.KEN_FLAG) AS KEN_FLAG
                    ,   SUM(PACK.RESULT_QTY) AS RESULT_QTY
                    ,   MAX(PACK.TRANSPORTER_ID) AS  TRANSPORTER_ID
                FROM
                        T_SHIP_PACKING_INFO PACK
                INNER JOIN
                        TARGET_PACKING_INFO TGT
                ON
                        PACK.SHIP_INSTRUCT_ID = TGT.SHIP_INSTRUCT_ID
                    AND PACK.CENTER_ID = TGT.CENTER_ID
                    AND PACK.SHIPPER_ID = TGT.SHIPPER_ID
                WHERE
                        PACK.CENTER_ID = :CENTER_ID
                    AND PACK.SHIPPER_ID = :SHIPPER_ID
                    AND PACK.EC_FLAG = 1
                GROUP BY
                        PACK.SHIP_INSTRUCT_ID
            ");
            parameters.AddDynamicParams(new { BOX_NO = conditions.BoxNo });
            parameters.AddDynamicParams(new { CENTER_ID = conditions.CenterId });
            parameters.AddDynamicParams(new { SHIPPER_ID = Profile.User.ShipperId });
            var target_packing_info = MvcDbContext.Current.Database.Connection.Query<PrintEcInvoiceCheck>(query.ToString(), parameters).SingleOrDefault();

            if(target_packing_info == null)
            {
                errstatus = 11;
                return;
            }

            if (target_packing_info.NouhinPrnFlag != 0 || target_packing_info.DeliPrnFlag != 0)
            {
                errstatus = 8;
                return;
            }

            var target_ec_ship = MvcDbContext.Current.Ecships
                .Where(m => m.BatchNo == target_packing_info.BatchNo
                         && m.CenterId == conditions.CenterId
                         && m.ShipperId == Profile.User.ShipperId
                         && m.ShipInstructId == target_packing_info.ShipInstructId)
                .GroupBy(m => m.ShipInstructId)
                .Select(m => new
                {
                    EcShipClass = m.Max(x => x.EcShipClass),
                    MaxAftAllocCancelFlag = m.Max(x => x.AftAllocCancelFlag),
                    MaxAftAllocUpFlag = m.Max(x => x.AftAllocUpFlag),
                    SumAllocQty = m.Sum(x => x.AllocQty)
                })
                .SingleOrDefault();
            if(target_ec_ship == null)
            {
                errstatus = 9;
                return;
            }
            if (target_ec_ship.EcShipClass == 2 && target_ec_ship.SumAllocQty != target_packing_info.ResultQty)
            {
                errstatus = 180;
                return;
            }
            if(target_packing_info.KenFlag == 0 || target_packing_info.KenFlag == 1)
            {
                errstatus = 181;
                return;
            }
            if (target_packing_info.KenFlag == 2)
            {
                errstatus = 182;
                return;
            }
            if (target_ec_ship.MaxAftAllocCancelFlag == 1)
            {
                conditions.ErrShipInstructId = target_packing_info.ShipInstructId;
                errstatus = 100;
                return;
            }
             if(target_ec_ship.MaxAftAllocUpFlag == 1)
             {
                 conditions.ErrShipInstructId = target_packing_info.ShipInstructId;
                 errstatus = 101;
                 return;
             }
            //ヤマトの場合、箱サイズ必須
            if (target_packing_info.TransporterId == "01")
            {
                if (conditions.PrnClass == PrintEcInvoiceConditions.PrnClasses.New )
                {
                    if ( conditions.BoxSize1 == "")
                    {
                        errstatus = 1401;   //ヤマトの場合箱サイズは必須です
                       return;
                    }

                    if (conditions.UnitCnt != "")
                    {
                        int numUnitCnt = Int32.Parse(conditions.UnitCnt);

                        if (numUnitCnt >= 2 && conditions.BoxSize2 == "")
                        {
                            errstatus = 1402;   //ヤマトの場合箱サイズは必須です
                            return;
                        }

                        if (numUnitCnt >= 3 && conditions.BoxSize3 == "")
                        {
                            errstatus = 1403;   //ヤマトの場合箱サイズは必須です
                            return;
                        }

                        if (numUnitCnt >= 4 && conditions.BoxSize4 == "")
                        {
                            errstatus = 1404;   //ヤマトの場合箱サイズは必須です
                            return;
                        }

                        if (numUnitCnt >= 5 && conditions.BoxSize5 == "")
                        {
                            errstatus = 1405;   //ヤマトの場合箱サイズは必須です
                            return;
                        }

                        if (numUnitCnt >= 6 && conditions.BoxSize6 == "")
                        {
                            errstatus = 1406;   //ヤマトの場合箱サイズは必須です
                            return;
                        }

                        if (numUnitCnt >= 7 && conditions.BoxSize7 == "")
                        {
                            errstatus = 1407;   //ヤマトの場合箱サイズは必須です
                            return;
                        }

                        if (numUnitCnt >= 8 && conditions.BoxSize8 == "")
                        {
                            errstatus = 1408;   //ヤマトの場合箱サイズは必須です
                            return;
                        }

                        if (numUnitCnt >= 9 && conditions.BoxSize9 == "")
                        {
                            errstatus = 1409;   //ヤマトの場合箱サイズは必須です
                            return;
                        }

                        if (numUnitCnt >= 10 && conditions.BoxSize10 == "")
                        {
                            errstatus = 1410;   //ヤマトの場合箱サイズは必須です
                            return;
                        }
                    }

                }

                if (conditions.BoxSize1 != "" && CheckBoxSize(conditions.BoxSize1) == false)
                {
                    errstatus = 1501;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize2 != "" && CheckBoxSize(conditions.BoxSize2) == false)
                {
                    errstatus = 1502;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize3 != "" && CheckBoxSize(conditions.BoxSize3) == false)
                {
                    errstatus = 1503;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize4 != "" && CheckBoxSize(conditions.BoxSize4) == false)
                {
                    errstatus = 1504;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize5 != "" && CheckBoxSize(conditions.BoxSize5) == false)
                {
                    errstatus = 1505;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize6 != "" && CheckBoxSize(conditions.BoxSize6) == false)
                {
                    errstatus = 1506;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize7 != "" && CheckBoxSize(conditions.BoxSize7) == false)
                {
                    errstatus = 1507;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize8 != "" && CheckBoxSize(conditions.BoxSize8) == false)
                {
                    errstatus = 1508;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize9 != "" && CheckBoxSize(conditions.BoxSize9) == false)
                {
                    errstatus = 1509;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize10 != "" && CheckBoxSize(conditions.BoxSize10) == false)
                {
                    errstatus = 1510;   //ヤマト箱サイズが不正です
                    return;
                }

            }
            conditions.ShipInstructId = target_packing_info.ShipInstructId;
            conditions.TransporterId = target_packing_info.TransporterId;

            errstatus = 0;
        }

        /// <summary>
        /// GAS 新規発行エラーチェック処理
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="errstatus"></param>
        public void CheckErrorGas(PrintEcInvoiceConditions conditions, out int errstatus)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
                WITH
                    SELECTED_GAS AS (
                        SELECT
                                GAS.SHIP_INSTRUCT_ID
                            ,   SUM(GAS.GAS_QTY) AS GAS_QTY
                            ,   SUM(GAS.STOCK_OUT_FIX_QTY) AS STOCK_OUT_FIX_QTY
                            ,   GAS.CENTER_ID
                            ,   GAS.SHIPPER_ID
                            ,   MAX(NVL(PACK.BOX_NO, N' ')) AS PACK_BOX_NO
                            ,   MAX(NVL(TO_CHAR(TO_DATE(TRIM(GAS.GAS_DATE)),'YYYY/MM/DD'), '9999/99/99')) AS GAS_DATE
                        FROM
                                T_GAS GAS
                        LEFT OUTER JOIN
                                T_SHIP_PACKING_INFO PACK
                        ON
                                GAS.BOX_NO = PACK.BOX_NO
                            AND GAS.CENTER_ID = PACK.CENTER_ID
                            AND GAS.SHIPPER_ID = PACK.SHIPPER_ID
                        WHERE
                                GAS.BOX_NO = :BOX_NO
                            AND GAS.CENTER_ID = :CENTER_ID
                            AND GAS.SHIPPER_ID = :SHIPPER_ID
                        GROUP BY
                                GAS.SHIP_INSTRUCT_ID
                            ,   GAS.CENTER_ID
                            ,   GAS.SHIPPER_ID
                )
                SELECT
                        EC.SHIP_INSTRUCT_ID
                    ,   MAX(EC.AFT_ALLOC_CANCEL_FLAG)   AS AFT_ALLOC_CANCEL_FLAG
                    ,   MAX(EC.AFT_ALLOC_UP_FLAG)       AS AFT_ALLOC_UP_FLAG
                    ,   MAX(TGT.GAS_QTY)                AS GAS_QTY
                    ,   MAX(TGT.STOCK_OUT_FIX_QTY)      AS STOCK_OUT_FIX_QTY
                    ,   NVL(MAX(TGT.PACK_BOX_NO), N' ') AS PACK_BOX_NO
                    ,   SUM(EC.ALLOC_QTY)               AS EC_ALLOC_QTY
                    ,   MAX(GAS_DATE)                   AS GAS_DATE
                    ,   MAX(EC.TRANSPORTER_ID)          AS TRANSPORTER_ID
                FROM
                        T_ECSHIPS EC
                INNER JOIN
                        SELECTED_GAS TGT
                ON
                        EC.SHIP_INSTRUCT_ID = TGT.SHIP_INSTRUCT_ID
                    AND EC.CENTER_ID = TGT.CENTER_ID
                    AND EC.SHIPPER_ID = TGT.SHIPPER_ID
                GROUP BY
                        EC.SHIP_INSTRUCT_ID
            ");
            parameters.AddDynamicParams(new { BOX_NO = conditions.BoxNo });
            parameters.AddDynamicParams(new { CENTER_ID = conditions.CenterId });
            parameters.AddDynamicParams(new { SHIPPER_ID = Profile.User.ShipperId });
            var target_gas_data = MvcDbContext.Current.Database.Connection.Query<PrintEcInvoiceCheck>(query.ToString(), parameters).SingleOrDefault();

            if (target_gas_data == null)
            {
                errstatus = 11; //該当ケースなし
                return;
            }
            if(target_gas_data.GasDate == "9999/99/99") //実績なし
            {
                errstatus = 13;
                return;
            }
            if (target_gas_data.AftAllocCancelFlag == 1)
            {
                conditions.ErrShipInstructId = target_gas_data.ShipInstructId;
                errstatus = 100;
                return;
            }
            if (target_gas_data.AftAllocUpFlag == 1)
            {
                conditions.ErrShipInstructId = target_gas_data.ShipInstructId;
                errstatus = 101;
                return;
            }
            if (target_gas_data.PackBoxNo.Trim() != "")
            {
                errstatus = 8; //すでに納品書発行済
                return;
            }
            if(target_gas_data.EcAllocQty != target_gas_data.GasQty)
            {
                errstatus = 12; //欠品あり
                return;
            }
 
            //ヤマトの場合、箱サイズ必須
            if (target_gas_data.TransporterId == "01")
            {
                if (conditions.PrnClass == PrintEcInvoiceConditions.PrnClasses.New)
                {
                    if (conditions.BoxSize1 == "")
                    {
                        errstatus = 1401;   //ヤマトの場合箱サイズは必須です
                        return;
                    }

                    if (conditions.UnitCnt != "")
                    {
                        int numUnitCnt = Int32.Parse(conditions.UnitCnt);

                        if (numUnitCnt >= 2 && conditions.BoxSize2 == "")
                        {
                            errstatus = 1402;   //ヤマトの場合箱サイズは必須です
                            return;
                        }

                        if (numUnitCnt >= 3 && conditions.BoxSize3 == "")
                        {
                            errstatus = 1403;   //ヤマトの場合箱サイズは必須です
                            return;
                        }

                        if (numUnitCnt >= 4 && conditions.BoxSize4 == "")
                        {
                            errstatus = 1404;   //ヤマトの場合箱サイズは必須です
                            return;
                        }

                        if (numUnitCnt >= 5 && conditions.BoxSize5 == "")
                        {
                            errstatus = 1405;   //ヤマトの場合箱サイズは必須です
                            return;
                        }

                        if (numUnitCnt >= 6 && conditions.BoxSize6 == "")
                        {
                            errstatus = 1406;   //ヤマトの場合箱サイズは必須です
                            return;
                        }

                        if (numUnitCnt >= 7 && conditions.BoxSize7 == "")
                        {
                            errstatus = 1407;   //ヤマトの場合箱サイズは必須です
                            return;
                        }

                        if (numUnitCnt >= 8 && conditions.BoxSize8 == "")
                        {
                            errstatus = 1408;   //ヤマトの場合箱サイズは必須です
                            return;
                        }

                        if (numUnitCnt >= 9 && conditions.BoxSize9 == "")
                        {
                            errstatus = 1409;   //ヤマトの場合箱サイズは必須です
                            return;
                        }

                        if (numUnitCnt >= 10 && conditions.BoxSize10 == "")
                        {
                            errstatus = 1410;   //ヤマトの場合箱サイズは必須です
                            return;
                        }
                    }

                }

                if (conditions.BoxSize1 != "" && CheckBoxSize(conditions.BoxSize1) == false)
                {
                    errstatus = 1501;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize2 != "" && CheckBoxSize(conditions.BoxSize2) == false)
                {
                    errstatus = 1502;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize3 != "" && CheckBoxSize(conditions.BoxSize3) == false)
                {
                    errstatus = 1503;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize4 != "" && CheckBoxSize(conditions.BoxSize4) == false)
                {
                    errstatus = 1504;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize5 != "" && CheckBoxSize(conditions.BoxSize5) == false)
                {
                    errstatus = 1505;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize6 != "" && CheckBoxSize(conditions.BoxSize6) == false)
                {
                    errstatus = 1506;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize7 != "" && CheckBoxSize(conditions.BoxSize7) == false)
                {
                    errstatus = 1507;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize8 != "" && CheckBoxSize(conditions.BoxSize8) == false)
                {
                    errstatus = 1508;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize9 != "" && CheckBoxSize(conditions.BoxSize9) == false)
                {
                    errstatus = 1509;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize10 != "" && CheckBoxSize(conditions.BoxSize10) == false)
                {
                    errstatus = 1510;   //ヤマト箱サイズが不正です
                    return;
                }
            }

            conditions.ShipInstructId = target_gas_data.ShipInstructId;
            conditions.TransporterId = target_gas_data.TransporterId;

            errstatus = 0;
        }


        /// <summary>
        /// Gas/オーダー新規発行前処理
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public void UpdatePackingInfoGasOrder(PrintEcInvoiceConditions conditions, out int status, out string message)
        {
            var strStoredName = "";
            if (conditions.EcShipClass == EcShipClasses.Orders)
            {
                strStoredName = "SP_W_SHP_DELIVERYNOTE_INVOICE03";
            }
            else
            {
                strStoredName = "SP_W_SHP_DELIVERYNOTE_INVOICE05";
            }
            int? intUnitCnt = null;
            if (conditions.UnitCnt != "")
            {
                intUnitCnt = int.Parse(conditions.UnitCnt);
            }
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", conditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_NO", conditions.BoxNo, DbType.String, ParameterDirection.Input);
            param.Add("IN_UNIT_CNT", intUnitCnt, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_SHIP_INSTRUCT_ID", conditions.ShipInstructId, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE", conditions.BoxSize1, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE2", conditions.BoxSize2, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE3", conditions.BoxSize3, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE4", conditions.BoxSize4, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE5", conditions.BoxSize5, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE6", conditions.BoxSize6, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE7", conditions.BoxSize7, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE8", conditions.BoxSize8, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE9", conditions.BoxSize9, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE10", conditions.BoxSize10, DbType.String, ParameterDirection.Input);
            param.Add("IN_TRANSPORTER_ID", conditions.TransporterId, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                strStoredName,
                param,
                commandType: CommandType.StoredProcedure);

            status = param.Get<int>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }

        /// <summary>
        /// 再発行処理
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public void PrintReissueMain(PrintEcInvoiceConditions conditions, out int status, out string message)
        {
            status = 0;
            message = "";
            var errstatus = 0;
            CheckErrorReissue(conditions, out errstatus);
            if (errstatus != 0)
            {
                status = errstatus;
                switch (errstatus)
                {
                    case 13:
                        message = Resources.PrintEcInvoiceResource.ErrorNotEcShips;
                        break;
                    case 131:
                        message = Resources.PrintEcInvoiceResource.ErrorNotShipInstructId;
                        break;
                    case 140:
                        message = Resources.PrintEcInvoiceResource.ErrorOutPutNouhin;
                        break;
                    case 141:
                        message = Resources.PrintEcInvoiceResource.ErrorOutPutDeil;
                        break;
                    case 142:
                        message = Resources.PrintInvoiceResource.ErrNotDeliNo;
                        break;
                    case 15:
                        message = Wms.Resources.MessageResource.ERR_DEFECT;
                        break;
                    case 1601:
                        message = Resources.PrintEcInvoiceResource.ErrorBoxSize + "（箱サイズ1)";
                        break;
                    case 1602:
                        message = Resources.PrintEcInvoiceResource.ErrorBoxSize + "（箱サイズ2)";
                        break;
                    case 1603:
                        message = Resources.PrintEcInvoiceResource.ErrorBoxSize + "（箱サイズ3)";
                        break;
                    case 1604:
                        message = Resources.PrintEcInvoiceResource.ErrorBoxSize + "（箱サイズ4)";
                        break;
                    case 1605:
                        message = Resources.PrintEcInvoiceResource.ErrorBoxSize + "（箱サイズ5)";
                        break;
                    case 1606:
                        message = Resources.PrintEcInvoiceResource.ErrorBoxSize + "（箱サイズ6)";
                        break;
                    case 1607:
                        message = Resources.PrintEcInvoiceResource.ErrorBoxSize + "（箱サイズ7)";
                        break;
                    case 1608:
                        message = Resources.PrintEcInvoiceResource.ErrorBoxSize + "（箱サイズ8)";
                        break;
                    case 1609:
                        message = Resources.PrintEcInvoiceResource.ErrorBoxSize + "（箱サイズ9)";
                        break;
                    case 1610:
                        message = Resources.PrintEcInvoiceResource.ErrorBoxSize + "（箱サイズ10)";
                        break;
                    case 180:
                        message = Resources.PrintEcInvoiceResource.ErrorMessage;
                        break;
                }
            };

            if(errstatus == 0 && conditions.PrnClass == PrnClasses.ReDeliPrn)
            {
                UpdatePackingInfoReissue(conditions, out status, out message);
            }
        }

        /// <summary>
        /// 再発行エラーチェック処理
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="errstatus"></param>
        public void CheckErrorReissue(PrintEcInvoiceConditions conditions, out int errstatus)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
                WITH
                    SELECTED_PACKING_DATA AS (
                        SELECT
                                CENTER_ID 
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   BOX_NO
                            ,   EC_CLASS
                            ,   DELI_NO
                            ,   BATCH_NO
                            ,   KEN_FLAG
                            ,   NOUHIN_PRN_FLAG
                            ,   DELI_PRN_FLAG
                            ,   TRANSPORTER_ID
                        FROM
                                T_SHIP_PACKING_INFO
                        WHERE
                                CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                            AND EC_FLAG = 1
            ");

            if (!string.IsNullOrEmpty(conditions.BoxNo))
            {
                query.AppendLine(@"  AND BOX_NO = :BOX_NO ");
                parameters.AddDynamicParams(new { BOX_NO = conditions.BoxNo });
            }

            if (!string.IsNullOrEmpty(conditions.DeliNo))
            {
                query.AppendLine(@" AND 1 = CASE WHEN DELI_NO = :DELI_NO THEN 1 WHEN SF_GET_DELI_NO_EXIST(CENTER_ID,SHIPPER_ID,SHIP_INSTRUCT_ID,:DELI_NO) = 1 THEN 1 ELSE 0 END ");
                parameters.AddDynamicParams(new { DELI_NO = conditions.DeliNo });
            }

            query.AppendLine(@"
                    )
            ");

            if (conditions.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                    , SELECTED_APACKING_DATA AS(
                        SELECT
                                CENTER_ID 
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   BOX_NO
                            ,   EC_CLASS
                            ,   DELI_NO
                            ,   BATCH_NO
                            ,   KEN_FLAG
                            ,   NOUHIN_PRN_FLAG
                            ,   DELI_PRN_FLAG
                            ,   TRANSPORTER_ID

                        FROM
                                A_SHIP_PACKING_INFO
                        WHERE
                                CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                            AND EC_FLAG = 1
                ");
                if (!string.IsNullOrEmpty(conditions.BoxNo))
                {
                    query.AppendLine(@"  AND BOX_NO = :BOX_NO ");
                }

                if (!string.IsNullOrEmpty(conditions.DeliNo))
                {
                    query.AppendLine(@"  AND  1 = CASE WHEN DELI_NO = :DELI_NO THEN 1 WHEN SF_GET_DELI_NO_EXIST(CENTER_ID,SHIPPER_ID,SHIP_INSTRUCT_ID,:DELI_NO) = 1 THEN 1 ELSE 0 END  ");
                }


                query.AppendLine(@"
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
                                CENTER_ID 
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   BOX_NO
                            ,   EC_CLASS
                            ,   DELI_NO
                            ,   BATCH_NO
                            ,   KEN_FLAG
                            ,   NOUHIN_PRN_FLAG
                            ,   DELI_PRN_FLAG
                            ,   TRANSPORTER_ID
                FROM
           ");
            if (conditions.ChkOldData == true)  //過去分含む
            {
                query.AppendLine(@"
                        SELECTED_ALL_PACKING_DATA 
                ");
            }
            else
            {
                query.AppendLine(@"
                        SELECTED_PACKING_DATA  
                ");
            }

            parameters.AddDynamicParams(new { CENTER_ID = conditions.CenterId });
            parameters.AddDynamicParams(new { SHIPPER_ID = Profile.User.ShipperId });
            var target_packing_info_box = MvcDbContext.Current.Database.Connection.Query<ShipPackingInfo>(query.ToString(), parameters);

            if (target_packing_info_box.Count() == 0)
            {
                errstatus = 13; //ケースNo
                return;
            }
            var target_packing_info = target_packing_info_box
                .Where(m => m.ShipInstructId == (conditions.ShipInstructId == "" || conditions.ShipInstructId == null ? m.ShipInstructId : conditions.ShipInstructId))
                .GroupBy(m => m.ShipInstructId)
                .Select(m => new
                {
                    ShipInstructId = m.Max(x => x.ShipInstructId),
                    TransporterId = m.Max(x => x.TransporterId),
                    BatchNo = m.Max(x => x.BatchNo),
                    MinNouhinPrnFlag = m.Min(x => x.NouhinPrnFlag),
                    MinDeliPrnFlag = m.Min(x => x.DeliPrnFlag),
                    MinKenpinFlag = m.Min(x => x.KenFlag),
                    MaxDeliNo = m.Max(x => x.DeliNo),
                })
                .SingleOrDefault();
            if (target_packing_info == null)
            {
                errstatus = 131; //注文番号もしくは送り状番号が存在しない
                return;
            }
            if (conditions.PrnClass == PrnClasses.ReNouhinPrn && target_packing_info.MinNouhinPrnFlag != 1 && target_packing_info.MinNouhinPrnFlag != 9)
            {
                errstatus = 140;
                return;
            }
            if(conditions.PrnClass == PrnClasses.ReDeliPrn && target_packing_info.MinDeliPrnFlag == 0)
            {
                errstatus = 141;
                return;
            }
            if (conditions.PrnClass == PrnClasses.ReDeliPrn && target_packing_info.MaxDeliNo.Trim() == "")
            {
                errstatus = 142;
                return;
            }

            //出荷指示ID単位の実績数合計取得
            StringBuilder query_sum = new StringBuilder();
            query_sum.AppendLine(@"
                WITH
                    SELECTED_PACKING_DATA AS (
                        SELECT
                                CENTER_ID 
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   RESULT_QTY
                        FROM
                                T_SHIP_PACKING_INFO
                        WHERE
                                SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                            AND EC_FLAG = 1
                )
            ");

            if (conditions.ChkOldData == true)  //過去分含む
            {
                query_sum.AppendLine(@"
                    , SELECTED_APACKING_DATA AS(
                        SELECT
                                CENTER_ID 
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   RESULT_QTY
                        FROM
                                A_SHIP_PACKING_INFO
                        WHERE
                                SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                            AND EC_FLAG = 1
                    )
                    , SELECTED_ALL_PACKING_DATA AS(
                            SELECT * FROM SELECTED_PACKING_DATA
                            UNION
                            SELECT * FROM SELECTED_APACKING_DATA
                    )
            ");
            }
            query_sum.AppendLine(@"
                SELECT
                             SUM(RESULT_QTY) AS SUM_RESULT_QTY

                FROM
           ");
            if (conditions.ChkOldData == true)  //過去分含む
            {
                query_sum.AppendLine(@"
                        SELECTED_ALL_PACKING_DATA 
            ");
            }
            else
            {
                query_sum.AppendLine(@"
                        SELECTED_PACKING_DATA  
            ");
            }
            query_sum.AppendLine(@"
                        GROUP BY
                                SHIP_INSTRUCT_ID
            ");


            parameters.AddDynamicParams(new { SHIP_INSTRUCT_ID = target_packing_info.ShipInstructId });
            var sum_packing_info = MvcDbContext.Current.Database.Connection.Query(query_sum.ToString(), parameters).SingleOrDefault();


            //出荷指示ID単位の実績数合計取得
            StringBuilder query_ecships = new StringBuilder();

            query_ecships.AppendLine(@"
                WITH
                    SELECTED_ECSHIPS_DATA AS (
                        SELECT
                                CENTER_ID 
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   BATCH_NO 
                            ,   EC_SHIP_CLASS
                            ,   ALLOC_QTY
                        FROM
                                T_ECSHIPS
                        WHERE
                                SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND BATCH_NO = :BATCH_NO
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                )
            ");

            if (conditions.ChkOldData == true)  //過去分含む
            {
                query_ecships.AppendLine(@"
                    , SELECTED_AECSHIPS_DATA AS(
                        SELECT
                                CENTER_ID 
                            ,   SHIPPER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   BATCH_NO 
                            ,   EC_SHIP_CLASS
                            ,   ALLOC_QTY
                        FROM
                                A_ECSHIPS
                        WHERE
                                SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
                            AND BATCH_NO = :BATCH_NO
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
            query_ecships.AppendLine(@"
                SELECT
                             MAX(EC_SHIP_CLASS) AS EC_SHIP_CLASS
                         ,   SUM(ALLOC_QTY) AS SUM_ALLOC_QTY

                FROM
           ");
            if (conditions.ChkOldData == true)  //過去分含む
            {
                query_ecships.AppendLine(@"
                        SELECTED_ALL_ECSHIPS_DATA 
            ");
            }
            else
            {
                query_ecships.AppendLine(@"
                        SELECTED_ECSHIPS_DATA  
            ");
            }
            query_ecships.AppendLine(@"
                        GROUP BY
                                SHIP_INSTRUCT_ID
            ");


            parameters.AddDynamicParams(new { BATCH_NO = target_packing_info.BatchNo });
            var target_ec_ship = MvcDbContext.Current.Database.Connection.Query(query_ecships.ToString(), parameters).SingleOrDefault();

            if (target_ec_ship == null)
            {
                errstatus = 15;
                return;
            }
            if (target_ec_ship.EcShipClass == 2 && target_ec_ship.SumAllocQty != sum_packing_info.SumResultQty)
            {
                errstatus = 180;
                return;
            }

            //ヤマトの場合、箱サイズ必須
            if (target_packing_info.TransporterId == "01")
            {

                if (conditions.BoxSize1 != "" && CheckBoxSize(conditions.BoxSize1) == false)
                {
                    errstatus = 1601;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize2 != "" && CheckBoxSize(conditions.BoxSize2) == false)
                {
                    errstatus = 1602;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize3 != "" && CheckBoxSize(conditions.BoxSize3) == false)
                {
                    errstatus = 1603;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize4 != "" && CheckBoxSize(conditions.BoxSize4) == false)
                {
                    errstatus = 1604;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize5 != "" && CheckBoxSize(conditions.BoxSize5) == false)
                {
                    errstatus = 1605;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize6 != "" && CheckBoxSize(conditions.BoxSize6) == false)
                {
                    errstatus = 1606;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize7 != "" && CheckBoxSize(conditions.BoxSize7) == false)
                {
                    errstatus = 1607;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize8 != "" && CheckBoxSize(conditions.BoxSize8) == false)
                {
                    errstatus = 1608;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize9 != "" && CheckBoxSize(conditions.BoxSize9) == false)
                {
                    errstatus = 1609;   //ヤマト箱サイズが不正です
                    return;
                }

                if (conditions.BoxSize10 != "" && CheckBoxSize(conditions.BoxSize10) == false)
                {
                    errstatus = 1610;   //ヤマト箱サイズが不正です
                    return;
                }

            }

            conditions.ShipInstructId = target_packing_info.ShipInstructId;
            conditions.TransporterId = target_packing_info.TransporterId;

            errstatus = 0;

        }

        /// <summary>
        /// 再発行前処理
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public void UpdatePackingInfoReissue(PrintEcInvoiceConditions conditions, out int status, out string message)
        {
            int? intUnitCnt = null;
            if(conditions.UnitCnt != "")
            {
                intUnitCnt = int.Parse(conditions.UnitCnt);
            }
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", conditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_NO", conditions.BoxNo, DbType.String, ParameterDirection.Input);
            param.Add("IN_SHIP_INSTRUCT_ID", conditions.ShipInstructId, DbType.String, ParameterDirection.Input);
            param.Add("IN_UNIT_CNT", intUnitCnt, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE", conditions.BoxSize1, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE2", conditions.BoxSize2, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE3", conditions.BoxSize3, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE4", conditions.BoxSize4, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE5", conditions.BoxSize5, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE6", conditions.BoxSize6, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE7", conditions.BoxSize7, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE8", conditions.BoxSize8, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE9", conditions.BoxSize9, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE10", conditions.BoxSize10, DbType.String, ParameterDirection.Input);
            param.Add("IN_TRANSPORTER_ID", conditions.TransporterId, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_DELIVERYNOTE_INVOICE02",
                param,
                commandType: CommandType.StoredProcedure);

            status = param.Get<int>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }


        /// <summary>
        /// バッチNo取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public string GetBatchName(string centerId, string BatchNo)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            var batch_name = string.Empty;
            var batch_data = MvcDbContext.Current.AllocInfos
                .Where(m => m.AllocNo == BatchNo
                         && m.CenterId == centerId
                         && m.ShipperId == Profile.User.ShipperId)
                .Select(m => new { BatchName = m.BatchName, BatchNo = m.AllocNo } )
                .SingleOrDefault();

            if (batch_data == null)
            {
                batch_name = "";
            }
            else
            {
                batch_name = batch_data.BatchName;
            }

            return batch_name;
        }

        /// <summary>
        /// ヤマト箱サイズチェック
        /// </summary>
        /// <param name="box_size"></param>
        /// <returns></returns>
        public bool CheckBoxSize(string box_size)
        {
            var size_gen_data = MvcDbContext.Current.Generals
                .Where(m => m.RegisterDiviCd == "1"
                        && m.GenDivCd == "YAMATO_BOX_SIZE"
                        && m.GenCd == box_size
                        && m.CenterId == "@@@"
                        && m.ShipperId == Profile.User.ShipperId)
                .ToList();
            if (size_gen_data.Count == 0)
            {
                return false;
            }
            return true;
        }

    }
}