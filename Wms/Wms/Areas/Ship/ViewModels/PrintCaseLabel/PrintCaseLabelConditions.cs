namespace Wms.Areas.Ship.ViewModels.PrintCaseLabel
{
    using Share.Common.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class PrintCaseLabelConditions
    {
        /// <summary>
        /// 発行区分
        /// </summary>
        public enum ReleaseClasses : byte
        {
            [Display(Name = nameof(Resources.PrintCaseLabelResource.Release), ResourceType = typeof(Resources.PrintCaseLabelResource))]
            Release,

            [Display(Name = nameof(Resources.PrintCaseLabelResource.AgainRelease), ResourceType = typeof(Resources.PrintCaseLabelResource))]
            AgainRelease
        }

        /// <summary>
        /// 出荷先種別
        /// </summary>
        public enum ShipToClasses : byte
        {
            [Display(Name = nameof(Resources.PrintCaseLabelResource.BtoB), ResourceType = typeof(Resources.PrintCaseLabelResource))]
            BtoB,

            //[Display(Name = nameof(Resources.PrintCaseLabelResource.EC), ResourceType = typeof(Resources.PrintCaseLabelResource))]
            //EC
        }

        /// <summary>
        /// 店舗区分
        /// </summary>
        public enum StoreClasses : byte
        {
            [Display(Name = nameof(Resources.PrintCaseLabelResource.Store), ResourceType = typeof(Resources.PrintCaseLabelResource))]
            Store,

            [Display(Name = nameof(Resources.PrintCaseLabelResource.Centers), ResourceType = typeof(Resources.PrintCaseLabelResource))]
            Centers
        }

        /// <summary>
        /// 催事店舗区分
        /// </summary>
        public enum StoreOutletsClasses : byte
        {
            [Display(Name = nameof(Resources.PrintCaseLabelResource.InStoreOutlets), ResourceType = typeof(Resources.PrintCaseLabelResource))]
            Outlets,

            [Display(Name = nameof(Resources.PrintCaseLabelResource.NotStoreOutlets), ResourceType = typeof(Resources.PrintCaseLabelResource))]
            NotOutlets,

            [Display(Name = nameof(Resources.PrintCaseLabelResource.OnlyStoreOutlets), ResourceType = typeof(Resources.PrintCaseLabelResource))]
            OnlyOutlets
        }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Profile.User.CenterId;
        
        /// <summary>
        /// 発行区分
        /// </summary>
        public ReleaseClasses ReleaseClass { get; set; } = ReleaseClasses.Release;

        /// <summary>
        /// 出荷先種別
        /// </summary>
        public ShipToClasses ShipToClass { get; set; } = ShipToClasses.BtoB;

        /// <summary>
        /// 店舗区分
        /// </summary>
        public StoreClasses StoreClass { get; set; } = StoreClasses.Store;

        /// <summary>
        ///// 催事店舗区分
        ///// </summary>
        public StoreOutletsClasses StoreOutletsClass { get; set; } = StoreOutletsClasses.NotOutlets;

        /// <summary>
        /// ブランド
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 出荷先区分
        /// </summary>
        public string ShipToStoreClass { get; set; }

        /// <summary>
        /// 選択 0メイン  1月  2火  3水  4木  5金  6土  7日  8祝
        /// </summary>
        public string DayClass { get; set; }

        /// <summary>
        /// 出荷先ID
        /// </summary>
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先NAME
        /// </summary>
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// 配送業者NAME
        /// </summary>
        public string TransporterName { get; set; }

        /// <summary>
        /// 配送業者ID
        /// </summary>
        public string TransporterId { get; set; }

        /// <summary>
        /// 出荷先-配送業者NAME
        /// </summary>
        public string ShipToTransporterName { get; set; }

        /// <summary>
        /// 枚数
        /// </summary>
        [Display(Name = nameof(Resources.PrintCaseLabelResource.NumberofSheets), ResourceType = typeof(Resources.PrintCaseLabelResource))]
        public long? NumberofSheets { get; set; } = 0;

        /// <summary>
        /// 再発行ケースNo
        /// </summary>
        [Display(Name = nameof(Resources.PrintCaseLabelResource.ReleaseBoxNo), ResourceType = typeof(Resources.PrintCaseLabelResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        [MinLength(5, ErrorMessageResourceName = nameof(MessageResource.MinLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string ReleaseBoxNo { get; set; }

        /// <summary>
        /// 出荷指示データに存在する店舗のみ出力
        /// </summary>
        public bool ShipInstructFlag { get; set; }

        /// <summary>
        /// バッチNo
        /// </summary>
       public string BatchNo { get; set; }
    }
}