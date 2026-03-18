namespace Wms.Areas.Master.ViewModels.Warehouses
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Foolproof;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Common;
    using Wms.Models;

    public partial class Report
    {
        /// <summary>
        /// 新規追加日
        /// </summary>
        [Display(Name = nameof(Resources.WarehousesResource.MakeDate), ResourceType = typeof(Resources.StoreResource), Order = 1)]
        public string MakeDate { get; set; }

        /// <summary>
        /// 更新日
        /// </summary>
        [Display(Name = nameof(Resources.WarehousesResource.UpdateDate), ResourceType = typeof(Resources.StoreResource), Order = 2)]
        public string UpdateDate { get; set; }


        /// <summary>
        /// センターコード
        /// </summary>
        [Display(Name = nameof(Resources.WarehousesResource.CenterId), ResourceType = typeof(Resources.StoreResource), Order = 3)]
        public string CenterId { get; set; }

        /// <summary>
        /// 店舗区分
        /// </summary>
        [Display(Name = nameof(Resources.WarehousesResource.CenterClass), ResourceType = typeof(Resources.StoreResource), Order = 4)]
        public string CenterClass { get; set; }

        /// <summary>
        /// センター名
        /// </summary>
        [Display(Name = nameof(Resources.WarehousesResource.CenterName), ResourceType = typeof(Resources.StoreResource), Order = 5)]
        public string CenterName1 { get; set; }

        /// <summary>
        /// センター名(略称)
        /// </summary>
        [Display(Name = nameof(Resources.WarehousesResource.CenterShortName), ResourceType = typeof(Resources.StoreResource), Order = 6)]
        public string CenterShortName { get; set; }

        /// <summary>
        /// 郵便番号
        /// </summary>
        [Display(Name = nameof(Resources.WarehousesResource.CenterZip), ResourceType = typeof(Resources.StoreResource), Order = 7)]
        public string CenterZip { get; set; }

        /// <summary>
        /// 都道府県
        /// </summary>
        [Display(Name = nameof(Resources.WarehousesResource.CenterPrefName), ResourceType = typeof(Resources.StoreResource), Order = 8)]
        public string CenterPrefName { get; set; }

        /// <summary>
        /// 市区町村
        /// </summary>
        [Display(Name = nameof(WarehousesResource.CenterCityName), ResourceType = typeof(WarehousesResource), Order = 9)]
        public string CenterCityName { get; set; }

        /// <summary>
        /// それ以降の住所1
        /// </summary>
        [Display(Name = nameof(Resources.WarehousesResource.CenterAddress1), ResourceType = typeof(Resources.StoreResource), Order = 10)]
        public string CenterAddress1 { get; set; }

        /// <summary>
        /// それ以降の住所2	
        /// </summary>
        [Display(Name = nameof(Resources.WarehousesResource.CenterAddress2), ResourceType = typeof(Resources.StoreResource), Order = 11)]
        public string CenterAddress2 { get; set; }

        /// <summary>
        /// それ以降の住所3	
        /// </summary>
        [Display(Name = nameof(Resources.WarehousesResource.CenterAddress3), ResourceType = typeof(Resources.StoreResource), Order = 12)]
        public string CenterAddress3 { get; set; }

        /// <summary>
        /// TEL
        /// </summary>
        [Display(Name = nameof(Resources.WarehousesResource.CenterTel), ResourceType = typeof(Resources.StoreResource), Order = 13)]
        public string CenterTel { get; set; }

        /// <summary>
        /// FAX
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreFax), ResourceType = typeof(Resources.StoreResource), Order = 14)]
        public string CenterFax { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        [Display(Name = nameof(Resources.WarehousesResource.PrefId), ResourceType = typeof(Resources.StoreResource), Order = 15)]
        public string PrefId { get; set; }

        /// <summary>
        /// チャネルID
        /// </summary>
        [Display(Name = nameof(Resources.WarehousesResource.ChannelId), ResourceType = typeof(Resources.StoreResource), Order = 16)]
        public string ChannelId { get; set; }

        /// <summary>
        /// チャネル名
        /// </summary>
        [Display(Name = nameof(Resources.WarehousesResource.ChannelName), ResourceType = typeof(Resources.StoreResource), Order = 17)]
        public string ChannelName { get; set; }

        /// <summary>
        /// WMS対象倉庫区分	
        /// </summary>
        /// <remarks>
        /// 0:WMS対象外、1:WMS対象
        /// </remarks>
        [Display(Name = nameof(Resources.WarehousesResource.WmsClass), ResourceType = typeof(Resources.StoreResource), Order = 18)]
        public string WmsClass { get; set; }

        /// <summary>
        /// ブランド別作業区分	
        /// </summary>
        /// <remarks>
        /// 0:ブランド別に作業しない、1:ブランド別に作業する
        /// </remarks>
        [Display(Name = nameof(Resources.WarehousesResource.BrandWorkClass), ResourceType = typeof(Resources.StoreResource), Order = 19)]
        public string BrandWorkClass { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        /// <remarks>
        /// 0:未削除 1:削除済み
        /// </remarks>
        [Display(Name = nameof(Resources.WarehousesResource.DeleteFlag), ResourceType = typeof(Resources.StoreResource), Order = 20)]
        public string DeleteFlag { get; set; }

        /// <summary>
        /// 送り状用倉庫名 (INVOICE_CENTER_NAME)
        /// </summary>
        /// <remarks>
        /// IF倉庫マスタ.倉庫名[文字(60)]
        /// </remarks>
        [Display(Name = nameof(WarehousesResource.InvoiceCenterName), ResourceType = typeof(WarehousesResource), Order = 21)]
        public string InvoiceCenterName { get; set; }

    }
}