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
        List<string> current_path = new List<string>();  // текущий путь
        List<string> saved_paths = new List<string>();  // кнопки для действий над файлами и папками
        List<Button> service_buttons = new List<Button>();  // кнопки с действиями
        string copy_path = "";  // путь который надо копировать
        List<string> path_to_copy = new List<string>();  // путь В который надо копировать

        public Form1()
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
            service_buttons.Add(this.del_btn);
            service_buttons.Add(this.rename_btn);
            service_buttons.Add(this.copy_btn);
            service_buttons.Add(this.arh_btn);
            service_buttons.Add(this.izbr_btn);
            service_buttons.Add(this.move_btn);
            service_buttons.Add(this.make_btn);
            this.cancel_btn.Visible = false;
            this.ok_button.Visible = false;
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
                if (toolStripLabel2.Text == "Сначала папки")
                {
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
                } else
                {
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
                this.listBox1.Items.Clear();
                if (toolStripLabel2.Text == "Сначала папки")
                {
                    this.listBox1.Items.AddRange(output_dirs.ToArray());
                    this.listBox1.Items.AddRange(output_files.ToArray());
                } else
                {
                    this.listBox1.Items.AddRange(output_files.ToArray());
                    this.listBox1.Items.AddRange(output_dirs.ToArray());
                }

                this.textBox1.Text = "";
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

        private void rename_btn_Click(object sender, EventArgs e)
        {
            string new_value = textBox1.Text;
            string old_value;
            string index = listBox1.SelectedIndex.ToString();
            try
            {
                if (index != "-1")
                    old_value = listBox1.SelectedItem.ToString();
                else
                {
                    MessageBox.Show("Выберите элемент,\nкоторый хотите переименовать");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Выберите элемент,\nкоторый хотите переименовать");
                return;
            }
            if (old_value == new_value)
            {
                MessageBox.Show("Чтобы переименовать выбранный элемент\nвнести изменения в его название");
                return;
            }
            if (new_value == "")
            {
                MessageBox.Show("Новое имя не может быть пустым");
                return;
            }
            
            string path = string.Join(@"\", current_path);
            if (current_path.Count == 1)
                path += @"\";
            if (File.Exists(path + @"\" + old_value))
            {
                if (new_value.EndsWith(Path.GetExtension(path + @"\" + old_value)))
                    File.Move(path + @"\" + old_value, path + @"\" + new_value);
                else
                    MessageBox.Show("Нельзя менять расширение файла");
            }
            else
            {
                Directory.Move(path + @"\" + old_value, path + @"\" + new_value);
            }
            this.listBox1.Items.Clear();
            this.listBox1.Items.AddRange(MakeOutputDirs(path).ToArray());
            this.listBox1.SelectedIndex = Convert.ToInt32(index);
        }  // переименование элемента, только если он выбран и новое название не противоречит исходному расширению
        private List<string> MakeOutputDirs(string root_output_dir)
        {
            string[] directories = Directory.GetDirectories(root_output_dir);
            string[] files = Directory.GetFiles(root_output_dir);

            List<string> output_dirs = new List<string>();
            int root_dir_size = root_output_dir.Length;
            int delta = 0;
            if (toolStripLabel2.Text == "Сначала папки")
            {
                for (int i = 0; i < directories.Length; i += 1)
                {
                    delta = 0;
                    if (!directories[i].Contains("$"))
                    {
                        while (directories[i][(root_dir_size + delta)..][0].ToString() == @"\")
                            delta += 1;
                        output_dirs.Add(directories[i][(root_dir_size + delta)..]);
                    }
                }
                for (int i = 0; i < files.Length; i += 1)
                {
                    delta = 0;
                    if (!files[i].Contains("deskt") & !files[i].Contains("devlist") & !files[i].Contains("Dump") &
                        !files[i].Contains("F306") & !files[i].Contains("sys"))
                    {
                        while (files[i][(root_dir_size + delta)..][0].ToString() == @"\")
                            delta += 1;
                        output_dirs.Add(files[i][(root_dir_size + delta)..]);
                    }
                }
            } else
            {
                for (int i = 0; i < files.Length; i += 1)
                {
                    delta = 0;
                    if (!files[i].Contains("deskt") & !files[i].Contains("devlist") & !files[i].Contains("Dump") &
                        !files[i].Contains("F306") & !files[i].Contains("sys"))
                    {
                        while (files[i][(root_dir_size + delta)..].StartsWith(@"\"))
                            delta += 1;
                        output_dirs.Add(files[i][(root_dir_size + delta)..]);
                    }
                }
                for (int i = 0; i < directories.Length; i += 1)
                {
                    delta = 0;
                    if (!directories[i].Contains("$"))
                    {
                        while (directories[i][(root_dir_size + delta)..][0].ToString() == @"\")
                            delta += 1;
                        output_dirs.Add(directories[i][(root_dir_size + delta)..]);
                    }
                }
            }
            return output_dirs;
        }  // создаёт список файлов и папок в текущей директории
        private void del_btn_Click(object sender, EventArgs e)  // удаление выбранного элемента после подтверждения
        {
            string value = this.textBox1.Text;
            if (value == "")
            {
                MessageBox.Show("Для удаления выберите элемент");
                return;
            }
            DialogResult result = MessageBox.Show("Вы точно хотите удалить выбранный элемент?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string path = string.Join(@"\", current_path);
                if (current_path.Count == 1)
                    path += @"\";
                if (File.Exists(path + value))
                {
                    File.Delete(path + value);
                    MessageBox.Show("Выбранный файл удалён");
                }
                else
                {
                    if (Directory.Exists(path + value))
                    {
                        Directory.Delete(path + value);
                        MessageBox.Show("Выранная папка удалена");
                    }
                }
                this.listBox1.Items.Clear();
                this.listBox1.Items.AddRange(this.MakeOutputDirs(path).ToArray());
                this.textBox1.Text = "";
            }
        }

        private void copy_btn_Click(object sender, EventArgs e)
        {
            string value = textBox1.Text;
            if (value == "")
            {
                MessageBox.Show("Выберите элемент,\nкоторый необходимо скопировать");
                return;
            }

            Form2 choose_dir = new Form2();
            choose_dir.ShowDialog();
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)  // смена порядка вывода сначала папки/файлы
        {
            if (toolStripLabel2.Text == "Сначала папки")
            {
                toolStripLabel2.Text = "Сначала файлы";
            } else
            {
                toolStripLabel2.Text = "Сначала папки";
            }
            listBox1.Items.Clear();
            string path = string.Join(@"\", current_path);
            if (current_path.Count == 1)
                path += @"\";
            listBox1.Items.AddRange(this.MakeOutputDirs(path).ToArray());
        }
    }
}
