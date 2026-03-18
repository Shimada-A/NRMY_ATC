namespace Wms.Areas.Ship.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Ship.Resources;
    using Wms.Models;

    /// <summary>
    /// 引当情報
    /// </summary>
    [Table("T_ALLOC_INFO")]
    public partial class AllocInfo : BaseModel
    {
        #region プロパティ
        /// <summary>
        /// センターコード
        /// </summary>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(AllocInfoResource.CenterId), ResourceType = typeof(AllocInfoResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 引当No
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(AllocInfoResource.AllocNo), ResourceType = typeof(AllocInfoResource))]
        public string AllocNo { get; set; }

        /// <summary>
        /// 引当日
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = nameof(AllocInfoResource.AllocDate), ResourceType = typeof(AllocInfoResource))]
        public DateTime? AllocDate { get; set; }

        /// <summary>
        /// 引当担当者
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AllocInfoResource.AllocUserId), ResourceType = typeof(AllocInfoResource))]
        public string AllocUserId { get; set; }

        /// <summary>
        /// EC引当種別
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AllocInfoResource.EcAllocKind), ResourceType = typeof(AllocInfoResource))]
        public int EcAllocKind { get; set; }

        /// <summary>
        /// 出荷種別
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AllocInfoResource.ShipKind), ResourceType = typeof(AllocInfoResource))]
        public int? ShipKind { get; set; }

        /// <summary>
        /// バッチ名称
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AllocInfoResource.BatchName), ResourceType = typeof(AllocInfoResource))]
        public string BatchName { get; set; }

        /// <summary>
        /// 1バッチで１バッチフラグ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AllocInfoResource.EcOneBatchFlag), ResourceType = typeof(AllocInfoResource))]
        public int EcOneBatchFlag { get; set; }

        /// <summary>
        /// オーダーピックにするフラグ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AllocInfoResource.DoOrderPick), ResourceType = typeof(AllocInfoResource))]
        public int DoOrderPick { get; set; }

        /// <summary>
        /// ピック種別
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AllocInfoResource.PickKind), ResourceType = typeof(AllocInfoResource))]
        public int PickKind { get; set; }

        /// <summary>
        /// 引当Noグループ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AllocInfoResource.AllocGroupNo), ResourceType = typeof(AllocInfoResource))]
        public string AllocGroupNo { get; set; }

        #endregion プロパティ
    }
}