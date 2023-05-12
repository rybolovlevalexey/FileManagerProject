using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace FileManagerProject
{
    public partial class SettingsWindow : Form
    {
        public PersonSettings settings = new PersonSettings();

        public SettingsWindow()
        {
            InitializeComponent();
            try
            {
                this.DeserializeForSetWin();
            }
            catch (FileNotFoundException)
            {
                settings = new PersonSettings();
            }
            if (settings.ShouldColorButtons)
                color_buttons_btn.BackColor = Color.Green;
            else
                color_buttons_btn.BackColor = Color.Red;
            this.trackBar1.Value = 5;
            if (settings.BlackThemeOn)
                black_theme_btn.BackColor = Color.Green;
            else
                black_theme_btn.BackColor = Color.Red;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            settings.track_bar_value = trackBar1.Value;
        }

        private void color_buttons_btn_Click(object sender, EventArgs e)
        {
            if (settings.ShouldColorButtons)
            {
                settings.ShouldColorButtons = false;
                color_buttons_btn.BackColor = Color.Red;
            }
            else
            {
                settings.ShouldColorButtons = true;
                color_buttons_btn.BackColor = Color.Green;
            }
        }

        private void black_theme_btn_Click(object sender, EventArgs e)
        {
            if (settings.BlackThemeOn)
            {
                settings.BlackThemeOn = false;
                black_theme_btn.BackColor = Color.Red;
            }
            else
            {
                settings.BlackThemeOn = true;
                black_theme_btn.BackColor = Color.Green;
            }
        }

        public void SerializeFromSetWin()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("person.dat", FileMode.Create))
            {
                // Сериализуем объект в поток
                formatter.Serialize(fs, settings);
            }
        }

        public void DeserializeForSetWin()
        {
            byte[] bytes = File.ReadAllBytes("person.dat");
            MemoryStream stream = new MemoryStream(bytes);
            BinaryFormatter formatter = new BinaryFormatter();
            settings = (PersonSettings)formatter.Deserialize(stream);
        }

        private void SettingsWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.SerializeFromSetWin();
        }
    }
}
