using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wms.Areas.DataEx.ViewModels.OutputGeneralData;
using Wms.Areas.Master.Models;
using Wms.Common;
using Wms.Controllers;

namespace Wms.Areas.DataEx.Controllers
{
    public partial class OutputGeneralDataController : BaseController
    {
        public ActionResult Sort(OutputGeneralDataViewModel vm)
        {
            
            return GetView("Sort",vm);
        }


        /// <summary>
        /// 条件設定のオブジェクト明細取得処理
        /// </summary>
        /// <param name="shipperId"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult GetOutputGeneralDataSortingObjectDetail(long templateId)
        {
            return GetDetailView(templateId, "~/Areas/Master/Views/EditLayout/_EditSortDetail.cshtml");
        }
    }
}