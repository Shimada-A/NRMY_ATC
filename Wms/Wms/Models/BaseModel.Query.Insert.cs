using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wms.Models
{
    public partial class BaseModel
    {
        public virtual void Insert()
        {
            SetBaseInfoInsert();
            var param = CreateParam();
            MvcDbContext.OracleConnection.Execute(InsertSql(), param);
        }

        protected virtual string InsertSql() {
            throw new NotImplementedException();
        }
    }
}