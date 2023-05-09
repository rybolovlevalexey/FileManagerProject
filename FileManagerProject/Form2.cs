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
    public partial class Form2 : Form
    {
        List<string> current_path = new List<string>();  // текущий путь
        List<string> saved_paths = new List<string>();  // кнопки для действий над файлами и папками
        List<Button> service_buttons = new List<Button>();  // кнопки с действиями
        string copy_path = "";  // путь который надо копировать
        List<string> path_to_copy = new List<string>();  // путь В который надо копировать

        public Form2()
        {
            InitializeComponent();
            InitForm();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)  // выпадающщий список с именем диска
        {
            string root_dir = @$"{this.comboBox1.SelectedItem}";

            current_path.Clear();
            this.textBox1.Text = "";
            current_path.Add(root_dir[..(root_dir.Length - 1)]);
            this.listBox1.Items.Clear();
            this.listBox1.Items.AddRange(this.MakeOutputDirs(root_dir).ToArray());
        }
        private void InitForm()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (var elem in drives)
                this.comboBox1.Items.Add(elem.ToString()[..elem.ToString().Length]);
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)  // запись имени выбранного элемента в textBox
        {
            if (this.listBox1.SelectedIndex != -1)
            {
                string value = this.listBox1.SelectedItem.ToString();
                this.textBox1.Text = value;
            }
        }
        private void Form2_Load(object sender, EventArgs e)
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
                    if (!output_files[i].Contains("deskt") & !output_files[i].Contains("devlist") & !output_files[i].Contains("Dump") &
                        !output_files[i].Contains("F306") & !output_files[i].Contains("sys"))
                    {
                        if (!output_files[i].Contains("$"))
                        {
                            if (current_path.Count != 1)
                                output.Add(output_files[i][(path.Length + 1)..]);
                            else
                                output.Add(output_files[i][(path.Length)..]);
                        }
                    }
                }
                this.lbl_curdir.Text = "";
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
                    if (!output_files[i].Contains("deskt") & !output_files[i].Contains("devlist") & !output_files[i].Contains("Dump") &
                        !output_files[i].Contains("F306") & !output_files[i].Contains("sys"))
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
        }  // метод движения вглубь директории, передаётся следующая папка
        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string value = this.listBox1.SelectedItem.ToString();
            this.GoFrontDirectory(value);
        }  // двойное нажатие мышкой на список (переход вглубь папки)
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
        private List<string> MakeOutputDirs(string root_output_dir)
        {
            string[] directories = Directory.GetDirectories(root_output_dir);
            string[] files = Directory.GetFiles(root_output_dir);

            List<string> output_dirs = new List<string>();
            int root_dir_size = root_output_dir.Length;
            for (int i = 0; i < directories.Length; i += 1)
            {
                if (!directories[i].Contains("$"))
                    output_dirs.Add(directories[i][root_dir_size..]);
            }
            for (int i = 0; i < files.Length; i += 1)
                if (!files[i].Contains("deskt") & !files[i].Contains("devlist") & !files[i].Contains("Dump") &
                    !files[i].Contains("F306") & !files[i].Contains("sys"))
                    output_dirs.Add(files[i][root_dir_size..]);
            return output_dirs;
        }  // создаёт список файлов и папок в текущей директории

    }
}
