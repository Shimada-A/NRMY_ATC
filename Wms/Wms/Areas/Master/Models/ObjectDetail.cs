namespace Wms.Areas.Master.Models
{
    using Share.Common.Resources;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    /// <summary>
    /// オブジェクト明細
    /// </summary>
    [Table("M_OBJECT_DETAILS")]
    public partial class ObjectDetail : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 削除フラグ (DELETE_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未削除 1:削除済み
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ObjectDetailResource.DeleteFlag), ResourceType = typeof(ObjectDetailResource))]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　0905,0924,0942
        /// </remarks>
        [Key]
        [Column(Order = 13)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ObjectDetailResource.CenterId), ResourceType = typeof(ObjectDetailResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// オブジェクトID (OBJECT_ID)
        /// </summary>
        [Key]
        [Column(Order = 100)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ObjectDetailResource.ObjectId), ResourceType = typeof(ObjectDetailResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ObjectId { get; set; }

        /// <summary>
        /// カラムID (COLUMN_ID)
        /// </summary>
        [Key]
        [Column(Order = 101)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ObjectDetailResource.ColumnId), ResourceType = typeof(ObjectDetailResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ColumnId { get; set; }

        /// <summary>
        /// カラム名 (COLUMN_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ObjectDetailResource.ColumnName), ResourceType = typeof(ObjectDetailResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ColumnName { get; set; }

        /// <summary>
        /// カラム№ (COLUMN_NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ObjectDetailResource.ColumnNo), ResourceType = typeof(ObjectDetailResource))]
        [Range(-9999, 9999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int ColumnNo { get; set; }

        /// <summary>
        /// データ型 (DATA_TYPE)
        /// </summary>
        /// <remarks>
        /// 0:文字列　1:数値
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ObjectDetailResource.DataType), ResourceType = typeof(ObjectDetailResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public Common.DataType DataType { get; set; }

        /// <summary>
        /// 桁数 (DIGIT_INT)
        /// </summary>
        [Display(Name = nameof(ObjectDetailResource.DigitInt), ResourceType = typeof(ObjectDetailResource))]
        [Range(-99999, 99999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? DigitInt { get; set; }

        /// <summary>
        /// 小数点以下桁数 (DIGIT_DEC)
        /// </summary>
        [Display(Name = nameof(ObjectDetailResource.DigitDec), ResourceType = typeof(ObjectDetailResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte? DigitDec { get; set; }

        /// <summary>
        /// 使用フラグ (USE_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:利用不可　1:利用可
        /// </remarks>
        [Display(Name = nameof(ObjectDetailResource.UseFlag), ResourceType = typeof(ObjectDetailResource))]
        public bool? UseFlag { get; set; }

        /// <summary>
        /// プライマリキーフラグ (PRIMARY_FLAG)
        /// </summary>
        [Display(Name = nameof(ObjectDetailResource.PrimaryFlag), ResourceType = typeof(ObjectDetailResource))]
        public bool? PrimaryFlag { get; set; }

        /// <summary>
        /// 必須フラグ (REQUIRED_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:任意項目　1:必須項目
        /// </remarks>
        [Display(Name = nameof(ObjectDetailResource.RequiredFlag), ResourceType = typeof(ObjectDetailResource))]
        public bool? RequiredFlag { get; set; }

        #endregion
    }
}
