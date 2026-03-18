namespace Wms.Areas.Stock.Controllers
{
    using OfficeOpenXml;
    using Share.Common;
    using Share.Extensions.Classes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Wms.Areas.Stock.Query.ImportInstruction;
    using Wms.Areas.Stock.Resources;
    using Wms.Areas.Stock.ViewModels.ImportInstruction;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;

    public class ImportInstructionController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_STK_ImportInstruction.SearchConditions";

        private ImportInstructionQuery _ImportInstructionQuery;
        private Report _report = new Report();
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportInstructionController"/> class.
        /// </summary>
        public ImportInstructionController()
        {
            this._ImportInstructionQuery = new ImportInstructionQuery();
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
            var vm = new ImportInstructionViewModel
            {
                SearchConditions = searchInfo,
                Results = new ImportInstructionResult()
            };

            return this.View("~/Areas/Stock/Views/ImportInstruction/Index.cshtml", vm);
        }
        public ActionResult IndexSearch()
        {
            var searchInfo = this.GetPreviousSearchInfo(false);
            var msg = string.Empty;
            if (TempData[AppConst.ERROR] != null)
            {
                msg = TempData[AppConst.ERROR].ToString();
            }
            return this.GetSearchResultView(searchInfo, msg, true);
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="searchCondition"List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Search(ImportInstructionSearchConditions SearchConditions)
        {
            ModelState.Clear();
            bool flg = false;
            ImportInstructionSearchConditions condition;
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
                condition.PageSize = this.GetCurrentPageSize();
            }

            return this.GetSearchResultView(condition, "", flg);
        }

        #endregion Search

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <param name="searchCondition"List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Delete(ImportInstructionSearchConditions SearchConditions)
        {
            var retMsg = _ImportInstructionQuery.DeleteWkData(SearchConditions.InstructionViewModels, SearchConditions.CenterId);

            ImportInstructionSearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            condition.Page = 1;
            if(retMsg.Length == 0)
            {
                TempData[AppConst.SUCCESS] = ImportInstructionResource.SUCCESS_DELETE;
            }
            
            return this.GetSearchResultView(condition, retMsg, true);
        }

        #region ロード処理
        /// <summary>
        /// Indexレポートダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public ActionResult Download(ImportInstructionSearchConditions SearchConditions)
        {
            Reports.Export.ImportInstructionReport report = new Reports.Export.ImportInstructionReport(ReportTypes.Excel, SearchConditions);
            report.Export();
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        /// <summary>
        /// Upload処理
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Upload(ImportInstructionSearchConditions SearchConditions)
        {
            var guid = Guid.NewGuid().ToString();
            var uploadFile = Request.Files[0];

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);

            //エクセルデータチェック
            var msg = CheckExcelData(uploadFile);

            //エラーの場合return
            if (msg.Length != 0)
            {
                TempData[AppConst.ERROR] = msg;
                return this.Json(new { err = "true" });
            }

            // ファイルの取込
            var report = new Reports.Import.ImportInstructionReport(ReportTypes.Excel, uploadFile, guid, SearchConditions.SortInstructName, SearchConditions.CenterId);

            // Excelのデータをワークテーブルに登録（このタイミングでワークIDを採番する）
            report.Import();
            var workId = report._seq;
            var message = report._message;
            SearchConditions.Seq = workId;
            SearchConditions.FileName = uploadFile.FileName;

            //ワークデータチェック
            msg = _report.CheckWkData(workId, SearchConditions.CenterId);

            //エラーの場合return
            if (msg.Length != 0)
            {
                TempData[AppConst.ERROR] = msg;
                return this.Json(new { err = "true" });
            }

            // 登録
            if (string.IsNullOrWhiteSpace(message))
            {
                // 実績更新
                var retmessage = string.Empty;
                ProcedureStatus status = ProcedureStatus.Success;
                var locCnt = MvcDbContext.Current.SortStockInstructs02s.Where(x => x.Seq == workId).Count();
                if (locCnt == 0)
                {
                    TempData[AppConst.ERROR] = ImportInstructionResource.ErrNoData;
                    // Return index view
                    return this.Json(new { err = "true" });
                }
                else
                {
                    _ImportInstructionQuery.InsertSortStockIns(SearchConditions, out status, out retmessage);
                    if (status == ProcedureStatus.Success)
                    {
                        // Clear message to back to index screen
                        TempData[AppConst.SUCCESS] = string.Format(ImportInstructionResource.AllUploadSuc, string.Format("{0:N0}", locCnt));
                        // Return index view
                        return this.Json(new { err = "false" });
                    }
                    else
                    {
                        TempData[AppConst.ERROR] = retmessage;
                        // Return index view
                        return this.Json(new { err = "true" });
                    }
                }
            }
            else
            {
                // Clear message to back to index screen
                TempData[AppConst.ERROR] = message;
                // Return index view
                return this.Json(new { err = "true" });
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
        /// エクセルデータチェック
        /// </summary>
        /// <returns></returns>
        private string CheckExcelData(HttpPostedFileBase uploadFile)
        {
            var msg = string.Empty;
            //エクセルファイルチェック
            using (var excel = new ExcelPackage(uploadFile.InputStream))
            {
                var workSheet = excel.Workbook.Worksheets[1];
                var endColumn = workSheet.Dimension.End.Column;
                var endRow = LastRow(workSheet, endColumn);
                var data = new List<ViewModels.ImportInstruction.ImportInstructionOutputReport>();

                for (var row = 2; row <= endRow; row++)
                {
                    var SortClassLen = workSheet.Cells[row, 1].Text.Length;
                    var JanLen = workSheet.Cells[row, 2].Text.Length;
                    var SortCategoryNameLen = workSheet.Cells[row, 3].Text.Length;
                    var TransferNoLen = workSheet.Cells[row, 4].Text.Length;
                    var StockQtyLen = workSheet.Cells[row, 5].Text.Length;
                    var NoteLen = workSheet.Cells[row, 6].Text.Length;

                    //仕分方法区分チェック
                    if (SortClassLen == 0)
                    {
                        msg = string.Format(ImportInstructionResource.ERR_NULL_SORT_CLASS, string.Format("{0:N0}", row - 1));
                        return msg;
                    }
                    else if (SortClassLen > 1)
                    {
                        msg = string.Format(ImportInstructionResource.ERR_OVER_SORT_CLASS, string.Format("{0:N0}", row - 1));
                        return msg;
                    }

                    //JANチェック
                    if (JanLen == 0)
                    {
                        msg = string.Format(ImportInstructionResource.ERR_NULL_JAN, string.Format("{0:N0}", row - 1));
                        return msg;
                    }
                    else if (JanLen > 13)
                    {
                        msg = string.Format(ImportInstructionResource.ERR_OVER_JAN, string.Format("{0:N0}", row - 1));
                        return msg;
                    }

                    //カテゴリー名チェック(必須チェックは仕分方法区分：2カテゴリーのときのみ)
                    if (workSheet.Cells[row, 1].Text == "2" && SortCategoryNameLen == 0)
                    {
                        msg = string.Format(ImportInstructionResource.ERR_NULL_CATEGORY_NAME, string.Format("{0:N0}", row - 1));
                        return msg;
                    }
                    else if (SortCategoryNameLen > 100)
                    {
                        msg = string.Format(ImportInstructionResource.ERR_OVER_CATEGORY_NAME, string.Format("{0:N0}", row - 1));
                        return msg;
                    }

                    //仕分区分チェック
                    if (workSheet.Cells[row, 1].Text != "1" && workSheet.Cells[row, 1].Text != "2")
                    {
                        msg = string.Format(ImportInstructionResource.ERR_NOT_SORTCLASS, string.Format("{0:N0}", row - 1));
                        return msg;
                    }

                    //振替Noチェック
                    if (TransferNoLen == 0)
                    {
                        msg = string.Format(ImportInstructionResource.ERR_NULL_TRANSFER_NO, string.Format("{0:N0}", row - 1));
                        return msg;
                    }
                    else if (TransferNoLen > 20)
                    {
                        msg = string.Format(ImportInstructionResource.ERR_OVER_TRANSFER_NO, string.Format("{0:N0}", row - 1));
                        return msg;
                    }

                    //仕分予定数チェック
                    int intRet;
                    if (StockQtyLen > 9)
                    {
                        msg = string.Format(ImportInstructionResource.ERR_OVER_STOCK_QTY, string.Format("{0:N0}", row - 1));
                        return msg;
                    }
                    if (int.TryParse(workSheet.Cells[row, 5].Text, out intRet))
                    {
                        if (intRet < 0)
                        {
                            msg = string.Format(ImportInstructionResource.ERR_OVER_STOCK_QTY, string.Format("{0:N0}", row - 1));
                            return msg;
                        }
                    }

                    //備考チェック
                    if (NoteLen > 100)
                    {
                        msg = string.Format(ImportInstructionResource.ERR_OVER_NOTE, string.Format("{0:N0}", row - 1));
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
        private ImportInstructionSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new ImportInstructionSearchConditions() : Request.Cookies.Get<ImportInstructionSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new ImportInstructionSearchConditions();
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
        private ActionResult GetSearchResultView(ImportInstructionSearchConditions condition, string pMsg, bool insertFlg)
        {
            // 在庫照会明細（画面選択行更新用）
            if (condition.SearchType == Common.SearchTypes.SortPage)
            {
                _ImportInstructionQuery.UpdateWkData(condition.InstructionViewModels, condition.CenterId);
            }

            //ワークデータ登録/取得
            if (insertFlg && condition.SearchType == Common.SearchTypes.Search)
            {
                _ImportInstructionQuery.InsertSortStockWk(condition);
            }
            if (condition.Seq == 0 && condition.InstructionViewModels != null)
            {
                condition.Seq = condition.InstructionViewModels.FirstOrDefault().Seq;
            }
            var vm = new ImportInstructionViewModel
            {
                SearchConditions = condition,
                Results = new ImportInstructionResult()
                {
                    ImportInstructions = _ImportInstructionQuery.GetSortStockWkData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.ImportInstructions.TotalItemCount > ProcNumLimit)
                {
                    if (pMsg.Length == 0)
                    {
                        pMsg = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    }
                    vm.Results.ImportInstructions = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);
            TempData[AppConst.ERROR] = pMsg;
            // Return index view
            return this.View("~/Areas/Stock/Views/ImportInstruction/Index.cshtml", vm);
        }

        #endregion Private

        #region Selected

        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        public ActionResult AllSelectedSearch(ImportInstructionSearchConditions searchConditions)
        {
            ModelState.Clear();
            // 全選択
            if (searchConditions.InstructionViewModels != null)
            {
                searchConditions.Seq = searchConditions.InstructionViewModels.FirstOrDefault().Seq;
            }
            _ImportInstructionQuery.WkDataAllChange(searchConditions, true);

            searchConditions.PageSize = this.GetCurrentPageSize();
            //ワークデータ取得
            var vm = new ImportInstructionViewModel
            {
                SearchConditions = searchConditions,
                Results = new ImportInstructionResult()
                {
                    ImportInstructions = _ImportInstructionQuery.GetSortStockWkData(searchConditions)
                }
            };

            // Return index view
            return this.View("~/Areas/Stock/Views/ImportInstruction/Index.cshtml", vm);
        }

        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        public ActionResult UnSelectedSearch(ImportInstructionSearchConditions searchConditions)
        {
            ModelState.Clear();
            // 全解除
            if (searchConditions.InstructionViewModels != null)
            {
                searchConditions.Seq = searchConditions.InstructionViewModels.FirstOrDefault().Seq;
            }
            _ImportInstructionQuery.WkDataAllChange(searchConditions, false);
            searchConditions.PageSize = this.GetCurrentPageSize();
            //ワークデータ取得
            var vm = new ImportInstructionViewModel
            {
                SearchConditions = searchConditions,
                Results = new ImportInstructionResult()
                {
                    ImportInstructions = _ImportInstructionQuery.GetSortStockWkData(searchConditions)
                }
            };

            // Return index view
            return this.View("~/Areas/Stock/Views/ImportInstruction/Index.cshtml", vm);
        }

        //カンバン発行
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Print(ImportInstructionSearchConditions SearchConditions)
        {
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);

            //選択状態をワークに反映
            _ImportInstructionQuery.UpdateWkData(SearchConditions.InstructionViewModels, SearchConditions.CenterId);

            var query = new Report();

            //SKU別データワーク登録
            string retMsg = query.InsStockSortKanbanSKU(SearchConditions.InstructionViewModels.First().Seq);
            if (!string.IsNullOrEmpty(retMsg))
            {
                var vm = new ImportInstructionViewModel
                {
                    SearchConditions = SearchConditions
                };
                TempData[AppConst.ERROR] = retMsg;
                return this.View("~/Areas/Stock/Views/ImportInstruction/Index.cshtml", vm);
            }

            //カテゴリー別データワーク登録
            retMsg = query.InsStockSortKanbanCategory(SearchConditions.InstructionViewModels.First().Seq);
            if (!string.IsNullOrEmpty(retMsg))
            {
                var vm = new ImportInstructionViewModel
                {
                    SearchConditions = SearchConditions
                };
                TempData[AppConst.ERROR] = retMsg;
                return this.View("~/Areas/Stock/Views/ImportInstruction/Index.cshtml", vm);
            }

            //印刷処理
            string controllerName = this.RouteData.Values["controller"].ToString();

            var report = new Reports.Export.ImportInstructionReportForCsv(SearchConditions);
            report.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            SearchConditions.Ret = GetWfrPrintUrl("ImportInstruction.wfr", report.DownloadFileName);
            SearchConditions.Print = "Print";
            SearchConditions.SearchType = Common.SearchTypes.Search;

            return Search(SearchConditions);
        }

        #endregion Selected
    }
}