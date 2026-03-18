namespace Wms.Areas.Returns.ViewModels.PurchaseReturnReference
{
    /// <summary>
    /// Store list country which is posted from view
    /// </summary>
    public class SelectedPurchaseReturnReference02ViewModel
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