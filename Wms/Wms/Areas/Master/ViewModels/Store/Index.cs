namespace Wms.Areas.Master.ViewModels.Store
{
    using System.ComponentModel.DataAnnotations;
    using PagedList;
    using Wms.Common;
    using System;
    using Wms.Resources;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class StoreSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum StoreSortKey : byte
        {
            [Display(Name = nameof(Resources.StoreResource.StoreId), ResourceType = typeof(Resources.StoreResource))]
            StoreId,

            [Display(Name = nameof(Resources.StoreResource.StoreName), ResourceType = typeof(Resources.StoreResource))]
            StoreName,

            [Display(Name = nameof(Resources.StoreResource.OpenDateStoreId), ResourceType = typeof(Resources.StoreResource))]
            OpenDateStoreId
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
        /// 店舗区分
        /// </summary>
        public string StoreClass { get; set; }

        /// <summary>
        /// 店舗ID
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// 店舗名
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// 住所
        /// </summary>
        public string StoreAddress { get; set; }

        /// <summary>
        /// TEL
        /// </summary>
        [RegularExpression(@"[0-9]+", ErrorMessage = "TELはハイフンなしで入力してください。")]
        public string StoreTel { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public StoreSortKey SortKey { get; set; }

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 0;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// 削除フラグ
        /// </summary>
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 検品必須フラグ
        /// </summary>
        public bool InspectionMustFlag { get; set; }
        
        /// <summary>
        /// データ新規登録日From
        /// </summary>
        public string MakeDateFrom { get; set; } = CommonResource.None;

        /// <summary>
        /// データ新規登録日To
        /// </summary>
        public string MakeDateTo { get; set; } = CommonResource.None;
    }

    public class StoreResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<Models.Store> Stores { get; set; }
    }

    public class Index
    {
        public StoreSearchCondition SearchConditions { get; set; }

        public StoreResult StoreResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new StoreSearchCondition();
            this.StoreResult = new StoreResult();
        }
    }
}