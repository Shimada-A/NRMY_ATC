namespace Wms.Areas.Inventory.ViewModels.Input
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Inventory.Resources;
    using Wms.Common;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class InputSearchConditions
    {
        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// 棚卸No
        /// </summary>
        public string InventoryNo { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        public string LocationCd { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// TotalCount
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// Search Type
        /// </summary>
        public SearchTypes SearchType { get; set; } = SearchTypes.Search;

        /// <summary>
        /// ケースエリア・バラエリア・荷姿混在エリア
        /// </summary>
        public AreaTypes AreaType { get; set; }

        public enum AreaTypes : byte
        {
            [Display(Name = nameof(InputResource.Case), ResourceType = typeof(Resources.InputResource))]
            Case = 1,
            [Display(Name = nameof(InputResource.Bara), ResourceType = typeof(Resources.InputResource))]
            Bara = 2,
            [Display(Name = nameof(InputResource.Mix), ResourceType = typeof(Resources.InputResource))]
            Mix = 3,
        }
    }
}