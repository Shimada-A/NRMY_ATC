namespace Wms.Areas.Master.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Foolproof;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Attributes;
    using Wms.Areas.Master.Resources;
    using Wms.Common;
    using Wms.Models;

    /// <summary>
    /// ケース閾値設定
    /// </summary>
    [Table("M_BOX_SETTINGS")]
    public partial class BoxSetting : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 設定ID (BOX_SETTINGS_ID)
        /// </summary>
        /// <remarks>
        /// 連番　(重複不可項目については備考参照)
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(BoxSettingResource.BoxSettingsId), ResourceType = typeof(BoxSettingResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int BoxSettingsId { get; set; } = 0;

        /// <summary>
        /// 分類1 (CATEGORY_ID1)
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.CategoryId1), ResourceType = typeof(BoxSettingResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類2 (CATEGORY_ID2)
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.CategoryId2), ResourceType = typeof(BoxSettingResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId2 { get; set; }

        /// <summary>
        /// 分類3 (CATEGORY_ID3)
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.CategoryId3), ResourceType = typeof(BoxSettingResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId3 { get; set; }

        /// <summary>
        /// 分類4 (CATEGORY_ID4)
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.CategoryId4), ResourceType = typeof(BoxSettingResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId4 { get; set; }

        /// <summary>
        /// 商品ID(品番) (ITEM_ID)
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.ItemId), ResourceType = typeof(BoxSettingResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 閾値区分 (THRESHOLD_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:率(%) 2:ケース数
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(BoxSettingResource.ThresholdClass), ResourceType = typeof(BoxSettingResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public ThresholdClasses ThresholdClass { get; set; }

        /// <summary>
        /// 閾値 (THRESHOLD)
        /// </summary>
        /// <remarks>
        /// ケース保管閾値　この値を超えたらケース管理、以下はバラ
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(BoxSettingResource.Threshold), ResourceType = typeof(BoxSettingResource))]
        public decimal? Threshold { get; set; }

        #endregion
    }
}