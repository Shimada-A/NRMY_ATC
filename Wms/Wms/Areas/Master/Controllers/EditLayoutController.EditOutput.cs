using PagedList;
using Share.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wms.Areas.Master.Models;
using Wms.Areas.Master.Query.EditLayout;
using Wms.Areas.Master.Resources;
using Wms.Areas.Master.ViewModels.EditLayout;
using Wms.Common;
using Wms.Common.Resources;
using Wms.Controllers;

namespace Wms.Areas.Master.Controllers
{
    public partial class EditLayoutController : BaseController
    {
        /// <summary>
        /// Indexからの画面遷移
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOutputFromIndex(Index vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));

            ModelState.Clear();

            if (vm.Results == null)
            {
                // vm.Resultsがない→新規
               return EditOutputNewData(vm);
            }
            else
            {
                // vm.Resultsがある→更新
                return EditOuputExistData(vm);
            }
        }

        /// <summary>
        ///  新規データ用
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        private ActionResult EditOutputNewData(Index vm)
        {
            // インスタンスを作成し、データをコピーする
            var newData = new EditOutput();
            newData.ShipperId = vm.ShipperId;
            newData.IsNewLayout = true; 
            newData.LayoutName = vm.LayoutName;
            newData.IoClass = vm.IoClass;
            newData.IoClassName = EnumHelperEx.GetDisplayValue(vm.IoClass);

            // セッションにインスタンス作成
            Session.Add(nameof(EditOutput),newData);
            Session.Add(nameof(EditCondition), new EditCondition());

            return EditOutputView(newData);
        }

        /// <summary>
        /// Indexでチェックを入れたデータを表示する
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        private ActionResult EditOuputExistData(Index vm)
        {
            // チェックが入っているデータを対象としてデータを取得する
            var (output, condition) = EditLayoutQuery.GetData(Common.Profile.User.ShipperId, vm.Results.FirstOrDefault(x => x.Checked).TemplateId);

            // セッションにインスタンス作成
            Session.Add(nameof(EditOutput), output);
            Session.Add(nameof(EditCondition), condition);

            return EditOutputView(output);
        }

        /// <summary>
        /// ソート設定からの遷移
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOutputFromSort(EditCondition vm)
        {
            return EditOutputView(vm);
        }

        /// <summary>
        /// 条件設定からの遷移
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOutputFromCondition(EditCondition vm)
        {
            return EditOutputView(vm);
        }

        /// <summary>
        ///  条件設定ViewModelを入力とし、出力設定のViewを取得
        /// </summary>
        /// <param name="condition">条件設定ViewModel</param>
        /// <returns></returns>
        private ActionResult EditOutputView(EditCondition condition)
        {
            ModelState.Clear();

            // 条件設定のViewを保存
            Session[nameof(EditCondition)] = condition;
            var vm = Session[nameof(EditOutput)] as EditOutput;
            CopyLayoutBase(condition, vm);
            return EditOutputView(vm);
        }

        /// <summary>
        /// 確定処理
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOutput(EditOutput vm)
        {
            // 確定処理を呼び出す
            var editCondition = Session[nameof(EditCondition)] as EditCondition;
            EditLayoutQuery.Confirm(vm, vm, editCondition);

            return IndexAfterConfirm(); 
        }

        /// <summary>
        /// 更新完了後、Indexに戻す
        /// </summary>
        /// <returns></returns>
        private ActionResult IndexAfterConfirm()
        {
            // Indexにリダイレクトする。
            // 更新完了メッセージも渡す
            return RedirectToAction(nameof(Index),
                new { infoMessage = EditLayoutResource.Updated});
        }

        /// <summary>
        /// Editoutput View 取得処理、オブジェクトのリストも取得する
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        private ActionResult EditOutputView(EditOutput vm)
        {
            vm.ObjectList = GetObjectList();
            return View(nameof(EditOutput), vm);
        }

        /// <summary>
        /// オブジェクト明細一覧取得用
        /// </summary>
        /// <param name="shipperId"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult GetEditOutputObjectDetail(string shipperId,string objectId)
        {
            UpdateObjectDetails(shipperId, objectId);

            return PartialView("_EditOutputDetail", Session[nameof(EditOutput)] as EditOutput);
        }

        /// <summary>
        /// 各ViewModelのオブジェクト詳細一覧更新
        /// </summary>
        /// <param name="shipperId"></param>
        /// <param name="objectId"></param>
        private void UpdateObjectDetails(string shipperId,string objectId)
        {
            
            // オブジェクト明細一覧を取得する時は、新規の場合なので、
            // 条件設定の方の一覧もインスタンスを作成しておく
            var condition = Session[nameof(EditCondition)] as EditCondition;

            // オブジェクトIDが同じ場合は更新不要
            if (objectId == condition.ObjectId)
                return;

            // オブジェクトIDが異なる場合は
            // オブジェクト一覧を、出力設定ViewModel、条件設定ViewModel共に更新する必要がある
            condition.Details = GetObjectDetail(objectId);
            condition.ObjectId = objectId;

            //  出力設定ViewModel更新
            var output = Session[nameof(EditOutput)] as EditOutput;
            output.ObjectDetails = condition.Details; //条件設定と同じオブジェクト詳細を持っていて良い
            output.EditOutputFileDetails = new List<EditOutputFileDetail>();
            output.ObjectId = objectId;
            //  出力設定の出力ファイル詳細のインスタンス作成
            for (int i = 0; i < output.ObjectDetails.Count; i++)
            {
                output.EditOutputFileDetails.Add(new EditOutputFileDetail()
                { ShipperId = shipperId, RowNo = i + 1, SubNo = 1 });
            }
        }

        /// <summary>
        ///  オブジェクト明細リスト取得
        /// </summary>
        /// <param name="shipperId"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        private static List<ObjectDetailDTO> GetObjectDetail(string objectId)
        {
            var objectDetails = ObjectDetail.GetData(Common.Profile.User.ShipperId, objectId).ToList();
            objectDetails.ForEach(x => x.DataTypeName = EnumHelperEx.GetDisplayValue(x.DataType));
            return objectDetails;
        }

        /// <summary>
        /// インスタンス情報コピー
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private void CopyLayoutBase(EditLayoutBase from,EditLayoutBase to)
        {
            to.ObjectList = GetObjectList();
            to.ShipperId = from.ShipperId;
            to.IsNewLayout = from.IsNewLayout;
            to.EncloseType = from.EncloseType;
            to.EncodeType = from.EncodeType;
            to.FileType = from.FileType;
            to.HeadingRow = from.HeadingRow;
            to.IoClass = from.IoClass;
            to.IoClassName = from.IoClassName;
            to.ObjectId = from.ObjectId;
            to.ObjectType = from.ObjectType;
            to.TemplateId = from.TemplateId;
            to.LayoutName = from.LayoutName;
        }

        /// <summary>
        /// オブジェクトテーブルのSelectList取得
        /// </summary>
        /// <returns></returns>
        private static SelectList GetObjectList()
        {
            var list = new Objects().GetData(Common.Profile.User.ShipperId);
            return new SelectList(list.ToList(), "ObjectId","ObjectName");
        }

        /// <summary>
        /// 出力設定の情報更新
        /// </summary>
        /// <param name="vm"></param>
        private void SaveEditOutput(EditOutput vm)
        {
            var output = Session[nameof(EditOutput)] as EditOutput;
            output.EditOutputFileDetails = vm.EditOutputFileDetails;
        }
    }
} 