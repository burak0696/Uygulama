using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BaranYardimci
{
    public static class DB
    {
        private static readonly string _connStr =
            ConfigurationManager.ConnectionStrings["LoginDB"].ConnectionString;

        public static string ConnStr
        {
            get { return _connStr; }
        }

        public static DataTable GetTable(string query, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                var dt = new DataTable();
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                return dt;
            }
        }

        public static int Execute(string query, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static object GetValue(string query, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                return cmd.ExecuteScalar();
            }
        }
    }
} 