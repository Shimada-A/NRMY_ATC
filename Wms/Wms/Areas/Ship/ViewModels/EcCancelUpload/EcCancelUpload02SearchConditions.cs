namespace Wms.Areas.Ship.ViewModels.EcCancelUpload
{
    using Share.Common.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;
    using Wms.Common;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class EcCancelUpload02SearchConditions
    {

        /// <summary>
        /// Data to sort
        /// </summary>
        public enum EcCancelUpload02SortKey : byte
        {
            [Display(Name = nameof(Resources.EcCancelUploadResource.ShipInstructSeqId), ResourceType = typeof(Resources.EcCancelUploadResource))]
            ShipInstructSeqId,

            [Display(Name = nameof(Resources.EcCancelUploadResource.BoxNoItemSku), ResourceType = typeof(Resources.EcCancelUploadResource))]
            BoxNoItemSku
        }

        /// <summary>
        /// 注文番号
        /// </summary>
        public string CenterId { get; set; }

        /// <summary>
        /// 状態
        /// </summary>
        public string ShipStatus { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public EcCancelUpload02SortKey SortKey { get; set; } = EcCancelUpload02SortKey.ShipInstructSeqId;

        /// <summary>
        /// ケース数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? BoxNoSum { get; set; }

        /// <summary>
        /// SKU合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuSum { get; set; }

        /// <summary>
        /// 実績数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQtySum { get; set; }

        /// <summary>
        /// 予定数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQtySum { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        public long LineNo { get; set; }

        //画面選択注文番号
        public string ShipInstructId { get; set; }

    }
}