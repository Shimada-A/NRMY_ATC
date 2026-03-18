namespace Wms.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Resources;

    /// <summary>
    /// 連携システム
    /// </summary>
    [Table("M_IF_SYSTEMS")]
    public partial class IfSystem : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 連携システムID (IF_SYSTEM_ID)
        /// </summary>
        /// <remarks>
        /// 外部システム連携時のS3キーのフォルダ名と一致させる
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(IfSystemResource.IfSystemId), ResourceType = typeof(IfSystemResource))]
        public string IfSystemId { get; set; }

        /// <summary>
        /// 連携システム名 (IF_SYSTEM_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(IfSystemResource.IfSystemName), ResourceType = typeof(IfSystemResource))]
        public string IfSystemName { get; set; }

        /// <summary>
        /// システム区分 (SYSTEM_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:基幹 2:店舗BO 3:WMS 4:生産管理 5:自社EC 6:他社EC 7:画面
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(IfSystemResource.SystemClass), ResourceType = typeof(IfSystemResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte SystemClass { get; set; }

        /// <summary>
        /// システム区分名 (SYSTEM_CLASS_NAME)
        /// </summary>
        /// <remarks>
        /// 1:基幹 2:店舗BO 3:WMS 4:生産管理 5:自社EC 6:他社EC 7:画面
        /// 開発者識別用（ビューで使用する）
        /// </remarks>
        [Display(Name = nameof(IfSystemResource.SystemClassName), ResourceType = typeof(IfSystemResource))]
        public string SystemClassName { get; set; }

        /// <summary>
        /// ECサイト区分 (EC_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:Amazon 2:Yahoo 3:Rakuten 4:Zozo 5:SHOPLIST 6:ecbeing
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(IfSystemResource.EcClass), ResourceType = typeof(IfSystemResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte EcClass { get; set; }

        #endregion
    }
}
