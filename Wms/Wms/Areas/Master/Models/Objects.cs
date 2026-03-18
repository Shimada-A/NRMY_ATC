namespace Wms.Areas.Master.Models
{
    using Share.Common.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    /// <summary>
    /// オブジェクト
    /// </summary>
    [Table("M_OBJECTS")]
    public partial class Objects : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 削除フラグ (DELETE_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未削除 1:削除済み
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ObjectResource.DeleteFlag), ResourceType = typeof(ObjectResource))]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　0905,0924,0942
        /// </remarks>
        [Key]
        [Column(Order = 13)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ObjectResource.CenterId), ResourceType = typeof(ObjectResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// オブジェクトID (OBJECT_ID)
        /// </summary>
        [Key]
        [Column(Order = 100)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ObjectResource.ObjectId), ResourceType = typeof(ObjectResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ObjectId { get; set; }

        /// <summary>
        /// オブジェクト名 (OBJECT_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ObjectResource.ObjectName), ResourceType = typeof(ObjectResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ObjectName { get; set; }

        /// <summary>
        /// オブジェクトタイプ (OBJECT_TYPE)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ObjectResource.ObjectType), ResourceType = typeof(ObjectResource))]
        public bool ObjectType { get; set; }

        #endregion
    }
}
