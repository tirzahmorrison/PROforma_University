using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROforma_University
{
    class BaseAction
    {
        protected DbConnection dbConn;
        public BaseAction(DbConnection dbConnection)
        {
            dbConn = dbConnection;
        }
    }
}

