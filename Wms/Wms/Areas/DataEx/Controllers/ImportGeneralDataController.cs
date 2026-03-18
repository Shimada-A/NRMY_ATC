using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wms.Areas.DataEx.ViewModels.ImportGeneralData;
using Wms.Areas.Master.Models;
using Wms.Common;
using Wms.Controllers;

namespace Wms.Areas.DataEx.Controllers
{
    public class ImportGeneralDataController : BaseController
    {
        // GET: DataEx/ImportGeneralData
        public ActionResult Index(ImportGeneralDataViewModel vm)
        {
            return GetView("Index", vm);
        }

        /// <summary>
        /// ImportGeneralDataのビュー取得
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        private ActionResult GetView(string viewName, ImportGeneralDataViewModel vm)
        {
            ModelState.Clear();
            if (Session[nameof(ImportGeneralDataViewModel)] == null)
            {
                Session[nameof(ImportGeneralDataViewModel)] = vm;
            }

            var layouts = Layout.GetLayouts(Common.Profile.User.ShipperId, IoClass.Import, "");
            vm.LayoutList = new SelectList(layouts, "TemplateId", "TemplateName");
            return View(viewName, vm);
        }

        [HttpPost]
        public ActionResult FileDrop()
        {
            return null;
        }

        [HttpPost]
        public ActionResult Import(ImportGeneralDataViewModel vm)
        {


            return null;
        }

        /// <summary>
        /// レイアウト情報ビュー取得処理
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public PartialViewResult GetLayoutInfo(long templateId)
        {
            // レイアウト情報取得してViewModelのプロパティ更新、部分ビューを返却
            var layout = Layout.GetLayoutWithObject(Common.Profile.User.ShipperId, templateId);
            var vm = Session[nameof(ImportGeneralDataViewModel)] as ImportGeneralDataViewModel;
            vm.GeneralDataTemplateId = layout.TemplateId;
            vm.HeadingRow = layout.HeadingRow;
            vm.EncodeType = layout.EncodeType;
            vm.EncloseType = layout.EncloseType;
            vm.FileType = layout.FileType;
            vm.ObjectName = layout.ObjectName;

            return PartialView("~/Areas/DataEx/Views/ImportGeneralData/_ImportGeneralDataHeader.cshtml", vm);

        }

    }
}