﻿using System;
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
        List<string> saved_paths = new List<string>();

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

        private void Form1_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedItem = this.comboBox1.Items[0];
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string value = this.listBox1.SelectedItem.ToString();
                this.GoFrontDirectory(value);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void GoBackDirectory()  // метод возврата назад по директории
        {
            if (current_path.Count > 1)
            {
                current_path.RemoveAt(current_path.Count - 1);
                string path = string.Join(@"\", current_path);
                if (current_path.Count == 1)
                    path = current_path[0] + @"\";
                string[] output_dirs = Directory.GetDirectories(path);
                string[] output_files = Directory.GetFiles(path);
                List<string> output = new List<string>();
                for (int i = 0; i < output_dirs.Length; i += 1)
                {
                    if (!output_dirs[i].Contains("$"))
                    {
                        if (current_path.Count != 1)
                            output.Add(output_dirs[i][(path.Length + 1)..]);
                        else
                            output.Add(output_dirs[i][(path.Length)..]);
                    }
                }
                for (int i = 0; i < output_files.Length; i += 1)
                {
                    if (!output_files[i].Contains("$"))
                    {
                        if (current_path.Count != 1)
                            output.Add(output_files[i][(path.Length + 1)..]);
                        else
                            output.Add(output_files[i][(path.Length)..]);
                    }
                }

                this.textBox1.Text = "";
                this.listBox1.Items.Clear();
                this.listBox1.Items.AddRange(output.ToArray());

                path = string.Join(@"\", current_path.ToArray()[1..]);
                this.lbl_curdir.Text = path;
            }
        }
        private void GoFrontDirectory(string next_dir)
        {
            current_path.Add(next_dir);

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

                path = string.Join(@"\", current_path.ToArray()[1..]);
                this.lbl_curdir.Text = path;
            }
            catch (IOException)
            {
                MessageBox.Show("Выбран файл, а не папка");
                current_path.RemoveAt(current_path.Count - 1);
            }
            catch (System.UnauthorizedAccessException)
            {
                MessageBox.Show("Невозможно открыть данную папку");
                current_path.RemoveAt(current_path.Count - 1);
            }
        }  // метод движения вглубь директории

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string value = this.listBox1.SelectedItem.ToString();
            this.GoFrontDirectory(value);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)  // обработка нажатий мышкой на Form
        {
            if (e.Button.ToString() == "XButton1")  // кнопка назад на мышке
            {
                this.GoBackDirectory();
            }
        }
        private void listBox1_MouseDown(object sender, MouseEventArgs e)  // обработка нажатий мышки на listBox
        {
            if (e.Button.ToString() == "XButton1")  // кнопка назад на мышке
            {
                this.GoBackDirectory();
            }
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)  // кнопка назад
        {
            this.GoBackDirectory();
        }
    }
}
