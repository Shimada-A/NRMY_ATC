namespace Wms.Areas.Master.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Common;
    using Wms.Models;

    /// <summary>
    /// 浪速用仕分コード
    /// </summary>
    [Table("M_NANIWA_SORTING")]
    public partial class NaniwaSorting : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 店舗ID (STORE_ID)
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NaniwaSortingResource.StoreId), ResourceType = typeof(NaniwaSortingResource))]
        [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreId { get; set; }

        /// <summary>
        /// 配送センターコード (NANIWA_DELI_CENTER_CD)
        /// </summary>
        /// <remarks>
        /// 汎用コードマスタ「浪速運送配送センター」
        /// </remarks>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NaniwaSortingResource.NaniwaDeliCenterCd), ResourceType = typeof(NaniwaSortingResource))]
        [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string NaniwaDeliCenterCd { get; set; }

        #endregion
    }
}
