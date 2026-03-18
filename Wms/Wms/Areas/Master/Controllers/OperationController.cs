namespace Wms.Areas.Master.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Web.Mvc;
    using Dapper;
    using Glimpse.Core.Extensions;
    using NLog.Targets;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using StyleCop;
    using WebGrease.Css.Extensions;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.Resources;
    using Wms.Areas.Master.ViewModels.Operation;
    using Wms.Areas.Move.Query.InputTransfer;
    using Wms.Areas.Ship.ViewModels.PrintEcInvoice;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;

    public class OperationController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "WMASOperation01.SearchConditions";

        private Operation _OperationQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationController/> class.
        /// </summary>
        public OperationController()
        {
            this._OperationQuery = new Operation();
        }

        #endregion Constants

        #region Search

        /// <summary>
        /// Search
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult Index()
        {
            var searchInfo = this.GetPreviousSearchInfo(true);
            return this.GetSearchResultView(searchInfo, true);
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult IndexSearch()
        {
            var searchInfo = this.GetPreviousSearchInfo(false);
            return this.GetSearchResultView(searchInfo, false);

        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="SearchConditions"List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Search(OperationSearchCondition SearchConditions)
        {
            OperationSearchCondition condition;

            // When page is clicked, page > 1
            if (SearchConditions.Page >= 1)
            {
                condition = this.GetPreviousSearchInfo(false);
                condition.Page = SearchConditions.Page;
            }
            else
            {
                condition = SearchConditions;
                condition.PageSize = this.GetCurrentPageSize();
                condition.Page = 1;
            }

            return this.GetSearchResultView(condition, false);

        }

        public JsonResult GetPKeys(OperationSearchCondition SearchConditions)
        {
            List<string> ids = _OperationQuery.GetOperationId(SearchConditions).Select(n => n.OperationId).ToList();
            return Json(ids, JsonRequestBehavior.AllowGet);
        }
        #endregion Search

        #region Add

        /// <summary>
        /// 新規作成画面
        /// </summary>
        /// <returns>Create View</returns>
        public ActionResult Create(OperationSearchCondition SearchConditions)
        {
            var input = new Detail
            {
                InsertFlag = true
            };
            input.SearchFlag = SearchConditions.SearchFlag;
            ViewBag.CategoryName = _OperationQuery.GetSelectListCategoryName();
            return View("~/Areas/Master/Views/Operation/Edit.cshtml", input);
        }

        /// <summary>
        /// 新規作成処理
        /// </summary>
        /// <param name="operation">Operation Information</param>
        /// <returns>Create View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Detail operation, string Notes)
        {
            ModelState.Remove("ShipperId");

            //Editのhiddataに格納されているNotesをstring型のリストに変換
            List<string> noteList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(Notes);

            var tableNotes = OperationNoteErr(noteList);

            var input = new Detail()
            {
                OperationId = operation.OperationId,
                OperationName = operation.OperationName,
                CategoryName = operation.CategoryName,
                QtyZeroFlag = operation.QtyZeroFlag,
                OperationNotes = tableNotes,
                InsertFlag = true
            };

            if (ModelState.IsValid)
            {
                var operationExisted = _OperationQuery.GetTargetById(operation);

                if (operationExisted != null)
                {
                    TempData[AppConst.ERROR] = MessagesResource.ERR_ALREADY_SAVED;
                    ViewBag.CategoryNameList = CategoryItemListErr(operation);
                    return View("~/Areas/Master/Views/Operation/Edit.cshtml", input);
                }
                else
                {
                    if (_OperationQuery.Create(operation, noteList))
                    {
                        TempData[AppConst.SUCCESS] = MessagesResource.SUC_INSERT;
                        return RedirectToAction("Index");
                    }
                }
            }

            // エラー内容を取得・セット
            var errorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList();
            foreach (var message in errorMessages)
            {
                ModelState.AddModelError(string.Empty, message);
            }

            ViewBag.CategoryNameList = CategoryItemListErr(operation);
            return View("~/Areas/Master/Views/Operation/Edit.cshtml", input);
        }

        #endregion Add

        #region Edit

        /// <summary>
        /// Edit Operation Information
        /// </summary>
        /// <param name="operations">Operations</param>
        /// <returns>Edit View</returns>
        public ActionResult Edit(List<Detail> operations)
        {
            string operationId = operations.Where(n => n.IsCheck).Select(n => n.OperationId).FirstOrDefault();
            // Get record from DB
            var target = _OperationQuery.GetEditTargetById(operationId);
            var operationNote = _OperationQuery.OperationNotes(operationId);

            // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって削除済みの場合）
            if (target == null)
            {
                TempData[AppConst.ERROR] = MessagesResource.MSG_NOT_FOUND;
                return RedirectToAction("IndexSearch");
            }

            var input = new Detail
            {
                OperationId = target.OperationId,
                OperationName = target.OperationName,
                ShipperId = target.ShipperId,
                UpdateCount = target.UpdateCount,
                CategoryName = target.CategoryName,
                QtyZeroFlag = target.QtyZeroFlag,
                OperationNotes = operationNote,
                InsertFlag = false
            };
            input.SearchFlag = true;

            ViewBag.CategoryNameList = _OperationQuery.GetSelectListCategoryName();
            return View("~/Areas/Master/Views/Operation/Edit.cshtml", input);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="operation"></param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Detail operation, string Notes)
        {
            //Editのhiddataに格納されているNotesをstring型のリストに変換
            List<string> noteList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(Notes);

            var tableNotes = OperationNoteErr(noteList);

            var input = new Detail()
            {
                OperationId = operation.OperationId,
                OperationName = operation.OperationName,
                CategoryName = operation.CategoryName,
                ShipperId = operation.ShipperId,
                UpdateCount = operation.UpdateCount,
                QtyZeroFlag = operation.QtyZeroFlag,
                OperationNotes = tableNotes,
                InsertFlag = false
            };

            if (ModelState.IsValid)
            {
                if (_OperationQuery.UpdateOperation(operation, noteList))
                {
                    // Clear message to back to index screen
                    TempData[AppConst.SUCCESS] = MessagesResource.SUC_UPDATE;
                    return RedirectToAction("IndexSearch");
                }
                else
                {
                    TempData[AppConst.ERROR] = MessagesResource.ERR_EXCLUSIVE_UPDATE;
                    return RedirectToAction("IndexSearch");
                }
            }

            // エラー内容を取得・セット
            var errorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList();
            foreach (var message in errorMessages)
            {
                ModelState.AddModelError(string.Empty, message);
            }
            ViewBag.CategoryNameList = CategoryItemListErr(operation);
            return View("~/Areas/Master/Views/Operation/Edit.cshtml", input);
        }

        #endregion Edit

        #region Delete

        /// <summary>
        /// Delete patternId
        /// </summary>
        /// <param name="operationId">Operations</param>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string operationId)
        {
            List<string> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(operationId);

            var isSuccess = _OperationQuery.Delete(list);

            if (isSuccess)
            {
                TempData[AppConst.SUCCESS] = MessagesResource.SUC_DELETE;
            }
            else
            {
                TempData[AppConst.ERROR] = MessagesResource.MSG_ERR_EXCLUSIVE_DELETE;
            }

            return RedirectToAction("IndexSearch");
        }

        #endregion Delete

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>OperationSearchCondition</returns>
        private OperationSearchCondition GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new OperationSearchCondition() : Request.Cookies.Get<OperationSearchCondition>(COOKIE_SEARCHCONDITIONS) ?? new OperationSearchCondition();
            condition.PageSize = this.GetCurrentPageSize();
            condition.Page = 1;

            // return search object
            return condition;
        }

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="condition">Search Operation Information</param>
        /// <returns>Index View</returns>
        private ActionResult GetSearchResultView(OperationSearchCondition condition, bool indexFlag)
        {
            // Save search info
            var vm = new Index
            {

                SearchConditions = condition
                ,
                OperationResult = indexFlag ? new OperationResult() : new OperationResult()
                {
                    Operations = _OperationQuery.GetData(condition)
                }

            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.OperationResult.Operations.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.OperationResult.Operations = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);
            ViewBag.CategoryNameList = _OperationQuery.GetSelectListCategoryName();

            // Return index view
            return this.View("~/Areas/Master/Views/Operation/Index.cshtml", vm);
        }

        /// <summary>
        /// エラー時に更新画面に戻る時の備考欄作成
        /// </summary>
        /// <param name="noteList"></param>
        private List<OperationNoteItem> OperationNoteErr(List<string> noteList)
        {
            var operationNoteItems = new List<OperationNoteItem>();

            if (noteList != null)
            {
                for (int i = 0; i < noteList.Count; i++)
                {
                    var noteItem = new OperationNoteItem();

                    noteItem.OperationNote = noteList[i];
                    noteItem.Seq = i;

                    operationNoteItems.Add(noteItem);
                }
            }
            return operationNoteItems;
        }

        /// <summary>
        /// エラー時に戻る時のSelect2リストアイテム生成
        /// </summary>
        /// <param name="operation"></param>
        private IEnumerable<SelectListItem> CategoryItemListErr(Detail operation)
        {
            var cateories = _OperationQuery.GetSelectListCategoryName();

            bool existCheck = cateories.Where(m => m.Value == operation.CategoryName).Any();

            if (!existCheck)
            {
                cateories = cateories.Append(new SelectListItem()
                {
                    Text = operation.CategoryName,
                    Value = operation.CategoryName
                });
            }

            return cateories;
        }

        #endregion Private

    }
}