namespace Wms.Areas.Master.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;
    using Wms.Resources;

    /// <summary>
    /// 荷送人(浪速)
    /// </summary>
    [Table("M_CONSIGNORS_NANIWA")]
    public partial class ConsignorsNaniwa : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　
        /// </remarks>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConsignorsNaniwaResource.CenterId), ResourceType = typeof(ConsignorsNaniwaResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 管理番号 (CONTROL_ID)
        /// </summary>
        /// <remarks>
        /// 浪速EDI用　管理ID
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConsignorsNaniwaResource.ControlId), ResourceType = typeof(ConsignorsNaniwaResource))]
        [MaxLength(6, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ControlId { get; set; }

        /// <summary>
        /// 名称1 (CONSIGNOR_NAME1)
        /// </summary>
        /// <remarks>
        /// 荷送人名称（全角で設定されること！）
        /// </remarks>
        [Display(Name = nameof(ConsignorsNaniwaResource.ConsignorName1), ResourceType = typeof(ConsignorsNaniwaResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorName1 { get; set; }

        /// <summary>
        /// 名称2 (CONSIGNOR_NAME2)
        /// </summary>
        /// <remarks>
        /// 荷送人名称（全角で設定されること！）
        /// </remarks>
        [Display(Name = nameof(ConsignorsNaniwaResource.ConsignorName2), ResourceType = typeof(ConsignorsNaniwaResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorName2 { get; set; }

        /// <summary>
        /// 荷送人郵便番号 (CONSIGNOR_ZIP)
        /// </summary>
        /// <remarks>
        /// ハイフンなし７桁でセットされること
        /// </remarks>
        [Display(Name = nameof(ConsignorsNaniwaResource.ConsignorZip), ResourceType = typeof(ConsignorsNaniwaResource))]
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorZip { get; set; }

        /// <summary>
        /// 荷送人電話番号 (CONSIGNOR_TEL)
        /// </summary>
        [Display(Name = nameof(ConsignorsNaniwaResource.ConsignorTel), ResourceType = typeof(ConsignorsNaniwaResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorTel { get; set; }

        /// <summary>
        /// 荷送人FAX番号 (CONSIGNOR_FAX)
        /// </summary>
        [Display(Name = nameof(ConsignorsNaniwaResource.ConsignorFax), ResourceType = typeof(ConsignorsNaniwaResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorFax { get; set; }

        /// <summary>
        /// 荷送人住所１ (CONSIGNOR_ADDRESS1)
        /// </summary>
        /// <remarks>
        /// 全角で設定されること！（EDIデータ、送り状作成のため）
        /// </remarks>
        [Display(Name = nameof(ConsignorsNaniwaResource.ConsignorAddress1), ResourceType = typeof(ConsignorsNaniwaResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorAddress1 { get; set; }

        /// <summary>
        /// 荷送人住所２ (CONSIGNOR_ADDRESS2)
        /// </summary>
        /// <remarks>
        /// 全角で設定されること！（EDIデータ、送り状作成のため）
        /// </remarks>
        [Display(Name = nameof(ConsignorsNaniwaResource.ConsignorAddress2), ResourceType = typeof(ConsignorsNaniwaResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorAddress2 { get; set; }

        /// <summary>
        /// 基礎番号 (BASE_NO)
        /// </summary>
        /// <remarks>
        /// 浪速　送り状NO用　基礎番号
        /// </remarks>
        [Display(Name = nameof(ConsignorsNaniwaResource.BaseNo), ResourceType = typeof(ConsignorsNaniwaResource))]
        [MaxLength(3, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BaseNo { get; set; }

        #endregion
    }
}
