namespace Wms.Areas.Ship.Query.PrintInvoice
{
    using Dapper;
    using Share.Common;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using Wms.Areas.Ship.ViewModels.PrintInvoice;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Ship.ViewModels.PrintInvoice.ZanShipStore;

    public class PrintInvoiceQuery
    {
        public void PrintBeforeMain(PrintInvoiceConditions condition, out int status, out string message)
        {
            //データチェック
            CheckErrorPrint(condition, out int errstatus);

            if (errstatus != 0)
            {
                status = errstatus;

                switch (errstatus)
                {
                    case 1:
                        message = Wms.Resources.MessageResource.ERR_DEFECT;
                        break;
                    case 2:
                        message = Resources.PrintInvoiceResource.ErrorOutPut;
                        break;
                    case 5:
                        message = Resources.PrintInvoiceResource.ErrorReNouhin;
                        break;
                    case 51:
                        message = Resources.PrintInvoiceResource.ErrNotPrnNouhin;
                        break;
                    case 52:
                        message = Resources.PrintInvoiceResource.ErrNotPrnNouhinCom;
                        break;
                    case 6:
                        message = Resources.PrintInvoiceResource.ErrorTransporter;
                        break;
                    case 7:
                        message = Resources.PrintInvoiceResource.ErrorReDeli;
                        break;
                    case 8:
                        message = Resources.PrintInvoiceResource.ErrNotDeliNo;
                        break;
                    case 10:
                        message = Resources.PrintInvoiceResource.ErrorNotPic;
                        break;
                    case 11:
                        message = Resources.PrintInvoiceResource.ErrorPicComfirm;
                        break;
                    case 20:
                        message = Resources.PrintInvoiceResource.ErrorAllCancel;
                        break;
                    case 21:
                        message = Resources.PrintInvoiceResource.ErrorCancel;
                        break;
                    case 30:
                        message = string.Format(Share.Common.Resources.MessagesResource.Required, "ヤマトの場合箱サイズ");
                        break;
                    case 31:
                        message = Resources.PrintInvoiceResource.ErrorBoxSize;
                        break;
                    case 32:
                        message = Resources.PrintInvoiceResource.ErrorClientCdSagawa;
                        break;
                    case 100:
                        message = Resources.PrintInvoiceResource.ErrorNotExist;
                        break;
                    case 110:
                        message = Resources.PrintInvoiceResource.ErrorStoreInv;
                        break;
                    case 801:
                        message = Resources.PrintInvoiceResource.AltSagawaDoneEdi;
                        break;
                    case 802:
                        message = Resources.PrintInvoiceResource.AltSagawaNotYetEdi;
                        break;
                    default:
                        message = "";
                        break;
                }
            }
            else
            {
                //出荷梱包実績データ更新
                UpdatePackingInfo(condition, condition.BoxNo, condition.BoxSize, condition.TransporterId, out status, out message);
            }
        }

        public void CheckErrorPrint(PrintInvoiceConditions condition, out int status)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
                WITH
                   SELECTED_SHIP_PACK AS (
                        SELECT
                                BOX_NO
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_INSTRUCT_SEQ
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   TRANSPORTER_ID
                            ,   KEN_FLAG
                            ,   NOUHIN_PRN_FLAG
                            ,   DELI_PRN_FLAG
                            ,   DELI_NO
                            ,   SHIP_TO_STORE_ID
                            ,   IF_STATE_TRP
                            ,   CLIENT_CD
                        FROM
                                T_SHIP_PACKING_INFO
                        WHERE
                                EC_FLAG = 0
                            AND SHIPPER_ID = :SHIPPER_ID
            ");
            if (!string.IsNullOrEmpty(condition.BoxNo))
            {
                query.AppendLine(@" AND BOX_NO = :BOX_NO ");
                parameters.AddDynamicParams(new { BOX_NO = condition.BoxNo });
            }
            if (!string.IsNullOrEmpty(condition.DeliNo))
            {
                query.AppendLine(@" AND DELI_NO = :DELI_NO ");
                parameters.AddDynamicParams(new { DELI_NO = condition.DeliNo });
            }
            if (condition.ChkOldData == true)
            {
                query.AppendLine(@"
                    )
                    ,   SELECTED_ASHIP_PACK AS (
                            SELECT
                                    BOX_NO
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   CENTER_ID
                                ,   SHIPPER_ID
                                ,   TRANSPORTER_ID
                                ,   KEN_FLAG
                                ,   NOUHIN_PRN_FLAG
                                ,   DELI_PRN_FLAG
                                ,   DELI_NO
                                ,   SHIP_TO_STORE_ID
                                ,   IF_STATE_TRP
                                ,   CLIENT_CD
                            FROM
                                    A_SHIP_PACKING_INFO
                            WHERE
                                    EC_FLAG = 0
                                AND SHIPPER_ID = :SHIPPER_ID
                ");
                if (!string.IsNullOrEmpty(condition.BoxNo))
                {
                    query.AppendLine(@" AND BOX_NO = :BOX_NO ");
                    parameters.AddDynamicParams(new { BOX_NO = condition.BoxNo });
                }
                if (!string.IsNullOrEmpty(condition.DeliNo))
                {
                    query.AppendLine(@" AND DELI_NO = :DELI_NO ");
                    parameters.AddDynamicParams(new { DELI_NO = condition.DeliNo });
                }
                query.AppendLine(@"
                    )
                    ,   SELECTED_ALL_PACK AS (
                            SELECT * FROM SELECTED_SHIP_PACK
                            UNION
                            SELECT * FROM SELECTED_ASHIP_PACK
                    )
                    ,   SELECTED_SHIP_ALL AS (
                            SELECT
                                    CENTER_ID
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   SHIPPER_ID
                                ,   AFT_ALLOC_STOP_FLAG
                            FROM
                                    T_SHIPS
                            WHERE
                                    SHIPPER_ID = :SHIPPER_ID
                            UNION
                            SELECT
                                    CENTER_ID
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   SHIPPER_ID
                                ,   AFT_ALLOC_STOP_FLAG
                            FROM
                                    A_SHIPS
                            WHERE
                                    SHIPPER_ID = :SHIPPER_ID
                ");
            }
            query.AppendLine(@"
                    )
                SELECT
                        ROWNUM AS ROW_NO
                    ,   SHIP.CENTER_ID
                    ,   SHIP.SHIP_INSTRUCT_ID
                    ,   SHIP.SHIP_INSTRUCT_SEQ
                    ,   SHIP.SHIPPER_ID
                    ,   SHIP.AFT_ALLOC_STOP_FLAG
                    ,   PACK.BOX_NO
                    ,   PACK.TRANSPORTER_ID
                    ,   PACK.KEN_FLAG
                    ,   PACK.NOUHIN_PRN_FLAG
                    ,   PACK.DELI_PRN_FLAG
                    ,   NVL(PACK.DELI_NO, N' ') AS DELI_NO
                    ,   TRAN.INVOICE_PRINT_FLAG
                    ,   CASE WHEN INV.STORE_ID IS NOT NULL THEN '1' ELSE '0' END AS STORE_INV_FLAG
                    ,   SF_GET_CLIENT_CD_TRANSPORTER(:SHIPPER_ID, :USER_CENTER_ID, TRUNC(SYSDATE), PACK.SHIP_TO_STORE_ID, PACK.TRANSPORTER_ID) AS NEW_CLIENT_CD
                    ,   PACK.IF_STATE_TRP
                    ,   PACK.CLIENT_CD
                    ,   PACK.SHIP_TO_STORE_ID
                FROM
            ");
            if (condition.ChkOldData == true)
            {
                query.AppendLine(@"
                            SELECTED_SHIP_ALL SHIP
                    INNER JOIN
                            SELECTED_ALL_PACK PACK
                ");
            }
            else
            {
                query.AppendLine(@"
                            T_SHIPS SHIP
                    INNER JOIN
                            SELECTED_SHIP_PACK PACK
                ");
            }
            query.AppendLine(@"

                ON
                        SHIP.SHIP_INSTRUCT_ID = PACK.SHIP_INSTRUCT_ID
                    AND SHIP.SHIP_INSTRUCT_SEQ = PACK.SHIP_INSTRUCT_SEQ
                    AND SHIP.CENTER_ID = PACK.CENTER_ID
                    AND SHIP.SHIPPER_ID = PACK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_TRANSPORTERS TRAN
                ON
                        TRAN.TRANSPORTER_ID = PACK.TRANSPORTER_ID
                    AND TRAN.SHIPPER_ID = PACK.SHIPPER_ID
                LEFT OUTER JOIN
                        M_STORE_INV INV
                ON
                        INV.STORE_ID = PACK.SHIP_TO_STORE_ID
                    AND INV.CENTER_ID = PACK.CENTER_ID
                    AND INV.SHIPPER_ID = PACK.SHIPPER_ID
                    AND INV.STORE_INV_STATUS = '2'
            ");
            parameters.AddDynamicParams(new { SHIPPER_ID = Profile.User.ShipperId, USER_CENTER_ID = condition.UserCenterId });

            var target_ship_data = MvcDbContext.Current.Database.Connection.Query<PrintInvoiceCheck>(query.ToString(), parameters).ToList();

            if (target_ship_data.Count == 0)
            {
                status = 100;
                return;
            }

            var ship_packing_data = target_ship_data
                .GroupBy(m => m.BoxNo)
                .Select(m => new
                {
                    TransporterId = m.Max(x => x.TransporterId.ToString()),
                    NouhinPrnFlag = m.Max(x => x.NouhinPrnFlag.ToString()),
                    DeliPrnFlag = m.Max(x => x.DeliPrnFlag.ToString()),
                    KenFlag = m.Min(x => x.KenFlag.ToString()),
                    InvoicePrintFlag = m.Max(x => x.InvoicePrintFlag),
                    MaxAftAllocStopFlag = m.Max(x => x.AftAllocStopFlag),
                    MinAftAllocStopFlag = m.Min(x => x.AftAllocStopFlag),
                    DeliNo = m.Max(x => x.DeliNo.ToString()),
                    BoxNo = m.Max(x => x.BoxNo.ToString()),
                    StoreInvFlag = m.Max(x => x.StoreInvFlag.ToString()),
                    CenterId = m.Max(x => x.CenterId.ToString()),
                    NewClientCd = m.Max(x => x.NewClientCd),
                    IfStateTrp = m.Max(x => x.IfStateTrp),
                    ClientCd = m.Max(x => x.ClientCd),
                    ShipToStoreId = m.Max(x => x.ShipToStoreId)
                })
                .SingleOrDefault();

            condition.TransporterId = ship_packing_data.TransporterId;
            condition.InvoicePrintFlag = ship_packing_data.InvoicePrintFlag;
            condition.BoxNo = ship_packing_data.BoxNo;
            condition.NouhinPrnFlag = ship_packing_data.NouhinPrnFlag;
            condition.CenterId = ship_packing_data.CenterId;

            //共通
            if (ship_packing_data is null)
            {
                status = 1; //"データ不具合が発生しています。管理者に連絡してください。";
                return;
            };
            if (ship_packing_data.KenFlag == "0" || ship_packing_data.KenFlag == "1")
            {
                status = 10;    //未検品or検品中
                return;
            }
            if (ship_packing_data.KenFlag == "2")
            {
                status = 11;    //検品不足確認中
                return;
            }

            if (ship_packing_data.MaxAftAllocStopFlag == 1 && ship_packing_data.MinAftAllocStopFlag == 1)
            {
                status = 20;   //全数キャンセル
                return;
            }
            if (ship_packing_data.MaxAftAllocStopFlag == 1 && ship_packing_data.MinAftAllocStopFlag == 0)
            {
                status = 21;   //一部キャンセル
                return;
            }

            //新規発行
            if (condition.PrnClass == PrintInvoiceConditions.PrnClasses.New)
            {
                if (ship_packing_data.NouhinPrnFlag == "1" || ship_packing_data.NouhinPrnFlag == "9" || ship_packing_data.DeliPrnFlag != "0")
                {
                    status = 2;   //"納品書・送り状はすでに発行されています。";
                    return;
                }
                if (ship_packing_data.StoreInvFlag == "1")
                {
                    status = 110;   //"該当店舗は棚卸中です。";
                    return;
                }

                //納品伝票発行対象外店舗の場合
                if (IsNotPrintNouhin(condition.UserCenterId, ship_packing_data.ShipToStoreId))
                {
                    condition.NouhinPrnFlag = "8";      //発行不要とする（※データ上は発行済みとする）
                }
            }

            //再発行
            //納品書再発行
            if (condition.PrnClass == PrintInvoiceConditions.PrnClasses.ReNouhinPrn)
            {
                if (ship_packing_data.NouhinPrnFlag == "0")
                {
                    status = 5;   //納品書は一度も発行されていません。
                    return;
                }
                if (ship_packing_data.NouhinPrnFlag == "8" && ship_packing_data.DeliPrnFlag != "0")
                {
                    status = 51;   //納品書発行不要の出荷先です
                    return;
                }
                if (ship_packing_data.NouhinPrnFlag == "8" && ship_packing_data.DeliPrnFlag == "0")
                {
                    status = 52;   //納品書発行不要の出荷先です。新規発行アクションしてください。
                    return;
                }
            }

            //送り状再発行
            if (condition.PrnClass == PrintInvoiceConditions.PrnClasses.ReDeliPrn)
            {
                if (ship_packing_data.DeliPrnFlag != "0" && ship_packing_data.InvoicePrintFlag == 0)
                {
                    status = 6;  //送り状発行不要の配送業者です。
                    return;
                }
                if (ship_packing_data.DeliPrnFlag == "0")
                {
                    status = 7;   //送り状は一度も発行されていません。
                    return;
                }
                if (string.IsNullOrEmpty(ship_packing_data.DeliNo.Trim()))
                {
                    status = 8;   //PC画面で実績入力されています。発行できません
                    return;
                }
            }

            //ヤマト
            if (ship_packing_data.TransporterId == "01")
            {
                if (condition.PrnClass == PrintInvoiceConditions.PrnClasses.New && string.IsNullOrEmpty(condition.BoxSize))
                {
                    status = 30;   //ヤマトの場合箱サイズは必須です
                    return;
                }
                if (!string.IsNullOrEmpty(condition.BoxSize) && CheckBoxSize(condition.BoxSize) == false)
                {
                    status = 31;   //ヤマト箱サイズが不正です
                    return;
                };
            }

            //佐川・浪速・ワールドサプライ
            if (ship_packing_data.TransporterId == "02" || ship_packing_data.TransporterId == "03" || ship_packing_data.TransporterId == "08")
            {
                if (string.IsNullOrEmpty(ship_packing_data.NewClientCd))
                {
                    status = 32;    //配送業者顧客コード未設定
                    return;
                }

                //送り状再発行
                if (condition.PrnClass == PrintInvoiceConditions.PrnClasses.ReDeliPrn && !condition.Confirmed)
                {
                    //新規発行時の顧客コードと違う場合
                    if (ship_packing_data.ClientCd != ship_packing_data.NewClientCd)
                    {
                        if (ship_packing_data.IfStateTrp == 2)
                        {
                            status = 801;   //配送業者実績データ連携済みです。発行時のユーザーの拠点情報で再発行します。よろしいですか？
                            return;
                        }
                        else
                        {
                            status = 802;   //新規発行時のユーザーの拠点情報と違うため、送り状NO・配送業者顧客コードが変更されます。よろしいですか？
                            return;
                        }
                    }
                }
            }

            status = 0;
            return;
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

        /// <summary>
        /// 出荷梱包実績データフラグ更新
        /// </summary>
        /// <param name="box_no"></param>
        /// <param name="box_size"></param>
        /// <param name="transporter_id"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public void UpdatePackingInfo(PrintInvoiceConditions condition, string box_no, string box_size, string transporter_id, out int status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", condition.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_NO", box_no, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_SIZE", box_size, DbType.String, ParameterDirection.Input);
            param.Add("IN_TRANSPORTER_ID", transporter_id, DbType.String, ParameterDirection.Input);
            param.Add("IN_PRINT_CLASS", (int)condition.PrnClass, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_USER_CENTER_ID", condition.UserCenterId, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_DELIVERYNOTE_INVOICE04",
                param,
                commandType: CommandType.StoredProcedure);

            status = param.Get<int>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }

        /// <summary>
        /// 出荷確定処理
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public void ConfirmShipBefore(PrintInvoiceConditions condition, out int status, out string message)
        {
            //データチェック
            status = 0;
            message = "";
            var spstatus = 0;
            //出荷確定処理
            ConfirmShip(condition, condition.BoxNo, out spstatus, out message);
            status = int.Parse(spstatus.ToString());
        }

        /// <summary>
        /// 出荷確定ストアド実行
        /// </summary>
        /// <param name="box_no"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public void ConfirmShip(PrintInvoiceConditions condition, string box_no, out int status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", condition.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_NO", box_no, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_CONFIRM_SHIP",
                param,
                commandType: CommandType.StoredProcedure);

            status = param.Get<int>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }

        /// <summary>
        /// 直接印刷プリンタ名を取得
        /// </summary>
        /// <param name="userCenterId">利用者M.倉庫ID</param>
        /// <param name="reportId">帳票識別ID</param>
        /// <returns></returns>
        public string GetPrinterName(string userCenterId, string reportId)
        {
            return MvcDbContext.Current.Generals
                .Where(m => m.RegisterDiviCd == "1" &&
                            m.GenDivCd == "PRINTER_NAME" &&
                            m.GenCd == reportId &&
                            m.CenterId == userCenterId &&
                            m.ShipperId == Profile.User.ShipperId)
                .Select(m => m.GenName)
                .SingleOrDefault();
        }

        /// <summary>
        /// 納品伝票発行対象外か確認する
        /// </summary>
        /// <param name="userCenterId"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        private bool IsNotPrintNouhin(string userCenterId, string storeId)
        {
            return MvcDbContext.Current.Generals
                .Where(m => m.RegisterDiviCd == "1" &&
                            m.GenDivCd == "NOUHIN_NOT_PRINT_CENTER" &&
                            m.GenCd == storeId &&
                            m.CenterId == userCenterId &&
                            m.ShipperId == Profile.User.ShipperId)
                .Any();
        }

        /// <summary>
        /// 海外アソート店舗出荷か判定する
        /// </summary>
        /// <returns></returns>
        public PrintInvoiceAssortCheck GetAssortList(string BoxNo)
        {
            StringBuilder query = new StringBuilder();
            DynamicParameters param = new DynamicParameters();

            query.Append(@"
                SELECT
                        MAX(TPAP.CENTER_ID) CENTER_ID
                    ,   MAX(TPAP.SHIP_TO_STORE_ID) SHIP_TO_STORE_ID
                    ,   MAX(TPAP.SHIP_CLASS) SHIP_CLASS
                    ,   MAX(MIS.BRAND_ID) BRAND_ID
                    ,   MAX(TPAP.INVOICE_NO) INVOICE_NO
                    ,   TO_CHAR(MAX(TAP.SLIP_DATE),'YYYY/MM/DD') SLIP_DATE
                    ,   MAX(TAP.ARRIVE_BRANCH) ARRIVE_BRANCH
                FROM
                        T_PACKING_ARRIVE_PLANS TPAP
                INNER JOIN
                        M_ITEM_SKU MIS
                    ON
                        MIS.SHIPPER_ID = TPAP.SHIPPER_ID
                    AND MIS.ITEM_SKU_ID = TPAP.ITEM_SKU_ID
                INNER JOIN
                        T_ARRIVE_PLANS TAP
                    ON
                        TAP.SHIPPER_ID = TPAP.SHIPPER_ID
                    AND TAP.CENTER_ID = TPAP.CENTER_ID
                    AND TAP.INVOICE_NO = TPAP.INVOICE_NO
                    AND TAP.INVOICE_SEQ = TPAP.INVOICE_SEQ
                WHERE
                        TPAP.SHIPPER_ID = :SHIPPER_ID
                    AND TPAP.BOX_NO = :BOX_NO
            ");
            param.Add("SHIPPER_ID", Profile.User.ShipperId);
            param.Add("BOX_NO", BoxNo);

            return MvcDbContext.Current.Database.Connection.Query<PrintInvoiceAssortCheck>(query.ToString(), param).FirstOrDefault();
        }

        /// <summary>
        /// 入荷実績が存在するか確認する
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        public bool IsExistsArriveResult(string BoxNo)
        {
            var sql = @"
                SELECT
                        SHIPPER_ID
                FROM
                        T_PACKING_ARRIVE_RESULTS
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                    AND BOX_NO = :BOX_NO
            ";

            return MvcDbContext.Current.Database.Connection.Query(sql, 
                new {
                    SHIPPER_ID = Profile.User.ShipperId,
                    BOX_NO = BoxNo
                }).Any();
        }

        /// <summary>
        /// 出荷保留区分を取得する
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int GetShipHoldClass(PrintInvoiceAssortCheck condition)
        {
            StringBuilder query = new StringBuilder();
            DynamicParameters param = new DynamicParameters();
            //出荷先ビューから取得
            query.Append(@"
                SELECT
                        *
                FROM
                        V_SHIP_TO_STORES
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                    AND SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID
            ");
            param.Add("SHIPPER_ID", Profile.User.ShipperId);
            param.Add("SHIP_TO_STORE_ID", condition.ShipToStoreId);
            var stores = MvcDbContext.Current.Database.Connection.Query(query.ToString(), param).FirstOrDefault();
            //仮店舗区分が「1」の場合は「1:出荷保留」
            if (stores.TEMP_STORE_CLASS == "1") return 1;

            // 出荷レーン間口マスタから取得
            query = new StringBuilder();
            query.Append(@"
                SELECT
                        *
                FROM
                        M_SHIP_FRONTAGE
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                    AND BRAND_ID = :BRAND_ID
                    AND STORE_ID = :SHIP_TO_STORE_ID
            ");
            param.Add("BRAND_ID", condition.BrandId);
            var ShipFrontage = MvcDbContext.Current.Database.Connection.Query<Master.Models.ShipFrontage>(query.ToString(), param).FirstOrDefault();
            // 取得できなかった場合は「1:出荷保留」
            if (ShipFrontage is null) return 1;

            // 出荷保留店舗マスタから取得する
            query = new StringBuilder();
            query.Append(@"
                SELECT
                        *
                FROM
                        M_SHIPPING_HOLD_STORES
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                    AND BRAND_ID = :BRAND_ID
                    AND STORE_ID = :SHIP_TO_STORE_ID
            ");
            var ShippingHoldStore = MvcDbContext.Current.Database.Connection.Query<Master.Models.ShippingHoldStore>(query.ToString(), param).FirstOrDefault();
            //該当のデータがない場合は「0:即出荷」それ以外の場合は「出荷保留区分」を返す
            if (ShippingHoldStore is null) return 0; else return ShippingHoldStore.ShippingHoldClass ? 1 : 0;
        }

        /// <summary>
        /// 納品伝票内の入荷実績
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public PrintInvoiceAssortCheck CountArriveResult(PrintInvoiceAssortCheck condition)
        {
            StringBuilder query = new StringBuilder();
            DynamicParameters param = new DynamicParameters();

            query.Append(@"
                SELECT
                        INVOICE_NO
                    ,   MAX(SLIP_DATE) AS SLIPDATE
                FROM
                        T_ARRIVE_RESULTS
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                    AND INVOICE_NO = :INVOICE_NO
                GROUP BY
                        SHIPPER_ID
                    ,   INVOICE_NO
            ");
            param.Add("SHIPPER_ID", Profile.User.ShipperId);
            param.Add("INVOICE_NO", condition.InvoiceNo);
            return MvcDbContext.Current.Database.Connection.Query<PrintInvoiceAssortCheck>(query.ToString(), param).FirstOrDefault();
        }

        /// <summary>
        /// 入荷・出荷実績を作成する
        /// </summary>
        /// <param name="Conditions"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public void CreateResults(PrintInvoiceConditions Conditions, out int status, out string message)
        {
            status = 0;
            message = string.Empty;
            using (System.Data.Common.DbTransaction tran = MvcDbContext.Current.Database.Connection.BeginTransaction())
            {
                try
                {
                    // SP_W_SHP_DeliveryNote_Invoice06 を実行する
                    ExecuteSpInvoice06(Conditions, out status, out message);
                    if (status != (byte)ProcedureStatus.Success)
                    {
                        tran.Rollback();
                        return;
                    }
                    // SP_W_ARR_RESULTS_CONFIRM を実行する
                    ExecuteSpArrResults(Conditions, out status, out message);
                    if (status != (byte)ProcedureStatus.Success)
                    {
                        tran.Rollback();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();

                    status = (int)ProcedureStatus.Error;
                    message = ex.Message;
                    return;
                }
                tran.Commit();
            }
        }

        /// <summary>
        /// SP_W_SHP_DELIVERYNOTE_INVOICE06 を実行する
        /// </summary>
        /// <param name="confirmActual"></param>
        /// <returns>Update status</returns>
        public static void ExecuteSpInvoice06(PrintInvoiceConditions Conditions, out int status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", Conditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_NO", Conditions.BoxNo, DbType.String, ParameterDirection.Input);
            param.Add("IN_SLIP_DATE", Conditions.SlipDate, DbType.DateTime, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute("SP_W_SHP_DELIVERYNOTE_INVOICE06", param, commandType: CommandType.StoredProcedure);
            status = param.Get<int>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");

        }


        /// <summary>
        /// SP_W_ARR_RESULTS_CONFIRM を実行する 
        /// </summary>
        /// <param name="confirmActual"></param>
        /// <returns>Update status</returns>
        public static void ExecuteSpArrResults(PrintInvoiceConditions Conditions, out int status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", Conditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_INVOICE_NO", Conditions.InvoiceNo, DbType.String, ParameterDirection.Input);

            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute("SP_W_ARR_RESULTS_CONFIRM", param, commandType: CommandType.StoredProcedure);
            status = param.Get<int>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }

        /// <summary>
        /// 海外アソートビュー表示内容取得
        /// </summary>
        /// <returns></returns>
        public ScanAssortView GetView(string InvoiceNo)
        {
            StringBuilder query = new StringBuilder();
            DynamicParameters param = new DynamicParameters();

            query.Append(@"
                SELECT
                        TPAP.INVOICE_NO
                    ,   COUNT(DISTINCT TPAP.BOX_NO) AS PLAN_QTY
                    ,   COUNT(DISTINCT TPAR.BOX_NO) AS RESULT_QTY
                    ,   MAX(CENTER_NAME1) AS CENTER_NAME
                FROM
                        T_PACKING_ARRIVE_PLANS TPAP
                INNER JOIN
                        M_CENTERS MC
                    ON
                        MC.SHIPPER_ID = TPAP.SHIPPER_ID
                    AND MC.CENTER_ID = TPAP.CENTER_ID
                LEFT JOIN
                        T_PACKING_ARRIVE_RESULTS TPAR
                    ON
                        TPAP.SHIPPER_ID = TPAR.SHIPPER_ID
                    AND TPAP.CENTER_ID = TPAR.CENTER_ID
                    AND TPAP.BOX_NO = TPAR.BOX_NO
                WHERE
                        TPAP.SHIPPER_ID = :SHIPPER_ID
                    AND TPAP.INVOICE_NO = :INVOICE_NO
                    AND TPAP.SHIP_CLASS = 1
                GROUP BY
                        TPAP.SHIPPER_ID
                    ,   TPAP.CENTER_ID
                    ,   TPAP.INVOICE_NO
            ");
            param.Add("SHIPPER_ID", Profile.User.ShipperId);
            param.Add("INVOICE_NO", InvoiceNo);
            return MvcDbContext.Current.Database.Connection.Query<ScanAssortView>(query.ToString(), param).FirstOrDefault();
        }

        /// <summary>
        /// 店舗出荷残一覧内容取得
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<StoreInfo> GetZanStoreList(string InvoiceNo)
        {
            StringBuilder query = new StringBuilder();
            DynamicParameters param = new DynamicParameters();

            query.Append(@"
                SELECT DISTINCT
                        TPAP.SHIP_TO_STORE_ID
                    ,   STORE.SHIP_TO_STORE_NAME1 AS STORE_NAME
                FROM
                        T_PACKING_ARRIVE_PLANS TPAP
                LEFT JOIN
                        T_PACKING_ARRIVE_RESULTS TPAR
                    ON
                        TPAP.SHIPPER_ID = TPAR.SHIPPER_ID
                    AND TPAP.CENTER_ID = TPAR.CENTER_ID
                    AND TPAP.BOX_NO = TPAR.BOX_NO
                LEFT JOIN
                        V_SHIP_TO_STORES STORE
                    ON
                        TPAP.SHIPPER_ID = STORE.SHIPPER_ID
                    AND TPAP.SHIP_TO_STORE_ID = STORE.SHIP_TO_STORE_ID
                WHERE
                        TPAP.SHIPPER_ID = :SHIPPER_ID
                    AND TPAP.INVOICE_NO = :INVOICE_NO
                    AND TPAR.BOX_NO IS NULL
                    AND TPAP.SHIP_CLASS = 1
                ORDER BY
                        TPAP.SHIP_TO_STORE_ID
            ");
            param.Add("SHIPPER_ID", Profile.User.ShipperId);
            param.Add("INVOICE_NO", InvoiceNo);
            return MvcDbContext.Current.Database.Connection.Query<StoreInfo>(query.ToString(), param).ToList();
        }

        /// <summary>
        /// 締め年月日を取得
        /// </summary>
        /// <returns></returns>
        public string GetClosedDate()
        {
            var sql = @"
                SELECT
                    LAST_DAY(TO_DATE(CLOSED_MONTH, 'YYYYMM'))
                FROM
                    M_SHIPPERS
                WHERE
                    SHIPPER_ID = :SHIPPER_ID
            ";

            return MvcDbContext.Current.Database.Connection.Query<string>(sql, new { SHIPPER_ID = Profile.User.ShipperId }).Single();
        }

    }
}