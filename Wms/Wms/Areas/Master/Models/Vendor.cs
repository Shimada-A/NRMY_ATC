namespace Wms.Areas.Master.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Common;
    using Wms.Models;

    /// <summary>
    /// 仕入先
    /// </summary>
    [Table("M_VENDORS")]
    public partial class Vendor : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 削除フラグ (DELETE_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未削除 1:削除済み
        /// </remarks>
        [Display(Name = nameof(Resources.VendorResource.DeleteFlag), ResourceType = typeof(Resources.VendorResource))]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 仕入先ID (VENDOR_ID)
        /// </summary>
        /// <remarks>
        /// （仕入先コード）
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Display(Name = nameof(Resources.VendorResource.VendorId), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先名1 (VENDOR_NAME1)
        /// </summary>
        [Display(Name = nameof(Resources.VendorResource.VendorName1), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorName1 { get; set; }

        /// <summary>
        /// 仕入先名2 (VENDOR_NAME2)
        /// </summary>
        [Display(Name = nameof(Resources.VendorResource.VendorName2), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorName2 { get; set; }

        /// <summary>
        /// 仕入先名(略称) (VENDOR_SHORT_NAME)
        /// </summary>
        [Display(Name = nameof(Resources.VendorResource.VendorShortName), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorShortName { get; set; }

        /// <summary>
        /// 仕入先名1(カナ) (VENDOR_KANA_NAME1)
        /// </summary>
        [Display(Name = nameof(Resources.VendorResource.VendorKanaName1), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorKanaName1 { get; set; }

        /// <summary>
        /// 仕入先名2(カナ) (VENDOR_KANA_NAME2)
        /// </summary>
        [Display(Name = nameof(Resources.VendorResource.VendorKanaName2), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorKanaName2 { get; set; }

        /// <summary>
        /// 仕入先名(略称カナ) (VENDOR_KANA_SHORT_NAME)
        /// </summary>
        [Display(Name = nameof(Resources.VendorResource.VendorKanaShortName), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorKanaShortName { get; set; }

        /// <summary>
        /// 仕入先国 (VENDOR_COUNTRY)
        /// </summary>
        [Display(Name = nameof(Resources.VendorResource.VendorCountry), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorCountry { get; set; }

        /// <summary>
        /// 仕入先郵便番号 (VENDOR_ZIP)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.VendorResource.VendorZip), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorZip { get; set; }

        /// <summary>
        /// 仕入先都道府県 (VENDOR_PREF_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.VendorResource.VendorPrefName), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorPrefName { get; set; }

        /// <summary>
        /// 仕入先都道府県(カナ) (VENDOR_PREF_KANA_NAME)
        /// </summary>
        [Display(Name = nameof(Resources.VendorResource.VendorPrefKanaName), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorPrefKanaName { get; set; }

        /// <summary>
        /// 仕入先市区町村 (VENDOR_CITY_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.VendorResource.VendorCityName), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorCityName { get; set; }

        /// <summary>
        /// 仕入先市区町村(カナ) (VENDOR_CITY_KANA_NAME)
        /// </summary>
        [Display(Name = nameof(Resources.VendorResource.VendorCityKanaName), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorCityKanaName { get; set; }

        /// <summary>
        /// 仕入先それ以降の住所1 (VENDOR_ADDRESS1)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.VendorResource.VendorAddress1), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorAddress1 { get; set; }

        /// <summary>
        /// 仕入先それ以降の住所1(カナ) (VENDOR_KANA_ADDRESS1)
        /// </summary>
        [Display(Name = nameof(Resources.VendorResource.VendorKanaAddress1), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorKanaAddress1 { get; set; }

        /// <summary>
        /// 仕入先それ以降の住所2 (VENDOR_ADDRESS2)
        /// </summary>
        [Display(Name = nameof(Resources.VendorResource.VendorAddress2), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorAddress2 { get; set; }

        /// <summary>
        /// 仕入先それ以降の住所2(カナ) (VENDOR_KANA_ADDRESS2)
        /// </summary>
        [Display(Name = nameof(Resources.VendorResource.VendorKanaAddress2), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorKanaAddress2 { get; set; }

        /// <summary>
        /// 仕入先それ以降の住所3 (VENDOR_ADDRESS3)
        /// </summary>
        [Display(Name = nameof(Resources.VendorResource.VendorAddress3), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorAddress3 { get; set; }

        /// <summary>
        /// 仕入先それ以降の住所3(カナ) (VENDOR_KANA_ADDRESS3)
        /// </summary>
        [Display(Name = nameof(Resources.VendorResource.VendorKanaAddress3), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorKanaAddress3 { get; set; }

        /// <summary>
        /// TEL (VENDOR_TEL)
        /// </summary>
        [Display(Name = nameof(Resources.VendorResource.VendorTel), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorTel { get; set; }

        /// <summary>
        /// FAX (VENDOR_FAX)
        /// </summary>
        [Display(Name = nameof(Resources.VendorResource.VendorFax), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorFax { get; set; }

        /// <summary>
        /// Mail1 (VENDOR_MAIL1)
        /// </summary>
        [Display(Name = nameof(Resources.VendorResource.VendorMail1), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(300, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorMail1 { get; set; }

        /// <summary>
        /// Mail2 (VENDOR_MAIL2)
        /// </summary>
        [Display(Name = nameof(Resources.VendorResource.VendorMail2), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(300, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorMail2 { get; set; }

        /// <summary>
        /// 仕入先担当者名 (VENDOR_STAFF_NAME)
        /// </summary>
        [Display(Name = nameof(Resources.VendorResource.VendorStaffName), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(256, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string VendorStaffName { get; set; }

        /// <summary>
        /// 名義変更タイミング (CHANGE_ASSET_TIMING)
        /// </summary>
        /// <remarks>
        /// 1:荷主固定 2:入荷時名義変更 3:出荷時名義変更
        /// </remarks>
        [Display(Name = nameof(Resources.VendorResource.ChangeAssetTiming), ResourceType = typeof(Resources.VendorResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public ChangeAssetTimingEnum? ChangeAssetTiming { get; set; }

        /// <summary>
        /// 支払締日 (PAYMENT_CLOSING_DATE)
        /// </summary>
        /// <remarks>
        /// 末日の場合は99を設定
        /// </remarks>
        [Display(Name = nameof(Resources.VendorResource.PaymentClosingDate), ResourceType = typeof(Resources.VendorResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte? PaymentClosingDate { get; set; }

        /// <summary>
        /// 支払区分 (PAYMENT_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:当月 2:翌月 3:翌々月
        /// </remarks>
        [Display(Name = nameof(Resources.VendorResource.PaymentClass), ResourceType = typeof(Resources.VendorResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public PaymentClassEnum? PaymentClass { get; set; }

        /// <summary>
        /// 支払日 (PAYMENT_DATE)
        /// </summary>
        /// <remarks>
        /// 末日の場合は99を設定
        /// </remarks>
        [Display(Name = nameof(Resources.VendorResource.PaymentDate), ResourceType = typeof(Resources.VendorResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte? PaymentDate { get; set; }

        /// <summary>
        /// 支払種別 (PAYMENT_KIND_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:振込、2:手形、3:現金
        /// </remarks>
        [Display(Name = nameof(Resources.VendorResource.PaymentKindClass), ResourceType = typeof(Resources.VendorResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public PaymentKindClassEnum? PaymentKindClass { get; set; }

        /// <summary>
        /// 取引先区分 (SUPPLIER_CLASS)
        /// </summary>
        /// <remarks>
        /// 01：ODMﾒｰｶｰ、02：OEMﾒｰｶｰ、03：商社、04：直貿、99：その他
        /// </remarks>
        [Display(Name = nameof(Resources.VendorResource.SupplierClass), ResourceType = typeof(Resources.VendorResource))]
        [MaxLength(2, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string SupplierClass { get; set; }

        #endregion プロパティ
    }
}