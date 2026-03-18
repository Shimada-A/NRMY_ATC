namespace Wms.Areas.Ship.ViewModels.TransferReference
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class TransferReferenceSearchConditions
    {
        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// 出荷区分
        /// </summary>
        public ShipKinds ShipKind { get; set; } = ShipKinds.Dc;

        /// <summary>
        /// 出荷予定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 引当日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? AllocDate { get; set; }

        /// <summary>
        /// 引当日From
        /// </summary>
        public DateTime? AllocDateFrom { get; set; }

        /// <summary>
        /// 引当日To
        /// </summary>
        public DateTime? AllocDateTo { get; set; }

        /// <summary>
        /// バッチNo
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// バッチ名称
        /// </summary>
        [Display(Name = nameof(Resources.TransferReferenceResource.BatchName), ResourceType = typeof(Resources.TransferReferenceResource))]
        public string BatchName { get; set; }

        /// <summary>
        /// ページ数
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// ページ行番号
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// Search Type
        /// </summary>
        public SearchTypes SearchType { get; set; } = SearchTypes.Search;
    }
}