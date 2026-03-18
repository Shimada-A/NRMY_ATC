namespace Wms.Areas.Ship.ViewModels.PrintBatch
{
    using Share.Common.Resources;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class PrintBatchSearchConditions
    {
        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// バッチNo
        /// </summary>
        public string AllocGroupNo { get; set; }

        /// <summary>
        /// 未ピックのみ
        /// </summary>
        public bool OnlyUnpicked { get; set; }

        /// <summary>
        /// ピック中のみ
        /// </summary>
        public bool OnlyPicDuring { get; set; }

        /// <summary>
        /// ロケーションFROM
        /// </summary>
        public string LocationCdFrom { get; set; }

        /// <summary>
        /// ロケーションTO
        /// </summary>
        public string LocationCdTo { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 出荷先ID
        /// </summary>
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先名
        /// </summary>
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// 注文番号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 出荷種別
        /// </summary>
        public int ShipKind { get; set; }

        /// <summary>
        /// ピック種別
        /// </summary>
        public int PickKind { get; set; }

        /// <summary>
        /// 印刷フラグ
        /// </summary>
        public string PrintFlag { get; set; }

        /// <summary>
        /// No
        /// </summary>
        public int No { get; set; }

        public string Ret { get; set; }

        public string Print { get; set; }

        public bool chkJan { get; set; }

        public string StyleName { get; set; }

        public string DownloadFileName { get; set; }
    }
}