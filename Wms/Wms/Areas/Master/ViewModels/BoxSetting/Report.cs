namespace Wms.Areas.Master.ViewModels.BoxSetting
{
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Master.Resources;

    public class Report
    {
        /// <summary>
        /// 分類1 (CATEGORY_ID1)
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.CategoryId1), ResourceType = typeof(BoxSettingResource), Order = 1)]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類2 (CATEGORY_ID2)
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.CategoryId2), ResourceType = typeof(BoxSettingResource), Order = 2)]
        public string CategoryId2 { get; set; }

        /// <summary>
        /// 分類3 (CATEGORY_ID3)
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.CategoryId3), ResourceType = typeof(BoxSettingResource), Order = 3)]
        public string CategoryId3 { get; set; }

        /// <summary>
        /// 分類4 (CATEGORY_ID4)
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.CategoryId4), ResourceType = typeof(BoxSettingResource), Order = 4)]
        public string CategoryId4 { get; set; }

        /// <summary>
        /// 商品ID(品番) (ITEM_ID)
        /// </summary>
        [Display(Name = nameof(BoxSettingResource.ItemId), ResourceType = typeof(BoxSettingResource), Order = 5)]
        public string ItemId { get; set; }

        /// <summary>
        /// 閾値区分 (THRESHOLD_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:率(%) 2:ケース数
        /// </remarks>
        [Display(Name = nameof(BoxSettingResource.ThresholdClass), ResourceType = typeof(BoxSettingResource), Order = 6)]
        public string ThresholdClass { get; set; }

        /// <summary>
        /// 閾値 (THRESHOLD)
        /// </summary>
        /// <remarks>
        /// ケース保管閾値　この値を超えたらケース管理、以下はバラ
        /// </remarks>
        [Display(Name = nameof(BoxSettingResource.Threshold), ResourceType = typeof(BoxSettingResource), Order = 7)]
        public string Threshold { get; set; }
    }
}