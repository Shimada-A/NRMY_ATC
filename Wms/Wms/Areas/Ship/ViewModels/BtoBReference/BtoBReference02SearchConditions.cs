namespace Wms.Areas.Ship.ViewModels.BtoBReference
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
    public class BtoBReference02SearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum BtoBReference02SortKey : byte
        {
            [Display(Name = nameof(Resources.BtoBReferenceResource.ShipInstructIdSeq), ResourceType = typeof(Resources.BtoBReferenceResource))]
            ShipInstructIdSeq,

            [Display(Name = nameof(Resources.BtoBReferenceResource.SkuShipInstructIdSeq), ResourceType = typeof(Resources.BtoBReferenceResource))]
            SkuShipInstructIdSeq
        }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// Sort key
        /// </summary>
        public BtoBReference02SortKey SortKey { get; set; } = BtoBReference02SortKey.ShipInstructIdSeq;

        /// <summary>
        /// SKU数合計
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

        /// <summary>
        /// パターン
        /// </summary>
        public string Parten { get; set; }
    }
}