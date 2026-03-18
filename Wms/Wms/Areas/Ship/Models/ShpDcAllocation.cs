namespace Wms.Areas.Ship.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Ship.Resources;
    using Wms.Models;

    /// <summary>
    /// DC引当ワーク
    /// </summary>
    [Table("WW_SHP_DC_ALLOCATION")]
    public partial class ShpDcAllocation : BaseModel
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
        [Display(Name = nameof(ShpDcAllocationResource.Seq), ResourceType = typeof(ShpDcAllocationResource))]
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
        [Display(Name = nameof(ShpDcAllocationResource.LineNo), ResourceType = typeof(ShpDcAllocationResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.IsCheck), ResourceType = typeof(ShpDcAllocationResource))]
        public bool IsCheck { get; set; }

        /// <summary>
        /// センターコード
        /// </summary>
        [Display(Name = nameof(ShpDcAllocationResource.CenterId), ResourceType = typeof(ShpDcAllocationResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 出荷予定日
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = nameof(ShpDcAllocationResource.ShipPlanDate), ResourceType = typeof(ShpDcAllocationResource))]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 緊急
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.EmergencyClassName), ResourceType = typeof(ShpDcAllocationResource))]
        public string EmergencyClassName { get; set; }

        /// <summary>
        /// 出荷指示ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.ShipInstructId), ResourceType = typeof(ShpDcAllocationResource))]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 出荷指示明細ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.ShipInstructSeq), ResourceType = typeof(ShpDcAllocationResource))]
        public long ShipInstructSeq { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.ItemId), ResourceType = typeof(ShpDcAllocationResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.ItemName), ResourceType = typeof(ShpDcAllocationResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.ItemColorId), ResourceType = typeof(ShpDcAllocationResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.ItemColorName), ResourceType = typeof(ShpDcAllocationResource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.ItemSizeId), ResourceType = typeof(ShpDcAllocationResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.ItemSizeName), ResourceType = typeof(ShpDcAllocationResource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.Jan), ResourceType = typeof(ShpDcAllocationResource))]
        public string Jan { get; set; }

        /// <summary>
        /// 出荷先ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.ShipToStoreId), ResourceType = typeof(ShpDcAllocationResource))]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.ShipToStoreName), ResourceType = typeof(ShpDcAllocationResource))]
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// 出荷先区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.ShipToStoreClassName), ResourceType = typeof(ShpDcAllocationResource))]
        public string ShipToStoreClassName { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.TransporterName), ResourceType = typeof(ShpDcAllocationResource))]
        public string TransporterName { get; set; }

        /// <summary>
        /// 出荷先数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(ShpDcAllocationResource.ShipToQty), ResourceType = typeof(ShpDcAllocationResource))]
        public int? ShipToQty { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(ShpDcAllocationResource.InstructQty), ResourceType = typeof(ShpDcAllocationResource))]
        public int? InstructQty { get; set; }

        /// <summary>
        /// データ受信日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.AllocDate), ResourceType = typeof(ShpDcAllocationResource))]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? AllocDate { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.ItemSkuId), ResourceType = typeof(ShpDcAllocationResource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 指示区分名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.InstructClassName), ResourceType = typeof(ShpDcAllocationResource))]
        public string InstructClassName { get; set; }

        /// <summary>
        /// 売上区分名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.SalesClassName), ResourceType = typeof(ShpDcAllocationResource))]
        public string SalesClassName { get; set; }

        /// <summary>
        /// オフ率
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.OffRate), ResourceType = typeof(ShpDcAllocationResource))]
        public int OffRate { get; set; }

        /// <summary>
        /// ブランドID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDcAllocationResource.BrandId), ResourceType = typeof(ShpDcAllocationResource))]
        public string BrandId { get; set; }

        #endregion
    }
}