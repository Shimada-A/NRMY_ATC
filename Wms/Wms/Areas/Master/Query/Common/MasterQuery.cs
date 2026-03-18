using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wms.Models;

namespace Wms.Areas.Master.Query.Common
{
    public class MasterQuery
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetTransportersList()
        {
            return MvcDbContext.Current.Transporters
                .Where(
                    d=>d.DeleteFlag == false
                )
                .Select(m => new SelectListItem
                {
                    Value = m.TransporterId,
                    Text = m.TransporterName
                })
                .OrderBy(m => m.Text);
        }

        /// <summary>
        /// 店舗マスタList
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetStoresList()
        {
            return MvcDbContext.Current.Stores
                .Where(
                    d => d.DeleteFlag == false
                )
                .Select(m => new SelectListItem
                {
                    Value = m.LocId,
                    Text = m.LocName1
                })
                .OrderBy(m => m.Text);
        }

        /// <summary>
        /// 倉庫マスタList
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetWarehousesList()
        {
            return MvcDbContext.Current.Warehouses
                .Where(
                    d => d.DeleteFlag == false
                )
                .Select(m => new SelectListItem
                {
                    Value = m.LocId,
                    Text = m.LocName1
                })
                .OrderBy(m => m.Text);
        }

    }
}