namespace Wms.Areas.Ship.ViewModels.TransferReference
{
    using PagedList;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;
    using static Wms.Areas.Ship.ViewModels.TransferReference.TransferReferenceSearchConditions;

    public class PickSortDetailHead
    {
        /// <summary>
        /// バッチNo
        /// </summary>
        public string BatchNo { get; set; }

        public string BatchNoHid { get; set; }

        /// <summary>
        /// バッチ名称
        /// </summary>
        public string BatchName { get; set; }

        /// <summary>
        /// ピック種別
        /// </summary>
        public int PicKind { get; set; }

        /// <summary>
        /// 出荷区分
        /// </summary>
        public ShipKinds ShipKind { get; set; }

        /// <summary>
        /// 総出荷先数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ShipToStoreQty { get; set; }

        /// <summary>
        /// 総オーダー数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ShipInstructQty { get; set; }

        /// <summary>
        /// 総SKU数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuQty { get; set; }

        /// <summary>
        /// 総引当数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? HikiQty { get; set; }

        /// <summary>
        /// 出荷予定日 (画面表示)
        /// </summary>
        public string ShipPlanDate { get; set; }

        /// <summary>
        /// 出荷予定日 (検索用)
        /// </summary>
        public DateTime? ShipPlanDateSearch { get; set; }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; }

        /// <summary>
        /// ページ行数
        /// </summary>
        public int PageSize { get; set; }
    }

    public class PickSortDetailRow
    {
        /// <summary>
        /// バッチNo
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// 出荷先数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ShipToStoreQty { get; set; }

        /// <summary>
        /// オーダー数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ShipInstructQty { get; set; }

        /// <summary>
        /// 引当数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? HikiQty { get; set; }

        /// <summary>
        /// 完了数量
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PicQty { get; set; }

        /// <summary>
        /// 進捗率
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? Percent { get; set; }

        /// <summary>
        /// 欠品登録数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockOutRegQty { get; set; }

        /// <summary>
        /// 欠品確定数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockOutFixQty { get; set; }
    }

    public class PickResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<PickSortDetailRow> PickResults { get; set; }
    }

    public class PickSortDetailViewModel
    {
        public PickSortDetailHead Head { get; set; }

        public PickResult PickResults { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferReferenceViewModel"/> class.
        /// </summary>
        public PickSortDetailViewModel()
        {
            this.Head = new PickSortDetailHead();
            this.PickResults = new PickResult();
        }
    }

    public class PickSortDetailStoreHead
    {
        /// <summary>
        /// バッチNo
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        public string Jan { get; set; }
    }


    public class PickSortDetailStoreRow
    {
        /// <summary>
        /// レーン
        /// </summary>
        public string LaneNo { get; set; }

        /// <summary>
        /// 間口
        /// </summary>
        public string FrontageNo { get; set; }

        /// <summary>
        /// 店舗ID
        /// </summary>
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 店舗名
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// 仕分状況
        /// </summary>
        public string SortStatus { get; set; }

        /// <summary>
        /// 仕分担当者ID
        /// </summary>
        public string SortUserId { get; set; }

        /// <summary>
        /// 仕分担当者名
        /// </summary>
        public string SortUserName { get; set; }

        /// <summary>
        /// 仕分日時
        /// </summary>
        public string SortDate { get; set; }

        /// <summary>
        /// 引当数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQty { get; set; }

        /// <summary>
        /// 仕分数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? SortQty { get; set; }
    }

    public class PickStoreResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IList<PickSortDetailStoreRow> PickStoreResults { get; set; }
    }

    public class PickSortDetailStoreViewModel
    {
        public PickSortDetailStoreHead Head { get; set; }

        public PickStoreResult PickResults { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferReferenceViewModel"/> class.
        /// </summary>
        public PickSortDetailStoreViewModel()
        {
            this.Head = new PickSortDetailStoreHead();
            this.PickResults = new PickStoreResult();
        }
    }
}