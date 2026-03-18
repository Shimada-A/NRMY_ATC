namespace Wms.Controllers
{
    using System.Web.Mvc;
    using Wms.Models;
    using Wms.Query;
    using Wms.Resources;
    using Wms.ViewModels.Home;
    using Wms.Models.Home;
    using System.Collections.Generic;
    
    public class HomeController : BaseController
    {
        private HomeQuery _HomeQuery;

        public ActionResult Index()
        {
            ModelState.Clear();
            try
            {
                string centerId = Common.Profile.User.CenterId;
                return this.GetSearchResultView(centerId, centerId);
            }
            catch (System.Exception ex)
            {
                Mvc.Common.AppError.PutLogREF(ex, "index");
                throw;
            }

        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param></param>
        [HttpPost]
        public ActionResult Search(Arrive arrive, Ship ship)
        {
            try
            {
                string arriveCenterId = arrive.CenterId;
                string shipCenterId = ship.CenterId;
                return this.GetSearchResultView(arriveCenterId, shipCenterId);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public ActionResult About()
        {
            this.ViewBag.Message = "Your application description page.";

            return this.View();
        }

        public ActionResult Contact()
        {
            this.ViewBag.Message = "Your contact page.";

            return this.View();
        }

        /// <summary>
        /// サイドメニューを表示します。
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult Menu()
        {
            var menu = new Menu().Listing();
            return this.PartialView("_Menu", menu);
        }

        /// <summary>
        /// サブタイトルを取得する。
        /// </summary>
        /// <returns>サブタイトル</returns>
        [ChildActionOnly]
        public ActionResult GetSubTitle()
        {
            string mySubTitle = string.Format(HomeResource.SubTitle, new Areas.Master.Models.Warehouses().GetNameById(), Common.Profile.User.UserName);

            this.TempData["MySubTitle"] = mySubTitle;

            return this.PartialView("_MySubTitle");
        }

        #region 検索処理
        /// <summary>
        /// 検索処理
        /// </summary>
        public ActionResult GetSearchResultView(string arriveCenterId, string shipCenterId)
        {
            try
            {
                this._HomeQuery = new HomeQuery();
                ArriveModels arriveModels = new ArriveModels();
                ShipModels shipModels = new ShipModels();
                List<Message> messageModels = new List<Message>();
                 messageModels = _HomeQuery.GetDataMessage(Common.Profile.User.CenterId);
                arriveModels = _HomeQuery.GetDashArrives(arriveCenterId);
                shipModels = _HomeQuery.GetDataShip(shipCenterId);

                var vm = new Index();

                // 自動更新切替間隔
                vm.UpdTime = this.UpdInterval();

                // 初期値をセットする
                vm.TcDcKbn = "TC";

                // お知らせ
                if (messageModels != null)
                {
                    vm.Message = messageModels;
                }
                // 入荷進捗
                if (arriveModels != null)
                {
                    vm.Arrive = new Arrive()
                    {
                        ArrivePlanDate = arriveModels.ArrivePlanDate,
                        CenterId = arriveModels.CenterId,
                        ArriveProgress = new TcdcChart()
                        {
                            Title = HomeResource.ArriveProgress,
                            TcQty = arriveModels.ArriveResultQty,
                            DcQty = arriveModels.ArrivePlanQty,
                        },
                        TcArrivePartProgress = new TcdcChart()
                        {
                            Title = HomeResource.TcArrivePartProgress,
                            TcQty = arriveModels.ArriveTcResultQty,
                            DcQty = arriveModels.ArriveTcPlanQty,
                        },
                        DcShelvesProgress = new TcdcChart()
                        {
                            Title = HomeResource.DcShelvesProgress2,
                            TcQty = arriveModels.ArriveDcResultQty,
                            DcQty = arriveModels.ArriveDcPlanQty
                        },
                    };
                }
                else
                {
                    vm.Arrive = new Arrive()
                    {  // If文の判定をデバック環境以外でもできるように記述
                        CenterId = string.Empty,
                    };
                }
                //出荷進捗
                if (shipModels != null)
                {
                    vm.Ship = new Ship()
                    {
                        ShipPlanDate = shipModels.ShipPlanDate,
                        CenterId = shipModels.CenterId,
                        ShipEcQty = new TcdcChart()
                        {
                            Title = string.Format(HomeResource.EcQtyOrder, shipModels.ShipEcOrderQty),
                            TcQty = shipModels.ShipEcResultQty,
                            DcQty = shipModels.ShipEcPlanQty
                        },
                        ShipTcQty = new TcdcChart()
                        {
                            Title = HomeResource.TcQty,
                            TcQty = shipModels.ShipTcResultQty,
                            DcQty = shipModels.ShipTcPlanQty
                        },
                        ShipDcQty = new TcdcChart()
                        {
                            Title = HomeResource.DcQty,
                            TcQty = shipModels.ShipDcResultQty,
                            DcQty = shipModels.ShipDcPlanQty
                        },
                        PickEcQty = new TcdcChart()
                        {
                            Title = HomeResource.EcQty,
                            TcQty = shipModels.PickEcResultQty,
                            DcQty = shipModels.PickEcPlanQty
                        },
                        PickDcQty = new TcdcChart()
                        {
                            Title = HomeResource.DcQty,
                            TcQty = shipModels.PickDcResultQty,
                            DcQty = shipModels.PickDcPlanQty
                        },
                        StoreTcQty = new TcdcChart()
                        {
                            Title = HomeResource.TcQty,
                            TcQty = shipModels.StoreTcResultQty,
                            DcQty = shipModels.StoreTcPlanQty
                        },
                        StoreDcQty = new TcdcChart()
                        {
                            Title = HomeResource.DcQty,
                            TcQty = shipModels.StoreDcResultQty,
                            DcQty = shipModels.StoreDcPlanQty
                        },
                        InvoiceEcQty = new TcdcChart()
                        {
                            Title = string.Format(HomeResource.EcQtyOrder, shipModels.InvoiceEcOrderQty),
                            TcQty = shipModels.InvoiceEcResultQty,
                            DcQty = shipModels.InvoiceEcPlanQty
                        },
                        InvoiceTcQty = new TcdcChart()
                        {
                            Title = HomeResource.TcQty,
                            TcQty = shipModels.InvoiceTcResultQty,
                            DcQty = shipModels.InvoiceTcPlanQty
                        },
                        InvoiceDcQty = new TcdcChart()
                        {
                            Title = HomeResource.DcQty,
                            TcQty = shipModels.InvoiceDcResultQty,
                            DcQty = shipModels.InvoiceDcPlanQty
                        }
                    };
                }
                else
                {
                    vm.Ship = new Ship()
                    {  // If文の判定をデバック環境以外でもできるように記述
                        CenterId = string.Empty,
                    };
                }

                return this.View("~/Views/Home/Index.cshtml", vm);
            }
            catch (System.Exception ex)
            {
                Mvc.Common.AppError.PutLogREF(ex, "index");
                throw;
            }

        }
        #endregion 検索処理

        #region 自動更新間隔を取得する

        /// <summary>
        /// 自動更新間隔を取得する。
        /// </summary>
        /// <returns>int</returns>
        public int UpdInterval()
        {
            int intervalTime = _HomeQuery.GetIntervalTime();

            return intervalTime;
        }

        #endregion

        #region 画面遷移
        /// <summary>
        /// 出荷業務画面遷移
        /// </summary>
        /// <returns>int</returns>
        [HttpPost]
        public ActionResult Move(Index index)
        {
            string path = Url.Action("Search", "TransferReference", new { area = "Ship" });
            this.TempData["TcDcKbn"] = index.TcDcKbn;
            return this.Redirect(path);
        }
        #endregion

        #region ダウンロード

        [HttpGet]
        public ActionResult Download()
        {
            string downloadWfr = Server.MapPath("~/Reports/wfr2016_20230519_p_admin.exe");

            return File(downloadWfr, "application/octet-stream", "wfr2016_20230519_p_admin.exe");
        }

        #endregion

    }
}