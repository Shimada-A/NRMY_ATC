namespace Wms.Areas.Master.Models
{
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using Dapper;
    using PagedList;
    using Wms.Areas.Master.ViewModels.Transporter;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.Transporter.TransporterSearchCondition;

    public partial class Transporter
    {
        /// <summary>
        /// Get Transporter List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<Transporter> GetData(TransporterSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                       MT.UPDATE_COUNT
                      ,MT.SHIPPER_ID
                      ,MT.TRANSPORTER_ID
                      ,MT.TRANSPORTER_NAME
                      ,MT.TRANSPORTER_SHORT_NAME
                      ,MT.TRANSPORTER_TEL
                      ,MT.TRANSPORTER_FAX
                      ,MT.TRANSPORTER_MAIL
                      ,MT.TRANSPORTER_ZIP
                      ,MT.TRANSPORTER_ADDRESS1
                      ,MT.TRANSPORTER_ADDRESS2
                      ,MT.TRANSPORT_WEEK_FLAGS
                      ,MT.TRANSPORTER_CLASS
                      ,MT.INVOICE_PRINT_FLAG
                  FROM M_TRANSPORTERS MT
                 WHERE MT.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Add search condition
            if (!string.IsNullOrEmpty(condition.TransporterId))
            {
                query.Append(" AND MT.TRANSPORTER_ID LIKE :TRANSPORTER_ID ");
                parameters.Add(":TRANSPORTER_ID", condition.TransporterId + "%");
            }

            if (!string.IsNullOrEmpty(condition.TransporterName))
            {
                query.Append(@" AND MT.TRANSPORTER_NAME LIKE :TRANSPORTER_NAME ");
                parameters.Add(":TRANSPORTER_NAME", condition.TransporterName + "%");
            }

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<Transporter>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case TransporterSortKey.TransporterName:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MT.TRANSPORTER_NAME DESC,MT.TRANSPORTER_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MT.TRANSPORTER_NAME ASC,MT.TRANSPORTER_ID ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MT.TRANSPORTER_ID DESC,MT.TRANSPORTER_NAME DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MT.TRANSPORTER_ID ASC,MT.TRANSPORTER_NAME ASC");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var transporters = MvcDbContext.Current.Database.Connection.Query<Transporter>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<Transporter>(transporters, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Get Transporter By Id
        /// </summary>
        /// <param name="transporterId">transporterId</param>
        /// <param name="shipperId">shipperId</param>
        /// <returns>Country</returns>
        public Transporter GetTargetById(string transporterId, string shipperId)
        {
            return MvcDbContext.Current.Transporters.Find(transporterId, shipperId);
        }

        /// <summary>
        /// Update Transporter
        /// </summary>
        /// <param name="transporter"></param>
        /// <returns>Update status</returns>
        public bool UpdateTransporter(Transporter transporter)
        {
            var dbContext = MvcDbContext.Current;

            var updatedTransporter =
                  MvcDbContext.Current.Transporters
                  .Where(m => m.ShipperId == transporter.ShipperId && m.TransporterId == transporter.TransporterId && m.UpdateCount == transporter.UpdateCount)
                  .SingleOrDefault();

            // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合）
            if (updatedTransporter == null)
            {
                return false;
            }

            updatedTransporter.SetBaseInfoUpdate();

            updatedTransporter.TransporterTel = transporter.TransporterTel;
            updatedTransporter.TransporterFax = transporter.TransporterFax;
            updatedTransporter.TransporterMail = transporter.TransporterMail;
            updatedTransporter.TransporterZip = transporter.TransporterZip;
            updatedTransporter.TransporterAddress1 = transporter.TransporterAddress1;
            updatedTransporter.TransporterAddress2 = transporter.TransporterAddress2;
            updatedTransporter.InvoicePrintFlag = transporter.InvoicePrintFlag;

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
    }
}