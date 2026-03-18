namespace Wms.Areas.Returns.ViewModels.PurchaseCorrection
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;
    using System.Collections.Generic;
    using Wms.Areas.Returns.ViewModels.JanSearchModal;
    using Wms.Areas.Returns.ViewModels.LocationSearchModal;
    using Wms.Areas.Returns.ViewModels.CaseSearchModal;
    using Wms.Areas.Returns.ViewModels.InvoiceSearchModal;

    public class PurchaseCorrectionSearchConditions
    {
        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// JAN
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        public string LocationCd { get; set; }

        /// <summary>
        /// ロケーション区分
        /// </summary>
        public string LocationClass { get; set; }

        /// <summary>
        /// 格付け
        /// </summary>
        public string GradeId { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 差異数
        /// </summary>
        public int? SaiQty { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        public string VendorName { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

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

        public DateTime? ArriveDate { get; set; }

        public DateTime? ConfirmDate { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        public string LineNo { get; set; }

        /// <summary>
        /// 指定ラジオボタン(検索用)
        /// </summary>
        public int CheckRadio { get; set; }

        public IList<JanViewModel> janViewModel { get; set; }

        public IList<LocationViewModel> locationViewModel { get; set; }

        public IList<CaseViewModel> caseViewModel { get; set; }

        public IList<InvoiceViewModel> invoiceViewModel { get; set; }

        public int? totalCnt { get; set; }

        public string ReturnId { get; set; }

        public string Ret { get; set; }

        public string Print { get; set; }
    }
}