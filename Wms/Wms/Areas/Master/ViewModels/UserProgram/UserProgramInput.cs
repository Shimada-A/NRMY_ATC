namespace Wms.Areas.Master.ViewModels.UserProgram
{
    using System.Collections.Generic;

    /// <summary>
    /// Store list country which is posted from view
    /// </summary>
    public class UserProgramInput
    {
        public List<SelectedUserProgramViewModel> UserProgramViewModel { get; set; }

        public string ShipperId { get; set; }
    }
}