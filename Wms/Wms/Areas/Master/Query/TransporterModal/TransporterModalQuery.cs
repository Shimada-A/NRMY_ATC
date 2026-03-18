namespace Wms.Areas.Master.ViewModels.TransporterSearchModal
{
    using System.Linq;
    using System.Text;
    using Dapper;
    using PagedList;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.TransporterSearchModal.TransporterSearchCondition;

    public partial class TransporterViewModel
    {
        /// <summary>
        /// 検索結果取得
        /// </summary>
        /// <param name="condition">画面検索条件</param>
        /// <param name="pageSize">最大ページサイズ</param>
        /// <returns>検索結果</returns>
        public IPagedList<TransporterViewModel> Listing(TransporterSearchCondition conditions, int pageSize)
        {
            DynamicParameters parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT * 
                  FROM M_TRANSPORTERS
                 WHERE SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            if (!string.IsNullOrWhiteSpace(conditions.TransporterId))
            {
                sql.AppendLine(" AND TRANSPORTER_ID LIKE :TRANSPORTER_ID ");
                parameters.Add(":TRANSPORTER_ID", "%" + conditions.TransporterId + "%");
            }

            if (!string.IsNullOrWhiteSpace(conditions.TransporterName))
            {
                sql.AppendLine(" AND TRANSPORTER_NAME LIKE :TRANSPORTER_NAME ");
                parameters.Add(":TRANSPORTER_NAME", "%" + conditions.TransporterName + "%");
            }

            // 全レコード数を取得
            var totalCount = MvcDbContext.Current.Database.Connection.Query<TransporterViewModel>(sql.ToString(), parameters).Count();

            switch (conditions.SortKey)
            {
                case SortKey1.TransporterName:
                    sql.AppendLine(" ORDER BY ");
                    switch (conditions.OrderKey)
                    {
                        case SortKey2.Desc:
                            sql.AppendLine("    TRANSPORTER_NAME DESC, TRANSPORTER_ID DESC ");
                            break;
                        default:
                            sql.AppendLine("    TRANSPORTER_NAME ASC, TRANSPORTER_ID ASC ");
                            break;
                    }

                    break;
                default:
                    sql.AppendLine(" ORDER BY ");
                    switch (conditions.OrderKey)
                    {
                        case SortKey2.Desc:
                            sql.AppendLine("    TRANSPORTER_ID DESC, TRANSPORTER_NAME DESC ");
                            break;
                        default:
                            sql.AppendLine("    TRANSPORTER_ID ASC, TRANSPORTER_NAME ASC ");
                            break;
                    }

                    break;
            }

            sql.AppendLine("OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");

            parameters.AddDynamicParams(new { OFFSET = (conditions.Page - 1) * pageSize });
            parameters.AddDynamicParams(new { PAGE_SIZE = pageSize });
            var transporterIds = string.IsNullOrWhiteSpace(conditions.ParameterTransporterId) ? null : conditions.ParameterTransporterId.Split(',');
            var Transporters =
                Wms.Models.MvcDbContext.Current.Database.Connection.Query<TransporterViewModel, Models.Transporter, TransporterViewModel>(
                    sql.ToString(),
                    (vm, Transporter) =>
                    {
                        vm.Transporters = Transporter;
                        vm.IsCheck = transporterIds == null ? false : transporterIds.Contains(Transporter.TransporterId);
                        return vm;
                    },
                    param: parameters,
                    splitOn: "TRANSPORTER_ID")
                    .ToList();

            return new StaticPagedList<TransporterViewModel>(Transporters, (int)conditions.Page, pageSize, totalCount);
        }
    }
}