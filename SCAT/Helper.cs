using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCAT
{
    static class Helper
    {
        //private static string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|DatabaseFile.mdf;Integrated Security = True";
        public static string ConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["DbCon"].ConnectionString;
            }
        }
    }

    class DbaseOpretions
    {
        public SqlConnection con;
        public SqlCommand cmd;
        public SqlDataAdapter sda;

        public void connection()
        {
           con = new SqlConnection(Helper.ConnectionString); 
            con.Open();
        }
        public void sendData(string SQL)
        {
            try
            {
                connection();
                cmd = new SqlCommand(SQL,con);
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        public void getData(string SQL)
        {
            try
            {
                connection();
                sda = new SqlDataAdapter(SQL, con);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
    }

    class MyGlobalVariables
    {
        public static string uniqueID;
        public static string consulatantName;
        public static string companyName;
        public static string serialKeyID;
        public static bool isActive;
        public static byte[] pImg;
        public static byte[] pBuffer = null;

    }
}
