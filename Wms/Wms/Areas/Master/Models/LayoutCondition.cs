namespace Wms.Areas.Master.Models
{
    using Share.Common.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    /// <summary>
    /// レイアウト出力条件
    /// </summary>
    [Table("M_LAYOUT_CONDITIONS")]
    public partial class LayoutCondition : BaseModel
    {
        #region プロパティ


        /// <summary>
        /// 削除フラグ (DELETE_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未削除 1:削除済み
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutConditionResource.DeleteFlag), ResourceType = typeof(LayoutConditionResource))]
        public byte DeleteFlag { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　0905,0924,0942
        /// </remarks>
        [Key]
        [Column(Order = 13)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutConditionResource.CenterId), ResourceType = typeof(LayoutConditionResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// テンプレートID (TEMPLATE_ID)
        /// </summary>
        /// <remarks>
        /// テンプレート単位の連番
        /// </remarks>
        [Key]
        [Column(Order = 100)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutConditionResource.TemplateId), ResourceType = typeof(LayoutConditionResource))]
        [Range(-99999999999999999, 99999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long TemplateId { get; set; }

        /// <summary>
        /// オブジェクトID (OBJECT_ID)
        /// </summary>
        [Key]
        [Column(Order = 101)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutConditionResource.ObjectId), ResourceType = typeof(LayoutConditionResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ObjectId { get; set; }

        /// <summary>
        /// カラムID (COLUMN_ID)
        /// </summary>
        [Key]
        [Column(Order = 102)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutConditionResource.ColumnId), ResourceType = typeof(LayoutConditionResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ColumnId { get; set; }

        /// <summary>
        /// 条件区分 (CONDITION_CLASS)
        /// </summary>
        /// <remarks>
        /// 0:制約なし　1:イコール　2:ノットイコール　3:以上　4:以下
        /// 5:範囲　6:あいまい　7:当日　8:当月
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutConditionResource.ConditionClass), ResourceType = typeof(LayoutConditionResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte ConditionClass { get; set; }

        /// <summary>
        /// 条件値FROM (CONDITION_VALUE_FROM)
        /// </summary>
        [Display(Name = nameof(LayoutConditionResource.ConditionValueFrom), ResourceType = typeof(LayoutConditionResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConditionValueFrom { get; set; }

        /// <summary>
        /// 条件値TO (CONDITION_VALUE_TO)
        /// </summary>
        [Display(Name = nameof(LayoutConditionResource.ConditionValueTo), ResourceType = typeof(LayoutConditionResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConditionValueTo { get; set; }

        /// <summary>
        /// 並び順 (SORT_ORDER)
        /// </summary>
        [Display(Name = nameof(LayoutConditionResource.SortOrder), ResourceType = typeof(LayoutConditionResource))]
        [Range(-999, 999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? SortOrder { get; set; }

        /// <summary>
        /// 並び方向 (SORT_DIRECTION)
        /// </summary>
        /// <remarks>
        /// 1:昇順　2:降順
        /// </remarks>
        [Display(Name = nameof(LayoutConditionResource.SortDirection), ResourceType = typeof(LayoutConditionResource))]
        public byte? SortDirection { get; set; }

        #endregion
    }
}
