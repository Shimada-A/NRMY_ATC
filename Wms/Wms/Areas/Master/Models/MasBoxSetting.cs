namespace Wms.Areas.Master.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    /// <summary>
    /// ケース閾値設定マスタワーク
    /// </summary>
    [Table("WW_MAS_BOX_SETTINGS")]
    public partial class MasBoxSetting : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(MasBoxSettingResource.Seq), ResourceType = typeof(MasBoxSettingResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// NO (NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(MasBoxSettingResource.No), ResourceType = typeof(MasBoxSettingResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long No { get; set; }

        /// <summary>
        /// 分類1 (CATEGORY_ID1)
        /// </summary>
        [Display(Name = nameof(MasBoxSettingResource.CategoryId1), ResourceType = typeof(MasBoxSettingResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類2 (CATEGORY_ID2)
        /// </summary>
        [Display(Name = nameof(MasBoxSettingResource.CategoryId2), ResourceType = typeof(MasBoxSettingResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId2 { get; set; }

        /// <summary>
        /// 分類3 (CATEGORY_ID3)
        /// </summary>
        [Display(Name = nameof(MasBoxSettingResource.CategoryId3), ResourceType = typeof(MasBoxSettingResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId3 { get; set; }

        /// <summary>
        /// 分類4 (CATEGORY_ID4)
        /// </summary>
        [Display(Name = nameof(MasBoxSettingResource.CategoryId4), ResourceType = typeof(MasBoxSettingResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId4 { get; set; }

        /// <summary>
        /// 商品ID(品番) (ITEM_ID)
        /// </summary>
        [Display(Name = nameof(MasBoxSettingResource.ItemId), ResourceType = typeof(MasBoxSettingResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 閾値区分 (THRESHOLD_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:率(%) 2:数量
        /// </remarks>
        [Display(Name = nameof(MasBoxSettingResource.ThresholdClass), ResourceType = typeof(MasBoxSettingResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ThresholdClass { get; set; }

        /// <summary>
        /// 閾値 (THRESHOLD)
        /// </summary>
        /// <remarks>
        /// ケース保管閾値（この値を超えたらケース管理、以下はバラへ移動）%値　か、数量値をセットする
        /// </remarks>
        [Display(Name = nameof(MasBoxSettingResource.Threshold), ResourceType = typeof(MasBoxSettingResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Threshold { get; set; }

        /// <summary>
        /// エラー情報 (ERR_MSG)
        /// </summary>
        [Display(Name = nameof(MasBoxSettingResource.ErrMsg), ResourceType = typeof(MasBoxSettingResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ErrMsg { get; set; }

        #endregion プロパティ
    }
}