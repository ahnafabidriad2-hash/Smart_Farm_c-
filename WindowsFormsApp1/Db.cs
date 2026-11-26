using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    // database er connection ek jaygay. age 28 ta file e alada kore lekha chilo,
    // tai onno PC te chalate holo sob gula bodlate hoto. ekhon shudhu App.config e
    public static class Db
    {
        public static string ConnectionString
        {
            get
            {
                var cs = System.Configuration.ConfigurationManager.ConnectionStrings["SmartFarm"];
                return cs != null ? cs.ConnectionString : @"data source=.\SQLEXPRESS; database=SmartFarm; integrated security=SSPI";
            }
        }

        // INSERT/UPDATE/DELETE. vul hole crash na kore message dekhay ar -1 dey
        public static int Execute(string sql, params SqlParameter[] parameters)
        {
            try
            {
                using (var con = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddRange(parameters);
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                ShowError(ex);
                return -1;
            }
        }

        public static object Scalar(string sql, params SqlParameter[] parameters)
        {
            try
            {
                using (var con = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddRange(parameters);
                    con.Open();
                    return cmd.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                ShowError(ex);
                return null;
            }
        }

        public static DataTable Query(string sql, params SqlParameter[] parameters)
        {
            var dt = new DataTable();
            try
            {
                using (var con = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddRange(parameters);
                    new SqlDataAdapter(cmd).Fill(dt);
                }
            }
            catch (SqlException ex)
            {
                ShowError(ex);
            }
            return dt;
        }

        public static SqlParameter P(string name, object value)
        {
            return new SqlParameter(name, value ?? DBNull.Value);
        }

        // sql er lomba error er bodole bujhar moto message
        public static void ShowError(SqlException ex)
        {
            string msg;
            if (ex.Number == 2627 || ex.Number == 2601) msg = "This ID already exists. Please use a different ID.";
            else if (ex.Number == 245 || ex.Number == 8114) msg = "Please enter numbers where numbers are needed.";
            else if (ex.Number == 8152) msg = "Some text is too long. Please shorten it.";
            else if (ex.Number == 547) msg = "This record is linked to other data and cannot be changed.";
            else if (ex.Number == -1 || ex.Number == 2 || ex.Number == 53 || ex.Number == 4060)
                msg = "Cannot connect to the database. Make sure SQL Server is running and the SmartFarm database exists.";
            else msg = "Database error: " + ex.Message;
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
