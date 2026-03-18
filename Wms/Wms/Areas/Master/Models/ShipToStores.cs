namespace Wms.Areas.Master.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    /// <summary>
    /// センター別店舗別配送業者
    /// </summary>
    [Table("V_SHIP_TO_STORES")]
    public partial class ShipToStores
    {
        #region プロパティ

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Column(Order = 11)]
        public string ShipperId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Column(Order = 12)]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string ShipToStoreClass { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string ShipToStoreName1 { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string ShipToStoreShortName { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string ShipToZip { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string ShipToPrefName { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string ShipToCityName { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string ShipToAddress1 { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string ShipToAddress2 { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string ShipToAddress3 { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string ShipToTel { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string ShipToFax { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string ShipToMail1 { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string ShipToPrefId { get; set; }


        #endregion
    }
}
