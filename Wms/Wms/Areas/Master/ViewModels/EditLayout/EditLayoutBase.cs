using Share.Common.Resources;
using Share.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wms.Areas.Master.Models;
using Wms.Areas.Master.Resources;
using Wms.Common;
using Wms.ViewModels;

namespace Wms.Areas.Master.ViewModels.EditLayout
{
    public class EditLayoutBase : ViewModelBase
    {
        public EditLayoutBase()
        {

        }

        public EditLayoutBase(Layout layout)
        {
            IsNewLayout = false;
            ShipperId = layout.ShipperId;

            TemplateId = layout.TemplateId;
            LayoutName = layout.TemplateName;
            ObjectType = layout.ObjectType;
            ObjectId = layout.ObjectId;
            IoClass = (IoClass)layout.IoClass;
            IoClassName = EnumHelperEx.GetDisplayValue(IoClass);
            FileType = (FileType)layout.FileType;
            EncodeType = (EncodeType)layout.EncodeType;
            EncloseType = (EncloseType)layout.EncloseType;
            HeadingRow = (HeadingRow)layout.TitleClass;
            ObjectId = layout.ObjectId;
        }

        /// <summary>
        /// 新規レイアウトか？
        /// </summary>
        public bool IsNewLayout { get; set; }

        /// <summary>
        /// テンプレートID
        /// </summary>
        public long TemplateId { get; set; }

        /// <summary>
        ///  オブジェクトのリスト
        /// </summary>
        public SelectList ObjectList { get; set; }


        /// <summary>
        /// レイアウト名
        /// </summary>
        public string LayoutName { get; set; }

        /// <summary>
        /// 入出力区分
        /// </summary>
        public string IoClassName { get; set; }

        /// <summary>
        /// 入出力区分
        /// </summary>
        public IoClass IoClass { get; set; }

        /// <summary>
        /// オブジェクトId
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// オブジェクトタイプ
        /// </summary>
        public int ObjectType { get; set; }

        /// <summary>
        /// 文字コード
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(EditLayoutResource.EncodeType), ResourceType = typeof(EditLayoutResource))]
        public EncodeType? EncodeType { get; set; }

        public string EncodeTypeName { 
            get {
                return EnumHelperEx.GetDisplayValue(EncodeType.GetValueOrDefault());
            }
        }

        /// <summary>
        /// ファイル形式
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(EditLayoutResource.FileType), ResourceType = typeof(EditLayoutResource))]
        public FileType? FileType { get; set; }

        public string FileTypeName
        {
            get
            {
                return EnumHelperEx.GetDisplayValue(FileType.GetValueOrDefault());
            }
        }

        /// <summary>
        /// ファイル形式取込時
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(EditLayoutResource.FileType), ResourceType = typeof(EditLayoutResource))]
        public FileTypeImport? FileTypeImport { get; set; }


        /// <summary>
        /// 見出し行
        /// </summary>
        public HeadingRow HeadingRow { get; set; } = HeadingRow.Available;

        public string HeadingRowName
        {
            get
            {
                return EnumHelperEx.GetDisplayValue(HeadingRow);
            }
        }


        /// <summary>
        /// 囲み文字
        /// </summary>
        public EncloseType EncloseType { get; set; } = EncloseType.NotAvailable;

        public string EncloseTypeName
        {
            get
            {
                return EnumHelperEx.GetDisplayValue(EncloseType);
            }
        }

    }
}