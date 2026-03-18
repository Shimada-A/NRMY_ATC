using PagedList;
using Share.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wms.Areas.Master.Models;
using Wms.Areas.Master.Query.EditLayout;
using Wms.Areas.Master.Resources;
using Wms.Areas.Master.ViewModels.EditLayout;
using Wms.Common;
using Wms.Controllers;

namespace Wms.Areas.Master.Controllers
{
    public partial class EditLayoutController : BaseController
    {
        /// <summary>
        /// コントローラー名
        /// </summary>
        public const string ControllerName = "EditLayout";

        // GET: Master/EditLayout
        public ActionResult Index(string infoMessage = "")
        {
            ViewBag.InfoMessage = infoMessage;
            return GetIndex();
        }

        /// <summary>
        /// Indexでのデータ検索処理
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchIndex(Index vm)
        {
            // レイアウト一覧取得
            vm.Results = Layout.GetLayouts(Common.Profile.User.ShipperId, vm.IoClass, vm.LayoutName);
            return View(nameof(Index),vm);
        }

        /// <summary>
        /// 指定のテンプレート名を持ったデータがあるか検索
        /// </summary>
        /// <param name="ioClass"></param>
        /// <param name="templateName"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchLayout(IoClass ioClass,string templateName)
        {
            var data = Layout.GetLayout(Common.Profile.User.ShipperId, ioClass, templateName);
            return Json(data ?? new Layout(),JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// データ削除処理
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Index vm)
        {
            EditLayoutQuery.Delete(vm.Results.FirstOrDefault(x => x.Checked).TemplateId);
            return RedirectToAction(nameof(Index),new { infoMessage = EditLayoutResource.Deleted});
        }
                
        /// <summary>
        ///  Indexの画面取得
        /// </summary>
        /// <returns></returns>
        public ActionResult GetIndex()
        {
            // 会社IDをセットしてIndexViewModel作成
            var vm = new Index
            {
                ShipperId = Common.Profile.User.ShipperId
            };
            return View(nameof(Index),vm);
        }
    }
}