using Share.Common.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using Wms.Common;

namespace Wms.Areas.Stock.ViewModels.InOutReference
{
    public class InOutReferenceSearchConditions
    {
        /// <summary>
        /// 表示順（在庫明細）
        /// </summary>
        public enum StockSortKey
        {
            [Display(Name = nameof(Resources.InOutReferenceResource.StockSortKeyLocation), ResourceType = typeof(Resources.InOutReferenceResource))]
            Location,
            [Display(Name = nameof(Resources.InOutReferenceResource.StockSortKeySku), ResourceType = typeof(Resources.InOutReferenceResource))]
            ItemSku,
            [Display(Name = nameof(Resources.InOutReferenceResource.StockSortKeyOperation), ResourceType = typeof(Resources.InOutReferenceResource))]
            Operation
        }

        /// <summary>
        /// 表示順（ケース明細）
        /// </summary>
        public enum PackageStockSortKey
        {
            [Display(Name = nameof(Resources.InOutReferenceResource.PackageStockSortKeyLocation), ResourceType = typeof(Resources.InOutReferenceResource))]
            Location,
            [Display(Name = nameof(Resources.InOutReferenceResource.PackageStockSortKeySku), ResourceType = typeof(Resources.InOutReferenceResource))]
            ItemSku,
            [Display(Name = nameof(Resources.InOutReferenceResource.PackageStockSortKeyBox), ResourceType = typeof(Resources.InOutReferenceResource))]
            Box,
            [Display(Name = nameof(Resources.InOutReferenceResource.PackageStockSortKeyOperation), ResourceType = typeof(Resources.InOutReferenceResource))]
            Operation
        }

        /// <summary>
        /// 昇順・降順
        /// </summary>
        public enum AscDescSort
        {
            [Display(Name = nameof(FormsResource.ASC), ResourceType = typeof(FormsResource))]
            Asc,

            [Display(Name = nameof(FormsResource.DESC), ResourceType = typeof(FormsResource))]
            Desc
        }

        public string CenterId { get; set; }

        [Display(Name = nameof(Resources.InOutReferenceResource.MoveDateFrom), ResourceType = typeof(Resources.InOutReferenceResource))]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public DateTime? MoveDateFrom { get; set; }

        [Display(Name = nameof(Resources.InOutReferenceResource.MoveDateTo), ResourceType = typeof(Resources.InOutReferenceResource))]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public DateTime? MoveDateTo { get; set; }

        public ResultTypes ResultType { get; set; } = ResultTypes.Stock;

        public string Jan { get; set; }

        public string LocationClass { get; set; }

        public string ItemId { get; set; }

        public string LocationCode { get; set; }

        public string ItemSkuId { get; set; }

        public string BoxNo { get; set; }

        public StockSortKey SortKey { get; set; } = StockSortKey.Operation;

        public PackageStockSortKey PackageSortKey { get; set; } = PackageStockSortKey.Operation;

        public AscDescSort Sort { get; set; } = AscDescSort.Asc;

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        /// <summary>
        /// 0削除は表示しない
        /// </summary>
        public bool NotZeroDisp { get; set; }

        public SearchTypes SearchType { get; set; } = SearchTypes.Search;
    }
}