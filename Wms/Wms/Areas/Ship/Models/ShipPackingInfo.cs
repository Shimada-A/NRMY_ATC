namespace Wms.Areas.Ship.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Models;

    /// <summary>
    /// 出荷梱包実績
    /// </summary>
    [Table("T_SHIP_PACKING_INFO")]
    public partial class ShipPackingInfo : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　0905,0924,0942
        /// </remarks>
        [Key]
        [Column(Order = 14)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// ECフラグ (EC_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:BtoB　1:自社EC　（ecbeing)
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public bool EcFlag { get; set; }

        /// <summary>
        /// 梱包番号 (BOX_NO)
        /// </summary>
        /// <remarks>
        /// 庫内出荷ケースNo
        /// ec出荷でシングルの場合は送り状Noをセットする。
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(36, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// 出荷指示ID (SHIP_INSTRUCT_ID)
        /// </summary>
        /// <remarks>
        /// ec受注の場合は注文番号
        /// </remarks>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(30, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 出荷指示明細ID (SHIP_INSTRUCT_SEQ)
        /// </summary>
        [Key]
        [Column(Order = 13)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int ShipInstructSeq { get; set; }

        /// <summary>
        /// 送り状番号 (DELI_NO)
        /// </summary>
        /// <remarks>
        /// ※あんしん、ハイエス、ルート便は送り状発行しないため設定しない。
        /// 浪速の場合：子番号　（親番は　送り状番号2にいれる。）
        /// </remarks>
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DeliNo { get; set; }

        /// <summary>
        /// 送り状番号2 (DELI_NO2)
        /// </summary>
        /// <remarks>
        /// 浪速の送り状番号親番
        /// </remarks>
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DeliNo2 { get; set; }

        /// <summary>
        /// SKU (ITEM_SKU_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(30, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 品名 (ITEM_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        [MaxLength(13, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Jan { get; set; }

        /// <summary>
        /// 商品ID(品番) (ITEM_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// カラーID (ITEM_COLOR_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(5, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// サイズID (ITEM_SIZE_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(5, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// 実績数 (RESULT_QTY)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int ResultQty { get; set; }

        /// <summary>
        /// 出荷先店舗区分 (SHIP_TO_STORE_CLASS)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte ShipToStoreClass { get; set; }

        /// <summary>
        /// 出荷先店舗ID (SHIP_TO_STORE_ID)
        /// </summary>
        /// <remarks>
        /// 店舗、センター、他社EC倉庫　ID　個人向けの場合は空白
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// ECサイト区分 (EC_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:Amazon　・・・WEGO様の場合他社EC
        /// 2:Yahoo・・・WEGO様の場合他社EC
        /// 3:Rakuten・・・WEGO様の場合他社EC
        /// 4:Zozo・・・WEGO様の場合他社EC
        /// 5:SHOPLIST・・・WEGO様の場合他社EC
        /// 6:ecbeing　・・・WEGO様の場合自社EC
        /// 出荷テーブルからセットする
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte EcClass { get; set; }

        /// <summary>
        /// バッチNo (BATCH_NO)
        /// </summary>
        /// <remarks>
        /// 出荷指示ID,明細IDの出荷時バッチNo 
        /// １梱包には複数バッチNOの可能性あり、また、TC,DCが発生する可能性あり。TCは空白
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BatchNo { get; set; }

        /// <summary>
        /// 仕分種別 (ASORT_KIND)
        /// </summary>
        /// <remarks>
        /// 画面／ハンディ　画面のPC実績修正で梱包レコード追加された場合
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte AsortKind { get; set; }

        /// <summary>
        /// 仕分日 (ASORT_DATE)
        /// </summary>
        /// <remarks>
        /// ケースに商品をスキャンした日時（このレコードが作成された日）
        /// </remarks>
        public DateTime? AsortDate { get; set; }

        /// <summary>
        /// 仕分担当者 (ASORT_USER_ID)
        /// </summary>
        /// <remarks>
        /// ケースに商品をスキャンした担当者（このレコードを作成した人）
        /// </remarks>
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string AsortUserId { get; set; }

        /// <summary>
        /// 検品フラグ (KEN_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:検品必要未検品、1：検品中、2:不足確認中、8:検品不要、9：検品済
        /// ※EC外部倉庫と、ECオーダーピッキング、ほかマスタで設定されている検品必須倉庫の場合、レコード作成時点で0で登録される。※ecオーダーの場合：検品時にこのレコードが作成され検品済とする。その後納品書発行される。
        /// 上記以外は：納品書発行時レコード追加され、同時に検品不要でセットされる。
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte KenFlag { get; set; }

        /// <summary>
        /// 検品日 (KEN_DATE)
        /// </summary>
        public DateTime? KenDate { get; set; }

        /// <summary>
        /// 検品担当者 (KEN_USER_ID)
        /// </summary>
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string KenUserId { get; set; }

        /// <summary>
        /// 納品書発行フラグ (NOUHIN_PRN_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:発行必要未発行　1:再発行まち　8:発行不要（拠点間移動）9:発行済　
        /// 
        /// 拠点間移動の場合、発行しないため最初から8でセットする。
        /// その他BtoB　の場合１梱包で１納品書発行する梱包情報が変更となった場合、納品書の再発行が必要のため、いったん2となっても1となる。
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte NouhinPrnFlag { get; set; }

        /// <summary>
        /// 納品書発行日 (NOUHIN_PRN_DATE)
        /// </summary>
        /// <remarks>
        /// 発行時点の日付（再発行まちになってもクリアしない）
        /// </remarks>
        public DateTime? NouhinPrnDate { get; set; }

        /// <summary>
        /// 納品書発行担当者 (NOUHIN_PRN_USER_ID)
        /// </summary>
        /// <remarks>
        /// 発行時点の日付（再発行まちになってもクリアしない）
        /// </remarks>
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string NouhinPrnUserId { get; set; }

        /// <summary>
        /// 納品書番号 (NOUHIN_NO)
        /// </summary>
        /// <remarks>
        /// ec受注以外は、納品書No＝出荷ケースNOをセットする。
        /// ecbeingの場合出荷指示IDをセットする。
        /// </remarks>
        [MaxLength(30, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string NouhinNo { get; set; }

        /// <summary>
        /// 初期配送業者ID (INIT_TRANSPORTER_ID)
        /// </summary>
        /// <remarks>
        /// 出荷データ（EC出荷データ）の配送業者で作成する。
        /// 01：ヤマト運輸
        /// 02：佐川急便
        /// 03：浪速運送
        /// 04：東京納品代行
        /// 05：ハイエスサービス
        /// 06：ルート便 
        /// 07：あんしん物流
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InitTransporterId { get; set; }

        /// <summary>
        /// 配送業者ID (TRANSPORTER_ID)
        /// </summary>
        /// <remarks>
        /// レコード作成時は初期と同じ値。
        /// 配送業者変更された場合でこのテーブル作成後の場合は、初期と違う配送業者となる。
        /// 変更した場合は、送り状再発行必要。また、出荷指示の配送業者と違う場合もあり。
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterId { get; set; }

        /// <summary>
        /// 枝番 (DELI_SEQ)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? DeliSeq { get; set; }

        /// <summary>
        /// 送り状発行フラグ (DELI_PRN_FLAG)
        /// </summary>
        /// <remarks>
        /// 宅配伝票　0:未発行　1:発行済　2:再発行済

        /// 発行不要な配送業者の場合でも、納品書発行アクションでフラグを更新する。再発行済は、「再発行」した場合のみ（送り状発行済の配送業者変更時に送り状発行した場合は1とする）
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte DeliPrnFlag { get; set; }

        /// <summary>
        /// 送り状発行日 (DELI_PRN_DATE)
        /// </summary>
        /// <remarks>
        /// 東京納品代行：送り状に「納品日」として印字するリードタイムは印刷日とする
        /// </remarks>
        public DateTime? DeliPrnDate { get; set; }

        /// <summary>
        /// 送り状発行担当者 (DELI_PRN_USER_ID)
        /// </summary>
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DeliPrnUserId { get; set; }

        /// <summary>
        /// 確定フラグ (KAKU_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未確定、1：確定済
        /// PC出荷確定画面で1梱包NO単位に確定済にする。
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte KakuFlag { get; set; }

        /// <summary>
        /// 確定日 (KAKU_DATE)
        /// </summary>
        /// <remarks>
        /// 上位へは出庫日として送信する
        /// </remarks>
        public DateTime? KakuDate { get; set; }

        /// <summary>
        /// 確定担当者 (KAKU_USER_ID)
        /// </summary>
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string KakuUserId { get; set; }

        /// <summary>
        /// 連携処理中フラグ(基幹) (SENDING_FLAG_ERP)
        /// </summary>
        /// <remarks>
        /// 1:処理中（送信中）
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte SendingFlagErp { get; set; }

        /// <summary>
        /// 連携状況(基幹) (IF_STATE_ERP)
        /// </summary>
        /// <remarks>
        /// 上位システムへの送信
        /// 0:未送信 、2:送信済み
        /// 　・S作成処理でS作成時、送信済にする
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte IfStateErp { get; set; }

        /// <summary>
        /// 連携実行ID(基幹) (IF_RUN_ID_ERP)
        /// </summary>
        /// <remarks>
        /// 送信時　通信連携実行ID　（1送信処理単位）
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long IfRunIdErp { get; set; }

        /// <summary>
        /// 送信日 (SEND_DATE)
        /// </summary>
        /// <remarks>
        /// 出荷梱包実績送信日　YYYY/MM/DD hh:mm:ss
        /// </remarks>
        public DateTime? SendDate { get; set; }

        /// <summary>
        /// 連携状況(配送業者) (IF_STATE_TRP)
        /// </summary>
        /// <remarks>
        /// 配送業者への送信
        /// 0:未送信 、2:送信済み
        /// 　・S作成処理でS作成時、送信済にする
        /// 3:送信対象外
        ///    ・送信対象でない配送業者の場合
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte IfStateTrp { get; set; }

        /// <summary>
        /// 連携実行ID(配送業者) (IF_RUN_ID_TRP)
        /// </summary>
        /// <remarks>
        /// 送信時　通信連携実行ID　（1送信処理単位）
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long IfRunIdTrp { get; set; }

        /// <summary>
        /// 送信日(配送業者) (SEND_DATE_TRP)
        /// </summary>
        /// <remarks>
        /// 出荷梱包実績送信日　YYYY/MM/DD hh:mm:ss
        /// </remarks>
        public DateTime? SendDateTrp { get; set; }

        /// <summary>
        /// 箱サイズ (BOX_SIZE)
        /// </summary>
        /// <remarks>
        /// ヤマトの場合の箱サイズ（送り状発行で画面で指定するEDI送信用項目）
        /// 0101:宅急便発払い60サイズ
        /// 0102:宅急便発払い80サイズ
        /// 0103:宅急便発払い100サイズ
        /// </remarks>
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BoxSize { get; set; }

        /// <summary>
        /// 個口数 (UNIT_CNT)
        /// </summary>
        /// <remarks>
        /// EC佐川の場合のみ有効。個口数　1~最大　99999。
        /// GASからの梱包実績が1ケースNOのみとなったため、納品書発行時個口数を登録することとなった。（備考参照）
        /// セットされるのはEC納品書発行時
        /// シングル：1固定
        /// GAS：送り状発行時、指定個口数で印字時または再発行で個口数変更された場合
        /// オーダー：最初のケースで送り状発行時、指定個口で印字時。または再発行で個口数変更された場合
        /// </remarks>
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? UnitCnt { get; set; }

        /// <summary>
        /// 配送業者仕分コード (DELI_SHIWAKE_CD)
        /// </summary>
        /// <remarks>
        /// レコード作成時、出荷データ、EC出荷データからセットする。
        /// タ。配送業者変更画面で変更されると、更新される。
        /// 仕分けコード不要な業者の場合は「@@@」設定
        /// 必ず設定されるので空白のデータは発生しないはず
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DeliShiwakeCd { get; set; }

        #endregion プロパティ
    }
}