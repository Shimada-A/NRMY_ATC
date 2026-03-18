using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Wms.Areas.Master.Models;
using Wms.Areas.Master.ViewModels.EditLayout;
using Wms.Common;
using Wms.Controllers;
using Wms.Areas.Master.Extensions;
using Wms.Areas.Master.Query.EditLayout;

namespace Wms.Areas.Master.Controllers
{
    public partial class EditLayoutController : BaseController
    {

        /// <summary>
        /// 条件設定画面呼び出し
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private ActionResult GetEditCondition(EditLayoutBase data)
        {
            return View(nameof(EditCondition),UpdateEditCondition(data));
        }

        private EditCondition UpdateEditCondition(EditLayoutBase data)
        {
            // Session内のインスタンスの状態を最新にする
            var vm = Session[nameof(EditCondition)] as EditCondition;

            // 新規登録時
            if (data.IsNewLayout)
            {
                if (string.IsNullOrEmpty(data.ObjectId))
                {
                    // オブジェクトID が空の場合はリストも空
                    vm.Details = null;
                }
                else if (!string.IsNullOrEmpty(data.ObjectId)
                    && data.ObjectId != vm.ObjectId)
                {
                    // オブジェクトID が入力されていて、
                    // かつ、元々のオブジェクトIDと異なる場合はリストの取得をする
                    vm.Details = GetObjectDetail(data.ObjectId);
                }
            }
            CopyLayoutBase(data, vm);
            return vm;
        }

        /// <summary>
        /// 出力設定からの遷移
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditConditionFromOutput(EditOutput vm)
        {
            ModelState.Clear();
            SaveEditOutput(vm);
            return GetEditCondition(vm);
        }

        /// <summary>
        /// ソート設定からの遷移
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditConditionFromSort(EditCondition vm)
        {
            ModelState.Clear();

            // EditCondition保存
            SaveEditCondition(vm);
            return GetEditCondition(vm);
        }

        /// <summary>
        /// 条件設定ViewModelの状態保存
        /// </summary>
        /// <param name="vm"></param>
        private void SaveEditCondition(EditCondition vm)
        {
            var condition = Session[nameof(EditCondition)] as EditCondition;
            condition.Details = vm.Details;
        }

        /// <summary>
        /// 登録/更新処理
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCondition(EditCondition vm)
        {
            return ConfirmEditCodition(vm);
        }

        /// <summary>
        /// 登録/更新後、Indexに戻る
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        private ActionResult ConfirmEditCodition(EditCondition vm)
        {
            var editOutput = Session[nameof(EditOutput)] as EditOutput;
            EditLayoutQuery.Confirm(vm, editOutput, vm);

            return IndexAfterConfirm();
        }

        /// <summary>
        /// 条件設定のオブジェクト明細取得処理
        /// </summary>
        /// <param name="shipperId"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult GetEditConditionObjectDetail(string shipperId,string objectId)
        {
            // 状態を更新
            UpdateObjectDetails(shipperId, objectId);
            return PartialView("_EditConditionDetail", Session[nameof(EditCondition)] as EditCondition);
        }
    }
}