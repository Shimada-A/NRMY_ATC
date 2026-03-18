using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Areas.Master.Models;

namespace Wms.Areas.Master.ViewModels.EditLayout
{
    public class EditSort : EditLayoutBase
    {
        /// <summary>
        /// オブジェクト明細
        /// </summary>
        public List<ObjectDetailDTO> Details { get; set; }
    }
}