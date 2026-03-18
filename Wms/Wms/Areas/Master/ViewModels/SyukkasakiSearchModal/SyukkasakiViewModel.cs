namespace Wms.Areas.Master.ViewModels.SyukkasakiSearchModal
{
    public partial class SyukkasakiViewModel
    {
        public bool IsCheck { get; set; }

        /// <summary>
        /// 出荷先所在ID
        /// </summary>
        public string SHIP_TO_STORE_ID { get; set; }

        /// <summary>
        /// 出荷先所在名称
        /// </summary>
        public string SHIP_TO_STORE_NAME { get; set; }

        /// <summary>
        /// 出荷先店舗区分
        /// </summary>
        public string SHIP_TO_STORE_CLASS { get; set; }

        /// <summary>
        /// [エリアID]
        /// </summary>
        public string AREA_ID { get; set; }

        /// <summary>
        /// [都道府県名]
        /// </summary>
        public string PREF_NAME { get; set; }
    }
}