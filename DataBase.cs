using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace Trucker
{
    class DataBase
    {
        /// <summary>
        /// Connection string
        /// </summary>
        public SqlConnection SqlConnection = new SqlConnection(@"Data Source=KOMPUTER;Initial Catalog=WinBase;Integrated Security=True");
        public void OpenConn()
        {
            if(SqlConnection.State == System.Data.ConnectionState.Closed)
                SqlConnection.Open();
        }
        public void CloseConn()
        {
            if (SqlConnection.State == System.Data.ConnectionState.Open)
                SqlConnection.Close();
        }
        public SqlConnection GetConn()
        {
            return SqlConnection;
        }
    }
}
