namespace Wms.Areas.Master.ViewModels.BoxSetting
{

    using System.ComponentModel.DataAnnotations;
    using Share.Common.Resources;
    using Share.Extensions.Attributes;
    using Wms.Areas.Master.Resources;
    using Wms.Common;
    using Wms.Models;

    /// <summary>
    /// ケース閾値設定
    /// </summary>
    public class BoxSettingList : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 所在
        /// </summary>
        /// <remarks>
        /// （センター）
        /// </remarks>
        [Display(Name = nameof(BoxSettingResource.CenterName), ResourceType = typeof(BoxSettingResource))]
        public string CenterName { get; set; }

        /// <summary>
        /// 設定ID (BOX_SETTINGS_ID)
        /// </summary>
        /// <remarks>
        /// 連番　(重複不可項目については備考参照)
        /// </remarks>
        [Display(Name = nameof(BoxSettingResource.BoxSettingsId), ResourceType = typeof(BoxSettingResource))]
        public int BoxSettingsId { get; set; } = 0;

        /// <summary>
        /// 分類1 (CATEGORY_ID1)
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.CategoryId1), ResourceType = typeof(BoxSettingResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.CategoryId1), ResourceType = typeof(BoxSettingResource))]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 分類2 (CATEGORY_ID2)
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.CategoryId2), ResourceType = typeof(BoxSettingResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId2 { get; set; }

        /// <summary>
        /// 分類2
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.CategoryId2), ResourceType = typeof(BoxSettingResource))]
        public string CategoryName2 { get; set; }

        /// <summary>
        /// 分類3 (CATEGORY_ID3)
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.CategoryId3), ResourceType = typeof(BoxSettingResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId3 { get; set; }

        /// <summary>
        /// 分類3
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.CategoryId3), ResourceType = typeof(BoxSettingResource))]
        public string CategoryName3 { get; set; }

        /// <summary>
        /// 分類4 (CATEGORY_ID4)
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.CategoryId4), ResourceType = typeof(BoxSettingResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId4 { get; set; }

        /// <summary>
        /// 分類4
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.CategoryId4), ResourceType = typeof(BoxSettingResource))]
        public string CategoryName4 { get; set; }

        /// <summary>
        /// 商品ID(品番) (ITEM_ID)
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.ItemId), ResourceType = typeof(BoxSettingResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 商品
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.ItemId), ResourceType = typeof(BoxSettingResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// 閾値区分 (THRESHOLD_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:率(%) 2:ケース数
        /// </remarks>
        [Display(Name = nameof(BoxSettingResource.ThresholdClass), ResourceType = typeof(BoxSettingResource))]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public ThresholdClasses ThresholdClass { get; set; } = ThresholdClasses.None;

        /// <summary>
        /// 閾値 (THRESHOLD)
        /// </summary>
        /// <remarks>
        /// ケース保管閾値　この値を超えたらケース管理、以下はバラ
        /// </remarks>
        [Display(Name = nameof(BoxSettingResource.ThresholdSku), ResourceType = typeof(BoxSettingResource))]
        [DecimalChk("9", "0", "-999999999", "999999999")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ThresholdSku { get; set; }

        /// <summary>
        /// 閾値 (THRESHOLD)
        /// </summary>
        /// <remarks>
        /// ケース保管閾値　この値を超えたらケース管理、以下はバラ
        /// </remarks>
        [Display(Name = nameof(BoxSettingResource.ThresholdRate), ResourceType = typeof(BoxSettingResource))]
        [DecimalChk("9", "1", "0", "100")]
        [DisplayFormat(DataFormatString = "{0:N1}")]
        public decimal? ThresholdRate { get; set; }

        /// <summary>
        /// Msg
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.ErrMsg), ResourceType = typeof(BoxSettingResource))]
        public string ErrMsg { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        [Display(Name = nameof(MasBoxSettingResource.Seq), ResourceType = typeof(MasBoxSettingResource))]
        public long Seq { get; set; }

        /// <summary>
        /// NO (NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        [Display(Name = nameof(MasBoxSettingResource.No), ResourceType = typeof(MasBoxSettingResource))]
        public long No { get; set; }

        /// <summary>
        /// 検索済み判断フラグ
        /// </summary>
        public bool SearchFlag { get; set; }
        #endregion プロパティ
    }
}