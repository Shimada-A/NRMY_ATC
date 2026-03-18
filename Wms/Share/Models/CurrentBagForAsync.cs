namespace Share.Model
{
    using System.Data.Entity;
    using Wms.Models;
    public class CurrentBagForAsync
    {
        public string ShipperId { get; set; }
        public string CenterId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ProgramName { get; set; }
        public MvcDbContext Database { get; set; }
    }
}
