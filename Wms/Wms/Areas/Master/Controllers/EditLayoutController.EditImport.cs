using PagedList;
using Share.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wms.Areas.Master.Models;
using Wms.Areas.Master.Models.EditLayout;
using Wms.Areas.Master.Query.EditLayout;
using Wms.Areas.Master.ViewModels.EditLayout;
using Wms.Common;
using Wms.Controllers;

namespace Wms.Areas.Master.Controllers
{
    public partial class EditLayoutController : BaseController
    {
        /// <summary>
        /// 新規作成時
        /// </summary>
        /// <param name="shipperId"></param>
        /// <param name="LayoutName"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditImport(Index vm)
        {
            ModelState.Clear();
            return GetEditImport(vm);
        }

        /// <summary>
        /// 取込登録画面、INDEXから遷移
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditImportFromIndex(Index vm)
        {
            ModelState.Clear();
            if (vm.IsNewLayout)
            {
                return GetEditImport(vm);
            }
            else
            {
                return GetEditImportExistsData(vm);
            }
        }

        /// <summary>
        /// 登録/更新処理
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditImport(EditImport vm)
        {
            // 登録/更新処理
            EditLayoutQuery.ConfirmImport(vm);

            // INDEXに遷移
            ModelState.Clear();
            return IndexAfterConfirm();
        }

        [HttpPost]
#pragma warning disable CA3147 // 偽造防止トークンの検証での動詞ハンドラーのマーク
        public PartialViewResult FileDrop(EncodeType encodeType, FileTypeImport fileType,HeadingRow headingRow)
#pragma warning restore CA3147 // 偽造防止トークンの検証での動詞ハンドラーのマーク
        {
            // 取込データを画面に返す
            var file = HttpContext.Request.Files[0];
            var vm = EditImportModel.InstanceFromDroppedFile(file, encodeType, fileType, headingRow);
            if (vm == null)
                return null;
            return PartialView("_EditImportDetail",vm);
        }

        /// <summary>
        /// テーブル情報一覧取得
        /// </summary>
        /// <param name="shipperId"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult GetEditImportObjectDetail(string shipperId, string objectId)
        {
            var vm = new EditImport { TableInfos = GetObjectDetail(objectId)};
            return PartialView("_EditImportData", vm);
        }

        /// <summary>
        /// EditImportインスタンスを作成しViewを返却
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private ActionResult GetEditImport(EditLayoutBase data)
        {
            var vm = CreateEditImportInstance(data);
            vm.IsNewLayout = true;
            return View(nameof(EditImport),vm);
        }

        /// <summary>
        /// 区分：取込の対象データを取得する
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private ActionResult GetEditImportExistsData(Index data)
        {
            var import = EditLayoutQuery.GetImportData(Common.Profile.User.ShipperId, data.Results.FirstOrDefault(x => x.Checked).TemplateId);
            import.ObjectList = GetObjectList();
            return View(nameof(EditImport), import);
        }

        /// <summary>
        /// EditImportのインスタンス作成
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private EditImport CreateEditImportInstance(EditLayoutBase data)
        {
            var vm = new EditImport();

            vm.ObjectList = GetObjectList();
            vm.ShipperId = data.ShipperId;
            vm.TemplateId = data.TemplateId;
            vm.LayoutName = data.LayoutName;
            vm.IoClass = data.IoClass;
            vm.IoClassName = EnumHelperEx.GetDisplayValue(data.IoClass);
            return vm;
        }
    }
}