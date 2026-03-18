namespace Wms.Areas.Master.Models
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Wms.Areas.Master.ViewModels.EcPrefTransporter;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.EcPrefTransporter.EcPrefTransporterSearchCondition;

    public partial class EcPrefTransporter
    {
        /// <summary>
        /// Get EcPrefTransporter List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<EcPrefTransporterList> GetData(EcPrefTransporterSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                        PREF_TRANSPORTER.CENTER_ID
                    ,   PREF_TRANSPORTER.PREF_ID
                    ,   PREF_TRANSPORTER.PREF_NAME
                    ,   PREF_TRANSPORTER.TRANSPORTER_ID
                    ,   TRANSPORTER.TRANSPORTER_NAME
                FROM 
                        M_EC_PREF_TRANSPORTER PREF_TRANSPORTER
                LEFT JOIN 
                        M_TRANSPORTERS TRANSPORTER
                ON 
                        PREF_TRANSPORTER.TRANSPORTER_ID = TRANSPORTER.TRANSPORTER_ID
                    AND PREF_TRANSPORTER.SHIPPER_ID = TRANSPORTER.SHIPPER_ID
                WHERE 
                        PREF_TRANSPORTER.CENTER_ID =:CENTER_ID 
                    AND PREF_TRANSPORTER.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);

            //取得条件の追加
            if (!string.IsNullOrEmpty(condition.TransporterId))
            {
                query.Append("AND PREF_TRANSPORTER.TRANSPORTER_ID =:TRANSPORTER_ID");
                parameters.Add(":TRANSPORTER_ID", condition.TransporterId);
            }

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<EcPrefTransporterList>(query.ToString(), parameters).Count();

            //Sort function
            switch (condition.SortKey)
            {
                case EcPrefTransporterSortKey.PrefId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"
                                ORDER BY
                                        TO_NUMBER(PREF_TRANSPORTER.PREF_ID) DESC");
                            break;

                        default:
                            query.AppendLine(@"
                                ORDER BY
                                        TO_NUMBER(PREF_TRANSPORTER.PREF_ID) ASC");
                            break;
                    }

                    break;

                case EcPrefTransporterSortKey.TransporterIdPrefId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"
                                ORDER BY
                                        PREF_TRANSPORTER.TRANSPORTER_ID DESC
                                    ,   TO_NUMBER(PREF_TRANSPORTER.PREF_ID) DESC");
                            break;

                        default:
                            query.AppendLine(@"
                                ORDER BY
                                        PREF_TRANSPORTER.TRANSPORTER_ID ASC
                                    ,   TO_NUMBER(PREF_TRANSPORTER.PREF_ID) ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"
                                ORDER BY
                                        TO_NUMBER(PREF_TRANSPORTER.PREF_ID) DESC");
                            break;

                        default:
                            query.AppendLine(@"
                                ORDER BY
                                        TO_NUMBER(PREF_TRANSPORTER.PREF_ID) ASC");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            List<EcPrefTransporterList> ecPrefTransporters = MvcDbContext.Current.Database.Connection.Query<EcPrefTransporterList>(query.ToString(), parameters).ToList();

            // Excute paging
            return new StaticPagedList<EcPrefTransporterList>(ecPrefTransporters, condition.Page, condition.PageSize, totalCount);

        }

        /// <summary>
        /// 詳細画面表示に必要なデータを取得。（一覧画面で選択された行のデータを取得する）
        /// </summary>
        /// <param name="ecPrefTransporter">一覧画面で選択された行</param>
        /// <returns></returns>
        public Detail GetTargetById(EcPrefTransporterList ecPrefTransporter)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                        PREF_TRANSPORTER.UPDATE_COUNT
                    ,   PREF_TRANSPORTER.CENTER_ID
                    ,   CENTER.CENTER_NAME1 AS CENTER_NAME
                    ,   PREF_TRANSPORTER.PREF_ID
                    ,   PREF_TRANSPORTER.PREF_NAME
                    ,   PREF_TRANSPORTER.TRANSPORTER_ID
                FROM 
                        M_EC_PREF_TRANSPORTER PREF_TRANSPORTER
                LEFT JOIN 
                        M_CENTERS CENTER
                ON 
                        CENTER.CENTER_ID = PREF_TRANSPORTER.CENTER_ID
                    AND CENTER.SHIPPER_ID = PREF_TRANSPORTER.SHIPPER_ID
                WHERE 
                        PREF_TRANSPORTER.CENTER_ID =:CENTER_ID 
                    AND PREF_TRANSPORTER.PREF_ID =:PREF_ID
                    AND PREF_TRANSPORTER.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", ecPrefTransporter.CenterId);
            parameters.Add(":PREF_ID", ecPrefTransporter.PrefId);

            return MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters).FirstOrDefault();

        }

        /// <summary>
        /// EC注文都道府県別配送業者マスタ更新
        /// </summary>
        /// <param name="ecPrefTransporter"></param>
        /// <returns>Update status</returns>
        public bool UpdateEcPrefTransporter(Detail detail)
        {
            var dbContext = MvcDbContext.Current;

            var updatedEcPrefTransporter =
                  MvcDbContext.Current.EcPrefTransporter
                  .Where(m => m.ShipperId == Profile.User.ShipperId
                           && m.CenterId == detail.CenterId
                           && m.PrefId == detail.PrefId
                           && m.UpdateCount == detail.UpdateCount)
                  .SingleOrDefault();

            // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって削除済みの場合）
            if (updatedEcPrefTransporter == null)
            {
                return false;
            }

            updatedEcPrefTransporter.SetBaseInfoUpdate();

            updatedEcPrefTransporter.TransporterId = detail.TransporterId;

            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return false;
                }

                trans.Commit();
            }

            return true;
        }

        /// <summary>
        /// センターのドロップダウンリストに表示するデータを取得
        /// </summary>
        /// <returns>センターのドロップダウンリストに表示するデータ</returns>
        public IEnumerable<SelectListItem> GetSelectCenterListItems()
        {
            return MvcDbContext.Current.Warehouses
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.CenterId,
                    Text = m.CenterName1
                })
                .OrderBy(m => m.Value);
        }


        /// <summary>
        /// 配送業者のドロップダウンリストに表示するデータを取得
        /// </summary>
        /// <returns>配送業者のドロップダウンリストに表示するデータ</returns>
        public IEnumerable<SelectListItem> GetSelectTransporterListItems()
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                        GENERAL.GEN_CD AS VALUE
                    ,   GENERAL.GEN_CD||':'||GENERAL.GEN_NAME AS TEXT
                FROM
                        M_GENERALS GENERAL
                WHERE
                        GENERAL.SHIPPER_ID = :SHIPPER_ID
                    AND GENERAL.CENTER_ID = '@@@'
                    AND GENERAL.GEN_DIV_CD = 'EC_TRANSPORTER'
                    AND GENERAL.REGISTER_DIVI_CD = '1'
                ORDER BY
                        GENERAL.GEN_CD
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Fill data to memory
            var general = MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters).ToList();

            // Excute paging
            return general;
        }
    }
}