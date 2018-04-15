using System;
using System.Data.SqlClient;

namespace PROforma_University
{
    public class DbConnection
    {
        private SqlConnection sqlConnection;   //field to hold SQL connection 
        public DbConnection()  //constructor 
        {
            sqlConnection = new SqlConnection(@"Data Source=TMORRISON01\SQLEXPRESS;Initial Catalog=University;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
        internal SqlDataReader Execute(SqlCommand cmd)
        {
            using (cmd)  //allows proper clean up for the connection and cmd object
            {
                if (sqlConnection.State != System.Data.ConnectionState.Open)
                    sqlConnection.Open();  //open connection
                cmd.Connection = sqlConnection;   //attach the connection to the command
                return cmd.ExecuteReader();    ///run the query
            }
        }

        internal object ExecuteScalar(SqlCommand cmd)
        {
            using (cmd)  //allows proper clean up for the connection and cmd object
            {
                if (sqlConnection.State != System.Data.ConnectionState.Open)
                    sqlConnection.Open();
                cmd.Connection = sqlConnection;   //attach the connection to the command
                return cmd.ExecuteScalar();    ///run the query returing single value
            }
        }
    }
}