namespace Wms.Areas.Ship.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Models;

    /// <summary>
    /// EC出荷
    /// </summary>
    [Table("T_ECSHIPS")]
    public partial class Ecship : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 連携実行ID (IF_RUN_ID)
        /// </summary>
        /// <remarks>
        /// 受信時通信連携実行ID　（1受信処理単位）
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long IfRunId { get; set; }

        /// <summary>
        /// システム区分 (SYSTEM_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:基幹　3:WNS画面
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte SystemClass { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　0905,0924,0942
        /// </remarks>
        [Key]
        [Column(Order = 13)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 送信処理区分 (SEND_CLASS)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte SendClass { get; set; }

        /// <summary>
        /// 出荷指示ID (SHIP_INSTRUCT_ID)
        /// </summary>
        /// <remarks>
        /// ECbeing注文番号(オーダーID）　ECyymmdd-MMMMMDDRR
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(30, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 出荷指示明細ID (SHIP_INSTRUCT_SEQ)
        /// </summary>
        /// <remarks>
        /// 行番号
        /// </remarks>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int ShipInstructSeq { get; set; }

        /// <summary>
        /// 注文日時 (ORDER_DATE)
        /// </summary>
        /// <remarks>
        /// YYYY/MM/DD hh:mm:ss
        /// </remarks>
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// キャンセルフラグ (CANCEL_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:なし 1:キャンセル
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public bool CancelFlag { get; set; }

        /// <summary>
        /// キャンセル日時 (CANCEL_DATE)
        /// </summary>
        public DateTime? CancelDate { get; set; }

        /// <summary>
        /// 出荷指定日 (SHIP_REQUEST_DATE)
        /// </summary>
        /// <remarks>
        /// 出荷予定日　YYYY/MM/DD
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public DateTime ShipRequestDate { get; set; }

        /// <summary>
        /// 着荷希望日 (ARRIVE_REQUEST_DATE)
        /// </summary>
        /// <remarks>
        /// YYYY/MM/DD
        /// </remarks>
        public DateTime? ArriveRequestDate { get; set; }

        /// <summary>
        /// 着荷希望時刻条件 (ARRIVE_REQUEST_TIME)
        /// </summary>
        /// <remarks>
        /// 【佐川】（ダイアログさんから2019/3/18　メール）
        /// 00:指定なし及び範囲外の時 
        /// 　　　※受信データがnullの場合は00でセットする。
        /// 01:午前中
        /// 12:12:00～14:00
        /// 14:14:00～16:00
        /// 16:16:00～18:00
        /// 18:18:00～20:00
        /// 19:19:00～21:00
        /// 04:18:00～21:00
        /// </remarks>
        [MaxLength(4, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ArriveRequestTime { get; set; }

        /// <summary>
        /// 即日配達区分 (SAME_DAY_DELIVERY_CLASS)
        /// </summary>
        /// <remarks>
        /// 0：通常
        /// 1：当日配達希望　
        /// 2：翌日配達希望
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public bool SameDayDeliveryClass { get; set; }

        /// <summary>
        /// 配送業者ID (TRANSPORTER_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterId { get; set; }

        /// <summary>
        /// 配送方法名 (DELIVERY_METHOD_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DeliveryMethodName { get; set; }

        /// <summary>
        /// 配送メモ (DELIVERY_NOTE)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DeliveryNote { get; set; }

        /// <summary>
        /// 届先名前 (DEST_FIRST_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestFirstName { get; set; }

        /// <summary>
        /// 届先名前カナ (DEST_FIRST_KANA_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestFirstKanaName { get; set; }

        /// <summary>
        /// 届先名字 (DEST_FAMILY_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestFamilyName { get; set; }

        /// <summary>
        /// 届先名字カナ (DEST_FAMILY_KANA_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestFamilyKanaName { get; set; }

        /// <summary>
        /// 届先国 (DEST_COUNTRY)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestCountry { get; set; }

        /// <summary>
        /// 届先郵便番号 (DEST_ZIP)
        /// </summary>
        /// <remarks>
        /// 受信時、ハイフン削除する！仕分コード設定画面で更新される可能性あり。
        /// </remarks>
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestZip { get; set; }

        /// <summary>
        /// 届先都道府県 (DEST_PREF_NAME)
        /// </summary>
        /// <remarks>
        /// ecbeingからのデータはnvarchar(24)
        /// </remarks>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestPrefName { get; set; }

        /// <summary>
        /// 届先都道府県カナ (DEST_PREF_KANA_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestPrefKanaName { get; set; }

        /// <summary>
        /// 届先市区町村 (DEST_CITY_NAME)
        /// </summary>
        /// <remarks>
        /// ecbeingからのJETデータは住所nvarchar(80)
        /// </remarks>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestCityName { get; set; }

        /// <summary>
        /// 届先市区町村カナ (DEST_CITY_KANA_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestCityKanaName { get; set; }

        /// <summary>
        /// 届先それ以降の住所1 (DEST_ADDRESS1)
        /// </summary>
        /// <remarks>
        /// ecbeingからのデータは住所2nvarchar(80)
        /// </remarks>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestAddress1 { get; set; }

        /// <summary>
        /// 届先それ以降の住所1カナ (DEST_KANA_ADDRESS1)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestKanaAddress1 { get; set; }

        /// <summary>
        /// 届先それ以降の住所2 (DEST_ADDRESS2)
        /// </summary>
        /// <remarks>
        /// ecbeingからのデータは住所3nvarchar(80)
        /// </remarks>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestAddress2 { get; set; }

        /// <summary>
        /// 届先それ以降の住所2カナ (DEST_KANA_ADDRESS2)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestKanaAddress2 { get; set; }

        /// <summary>
        /// 届先それ以降の住所3 (DEST_ADDRESS3)
        /// </summary>
        /// <remarks>
        /// ecbeingからのデータはなし
        /// </remarks>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestAddress3 { get; set; }

        /// <summary>
        /// 届先それ以降の住所3カナ (DEST_KANA_ADDRESS3)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestKanaAddress3 { get; set; }

        /// <summary>
        /// 届先会社 (DEST_COMPANY)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestCompany { get; set; }

        /// <summary>
        /// 届先部署 (DEST_POST)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestPost { get; set; }

        /// <summary>
        /// 届先電話番号 (DEST_TEL)
        /// </summary>
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestTel { get; set; }

        /// <summary>
        /// 届先コンビニコード (DEST_CONVENIENCE_CODE)
        /// </summary>
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestConvenienceCode { get; set; }

        /// <summary>
        /// 届先ストア分類コード (DEST_STORE_GROUP_CODE)
        /// </summary>
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestStoreGroupCode { get; set; }

        /// <summary>
        /// 届先ストアコード (DEST_STORE_CODE)
        /// </summary>
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestStoreCode { get; set; }

        /// <summary>
        /// 届先発注エリアコード (DEST_ORDER_AREA_CODE)
        /// </summary>
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestOrderAreaCode { get; set; }

        /// <summary>
        /// 届先センターデポコード (DEST_CENTER_DEPOT_CODE)
        /// </summary>
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestCenterDepotCode { get; set; }

        /// <summary>
        /// 届先開店時間 (DEST_OPEN_TIME)
        /// </summary>
        [MaxLength(4, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestOpenTime { get; set; }

        /// <summary>
        /// 届先閉店時間 (DEST_CLOSE_TIME)
        /// </summary>
        [MaxLength(4, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestCloseTime { get; set; }

        /// <summary>
        /// 届先特記事項 (DEST_NOTICES)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestNotices { get; set; }

        /// <summary>
        /// 依頼主名前 (CLIENT_FIRST_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientFirstName { get; set; }

        /// <summary>
        /// 依頼主名前カナ (CLIENT_FIRST_KANA_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientFirstKanaName { get; set; }

        /// <summary>
        /// 依頼主名字 (CLIENT_FAMILY_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientFamilyName { get; set; }

        /// <summary>
        /// 依頼主名字カナ (CLIENT_FAMILY_KANA_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientFamilyKanaName { get; set; }

        /// <summary>
        /// 依頼主国 (CLIENT_COUNTRY)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientCountry { get; set; }

        /// <summary>
        /// 依頼主郵便番号 (CLIENT_ZIP)
        /// </summary>
        /// <remarks>
        /// 受信時、ハイフン削除する！
        /// </remarks>
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientZip { get; set; }

        /// <summary>
        /// 依頼主都道府県 (CLIENT_PREF_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientPrefName { get; set; }

        /// <summary>
        /// 依頼主都道府県カナ (CLIENT_PREF_KANA_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientPrefKanaName { get; set; }

        /// <summary>
        /// 依頼主市区町村 (CLIENT_CITY_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientCityName { get; set; }

        /// <summary>
        /// 依頼主市区町村カナ (CLIENT_CITY_KANA_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientCityKanaName { get; set; }

        /// <summary>
        /// 依頼主それ以降の住所1 (CLIENT_ADDRESS1)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientAddress1 { get; set; }

        /// <summary>
        /// 依頼主それ以降の住所1カナ (CLIENT_KANA_ADDRESS1)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientKanaAddress1 { get; set; }

        /// <summary>
        /// 依頼主それ以降の住所2 (CLIENT_ADDRESS2)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientAddress2 { get; set; }

        /// <summary>
        /// 依頼主それ以降の住所2カナ (CLIENT_KANA_ADDRESS2)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientKanaAddress2 { get; set; }

        /// <summary>
        /// 依頼主それ以降の住所3 (CLIENT_ADDRESS3)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientAddress3 { get; set; }

        /// <summary>
        /// 依頼主それ以降の住所3カナ (CLIENT_KANA_ADDRESS3)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientKanaAddress3 { get; set; }

        /// <summary>
        /// 依頼主会社 (CLIENT_COMPANY)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientCompany { get; set; }

        /// <summary>
        /// 依頼主部署 (CLIENT_POST)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientPost { get; set; }

        /// <summary>
        /// 依頼主電話番号 (CLIENT_TEL)
        /// </summary>
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientTel { get; set; }

        /// <summary>
        /// 贈り主名前 (PRESENTER_FIRST_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PresenterFirstName { get; set; }

        /// <summary>
        /// 贈り主名前カナ (PRESENTER_FIRST_KANA_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PresenterFirstKanaName { get; set; }

        /// <summary>
        /// 贈り主名字 (PRESENTER_FAMILY_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PresenterFamilyName { get; set; }

        /// <summary>
        /// 贈り主名字カナ (PRESENTER_FAMILY_KANA_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PresenterFamilyKanaName { get; set; }

        /// <summary>
        /// 贈り主国 (PRESENTER_COUNTRY)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PresenterCountry { get; set; }

        /// <summary>
        /// 贈り主郵便番号 (PRESENTER_ZIP)
        /// </summary>
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PresenterZip { get; set; }

        /// <summary>
        /// 贈り主都道府県 (PRESENTER_PREF_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PresenterPrefName { get; set; }

        /// <summary>
        /// 贈り主都道府県カナ (PRESENTER_PREF_KANA_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PresenterPrefKanaName { get; set; }

        /// <summary>
        /// 贈り主市区町村 (PRESENTER_CITY_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PresenterCityName { get; set; }

        /// <summary>
        /// 贈り主市区町村カナ (PRESENTER_CITY_KANA_NAME)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PresenterCityKanaName { get; set; }

        /// <summary>
        /// 贈り主それ以降の住所1 (PRESENTER_ADDRESS1)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PresenterAddress1 { get; set; }

        /// <summary>
        /// 贈り主それ以降の住所1カナ (PRESENTER_KANA_ADDRESS1)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PresenterKanaAddress1 { get; set; }

        /// <summary>
        /// 贈り主それ以降の住所2 (PRESENTER_ADDRESS2)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PresenterAddress2 { get; set; }

        /// <summary>
        /// 贈り主それ以降の住所2カナ (PRESENTER_KANA_ADDRESS2)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PresenterKanaAddress2 { get; set; }

        /// <summary>
        /// 贈り主それ以降の住所3 (PRESENTER_ADDRESS3)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PresenterAddress3 { get; set; }

        /// <summary>
        /// 贈り主それ以降の住所3カナ (PRESENTER_KANA_ADDRESS3)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PresenterKanaAddress3 { get; set; }

        /// <summary>
        /// 贈り主会社 (PRESENTER_COMPANY)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PresenterCompany { get; set; }

        /// <summary>
        /// 贈り主部署 (PRESENTER_POST)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PresenterPost { get; set; }

        /// <summary>
        /// 贈り主電話番号 (PRESENTER_TEL)
        /// </summary>
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PresenterTel { get; set; }

        /// <summary>
        /// 販売通貨 (SALE_CURRENCY)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string SaleCurrency { get; set; }

        /// <summary>
        /// 原価合計金額 (TOTAL_COST_AMOUNT)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int TotalCostAmount { get; set; }

        /// <summary>
        /// 商品合計金額 (TOTAL_ITEM_AMOUNT)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int TotalItemAmount { get; set; }

        /// <summary>
        /// 税額合計金額 (TOTAL_TAX_AMOUNT)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int TotalTaxAmount { get; set; }

        /// <summary>
        /// 送料合計金額 (CARRIAGE_AMOUNT)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int CarriageAmount { get; set; }

        /// <summary>
        /// 送料合計金額(税抜) (CARRIAGE_AMOUNT_EX_TAX)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int CarriageAmountExTax { get; set; }

        /// <summary>
        /// 手数料合計金額 (COMMISSION_AMOUNT)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int CommissionAmount { get; set; }

        /// <summary>
        /// 手数料合計金額(税抜) (COMMISSION_AMOUNT_EX_TAX)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int CommissionAmountExTax { get; set; }

        /// <summary>
        /// のし・ラッピング合計金額 (NOSHI_AMOUNT)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int NoshiAmount { get; set; }

        /// <summary>
        /// のし・ラッピング合計金額(税抜) (NOSHI_AMOUNT_EX_TAX)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int NoshiAmountExTax { get; set; }

        /// <summary>
        /// ポイント値引き金額 (POINT_DISCOUNT_AMOUNT)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int PointDiscountAmount { get; set; }

        /// <summary>
        /// クーポン値引き金額 (COUPON_DISCOUNT_AMOUNT)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int CouponDiscountAmount { get; set; }

        /// <summary>
        /// キャンペーン値引き金額 (CAMPAIGN_DISCOUNT_AMOUNT)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int CampaignDiscountAmount { get; set; }

        /// <summary>
        /// その他値引き金額 (ETC_DISCOUNT_AMOUNT)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int EtcDiscountAmount { get; set; }

        /// <summary>
        /// 割引合計金額 (DISCOUNT_AMOUNT)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int DiscountAmount { get; set; }

        /// <summary>
        /// 請求合計金額 (TOTAL_CLAIM_AMOUNT)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int TotalClaimAmount { get; set; }

        /// <summary>
        /// 税率 (TAX_RATE)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999, 999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int TaxRate { get; set; }

        /// <summary>
        /// 支払方法 (PAYMENT_METHOD)
        /// </summary>
        /// <remarks>
        /// ecbeingの場合
        /// ・支払い金額なし
        /// ・代金引換
        /// ・クレジットカード
        /// ・Paidy翌月払い(コンビニ/銀行)
        /// ・d払い
        /// ・auかんたん決済／au WALLET
        /// ・softbankまとめて決済
        /// ・AmazonPay決済
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PaymentMethod { get; set; }

        /// <summary>
        /// ギフト配送希望フラグ (GIFT_DELIVERY_FLAG)
        /// </summary>
        /// <remarks>
        /// 0：ギフトでない  
        /// 1：ギフト
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public bool GiftDeliveryFlag { get; set; }

        /// <summary>
        /// ラッピング方法 (WRAPPING_METHOD)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte WrappingMethod { get; set; }

        /// <summary>
        /// ギフト付随情報 (GIFT_INFORMATION)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string GiftInformation { get; set; }

        /// <summary>
        /// 備考 (NOTE)
        /// </summary>
        [MaxLength(1000, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Note { get; set; }

        /// <summary>
        /// ポイント発生 (POINT_IN)
        /// </summary>
        /// <remarks>
        /// 獲得したポイント
        /// </remarks>
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? PointIn { get; set; }

        /// <summary>
        /// 会員番号 (MEMBERS_ID)
        /// </summary>
        /// <remarks>
        /// CRM管理をしている場合の会員番号、登録がないときは空
        /// </remarks>
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string MembersId { get; set; }

        /// <summary>
        /// 依頼主メールアドレス (CLIENT_MAIL_ADDRESS)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientMailAddress { get; set; }

        /// <summary>
        /// 性別 (SEXUALITY)
        /// </summary>
        /// <remarks>
        /// ０：未設定、１：男性、２：女性
        /// </remarks>
        [MaxLength(1, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Sexuality { get; set; }

        /// <summary>
        /// SKU (ITEM_SKU_ID)
        /// </summary>
        /// <remarks>
        /// 品番＋カラーID＋サイズID　　　これ以降明細情報
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(30, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        /// <remarks>
        /// 受信時品番SKUから設定する。
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
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
        /// 品名 (ITEM_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// 標準上代(税抜) (NORMAL_SELLING_PRICE_EX_TAX)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int NormalSellingPriceExTax { get; set; }

        /// <summary>
        /// 標準下代 (NORMAL_BUYING_PRICE)
        /// </summary>
        /// <remarks>
        /// 商品原価
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999.99, 999999999.99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public decimal NormalBuyingPrice { get; set; }

        /// <summary>
        /// 仕入下代 (PURCHASE_BUYING_PRICE)
        /// </summary>
        /// <remarks>
        /// 商品原価＋輸入コスト
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999.99, 999999999.99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public decimal PurchaseBuyingPrice { get; set; }

        /// <summary>
        /// 販売単価(税抜) (SALE_UNIT_PRICE_EX_TAX)
        /// </summary>
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? SaleUnitPriceExTax { get; set; }

        /// <summary>
        /// 販売単価(税込) (SALE_UNIT_PRICE)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int SaleUnitPrice { get; set; }

        /// <summary>
        /// 数量 (ORDER_QTY)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int OrderQty { get; set; }

        /// <summary>
        /// 金額 (ITEM_AMOUNT)
        /// </summary>
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? ItemAmount { get; set; }

        /// <summary>
        /// 明細備考 (DETAIL_NOTE)
        /// </summary>
        [MaxLength(1000, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DetailNote { get; set; }

        /// <summary>
        /// 都道府県コード (PREF_ID)
        /// </summary>
        /// <remarks>
        /// 受信時　都道府県マスタからセットする
        /// </remarks>
        [MaxLength(2, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PrefId { get; set; }

        /// <summary>
        /// 配送業者仕分コード (DELI_SHIWAKE_CD)
        /// </summary>
        /// <remarks>
        /// 受信時、配送業者別に設定する。受信時設定できない場合、空白セットする。
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DeliShiwakeCd { get; set; }

        /// <summary>
        /// 代引フラグ (DAIBIKI_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:代引き以外、1:代引き　　支払方法が"代金引換"の場合　1
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public bool DaibikiFlag { get; set; }

        /// <summary>
        /// EC出荷形態 (EC_SHIP_CLASS)
        /// </summary>
        /// <remarks>
        /// EC出荷形態　0:自社EC以外、
        /// 1:ecシングルピック(1オーダー1ピース）
        /// 2:ecオーダー（容積オーバー）
        /// 3:ecGAS：上記以外
        /// 
        /// </remarks>
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte? EcShipClass { get; set; }

        /// <summary>
        /// 単品容積 (PIECE_VOL)
        /// </summary>
        /// <remarks>
        /// 受信時品番SKUマスタから設定する。（単品容積　単位はCBM(立法メートル）仕入梱包実績受信でWMSでマスタに設定する。）
        /// </remarks>
        [Range(-99999999999.99999999, 99999999999.99999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public decimal? PieceVol { get; set; }

        /// <summary>
        /// 引当後キャンセルフラグ (AFT_ALLOC_CANCEL_FLAG)
        /// </summary>
        /// <remarks>
        /// 1:引当後キャンセルデータ受信
        /// 受信日もセット。
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte AftAllocCancelFlag { get; set; }

        /// <summary>
        /// 引当後キャンセル受信日 (AFT_ALLOC_CANCEL_DATE)
        /// </summary>
        /// <remarks>
        /// 上記受信日
        /// </remarks>
        public DateTime? AftAllocCancelDate { get; set; }

        /// <summary>
        /// 引当後更新ありフラグ (AFT_ALLOC_UP_FLAG)
        /// </summary>
        /// <remarks>
        /// １：引当後同一出荷指示IDでキャンセル以外のデータを受信した。（引当前はEC取消累積に移動されている）データ自体はそままで更新しない！受信した更新データはEC更新待ちテーブルへ登録されている。）
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte AftAllocUpFlag { get; set; }

        /// <summary>
        /// 引当後更新データ受信日 (AFT_ALLOC_UP_DATE)
        /// </summary>
        /// <remarks>
        /// 上記受信日
        /// </remarks>
        public DateTime? AftAllocUpDate { get; set; }

        /// <summary>
        /// 引当日 (ALLOC_DATE)
        /// </summary>
        /// <remarks>
        /// 初回引当日時（１行単位で設定する？）
        /// </remarks>
        public DateTime? AllocDate { get; set; }

        /// <summary>
        /// 再引当日 (RE_ALLOC_DATE)
        /// </summary>
        /// <remarks>
        /// 再引当日時（１行単位で設定する？）
        /// </remarks>
        public DateTime? ReAllocDate { get; set; }

        /// <summary>
        /// 引当フラグ (ALLOC_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未引当、1:引当済　
        /// ※出荷指示ID単位で設定する。引き当てエラーが発生していても、それは引当フラグ=1にする　引当フラグ=1でバッチNoなし＝引当エラー発生で作業されていないもの。
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public bool AllocFlag { get; set; }

        /// <summary>
        /// 引当エラーフラグ (ALLOC_ERR_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未引き当てorエラーなし　1:引当エラー　（引き当てエラーがある場合、でも、出荷指示ID単位で1ピース以上引当があれば、バッチNO設定される。）
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public bool AllocErrFlag { get; set; }

        /// <summary>
        /// 引当数 (ALLOC_QTY)
        /// </summary>
        /// <remarks>
        /// 引当数（引当エラー発生しても、引当済数分だけ引き当てされている）
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int AllocQty { get; set; }

        /// <summary>
        /// バッチNo (BATCH_NO)
        /// </summary>
        /// <remarks>
        /// 作業OK（引当エラーなし）となった出荷指示ID（＝注文）のみ出荷指示ID単位でバッチNo設定される。初期値は半角空白
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BatchNo { get; set; }

        /// <summary>
        /// GASバッチNo (GAS_BATCH_NO)
        /// </summary>
        /// <remarks>
        /// EC　GAS使用の場合のGASバッチNo
        /// </remarks>
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string GasBatchNo { get; set; }

        /// <summary>
        /// GAS間口No (GAS_MAGUCHI_NO)
        /// </summary>
        /// <remarks>
        /// 1GAS使用の場合の１GASバッチNO内の間口No
        /// </remarks>
        [Range(-999, 999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? GasMaguchiNo { get; set; }

        /// <summary>
        /// 出荷実績数 (RESULT_QTY)
        /// </summary>
        /// <remarks>
        /// 出荷実績数。引当数以上には設定できない。
        /// ・ECｼﾝｸﾞﾙ：納品書発行時セットされる。
        /// ・ECｵｰﾀﾞｰ：ｵｰﾀﾞｰ検品時セットされる。
        /// ・ECGAS：納品書発行時セットされる。
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int ResultQty { get; set; }

        /// <summary>
        /// 欠品数 (STOCKOUT_QTY)
        /// </summary>
        /// <remarks>
        /// 欠品数（数量-実績　数　で設定されてること）セットタイミングは上記同様
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int StockoutQty { get; set; }

        /// <summary>
        /// 出荷確定フラグ (KAKU_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未確定　1:確定済
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public bool KakuFlag { get; set; }

        /// <summary>
        /// 出荷確定日 (KAKU_DATE)
        /// </summary>
        /// <remarks>
        /// 上位へは出荷日として送信する
        /// </remarks>
        public DateTime? KakuDate { get; set; }

        /// <summary>
        /// 出荷確定担当者 (KAKU_USER_ID)
        /// </summary>
        /// <remarks>
        /// キャンセル受信処理で自動で確定した場合は受信処理
        /// </remarks>
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string KakuUserId { get; set; }

        /// <summary>
        /// 連携処理中フラグ(基幹) (SENDING_FLAG_ERP)
        /// </summary>
        /// <remarks>
        /// 1:処理中（送信中）
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        public bool SendingFlagErp { get; set; }

        /// <summary>
        /// 連携状況(基幹) (IF_STATE_ERP)
        /// </summary>
        /// <remarks>
        /// 0:未設定
        /// 2:送信済み
        /// 　・S作成処理でS作成時、送信済にする
        /// 3:送信対象外
        ///    ・送信対象でないシステムの場合
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
        /// 出荷実績送信日　YYYY/MM/DD hh:mm:ss
        /// </remarks>
        public DateTime? SendDate { get; set; }

        /// <summary>
        /// 引当グループNo (ALLOC_GROUP_NO)
        /// </summary>
        /// <remarks>
        /// １度の引当で複数のバッチNoが作成された場合は先頭の引当NoをバッチグループNoとしてセットする。1バッチのみの場合はバッチNoと同じ。引当エラーが発生した場合はクリアされている！
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string AllocGroupNo { get; set; }

        /// <summary>
        /// ECサイト区分 (EC_CLASS)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte EcClass { get; set; }

        /// <summary>
        /// 関連注文番号 (RELATED_ORDER_NO)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string RelatedOrderNo { get; set; }

        #endregion
    }
}