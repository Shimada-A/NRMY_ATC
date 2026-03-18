namespace Wms.Areas.Master.Models
{
    using Share.Common.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    /// <summary>
    /// レイアウト
    /// </summary>
    [Table("M_LAYOUTS")]
    public partial class Layout : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 削除フラグ (DELETE_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未削除 1:削除済み
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutResource.DeleteFlag), ResourceType = typeof(LayoutResource))]
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
        [Display(Name = nameof(LayoutResource.CenterId), ResourceType = typeof(LayoutResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// テンプレートID (TEMPLATE_ID)
        /// </summary>
        /// <remarks>
        /// テンプレート単位の連番
        /// </remarks>
        [Key]
        [Column(Order = 100)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutResource.TemplateId), ResourceType = typeof(LayoutResource))]
        [Range(-99999999999999999, 99999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long TemplateId { get; set; }

        /// <summary>
        /// テンプレート名 (TEMPLATE_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutResource.TemplateName), ResourceType = typeof(LayoutResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TemplateName { get; set; }

        /// <summary>
        /// オブジェクトタイプ (OBJECT_TYPE)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutResource.ObjectType), ResourceType = typeof(LayoutResource))]
        public int ObjectType { get; set; }

        /// <summary>
        /// 入出力区分 (IO_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:取込　2:出力
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutResource.IoClass), ResourceType = typeof(LayoutResource))]
        public int IoClass { get; set; }

        /// <summary>
        /// ファイルタイプ (FILE_TYPE)
        /// </summary>
        /// <remarks>
        /// 1:csv　2:tsv　3:excel　4:固定長
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutResource.FileType), ResourceType = typeof(LayoutResource))]
        public int FileType { get; set; }

        /// <summary>
        /// 文字コード (ENCODE_TYPE)
        /// </summary>
        /// <remarks>
        /// 1:Shift-JIS　2:UTF-8
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutResource.EncodeType), ResourceType = typeof(LayoutResource))]
        public int EncodeType { get; set; }

        /// <summary>
        /// 囲み文字 (ENCLOSE_TYPE)
        /// </summary>
        /// <remarks>
        /// 0:なし　1:あり
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutResource.EncloseType), ResourceType = typeof(LayoutResource))]
        public int EncloseType { get; set; }

        /// <summary>
        /// タイトル有無 (TITLE_CLASS)
        /// </summary>
        /// <remarks>
        /// 0:なし　1:あり
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutResource.TitleClass), ResourceType = typeof(LayoutResource))]
        public int TitleClass { get; set; }

        /// <summary>
        /// オブジェクトID (OBJECT_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutResource.ObjectId), ResourceType = typeof(LayoutResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ObjectId { get; set; }

        #endregion
    }
}
