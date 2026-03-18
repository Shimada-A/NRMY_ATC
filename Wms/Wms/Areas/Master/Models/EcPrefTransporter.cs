namespace Wms.Areas.Master.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common;
    using Share.Common.Resources;
    using Wms.Models;

    /// <summary>
    /// EC注文都道府県別配送業者マスタ
    /// </summary>
    [Table("M_EC_PREF_TRANSPORTER")]
    public partial class EcPrefTransporter : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　（ECセンターのみ必要）
        /// </remarks>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.EcPrefTransporterResource.CenterId), ResourceType = typeof(Resources.EcPrefTransporterResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 都道府県コード (PREF_ID)
        /// </summary>
        /// <remarks>
        /// 都道府県マスタから
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.EcPrefTransporterResource.PrefId), ResourceType = typeof(Resources.EcPrefTransporterResource))]
        [MaxLength(2, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PrefId { get; set; }

        /// <summary>
        /// 都道府県名 (PREF_NAME)
        /// </summary>
        /// <remarks>
        /// 都道府県マスタから
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.EcPrefTransporterResource.PrefName), ResourceType = typeof(Resources.EcPrefTransporterResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PrefName { get; set; }

        /// <summary>
        /// 配送業者ID (TRANSPORTER_ID)
        /// </summary>
        /// <remarks>
        /// 該当都道府県の配送業者
        /// 01：ヤマト運輸
        /// 02：佐川急便
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.EcPrefTransporterResource.TransporterId), ResourceType = typeof(Resources.EcPrefTransporterResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterId { get; set; }

        #endregion
    }
}
