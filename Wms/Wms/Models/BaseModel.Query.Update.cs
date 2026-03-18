using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wms.Models
{
    public partial class BaseModel
    {
        public virtual void Update()
        {
            SetBaseInfoInsert();
            var param = CreateParam();
            MvcDbContext.OracleConnection.Execute(UpdateSql(), param);
        }


        protected virtual string UpdateSql()
        {
            throw new NotImplementedException();
        }
    }
}