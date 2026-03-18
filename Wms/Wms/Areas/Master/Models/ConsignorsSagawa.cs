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
    /// 荷送人(佐川)
    /// </summary>
    [Table("M_CONSIGNORS_SAGAWA")]
    public partial class ConsignorsSagawa : BaseModel
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
        [Display(Name = nameof(ConsignorsSagawaResource.CenterId), ResourceType = typeof(ConsignorsSagawaResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// お客様コード (CLIENT_CD)
        /// </summary>
        /// <remarks>
        /// 【佐川EDI用顧客コード、送り状お客様コード】  
        /// 　　　 顧客コード　 枝番　C/D　佐川より受領資料
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConsignorsSagawaResource.ClientCd), ResourceType = typeof(ConsignorsSagawaResource))]
        [MaxLength(12, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientCd { get; set; }

        /// <summary>
        /// 名称1 (CONSIGNOR_NAME1)
        /// </summary>
        /// <remarks>
        /// 荷送人名称（全角で設定されること！）
        /// </remarks>
        [Display(Name = nameof(ConsignorsSagawaResource.ConsignorName1), ResourceType = typeof(ConsignorsSagawaResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorName1 { get; set; }

        /// <summary>
        /// 名称2 (CONSIGNOR_NAME2)
        /// </summary>
        /// <remarks>
        /// 荷送人名称（全角で設定されること！）
        /// </remarks>
        [Display(Name = nameof(ConsignorsSagawaResource.ConsignorName2), ResourceType = typeof(ConsignorsSagawaResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorName2 { get; set; }

        /// <summary>
        /// 荷送人郵便番号 (CONSIGNOR_ZIP)
        /// </summary>
        /// <remarks>
        /// ハイフンなし７桁でセットされること
        /// </remarks>
        [Display(Name = nameof(ConsignorsSagawaResource.ConsignorZip), ResourceType = typeof(ConsignorsSagawaResource))]
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorZip { get; set; }

        /// <summary>
        /// 荷送人電話番号 (CONSIGNOR_TEL)
        /// </summary>
        [Display(Name = nameof(ConsignorsSagawaResource.ConsignorTel), ResourceType = typeof(ConsignorsSagawaResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorTel { get; set; }

        /// <summary>
        /// 荷送人FAX番号 (CONSIGNOR_FAX)
        /// </summary>
        [Display(Name = nameof(ConsignorsSagawaResource.ConsignorFax), ResourceType = typeof(ConsignorsSagawaResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorFax { get; set; }

        /// <summary>
        /// 荷送人住所１ (CONSIGNOR_ADDRESS1)
        /// </summary>
        /// <remarks>
        /// 全角で設定されること！（EDIデータ、送り状作成のため）
        /// </remarks>
        [Display(Name = nameof(ConsignorsSagawaResource.ConsignorAddress1), ResourceType = typeof(ConsignorsSagawaResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorAddress1 { get; set; }

        /// <summary>
        /// 荷送人住所２ (CONSIGNOR_ADDRESS2)
        /// </summary>
        /// <remarks>
        /// 全角で設定されること！（EDIデータ、送り状作成のため）
        /// </remarks>
        [Display(Name = nameof(ConsignorsSagawaResource.ConsignorAddress2), ResourceType = typeof(ConsignorsSagawaResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorAddress2 { get; set; }

        /// <summary>
        /// 営業所名 (SALES_OFFICE_NAME)
        /// </summary>
        /// <remarks>
        /// 【佐川送り状用】  営業所名
        /// </remarks>
        [Display(Name = nameof(ConsignorsSagawaResource.SalesOfficeName), ResourceType = typeof(ConsignorsSagawaResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string SalesOfficeName { get; set; }

        /// <summary>
        /// 営業所TEL (SALES_OFFICE_TEL)
        /// </summary>
        /// <remarks>
        /// 【佐川送り状用】
        /// </remarks>
        [Display(Name = nameof(ConsignorsSagawaResource.SalesOfficeTel), ResourceType = typeof(ConsignorsSagawaResource))]
        [MaxLength(15, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string SalesOfficeTel { get; set; }

        /// <summary>
        /// 営業所FAX (SALES_OFFICE_FAX)
        /// </summary>
        /// <remarks>
        /// 【佐川送り状用】
        /// </remarks>
        [Display(Name = nameof(ConsignorsSagawaResource.SalesOfficeFax), ResourceType = typeof(ConsignorsSagawaResource))]
        [MaxLength(15, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string SalesOfficeFax { get; set; }

        /// <summary>
        /// 発店精算コード (HATTEN_SEISAN_CD)
        /// </summary>
        /// <remarks>
        /// 【佐川EDI用顧客コード】 佐川より受領資料
        /// 　　　発店清算コード　　枝番
        /// </remarks>
        [Display(Name = nameof(ConsignorsSagawaResource.HattenSeisanCd), ResourceType = typeof(ConsignorsSagawaResource))]
        [MaxLength(4, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string HattenSeisanCd { get; set; }

        /// <summary>
        /// 発店精算コード枝番 (HATTEN_SEISAN_CD_EDA)
        /// </summary>
        /// <remarks>
        /// 【佐川発店清算コード枝番】  上記
        /// </remarks>
        [Display(Name = nameof(ConsignorsSagawaResource.HattenSeisanCdEda), ResourceType = typeof(ConsignorsSagawaResource))]
        [MaxLength(2, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string HattenSeisanCdEda { get; set; }

        #endregion
    }
}
