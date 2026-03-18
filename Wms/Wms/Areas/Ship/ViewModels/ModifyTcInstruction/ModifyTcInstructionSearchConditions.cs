namespace Wms.Areas.Ship.ViewModels.ModifyTcInstruction
{
    using Share.Common.Resources;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Models;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.JanSearchModal;
    using Wms.Common;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class ModifyTcInstructionSearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum ModifyTcInstructionSortKey : byte
        {
            [Display(Name = nameof(Resources.ModifyTcInstructionResource.PriorityOrderStoreId), ResourceType = typeof(Resources.ModifyTcInstructionResource))]
            PriorityOrderStoreId,

            [Display(Name = nameof(Resources.ModifyTcInstructionResource.LaneNoFrontageNo), ResourceType = typeof(Resources.ModifyTcInstructionResource))]
            LaneNoFrontageNo
        }

        /// <summary>
        /// 昇順降順リスト
        /// </summary>
        public enum AscDescSort
        {
            [Display(Name = nameof(Share.Common.Resources.FormsResource.ASC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Asc,

            [Display(Name = nameof(Share.Common.Resources.FormsResource.DESC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Desc
        }

        /// <summary>
        /// 順
        /// </summary>
        public enum Order
        {
            [Display(Name = nameof(Resources.ModifyTcInstructionResource.PriorityOrderS), ResourceType = typeof(Resources.ModifyTcInstructionResource))]
            PriorityOrder,

            [Display(Name = nameof(Resources.ModifyTcInstructionResource.Equal), ResourceType = typeof(Resources.ModifyTcInstructionResource))]
            Equal
        }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// バッチNo
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ModifyTcInstructionResource.Jan), ResourceType = typeof(ModifyTcInstructionResource))]
        public string Jan { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        public string InvoiceNoHidden { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public ModifyTcInstructionSortKey SortKey { get; set; } = ModifyTcInstructionSortKey.PriorityOrderStoreId;

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// 順
        /// </summary>
        /// <remarks>
        public Order Orders { get; set; }

        /// <summary>
        /// TC出荷指示数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQtySum { get; set; }

        /// <summary>
        /// TC入荷実績数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArriveResultQtySum { get; set; }

        /// <summary>
        /// 修正後指示数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? WmsInstructQtySum { get; set; }

        /// <summary>
        /// 修正後指示数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? WmsInstructQtySumOtherPage { get; set; }

        /// <summary>
        /// Search Type
        /// </summary>
        public SearchTypes SearchType { get; set; } = SearchTypes.Search;

        public long Seq { get; set; }

        public bool EditFlag { get; set; } = true;

        public IList<ShpModTcInstruction> ModifyTcInstructions { get; set; }

        public bool ErrFlag { get; set; } = false;

        public IList<JanViewModel> janViewModel { get; set; }

        public int? totalCnt { get; set; }
    }
}