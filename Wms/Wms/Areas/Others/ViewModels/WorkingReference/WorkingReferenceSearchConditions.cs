namespace Wms.Areas.Others.ViewModels.WorkingReference
{
    using Share.Common.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Others.Resources;
    using Wms.Common;
    using Wms.ViewModels.Shared;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class WorkingReferenceSearchConditions : IndicateViewModel
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum ShiireSortKey : byte
        {
            [Display(Name = nameof(WorkingReferenceResource.StartDateWorkingUser), ResourceType = typeof(WorkingReferenceResource))]
            StartDateWorkingUser,
            [Display(Name = nameof(WorkingReferenceResource.InvoiceNoItemSKU), ResourceType = typeof(WorkingReferenceResource))]
            InvoiceNoItemSKU
        }
        public enum IdoSortKey : byte
        {
            [Display(Name = nameof(WorkingReferenceResource.StartDateWorkingUser), ResourceType = typeof(WorkingReferenceResource))]
            StartDateWorkingUser,
            [Display(Name = nameof(WorkingReferenceResource.SlipNoSlipSeq), ResourceType = typeof(WorkingReferenceResource))]
            SlipNoSlipSeq
        }
        public enum PicSortKey : byte
        {
            [Display(Name = nameof(WorkingReferenceResource.PicDatePicUser), ResourceType = typeof(WorkingReferenceResource))]
            PicDatePicUser,
            [Display(Name = nameof(WorkingReferenceResource.BatchNoLocationCdItemSKU), ResourceType = typeof(WorkingReferenceResource))]
            BatchNoLocationCdItemSKU
        }
        public enum StoreSortKey : byte
        {
            [Display(Name = nameof(WorkingReferenceResource.ShiwakeDateWorkingUser), ResourceType = typeof(WorkingReferenceResource))]
            ShiwakeDateWorkingUser,
            [Display(Name = nameof(WorkingReferenceResource.BatchNoItemSKULaneNoFrontageNo), ResourceType = typeof(WorkingReferenceResource))]
            BatchNoItemSKULaneNoFrontageNo
        }
        public enum StockSortKey : byte
        {
            [Display(Name = nameof(WorkingReferenceResource.StartDateWorkingUser), ResourceType = typeof(WorkingReferenceResource))]
            StartDateWorkingUser,
            [Display(Name = nameof(WorkingReferenceResource.BoxNo), ResourceType = typeof(WorkingReferenceResource))]
            BoxNo
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
        /// Detail Sort key
        /// </summary>
        public ShiireSortKey ShiireKey { get; set; } = ShiireSortKey.StartDateWorkingUser;

        public IdoSortKey IdoKey { get; set; } = IdoSortKey.StartDateWorkingUser;

        public PicSortKey PicKey { get; set; } = PicSortKey.PicDatePicUser;

        public StoreSortKey StoreKey { get; set; } = StoreSortKey.ShiwakeDateWorkingUser;

        public StockSortKey StockKey { get; set; } = StockSortKey.StartDateWorkingUser;

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
        public string CenterId { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        public string InvoiceNo { get; set; }

        public string ItemSkuId { get; set; }
        
        public string SlipNo { get; set; }

        public string BoxNo { get; set; }

        public int? SlipSeq { get; set; }

        public long? HaitaSeq { get; set; }

        public IList<SelectedWorkingReferenceViewModel> CaseViewModels { get; set; }

        /// <summary>
        /// 作業中業務種類
        /// </summary>
        public enum SearchKinds
        {
            [Display(Name = nameof(WorkingReferenceResource.Shiire), ResourceType = typeof(WorkingReferenceResource))]
            Shiire = 1,

            [Display(Name = nameof(WorkingReferenceResource.Ido), ResourceType = typeof(WorkingReferenceResource))]
            Ido = 2,

            [Display(Name = nameof(WorkingReferenceResource.Pic), ResourceType = typeof(WorkingReferenceResource))]
            Pic = 3,

            [Display(Name = nameof(WorkingReferenceResource.Store), ResourceType = typeof(WorkingReferenceResource))]
            Store = 4,

            [Display(Name = nameof(WorkingReferenceResource.Stock), ResourceType = typeof(WorkingReferenceResource))]
            Stock = 5
        }

        public SearchKinds SearchKind { get; set; } = SearchKinds.Shiire;
     }
}