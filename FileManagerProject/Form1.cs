using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace FileManagerProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitForm();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string root_dir = @$"{this.comboBox1.SelectedItem}";
            string[] directories = Directory.GetDirectories(root_dir);
            List<string> output_dirs = new List<string>();
            int root_dir_size = root_dir.Length;
            for (int i = 0; i < directories.Length; i += 1)
            {
                if (!directories[i].Contains("$"))
                    output_dirs.Add(directories[i][root_dir_size..]);
            }
            this.listBox1.Items.AddRange(output_dirs.ToArray());
        }

        private void InitForm()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            this.comboBox1.Items.AddRange(drives);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = this.listBox1.SelectedItem.ToString();
            this.textBox1.Text = value;
        }

        private void comboBox1_EnterPushed(object sender, EventArgs e)
        {
            MessageBox.Show("here");
        }
    }
}
