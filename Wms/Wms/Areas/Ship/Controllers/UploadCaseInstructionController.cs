namespace Wms.Areas.Ship.Controllers
{
    using OfficeOpenXml;
    using Share.Common;
    using Share.Extensions.Classes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using Wms.Areas.Ship.Query.UploadCaseInstruction;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.UploadCaseInstruction;
    using Wms.Controllers;
    using Wms.Hubs;
    using Wms.Models;
    using Wms.Resources;
    using Wms.Common;

    public class UploadCaseInstructionController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_STK_UploadCaseInstruction.SearchConditions";

        private UploadCaseInstructionQuery _UploadCaseInstructionQuery;
        private Report _report = new Report();
        /// <summary>
        /// Initializes a new instance of the <see cref="UploadCaseInstructionController"/> class.
        /// </summary>
        public UploadCaseInstructionController()
        {
            this._UploadCaseInstructionQuery = new UploadCaseInstructionQuery();
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
            searchInfo.CenterId = Common.Profile.User.CenterId;
            var vm = new UploadCaseInstructionViewModel
            {
                SearchConditions = searchInfo,
                Results = new UploadCaseInstructionResult()
            };

            ViewBag.SaleClassList = _UploadCaseInstructionQuery.GetSelectListSaleClasses();

            return this.View("~/Areas/Ship/Views/UploadCaseInstruction/Index.cshtml", vm);
        }

        private ActionResult IndexSearch(bool initial)
        {
            var searchInfo = this.GetPreviousSearchInfo(initial);

            var vm = new UploadCaseInstructionViewModel
            {
                SearchConditions = searchInfo,
                Results = new UploadCaseInstructionResult()
            };

            ViewBag.SaleClassList = _UploadCaseInstructionQuery.GetSelectListSaleClasses();

            return this.View("~/Areas/Ship/Views/UploadCaseInstruction/Index.cshtml", vm);
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="searchCondition"List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Search(UploadCaseInstructionSearchConditions SearchConditions)
        {
            bool flg = false;
            UploadCaseInstructionSearchConditions condition;
            if (SearchConditions.CenterId == null)
            {
                //ページング
                condition = this.GetPreviousSearchInfo(false);
                condition.Page = SearchConditions.Page;
            }
            else
            {
                flg = true;
                condition = SearchConditions;
                condition.HidKey = SearchConditions.Key;
                condition.HidSort = SearchConditions.Sort;
                condition.PageSize = this.GetCurrentPageSize();
            }

            return this.GetSearchResultView(condition, "", flg);
        }

        #endregion Search

        /// <summary>
        /// 引当解除・削除処理
        /// </summary>
        /// <param name="searchCondition"List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Delete(UploadCaseInstructionSearchConditions SearchConditions)
        {
            var retmessage = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            _UploadCaseInstructionQuery.DeleteCaseInstructionIns(SearchConditions, out status, out retmessage);

            if (status == ProcedureStatus.Success)
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = string.Format(UploadCaseInstructionResource.DeleteSuc + SearchConditions.HidBatchNo + ")");
            }
            else
            {
                TempData[AppConst.ERROR] = retmessage;
            }

            var vm = new UploadCaseInstructionViewModel
            {
                SearchConditions = SearchConditions,
                Results = new UploadCaseInstructionResult()
            };
            vm.SearchConditions.CenterId = SearchConditions.HidCenterId;

            ViewBag.SaleClassList = _UploadCaseInstructionQuery.GetSelectListSaleClasses();

            return this.View("~/Areas/Ship/Views/UploadCaseInstruction/Index.cshtml", vm);
        }

        #region ロード処理
        /// <summary>
        /// Indexレポートダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public ActionResult Download(UploadCaseInstructionSearchConditions SearchConditions)
        {
            //ケース出荷
            if (SearchConditions.HidShipKind == UploadCaseInstructionSearchConditions.ShipKinds.KindCase)
            {
                Reports.Export.UploadCaseInstructionReportCase report = new Reports.Export.UploadCaseInstructionReportCase(ReportTypes.Excel, SearchConditions);
                report.Export();
                return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
            }
            else
            {
                //JAN抜取
                Reports.Export.UploadCaseInstructionReportJan report = new Reports.Export.UploadCaseInstructionReportJan(ReportTypes.Excel, SearchConditions);
                report.Export();
                return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
            }
        }

        /// <summary>
        /// 直近取込エラーデータダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public ActionResult DownloadErr(UploadCaseInstructionSearchConditions SearchConditions)
        {
           int intShipKind = _report.GetShipKind(SearchConditions);
            //ケース出荷
            if (intShipKind == (int)UploadCaseInstructionSearchConditions.ShipKinds.KindCase)
            {
                Reports.Export.UploadCaseErrReportCase report = new Reports.Export.UploadCaseErrReportCase(ReportTypes.Excel, SearchConditions);
                report.Export();
                return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
            }
            else
            {
                //JAN抜取
                Reports.Export.UploadCaseErrReportJan report = new Reports.Export.UploadCaseErrReportJan(ReportTypes.Excel, SearchConditions);
                report.Export();
                return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
            }
        }

        /// <summary>
        /// Upload処理
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Upload(UploadCaseInstructionSearchConditions SearchConditions)
        {
            var guid = Guid.NewGuid().ToString();
            var uploadFile = Request.Files[0];
            var workId = new long();
            var message = string.Empty;
            var msg = string.Empty;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);

            //エクセルデータチェック
            if (SearchConditions.ShipKind == UploadCaseInstructionSearchConditions.ShipKinds.KindCase)
            {
                //ケース出荷
                msg = CheckExcelDataCase(uploadFile);
            }
            else
            {
                msg = CheckExcelDataJan(uploadFile);
            }
            //エラーの場合return
            if (msg.Length != 0)
            {
                TempData[AppConst.ERROR] = msg;
                return this.Json(new { err = "true", title = SearchConditions.CaseShipName,kindCase = SearchConditions.HidShipKind });
            }

            // ファイルの取込
            if (SearchConditions.ShipKind == UploadCaseInstructionSearchConditions.ShipKinds.KindCase)
            {
                //ケース出荷
                var report = new Reports.Import.UploadCaseInstructionReportCase(ReportTypes.Excel, uploadFile, guid, SearchConditions.CaseShipName, SearchConditions.CenterId);
                // Excelのデータをワークテーブルに登録（このタイミングでワークIDを採番する）
                report.Import();
                workId = report._seq;
                message = report._message;
                SearchConditions.Seq = workId;
                SearchConditions.FileName = uploadFile.FileName;
            }
            else
            {
                //JAN抜き取り
                var report = new Reports.Import.UploadCaseInstructionReportJan(ReportTypes.Excel, uploadFile, guid, SearchConditions.CaseShipName, SearchConditions.CenterId);
                // Excelのデータをワークテーブルに登録（このタイミングでワークIDを採番する）
                report.Import();
                workId = report._seq;
                message = report._message;
                SearchConditions.Seq = workId;
                SearchConditions.FileName = uploadFile.FileName;
            }

            // 登録
            if (string.IsNullOrWhiteSpace(message))
            {
                // Return index view
                return this.Json(new { err = "false",seq = workId });
            }
            else
            {
                // Clear message to back to index screen
                TempData[AppConst.ERROR] = message;
                // Return index view
                return this.Json(new { err = "true" , title = SearchConditions.CaseShipName, kindCase = SearchConditions.HidShipKindName });
            }
        }

        /// <summary>
        /// 引当処理
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public ActionResult Alloc(UploadCaseInstructionSearchConditions SearchConditions)
        {

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);

            // 実績更新
            var retmessage = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            var locCnt = MvcDbContext.Current.ShpCaseInstructions.Where(x => x.Seq == SearchConditions.Seq).Count();
            if (locCnt == 0)
            {
                TempData[AppConst.ERROR] = UploadCaseInstructionResource.ErrNoData;
                // Return index view
                return this.Json(new { err = "true" });
            }
            else
            {
                   //引当ストアド実行
                 _UploadCaseInstructionQuery.InsertUploadCaseInstructionIns(SearchConditions, out status, out retmessage);

                // 非同期処理開始
                int i = 0;
                long wkId = SearchConditions.Seq;
                SearchConditions.IndicateTitle = MessageResource.ALLOC_DOING;
                for (; ; )
                {
                    Functions.SendProgress(SearchConditions.IndicateTitle, i, 100, SearchConditions.ProcessColor);
                    Thread.Sleep(100);

                    if (i == 100)
                    {
                        Thread.Sleep(500);
                        break;
                    }

                    if (_UploadCaseInstructionQuery.GetAllocStatus(wkId).Status == 1)
                    {
                        i = 100;
                        SearchConditions.IndicateTitle = MessageResource.NORMAL_END;

                    }
                    else if (_UploadCaseInstructionQuery.GetAllocStatus(wkId).Status == 9)
                    {
                        i = 100;
                        // 異常の状態を設定する
                        SearchConditions.IndicateTitle = MessageResource.ERROR_END;
                        SearchConditions.ProcessColor = "red";

                    }
                    else
                    {
                        i = _UploadCaseInstructionQuery.GetAllocStatus(wkId).Progress > i ? i + 1 : _UploadCaseInstructionQuery.GetAllocStatus(wkId).Progress;
                    }
                }

                //////  string message = _DcAllocationQuery.GetAllocStatus(wkId).Message;
                ProcedureStatus status2 = (ProcedureStatus)_UploadCaseInstructionQuery.GetAllocStatus(wkId).Status2;
                if (status2 == ProcedureStatus.Success || status2 == ProcedureStatus.NoAllocData)
                {
                    // Clear message to back to index screen
                    TempData[AppConst.SUCCESS] = retmessage;
                    return IndexSearch(true);
                }
                else
                {
                    ViewBag.Status = status;
                    ViewBag.ErrorMessage = retmessage;
                    // 検索
                    return IndexSearch(false);
                }
            }
        }

        /// <summary>
        /// 値が入っている最終行を取得
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="endColumn"></param>
        /// <returns></returns>
        private int LastRow(ExcelWorksheet workSheet, int endColumn)
        {
            var endRow = workSheet.Dimension.End.Row;

            for (; endRow > 1; endRow--)
            {
                var exists = false;
                for (int i = 1; i <= endColumn; i++)
                {
                    if (!string.IsNullOrEmpty(workSheet.Cells[endRow, i].Text))
                    {
                        exists = true;
                        break;
                    }
                }

                if (exists) break;
            }

            return endRow;
        }

        /// <summary>
        /// エクセルデータチェック(ケース出荷)
        /// </summary>
        /// <returns></returns>
        private string CheckExcelDataCase(HttpPostedFileBase uploadFile)
        {
            var msg = string.Empty;
            //エクセルファイルチェック
            using (var excel = new ExcelPackage(uploadFile.InputStream))
            {
                var workSheet = excel.Workbook.Worksheets[1];
                var endColumn = workSheet.Dimension.End.Column;
                if (endColumn != 4)
                {
                    return UploadCaseInstructionResource.ERR_IMP_FILE_TYPE;
                }
                var endRow = LastRow(workSheet, endColumn);
                var data = new List<ViewModels.UploadCaseInstruction.UploadCaseInstructionOutputReport>();
                //１行もデータが入ってない場合
                if(endRow == 1)
                {
                    return UploadCaseInstructionResource.ERR_IMP_DATA;
                }
                for (var row = 2; row <= endRow; row++)
                {
                    var ShipPlanDateLen = workSheet.Cells[row, 1].Text.Length;
                    var BoxNoLen = workSheet.Cells[row, 2].Text.Length;
                    var ShipToStoreIdLen = workSheet.Cells[row, 3].Text.Length;
                    var PrioritOrderLen = workSheet.Cells[row, 4].Text.Length;

                    //出荷予定日チェック
                    if (ShipPlanDateLen == 0)
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_NULL_SHIP_PLAN_DATE, string.Format("{0:N0}", row - 1));
                        return msg;
                    }
                    else if (ShipPlanDateLen > 10)
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_OVER_SHIP_PLAN_DATE, string.Format("{0:N0}", row - 1));
                        return msg;
                    }

                    DateTime dt;
                    if (!DateTime.TryParse(workSheet.Cells[row, 1].Text, out dt))
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_NOT_DATE_SHIP_PLAN_DATE, string.Format("{0:N0}", row - 1));
                        return msg;
                    }

                    //ケースNoチェック
                    if (BoxNoLen == 0)
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_NULL_BOX_NO, string.Format("{0:N0}", row - 1));
                        return msg;
                    }
                    else if (BoxNoLen > 36)
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_OVER_BOX_NO, string.Format("{0:N0}", row - 1));
                        return msg;
                    }

                    //店舗IDチェック
                    if (ShipToStoreIdLen == 0)
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_NULL_SHIP_TO_STORE_ID, string.Format("{0:N0}", row - 1));
                        return msg;
                    }
                    else if (ShipToStoreIdLen > 20)
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_OVER_SHIP_TO_STORE_ID, string.Format("{0:N0}", row - 1));
                        return msg;
                    }

                    //優先度チェック
                    int intRet;
                    if (PrioritOrderLen > 9)
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_OVER_PRIORIT_ORDER, string.Format("{0:N0}", row - 1));
                        return msg;
                    }
                    if (PrioritOrderLen == 0)
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_NULL_PRIORIT_ORDER, string.Format("{0:N0}", row - 1));
                        return msg;
                    }

                    if (int.TryParse(workSheet.Cells[row, 4].Text, out intRet))
                    {
                        if (intRet < 0)
                        {
                            msg = string.Format(UploadCaseInstructionResource.ERR_NOT_PRIORIT_ORDER, string.Format("{0:N0}", row - 1));
                            return msg;
                        }
                    }
                    else
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_NOT_PRIORIT_ORDER, string.Format("{0:N0}", row - 1));
                        return msg;
                    }
                }
            }
            return msg;
        }


        /// <summary>
        /// エクセルデータチェック(JAN抜取)
        /// </summary>
        /// <returns></returns>
        private string CheckExcelDataJan(HttpPostedFileBase uploadFile)
        {
            var msg = string.Empty;
            //エクセルファイルチェック
            using (var excel = new ExcelPackage(uploadFile.InputStream))
            {
                var workSheet = excel.Workbook.Worksheets[1];
                var endColumn = workSheet.Dimension.End.Column;
                if(endColumn != 5)
                {
                    return UploadCaseInstructionResource.ERR_IMP_FILE_TYPE;
                }

                var endRow = LastRow(workSheet, endColumn);
                var data = new List<ViewModels.UploadCaseInstruction.UploadCaseInstructionOutputReport>();
                //１行もデータが入ってない場合
                if (endRow == 1)
                {
                    return UploadCaseInstructionResource.ERR_IMP_DATA;
                }
                for (var row = 2; row <= endRow; row++)
                {
                    var ShipPlanDateLen = workSheet.Cells[row, 1].Text.Length;
                    var ShipToStoreIdLen = workSheet.Cells[row, 2].Text.Length;
                    var PrioritOrderLen = workSheet.Cells[row, 3].Text.Length;
                    var JanLen = workSheet.Cells[row, 4].Text.Length;
                    var InstructQtyLen = workSheet.Cells[row, 5].Text.Length;

                    //出荷予定日チェック
                    if (ShipPlanDateLen == 0)
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_NULL_SHIP_PLAN_DATE, string.Format("{0:N0}", row - 1));
                        return msg;
                    }
                    else if (ShipPlanDateLen > 10)
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_OVER_SHIP_PLAN_DATE, string.Format("{0:N0}", row - 1));
                        return msg;
                    }

                    DateTime dt;
                    if (!DateTime.TryParse(workSheet.Cells[row, 1].Text, out dt))
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_NOT_DATE_SHIP_PLAN_DATE, string.Format("{0:N0}", row - 1));
                        return msg;
                    }

                    //店舗IDチェック
                    if (ShipToStoreIdLen == 0)
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_NULL_SHIP_TO_STORE_ID, string.Format("{0:N0}", row - 1));
                        return msg;
                    }
                    else if (ShipToStoreIdLen > 20)
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_OVER_SHIP_TO_STORE_ID, string.Format("{0:N0}", row - 1));
                        return msg;
                    }

                    //優先度チェック
                    int intRet;
                    if (PrioritOrderLen > 9)
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_OVER_PRIORIT_ORDER, string.Format("{0:N0}", row - 1));
                        return msg;
                    }
                    if (PrioritOrderLen == 0)
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_NULL_PRIORIT_ORDER, string.Format("{0:N0}", row - 1));
                        return msg;
                    }

                    if (int.TryParse(workSheet.Cells[row, 3].Text, out intRet))
                    {
                        if (intRet < 0)
                        {
                            msg = string.Format(UploadCaseInstructionResource.ERR_NOT_PRIORIT_ORDER, string.Format("{0:N0}", row - 1));
                            return msg;
                        }
                    }
                    else
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_NOT_PRIORIT_ORDER, string.Format("{0:N0}", row - 1));
                        return msg;
                    }

                    //抜き取りJANチェック
                    if (JanLen == 0)
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_NULL_JAN, string.Format("{0:N0}", row - 1));
                        return msg;
                    }
                    else if (JanLen > 13)
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_OVER_JAN, string.Format("{0:N0}", row - 1));
                        return msg;
                    }


                    //数量チェック
                    intRet = 0;
                    if (InstructQtyLen > 9)
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_OVER_INSTRUCT_QTY, string.Format("{0:N0}", row - 1));
                        return msg;
                    }
                    if (InstructQtyLen == 0)
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_NULL_STOCK_QTY, string.Format("{0:N0}", row - 1));
                        return msg;
                    }
                    if (int.TryParse(workSheet.Cells[row, 5].Text, out intRet))
                    {
                        if (intRet < 0)
                        {
                            msg = string.Format(UploadCaseInstructionResource.ERR_NOT_STOCK_QTY, string.Format("{0:N0}", row - 1));
                            return msg;
                        }
                    }
                    else
                    {
                        msg = string.Format(UploadCaseInstructionResource.ERR_NOT_STOCK_QTY, string.Format("{0:N0}", row - 1));
                        return msg;
                    }

                }
            }
            return msg;
        }
        #endregion ロード処理

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private UploadCaseInstructionSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new UploadCaseInstructionSearchConditions() : Request.Cookies.Get<UploadCaseInstructionSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new UploadCaseInstructionSearchConditions();
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
        private ActionResult GetSearchResultView(UploadCaseInstructionSearchConditions condition, string pMsg, bool insertFlg)
        {
            //ワークデータ登録/取得
            if (insertFlg && condition.SearchType == Common.SearchTypes.Search)
            {
                _UploadCaseInstructionQuery.InsertCaseWk(condition);
            }
            if (condition.Seq == 0 && condition.CaseViewModels != null)
            {
                condition.Seq = condition.CaseViewModels.FirstOrDefault().Seq;
            }
            var vm = new UploadCaseInstructionViewModel
            {
                SearchConditions = condition,
                Results = new UploadCaseInstructionResult()
                {
                    UploadCaseInstructions = _UploadCaseInstructionQuery.GetCaseWkData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.UploadCaseInstructions.TotalItemCount > ProcNumLimit)
                {
                    if (pMsg.Length == 0)
                    {
                        pMsg = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    }
                    vm.Results.UploadCaseInstructions = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);
            TempData[AppConst.ERROR] = pMsg;

            ViewBag.SaleClassList = _UploadCaseInstructionQuery.GetSelectListSaleClasses();

            // Return index view
            return this.View("~/Areas/Ship/Views/UploadCaseInstruction/Index.cshtml", vm);
        }

        /// 詳細画面検索結果ビューを取得する
        public ActionResult Detail(UploadCaseInstructionSearchConditions SearchConditions)
        {
            var vm = new UploadCaseInstructionViewModel();
            SearchConditions.HidPage = 1;
            SearchConditions.HidPageSize = this.GetCurrentPageSize();
            if (SearchConditions.HidShipKindName == UploadCaseInstructionResource.KindCase)
            {
                //ケース出荷
                vm = new UploadCaseInstructionViewModel
                {
                    SearchConditions = SearchConditions,
                    Results = new UploadCaseInstructionResult()
                    {
                        CaseDetails = _UploadCaseInstructionQuery.GetCaseDetailData(SearchConditions)
                    }
                };
            }
            else
            {
                //JAN抜取
                vm = new UploadCaseInstructionViewModel
                {
                    SearchConditions = SearchConditions,
                    Results = new UploadCaseInstructionResult()
                    {
                        JanDetails = _UploadCaseInstructionQuery.GetJanDetailData(SearchConditions)
                    }
                };
            }
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);
            return this.View("~/Areas/Ship/Views/UploadCaseInstruction/Detail.cshtml", vm);
        }

        /// <summary>
        /// 詳細画面検索処理
        /// </summary>
        /// <param name="searchCondition"List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult SearchDetail(UploadCaseInstructionSearchConditions SearchConditions)
        {
            UploadCaseInstructionSearchConditions condition;
            //ページング
            SearchConditions.HidPageSize = this.GetCurrentPageSize();
            condition = SearchConditions;

            var vm = new UploadCaseInstructionViewModel();
            if (condition.HidShipKindName == UploadCaseInstructionResource.KindCase)
            {
                //ケース出荷
                vm = new UploadCaseInstructionViewModel
                {
                    SearchConditions = condition,
                    Results = new UploadCaseInstructionResult()
                    {
                        CaseDetails = _UploadCaseInstructionQuery.GetCaseDetailData(condition)
                    }
                };
            }
            else
            {
                //JAN抜取
                vm = new UploadCaseInstructionViewModel
                {
                    SearchConditions = condition,
                    Results = new UploadCaseInstructionResult()
                    {
                        JanDetails = _UploadCaseInstructionQuery.GetJanDetailData(condition)
                    }
                };
            }
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);
            return this.View("~/Areas/Ship/Views/UploadCaseInstruction/Detail.cshtml", vm);
        }

        /// <summary>
        /// 戻る処理
        /// </summary>
        /// <param name="searchCondition"List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Return(UploadCaseInstructionSearchConditions SearchConditions)
        {
            bool flg = false;
            UploadCaseInstructionSearchConditions condition = SearchConditions;
            //ページング
            condition.CenterId = SearchConditions.HidCenterId;
            condition.ShipKind = SearchConditions.HidShipKind;
            condition.Page = SearchConditions.HidIndexPage;
            condition.Sort = SearchConditions.HidSort;
            condition.Key = SearchConditions.HidKey;
            return this.GetSearchResultView(condition, "", flg);
        }
        #endregion Private

    }
}