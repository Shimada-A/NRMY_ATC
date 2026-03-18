namespace Wms.Models
{
    public class CurrentBagForCpuTask
    {
        public string ShipperId { get; set; }
        public string CenterId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ProgramName { get; set; }
        public MvcDbContext CurrentMvcDbContext { get; set; }
    }
}
