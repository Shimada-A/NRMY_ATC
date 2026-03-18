namespace Wms.Areas.Inventory.ViewModels.Start
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Inventory.Resources;
    using Wms.Common;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class StartSearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum SortKey : byte
        {
            [Display(Name = nameof(StartResource.InventoryNo), ResourceType = typeof(Resources.StartResource))]
            InventoryNo,
            [Display(Name = nameof(StartResource.InventoryStartDateNo), ResourceType = typeof(Resources.StartResource))]
            InventoryStartDateNo,
            [Display(Name = nameof(StartResource.InventoryNameNo), ResourceType = typeof(Resources.StartResource))]
            InventoryNameNo,
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
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// 棚卸区分
        /// </summary>
        public string InventoryClass { get; set; } = "2";

        /// <summary>
        /// 外装棚卸許可モード
        /// </summary>
        public bool SimpleIInventoryFlag { get; set; }
        /// <summary>
        /// Detail Sort key
        /// </summary>
        public SortKey Key { get; set; } = SortKey.InventoryNo;

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
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 取込ファイル名
        /// </summary>
        public string FileName { get; set; }

    }
}