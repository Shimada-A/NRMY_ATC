using Foolproof;
using Share.Common.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Wms.Areas.Master.Models;
using Wms.Areas.Master.Resources;
using Wms.Common;
using Wms.Extensions.Attributes;

namespace Wms.Areas.Master.ViewModels.EditLayout
{
    public class EditOutputFileDetail
    {
        public EditOutputFileDetail()
        {

        }

        public EditOutputFileDetail(LayoutDetail detail)
        {
            IsNewData = false;
            ShipperId = detail.ShipperId;
            TemplateId = detail.TemplateId;
            RowNo = detail.LineNo;
            ColumnNo = detail.ColumnNo;
            SubNo = detail.SubNo;
            ObjectId = detail.ObjectId;
            ColumnId = detail.ColumnId;
            DataType = (Common.DataType)detail.DataType;
            ColumnName = detail.TitleName;
            Digit = detail.Digit;
            PadClass = detail.PadClass;
            PadDirection = (PadDirection)detail.PadDirection.GetValueOrDefault();
            PadValue = detail.PadChar;
        }

        public bool IsNewData { get; set; } = true;

        public long TemplateId { get; set; }

        /// <summary>
        /// 会社ID
        /// </summary>
        public string ShipperId { get; set; }

        /// <summary>
        ///  行No
        /// </summary>
        public int? RowNo { get; set; }

        /// <summary>
        /// ファイルの種類
        /// </summary>
        [RequiredIfNotEmpty(nameof(ColumnName),
           ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(EditLayoutResource.No), ResourceType = typeof(EditLayoutResource))]
        public int? ColumnNo { get; set; }

        /// <summary>
        /// サブNo
        /// </summary>
        public int? SubNo { get; set; }

        /// <summary>
        /// オブジェクトId
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// カラムID
        /// </summary>
        public string ColumnId { get; set; }

        /// <summary>
        /// データ型
        /// </summary>
        public Common.DataType? DataType { get; set; }

        /// <summary>
        /// 出力タイトル
        /// カラムNoが入力されてる場合は必須
        /// </summary>
        [RequiredIfNotEmpty(nameof(ColumnNo),
            ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(EditLayoutResource.OutputTitle), ResourceType = typeof(EditLayoutResource))]
        public string ColumnName { get; set; }

        /// <summary>
        /// バイト数
        /// </summary>
        [Range(-99999, 99999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? Digit { get; set; }

        /// <summary>
        /// 文字埋め有無
        /// </summary>
        public byte? PadClass { get; set; }

        /// <summary>
        /// 埋め文字
        /// 文字埋め有無が1の時は必須
        /// </summary>
        [RequiredIf(nameof(PadClass), 1,
            ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(EditLayoutResource.PadChar), ResourceType = typeof(EditLayoutResource))]
        [MaxLength(1, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PadValue { get; set; }

        /// <summary>
        /// 埋め方向
        /// </summary>
        public PadDirection PadDirection { get; set; }
    }
}