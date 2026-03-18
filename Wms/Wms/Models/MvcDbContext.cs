namespace Wms.Models
{
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.Web;
    using EntityFramework.OracleHelpers;
    using Glimpse.Ado.AlternateType;
    using Oracle.ManagedDataAccess.Client;
    using Wms.Areas.Arrival.Models;
    using Wms.Areas.Inventory.Models;
    using Wms.Areas.Log.Models;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Move.Models;
    using Wms.Areas.Ship.Models;
    using Wms.Areas.Stock.Models;
    using Wms.Areas.Returns.Models;
    using Wms.Areas.Ship.Models;

    /// <summary>
    /// DBコンテキスト
    /// </summary>
    public class MvcDbContext : DbContext
    {
        #region プロパティ

        // マスター
        public DbSet<EcPrefTransporter> EcPrefTransporter { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Program> Programs { get; set; }

        public DbSet<UserProgram> UserPrograms { get; set; }

        public DbSet<Transporter> Transporters { get; set; }

        public DbSet<Warehouses> Warehouses { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<Paging> Pagings { get; set; }

        public DbSet<ItemSku> ItemSkus { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Vendor> Vendors { get; set; }

        public DbSet<General> Generals { get; set; }

        public DbSet<ItemCategories4> ItemCategories4 { get; set; }

        public DbSet<LocTransporter> LocTransporters { get; set; }

        public DbSet<ConsignorsSagawa> ConsignorsSagawas { get; set; }

        public DbSet<ConsignorsNaniwa> ConsignorsNaniwa { get; set; }

        public DbSet<BoxSetting> BoxSettings { get; set; }

        public DbSet<ShipFrontage> ShipFrontages { get; set; }

        public DbSet<MasShipFrontage> MasShipFrontages { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<MasLocation> MasLocations { get; set; }

        public DbSet<MasBoxSetting> MasBoxSettings { get; set; }

        public DbSet<Division> Divisions { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Operations> Operations { get; set; }

        public DbSet<BrandStore> BrandStores { get; set; }

        public DbSet<OperationNotes> OperationNotes { get; set; }

        public DbSet<LocationClasses> LocationClasses { get; set; }

        public DbSet<LocationClassGrade> LocationClassGrade { get; set; }

        public DbSet<Grade> Grades { get; set; }

        public DbSet<NaniwaSorting> NaniwaSorting { get; set; }

        public DbSet<ConsignorsWorld> ConsignorsWorld { get; set; }

        public DbSet<StkStock01> StkStock01s { get; set; }

        public DbSet<StkStock02> StkStock02s { get; set; }

        public DbSet<StockingPattern> StockingPatterns { get; set; }

        public DbSet<DeliareaGroup> DeliareaGroup { get; set; }

        public DbSet<SystemLog> SystemLogs { get; set; }

        public DbSet<MasLocTransporter> MasLocTransporters { get; set; }

        public DbSet<Pref> Prefs { get; set; }

        public DbSet<ArrPurRef01> ArrPurRef01s { get; set; }

        public DbSet<ArrPurRef02> ArrPurRef02s { get; set; }

        public DbSet<ArrConAct01> ArrConAct01s { get; set; }

        public DbSet<InventoryPlan> InventoryPlans { get; set; }

        public DbSet<ArriveResult> ArriveResults { get; set; }

        public DbSet<RetEcReference> RetEcReference01s { get; set; }

        public DbSet<ArrInputPurchase01> ArrInputPurchase01s { get; set; }

        public DbSet<ArrInputPurchase02> ArrInputPurchase02s { get; set; }

        public DbSet<ShpDcAllocation> ShpDcAllocations { get; set; }

        public DbSet<ShipToStores> ShipToStores { get; set; }
        
        public DbSet<AllocInfo> AllocInfos { get; set; }

        public DbSet<StkAdjust01> StkAdjust01s { get; set; }

        public DbSet<StkAdjust02> StkAdjust02s { get; set; }

        public DbSet<StkMove> StkLocMoves { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<PackageStock> PackageStocks { get; set; }

        public DbSet<MovTransRef01> MovTransRef01s { get; set; }

        public DbSet<MovTransRef02> MovTransRef02s { get; set; }

        public DbSet<MovTransInput01> MovTransInput01s { get; set; }

        public DbSet<MovTransInput02> MovTransInput02s { get; set; }

        public DbSet<ShpEcAllocation> ShpEcAllocations { get; set; }

        public DbSet<ShpModTcInstruction> ShpModTcInstructions { get; set; }

        public DbSet<ShpBtoBReference> ShpBtoBReference01s { get; set; }

        public DbSet<ShpSortingChng> ShpSortingChngs { get; set; }

        public DbSet<ShpBtoBInstructionReference> ShpBtoBInstructionReferences { get; set; }

        public DbSet<ShpBtoBInstructionInput> ShpBtoBInstructionInputs { get; set; }

        public DbSet<AllocStatus> AllocStatus { get; set; }

        public DbSet<ShpEcConfirmProgress> ShpEcConfirmProgress { get; set; }

        public DbSet<InventoryConfirm> InventoryConfirms { get; set; }

        public DbSet<InvStart_01> InvStart_01s { get; set; }

        public DbSet<InvInput01> InvInput01s { get; set; }

        public DbSet<InvReference01> InvReference01s { get; set; }

        public DbSet<InvReference02> InvReference02s { get; set; }

        public DbSet<InvReference03> InvReference03s { get; set; }

        public DbSet<InvReference04> InvReference04s { get; set; }

        public DbSet<ShpTransporterChng> ShpTransporterChngs { get; set; }

        public DbSet<ShpTransporterChngEc> ShpTransporterChngEcs { get; set; }

        public DbSet<ShipPackingInfo> ShipPackingInfoes { get; set; }

        public DbSet<EcunitSort> EcunitSorts { get; set; }

        public DbSet<Ecship> Ecships { get; set; }

        public DbSet<SortStockInstructs01> SortStockInstructs01s { get; set; }

        public DbSet<SortStockInstructs02> SortStockInstructs02s { get; set; }

        public DbSet<SortStockResult> SortStockResults { get; set; }

        public DbSet<ShpDelInstruction> ShpDelInstructions { get; set; }

        public DbSet<ShpCaseInstruction> ShpCaseInstructions { get; set; }

        public DbSet<Objects> Objects { get; set; }

        public DbSet<ObjectDetail> ObjectDetail { get; set; }

        public DbSet<Layout> Layout { get; set; }

        public DbSet<LayoutCondition> LayoutCondition { get; set; }

        public DbSet<LayoutDetail> LayoutDetail { get; set; }

        /// <summary>
        /// Oracleコネクション
        /// </summary>
        /// <remarks>Oracle独自プロパティを触る必要があるときのみ使用してください</remarks>
        public static OracleConnection OracleConnection
        {
            get
            {
                var glimpseDbConnection = Current.Database.Connection as GlimpseDbConnection;
                return glimpseDbConnection.InnerConnection as OracleConnection;
            }
        }

        #endregion プロパティ

        public MvcDbContext() : base("OracleDbContext")
        {
            System.Diagnostics.Debug.WriteLine("Create DbContext.");

            // DebugモードではSQLログを出力する
            this.Database.Log = (log) => System.Diagnostics.Debug.WriteLine(log);

            // Oracleのグローバリゼーションを設定する。
            var glimpseDbConnection = this.Database.Connection as GlimpseDbConnection;
            var oracleConnection = glimpseDbConnection.InnerConnection as OracleConnection;
            this.Database.Connection.Open();
            var oracleGlobalization = this.SetGlobalization(oracleConnection.GetSessionInfo());
            oracleConnection.SetSessionInfo(oracleGlobalization);

            // Dapperを使うときにsnake caseの結果セットをpascal caseのクラスにマップする。
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        /// <summary>
        /// DBコンテキストの初期化
        /// </summary>
        /// <param name="modelBuilder">DBコンテキストのモデルを定義するビルダー</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Oracle接続設定
            var connString = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
            var oracleBuilder = new OracleConnectionStringBuilder(connString);
            if (!string.IsNullOrEmpty(connString))
            {
                this.ApplyAllConventionsIfOracle(modelBuilder);
                modelBuilder.HasDefaultSchema(oracleBuilder.UserID);
            }

            // 個別テーブルのカラム設定
            // var assemblies = Assembly.GetExecutingAssembly();
            // foreach (var type in assemblies.GetTypes())
            // {
            //    if (type.IsSubclassOf(new BaseModel().GetType()))
            //    {
            //        dynamic subClass = Activator.CreateInstance(type);
            //        subClass.OnModelCreating(modelBuilder, subClass);
            //    }
            // }

            ////インデックス設定
            // modelBuilder.Entity<Sample>().HasIndex(m => new { m.SampleStyle, m.SampleDate }).IsUnique(false);

            ////外部キー制約 https://docs.microsoft.com/ja-jp/ef/core/modeling/relationships
            // modelBuilder.Entity<SampleDetail>().HasRequired(m => m.Sample).WithMany(m => m.SampleDetails).WillCascadeOnDelete(false);
            ////modelBuilder.Entity<IfGroup>().HasOptional(m => m.IfGroupDetails).WithRequired().WillCascadeOnDelete(false);
            ////modelBuilder.Entity<If>().HasOptional(m => m.IfGroupDetails).WithRequired().WillCascadeOnDelete(false);
            // modelBuilder.Entity<IfGroupDetail>().HasRequired(m => m.IfGroup).WithMany(m => m.IfGroupDetails).WillCascadeOnDelete(false);
            // modelBuilder.Entity<IfGroupDetail>().HasRequired(m => m.If).WithMany(m => m.IfGroupDetails).WillCascadeOnDelete(false);
            // modelBuilder.Entity<IfTaskSeq>().HasRequired(m => m.If).WithMany(m => m.IfTaskSeqs).WillCascadeOnDelete(false);
            // modelBuilder.Entity<IfTaskSeq>().HasRequired(m => m.IfTask).WithMany(m => m.IfTaskSeqs).WillCascadeOnDelete(false);
            // modelBuilder.Entity<IfRunDetail>().HasRequired(m => m.If).WithMany(m => m.IfRunDetails).WillCascadeOnDelete(false);
            // modelBuilder.Entity<IfRunDetail>().HasRequired(m => m.IfRun).WithMany(m => m.IfRunDetails).WillCascadeOnDelete(false);
            // modelBuilder.Entity<IfFile>().HasRequired(m => m.IfRun).WithMany(m => m.IfFiles).WillCascadeOnDelete(false);

            ////個別カラム設定（BaseModelを継承していないモデルはここで定義してください。）
            // modelBuilder.Entity<SystemLog>().Property(m => (decimal)m.StatusClass).HasPrecision(2, 0);
            // modelBuilder.Entity<SystemLog>().Property(m => (decimal)m.ResultClass).HasPrecision(2, 0);

            // base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// OracleGlobalizationにこのアプリケーションのグローバリゼーションを設定する。
        /// </summary>
        /// <param name="globalization">グローバリゼーション</param>
        /// <returns>このアプリケーションのグローバリゼーション</returns>
        private OracleGlobalization SetGlobalization(OracleGlobalization globalization)
        {
            globalization.Calendar = "GREGORIAN";
            globalization.Comparison = "BINARY";
            globalization.Currency = "\\";
            globalization.DateFormat = "RR-MM-DD";
            globalization.DateLanguage = "JAPANESE";
            globalization.DualCurrency = "\\";
            globalization.ISOCurrency = "JAPAN";
            globalization.Language = "AMERICAN"; // Beanstalkのサーバーに日本語の言語パックがないため
            globalization.LengthSemantics = "BYTE";
            globalization.NCharConversionException = false;
            globalization.NumericCharacters = ".,";
            globalization.Sort = "BINARY";
            globalization.Territory = "JAPAN";
            globalization.TimeStampFormat = "RR-MM-DD HH24:MI:SSXFF";
            globalization.TimeStampTZFormat = "RR-MM-DD HH24:MI:SSXFF TZR";

            return globalization;
        }

        #region DBコンテキストライフサイクル管理

        /// <summary>
        /// DBコンテキストキャッシュキー
        /// </summary>
        /// <remarks>
        /// やりたいこと
        /// ・モデルからDbContextを生成してDBアクセスしたい
        /// ・DbContextのDispose管理したい
        /// ・DIコンテナは使わない（現時点ではメリットを正しく把握できていない）
        /// 参考URL
        /// http://blog.shibayan.jp/entry/20130504/1367672809
        /// </remarks>
        private const string CacheKey = "__MvcDbContext__";

        /// <summary>
        /// 現在DbCotextを保持しているか
        /// </summary>
        public static bool HasCurrent
        {
            get { return HttpContext.Current.Items[CacheKey] != null; }
        }

        /// <summary>
        /// 現在のDbコンテキストを取得（無ければ生成する）
        /// </summary>
        public static MvcDbContext Current
        {
            get
            {
                var context = (MvcDbContext)HttpContext.Current.Items[CacheKey];

                if (context == null)
                {
                    context = new MvcDbContext();
                    HttpContext.Current.Items[CacheKey] = context;
                }

                return context;
            }
        }

        #endregion DBコンテキストライフサイクル管理
    }
}