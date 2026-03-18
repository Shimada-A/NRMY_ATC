namespace Wms.Areas.Master.Query.ShippingHoldStores
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Dapper;
    using PagedList;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.ViewModels.ShippingHoldStores;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.ShippingHoldStores.SearchCondition;

    public class ShippingHoldStoresQuery
    {
        /// <summary>
        /// ワークテーブル登録
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public bool InsertWork(SearchCondition conditions){
            StringBuilder query= new StringBuilder();
            DynamicParameters param = new DynamicParameters();
            param.Add(":SHIPPER_ID", Profile.User.ShipperId);
            param.Add(":USER_ID", Profile.User.UserId);
            param.Add(":PROGRAM_NAME", "ShippingHoldStoresQuery.InsertWork");
            param.Add(":SEQ", conditions.Seq);

            query.Append(@"
                INSERT INTO
                        WW_MAS_SHIPPING_HOLD_STORES
                SELECT
                        SYSTIMESTAMP  MAKE_DATE
                    ,   :USER_ID MAKE_USER
                    ,   :PROGRAM_NAME MAKE_PROGRAM_NAME
                    ,   SYSTIMESTAMP UPDATE_DATE
                    ,   :USER_ID UPDATE_USER_ID
                    ,   :PROGRAM_NAME AS UPDATE_PROGRAM_NAME
                    ,   UPDATE_COUNT
                    ,   SHIPPER_ID
                    ,   SEQ
                    ,   ROW_NUMBER() OVER(" + getSortCondition(conditions) + @") AS No
                    ,   BRAND_ID
                    ,   BRAND_NAME
                    ,   STORE_ID
                    ,   STORE_NAME
                    ,   NVL(SHIPPING_HOLD_CLASS,0) SHIPPING_HOLD_CLASS
                FROM (
                    SELECT
                            MAX(NVL(MSHS.UPDATE_COUNT,0)) AS UPDATE_COUNT
                        ,   MAX(MSF.SHIPPER_ID) AS SHIPPER_ID
                        ,   :SEQ  AS SEQ
                        ,   MSF.BRAND_ID AS BRAND_ID
                        ,   MAX(MB.BRAND_NAME) AS BRAND_NAME
                        ,   MSF.STORE_ID AS STORE_ID
                        ,   MAX(VS.SHIP_TO_STORE_NAME1) AS STORE_NAME
                        ,   MAX(MSHS.SHIPPING_HOLD_CLASS) AS SHIPPING_HOLD_CLASS
                    FROM
                            M_SHIP_FRONTAGE MSF
                    LEFT JOIN
                            V_SHIP_TO_STORES VS
                        ON
                            MSF.SHIPPER_ID = VS.SHIPPER_ID
                        AND MSF.STORE_ID = VS.SHIP_TO_STORE_ID
                    LEFT JOIN
                            M_BRANDS MB
                        ON
                            MSF.SHIPPER_ID = MB.SHIPPER_ID
                        AND MSF.BRAND_ID = MB.BRAND_ID
                    LEFT JOIN
                            M_SHIPPING_HOLD_STORES MSHS
                        ON
                            MSF.SHIPPER_ID = MSHS.SHIPPER_ID
                        AND MSF.BRAND_ID = MSHS.BRAND_ID
                        AND MSF.STORE_ID = MSHS.STORE_ID
                    WHERE
                            MSF.SHIPPER_ID = :SHIPPER_ID
                        AND VS.TEMP_STORE_CLASS = '0'
               ");
            // ブランドID
            if (!string.IsNullOrEmpty(conditions.BrandId)){
                query.Append(@"                    AND MSF.BRAND_ID = :BRAND_ID ");
                param.Add("BRAND_ID", conditions.BrandId);
            }
            // ブランド名
            if (!string.IsNullOrEmpty(conditions.BrandName))
            {
                query.Append(@"                    AND MB.BRAND_NAME LIKE :BRAND_NAME ");
                param.Add("BRAND_NAME", conditions.BrandName + '%');
            }
            // 店舗ID
            if (!string.IsNullOrEmpty(conditions.StoreId))
            {
                query.Append(@"                    AND MSF.STORE_ID = :STORE_ID ");
                param.Add("STORE_ID", conditions.StoreId);
            }
            // 店舗名
            if (!string.IsNullOrEmpty(conditions.StoreName))
            {
                query.Append(@"                    AND VS.SHIP_TO_STORE_NAME1 LIKE :STORE_NAME ");
                param.Add("STORE_NAME", conditions.StoreName + '%');
            }

            query.Append(@"
                    GROUP BY
                            MSF.BRAND_ID
                        ,   MSF.STORE_ID
                ) WW_MAS
            ");

            // 登録処理実行
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), param);
                }
                catch (Exception) //デバッグ用
                {
                    trans.Rollback();
                    throw;
                }
                trans.Commit();
            }
            return true;
        }

        /// <summary>
        /// order by の条件を取得する
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public static string getSortCondition(SearchCondition conditions){
            switch (conditions.SortKey)
            {
                case SortKeys.SortKeyStore:
                    switch (conditions.Sort)
                    {
                        case AscDescSort.Desc:
                            return @" ORDER BY STORE_ID DESC , BRAND_ID DESC ";
                        default:
                            return @" ORDER BY STORE_ID ASC , BRAND_ID ASC ";
                    }
                default:
                    switch (conditions.Sort)
                    {
                        case AscDescSort.Desc:
                            return @" ORDER BY BRAND_ID DESC , STORE_ID DESC ";
                        default:
                            return @" ORDER BY BRAND_ID ASC, STORE_ID ASC ";
                    }
            }
        }

        /// <summary>
        /// ワークテーブル取得SQL
        /// </summary>
        /// <returns></returns>
        public string GetWorlQuery(){
            return @"
                SELECT 
                        SHIPPER_ID
                    ,   SEQ
                    ,   NO
                    ,   BRAND_ID
                    ,   BRAND_NAME
                    ,   STORE_ID
                    ,   STORE_NAME
                    ,   SHIPPING_HOLD_CLASS
                FROM
                        WW_MAS_SHIPPING_HOLD_STORES
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                    AND SEQ = :SEQ 
                ORDER BY
                        No
            ";
        }

        /// <summary>
        /// 一覧情報取得
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<Models.MasShippingHoldStore> GetData(SearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();

            // SQL取得
            query.Append(GetWorlQuery());
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<Models.MasShippingHoldStore>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            var resultList = MvcDbContext.Current.Database.Connection.Query<Models.MasShippingHoldStore>(query.ToString(), parameters);

            return new StaticPagedList<Models.MasShippingHoldStore>(resultList, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// キー項目取得
        /// </summary>
        /// <param name="Seq"></param>
        /// <returns></returns>
        public List<MasShippingHoldStore> GetKeyWords(long Seq){
            DynamicParameters parameters = new DynamicParameters();
            string query = GetWorlQuery();
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":SEQ", Seq);
            return MvcDbContext.Current.Database.Connection.Query<MasShippingHoldStore>(query.ToString(),parameters).ToList();
        }

        /// <summary>
        /// ワークテーブル更新
        /// </summary>
        /// <returns></returns>
        public bool UpdateWork(List<MasShippingHoldStore> updateList){
            StringBuilder query = new StringBuilder();
            query.Append(@"
                UPDATE 
                        WW_MAS_SHIPPING_HOLD_STORES
                SET
                        SHIPPING_HOLD_CLASS = :SHIPPING_HOLD_CLASS
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                    AND SEQ = :SEQ
                    AND NO = :NO
            ");

            // 更新処理を実行
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    foreach(var upd in updateList){
                        DynamicParameters param = new DynamicParameters();
                        param.Add("SHIPPER_ID", Profile.User.ShipperId);
                        param.Add("SEQ", upd.Seq);
                        param.Add("NO", upd.No);
                        param.Add("SHIPPING_HOLD_CLASS", upd.ShippingHoldClass ? 1 : 0);
                        MvcDbContext.Current.Database.Connection.Execute(query.ToString(), param);
                    }
                }
                catch (Exception ex) //デバッグ用
                {
                    trans.Rollback();
                    return false;
                }
                trans.Commit();
            }
            return true;
        }

        /// <summary>
        /// 確定処理を実行
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public bool Confirm(long Seq){
            StringBuilder query = new StringBuilder();
            DynamicParameters param = new DynamicParameters();

            query.Append(@"
                MERGE INTO
                        M_SHIPPING_HOLD_STORES MSHS
                USING(
                    SELECT
                            *
                    FROM
                            WW_MAS_SHIPPING_HOLD_STORES 
                    WHERE
                            SHIPPER_ID = :SHIPPER_ID
                        AND SEQ = :SEQ
                    ) WW
                    ON (
                        MSHS.SHIPPER_ID = WW.SHIPPER_ID
                    AND MSHS.BRAND_ID = WW.BRAND_ID
                    AND MSHS.STORE_ID = WW.STORE_ID
                )
                WHEN MATCHED THEN
                    UPDATE SET
                            MSHS.UPDATE_DATE = SYSTIMESTAMP
                        ,   MSHS.UPDATE_USER_ID = :USER_ID
                        ,   MSHS.UPDATE_PROGRAM_NAME = :PROGRAM_NAME
                        ,   MSHS.UPDATE_COUNT = MSHS.UPDATE_COUNT + 1
                        ,   MSHS.SHIPPING_HOLD_CLASS = WW.SHIPPING_HOLD_CLASS
                WHEN NOT MATCHED THEN
                    INSERT (
                            MSHS.MAKE_DATE
                        ,   MSHS.MAKE_USER_ID
                        ,   MSHS.MAKE_PROGRAM_NAME
                        ,   MSHS.UPDATE_DATE
                        ,   MSHS.UPDATE_USER_ID
                        ,   MSHS.UPDATE_PROGRAM_NAME 
                        ,   MSHS.UPDATE_COUNT
                        ,   MSHS.SHIPPER_ID
                        ,   MSHS.BRAND_ID
                        ,   MSHS.STORE_ID
                        ,   MSHS.SHIPPING_HOLD_CLASS
                    ) 
                    VALUES(
                            SYSTIMESTAMP
                        ,   :USER_ID
                        ,   :PROGRAM_NAME
                        ,   SYSTIMESTAMP
                        ,   :USER_ID
                        ,   :PROGRAM_NAME
                        ,   0
                        ,   WW.SHIPPER_ID
                        ,   WW.BRAND_ID
                        ,   WW.STORE_ID
                        ,   WW.SHIPPING_HOLD_CLASS
                    )
            ");
            param.Add(":SHIPPER_ID", Profile.User.ShipperId);
            param.Add(":USER_ID", Profile.User.UserId);
            param.Add(":PROGRAM_NAME", "ShippingHoldStoresQuery.Confirm");
            param.Add(":SEQ", Seq);
            // 更新処理を実行
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), param);
                }
                catch (Exception ex) //デバッグ用
                {
                    trans.Rollback();
                    return false;
                }
                trans.Commit();
            }
            return true;
        }
    }
}