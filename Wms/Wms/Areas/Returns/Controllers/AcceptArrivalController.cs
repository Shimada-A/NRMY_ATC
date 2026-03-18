namespace Wms.Areas.Returns.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Wms.Areas.Returns.Query.AcceptArrival;
    using Wms.Areas.Returns.ViewModels.AcceptArrival;
    using Wms.Controllers;
    using Wms.Areas.Returns.Resources;
    using Share.Common;

    public class AcceptArrivalController : BaseController
    {
        /// <summary>
        /// Search Country
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult Index()
        {
            AcceptArrival01ViewModel vm = new AcceptArrival01ViewModel();
            return this.View("~/Areas/Returns/Views/AcceptArrival/Index.cshtml", vm);
        }

        #region 第2画面より戻るボタンにて第1画面へ遷移
        /// <summary>
        /// 第2画面より戻るボタンにて第1画面へ遷移
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult IndexReturn(AcceptArrival02ViewModel acceptArrival02)
        {
            AcceptArrival01ViewModel vm = new AcceptArrival01ViewModel();
            if (acceptArrival02.SearchConditions != null)
            {
                if (!string.IsNullOrEmpty(acceptArrival02.SearchConditions.JanCombine))
                {
                    this.ModelState.Clear();

                    //JAN以外の検索条件
                    vm.SearchConditions = new AcceptArrival01SearchConditions()
                    {
                        DestZip = acceptArrival02.SearchConditions.DestZip,
                        DestName = acceptArrival02.SearchConditions.DestName,
                        DestAddress = acceptArrival02.SearchConditions.DestAddress,
                        DestTel = acceptArrival02.SearchConditions.DestTel
                    };
                    if (acceptArrival02.SearchConditions.ShpScanClass == AcceptArrival02SearchConditions.ShpScanClasses.ShpScan)
                    {
                        vm.SearchConditions.ShpScanClass = AcceptArrival01SearchConditions.ShpScanClasses.ShpScan;
                    }
                    else
                    {
                        vm.SearchConditions.ShpScanClass = AcceptArrival01SearchConditions.ShpScanClasses.JanScan;
                    }
                    //保持しておいたカンマつながりのJANコードを配列へ格納
                    string[] arrJan = (acceptArrival02.SearchConditions.JanCombine).Split(',');
                    string[] arrScanQty = (acceptArrival02.SearchConditions.ScanQtyCombine).Split(',');

                    //Modelを初期化
                    vm.Results = new ScanResult();
                    vm.Results.AcceptArrival01Result = new List<AcceptArrival01ResultRow>();
                    for (int i = 0; i < arrJan.Length; i++)
                    {
                        var itemData = new AcceptArrivalQuery().GetItemInfo(arrJan[i]);
                        //スキャンしたJANに対するデータが存在する場合
                        if (itemData != null)
                        {
                            for (int r = 0; r < itemData.Count; r++)
                            {
                                var newResultRow = new AcceptArrival01ResultRow();
                                newResultRow.Jan = itemData[r].Jan;
                                newResultRow.ItemId = itemData[r].ItemId;
                                newResultRow.ItemName = itemData[r].ItemName;
                                newResultRow.ItemColorId = itemData[r].ItemColorId;
                                newResultRow.ItemColorName = itemData[r].ItemColorName;
                                newResultRow.ItemSizeId = itemData[r].ItemSizeId;
                                newResultRow.ItemSizeName = itemData[r].ItemSizeName;
                                newResultRow.ScanQty = int.Parse(arrScanQty[i]);
                                vm.Results.AcceptArrival01Result.Add(newResultRow);
                            }
                        }
                    }
                }
            }
            return this.View("~/Areas/Returns/Views/AcceptArrival/Index.cshtml", vm);
        }
        #endregion

        #region スキャン商品明細情報
        /// <summary>
        /// 行変更
        /// </summary>
        /// <param name="acceptArrival01">Route Information</param>
        /// <returns>_AcceptArrival01ResultRow View</returns>
        [HttpPost]
        public ActionResult Search(AcceptArrival01ViewModel acceptArrival01)
        {
            this.ModelState.Clear();

            if (acceptArrival01.ChangeModel == "Read")
            {
                this.ModelState.Clear();
                var chkCnt = 0;
                if(acceptArrival01.Results.AcceptArrival01Result != null)
                {
                    //スキャンしたJANが既にスキャン済みであればカウントアップ
                    for (int i = 0; i < acceptArrival01.Results.AcceptArrival01Result.Count; i++)
                    {
                        if (acceptArrival01.SearchConditions.Jan == acceptArrival01.Results.AcceptArrival01Result[i].Jan)
                        {
                            acceptArrival01.Results.AcceptArrival01Result[i].ScanQty = acceptArrival01.Results.AcceptArrival01Result[i].ScanQty + 1;
                            chkCnt = chkCnt + 1;
                        }
                    }
                }

                //スキャンしたJANが初スキャンだった場合
                if(chkCnt == 0)
                {
                    var newResultRow = new AcceptArrival01ResultRow();
                    var itemData = new AcceptArrivalQuery().GetItemInfo(acceptArrival01.SearchConditions.Jan);

                    List<AcceptArrival01ResultRow> tempLst = new List<AcceptArrival01ResultRow>();
                    if (acceptArrival01.Results.AcceptArrival01Result == null)
                    {
                        acceptArrival01.Results = new ScanResult()
                        {
                            AcceptArrival01Result = tempLst
                        };
                    }
                    //スキャンしたJANに対するデータが存在する場合
                    if (itemData != null)
                    {
                        for (int i = 0; i < itemData.Count; i++)
                        {
                            newResultRow.Jan = itemData[i].Jan;
                            newResultRow.ItemId = itemData[i].ItemId;
                            newResultRow.ItemName = itemData[i].ItemName;
                            newResultRow.ItemColorId = itemData[i].ItemColorId;
                            newResultRow.ItemColorName = itemData[i].ItemColorName;
                            newResultRow.ItemSizeId = itemData[i].ItemSizeId;
                            newResultRow.ItemSizeName = itemData[i].ItemSizeName;
                            acceptArrival01.Results.AcceptArrival01Result.Add(newResultRow);
                        }
                    }
                }

            }
            return this.PartialView("~/Areas/Returns/Views/AcceptArrival/_ResultRows.cshtml", acceptArrival01);
        }
        #endregion

        #region 行変更(削除)
        /// <summary>
        /// 行変更(削除)
        /// </summary>
        /// <param name="acceptArrival01">Route Information</param>
        /// <returns>_AcceptArrival01ResultRow View</returns>
        public ActionResult Delete(AcceptArrival01ViewModel acceptArrival01)
        {
            this.ModelState.Clear();

            if (acceptArrival01.DelRowNo != null)
            {
                this.ModelState.Clear();
                //現在の画面の状態を保持
                var tempLst = acceptArrival01.Results.AcceptArrival01Result;
                if (acceptArrival01.Results.AcceptArrival01Result != null)
                {
                    //Modelを初期化
                    acceptArrival01.Results.AcceptArrival01Result = new List<AcceptArrival01ResultRow>();
                    for (int i = 0; i < tempLst.Count; i++)
                    {
                        //削除行ではない場合にデータをモデルに挿入
                        if (i != acceptArrival01.DelRowNo)
                        {
                            acceptArrival01.Results.AcceptArrival01Result.Add(tempLst[i]);
                        }
                    }
                }
            }
            return this.PartialView("~/Areas/Returns/Views/AcceptArrival/_ResultRows.cshtml", acceptArrival01);
        }
        #endregion

        #region 出荷情報一覧
        /// <summary>
        /// 出荷情報一覧
        /// </summary>
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult List(AcceptArrival01ViewModel acceptArrival01)
        {
            this.ModelState.Clear();
            var vm = new AcceptArrival02ViewModel();
            var janCombine = string.Empty;
            var scanQtyCombine = string.Empty;
            if (acceptArrival01.Results.AcceptArrival01Result != null)
            {
                for (int i = 0; i < acceptArrival01.Results.AcceptArrival01Result.Count; i++)
                {
                    if (i == 0)
                    {
                        janCombine = acceptArrival01.Results.AcceptArrival01Result[i].Jan;
                        scanQtyCombine = acceptArrival01.Results.AcceptArrival01Result[i].ScanQty.ToString();
                    }
                    else
                    {
                        janCombine = janCombine + ',' + acceptArrival01.Results.AcceptArrival01Result[i].Jan;
                        scanQtyCombine = scanQtyCombine + ',' + acceptArrival01.Results.AcceptArrival01Result[i].ScanQty.ToString();
                    }
                }
            }

            vm.SearchConditions = new AcceptArrival02SearchConditions()
            {
                CenterId = acceptArrival01.SearchConditions.CenterId,
                JanCombine = janCombine,
                ScanQtyCombine = scanQtyCombine,
                DestZip = acceptArrival01.SearchConditions.DestZip,
                DestName = acceptArrival01.SearchConditions.DestName,
                DestAddress = acceptArrival01.SearchConditions.DestAddress,
                DestTel = acceptArrival01.SearchConditions.DestTel,
                ShipInstructId = acceptArrival01.SearchConditions.ShipInstructId,
            };
            if (acceptArrival01.SearchConditions.ShpScanClass == AcceptArrival01SearchConditions.ShpScanClasses.ShpScan)
            {
                vm.SearchConditions.ShpScanClass = AcceptArrival02SearchConditions.ShpScanClasses.ShpScan;
            }
            else
            {
                vm.SearchConditions.ShpScanClass = AcceptArrival02SearchConditions.ShpScanClasses.JanScan;
            }

            var shipData = new AcceptArrivalQuery().GetShipInfo(vm.SearchConditions);

            //スキャンしたJANに対するデータが存在する場合
            if (shipData.Count != 0)
            {
                //Modelを初期化
                vm.Results = new SearchResults();
                vm.Results.AcceptArrival02Result = new List<AcceptArrival02ResultRow>();
                for (int i = 0; i < shipData.Count; i++)
                {
                    var newResultRow = new AcceptArrival02ResultRow();
                    newResultRow.ShipInstructId = shipData[i].ShipInstructId;
                    newResultRow.KakuDate = shipData[i].KakuDate;
                    newResultRow.DestPrefName = shipData[i].DestPrefName;
                    newResultRow.DestZip = shipData[i].DestZip;
                    newResultRow.DestName = shipData[i].DestName;
                    vm.Results.AcceptArrival02Result.Add(newResultRow);
                }
            }

            return this.View("~/Areas/Returns/Views/AcceptArrival/List.cshtml", vm);
        }

        /// <summary>
        /// 出荷情報一覧
        /// </summary>
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult ListReturn(AcceptArrival03ViewModel acceptArrival03)
        {
            this.ModelState.Clear();
            var vm = new AcceptArrival02ViewModel();

            vm.SearchConditions = new AcceptArrival02SearchConditions()
            {
                CenterId = acceptArrival03.SearchConditions.CenterId,
                JanCombine = acceptArrival03.SearchConditions.JanCombine,
                ScanQtyCombine = acceptArrival03.SearchConditions.ScanQtyCombine,
                DestZip = acceptArrival03.SearchConditions.DestZip,
                DestName = acceptArrival03.SearchConditions.DestName,
                DestAddress = acceptArrival03.SearchConditions.DestAddress,
                DestTel = acceptArrival03.SearchConditions.DestTel,
                ShipInstructId = acceptArrival03.InputResults.AcceptArrival03Result[0].ShipInstructId,
            };

            if (acceptArrival03.SearchConditions.ShpScanClass == AcceptArrival03SearchConditions.ShpScanClasses.ShpScan)
            {
                vm.SearchConditions.ShpScanClass = AcceptArrival02SearchConditions.ShpScanClasses.ShpScan;
            }
            else
            {
                vm.SearchConditions.ShpScanClass = AcceptArrival02SearchConditions.ShpScanClasses.JanScan;
            }

            var shipData = new AcceptArrivalQuery().GetShipInfo(vm.SearchConditions);

            //スキャンしたJANに対するデータが存在する場合
            if (shipData.Count != 0)
            {
                //Modelを初期化
                vm.Results = new SearchResults();
                vm.Results.AcceptArrival02Result = new List<AcceptArrival02ResultRow>();
                for (int i = 0; i < shipData.Count; i++)
                {
                    var newResultRow = new AcceptArrival02ResultRow();
                    newResultRow.ShipInstructId = shipData[i].ShipInstructId;
                    newResultRow.KakuDate = shipData[i].KakuDate;
                    newResultRow.DestPrefName = shipData[i].DestPrefName;
                    newResultRow.DestZip = shipData[i].DestZip;
                    newResultRow.DestName = shipData[i].DestName;
                    vm.Results.AcceptArrival02Result.Add(newResultRow);
                }
            }

            return this.View("~/Areas/Returns/Views/AcceptArrival/List.cshtml", vm);
        }
        #endregion

        #region 実績入力
        /// <summary>
        /// 実績入力
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult SortList(AcceptArrival02ViewModel acceptArrival02)
        {
            this.ModelState.Clear();
            var shipData = new AcceptArrivalQuery().GetShipInfo(acceptArrival02.SearchConditions);

            //スキャンしたJANに対するデータが存在する場合
            if (shipData.Count != 0)
            {
                //Modelを初期化
                acceptArrival02.Results = new SearchResults();
                acceptArrival02.Results.AcceptArrival02Result = new List<AcceptArrival02ResultRow>();
                for (int i = 0; i < shipData.Count; i++)
                {
                    var newResultRow = new AcceptArrival02ResultRow();
                    newResultRow.ShipInstructId = shipData[i].ShipInstructId;
                    newResultRow.KakuDate = shipData[i].KakuDate;
                    newResultRow.DestPrefName = shipData[i].DestPrefName;
                    newResultRow.DestZip = shipData[i].DestZip;
                    newResultRow.DestName = shipData[i].DestName;
                    acceptArrival02.Results.AcceptArrival02Result.Add(newResultRow);
                }
            }
            return this.View("~/Areas/Returns/Views/AcceptArrival/List.cshtml", acceptArrival02);
        }

        /// <summary>
        /// 実績入力
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult Input(AcceptArrival02ViewModel acceptArrival02)
        {
            this.ModelState.Clear();
            var selectList = acceptArrival02.Results.AcceptArrival02Result.Where(x => x.IsCheck == true).Single();
            var itemData = new AcceptArrivalQuery().GetReturnInfo(acceptArrival02.SearchConditions.CenterId, selectList.ShipInstructId);
            AcceptArrival03ViewModel vm = new AcceptArrival03ViewModel();
            //検索条件引継ぎ
            vm.SearchConditions = new AcceptArrival03SearchConditions();
            vm.SearchConditions.CenterId = acceptArrival02.SearchConditions.CenterId;
            vm.SearchConditions.DestZip = acceptArrival02.SearchConditions.DestZip;
            vm.SearchConditions.DestName = acceptArrival02.SearchConditions.DestName;
            vm.SearchConditions.DestAddress = acceptArrival02.SearchConditions.DestAddress;
            vm.SearchConditions.DestTel = acceptArrival02.SearchConditions.DestTel;
            vm.SearchConditions.JanCombine = acceptArrival02.SearchConditions.JanCombine;
            vm.SearchConditions.ScanQtyCombine = acceptArrival02.SearchConditions.ScanQtyCombine;
            vm.InputResults = new SearchInput();
            vm.InputResults.AcceptArrival03Result = new List<AcceptArrival03ResultRow>();
            vm.InputResults.AcceptArrival03Result = itemData;

            if (acceptArrival02.SearchConditions.ShpScanClass == AcceptArrival02SearchConditions.ShpScanClasses.ShpScan)
            {
                vm.SearchConditions.ShpScanClass = AcceptArrival03SearchConditions.ShpScanClasses.ShpScan;
            }
            else
            {
                vm.SearchConditions.ShpScanClass = AcceptArrival03SearchConditions.ShpScanClasses.JanScan;
            }

            return this.View("~/Areas/Returns/Views/AcceptArrival/Input.cshtml", vm);
        }
        /// <summary>
        /// 実績入力
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult SkipInput(AcceptArrival01ViewModel acceptArrival01)
        {
            this.ModelState.Clear();
            var itemData = new AcceptArrivalQuery().GetReturnInfo(acceptArrival01.SearchConditions.CenterId, acceptArrival01.SearchConditions.ShipInstructId);
            AcceptArrival03ViewModel vm = new AcceptArrival03ViewModel();
            vm.InputResults = new SearchInput();
            vm.InputResults.AcceptArrival03Result = new List<AcceptArrival03ResultRow>();
            vm.InputResults.AcceptArrival03Result = itemData;
            vm.SearchConditions = new AcceptArrival03SearchConditions();
            vm.SearchConditions.CenterId = acceptArrival01.SearchConditions.CenterId;
            this.TempData["ReturnFlg"] = "1";
            return this.View("~/Areas/Returns/Views/AcceptArrival/Input.cshtml", vm);
        }
        #endregion 実績入力

        #region 実績登録
        /// <summary>
        /// 実績登録
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult Create(AcceptArrival03ViewModel acceptArrival03)
        {
            this.ModelState.Clear();
            List<AcceptArrival03ResultRow> inputData = acceptArrival03.InputResults.AcceptArrival03Result;

            // 返品実績登録
            var ret = new AcceptArrivalQuery().InputReturn(acceptArrival03.SearchConditions.CenterId, inputData);
            AcceptArrival01ViewModel vm = new AcceptArrival01ViewModel();
            if (ret == null)
            {
                TempData[AppConst.SUCCESS] = AcceptArrivalResource.UpdateSuc;
            }
            else
            {
                TempData[AppConst.ERROR] = ret;
            }

            return this.RedirectToAction("Index");
        }
        #endregion

        #region JANチェック
        /// <summary>
        /// JANチェック
        /// </summary>
        /// <param name="AllocationNo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetJanCheck(string jan)
        {
            bool result = new AcceptArrivalQuery().ExistJan(jan);

            if (result)
            {
                return this.Json(1);
            }

            return this.Json(0);
        }
        #endregion

        #region 出荷指示IDチェック
        /// <summary>
        /// 出荷指示IDチェック
        /// </summary>
        /// <param name="AllocationNo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetIdCheck(string shipInstructId, string centerId)
        {
            bool result = new AcceptArrivalQuery().ExistId(shipInstructId, centerId);

            if (result)
            {
                return this.Json(1);
            }

            return this.Json(0);
        }
        #endregion
    }

}