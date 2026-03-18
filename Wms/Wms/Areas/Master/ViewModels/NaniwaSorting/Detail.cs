namespace Wms.Areas.Master.ViewModels.NaniwaSorting
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Common;
    using Wms.Models;

    public class Detail : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 店舗ID
        /// </summary>
        /// <remarks>
        /// （店舗コード）
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.NaniwaSortingResource.StoreId), ResourceType = typeof(Resources.NaniwaSortingResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreId { get; set; }

        /// <summary>
        /// 店舗名
        /// </summary>
        [Display(Name = nameof(Resources.NaniwaSortingResource.StoreName), ResourceType = typeof(Resources.NaniwaSortingResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreName { get; set; }

        /// <summary>
        /// 配送センターコード
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.NaniwaSortingResource.NaniwaDeliCenterCd), ResourceType = typeof(Resources.NaniwaSortingResource))]
        [MaxLength(4, ErrorMessageResourceName = nameof(MessagesResource.LengthError), ErrorMessageResourceType = typeof(MessagesResource))]
        [MinLength(4, ErrorMessageResourceName = nameof(MessagesResource.LengthError), ErrorMessageResourceType = typeof(MessagesResource))]
        public string NaniwaDeliCenterCd { get; set; }

        /// <summary>
        /// true:新規登録、false:更新
        /// </summary>
        public bool InsertFlag { get; set; }

        /// <summary>
        /// 検索済み判断フラグ
        /// </summary>
        public bool SearchFlag { get; set; }

        #endregion プロパティs
    }
}