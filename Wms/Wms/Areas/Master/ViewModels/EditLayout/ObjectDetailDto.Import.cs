using Foolproof;
using Share.Common.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Wms.Areas.Master.Resources;
using Wms.Common;

namespace Wms.Areas.Master.ViewModels.EditLayout
{
    public partial class ObjectDetailDTO
    {

        [RequiredIf(nameof(ImportClass),1 ,
            ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutDetailResource.FixedValue), ResourceType = typeof(LayoutDetailResource))]
        public string FixedValue { get; set; }

        public int StartPosition { get; set; }

        public int EndPosition { get; set; }

        public UpdateClass UpdateClass { get; set; }

        public ImportClass ImportClass { get; set; }

        /// <summary>
        /// カラム№ (COLUMN_NO)
        /// </summary>
        [RequiredIf(nameof(RequiredFlag),true,
            ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-9999, 9999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LayoutDetailResource.ColumnNo), ResourceType = typeof(LayoutDetailResource))]
        public int? ColumnNoFirst { get; set; }

        /// <summary>
        /// カラム№ (COLUMN_NO)
        /// </summary>
        [Range(-9999, 9999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? ColumnNoSecond { get; set; }

        /// <summary>
        /// 一意キーフラグのテキスト
        /// </summary>
        public string PrimaryFlagText { 
            get 
            {
                return PrimaryFlag.GetValueOrDefault() ? "〇" : string.Empty; 
            }
        }

        /// <summary>
        /// 必須フラグのテキスト
        /// </summary>
        public string RequiredFlagText { 
            get 
            { 
                return RequiredFlag.GetValueOrDefault() ? "〇" : string.Empty; 
            } 
        }
    }
}