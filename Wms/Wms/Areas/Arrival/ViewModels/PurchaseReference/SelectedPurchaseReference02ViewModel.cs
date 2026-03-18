namespace Wms.Areas.Arrival.ViewModels.PurchaseReference
{
    /// <summary>
    /// Store list country which is posted from view
    /// </summary>
    public class SelectedPurchaseReference02ViewModel
    {
        /// <summary>
        /// Checkbox Delete Checked
        /// </summary>
        public bool IsCheck { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        public long LineNo { get; set; }
    }
}