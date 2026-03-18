using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Common;

namespace Wms.Areas.Master.ViewModels.EditLayout
{
    public partial class ObjectDetailDTO
    {
        /// <summary>
        /// ソート順
        /// </summary>
        public int? SortOrder { get; set; }

        /// <summary>
        /// ソート方向
        /// </summary>
        public SortDirection SortDirection { get; set; }
    }
}