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
        List<string> current_path = new List<string>();

        public Form1()
        {
            InitializeComponent();
            InitForm();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)  // выпадающщий список с именем диска
        {
            string root_dir = @$"{this.comboBox1.SelectedItem}";
            current_path.Clear();
            current_path.Add(root_dir[..(root_dir.Length - 1)]);

            string[] directories = Directory.GetDirectories(root_dir);
            string[] files = Directory.GetFiles(root_dir);
            
            List<string> output_dirs = new List<string>();
            int root_dir_size = root_dir.Length;
            for (int i = 0; i < directories.Length; i += 1)
            {
                if (!directories[i].Contains("$"))
                    output_dirs.Add(directories[i][root_dir_size..]);
            }
            for (int i = 0; i < files.Length; i += 1)
                output_dirs.Add(files[i][root_dir_size..]);

            this.listBox1.Items.Clear();
            this.listBox1.Items.AddRange(output_dirs.ToArray());
        }

        private void InitForm()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (var elem in drives)
                this.comboBox1.Items.Add(elem.ToString()[..elem.ToString().Length]);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = this.listBox1.SelectedItem.ToString();
            this.textBox1.Text = value;
        }

        private void EnterPushed(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string value = this.listBox1.SelectedItem.ToString();
                current_path.Add(value);

                string path = string.Join(@"\", current_path);
                try
                {
                    string[] output_dirs = Directory.GetDirectories(path);
                    string[] output_files = Directory.GetFiles(path);
                    for (int i = 0; i < output_dirs.Length; i += 1)
                        output_dirs[i] = output_dirs[i][(path.Length + 1)..];
                    for (int i = 0; i < output_files.Length; i += 1)
                        output_files[i] = output_files[i][(path.Length + 1)..];

                    this.textBox1.Text = "";
                    this.listBox1.Items.Clear();
                    this.listBox1.Items.AddRange(output_dirs.ToArray());
                    this.listBox1.Items.AddRange(output_files.ToArray());
                } catch (IOException)
                {
                    
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
