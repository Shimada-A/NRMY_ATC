namespace Wms.Areas.Master.ViewModels.LocTransporter
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Security.AccessControl;
    using System.Xml.Linq;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class LocTransporterInsert
    {

        public IList<SearchItem> GetRows { get; set; }
    }

    public class Create
    {
        public LocTransporterInsert SearchConditions { get; set; }

        public string CenterId { get; set; }
        
        public string CenterName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public enum Holidays
        {
            Mon,
            Tue,
            Wed,
            Thu,
            Fri,
            Sat,
            Sun,
            Hol
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocTransporterViewModel"/> class.
        /// </summary>
        public Create()
        {
            this.SearchConditions = new LocTransporterInsert();
        }
    }
}