using Foolproof;
using Share.Common.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Wms.Areas.Master.Resources;
using Wms.Common;

namespace Wms.Areas.Master.ViewModels.EditLayout
{
    public partial class ObjectDetailDTO
    {
        /// <summary>
        /// 条件区分
        /// </summary>
        public ConditionClass ConditionClass { get; set; }

        private const string pt = "Equal|NotEqual|GreaterEqual|LessEqual|Range|Like";

        /// <summary>
        /// 条件値From
        /// </summary>
        [RequiredIfRegExMatch(nameof(ConditionClass), pt,
            ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(EditLayoutResource.Condition), ResourceType = typeof(EditLayoutResource))]
        public string ConditionValueFrom { get; set; }

        /// <summary>
        /// 条件値To
        /// </summary>
        [RequiredIfRegExMatch(nameof(ConditionClass), "Range",
            ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(EditLayoutResource.Condition), ResourceType = typeof(EditLayoutResource))]
        public string ConditionValueTo { get; set; }
    }
}