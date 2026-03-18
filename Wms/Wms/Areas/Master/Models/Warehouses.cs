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
    /// 倉庫
    /// </summary>
    [Table("M_CENTERS")]
    public partial class Warehouses : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード
        /// IF倉庫マスタ.倉庫コード[文字(3)]
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(WarehousesResource.CenterId), ResourceType = typeof(WarehousesResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 倉庫区分 (CENTER_CLASS)
        /// </summary>
        /// <remarks>
        ///  8:倉庫
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(WarehousesResource.CenterClass), ResourceType = typeof(WarehousesResource))]
        [MaxLength(2, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterClass { get; set; }

        /// <summary>
        /// 倉庫名1 (CENTER_NAME1)
        /// </summary>
        /// <remarks>
        /// IF倉庫マスタ.倉庫名[文字(60)]
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(WarehousesResource.CenterName1), ResourceType = typeof(WarehousesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterName1 { get; set; }

        /// <summary>
        /// 倉庫名2 (CENTER_NAME2)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.CenterName2), ResourceType = typeof(WarehousesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterName2 { get; set; }

        /// <summary>
        /// 倉庫名(略称) (CENTER_SHORT_NAME)
        /// </summary>
        /// <remarks>
        /// WMS側ハンディで使用する。全角５文字以下とする。(セット時チェックする
        /// 画面からメンテナンスする
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(WarehousesResource.CenterShortName), ResourceType = typeof(WarehousesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterShortName { get; set; }

        /// <summary>
        /// 倉庫名1(カナ) (CENTER_KANA_NAME1)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.CenterKanaName1), ResourceType = typeof(WarehousesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterKanaName1 { get; set; }

        /// <summary>
        /// 倉庫名2(カナ) (CENTER_KANA_NAME2)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.CenterKanaName2), ResourceType = typeof(WarehousesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterKanaName2 { get; set; }

        /// <summary>
        /// 倉庫名(略称カナ) (CENTER_KANA_SHORT_NAME)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.CenterKanaShortName), ResourceType = typeof(WarehousesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterKanaShortName { get; set; }

        /// <summary>
        /// 倉庫国 (CENTER_COUNTRY)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.CenterCountry), ResourceType = typeof(WarehousesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterCountry { get; set; }

        /// <summary>
        /// 倉庫郵便番号 (CENTER_ZIP)
        /// </summary>
        /// <remarks>
        /// ハイフン抜きで登録する
        /// 画面からメンテナンスする
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(WarehousesResource.CenterZip), ResourceType = typeof(WarehousesResource))]
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterZip { get; set; }

        /// <summary>
        /// 倉庫都道府県 (CENTER_PREF_NAME)
        /// </summary>
        /// <remarks>
        /// 画面からメンテナンスする
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(WarehousesResource.CenterPrefName), ResourceType = typeof(WarehousesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterPrefName { get; set; }

        /// <summary>
        /// 倉庫都道府県(カナ) (CENTER_PREF_KANA_NAME)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.CenterPrefKanaName), ResourceType = typeof(WarehousesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterPrefKanaName { get; set; }

        /// <summary>
        /// 倉庫市区町村 (CENTER_CITY_NAME)
        /// </summary>
        /// <remarks>
        /// 画面からメンテナンスする
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(WarehousesResource.CenterCityName), ResourceType = typeof(WarehousesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterCityName { get; set; }

        /// <summary>
        /// 倉庫市区町村(カナ) (CENTER_CITY_KANA_NAME)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.CenterCityKanaName), ResourceType = typeof(WarehousesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterCityKanaName { get; set; }

        /// <summary>
        /// 倉庫それ以降の住所1 (CENTER_ADDRESS1)
        /// </summary>
        /// <remarks>
        /// 画面からメンテナンスする
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(WarehousesResource.CenterAddress1), ResourceType = typeof(WarehousesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterAddress1 { get; set; }

        /// <summary>
        /// 倉庫それ以降の住所1(カナ) (CENTER_KANA_ADDRESS1)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.CenterKanaAddress1), ResourceType = typeof(WarehousesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterKanaAddress1 { get; set; }

        /// <summary>
        /// 倉庫それ以降の住所2 (CENTER_ADDRESS2)
        /// </summary>
        /// <remarks>
        /// 画面からメンテナンスする
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.CenterAddress2), ResourceType = typeof(WarehousesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterAddress2 { get; set; }

        /// <summary>
        /// 倉庫それ以降の住所2(カナ) (CENTER_KANA_ADDRESS2)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.CenterKanaAddress2), ResourceType = typeof(WarehousesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterKanaAddress2 { get; set; }

        /// <summary>
        /// 倉庫それ以降の住所3 (CENTER_ADDRESS3)
        /// </summary>
        /// <remarks>
        /// 画面からメンテナンスする
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.CenterAddress3), ResourceType = typeof(WarehousesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterAddress3 { get; set; }

        /// <summary>
        /// 倉庫それ以降の住所3(カナ) (CENTER_KANA_ADDRESS3)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.CenterKanaAddress3), ResourceType = typeof(WarehousesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterKanaAddress3 { get; set; }

        /// <summary>
        /// 倉庫TEL (CENTER_TEL)
        /// </summary>
        /// <remarks>
        /// 画面からメンテナンスする
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.CenterTel), ResourceType = typeof(WarehousesResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterTel { get; set; }

        /// <summary>
        /// 倉庫FAX (CENTER_FAX)
        /// </summary>
        /// <remarks>
        /// 画面からメンテナンスする
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.CenterFax), ResourceType = typeof(WarehousesResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterFax { get; set; }

        /// <summary>
        /// 倉庫Mail1 (CENTER_MAIL1)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.CenterMail1), ResourceType = typeof(WarehousesResource))]
        [MaxLength(300, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterMail1 { get; set; }

        /// <summary>
        /// 倉庫Mail2 (CENTER_MAIL2)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.CenterMail2), ResourceType = typeof(WarehousesResource))]
        [MaxLength(300, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterMail2 { get; set; }

        /// <summary>
        /// 都道府県コード (PREF_ID)
        /// </summary>
        /// <remarks>
        /// 登録時セットする
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.PrefId), ResourceType = typeof(WarehousesResource))]
        [MaxLength(2, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PrefId { get; set; }

        /// <summary>
        /// チャネルID (CHANNEL_ID)
        /// </summary>
        /// <remarks>
        /// IF倉庫マスタ.チャネルコード[文字(2)]
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.ChannelId), ResourceType = typeof(WarehousesResource))]
        [MaxLength(3, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ChannelId { get; set; }

        /// <summary>
        /// チャネル名 (CHANNEL_NAME)
        /// </summary>
        /// <remarks>
        /// IF倉庫マスタ.チャネル名[文字(60)]
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.ChannelName), ResourceType = typeof(WarehousesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ChannelName { get; set; }

        /// <summary>
        /// WMS対象倉庫区分 (WMS_CLASS)
        /// </summary>
        /// <remarks>
        /// 0:WMS対象外、1:WMS対象
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(WarehousesResource.WmsClass), ResourceType = typeof(WarehousesResource))]
        [MaxLength(1, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string WmsClass { get; set; }

        /// <summary>
        /// ブランド別作業区分 (BRAND_WORK_CLASS)
        /// </summary>
        /// <remarks>
        /// 0:ブランド別に作業しない、1:ブランド別に作業する
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(WarehousesResource.BrandWorkClass), ResourceType = typeof(WarehousesResource))]
        [MaxLength(1, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BrandWorkClass { get; set; }

        /// <summary>
        /// 削除フラグ (DELETE_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未削除 1:削除済み
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(WarehousesResource.DeleteFlag), ResourceType = typeof(WarehousesResource))]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 送り状用倉庫名 (INVOICE_CENTER_NAME)
        /// </summary>
        /// <remarks>
        /// IF倉庫マスタ.倉庫名[文字(60)]
        /// 画面からメンテナンスする
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.InvoiceCenterName), ResourceType = typeof(WarehousesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InvoiceCenterName { get; set; }

        #endregion
    }
}
