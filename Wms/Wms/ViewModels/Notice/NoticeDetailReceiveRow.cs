namespace Wms.ViewModels.Notice
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;
    using Wms.Models;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class NoticeDetailReceiveRow
    {
        /// <summary>
        /// 行No
        /// </summary>
        public int RowNo { get; set; }

        /// <summary>
        /// 受信ファイル名
        /// </summary>
        public string IfFileName { get; set; }

        /// <summary>
        /// ファイル内連番
        /// </summary>
        public int IfSeq { get; set; }

        /// <summary>
        /// エラー項目コメント
        /// </summary>
        public string IfErrColumnComment { get; set; }

        /// <summary>
        /// エラー区分
        /// </summary>
        public int IfErrClass { get; set; }

        /// <summary>
        ///エラー区分名称
        /// </summary>
        public string IfErrClassNm { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 納品書行番号
        /// </summary>
        public string InvoiceSeq { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        public string ItemColorId { get; set; }

        /// <summary>
        /// サイズID
        /// </summary>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// 指示番号
        /// </summary>
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 出荷指示明細ID
        /// </summary>
        public string ShipInstructSeq { get; set; }

        /// <summary>
        /// 指示日付
        /// </summary>
        public string InstructDate { get; set; }

        /// <summary>
        /// 得意先コード
        /// </summary>
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 倉庫ID
        /// </summary>
        public string CenterId { get; set; }

        /// <summary>
        /// 伝票番号
        /// </summary>
        public string SlipNo { get; set; }

        /// <summary>
        /// 移動伝票番号内連番
        /// </summary>
        public string SlipSeq { get; set; }

        /// <summary>
        /// ケースNO
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 梱包行番号
        /// </summary>
        public string BoxSeq { get; set; }

        /// <summary>
        /// GASバッチNo
        /// </summary>
        public string GasBatchNo { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 仕入先ID
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// 予定納期
        /// </summary>
        public string ArrivePlanDate { get; set; }

        /// <summary>
        /// 移動元コード
        /// </summary>
        public string TransferFromStoreId { get; set; }

        /// <summary>
        /// 出荷予定日
        /// </summary>
        public string ShipPlanDate { get; set; }

        /// <summary>
        /// Jan
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// 発注番号
        /// </summary>
        public string PoId { get; set; }


        /// <summary>
        /// 入荷予定分納枝番
        /// </summary>
        public string BranchNo { get; set; }
        
        /// <summary>
        /// 入倉庫コード
        /// </summary>
        public string ArriveCenterId { get; set; }

        /// <summary>
        /// 移動先倉庫コード
        /// </summary>
        public string TransferToCenterId { get; set; }

        /// <summary>
        /// 配分日
        /// </summary>
        public string DistributeDate { get; set; }

        /// <summary>
        /// 伝票日付
        /// </summary>
        public string SlipDate { get; set; }

        /// <summary>
        /// カートンID
        /// </summary>
        public string CartonId { get; set; }

        /// <summary>
        /// ロケーションコード
        /// </summary>
        public string LocationCd { get; set; }

        /// <summary>
        /// 相手先伝票番号
        /// </summary>
        public string PartnerSlipNo { get; set; }
        
        /// <summary>
        /// 棚卸NO
        /// </summary>
        public string InventoryNo { get; set; }
    }
}