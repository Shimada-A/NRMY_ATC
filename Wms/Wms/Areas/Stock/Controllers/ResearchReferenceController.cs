using Share.Extensions.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wms.Areas.Stock.Query.Reference;
using Wms.Areas.Stock.Query.AdjustReference;
using Wms.Areas.Stock.ViewModels.ResearchReference;
using Wms.Controllers;
using System.ComponentModel.DataAnnotations;
using PagedList;
using Wms.Areas.Stock.Query.ResearchReference;
using System.Web.Script.Serialization;
using Wms.Areas.Stock.Reports.Export;
using Share.Common;
using static Wms.Areas.Stock.ViewModels.ResearchReference.ResearchReferenceSearchConditions;
using Wms.Areas.Stock.Resources;
using Wms.Resources;

namespace Wms.Areas.Stock.Controllers
{
    public class ResearchReferenceController : BaseController
    {
        private const string CookieName = "W-W_STK_ResearchReference.SearchConditions";

        private readonly ReferenceQuery _referenceQuery;

        public ResearchReferenceController()
        {
            _referenceQuery = new ReferenceQuery();
        }

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                CookieExtention.SetSearchConditonCookie(CookieName, new ResearchReferenceSearchConditions());
                SetViewBagProperty();
                return View(GetIndexViewName(), CreateNewViewModel());
            }
            catch (Exception ex)
            {
                Mvc.Common.AppError.PutLogREF(ex, "index");
                throw;
            };
        }

        [HttpGet]
        public ActionResult ReSearch(int? pageNumber, string uniqueKey)
        {
            var condition = GetPrevCondition(new ResearchReferenceSearchConditions());

            condition.PageNumber = pageNumber ?? condition.PageNumber;
            condition.SelectedUniqueKey = uniqueKey ?? condition.SelectedUniqueKey;
            CookieExtention.SetSearchConditonCookie(CookieName, condition);

            return SearchResult(condition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReSearch([Required] ResearchReferenceSearchConditions condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            var newCondition = GetPrevCondition(condition);

            newCondition.SelectedUniqueKey = condition.SelectedUniqueKey;
            newCondition.SortOrder = condition.SortOrder;
            newCondition.AscDescSort = condition.AscDescSort;
            CookieExtention.SetSearchConditonCookie(CookieName, newCondition);

            return SearchResult(newCondition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search([Required] ResearchReferenceSearchConditions condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            condition.SelectedUniqueKey = string.Empty;
            condition.PageNumber = 1;
            CookieExtention.SetSearchConditonCookie(CookieName, condition);

            return SearchResult(condition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DownloadExcel()
        {
            var condition = GetPrevCondition(new ResearchReferenceSearchConditions());
            var report = new ResearchReferenceReport(condition);

            report.Export();

            return File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Print()
        {
            const string styleName = "ResearchReference.wfr";
            string controllerName = RouteData.Values["controller"].ToString();
            var condition = GetPrevCondition(new ResearchReferenceSearchConditions());
            var report = new ResearchReferenceReportForCsv(condition);
            var csv = new CsvPrintFileCreate();

            report.Export();
            csv.CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            return WfrPrint(styleName, report.DownloadFileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Input([Required] ResearchReferenceInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            var condition = GetPrevCondition(new ResearchReferenceSearchConditions());
            var vm = new ResearchReferenceInputViewModel
            {
                ResultRow = input.ToObject(),
                ResultCheckD = ResearchReferenceQuery.GetT_STOCK_REDEARCH_CHECK_D(condition, input.ToObject().SlipNo),
            };

            return View(GetInputViewName(), vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Required] ResearchReferenceInput input)
        {
            if (input == null || input.ToObject().RegistClass != ResearchReferenceRegistClass.MoveArrivalResult)
            {
                throw new ArgumentNullException(nameof(input));
            }
            //状況更新処理
            ProcedureStatus ret_status = ProcedureStatus.Success;
            ret_status = ResearchReferenceQuery.UpdateStatus(input.ToObject().CenterId, input.ToObject().SlipNo);
            if (ret_status == ProcedureStatus.Success)
            {
                TempData[AppConst.SUCCESS] = ResearchReferenceResource.MsgSuccess;
                // 検索部を表示
                SetViewBagProperty();
                return View(GetIndexViewName(), CreateNewViewModel());
            }
            else
            {
                ViewBag.Status = ret_status;
                ViewBag.ErrorMessage = MessageResource.ERR_STOCK_RESEARCH_UPDATE;
                var condition = GetPrevCondition(new ResearchReferenceSearchConditions());
                var vm = new ResearchReferenceInputViewModel
                {
                    ResultRow = input.ToObject(),
                    ResultCheckD = ResearchReferenceQuery.GetT_STOCK_REDEARCH_CHECK_D(condition, input.ToObject().SlipNo),
                };
                return View(GetIndexViewName(), vm);

            }
        }

        private ActionResult SearchResult(ResearchReferenceSearchConditions condition)
        {
            condition.PageSize = GetCurrentPageSize();

            var resultRowList = ResearchReferenceQuery.GetResultRowList(condition,false);
            var resultCount = ResearchReferenceQuery.GetResultCount(condition);
            var vm = new ResearchReferenceViewModel
            {
                Condition = condition,
                Result = CreateResult(condition, resultRowList, resultCount),
                IsShowResultList = true,
                CanChangeCenter = ResearchReferenceQuery.CanChangeCenter(),
            };

            SetViewBagProperty();

            return View(GetIndexViewName(), vm);
        }

        private void SetViewBagProperty()
        {
            ViewBag.GradeList = _referenceQuery.GetSelectListGrades();
        }

        private ResearchReferenceSearchConditions GetPrevCondition(ResearchReferenceSearchConditions condition)
        {
            return Request.Cookies.Get<ResearchReferenceSearchConditions>(CookieName) ?? condition;
        }

        private static ResearchReferenceViewModel CreateNewViewModel()
        {
            return new ResearchReferenceViewModel
            {
                IsShowResultList = false,
                CanChangeCenter = ResearchReferenceQuery.CanChangeCenter()
            };
        }

        private static string GetIndexViewName()
        {
            return "~/Areas/Stock/Views/ResearchReference/Index.cshtml";
        }

        private static string GetInputViewName()
        {
            return "~/Areas/Stock/Views/ResearchReference/Input.cshtml";
        }

        private static ResearchReferenceResult CreateResult(
            ResearchReferenceSearchConditions condition,
            List<ResearchReferenceResultRow> resultRowList,
            ResearchReferenceResultCount resultCount)
        {
            return new ResearchReferenceResult
            {
                ResultRowList = new StaticPagedList<ResearchReferenceResultRow>(resultRowList, condition.PageNumber, condition.PageSize, resultCount.Count),
                ResultCount = resultCount,
                SelectedUniqueKey = condition.SelectedUniqueKey,
            };
        }
    }
}