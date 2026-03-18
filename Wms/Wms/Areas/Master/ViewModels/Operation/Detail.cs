namespace Wms.Areas.Master.ViewModels.Operation
{
    using Microsoft.Ajax.Utilities;
    using PagedList;
    using Share.Common.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Windows.Media.TextFormatting;
    using Wms.Areas.Master.Resources;
    using Wms.Common;
    using Wms.Models;

    /// <summary>
    /// 仕分けパターン
    /// </summary>
    public class Detail : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 業務ID (OPERATION_ID)
        /// </summary>
        /// <remarks>
        /// WMS業務外　は3桁（メンテナンス画面でメンテされる）
        /// WMS業務内　は4桁（スマホ出荷作業など。IDはシステムで固定）
        /// 　1001：トータルピック
        /// 　1002：店別ピック
        /// 　1003：店仕分
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(OperationResource.OperationId), ResourceType = typeof(OperationResource))]
        [MaxLength(3, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string OperationId { get; set; }

        /// <summary>
        /// 業務名 (OPERATION_NAME)
        /// </summary>
        [Display(Name = nameof(OperationResource.OperationName), ResourceType = typeof(OperationResource))]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string OperationName { get; set; }

        /// <summary>
        /// 業務カテゴリ名 (CATEGORY_NAME)
        /// </summary>
        [Display(Name = nameof(OperationResource.CategoryName), ResourceType = typeof(OperationResource))]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryName { get; set; }

        /// <summary>
        /// 数量０許可フラグ (QTY_ZERO_FLAG)
        /// </summary>
        /// <remarks>
        /// 0：数量0を許可しない、1：数量0を許可する
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(OperationResource.QtyZeroFlag), ResourceType = typeof(OperationResource))]
        public bool QtyZeroFlag { get; set; }

        /// <summary>
        /// Checkbox Delete Checked
        /// </summary>
        public bool IsCheck { get; set; }

        /// <summary>
        /// 新規登録フラグ
        /// </summary>
        public bool InsertFlag { get; set; }

        /// <summary>
        /// 更新回数（排他制御用）
        /// </summary>
        public int UpdateCount { get; set; }


        /// <summary>
        /// 検索済み判断フラグ
        /// </summary>
        public bool SearchFlag { get; set; }

        [Display(Name = nameof(OperationResource.OperationNote), ResourceType = typeof(OperationResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string OperationNote { get; set; }

        public List<OperationNoteItem> OperationNotes { get; set; }

        #endregion プロパティ
        public Detail()
        {
            this.OperationNotes = new List<OperationNoteItem>();
        }
    }
    public class OperationNoteItem
    {
        [Display(Name = nameof(OperationNoteResource.Seq), ResourceType = typeof(OperationNoteResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Seq { get; set; }


        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(OperationNoteResource.OperationNote), ResourceType = typeof(OperationNoteResource))]
        public string OperationNote { get; set; }

    }
}