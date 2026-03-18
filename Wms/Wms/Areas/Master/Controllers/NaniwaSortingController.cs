namespace Wms.Areas.Master.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Master.Reports.Export;
    using Wms.Areas.Master.Resources;
    using Wms.Areas.Master.ViewModels.NaniwaSorting;
    using Wms.Controllers;
    using Wms.Resources;
    using NaniwaSortingQuery = Query.NaniwaSorting.NaniwaSortingQuery;

    public class NaniwaSortingController : BaseController
    {
        #region Constants

        private const string COOKIE_SEARCHCONDITIONS = "WMASNaniwaSorting01.SearchConditions";
        private NaniwaSortingQuery _NaniwaSortingQuery;

        public NaniwaSortingController()
        {
            this._NaniwaSortingQuery = new NaniwaSortingQuery();
        }

        /// <summary>
        /// 配送センターコード 汎用コードマスタ登録状況
        /// </summary>
        public enum EnumNaniwaDeliCenterCdStatus : int
        {
            /// <summary>
            /// 汎用コードマスタ 登録状況未確認
            /// </summary>
            UnConfirmed = -1,

            /// <summary>
            /// 汎用コードマスタ未登録
            /// </summary>
            NotExists = 0,

            /// <summary>
            /// 汎用コードマスタ未登録&未登録メッセージ確認済み
            /// </summary>
            IsConfirmed = 1
        }

        #endregion

        #region Search

        /// <summary>
        /// 一覧画面表示
        /// </summary>
        /// <returns>検索結果</returns>
        public ActionResult Index()
        {
            var searchInfo = this.GetPreviousSearchInfo(true);
            return this.GetSearchResultView(searchInfo, true);
        }

        /// <summary>
        /// 一覧画面表示
        /// </summary>
        /// <returns>検索結果</returns>
        public ActionResult IndexSearch()
        {
            var searchInfo = this.GetPreviousSearchInfo(false);
            return this.GetSearchResultView(searchInfo, false);
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="SearchConditions">検索条件</param>
        /// <returns>検索結果</returns>
        public ActionResult Search(NaniwaSortingSearchCondition SearchConditions)
        {
            NaniwaSortingSearchCondition condition;

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

            condition.SearchFlag= true;
            return this.GetSearchResultView(condition, false);
        }

        /// <summary>
        /// ROWID取得
        /// </summary>
        /// <param name="SearchConditions">検索条件</param>
        /// <returns></returns>
        public JsonResult GetPKeys(NaniwaSortingSearchCondition SearchConditions)
        {
            List<string> ids = _NaniwaSortingQuery.GetRowId(SearchConditions).Select(n => n.RowId).ToList();
            return Json(ids, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Add

        /// <summary>
        /// 新規作成画面表示
        /// </summary>
        /// <returns>Create View</returns>
        public ActionResult Create(NaniwaSortingSearchCondition SearchConditions)
        {
            var input = new Detail
            {
                ShipperId = Common.Profile.User.ShipperId,
                StoreId = SearchConditions.StoreId,
                StoreName = SearchConditions.StoreName,
                NaniwaDeliCenterCd = SearchConditions.NaniwaDeliCenterCd,
                InsertFlag = true
            };
            input.SearchFlag= SearchConditions.SearchFlag;

            return View("~/Areas/Master/Views/NaniwaSorting/Edit.cshtml", input);
        }

        /// <summary>
        /// 新規作成処理
        /// </summary>
        /// <param name="NaniwaSorting">画面に入力したデータ</param>
        /// <param name="naniwaDeliCenterCdStatus">配送センターコード 汎用コードマスタ登録状況</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Detail NaniwaSorting, EnumNaniwaDeliCenterCdStatus naniwaDeliCenterCdStatus)
        {
            // 店舗コードがマスタに登録されていない場合、エラーメッセージ表示
            if (!_NaniwaSortingQuery.CheckStoreIdMasterExists(NaniwaSorting.StoreId))
            {
                this.ModelState.AddModelError("StoreId", string.Format(MessagesResource.MasterNotExistsError, NaniwaSortingResource.StoreId));
            }

            if (ModelState.IsValid)
            {
                // 配送センターコードチェック
                if (ExistsNaniwaDeliCenterCd(NaniwaSorting, naniwaDeliCenterCdStatus))
                {
                    var NaniwaSortingExisted = _NaniwaSortingQuery.GetTargetById(NaniwaSorting.StoreId, Common.Profile.User.ShipperId);
                    if (NaniwaSortingExisted != null)
                    {
                        TempData[AppConst.ERROR] = MessagesResource.ERR_ALREADY_SAVED;
                        return View("~/Areas/Master/Views/NaniwaSorting/Edit.cshtml", new Detail() { InsertFlag = true });
                    }
                    else
                    {
                        if (_NaniwaSortingQuery.Create(NaniwaSorting))
                        {
                            TempData[AppConst.SUCCESS] = MessagesResource.SUC_INSERT;
                            return RedirectToAction("Index");
                        }
                    }
                }
            }

            // エラー内容を取得・セット
            var errorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList();
            foreach (var message in errorMessages)
            {
                ModelState.AddModelError(string.Empty, message);
            }

            return View("~/Areas/Master/Views/NaniwaSorting/Edit.cshtml", new Detail() { InsertFlag = true });
        }

        #endregion

        #region Edit

        /// <summary>
        /// 詳細画面表示
        /// </summary>
        /// <param name="rowid">一覧でチェックされた行のROWID</param>
        /// <returns></returns>
        public ActionResult Edit(string rowid)
        {
            var input = _NaniwaSortingQuery.GetTargetById(rowid);

            input.SearchFlag = true;
            // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって削除済みの場合）
            if (input == null)
            {
                TempData[AppConst.ERROR] = MessagesResource.MSG_NOT_FOUND;
                return RedirectToAction("IndexSearch");
            }

            return View("~/Areas/Master/Views/NaniwaSorting/Edit.cshtml", input);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="NaniwaSorting">入力データ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Detail NaniwaSorting, EnumNaniwaDeliCenterCdStatus naniwaDeliCenterCdStatus)
        {
            if (ModelState.IsValid)
            {
                // 配送センターコードチェック
                if (ExistsNaniwaDeliCenterCd(NaniwaSorting, naniwaDeliCenterCdStatus))
                {
                    if (_NaniwaSortingQuery.Update(NaniwaSorting))
                    {
                        TempData[AppConst.SUCCESS] = MessagesResource.SUC_UPDATE;
                        return RedirectToAction("IndexSearch");
                    }
                    else
                    {
                        TempData[AppConst.ERROR] = MessagesResource.ERR_EXCLUSIVE_UPDATE;
                        return RedirectToAction("IndexSearch");
                    }
                }
            }

            // エラー内容を取得・セット
            var errorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList();
            foreach (var message in errorMessages)
            {
                ModelState.AddModelError(string.Empty, message);
            }

            return View("~/Areas/Master/Views/NaniwaSorting/Edit.cshtml", NaniwaSorting);
        }
        #endregion

        #region Delete

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <param name="rowid">一覧でチェックされた行のROWID</param>
        /// <returns>Delete View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string rowid)
        {
            List<string> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(rowid);

            string isSuccess = _NaniwaSortingQuery.Delete(list);

            if (string.IsNullOrEmpty(isSuccess))
            {
                TempData[AppConst.SUCCESS] = MessagesResource.SUC_DELETE;
            }
            else
            {
                TempData[AppConst.ERROR] = MessagesResource.MSG_ERR_EXCLUSIVE_DELETE;
            }

            return RedirectToAction("IndexSearch");
        }

        #endregion

        #region ロード処理

        /// <summary>
        /// Excelダウンロード
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Download()
        {
            NaniwaSortingSearchCondition searchCondition = this.GetPreviousSearchInfo(false);

            NaniwaSortingReport report = new NaniwaSortingReport(ReportTypes.Excel, searchCondition);
            report.Export();
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        #endregion ロード処理

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <param name="indexFlag"></param>
        /// <returns>前回の検索条件</returns>
        private NaniwaSortingSearchCondition GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new NaniwaSortingSearchCondition() : Request.Cookies.Get<NaniwaSortingSearchCondition>(COOKIE_SEARCHCONDITIONS) ?? new NaniwaSortingSearchCondition();
            condition.PageSize = this.GetCurrentPageSize();
            condition.Page = 1;

            return condition;
        }

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="indexFlag"></param>
        /// <returns>検索結果</returns>
        private ActionResult GetSearchResultView(NaniwaSortingSearchCondition condition, bool indexFlag)
        {
            var vm = new Index()
            {
                SearchConditions = condition,
                NaniwaSortingResult = indexFlag ? new NaniwaSortingResult() : new NaniwaSortingResult()
                {
                    NaniwaSortings = _NaniwaSortingQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.NaniwaSortingResult.NaniwaSortings.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.NaniwaSortingResult.NaniwaSortings = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            // Return index view
            return this.View("~/Areas/Master/Views/NaniwaSorting/Index.cshtml", vm);
        }

        /// <summary>
        /// 配送センターコードが汎用コードマスタに登録されていない場合、メッセージ表示（ダイアログ）
        /// </summary>
        /// <param name="NaniwaSorting">入力データ</param>
        /// <param name="naniwaDeliCenterCdStatus">配送センターコード 汎用コードマスタ登録状況</param>
        /// <returns>true:汎用コードマスタに登録されている</returns>
        private bool ExistsNaniwaDeliCenterCd(Detail NaniwaSorting, EnumNaniwaDeliCenterCdStatus naniwaDeliCenterCdStatus)
        {
            // 未登録メッセージ確認済み場合、チェックしない
            if (naniwaDeliCenterCdStatus == EnumNaniwaDeliCenterCdStatus.IsConfirmed)
            {
                return true;
            }

            var existsNaniwaDeliCenterCd = _NaniwaSortingQuery.ExistsNaniwaDeliCenterCd(NaniwaSorting.NaniwaDeliCenterCd);
            if (!existsNaniwaDeliCenterCd)
            {
                ViewBag.NaniwaDeliCenterCdStatus = EnumNaniwaDeliCenterCdStatus.NotExists;
                ViewBag.NaniwaDeliCenterCdMessage = string.Format(MessageResource.GenCdNotExistsError, NaniwaSortingResource.NaniwaDeliCenterCd, NaniwaSortingResource.GenDivCdNaniwaDeliCenterCd);
            }
            return existsNaniwaDeliCenterCd;
        }

        #endregion
    }
}