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
using System.IO.Compression;

namespace FileManagerProject
{
    public partial class Form1 : Form
    {
        List<string> current_path = new List<string>();  // текущий путь
        List<string> saved_paths = new List<string>();  // кнопки для действий над файлами и папками
        List<Button> service_buttons = new List<Button>();  // кнопки с действиями

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
            ToolTip tool1 = new ToolTip();
            tool1.SetToolTip(this.del_btn, "Delete");
            tool1.SetToolTip(this.copy_btn, "Copy");
            tool1.SetToolTip(this.move_btn, "Move");
            tool1.SetToolTip(this.rename_btn, "Rename");
            tool1.SetToolTip(this.arh_btn, "Archive");
            tool1.SetToolTip(this.razarh_btn, "Unzip");
            tool1.SetToolTip(this.izbr_btn, "Add to favourites");
            tool1.SetToolTip(this.make_btn, "Make");

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
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex != -1)
            {
                string value = this.listBox1.SelectedItem.ToString();
                this.textBox1.Text = value;
            }
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedItem = this.comboBox1.Items[0];
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "ControlKey")  // нажат ctrl, отключается возможность выбирать элементы listBox
            {
                this.listBox1.SelectionMode = SelectionMode.None;
            }
            if (e.Control && e.KeyCode == Keys.C)  // ctrl + C, копирование выбранного элемента, после обратно включается возможность выбора на listBox
            {
                string value = textBox1.Text;
                if (value == "")
                {
                    MessageBox.Show("Выберите элемент,\nкоторый необходимо скопировать");
                    return;
                }
                // путь который надо копировать
                string copy_path = string.Join(@"\", current_path);
                if (current_path.Count == 1)
                    copy_path += @"\";
                copy_path += @"\" + value;

                // получение пути в который надо копировать
                Form2 choose_dir = new Form2();
                choose_dir.ShowDialog();
                switch (choose_dir.status_code)
                {
                    case 0:
                        break;
                    case 1:
                        if (File.Exists(choose_dir.path_to_copy + @"\" + value))
                        {
                            MessageBox.Show("Файл с таким названием уже существует в новой директории");
                            return;
                        }
                        if (File.Exists(copy_path) && !File.Exists(choose_dir.path_to_copy + @"\" + copy_path[copy_path.LastIndexOf(@"\")..]))
                        {
                            File.Copy(copy_path, choose_dir.path_to_copy + @"\" + value);
                        }
                        else
                        {
                            this.CopyDir(copy_path, choose_dir.path_to_copy);
                        }
                        break;
                    case 2:
                        break;
                }


                this.listBox1.SelectionMode = SelectionMode.One;
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (this.listBox1.SelectedIndex != -1)
                {
                    string value = this.listBox1.SelectedItem.ToString();
                    this.GoFrontDirectory(value);
                }
            }
            if (e.KeyCode == Keys.Delete)
            {
                if (this.listBox1.SelectedIndex != -1)
                {
                    DialogResult result = MessageBox.Show("Вы точно хотите удалить выбранный элемент?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        string value = this.listBox1.SelectedItem.ToString();
                        string path = string.Join(@"\", current_path);
                        if (current_path.Count == 1)
                            path += @"\";
                        path += @"\" + value;
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                            MessageBox.Show("Файл удалён");
                        }
                        else
                        {
                            this.DeleteDir(path);
                            MessageBox.Show("Папка удалена");
                        }
                        listBox1.Items.Clear();
                        path = string.Join(@"\", current_path);
                        if (current_path.Count == 1)
                            path += @"\";
                        listBox1.Items.AddRange(this.MakeOutputDirs(path).ToArray());
                        textBox1.Text = "";
                    }
                }
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
                        if (!output_files[i].Contains("deskt") && !output_files[i].Contains("devlist") && !output_files[i].Contains("Dump") &&
                            !output_files[i].Contains("F306") && !output_files[i].Contains("sys"))
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
                        if (!output_files[i].Contains("deskt") && !output_files[i].Contains("devlist") && !output_files[i].Contains("Dump") &&
                            !output_files[i].Contains("F306") && !output_files[i].Contains("sys"))
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
                    if (!output_files[i].Contains("deskt") && !output_files[i].Contains("devlist") && !output_files[i].Contains("Dump") &&
                        !output_files[i].Contains("F306") && !output_files[i].Contains("sys"))
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
            if (this.listBox1.SelectedIndex != -1)
            {
                string value = this.listBox1.SelectedItem.ToString();
                this.GoFrontDirectory(value);
            }
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
                    if (!files[i].Contains("deskt") && !files[i].Contains("devlist") && !files[i].Contains("Dump") &&
                        !files[i].Contains("F306") && !files[i].Contains("sys"))
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
                    if (!files[i].Contains("deskt") && !files[i].Contains("devlist") && !files[i].Contains("Dump") &&
                        !files[i].Contains("F306") && !files[i].Contains("sys"))
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
                        this.DeleteDir(path + value);
                        MessageBox.Show("Выранная папка удалена");
                    }
                }
                this.listBox1.Items.Clear();
                this.listBox1.Items.AddRange(this.MakeOutputDirs(path).ToArray());
                this.textBox1.Text = "";
            }
        }

        private void copy_btn_Click(object sender, EventArgs e)  // копирование элемента в новое место, если в списке курсор стоит на директории то копирование будет внутрь этой директории
        {
            string value = textBox1.Text;
            if (value == "")
            {
                MessageBox.Show("Выберите элемент,\nкоторый необходимо скопировать");
                return;
            }
            // путь который надо копировать
            string copy_path = string.Join(@"\", current_path);
            if (current_path.Count == 1)
                copy_path += @"\";
            copy_path += @"\" + value;

            // получение пути в который надо копировать
            Form2 choose_dir = new Form2();
            choose_dir.ShowDialog();
            switch (choose_dir.status_code)
            {
                case 0:
                    break;
                case 1:
                    if (File.Exists(choose_dir.path_to_copy + @"\" + value))
                    {
                        MessageBox.Show("Файл с таким названием уже существует в новой директории");
                        return;
                    }
                    if (File.Exists(copy_path) && !File.Exists(choose_dir.path_to_copy + @"\" + copy_path[copy_path.LastIndexOf(@"\")..]))
                    {
                        File.Copy(copy_path, choose_dir.path_to_copy + @"\" + value);
                    } else
                    {
                        this.CopyDir(copy_path, choose_dir.path_to_copy);
                    }
                    break;
                case 2:
                    break;
            }

        }

        private void CopyDir(string from_dir, string to_dir)
        {
            to_dir += from_dir[from_dir.LastIndexOf(@"\")..];
            Directory.CreateDirectory(to_dir);
            foreach (string s in Directory.GetFiles(from_dir))
                if (!File.Exists(to_dir + @"\" + Path.GetFileName(s)))
                    File.Copy(s, to_dir + @"\" + Path.GetFileName(s));
            foreach (string s in Directory.GetDirectories(from_dir))
                CopyDir(s, to_dir + @"\" + Path.GetFileName(s));
        }
        private void DeleteDir(string path)  // рекурентное удаление папок и их содержимого
        {
            foreach (string s in Directory.GetFiles(path))
                File.Delete(s);
            if (Directory.GetDirectories(path).Length == 0)
                Directory.Delete(path);
            else
            {
                foreach (string s in Directory.GetDirectories(path))
                    DeleteDir(s);
                Directory.Delete(path);
            }
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

        private void move_btn_Click(object sender, EventArgs e)  // перемещение файла или директории
        {
            string value = this.textBox1.Text;
            if (value == "")
            {
                MessageBox.Show("Для перемещения выберите элемент");
                return;
            }
            // путь который надо переместить
            string copy_path = string.Join(@"\", current_path);
            if (current_path.Count == 1)
                copy_path += @"\";
            copy_path += @"\" + value;

            // получение пути в который надо переместить
            Form2 choose_dir = new Form2();
            choose_dir.ShowDialog();
            switch (choose_dir.status_code)
            {
                case 0:
                    break;
                case 1:
                    if (File.Exists(choose_dir.path_to_copy + @"\" + value))
                    {
                        MessageBox.Show("Файл с таким названием уже существует в новой директории");
                        return;
                    }
                    if (File.Exists(copy_path) && !File.Exists(choose_dir.path_to_copy + @"\" + copy_path[copy_path.LastIndexOf(@"\")..]))
                    {
                        File.Copy(copy_path, choose_dir.path_to_copy + @"\" + value);
                        MessageBox.Show("Выбранный файл перемещён");
                    }
                    else
                    {
                        if (Directory.Exists(copy_path) && !Directory.Exists(choose_dir.path_to_copy + @"\" + copy_path[copy_path.LastIndexOf(@"\")..]))
                        {
                            this.CopyDir(copy_path, choose_dir.path_to_copy);
                            MessageBox.Show("Выбранная папка перемещена");
                        } else
                        {
                            MessageBox.Show("Не верные вводные для перемещения,\nпопробуйте ещё раз");
                        }
                    }
                    break;
                case 2:
                    break;
            }

            // удаление пути в старом расположении
            this.DeleteDir(copy_path);

            string path = string.Join(@"\", current_path);
            if (current_path.Count == 1)
                path += @"\";
            this.listBox1.Items.Clear();
            this.listBox1.Items.AddRange(this.MakeOutputDirs(path).ToArray());
            this.textBox1.Text = "";
        }
        private void arh_btn_Click(object sender, EventArgs e)  // архивирование выбранного элемента
        {
            string path = string.Join(@"\", current_path);
            if (current_path.Count == 1)
                path += @"\";
            if (textBox1.Text == "")
            {
                MessageBox.Show("Выберите элемент который\nнеобходимо архивировать");
                return;
            }
            path += @"\" + textBox1.Text;
            if (Directory.Exists(path))
            {
                ZipFile.CreateFromDirectory(path, path + ".zip");
                MessageBox.Show("Папка архивирована");
            }
            else
            {
                int count = 1;
                if (Directory.Exists(path[..path.LastIndexOf(".")])) {
                    while (Directory.Exists(path[..path.LastIndexOf(".")] + count.ToString()))
                        count += 1;
                    Directory.CreateDirectory(path[..path.LastIndexOf(".")] + count.ToString());
                    File.Copy(path, path[..path.LastIndexOf(".")] + count.ToString());
                    ZipFile.CreateFromDirectory(path[..path.LastIndexOf(".")] + count.ToString(), path[..path.LastIndexOf(".")] + count.ToString() + ".zip");
                } else
                {
                    Directory.CreateDirectory(path[..path.LastIndexOf(".")]);
                    File.Copy(path, path[..path.LastIndexOf(".")] + @"\" + textBox1.Text);
                    ZipFile.CreateFromDirectory(path[..path.LastIndexOf(".")], path[..path.LastIndexOf(".")] + ".zip");
                }
                MessageBox.Show("Файл архивирован");
            }
            
            path = string.Join(@"\", current_path);
            if (current_path.Count == 1)
                path += @"\";
            this.listBox1.Items.Clear();
            this.listBox1.Items.AddRange(this.MakeOutputDirs(path).ToArray());
            this.textBox1.Text = "";
        }
        private void razarh_btn_Click(object sender, EventArgs e)
        {
            string path = string.Join(@"\", current_path);
            if (current_path.Count == 1)
                path += @"\";
            if (textBox1.Text == "")
            {
                MessageBox.Show("Выберите элемент который\nнеобходимо разархивировать");
                return;
            }
            path += @"\" + textBox1.Text;
            if (!path.Contains(".zip"))
            {
                MessageBox.Show("Вы выбрали не архив");
                return;
            }
            int count = 1;
            if (Directory.Exists(path[..path.LastIndexOf(".zip")]))
            {
                while (Directory.Exists(path[..path.LastIndexOf(".zip")] + count.ToString()))
                    count += 1;
                ZipFile.ExtractToDirectory(path, path[..path.LastIndexOf(".zip")] + count.ToString());
                MessageBox.Show($"Выбранный архив разархивирован\nв папку {path[..path.LastIndexOf(".zip")] + count.ToString()}");
            }
            else
            {
                ZipFile.ExtractToDirectory(path, path[..path.LastIndexOf(".zip")]);
                MessageBox.Show($"Выбранный архив разархивирован\nв папку {path[..path.LastIndexOf(".zip")]}");
            }
            
            
            path = string.Join(@"\", current_path);
            if (current_path.Count == 1)
                path += @"\";
            this.listBox1.Items.Clear();
            this.listBox1.Items.AddRange(this.MakeOutputDirs(path).ToArray());
            this.textBox1.Text = "";
        }

        private void izbr_btn_Click(object sender, EventArgs e)
        {

        }
    }
}
