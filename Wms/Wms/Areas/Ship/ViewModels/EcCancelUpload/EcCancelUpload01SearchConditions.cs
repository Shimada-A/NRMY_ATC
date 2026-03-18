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
    public class EcCancelUpload01SearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum EcCancelUploadSortKey : byte
        {
            [Display(Name = nameof(EcCancelUploadResource.CUDateShipInstructId), ResourceType = typeof(EcCancelUploadResource))]
            CUDateShipInstructId,

            [Display(Name = nameof(EcCancelUploadResource.ShipInstructId), ResourceType = typeof(EcCancelUploadResource))]
            ShipInstructId
        }

        /// <summary>
        /// 昇順降順リスト
        /// </summary>
        public enum AscDescSort
        {
            [Display(Name = nameof(FormsResource.ASC), ResourceType = typeof(FormsResource))]
            Asc,

            [Display(Name = nameof(FormsResource.DESC), ResourceType = typeof(FormsResource))]
            Desc
        }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// 注文番号
        /// </summary>
        public string ShipInstructId { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public EcCancelUploadSortKey SortKey { get; set; } = EcCancelUploadSortKey.CUDateShipInstructId;

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
        /// SKU数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuSum { get; set; }

        /// <summary>
        /// 予定数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? InstructQtySum { get; set; }

        /// <summary>
        /// Search Type
        /// </summary>
        public SearchTypes SearchType { get; set; } = SearchTypes.Search;

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        public long LineNo { get; set; }

        public IList<SelectedEcCancelUpload01ViewModel> EcCancelUpload01s { get; set; }

        /// <summary>
        /// 隠し項目注文番号
        /// </summary>
        public string HidShipInstructId { get; set; }

        /// <summary>
        /// 隠し項目キャンセル更新区分
        /// </summary>
        public string HidCuClass { get; set; }

        public long Canup_Kbn { get; set; }
    }

}