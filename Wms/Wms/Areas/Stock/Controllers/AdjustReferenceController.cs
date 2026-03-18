using PagedList;
using Share.Common;
using Share.Extensions.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Wms.Areas.Stock.Query.AdjustReference;
using Wms.Areas.Stock.Query.Reference;
using Wms.Areas.Stock.Reports.Export;
using Wms.Areas.Stock.Resources;
using Wms.Areas.Stock.ViewModels.Adjust;
using Wms.Areas.Stock.ViewModels.AdjustReference;
using Wms.Controllers;

namespace Wms.Areas.Stock.Controllers
{
    public class AdjustReferenceController : BaseController
    {
        private const string CookieName = "W-STK_AdjustReferenceReference.SearchConditions";

        private readonly ReferenceQuery _referenceQuery;

        public AdjustReferenceController()
        {
            _referenceQuery = new ReferenceQuery();
        }

        [HttpGet]
        public ActionResult Index()
        {
            CookieExtention.SetSearchConditonCookie(CookieName, new AdjustReferenceSearchConditions());
            SetViewBagProperty();
            return View(GetIndexViewName(), CreateNewViewModel());
        }

        [HttpGet]
        public ActionResult ReSearch(int pageNumber)
        {
            var condition = GetPrevCondition(new AdjustReferenceSearchConditions());

            condition.PageNumber = pageNumber;
            CookieExtention.SetSearchConditonCookie(CookieName, condition);

            return SearchResult(condition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReSearch([Required] AdjustReferenceSearchConditions condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            var newCondition = GetPrevCondition(condition);

            newCondition.SortOrder = condition.SortOrder;
            newCondition.AscDescSort = condition.AscDescSort;
            CookieExtention.SetSearchConditonCookie(CookieName, newCondition);

            return SearchResult(newCondition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search([Required] AdjustReferenceSearchConditions condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            condition.PageNumber = 1;
            CookieExtention.SetSearchConditonCookie(CookieName, condition);

            return SearchResult(condition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DownloadExcel()
        {
            var condition = Request.Cookies.Get<AdjustReferenceSearchConditions>(CookieName);
            var report = new AdjustReferenceReport(condition);

            report.Export();

            return File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Print()
        {
            const string styleName = "AdjustReference.wfr";
            string controllerName = RouteData.Values["controller"].ToString();
            var condition = Request.Cookies.Get<AdjustReferenceSearchConditions>(CookieName);
            var report = new AdjustReferenceReportForCsv(condition);
            var csv = new CsvPrintFileCreate();

            report.Export();
            csv.CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            return WfrPrint(styleName,report.DownloadFileName);
        }

        private ActionResult SearchResult(AdjustReferenceSearchConditions condition)
        {
            condition.PageSize = GetCurrentPageSize();

            var resultRowList = AdjustReferenceQuery.GetResultRowList(condition);
            var total = AdjustReferenceQuery.CountResultRowList(condition);
            var vm = new AdjustReferenceViewModel
            {
                Condition = condition,
                Result = CreateResult(condition, resultRowList, total),
                IsShowResultList = true,
                CanChangeCenter = AdjustReferenceQuery.CanChangeCenter(),
            };

            SetViewBagProperty(condition);

            return View(GetIndexViewName(), vm);
        }

        private void SetViewBagProperty(AdjustReferenceSearchConditions condition = null)
        {
            if (condition == null)
            {
                condition = new AdjustReferenceSearchConditions();
            }

            ViewBag.AdjustReasonList = AdjustReferenceQuery.GetSelectListStockAdjustReason();
            ViewBag.DivisionList = _referenceQuery.GetSelectListDivisions();
            ViewBag.Category1List = _referenceQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _referenceQuery.GetSelectListCategorys2(condition.CategoryId1);
            ViewBag.Category3List = _referenceQuery.GetSelectListCategorys3(condition.CategoryId1, condition.CategoryId2);
            ViewBag.Category4List = _referenceQuery.GetSelectListCategorys4(condition.CategoryId1, condition.CategoryId2, condition.CategoryId3);
            ViewBag.ItemList = _referenceQuery.GetSelectListItems();
        }

        private AdjustReferenceSearchConditions GetPrevCondition(AdjustReferenceSearchConditions condition)
        {
            return Request.Cookies.Get<AdjustReferenceSearchConditions>(CookieName) ?? condition;
        }

        private static AdjustReferenceViewModel CreateNewViewModel()
        {
            return new AdjustReferenceViewModel
            {
                IsShowResultList = false,
                CanChangeCenter = AdjustReferenceQuery.CanChangeCenter()
            };
        }

        private static string GetIndexViewName()
        {
            return "~/Areas/Stock/Views/AdjustReference/Index.cshtml";
        }

        private static AdjustReferenceResult CreateResult(AdjustReferenceSearchConditions condition, List<AdjustReferenceResultRow> resultRowList, int total)
        {
            return new AdjustReferenceResult
            {
                ResultRowList = new StaticPagedList<AdjustReferenceResultRow>(resultRowList, condition.PageNumber, condition.PageSize, total),
            };
        }
    }
}