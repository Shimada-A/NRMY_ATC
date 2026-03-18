namespace Wms.Areas.Master.Models
{
    using Share.Common.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    /// <summary>
    /// レイアウト明細
    /// </summary>
    [Table("M_LAYOUT_DETAILS")]
    public partial class LayoutDetail : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 削除フラグ (DELETE_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未削除 1:削除済み
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutDetailResource.DeleteFlag), ResourceType = typeof(LayoutDetailResource))]
        public byte DeleteFlag { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　0905,0924,0942
        /// </remarks>
        [Key]
        [Column(Order = 13)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutDetailResource.CenterId), ResourceType = typeof(LayoutDetailResource))]
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
        [Display(Name = nameof(LayoutDetailResource.TemplateId), ResourceType = typeof(LayoutDetailResource))]
        [Range(-99999999999999999, 99999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long TemplateId { get; set; }

        /// <summary>
        /// 行№ (LINE_NO)
        /// </summary>
        [Key]
        [Column(Order = 101)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutDetailResource.LineNo), ResourceType = typeof(LayoutDetailResource))]
        [Range(-99999, 99999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int LineNo { get; set; }

        /// <summary>
        /// カラム№ (COLUMN_NO)
        /// </summary>
        [Key]
        [Column(Order = 102)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutDetailResource.ColumnNo), ResourceType = typeof(LayoutDetailResource))]
        [Range(-99999, 99999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int ColumnNo { get; set; }

        /// <summary>
        /// サブ№ (SUB_NO)
        /// </summary>
        /// <remarks>
        /// 取込時の複数項目結合用、出力時は1固定
        /// </remarks>
        [Key]
        [Column(Order = 103)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutDetailResource.SubNo), ResourceType = typeof(LayoutDetailResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte SubNo { get; set; }

        /// <summary>
        /// オブジェクトID (OBJECT_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutDetailResource.ObjectId), ResourceType = typeof(LayoutDetailResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ObjectId { get; set; }

        /// <summary>
        /// カラムID (COLUMN_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutDetailResource.ColumnId), ResourceType = typeof(LayoutDetailResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ColumnId { get; set; }

        /// <summary>
        /// データ型 (DATA_TYPE)
        /// </summary>
        /// <remarks>
        /// 0:文字列　1:数値
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutDetailResource.DataType), ResourceType = typeof(LayoutDetailResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte DataType { get; set; }

        /// <summary>
        /// タイトル名 (TITLE_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutDetailResource.TitleName), ResourceType = typeof(LayoutDetailResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TitleName { get; set; }

        /// <summary>
        /// 桁数 (DIGIT)
        /// </summary>
        [Display(Name = nameof(LayoutDetailResource.Digit), ResourceType = typeof(LayoutDetailResource))]
        [Range(-99999, 99999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? Digit { get; set; }

        /// <summary>
        /// 文字埋め有無 (PAD_CLASS)
        /// </summary>
        /// <remarks>
        /// 0:なし　1:あり　※出力時のみ
        /// </remarks>
        [Display(Name = nameof(LayoutDetailResource.PadClass), ResourceType = typeof(LayoutDetailResource))]
        public byte? PadClass { get; set; }

        /// <summary>
        /// 文字埋め方向 (PAD_DIRECTION)
        /// </summary>
        /// <remarks>
        /// 1:左文字埋め　2:右文字埋め　※出力時のみ
        /// </remarks>
        [Display(Name = nameof(LayoutDetailResource.PadDirection), ResourceType = typeof(LayoutDetailResource))]
        public byte? PadDirection { get; set; }

        /// <summary>
        /// 埋め文字 (PAD_CHAR)
        /// </summary>
        /// <remarks>
        /// ※出力時のみ
        /// </remarks>
        [Display(Name = nameof(LayoutDetailResource.PadChar), ResourceType = typeof(LayoutDetailResource))]
        [MaxLength(1, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PadChar { get; set; }

        /// <summary>
        /// 開始位置 (START_POSITION)
        /// </summary>
        /// <remarks>
        /// ※固定長取込時のみ
        /// </remarks>
        [Display(Name = nameof(LayoutDetailResource.StartPosition), ResourceType = typeof(LayoutDetailResource))]
        [Range(-99999, 99999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? StartPosition { get; set; }

        /// <summary>
        /// 終了位置 (END_POSITION)
        /// </summary>
        /// <remarks>
        /// ※固定長取込時のみ
        /// </remarks>
        [Display(Name = nameof(LayoutDetailResource.EndPosition), ResourceType = typeof(LayoutDetailResource))]
        [Range(-99999, 99999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? EndPosition { get; set; }

        /// <summary>
        /// 更新有無 (UPDATE_CLASS)
        /// </summary>
        /// <remarks>
        /// 0:更新しない　1:更新する
        /// </remarks>
        [Display(Name = nameof(LayoutDetailResource.UpdateClass), ResourceType = typeof(LayoutDetailResource))]
        public byte? UpdateClass { get; set; }

        /// <summary>
        /// 更新有無 (UPDATE_CLASS)
        /// </summary>
        /// <remarks>
        /// 0:更新しない　1:更新する
        /// </remarks>
        [Display(Name = nameof(LayoutDetailResource.ImportClass), ResourceType = typeof(LayoutDetailResource))]
        public byte? ImportClass { get; set; }

        /// <summary>
        /// 固定値 (FIXED_VALUE)
        /// </summary>
        [Display(Name = nameof(LayoutDetailResource.FixedValue), ResourceType = typeof(LayoutDetailResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string FixedValue { get; set; }

        #endregion
    }
}
