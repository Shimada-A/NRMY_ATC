namespace Wms.Areas.Ship.ViewModels.BtoBInstructDelete
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
    public class BtoBInsDelDetailSearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum BtoBInsDeleteDetailSortKey : byte
        {
            [Display(Name = nameof(Resources.BtoBInstructDeleteResource.ShipPlanDateInstructDetail), ResourceType = typeof(Resources.BtoBInstructDeleteResource))]
            ShipPlanDateInstructDetail,

            [Display(Name = nameof(Resources.BtoBInstructDeleteResource.SkuInstructIdDetailId), ResourceType = typeof(Resources.BtoBInstructDeleteResource))]
            SkuInstructIdDetailId,

            [Display(Name = nameof(Resources.BtoBInstructDeleteResource.ShipIdSkuInstructIdDetailId), ResourceType = typeof(Resources.BtoBInstructDeleteResource))]
            ShipIdSkuInstructIdDetailId
        }
        public enum AscDescSortDetail
        {
            [Display(Name = nameof(Share.Common.Resources.FormsResource.ASC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Asc,

            [Display(Name = nameof(Share.Common.Resources.FormsResource.DESC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Desc
        }
        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        /// <summary>
        /// Sort key
        /// </summary>
        public BtoBInsDeleteDetailSortKey DetailSortKey { get; set; } = BtoBInsDeleteDetailSortKey.ShipPlanDateInstructDetail;

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSortDetail Sort { get; set; }

        /// <summary>
        /// SKU数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuSum { get; set; }

        /// <summary>
        /// 明細数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? InstructSeqSum { get; set; }

        /// <summary>
        /// 予定数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? InstructQtySum { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        public long LineNo { get; set; }

        /// <summary>
        /// 出荷指示ID
        /// </summary>
        public string ShipInstructId { get; set; }

    }
}