using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Wms.Areas.Master.Models;
using Wms.Areas.Master.Query.EditLayout;
using Wms.Areas.Master.ViewModels.EditLayout;
using Wms.Common;
using Wms.Controllers;

namespace Wms.Areas.Master.Controllers
{
    public partial class EditLayoutController : BaseController
    {
        /// <summary>
        /// ソート設定画面呼び出し
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private ActionResult GetEditSort(EditLayoutBase data)
        {
            return View(nameof(EditSort), UpdateEditCondition(data));
        }

        /// <summary>
        /// ソート設定画面呼び出し、出力設定画面から遷移用
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSortFromOuput(EditOutput vm)
        {
            ModelState.Clear();

            // 出力設定ViewModel保存
            SaveEditOutput(vm);
            return GetEditSort(vm);
        }

        /// <summary>
        /// ソート設定画面呼び出し、条件設定画面から遷移用
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSortFromCondition(EditCondition vm)
        {
            ModelState.Clear();
            
            // 条件設定ViewModel保存
            SaveEditCondition(vm);
            return GetEditSort(vm);
        }

        /// <summary>
        ///  更新/登録処理
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSort(EditCondition vm)
        {
            return ConfirmEditCodition(vm);
        }

        /// <summary>
        /// 一覧取得処理
        /// </summary>
        /// <param name="shipperId"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult GetEditSortDetail(string shipperId,string objectId)
        {
            // 現在の状態を更新する
            UpdateObjectDetails(shipperId, objectId);
            return PartialView("_EditSortDetail", Session[nameof(EditCondition)] as EditCondition);
        }
    }
}