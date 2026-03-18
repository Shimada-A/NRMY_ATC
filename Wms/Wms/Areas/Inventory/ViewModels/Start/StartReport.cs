namespace Wms.Areas.Inventory.ViewModels.Start
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Inventory.Resources;
    using Wms.Common;

    /// <summary>
    /// 棚卸開始処理DownLoadRow
    /// </summary>
    public class StartReport
    {
        #region プロパティ

        /// <summary>
        /// センター
        /// </summary>
        [Display(Name = nameof(StartResource.CenterId), ResourceType = typeof(StartResource), Order = 1)]
        public string CenterId { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StartResource.LocationCd), ResourceType = typeof(StartResource), Order = 2)]
        public string LocationCd { get; set; }

        #endregion プロパティ
    }
}