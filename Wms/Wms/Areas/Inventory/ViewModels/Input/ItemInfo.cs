namespace Wms.Areas.Inventory.ViewModels.Input
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Inventory.Resources;
    using Wms.Common;

    /// <summary>
    /// 商品
    /// </summary>
    public class ItemInfo
    {
        #region プロパティ

        /// <summary>
        /// SKU
        /// </summary>
        /// <remarks>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        /// <remarks>
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        /// <remarks>
        public string ItemSizeName { get; set; }

        #endregion プロパティ
    }
}