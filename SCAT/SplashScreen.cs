using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SCAT;

namespace SCAT
{

    public partial class SplashScreen : Form
    {
        DataTable dt = new DataTable();

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
            (
            int nLeftRect,
            int nTopRect,
            int RightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );
        public SplashScreen()
        {
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
            progressBar1.Value = 0;
        }
        DbaseOpretions con = new DbaseOpretions(); // Coonection for implementing direct CRUD Operations
        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Value++;
            progressBar1.Text = progressBar1.Value.ToString() + "%";
            if(progressBar1.Value == 100)
            {
                timer1.Enabled = false;
                UserValidation userValidationForm = new UserValidation();
                userValidationForm.Show();
                this.Hide();
            }
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            getUniqueDeviceID();
            LoadUserValidationData();
        }
        public void getUniqueDeviceID()
        {
            ManagementObjectCollection objectList = null;
            ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher("Select * From Win32_processor");
            objectList = objectSearcher.Get();
            string id = "";

            foreach (ManagementObject obj in objectList)
            {
                id = obj["ProcessorID"].ToString();
            }

            objectSearcher = new ManagementObjectSearcher("Select * From Win32_BaseBoard");
            objectList = objectSearcher.Get();
            string mtherBoard = "";

            foreach (ManagementObject obj in objectList)
            {
                mtherBoard = (string)obj["SerialNumber"];
            }
            MyGlobalVariables.uniqueID = id + mtherBoard;

        }

        public void LoadUserValidationData()
        {
            con.getData("Select * From UserValidation");
            dt = new DataTable();
            con.sda.Fill(dt);
            if(dt.Rows.Count > 0)
            {
                MyGlobalVariables.consulatantName = dt.Rows[0]["consultantName"].ToString();
                MyGlobalVariables.companyName = dt.Rows[0]["companyName"].ToString();
                MyGlobalVariables.serialKeyID = dt.Rows[0]["SerialKey"].ToString();
                MyGlobalVariables.isActive = (bool)dt.Rows[0]["IsActive"];
                MyGlobalVariables.pImg = (byte[])(dt.Rows[0]["Photo"]);
            }
        }
    }
}
