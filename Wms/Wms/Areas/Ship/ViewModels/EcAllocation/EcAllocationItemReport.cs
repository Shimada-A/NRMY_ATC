namespace Wms.Areas.Ship.ViewModels.EcAllocation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class EcAllocationItemReport
    {
        /// <summary>
        /// センター(ID)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.EcAllocationResource.CenterId), ResourceType = typeof(Resources.EcAllocationResource), Order = 1)]
        public string CenterId { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.EcAllocationResource.ItemId), ResourceType = typeof(Resources.EcAllocationResource), Order = 2)]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.EcAllocationResource.ItemName), ResourceType = typeof(Resources.EcAllocationResource), Order = 3)]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー(ID)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.EcAllocationResource.ItemColorId), ResourceType = typeof(Resources.EcAllocationResource), Order = 4)]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー(名称)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.EcAllocationResource.ItemColorName), ResourceType = typeof(Resources.EcAllocationResource), Order = 5)]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ(ID)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.EcAllocationResource.ItemSizeId), ResourceType = typeof(Resources.EcAllocationResource), Order = 6)]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ(名称)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.EcAllocationResource.ItemSizeName), ResourceType = typeof(Resources.EcAllocationResource), Order = 7)]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.EcAllocationResource.Jan), ResourceType = typeof(Resources.EcAllocationResource), Order = 8)]
        public string Jan { get; set; }

        /// <summary>
        /// 引当エラー数量
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(Resources.EcAllocationResource.AllocErrorQty), ResourceType = typeof(Resources.EcAllocationResource), Order = 9)]
        public int? AllocErrorQty { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.EcAllocationResource.LocationCd), ResourceType = typeof(Resources.EcAllocationResource), Order = 10)]
        public string LocationCd { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.EcAllocationResource.GradeId), ResourceType = typeof(Resources.EcAllocationResource), Order = 11)]
        public string GradeId { get; set; }

        /// <summary>
        /// 在庫数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(Resources.EcAllocationResource.StockQty), ResourceType = typeof(Resources.EcAllocationResource), Order = 12)]
        public int? StockQty { get; set; }

        /// <summary>
        /// 引当数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(Resources.EcAllocationResource.AllocQty), ResourceType = typeof(Resources.EcAllocationResource), Order = 13)]
        public int? AllocQty { get; set; }
    }
}