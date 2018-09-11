using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Diagnostics;

namespace pos_read
{
    class DB
    {
        static SqlConnection Conn = new SqlConnection("Data Source=LAPTOP-EMJVR3G8;Initial Catalog=posDB;Integrated Security=True");

        public static SqlConnection OpenConn()
        {
            if (Conn.State == System.Data.ConnectionState.Closed)
            {
                Conn.Open();
            }
            return Conn;
        }

        public static void CloseConn()
        {
            if (Conn.State == System.Data.ConnectionState.Open)
            {
                Conn.Close();
            }
        }

        public static void ExecuteNonQuery(string sPassQuery)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(sPassQuery, OpenConn());
                //cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //ErrorLogEvents(ex.ToString());
            }
            //CloseConn();
        }
    }
}
