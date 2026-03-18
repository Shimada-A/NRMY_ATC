namespace Wms.ViewModels.Home
{
    using System.Collections.Generic;

    public class Index
    {
        /// <summary>
        /// お知らせ
        /// </summary>
        public List<Message> Message { get; set; }

        /// <summary>
        /// 入荷進捗
        /// </summary>
        public Arrive Arrive { get; set; }

        /// <summary>
        /// 出荷進捗
        /// </summary>
        public Ship Ship { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Index"/> class.
        /// </summary>
        public Index()
        {
            this.Message = new List<Message>();
            this.Arrive = new Arrive();
            this.Ship = new Ship();
        }

        /// <summary>
        /// Gets or sets 自動更新切替間隔
        /// </summary>
        public int UpdTime { get; set; }

        public string TcDcKbn { get; set; }
    }
}