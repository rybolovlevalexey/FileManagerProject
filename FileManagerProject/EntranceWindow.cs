using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FileManagerProject
{
    public partial class EntranceWindow : Form
    {
        public UsersInformation information = new UsersInformation();
        public EntranceWindow()
        {
            InitializeComponent();
            InitForm();
        }

        private void InitForm()
        {
            this.comboBox1.Items.Clear();
            this.comboBox1.Items.AddRange(information.GetUsersLogin().ToArray());
        }

        private void entr_btn_Click(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count == 0)
            {
                MessageBox.Show("Нет зарегистрированных пользователей");
                return;
            }
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите логин под которым вы хотите войти");
                return;
            }
            string login = comboBox1.SelectedItem.ToString();
            string password = textBox2.Text;
            if (!information.IsCorrectPasswordForUser(login, password))
            {
                MessageBox.Show("Для указанного логина введён неверный пароль");
                return;
            }
            MessageBox.Show($"Добро пожаловать {login}");
            information.user_now = login;
            information.status_id = 1;
        }  // вход пользователя в уже созданный аккаунт после ввода пароля

        private void registr_btn_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            string res = information.AppendNewUser(login, password);
            MessageBox.Show(res);
            if (res == $"Новый пользователь с логином {login} добавлен")
            {
                textBox1.Text = "";
                textBox2.Text = "";
                InitForm();
            }
        }

        private void EntranceWindow_Load(object sender, EventArgs e)
        {
            InitForm();
        }
    }
}
