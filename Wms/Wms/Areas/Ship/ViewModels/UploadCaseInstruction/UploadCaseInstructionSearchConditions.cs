namespace Wms.Areas.Ship.ViewModels.UploadCaseInstruction
{
    using Share.Common.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;
    using Wms.Common;
    using Wms.ViewModels.Shared;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class UploadCaseInstructionSearchConditions : IndicateViewModel
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum SortKey : byte
        {
            [Display(Name = nameof(UploadCaseInstructionResource.BatchNo), ResourceType = typeof(UploadCaseInstructionResource))]
            BatchNo,
            [Display(Name = nameof(UploadCaseInstructionResource.ShipKindBatchNo), ResourceType = typeof(UploadCaseInstructionResource))]
            ShipKindBatchNo
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
        /// ケース出荷指示名称
        /// </summary>
        public string CaseShipName { get; set; }

        /// <summary>
        /// Detail Sort key
        /// </summary>
        public SortKey Key { get; set; } = SortKey.BatchNo;

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        /// <summary>
        /// Search Type
        /// </summary>
        public SearchTypes SearchType { get; set; } = SearchTypes.Search;

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 取込ファイル名
        /// </summary>
        public string FileName { get; set; }

        public IList<SelectedUploadCaseInstructionViewModel> CaseViewModels { get; set; }

        /// <summary>
        /// 出荷種別
        /// </summary>
        public enum ShipKinds
        {
            [Display(Name = nameof(UploadCaseInstructionResource.KindCase), ResourceType = typeof(UploadCaseInstructionResource))]
            KindCase = 4,

            [Display(Name = nameof(UploadCaseInstructionResource.KindJan), ResourceType = typeof(UploadCaseInstructionResource))]
            KindJan = 5
        }

        public ShipKinds ShipKind { get; set; } = ShipKinds.KindCase;

        public int? SaleClass { get; set; }

        public int? Discount { get; set; }

        /// <summary>
        /// 隠し項目センター
        /// </summary>
        public string HidCenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// 隠し項目出荷種別
        /// </summary>
        public ShipKinds HidShipKind { get; set; } = ShipKinds.KindCase;

        /// <summary>
        /// 隠し項目バッチNo
        /// </summary>
        public string HidBatchNo { get; set; }

        /// <summary>
        /// 隠し項目ケース出荷指示名称
        /// </summary>
        public string HidShipInstructName { get; set; }

        /// <summary>
        /// 隠し項目店舗数
        /// </summary>
        public string HidStoreQty { get; set; }

        /// <summary>
        /// 隠し項目ケース数
        /// </summary>
        public string HidCaseQty { get; set; }

        /// <summary>
        /// 隠し項目明細行数
        /// </summary>
        public string HidDetailRowQty { get; set; }

        /// <summary>
        /// 隠し項目SKU数
        /// </summary>
        public string HidPicJanQty { get; set; }

        /// <summary>
        /// 隠し項目予定数
        /// </summary>
        public string HidPicInsQty { get; set; }

        /// <summary>
        /// 隠し項目引当エラー
        /// </summary>
        public string HidHikiErrQty { get; set; }

        /// <summary>
        /// 隠し項目出荷種別名
        /// </summary>
        public string HidShipKindName { get; set; }

        /// <summary>
        /// 隠し項目ケース出荷指示名称
        /// </summary>
        public string HidCaseShipName { get; set; }

        /// <summary>
        /// 隠し項目売上区分
        /// </summary>
        public int HidSaleClass { get; set; }
        
        /// <summary>
        /// 隠し項目オフ率
        /// </summary>
        public int HidDiscount { get; set; }
        
        /// <summary>
        /// Page number
        /// </summary>
        public int HidPage { get; set; } = 1;

        /// <summary>
        /// Row on page
        /// </summary>
        public int HidPageSize { get; set; } = 1;

        /// <summary>
        /// Page number
        /// </summary>
        public int HidIndexPage { get; set; } = 1;

        /// <summary>
        /// Detail Sort key
        /// </summary>
        public SortKey HidKey { get; set; }

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort HidSort { get; set; }

        /// <summary>
        /// Data to sort
        /// </summary>
        public enum DetailCaseSortKey : byte
        {
            [Display(Name = nameof(UploadCaseInstructionResource.ShipToStoreCaseNo), ResourceType = typeof(UploadCaseInstructionResource))]
            ShipToStoreCaseNo,
            [Display(Name = nameof(UploadCaseInstructionResource.CaseNoShipToStore), ResourceType = typeof(UploadCaseInstructionResource))]
            CaseNoShipToStore
        }

        /// <summary>
        /// ケース出荷明細ソートキー
        /// </summary>
        public DetailCaseSortKey DetailCaseKey { get; set; } = DetailCaseSortKey.ShipToStoreCaseNo;

        /// <summary>
        /// Data to sort
        /// </summary>
        public enum DetailJanSortKey : byte
        {
            [Display(Name = nameof(UploadCaseInstructionResource.JanShipToStore), ResourceType = typeof(UploadCaseInstructionResource))]
            JanShipToStore,
            [Display(Name = nameof(UploadCaseInstructionResource.ShipToStoreJan), ResourceType = typeof(UploadCaseInstructionResource))]
            ShipToStoreJan
        }

        /// <summary>
        /// JAN明細ソートキー
        /// </summary>
        public DetailJanSortKey DetailJanKey { get; set; } = DetailJanSortKey.JanShipToStore;

    }
}