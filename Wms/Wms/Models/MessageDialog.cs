namespace Wms.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using Share.Common.Resources;
    using Wms.Common;
    using Wms.Common.Resources;

    /// <summary>
    /// プログラム
    /// </summary>
    public class MessageDialog
    {
        #region プロパティ

        /// <summary>
        /// Dialogのid
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 確認メッセージ
        /// </summary>
        public string Message { get; set; }
            #endregion
    }
}
