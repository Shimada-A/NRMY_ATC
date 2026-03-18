namespace Wms.Areas.Inventory.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Inventory.Resources;
    using Wms.Models;

    /// <summary>
    /// 棚卸開始ワーク01
    /// </summary>
    [Table("WW_INV_START_01")]
    public partial class InvStart_01 : BaseModel
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
        [Display(Name = nameof(InvStart_01Resource.Seq), ResourceType = typeof(InvStart_01Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(InvStart_01Resource.LineNo), ResourceType = typeof(InvStart_01Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 倉庫ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InvStart_01Resource.CenterId), ResourceType = typeof(InvStart_01Resource))]
        public string CenterId { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InvStart_01Resource.LocationCd), ResourceType = typeof(InvStart_01Resource))]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public string LocationCd { get; set; }

        /// <summary>
        /// メッセージ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InvStart_01Resource.Message), ResourceType = typeof(InvStart_01Resource))]
        public string Message { get; set; }

        #endregion プロパティ
    }
}