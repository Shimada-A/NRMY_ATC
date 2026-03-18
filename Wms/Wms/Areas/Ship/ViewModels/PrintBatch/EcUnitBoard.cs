namespace Wms.Areas.Ship.ViewModels.PrintBatch
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;

    /// <summary>
    /// DC List
    /// </summary>
    public class EcUnitBoard
    {
        #region プロパティ

        /// <summary>
        /// センター
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.Center), ResourceType = typeof(PrintBatchResource), Order = 1)]
        public string Center { get; set; }

        /// <summary>
        /// 発行者
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.PrintUser), ResourceType = typeof(PrintBatchResource), Order = 2)]
        public string PrintUser { get; set; }

        /// <summary>
        /// バッチNo
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.BatchNo1), ResourceType = typeof(PrintBatchResource), Order = 3)]
        public string BatchNo { get; set; }

        /// <summary>
        /// バッチNo バーコード(ガイドあり）
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.BatchNoBarcode), ResourceType = typeof(PrintBatchResource), Order = 4)]
        public string BatchNoBarcode { get; set; }

        /// <summary>
        /// バッチ名称
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.BatchName), ResourceType = typeof(PrintBatchResource), Order = 5)]
        public string BatchName { get; set; }

        /// <summary>
        /// EC_SHIP_CLASS
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.EcShipClass), ResourceType = typeof(PrintBatchResource), Order = 6)]
        public string EcShipClass { get; set; }

        /// <summary>
        /// EC出荷形態
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.EcShipClass), ResourceType = typeof(PrintBatchResource), Order = 7)]
        public string EcShipClassName { get; set; }


        private string _EcUnitIdBarcode { get; set; }
        
            /// <summary>
            /// ECユニットIDバーコード
            /// </summary>
            [Display(Name = nameof(PrintBatchResource.EcUnitIdBarcode), ResourceType = typeof(PrintBatchResource), Order = 8)]
        public string EcUnitIdBarcode { 
                get 
                {
                if (EcShipClass == "1") return string.Empty;
                return _EcUnitIdBarcode;
                }
            set { _EcUnitIdBarcode = value; } }

        /// <summary>
        /// ECユニットIDバーコード
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.EcUnitIdBarcode), ResourceType = typeof(PrintBatchResource), Order = 9)]
        public string EcUnitIdBarcodeBatch
        {
            get
            {
                if (EcShipClass != "1") return string.Empty;
                return _EcUnitIdBarcode;
                }
            set { _EcUnitIdBarcode = value; }
        }

        /// <summary>
        /// ECユニットID名称
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.EcUnitName), ResourceType = typeof(PrintBatchResource), Order = 10)]
        public string EcUnitName { get; set; }

        #endregion プロパティ
    }
}