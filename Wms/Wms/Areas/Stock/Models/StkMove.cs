namespace Wms.Areas.Stock.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Stock.Resources;
    using Wms.Models;

    /// <summary>
    /// 在庫調整ワーク01
    /// </summary>
    [Table("WW_STK_MOVE")]
    public partial class StkMove : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(StkMoveResource.Seq), ResourceType = typeof(StkMoveResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(StkMoveResource.LineNo), ResourceType = typeof(StkMoveResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkMoveResource.IsCheck), ResourceType = typeof(StkMoveResource))]
        public bool IsCheck { get; set; }

        /// <summary>
        /// ロケーションコード
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkMoveResource.LocationCd), ResourceType = typeof(StkMoveResource))]
        public string LocationCd { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkMoveResource.GradeId), ResourceType = typeof(StkMoveResource))]
        public string GradeId { get; set; }

        /// <summary>
        /// ケース数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkMoveResource.CaseQty), ResourceType = typeof(StkMoveResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]        
        public int? CaseQty { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkMoveResource.SkuQty), ResourceType = typeof(StkMoveResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]        
        public int? SkuQty { get; set; }

        /// <summary>
        /// 在庫数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkMoveResource.StockQty), ResourceType = typeof(StkMoveResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]        
        public int? StockQty { get; set; }

        /// <summary>
        /// 引当数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkMoveResource.AllocQty), ResourceType = typeof(StkMoveResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        
        public int? AllocQty { get; set; }

        /// <summary>
        /// 未引当数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkMoveResource.NotAllocQty), ResourceType = typeof(StkMoveResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]        
        public int? NotAllocQty { get; set; }

        #endregion プロパティ
    }
}