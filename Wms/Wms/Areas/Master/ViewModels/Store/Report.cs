namespace Wms.Areas.Master.ViewModels.Store
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Foolproof;
    using Share.Common.Resources;
    using Wms.Common;
    using Wms.Models;

    public partial class Report
    {
        /// <summary>
        /// 新規追加日
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.MakeDate), ResourceType = typeof(Resources.StoreResource), Order = 1)]
        public string MakeDate { get; set; }

        /// <summary>
        /// 更新日
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.UpdateDate), ResourceType = typeof(Resources.StoreResource), Order = 2)]
        public string UpdateDate { get; set; }

        /// <summary>
        /// 店舗ID
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreId), ResourceType = typeof(Resources.StoreResource), Order = 3)]
        public string StoreId { get; set; }

        /// <summary>
        /// 店舗区分
        /// </summary>
        /// <remarks>
        /// 1:通常店舗, 2:催事店舗, 3:EC, 4:卸, 8:倉庫, 9:本部
        /// </remarks>
        [Display(Name = nameof(Resources.StoreResource.StoreClass), ResourceType = typeof(Resources.StoreResource), Order = 4)]
        public string StoreClass { get; set; }

        /// <summary>
        /// 店舗名1
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreName1), ResourceType = typeof(Resources.StoreResource), Order = 5)]
        public string StoreName1 { get; set; }

        /// <summary>
        /// 店舗名(略称) 
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreShortName), ResourceType = typeof(Resources.StoreResource), Order = 6)]
        public string StoreShortName { get; set; }

        /// <summary>
        /// 店舗郵便番号
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreZip), ResourceType = typeof(Resources.StoreResource), Order = 7)]
        public string StoreZip { get; set; }

        /// <summary>
        /// 店舗都道府県
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StorePrefName), ResourceType = typeof(Resources.StoreResource), Order = 7)]
        public string StorePrefName { get; set; }

        /// <summary>
        /// 店舗市区町村
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreCityName), ResourceType = typeof(Resources.StoreResource), Order = 8)]
        public string StoreCityName { get; set; }

        /// <summary>
        /// 店舗それ以降の住所1
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreAddress1), ResourceType = typeof(Resources.StoreResource), Order = 9)]
        public string StoreAddress1 { get; set; }

        /// <summary>
        /// 店舗それ以降の住所2
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreAddress2), ResourceType = typeof(Resources.StoreResource), Order = 10)]
        public string StoreAddress2 { get; set; }

        /// <summary>
        /// 店舗それ以降の住所3
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreAddress3), ResourceType = typeof(Resources.StoreResource), Order = 11)]
        public string StoreAddress3 { get; set; }

        /// <summary>
        /// 店舗TEL
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreTel), ResourceType = typeof(Resources.StoreResource), Order = 12)]
        public string StoreTel { get; set; }

        /// <summary>
        /// 店舗FAX
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreFax), ResourceType = typeof(Resources.StoreResource), Order = 13)]
        public string StoreFax { get; set; }

        /// <summary>
        /// 店舗Mail1
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreMail1), ResourceType = typeof(Resources.StoreResource), Order = 14)]
        public string StoreMail1 { get; set; }

        /// <summary>
        /// ECサイト区分
        /// </summary>
        /// <remarks>
        /// 1:Amazon, 2:Yahoo, 3:Rakuten, 4:Zozo, 5:SHOPLIST
        /// </remarks>
        [Display(Name = nameof(Resources.StoreResource.EcClass), ResourceType = typeof(Resources.StoreResource), Order = 15)]
        public string EcClass { get; set; }

        /// <summary>
        /// 店舗ランクID
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreRankId), ResourceType = typeof(Resources.StoreResource), Order = 16)]
        public string StoreRankId { get; set; }

        /// <summary>
        /// 開店日
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.OpenDate), ResourceType = typeof(Resources.StoreResource), Order = 17)]
        public string OpenDate { get; set; }

        /// <summary>
        /// 閉店日
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.CloseDate), ResourceType = typeof(Resources.StoreResource), Order = 18)]
        public string CloseDate { get; set; }

        /// <summary>
        /// エリアID
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.AreaId), ResourceType = typeof(Resources.StoreResource), Order = 19)]
        public string AreaId { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.PrefId), ResourceType = typeof(Resources.StoreResource), Order = 20)]
        public string PrefId { get; set; }

        /// <summary>
        /// 欠品不可フラグ
        /// </summary>
        /// <remarks>
        /// 1:引当時欠品不可
        /// メンテナンス画面からメンテされる。
        /// 受信時新規追加する場合、店舗区分が卸なら1を自動でセットする
        /// </remarks>
        [Display(Name = nameof(Resources.StoreResource.StockOutDisableFlag), ResourceType = typeof(Resources.StoreResource), Order = 21)]
        public string StockOutDisableFlag { get; set; }

        /// <summary>
        /// 検品必須フラグ
        /// </summary>
        /// <remarks>
        /// 1:検品必須
        /// メンテナンス画面からメンテされる。
        /// 受信時新規追加する場合、店舗区分が卸なら1を自動でセットする
        /// </remarks>
        [Display(Name = nameof(Resources.StoreResource.InspectionMustFlag), ResourceType = typeof(Resources.StoreResource), Order = 22)]
        public string InspectionMustFlag { get; set; }

        /// <summary>
        /// 事業部ID
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.DivisionId), ResourceType = typeof(Resources.StoreResource), Order = 23)]
        public string DivisionId { get; set; }

        /// <summary>
        /// 売上基準区分
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.InvoiceStoreName), ResourceType = typeof(Resources.StoreResource), Order = 24)]
        public string InvoiceStoreName { get; set; }

        /// <summary>
        /// 売上基準区分
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.PatternId), ResourceType = typeof(Resources.StoreResource), Order = 25)]
        public string PatternId { get; set; }

        /// <summary>
        /// 売上基準区分
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.SaleBaseClass), ResourceType = typeof(Resources.StoreResource), Order = 26)]
        public string SaleBaseClass { get; set; }

        /// <summary>
        /// 管轄倉庫コード
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.ControlCenterId), ResourceType = typeof(Resources.StoreResource), Order = 27)]
        public string ControlCenterId { get; set; }

        /// <summary>
        /// 金額丸目区分
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.RoundClass), ResourceType = typeof(Resources.StoreResource), Order = 28)]
        public string RoundClass { get; set; }

        /// <summary>
        /// 仮店舗区分
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.TempStoreClass), ResourceType = typeof(Resources.StoreResource), Order =29)]
        public string TempStoreClass { get; set; }

        /// <summary>
        /// 閉店区分
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.CloseClass), ResourceType = typeof(Resources.StoreResource), Order =30)]
        public string CloseClass { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.DeleteFlag), ResourceType = typeof(Resources.StoreResource), Order = 24)]
        public string DeleteFlag { get; set; }
    }
}