namespace Wms.Areas.Inventory.ViewModels.Reference
{
    using PagedList;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Reference02Head
    {
        /// <summary>
        /// センター
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CenterId), ResourceType = typeof(Resources.ReferenceResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 棚卸No
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.InventoryNo), ResourceType = typeof(Resources.ReferenceResource))]
        public string InventoryNo { get; set; }

        /// <summary>
        /// 棚卸開始日時
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = nameof(Resources.ReferenceResource.InventoryStartDate), ResourceType = typeof(Resources.ReferenceResource))]
        public DateTime? InventoryStartDate { get; set; }

        /// <summary>
        /// 棚卸区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.InventoryClass), ResourceType = typeof(Resources.ReferenceResource))]
        public string InventoryClass { get; set; }

        /// <summary>
        /// 棚卸名称
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.InventoryName), ResourceType = typeof(Resources.ReferenceResource))]
        public string InventoryName { get; set; }

        /// <summary>
        /// 外装棚卸許可フラグ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.SimpleInventoryFlag), ResourceType = typeof(Resources.ReferenceResource))]
        public string SimpleInventoryFlag { get; set; }

        /// <summary>
        /// 棚卸状況
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.InventoryStatus), ResourceType = typeof(Resources.ReferenceResource))]
        public string InventoryStatus { get; set; }

        /// <summary>
        /// 棚卸確定フラグ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.InventoryConfirmDate), ResourceType = typeof(Resources.ReferenceResource))]
        public DateTime? InventoryConfirmDate { get; set; }
    }

    public class Reference02Total
    {
        /// <summary>
        /// ロケーション数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(Resources.ReferenceResource.LocationCount), ResourceType = typeof(Resources.ReferenceResource))]
        public int LocationCount { get; set; }

        /// <summary>
        /// 帳簿在庫SKU数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(Resources.ReferenceResource.StockSku), ResourceType = typeof(Resources.ReferenceResource))]
        public int StockSku { get; set; }

        /// <summary>
        /// 実棚SKU数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(Resources.ReferenceResource.ResultSku), ResourceType = typeof(Resources.ReferenceResource))]
        public int ResultSku { get; set; }

        /// <summary>
        /// 帳簿在庫ケース数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(Resources.ReferenceResource.StockCase), ResourceType = typeof(Resources.ReferenceResource))]
        public int StockCase { get; set; }

        /// <summary>
        /// 実棚ケース数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(Resources.ReferenceResource.ResultCase), ResourceType = typeof(Resources.ReferenceResource))]
        public int ResultCase { get; set; }

        /// <summary>
        /// 帳簿在庫バラ数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(Resources.ReferenceResource.StockQty), ResourceType = typeof(Resources.ReferenceResource))]
        public int StockQty { get; set; }

        /// <summary>
        /// 実棚バラ数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(Resources.ReferenceResource.ResultQty), ResourceType = typeof(Resources.ReferenceResource))]
        public int ResultQty { get; set; }
    }

    public class Reference02Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<Reference02ResultRow> Reference { get; set; }
    }

    public class Reference02ViewModel
    {
        public Reference02SearchConditions SearchConditions { get; set; }

        public Reference02Head Head { get; set; }

        public Reference02Total Total { get; set; }

        public Reference02Result Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Reference02ViewModel"/> class.
        /// </summary>
        public Reference02ViewModel()
        {
            this.SearchConditions = new Reference02SearchConditions();
            this.Head = new Reference02Head();
            this.Total = new Reference02Total();
            this.Results = new Reference02Result();
        }
    }
}