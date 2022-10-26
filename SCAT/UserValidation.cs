using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Common;
using System.IO;
using System.Windows.Forms.VisualStyles;
using System.Net.NetworkInformation;

namespace SCAT
{
    public partial class UserValidation : Form
    {
        SqlConnection dbcon = new SqlConnection(Helper.ConnectionString);
        SqlDataAdapter adapter;
        SqlDataReader reader;
        bool status = true;

        string fileName;
        public UserValidation()
        {
            InitializeComponent();
        }
        DbaseOpretions con = new DbaseOpretions(); // Coonection for implementing direct CRUD Operations
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
            //this.Hide();
            //MainMenu loadmainMenuform = new MainMenu();
            //loadmainMenuform.Show();
        }

        private bool IfUserExist(string sKey)
        {
            con.getData("Select serialKey FROM UserValidation WHERE SerialKey='" + sKey + "'");
            DataTable dt = new DataTable();
            con.sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Validation()
        {
            bool result = false;
            if (string.IsNullOrEmpty(txtConsultantName.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtConsultantName, "Consultant Name is Required");
            }
            else if (string.IsNullOrEmpty(txtCompanyName.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtCompanyName, "Company / Organization Name is Required");
            }
            else if (string.IsNullOrEmpty(txtSerialKey.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtSerialKey, "Serial Key is Required... Reload Application to Generate New Serial Key");
            }
            else
            {
                errorProvider1.Clear();
                result = true;
            }
            return result;
        }

        //Converting Photo to Binary to Enable Database Storage
        byte[] ConvertImageToBinary(Image img)
        {
            using(MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.Text == "Update")
            {
                if (Validation())
                {
                    try
                    {
                        string sql = @"INSERT INTO UserValidation (ConsultantName, CompanyName,Photo,IsActive,SerialKey)
                                                   VALUES (@a1,@a2,@a3,@a4,@a5)";
                        SqlCommand cmd = new SqlCommand(sql, dbcon);
                        cmd.Parameters.Add(new SqlParameter("a1", txtConsultantName.Text));
                        cmd.Parameters.Add(new SqlParameter("a2", txtCompanyName.Text));
                        cmd.Parameters.Add(new SqlParameter("a3", ConvertImageToBinary(picPhoto.Image)));
                        cmd.Parameters.Add(new SqlParameter("a4", status));
                        cmd.Parameters.Add(new SqlParameter("a5", txtSerialKey.Text));

                        if (dbcon.State != ConnectionState.Open)
                            dbcon.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("User Validation Successfull...!", "Access Granted",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        MyGlobalVariables.consulatantName = txtConsultantName.Text;
                        MyGlobalVariables.companyName = txtCompanyName.Text;
                        this.Hide();
                        MainMenu loadmainMenuform = new MainMenu();
                        loadmainMenuform.Show();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        if (dbcon.State == ConnectionState.Open)
                            dbcon.Close();
                    }
                }

            }
            else 
            {
                if (txtSerialKey.Text == MyGlobalVariables.uniqueID)
                {
                    MessageBox.Show("You are Welcome...!", "Access Granted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MyGlobalVariables.consulatantName = txtConsultantName.Text;
                    MyGlobalVariables.companyName = txtCompanyName.Text;
                    this.Hide();
                    MainMenu loadmainMenuform = new MainMenu();
                    loadmainMenuform.Show();
                }
                else
                {
                    MessageBox.Show("Serial Key(s) are in Conflict..! Unable to Verify User", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Application.Exit();
                }
            }
            
        }

        private void UserValidation_Load(object sender, EventArgs e)
        {
            txtSerialKey.Text = MyGlobalVariables.serialKeyID;
            txtCompanyName.Text = MyGlobalVariables.companyName;
            txtConsultantName.Text = MyGlobalVariables.consulatantName;
            chkIsActive.Checked = MyGlobalVariables.isActive;
            
            if (MyGlobalVariables.pImg == null)
                picPhoto.Image = Properties.Resources.Safety;
            else
            {
                MemoryStream pms = new MemoryStream(MyGlobalVariables.pImg);
                picPhoto.Image = Image.FromStream(pms);
                MyGlobalVariables.pBuffer = MyGlobalVariables.pImg;
            }
            if (chkIsActive.Checked == true)
            {
                txtConsultantName.ReadOnly = true;
                txtCompanyName.ReadOnly = true;
                chkIsActive.Enabled = false;
                btnBrowsePhoto.Enabled = false;
                btnSave.Text = "Verify";
            }
            else
            {
                txtConsultantName.ReadOnly = false;
                txtCompanyName.ReadOnly = false;
                chkIsActive.Enabled = true;
                btnBrowsePhoto.Enabled = true;
                btnSave.Text = "Update";
                txtSerialKey.Text = MyGlobalVariables.uniqueID;
            }
        }

        private void btnBrowsePhoto_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPEG|*.jpg", ValidateNames = true, Multiselect = false })
            {
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    fileName = ofd.FileName;
                    picPhoto.Image = Image.FromFile(fileName);
                }
            }
        }

        private void txtConsultantName_Validated(object sender, EventArgs e)
        {
            txtConsultantName.Text = txtConsultantName.Text.ToUpper();
        }

        private void txtCompanyName_Validated(object sender, EventArgs e)
        {
            txtCompanyName.Text = txtCompanyName.Text.ToUpper();
        }
    }
}
