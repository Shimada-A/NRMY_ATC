namespace Wms.Areas.Ship.ViewModels.EcConfirmProgress
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
    public class EcConfirmProgress02SearchConditions
    {

        /// <summary>
        /// Data to sort
        /// </summary>
        public enum EcConfirmProgress02SortKey : byte
        {
            [Display(Name = nameof(Resources.EcConfirmProgressResource.ShipInstructSeqId), ResourceType = typeof(Resources.EcConfirmProgressResource))]
            ShipInstructSeqId,

            [Display(Name = nameof(Resources.EcConfirmProgressResource.BoxNoItemSku), ResourceType = typeof(Resources.EcConfirmProgressResource))]
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
        public EcConfirmProgress02SortKey SortKey { get; set; } = EcConfirmProgress02SortKey.ShipInstructSeqId;

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
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        public long LineNo { get; set; }

    }
}