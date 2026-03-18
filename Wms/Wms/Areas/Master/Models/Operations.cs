namespace Wms.Areas.Master.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    /// <summary>
    /// 業務マスタ
    /// </summary>
    [Table("M_OPERATIONS")]
    public partial class Operations : BaseModel
    {
        #region プロパティ


        /// <summary>
        /// 業務ID (OPERATION_ID)
        /// </summary>
        /// <remarks>
        /// WMS業務外　は3桁（メンテナンス画面でメンテされる）
        /// WMS業務内　は4桁（スマホ出荷作業など。IDはシステムで固定）
        /// 　1001：トータルピック
        /// 　1002：店別ピック
        /// 　1003：店仕分
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(OperationResource.OperationId), ResourceType = typeof(OperationResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string OperationId { get; set; }

        /// <summary>
        /// 業務名 (OPERATION_NAME)
        /// </summary>
        [Display(Name = nameof(OperationResource.OperationName), ResourceType = typeof(OperationResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string OperationName { get; set; }

        /// <summary>
        /// 業務カテゴリ名 (CATEGORY_NAME)
        /// </summary>
        [Display(Name = nameof(OperationResource.CategoryName), ResourceType = typeof(OperationResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryName { get; set; }

        /// <summary>
        /// 数量０許可フラグ (QTY_ZERO_FLAG)
        /// </summary>
        /// <remarks>
        /// 0：数量0を許可しない、1：数量0を許可する
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(OperationResource.QtyZeroFlag), ResourceType = typeof(OperationResource))]
        public bool QtyZeroFlag { get; set; }

        /// <summary>
        /// WMS業務フラグ (WMS_OPERATION_FLAG)
        /// </summary>
        /// <remarks>
        /// 0：WMS業務外、1：WMS業務
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(OperationResource.WmsOperationFlag), ResourceType = typeof(OperationResource))]
        public bool WmsOperationFlag { get; set; }

        #endregion プロパティ
    }
}