using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Wms.Areas.Master.Resources;
using Wms.Resources;

namespace Wms.Areas.Master.ViewModels.EditLayout
{

    public enum PadDirection
    {

        /// <summary>
        /// 左埋め
        /// </summary>
        [Display(Name = nameof(CommonResource.PadFront  ), ResourceType = typeof(CommonResource))]
        PadLeft = 1,
        /// <summary>
        /// 右埋め
        /// </summary>
        [Display(Name = nameof(CommonResource.PadEnd), ResourceType = typeof(CommonResource))]
        PadRight = 2
    }
}