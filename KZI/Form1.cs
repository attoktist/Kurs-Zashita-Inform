using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KZI
{
    public partial class Form1 : Form
    {
        public static string FilePath;
        public static string KeyPath;
        public static string IVPath;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
           
            try
            {
                textBoxStatus.Text = "";
                AES a = new AES(KeyPath);
                textBoxStatus.Text += "Ключ принят\r\n";
                textBoxStatus.Text += a.EncodeCFB(FilePath,IVPath);
            }
            catch(Exception ex)
            {
                textBoxStatus.Text += ex.Message;
            }

            
        }

        private void buttonDecode_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxStatus.Text = "";
                AES a = new AES(KeyPath);
                textBoxStatus.Text += "Ключ принят\r\n";
                //textBoxStatus.Text += a.DecodeCFB(FilePath, IVPath);
                textBoxStatus.Text += a.EncodeCFB(FilePath, IVPath,true);
            }
            catch (Exception ex)
            {
                textBoxStatus.Text += ex.Message;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxStatus.Text = "";
                OpenFileDialog file = new OpenFileDialog();
                file.ShowDialog();
                textBoxFilePath.Text = file.FileName;
                FilePath = file.FileName;
            }
            catch (Exception ex)
            {
                textBoxStatus.Text += ex.Message;
            }

        }

        private void buttonKeyPath_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxStatus.Text = "";
                OpenFileDialog file = new OpenFileDialog();
                file.ShowDialog();                
                KeyPath = file.FileName;
            }
            catch (Exception ex)
            {
                textBoxStatus.Text += ex.Message;
            }
        }

        private void buttonPathIV_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxStatus.Text = "";
                OpenFileDialog file = new OpenFileDialog();
                file.ShowDialog();                
                IVPath = file.FileName;
            }
            catch (Exception ex)
            {
                textBoxStatus.Text += ex.Message;
            }
        }
    }
}
