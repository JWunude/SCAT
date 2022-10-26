using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCAT
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);


        private void btnSlide_Click(object sender, EventArgs e)
        {
            if (MenuVertical.Width == 265)
            {
                MenuVertical.Width = 90;
            }
            else
            {
                MenuVertical.Width = 265;
            }
        }

        private void iconClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iconMaximize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            iconRestor.Visible = true;
            iconMaximize.Visible = false;
        }

        private void iconRestor_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            iconRestor.Visible = false;
            iconMaximize.Visible = true;
        }

        private void iconMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            lblConsultant.Text = MyGlobalVariables.consulatantName;
            lblCompany.Text = MyGlobalVariables.companyName;

            if (MyGlobalVariables.pImg == null)
                picPhoto.Image = Properties.Resources.Safety;
            else
            {
                MemoryStream pms = new MemoryStream(MyGlobalVariables.pImg);
                picPhoto.Image = Image.FromStream(pms);
                MyGlobalVariables.pBuffer = MyGlobalVariables.pImg;
            }
        }
    }
}
