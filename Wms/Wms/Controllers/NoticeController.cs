namespace Wms.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Query;
    using Wms.Resources;
    using Wms.ViewModels.Notice;

    public class NoticeController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_OTH_Notice.SearchConditions";

        private NoticeQuery _NoticeQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipFrontageController"/> class.
        /// </summary>
        public NoticeController()
        {
            this._NoticeQuery = new NoticeQuery();
        }

        #endregion Constants

        #region Search

        /// <summary>
        /// Search Country
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult Index()
        {
            var searchInfo = this.GetPreviousSearchInfo(true);
            return this.GetSearchResultView(searchInfo, true, -1);
        }

        /// <summary>
        /// Search Country
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult IndexSearch(int rowId)
        {
            var searchInfo = this.GetPreviousSearchInfo(false);
            return this.GetSearchResultView(searchInfo, false, rowId);
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="searchCondition"List Country Information</param>
        /// <returns>List Record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(NoticeSearchConditions SearchConditions)
        {
            NoticeSearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false, -1);
        }

        #endregion Search

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private NoticeSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new NoticeSearchConditions() : Request.Cookies.Get<NoticeSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new NoticeSearchConditions();
            condition.PageSize = this.GetCurrentPageSize();
            if (condition.Page == 0) { condition.Page = 1; }


            // return search object
            return condition;
        }

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="condition">Search Country Information</param>
        /// <returns>Index View</returns>
        private ActionResult GetSearchResultView(NoticeSearchConditions searchConditions, bool indexFlag, int index)
        {
            // 検索表示
            var vm = new NoticeViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new NoticeResult() : new NoticeResult()
                {
                    NoticeResultRows = _NoticeQuery.Listing(searchConditions)
                },
            };
            if (index >= 0) { vm.Results.NoticeResultRows[index].IsCheck = true; }

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.NoticeResultRows.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.NoticeResultRows = null;
                }
            }

            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);

            // Return index view
            return this.View("~/Views/Notice/Index.cshtml", vm);

        }


        #endregion Private

        #region Detail
        /// <summary>
        /// 詳細画面の表示
        /// </summary>
        /// <param name="SelectIndex">選択データ</param>
        /// <param name="IndexResult">検索結果</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Next(IList<NoticeDetailHeader> NoticeResultRows, string IsCheck)
        {
            var noticeResult = new NoticeDetailHeader();

            //チェックがないならエラー
            if (NoticeResultRows.Where(x => x.IsCheck == true).Count() != 1)
            {
                TempData[AppConst.ERROR] = string.Format(MessagesResource.SELECT_ONE_RECORD);
                return RedirectToAction("IndexSearch", new { rowId = -1 });
            }
            noticeResult = NoticeResultRows.Where(x => x.IsCheck == true).Single();

            string actionName = GetDetailActionName(noticeResult);

            return RedirectToAction(actionName, noticeResult);
        }

        /// <summary>
        /// 各詳細画面の呼び出し
        /// </summary>
        /// <param name="indexResult">検索結果</param>
        /// <returns></returns>
        private string GetDetailActionName(NoticeDetailHeader noticeHeader)
        {
            switch (noticeHeader.NoticeIfMessageId)
            {
                case "1":
                    //IF受信
                    return "DetailReceive";

                case "7":
                    //連携実行エラー
                    return "DetailIfRun";

                case "9":
                case "10":
                case "11":
                case "12":
                case "14":
                case "100":
                case "101":
                case "102":
                case "103":
                case "104":
                case "110":
                case "200":
                case "201":
                    //お知らせ明細表示
                    return "Detail";

                default:
                    //ヘッダのみ表示
                    return "DetailNot";
            }
        }

        /// <summary>
        /// 受信系エラー明細データ取得
        /// </summary>
        /// <param name="noticeHeader">検索結果情報</param>
        /// <returns></returns>
        public ActionResult DetailReceive(NoticeDetailHeader noticeHeader)
        {
            //エラー内容の設定
            SetHeaderContent(noticeHeader);

            // 詳細データ検索
            var vm = new NoticeViewModel
            {
                DetailHeader = noticeHeader,
                ReceiveDetails = new NoticeResultReceive()
                {
                    NoticeReceiveRows = _NoticeQuery.ReceiveListing(noticeHeader)
                }
            };
            //ファイル名のみ表示
            foreach (var receiveDetailErrorRow in vm.ReceiveDetails.NoticeReceiveRows)
            {
                var splitter = new char[] { '/' };
                var filename = receiveDetailErrorRow.IfFileName.Split(splitter, StringSplitOptions.None);
                receiveDetailErrorRow.IfFileName = filename[filename.Length - 1];
            }

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.ReceiveDetails.NoticeReceiveRows.Count > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.ReceiveDetails.NoticeReceiveRows = null;
                }
            }

            return this.PartialView("~/Views/Notice/DetailReceive.cshtml", vm);
        }

        /// <summary>
        /// お知らせ明細データ取得
        /// </summary>
        /// <param name="noticeHeader"></param>
        /// <returns></returns>
        public ActionResult Detail(NoticeDetailHeader noticeHeader)
        {
            //エラー内容の設定
            SetHeaderContent(noticeHeader);

            // 作成処理&検索表示
            var vm = new NoticeViewModel();
            vm.DetailHeader = noticeHeader;
            vm.Details = new NoticeResultDetail();
            vm.Details.NoticeDetailRows = _NoticeQuery.NoticeListing(noticeHeader.NoticeHeaderId);

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Details.NoticeDetailRows.Count > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Details.NoticeDetailRows = null;
                }
            }

            return this.PartialView("~/Views/Notice/Detail.cshtml", vm);
        }

        /// <summary>
        /// 連携実行エラー詳細
        /// </summary>
        /// <param name="noticeHeader"></param>
        /// <returns></returns>
        public ActionResult DetailIfRun(NoticeDetailHeader noticeHeader)
        {
            //エラー内容の設定
            SetHeaderContent(noticeHeader);
            //連携実行状態区分名称取得
            noticeHeader.IfRunStateNm = _NoticeQuery.GetIfRunStateNm(noticeHeader.IfRunState);

            // 作成処理&検索表示
            var vm = new NoticeViewModel
            {
                DetailHeader = noticeHeader
            };

            return this.PartialView("~/Views/Notice/DetailIfRun.cshtml", vm);
        }

        /// <summary>
        /// ヘッダのみ表示
        /// </summary>
        /// <param name="noticeHeader"></param>
        /// <returns></returns>
        public ActionResult DetailNot(NoticeDetailHeader noticeHeader)
        {
            //エラー内容の設定
            SetHeaderContent(noticeHeader);

            // 作成処理&検索表示
            var vm = new NoticeViewModel
            {
                DetailHeader = noticeHeader
            };

            return this.PartialView("~/Views/Notice/DetailNot.cshtml", vm);
        }

        /// <summary>
        /// お知らせ内容の設定
        /// </summary>
        /// <param name="result"></param>
        private void SetHeaderContent(NoticeDetailHeader noticeHeader)
        {
            var splitter = new char[] { '@' };
            var message = string.Format(noticeHeader.Message
                            , (noticeHeader.MessageParameter + "").Split(splitter, StringSplitOptions.None));
            noticeHeader.Content = message;
        }
        #endregion

        #region PopUp
        /// <summary>
        /// お知らせポップアップデータ取得
        /// </summary>
        /// <returns>お知らせポップアップ部分ビュー</returns>
        [HttpGet]
        public ActionResult GetPopUpList()
        {
            if (!Request.IsAjaxRequest()) return new EmptyResult();

            var vm = new NoticeViewModel
            {
                PopUpResults = new NoticePopUpResult()
                {
                    NoticePopUpRows = _NoticeQuery.GetPopUpList()
                },
            };
            return PartialView("_NoticePopUp", vm);
        }
        #endregion

    }
}