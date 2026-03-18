namespace Wms.ViewModels.Home
{
    public class Message
    {
        /// <summary>
        /// メッセージ区分1:エラー2:警告3:お知らせ
        /// </summary>
        public string MessageClass { get; set; }

        /// <summary>
        /// 表示メッセージ
        /// </summary>
        public string DispMessage { get; set; }

    }
}