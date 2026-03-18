namespace Wms.Areas.Ship.ViewModels.TransferReference
{
    using OfficeOpenXml.FormulaParsing.ExcelUtilities;
    using PagedList;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;
    using static Wms.Areas.Ship.ViewModels.TransferReference.TransferReferenceSearchConditions;

    public class PickResultHead
    {
        /// <summary>
        /// バッチNo
        /// </summary>
        public string BatchNo { get; set; }

        public string BatchNoHid { get; set; }

        public string BatchName { get; set; }

        /// <summary>
        /// 出荷区分
        /// </summary>
        public ShipKinds ShipKind { get; set; }

        /// <summary>
        /// ピック種別
        /// </summary>
        public int? PicKind { get; set; }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set;}

        /// <summary>
        /// ページ行数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 出荷日 (画面表示用)
        /// </summary>
        public string ShipDate { get; set; }

        /// <summary>
        /// 出荷日 (SQL実行用)
        /// </summary>
        public DateTime? ShipDateSearch { get; set; }

    }

    public class PickResultRow
    {
        /// <summary>
        /// ピッキンググループNo
        /// </summary>
        public string PickingGroupNo { get; set; }


        /// <summary>
        /// 出荷先ID
        /// </summary>
        public string ShipToStoreId { get; set; }
        
        /// <summary>
        /// 出荷先名
        /// </summary>
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// エリア
        /// </summary>
        public string Locsec1 { get; set; }

        /// <summary>
        /// ロケ
        /// </summary>
        public string LocationCd { get; set; }

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
        /// ピックステータス
        /// </summary>
        public int? PicStatus { get; set; }

        /// <summary>
        /// ピック予定数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQty { get; set; }

        /// <summary>
        /// ピック実績数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PicQty { get; set; }

        /// <summary>
        /// ピック日時
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? PicDate { get; set; }

        /// <summary>
        /// ピック担当者ID
        /// </summary>
        public string PicUserId { get; set; }

        /// <summary>
        /// ピック担当者名
        /// </summary>
        public string PicUserName { get; set; }

        /// <summary>
        /// 欠品登録数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockOutRegQty { get; set; }

        /// <summary>
        /// 欠品登録日時
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? StockOutRegDate { get; set; }

        /// <summary>
        /// 欠品登録担当者ID
        /// </summary>
        public string StockOutRegUserId { get; set; }

        /// <summary>
        /// 欠品登録担当者名
        /// </summary>
        public string StockOutRegUserName { get; set; }

        /// <summary>
        /// 欠品確定日時
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? StockOutFixDate { get; set; }

        /// <summary>
        /// 欠品確定者ID
        /// </summary>
        public string StockOutFixUserId { get; set; }

        /// <summary>
        /// 欠品確定者名
        /// </summary>
        public string StockOutFixUserName { get; set; }

        /// <summary>
        /// 欠品確定数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockOutFixQty { get; set; }

        /// <summary>
        /// 梱包No
        /// </summary>
        public string BoxNo { get; set; }

    }

    public class PickResults
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<PickResultRow> PickResult { get; set; }
    }

    public class PickResultViewModel
    {
        public PickResultHead Head { get; set; }

        public PickResults PickResult { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferReferenceViewModel"/> class.
        /// </summary>
        public PickResultViewModel()
        {
            this.Head = new PickResultHead();
            this.PickResult = new PickResults();
        }
    }
}