namespace Wms.Areas.Ship.ViewModels.SortSet
{
    using Share.Common.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 仕分コード設定List
    /// </summary>
    public class SortSetResultRow
    {
        #region プロパティ

        /// <summary>
        /// テーブル種別
        /// </summary>
        /// <remarks>
        public int TableClass { get; set; }

        /// <summary>
        /// 配送業者ID
        /// </summary>
        /// <remarks>
        public string TransporterId { get; set; }

        /// <summary>
        /// 県名
        /// </summary>
        /// <remarks>
        public string PrefName { get; set; }

        /// <summary>
        /// 市名
        /// </summary>
        /// <remarks>
        public string CityName { get; set; }

        /// <summary>
        /// 住所1
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// 区分
        /// </summary>
        /// <remarks>
        public string Class { get; set; }

        /// <summary>
        /// 出荷予定日
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 出荷指示ID/注文番号
        /// </summary>
        /// <remarks>
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 出荷先ID
        /// </summary>
        /// <remarks>
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先名
        /// </summary>
        /// <remarks>
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// 運送会社
        /// </summary>
        /// <remarks>
        public string TransporterName { get; set; }

        /// <summary>
        /// 郵便番号
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipToZip { get; set; }

        /// <summary>
        /// 配送先住所
        /// </summary>
        /// <remarks>
        public string ShipToAddress { get; set; }

        /// <summary>
        /// 仕分コード
        /// </summary>
        /// <remarks>
        public string SortingCd { get; set; }

        /// <summary>
        /// 仕分コードHid
        /// </summary>
        /// <remarks>
        public string SortingCdHid { get; set; }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        public long LineNo { get; set; }

        #endregion プロパティ
    }
}