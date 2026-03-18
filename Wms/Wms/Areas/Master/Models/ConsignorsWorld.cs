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
    /// 荷送人(WS)
    /// </summary>
    [Table("M_CONSIGNORS_WORLD")]
    public partial class ConsignorsWorld : BaseModel
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
        [Display(Name = nameof(ConsignorsWorldResource.CenterId), ResourceType = typeof(ConsignorsWorldResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 荷送人コード (CONSIGNOR_ID)
        /// </summary>
        /// <remarks>
        /// ワールドサプライ用　お客様コード
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConsignorsWorldResource.ConsignorId), ResourceType = typeof(ConsignorsWorldResource))]
        [MaxLength(8, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorId { get; set; }

        /// <summary>
        /// 名称1 (CONSIGNOR_NAME1)
        /// </summary>
        /// <remarks>
        /// 荷送人名称（全角で設定されること！）
        /// </remarks>
        [Display(Name = nameof(ConsignorsWorldResource.ConsignorName1), ResourceType = typeof(ConsignorsWorldResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorName1 { get; set; }

        /// <summary>
        /// 名称2 (CONSIGNOR_NAME2)
        /// </summary>
        /// <remarks>
        /// 荷送人名称（全角で設定されること！）
        /// </remarks>
        [Display(Name = nameof(ConsignorsWorldResource.ConsignorName2), ResourceType = typeof(ConsignorsWorldResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorName2 { get; set; }

        /// <summary>
        /// 荷送人郵便番号 (CONSIGNOR_ZIP)
        /// </summary>
        /// <remarks>
        /// ハイフンなし７桁でセットされること
        /// </remarks>
        [Display(Name = nameof(ConsignorsWorldResource.ConsignorZip), ResourceType = typeof(ConsignorsWorldResource))]
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorZip { get; set; }

        /// <summary>
        /// 荷送人電話番号 (CONSIGNOR_TEL)
        /// </summary>
        [Display(Name = nameof(ConsignorsWorldResource.ConsignorTel), ResourceType = typeof(ConsignorsWorldResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorTel { get; set; }

        /// <summary>
        /// 荷送人FAX番号 (CONSIGNOR_FAX)
        /// </summary>
        [Display(Name = nameof(ConsignorsWorldResource.ConsignorFax), ResourceType = typeof(ConsignorsWorldResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorFax { get; set; }

        /// <summary>
        /// 荷送人住所１ (CONSIGNOR_ADDRESS1)
        /// </summary>
        /// <remarks>
        /// 全角で設定されること！（EDIデータ、送り状作成のため）
        /// </remarks>
        [Display(Name = nameof(ConsignorsWorldResource.ConsignorAddress1), ResourceType = typeof(ConsignorsWorldResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorAddress1 { get; set; }

        /// <summary>
        /// 荷送人住所２ (CONSIGNOR_ADDRESS2)
        /// </summary>
        /// <remarks>
        /// 全角で設定されること！（EDIデータ、送り状作成のため）
        /// </remarks>
        [Display(Name = nameof(ConsignorsWorldResource.ConsignorAddress2), ResourceType = typeof(ConsignorsWorldResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorAddress2 { get; set; }

        #endregion
    }
}
