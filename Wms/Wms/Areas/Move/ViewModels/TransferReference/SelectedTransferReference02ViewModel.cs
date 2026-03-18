namespace Wms.Areas.Move.ViewModels.TransferReference
{
    /// <summary>
    /// Store list country which is posted from view
    /// </summary>
    public class SelectedTransferReference02ViewModel
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