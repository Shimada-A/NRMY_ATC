namespace Wms.Areas.Master.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    /// <summary>
    /// 汎用コード
    /// </summary>
    [Table("M_GENERALS")]
    public partial class General : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　0905,0924,0942
        /// </remarks>
        [Key]
        [Column(Order = 14)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(GeneralResource.CenterId), ResourceType = typeof(GeneralResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 登録分類コード (REGISTER_DIVI_CD)
        /// </summary>
        /// <remarks>
        /// 0:汎用タイトル 1:データ行
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(GeneralResource.RegisterDiviCd), ResourceType = typeof(GeneralResource))]
        [MaxLength(1, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string RegisterDiviCd { get; set; }

        /// <summary>
        /// 汎用分類コード (GEN_DIV_CD)
        /// </summary>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(GeneralResource.GenDivCd), ResourceType = typeof(GeneralResource))]
        [MaxLength(30, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string GenDivCd { get; set; }

        /// <summary>
        /// 汎用コード (GEN_CD)
        /// </summary>
        [Key]
        [Column(Order = 13)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(GeneralResource.GenCd), ResourceType = typeof(GeneralResource))]
        [MaxLength(30, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string GenCd { get; set; }

        /// <summary>
        /// 汎用値 (GEN_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(GeneralResource.GenName), ResourceType = typeof(GeneralResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string GenName { get; set; }

        /// <summary>
        /// 並び順 (ORDER_NO)
        /// </summary>
        /// <remarks>
        /// 登録分類コード=”1”のみ設定。
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(GeneralResource.OrderNo), ResourceType = typeof(GeneralResource))]
        [Range(0, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? OrderNo { get; set; }

        /// <summary>
        /// 汎用コード桁数 (GEN_CD_DIGITS)
        /// </summary>
        /// <remarks>
        /// 登録分類コード=”0”のみ設定。
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(GeneralResource.GenCdDigits), ResourceType = typeof(GeneralResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int GenCdDigits { get; set; }

        /// <summary>
        /// 汎用値桁数 (GEN_NAME_DIGITS)
        /// </summary>
        /// <remarks>
        /// 登録分類コード=”0”のみ設定。
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(GeneralResource.GenNameDigits), ResourceType = typeof(GeneralResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int GenNameDigits { get; set; }

        /// <summary>
        /// メンテナンス可能フラグ (MAINTENANCE_FLAG)
        /// </summary>
        /// <remarks>
        /// 0：メンテナンス不可能、1：メンテナンス可能
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(GeneralResource.MaintenanceFlag), ResourceType = typeof(GeneralResource))]
        public bool MaintenanceFlag { get; set; }

        #endregion プロパティ
    }
}