namespace Wms.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Resources;

    /// <summary>
    /// 連携実行単位名
    /// </summary>
    [Table("M_IF_RUN_UNIT_NAMES")]
    public partial class IfRunUnitName : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 連携実行単位ID (IF_RUN_UNIT_ID)
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(IfRunUnitNameResource.IfRunUnitId), ResourceType = typeof(IfRunUnitNameResource))]
        public string IfRunUnitId { get; set; }

        /// <summary>
        /// 連携実行単位名 (NAME)
        /// </summary>
        /// <remarks>
        /// 開発者用：分かりやすい名前でよい
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(IfRunUnitNameResource.Name), ResourceType = typeof(IfRunUnitNameResource))]
        public string Name { get; set; }

        #endregion
    }
}
