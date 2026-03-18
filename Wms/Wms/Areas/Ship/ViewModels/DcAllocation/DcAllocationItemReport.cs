namespace Wms.Areas.Ship.ViewModels.DcAllocation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;

    public class DcAllocationItemReport
    {
        /// <summary>
        /// センター
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(DcAllocationResource.CenterId), ResourceType = typeof(DcAllocationResource), Order = 1)]
        public string CenterId { get; set; }
        /// <summary>
        /// バッチNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(DcAllocationResource.BatchNo), ResourceType = typeof(DcAllocationResource), Order = 2)]
        public string BatchNo { get; set; }
        /// <summary>
        /// バッチ名称
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(DcAllocationResource.BatchName), ResourceType = typeof(DcAllocationResource), Order = 3)]
        public string BatchName { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(DcAllocationResource.CategoryId1), ResourceType = typeof(DcAllocationResource), Order = 6)]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類名1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(DcAllocationResource.CategoryName1), ResourceType = typeof(DcAllocationResource), Order = 7)]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(DcAllocationResource.ItemId), ResourceType = typeof(DcAllocationResource), Order = 8)]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(DcAllocationResource.ItemName), ResourceType = typeof(DcAllocationResource), Order = 9)]
        public string ItemName { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(DcAllocationResource.ItemColorId), ResourceType = typeof(DcAllocationResource), Order = 10)]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(DcAllocationResource.ItemColorName), ResourceType = typeof(DcAllocationResource), Order = 11)]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(DcAllocationResource.ItemSizeId), ResourceType = typeof(DcAllocationResource), Order = 12)]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(DcAllocationResource.ItemSizeName), ResourceType = typeof(DcAllocationResource), Order = 13)]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(DcAllocationResource.Jan), ResourceType = typeof(DcAllocationResource), Order = 14)]
        public string Jan { get; set; }

        /// <summary>
        /// 出荷予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(DcAllocationResource.InstructQty), ResourceType = typeof(DcAllocationResource), Order = 15)]
        public int? InstructQty { get; set; }

        /// <summary>
        /// 引当数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(DcAllocationResource.AllocQty), ResourceType = typeof(DcAllocationResource), Order = 16)]
        public int? AllocQty { get; set; }

        /// <summary>
        /// 欠品数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(DcAllocationResource.StockoutQty), ResourceType = typeof(DcAllocationResource), Order = 17)]
        public int? StockoutQty { get { return this.InstructQty - this.AllocQty; } }
    }
}