namespace Wms.Areas.Inventory.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using PagedList;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Inventory.Models;
    using Wms.Areas.Inventory.Query.Input;
    using Wms.Areas.Inventory.Resources;
    using Wms.Areas.Inventory.ViewModels.Input;
    using Wms.Common;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;

    public class InputController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_INV_Input.SearchConditions";

        private InputQuery _InputQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputController"/> class.
        /// </summary>
        public InputController()
        {
            this._InputQuery = new InputQuery();
        }

        #endregion Constants

        #region Search

        /// <summary>
        /// Search Country
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult Index()
        {
            InputViewModel vm = new InputViewModel()
            {
                SearchConditions = this.GetPreviousSearchInfo(true)
            };
            return this.GetSearchResultView(vm, true);
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="searchCondition"List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult IndexSearch(InputViewModel vm)
        {
            vm.SearchConditions = this.GetPreviousSearchInfo(false);
            vm.SearchConditions.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(vm, true);
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="searchCondition"List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult IndexUpd(InputViewModel vm)
        {
            vm.SearchConditions = this.GetPreviousSearchInfo(false);
            vm.SearchConditions.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(vm, true);
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="searchCondition"List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Search(InputViewModel vm)
        {
            vm.SearchConditions.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(vm, false);
        }

        #endregion Search

        #region 行追加

        /// <summary>
        /// 行変更
        /// </summary>
        [HttpPost]
        public ActionResult AddTr(InputViewModel vm)
        {
            this.ModelState.Clear();
            var newResultRow = new InputResultRow();
            newResultRow.AddFlag = true;
            newResultRow.ChangeModel = vm.ChangeModel;
            newResultRow.CaseClass = vm.ChangeModel == "Case" ? 1 : 2;
            newResultRow.CaseClassName = vm.ChangeModel == "Case" ? InputResource.CaseCom : InputResource.BaraCom;
            newResultRow.StockQtyStart = 0;
            newResultRow.DifferenceQty = null;
            List<InputResultRow> tempLst = new List<InputResultRow>
            {
                newResultRow
            };
            if (vm.Results == null || vm.Results.InputList == null)
            {
                vm.Results = new InputResult()
                {
                    InputList = tempLst
                };
            }
            else
            {
                vm.Results.InputList.Add(newResultRow);
            }
            return this.PartialView("~/Areas/Inventory/Views/Input/_InputRows.cshtml", vm);
        }
        #endregion

        #region 更新処理

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="searchConditions">searchConditions</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSearch(InputViewModel vm)
        {
            ModelState.Clear();
            vm.SearchConditions.PageSize = this.GetCurrentPageSize();
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, vm.SearchConditions);
            if (Check(vm))
            {
                // ワーク更新
                _InputQuery.UpdateInvInput(vm);

                // 実績更新
                var message = string.Empty;
                ProcedureStatus status = ProcedureStatus.Success;
                _InputQuery.ResultUpdate(vm, out status, out message);
                if (status == ProcedureStatus.Success)
                {
                    // Clear message to back to index screen
                    TempData[AppConst.SUCCESS] = MessagesResource.SUC_UPDATE;
                    // Return index view
                    return RedirectToAction("IndexUpd");
                }
                else
                {
                    TempData[AppConst.ERROR] = message;
                    ViewBag.InventoryNoList = _InputQuery.GetInventoryNoList(vm.SearchConditions);
                    vm.Results.Inputs = new StaticPagedList<InputResultRow>(vm.Results.InputList, vm.SearchConditions.Page, vm.SearchConditions.PageSize, vm.SearchConditions.TotalCount);
                    // Return index view
                    return this.View("~/Areas/Inventory/Views/Input/Index.cshtml", vm);
                }
            }
            else
            {
                ViewBag.InventoryNoList = _InputQuery.GetInventoryNoList(vm.SearchConditions);
                var condition = Request.Cookies.Get<InputSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new InputSearchConditions();
                vm.SearchConditions.Page = condition.Page;
                vm.Results.Inputs = new StaticPagedList<InputResultRow>(vm.Results.InputList, vm.SearchConditions.Page, vm.SearchConditions.PageSize, condition.TotalCount);
                // Return index view
                return this.View("~/Areas/Inventory/Views/Input/Index.cshtml", vm);
            }
        }
        #endregion

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private InputSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new InputSearchConditions() : Request.Cookies.Get<InputSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new InputSearchConditions();
            condition.PageSize = this.GetCurrentPageSize();
            condition.Page = 1;

            // return search object
            return condition;
        }

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="condition">Search Country Information</param>
        /// <returns>Index View</returns>
        private ActionResult GetSearchResultView(InputViewModel vm, bool indexFlag)
        {
            this.ModelState.Clear();
            if ((indexFlag || vm.Results.InputList == null || vm.SearchConditions.SearchType == SearchTypes.Search) ? true : Check(vm))
            {
                // 作成処理&検索表示
                vm.Results = indexFlag ? new InputResult() :
                ((vm.SearchConditions.SearchType == SearchTypes.Search ? _InputQuery.InsertInvInput(vm.SearchConditions) : _InputQuery.UpdateInvInput(vm)) ? new InputResult()
                {
                    Inputs = _InputQuery.GetData(vm.SearchConditions)
                } : new InputResult());

                var ProcNumLimit = this.GetCurrentProcNumLimit();
                if (ProcNumLimit != 0 && vm != null && !indexFlag)
                {
                    if (vm.Results.Inputs.TotalItemCount > ProcNumLimit)
                    {
                        TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                        vm.Results.Inputs = null;
                    }
                }
                if (!indexFlag && vm.Results.Inputs != null)
                {
                    vm.Results.InputList = vm.Results.Inputs.ToList();
                }
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, vm.SearchConditions);
                ViewBag.InventoryNoList = _InputQuery.GetInventoryNoList(vm.SearchConditions);

                // Return index view
                return this.View("~/Areas/Inventory/Views/Input/Index.cshtml", vm);
            }
            else
            {
                ViewBag.InventoryNoList = _InputQuery.GetInventoryNoList(vm.SearchConditions);
                var condition = Request.Cookies.Get<InputSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new InputSearchConditions();
                vm.SearchConditions.Page = condition.Page;
                vm.Results.Inputs = new StaticPagedList<InputResultRow>(vm.Results.InputList, vm.SearchConditions.Page, vm.SearchConditions.PageSize, condition.TotalCount);
                // Return index view
                return this.View("~/Areas/Inventory/Views/Input/Index.cshtml", vm);
            }
        }

        /// <summary>
        /// 登録チェック
        /// </summary>
        /// <returns></returns>
        private bool Check(InputViewModel vm)
        {
            ItemInfo item = new ItemInfo();
            var message = string.Empty;
            var gradeId = string.Empty;
            var errcnt = 0;
            foreach (var cModel in vm.Results.InputList.Select((v, i) => new { v, i }))
            {
                if (cModel.v.AddFlag 
                    && (!string.IsNullOrWhiteSpace(cModel.v.LocationCd) ||
                        !string.IsNullOrWhiteSpace(cModel.v.BoxNo) ||
                        !string.IsNullOrWhiteSpace(cModel.v.Jan) ||
                        cModel.v.ResultQty != null ))
                {
                    item = new ItemInfo();
                    _InputQuery.GetItemInfo(cModel.v.Jan, out item);
                    if (item == null || string.IsNullOrWhiteSpace(item.ItemSkuId))
                    {
                        this.ModelState.AddModelError("Results.InputList[" + cModel.i + "].Jan", InputResource.ErrNotExistJan);
                        errcnt = errcnt + 1;
                    }
                    else
                    {
                        cModel.v.ItemSkuId = item.ItemSkuId;
                        cModel.v.ItemId = item.ItemId;
                        cModel.v.ItemName = item.ItemName;
                        cModel.v.ItemColorId = item.ItemColorId;
                        cModel.v.ItemColorName = item.ItemColorName;
                        cModel.v.ItemSizeId = item.ItemSizeId;
                        cModel.v.ItemSizeName = item.ItemSizeName;
                    }

                    message = string.Empty;
                    gradeId = string.Empty;
                    _InputQuery.CheckLocation(cModel.v.LocationCd, vm.SearchConditions.CenterId, vm.SearchConditions.InventoryNo, cModel.v.CaseClass, out gradeId, out message);
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        this.ModelState.AddModelError("Results.InputList[" + cModel.i + "].LocationCd", message);
                        errcnt = errcnt + 1;
                    }
                    else
                    {
                        cModel.v.GradeId = gradeId;
                    }
                }

                // 入力された数値のチェック
                // 負の数、もしくは少数が入力されていたらエラーとする
                if (!IsResultQtyValid(cModel.i, cModel.v.ResultQty))
                {
                    ModelState.AddModelError("Results.InputList[" + cModel.i + "].ResultQty", InputResource.ResultQtyIsNotPositiveInteger);
                    errcnt = errcnt + 1;
                }

                // 実績が既に登録されていた状態から実績数がnullで更新された場合はエラーとする
                if (cModel.v.ResultQtyBefore.HasValue && !cModel.v.ResultQty.HasValue)
                {
                    ModelState.AddModelError("Results.InputList[" + cModel.i + "].ResultQty", InputResource.RegisterdResultQtyCannotBeEmpty);
                    errcnt = errcnt + 1;
                }

            }

            return errcnt > 0 ? false : true;
        }

        /// <summary>
        /// 実績数チェック
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private bool IsResultQtyValid(int i, int? resultQty) 
        {
            // 実績数持ってるとき、負の数ならエラー
            if (resultQty.HasValue )
            {
                if (resultQty.Value < 0)
                    return false;
                else
                    return true;
            }

            // 実績数持ってない時、小数点もってたらエラー
            var resQtyStr = HttpContext.Request.Form[$"Results.InputList[{i}].ResultQty"];
            if (resQtyStr.Contains("."))
            {
                return false;
            }

            return true;
        }

        #endregion Private

        #region Change

        /// <summary>
        /// JAN変更
        /// </summary>
        public JsonResult JanChange(string jan)
        {
            ItemInfo item = new ItemInfo();

            _InputQuery.GetItemInfo(jan, out item);
            item = item == null ? new ItemInfo()
            {
                ItemSkuId = string.Empty,
                ItemId = string.Empty,
                ItemName = string.Empty,
                ItemColorId = string.Empty,
                ItemColorName = string.Empty,
                ItemSizeId = string.Empty,
                ItemSizeName = string.Empty
            } : item;

            return this.Json(
                new
                {
                    itemSkuId = item.ItemSkuId,
                    itemId = item.ItemId,
                    itemName = item.ItemName,
                    itemColorId = item.ItemColorId,
                    itemColorName = item.ItemColorName,
                    itemSizeId = item.ItemSizeId,
                    itemSizeName = item.ItemSizeName,
                });
        }

        /// <summary>
        /// ロケーション変更
        /// </summary>
        public JsonResult LocationCdChange(string locationCd, string centerId, string inventoryNo, int lineClass)
        {
            var _message = string.Empty;
            var _gradeId = string.Empty;
            _InputQuery.CheckLocation(locationCd, centerId, inventoryNo, lineClass, out _gradeId, out _message);

            return this.Json(
                new
                {
                    gradeId = _gradeId,
                    message = _message
                });
        }

        /// <summary>
        /// Get コードより名称を取得
        /// <param name="kbn">マスタ区分</param>
        /// <param name="cd">検索用コード</param>
        /// </summary>
        /// <returns>検索名称</returns>
        public JsonResult GetInventoryNoList(string CenterId)
        {
            string _html = "";

            var listInventoryNo = _InputQuery.GetSelectInventoryNoList(CenterId);
            foreach (var inventoryNo in listInventoryNo)
            {
                _html = _html + "<option value='" + inventoryNo.Value + "'>" + inventoryNo.Text + "</option>";
            }

            return this.Json(new { html = _html });
        }

        #endregion
    }
}