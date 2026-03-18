namespace Wms.Areas.Master.ViewModels.General
{
    /// <summary>
    /// Store list country which is posted from view
    /// </summary>
    public class GeneralInput
    {
        public Models.General General { get; set; }

        public string InUpDiff { get; set; }

        /// <summary>
        /// 検索済み判断フラグ
        /// </summary>
        public bool SearchFlag { get; set; }
    }
}