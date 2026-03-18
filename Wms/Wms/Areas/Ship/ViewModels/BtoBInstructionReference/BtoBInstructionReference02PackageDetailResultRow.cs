namespace Wms.Areas.Ship.ViewModels.BtoBInstructionReference
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// DC List
    /// </summary>
    public class BtoBInstructionReference02PackageDetailResultRow
    {
        #region プロパティ
        /// <summary>
        /// ケースNo
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 送り状No
        /// </summary>
        public string DeliNo { get; set; }

        /// <summary>
        /// 出荷状況
        /// </summary>
        public string ShipStatusName { get; set; }

        /// <summary>
        /// 出荷予定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        public string CategoryName1 { get; set; }

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
        /// <remarks>
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        public string Jan { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 店仕分・摘取担当者
        /// </summary>
        public string AsortUserName { get; set; }

        /// <summary>
        /// 納品書発行担当者
        /// </summary>
        public string NouhinPrnUserName { get; set; }

        /// <summary>
        /// 欠品確定日
        /// </summary>
        public DateTime? StockOutFixDate { get; set; }

        /// <summary>
        /// 仕分状況 0:未作業　1:仕分中 2:中断中 3:仕分完了
        /// </summary>
        public int? SortStatus { get; set; }
        #endregion プロパティ
    }
}