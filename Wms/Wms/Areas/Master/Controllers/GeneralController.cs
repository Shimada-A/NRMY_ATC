namespace Wms.Areas.Master.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.ViewModels.General;
    using Wms.Controllers;
    using Wms.Resources;

    public class GeneralController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "WMASGeneral01.SearchConditions";

        private General _GeneralQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralController"/> class.
        /// </summary>
        public GeneralController()
        {
            this._GeneralQuery = new General();
        }

        #endregion Constants

        #region Search

        /// <summary>
        /// Search General
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult Index()
        {
            var searchInfo = this.GetPreviousSearchInfo(true);
            return this.GetSearchResultView(searchInfo, true);
        }

        /// <summary>
        /// Search General
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
        /// <param name="SearchConditions"</param>
        /// <returns>List Record</returns>
        public ActionResult Search(GeneralSearchCondition SearchConditions)
        {
            GeneralSearchCondition condition;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        public JsonResult GetPKeys(GeneralSearchCondition SearchConditions)
        {
            List<string> ids = _GeneralQuery.GetRowId(SearchConditions);
            return Json(ids, JsonRequestBehavior.AllowGet);
        }

        #endregion Search

        #region Edit

        /// <summary>
        /// Insert General Information
        /// </summary>
        /// <param name="SearchConditions"</param>
        /// <returns>Insert View</returns>
        public ActionResult Insert(GeneralSearchCondition SearchConditions)
        {
            var input = new GeneralInput()
            {
                General = new General(),
                InUpDiff = "0",
            };
            input.General.ShipperId = Common.Profile.User.ShipperId;
            input.General.RegisterDiviCd = "1";
            input.SearchFlag= SearchConditions.SearchFlag;

            ViewBag.Loc = _GeneralQuery.GetSelectLocListItems();
            ViewBag.Gen = _GeneralQuery.GetSelectGenListItems();

            return View("~/Areas/Master/Views/General/Edit.cshtml", input);
        }

        /// <summary>
        /// Delete General Information
        /// </summary>
        /// <param name="rowid">rowid</param>
        /// <returns>Edit View</returns>
        public ActionResult Delete(string rowid)
        {
            List<string> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(rowid);

            if (_GeneralQuery.DeleteGeneral(list))
            {
                TempData[AppConst.SUCCESS] = MessagesResource.SUC_DELETE;
            }
            else
            {
                TempData[AppConst.ERROR] = MessagesResource.ERR_EXCLUSIVE_DELETE;
            }

            return RedirectToAction("IndexSearch");
        }

        /// <summary>
        /// Edit General Information
        /// </summary>
        /// <param name="rowid">rowid</param>
        /// <returns>Edit View</returns>
        public ActionResult Edit(string rowid)
        {
            // Get record from DB
            var target = _GeneralQuery.GetTargetById(rowid);

            // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって削除済みの場合）
            if (target == null)
            {
                TempData[AppConst.ERROR] = MessagesResource.MSG_NOT_FOUND;
                return RedirectToAction("IndexSearch");
            }

            var input = new GeneralInput()
            {
                General = target,
                InUpDiff = "1",
            };
            input.SearchFlag = true;
            ViewBag.Loc = _GeneralQuery.GetSelectLocListItems();
            ViewBag.Gen = _GeneralQuery.GetSelectGenListItems();

            return View("~/Areas/Master/Views/General/Edit.cshtml", input);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="general">General Information</param>
        /// <param name="inUpDiff">"0":Insert, "1":Update</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind]General general,
                                    string inUpDiff)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList();

                // エラー内容を取得・セット
                foreach (var message in errorMessages)
                {
                    ModelState.AddModelError(string.Empty, message);
                }

                var input = new GeneralInput()
                {
                    General = general,
                    InUpDiff = inUpDiff,
                };
                input.General.UpdateCount = general.UpdateCount;
                input.General.ShipperId = Common.Profile.User.ShipperId;
                ViewBag.Loc = _GeneralQuery.GetSelectLocListItems();
                ViewBag.Gen = _GeneralQuery.GetSelectGenListItems();
                return View("~/Areas/Master/Views/General/Edit.cshtml", input);
            }

            if (inUpDiff == "1")
            {
                if (_GeneralQuery.UpdateGeneral(general))
                {
                    // Clear message to back to index screen
                    TempData[AppConst.SUCCESS] = MessagesResource.SUC_UPDATE;
                }
                else
                {
                    TempData[AppConst.ERROR] = MessagesResource.ERR_EXCLUSIVE_UPDATE;
                }
            }
            else if (inUpDiff == "0")
            {
                // 登録の場合
                var CreateGeneralObj = _GeneralQuery.GetTargetById(general.RegisterDiviCd, general.GenDivCd, general.GenCd, general.CenterId, Common.Profile.User.ShipperId);
                if (CreateGeneralObj != null)
                {
                    var input = new GeneralInput()
                    {
                        General = general,
                        InUpDiff = inUpDiff,
                    };
                    input.General.UpdateCount = general.UpdateCount;
                    input.General.ShipperId = Common.Profile.User.ShipperId;
                    ViewBag.Loc = _GeneralQuery.GetSelectLocListItems();
                    ViewBag.Gen = _GeneralQuery.GetSelectGenListItems();
                    TempData[AppConst.ERROR] = MessagesResource.ERR_ALREADY_SAVED;
                    return View("~/Areas/Master/Views/General/Edit.cshtml", input);
                }

                if (_GeneralQuery.InsertGeneral(general))
                {
                    TempData[AppConst.SUCCESS] = MessagesResource.SUC_INSERT;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData[AppConst.ERROR] = MessagesResource.ERR_ALREADY_SAVED;
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("IndexSearch");
        }

        #endregion Edit

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>GeneralSearchCondition</returns>
        private GeneralSearchCondition GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new GeneralSearchCondition() : Request.Cookies.Get<GeneralSearchCondition>(COOKIE_SEARCHCONDITIONS) ?? new GeneralSearchCondition();
            condition.PageSize = this.GetCurrentPageSize();
            condition.Page = 1;

            // return search object
            return condition;
        }

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="condition">Search General Information</param>
        /// <returns>Index View</returns>
        private ActionResult GetSearchResultView(GeneralSearchCondition condition, bool indexFlag)
        {
            // Save search info
            var vm = new Index
            {
                SearchConditions = condition,
                GeneralResult = indexFlag ? new GeneralResult() : new GeneralResult()
                {
                    Generals = _GeneralQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.GeneralResult.Generals.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.GeneralResult.Generals = null;
                }
            }

            // 検索用セレクトボックス値を取得
            ViewBag.Loc = _GeneralQuery.GetSelectListCenters(condition.GenDivCd);
            ViewBag.Gen = _GeneralQuery.GetSelectGenListItems();

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            // Return index view
            return this.View("~/Areas/Master/Views/General/Index.cshtml", vm);
        }

        #endregion Private

        #region GetList

        /// <summary>
        /// Get コードより名称を取得
        /// <param name="kbn">マスタ区分</param>
        /// <param name="cd">検索用コード</param>
        /// </summary>
        /// <returns>検索名称</returns>
        public JsonResult GetCenterList(string genDivCd)
        {
            string _html = "<option value=''></option>";
            string _value = "@@@";

            var listCenter = _GeneralQuery.GetSelectListCenters(genDivCd);

            if(!listCenter.Any())
            {
                _value = "";
            }

            foreach (var center in listCenter)
            {
                _html = _html + "<option value='" + center.Value + "'>" + center.Text + "</option>";
            }
            if (listCenter.Any() && listCenter.Count() > 1)
            {
                _value = Common.Profile.User.CenterId;
            }

            return this.Json(new { html = _html, value = _value });
        }
        #endregion
    }
}