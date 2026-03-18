namespace Wms.Areas.Returns.ViewModels.PurchaseReturns
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;
    using System.Collections.Generic;
    using Share.Common.Resources;
    using Wms.Areas.Returns.Resources;
    public class PurchaseReturnsSearchConditions
    {
        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// 仕入先
        /// </summary>
        //[Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(PurchaseReturnsResource.Vendor), ResourceType = typeof(PurchaseReturnsResource))]
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        public string VendorName { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 仕入先数
        /// </summary>
        public int VendorCount { get; set; } = 0;

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// Data to sort
        /// </summary>
        public enum DataSortKey : byte
        {
            [Display(Name = nameof(Resources.PurchaseReturnsResource.Sku), ResourceType = typeof(Resources.PurchaseReturnsResource))]
            Sku,

            [Display(Name = nameof(Resources.PurchaseReturnsResource.Jan), ResourceType = typeof(Resources.PurchaseReturnsResource))]
            Jan
        }

        /// <summary>
        /// Sort key
        /// </summary>
        public DataSortKey SortKey { get; set; } = DataSortKey.Sku;

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
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        public string ReturnId { get; set; }

        public int? ZeroFlgCnt { get; set; }

        public string Ret { get; set; }

        public string Print { get; set; }
    }
}