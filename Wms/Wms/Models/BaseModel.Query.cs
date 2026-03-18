using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Common;

namespace Wms.Models
{
    public partial class BaseModel
    {
        public static DynamicParameters CreateParamWith(dynamic option = null)
        {
            var param = new DynamicParameters(new
            {
                MAKE_USER_ID = Profile.User.UserId,
                MAKE_PROGRAM_NAME = GetProgramId(),
                UPDATE_USER_ID = Profile.User.UserId,
                UPDATE_PROGRAM_NAME = GetProgramId(),
                SHIPPER_ID = Profile.User.ShipperId,
                CENTER_ID = Profile.User.CenterId
            }) ;
            param.AddDynamicParams(option);
            return param;
        }

        protected virtual DynamicParameters CreateParam()
        {
            var param = new DynamicParameters(new
            {
                MAKE_USER_ID = MakeUserId,
                MAKE_PROGRAM_NAME = MakeProgramName,
                UPDATE_USER_ID = Profile.User.UserId,
                UPDATE_DATE = UpdateDate,
                UPDATE_PROGRAM_NAME = UpdateProgramName,
                UPDATE_COUNT = UpdateCount
            });
            return param;
        }

    }
}