using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FileManagerProject
{
    public partial class SettingsWindow : Form
    {
        public int track_bar_value;
        public bool buttons_colored = false;
        public bool black_theme_on = false;
        public List<Color> sp_buttons_color = new List<Color>();
        public SettingsWindow()
        {
            InitializeComponent();
            if (buttons_colored)
                color_buttons_btn.BackColor = Color.Green;
            else
                color_buttons_btn.BackColor = Color.Red;
            this.trackBar1.Value = 5;
            if (black_theme_on)
                black_theme_btn.BackColor = Color.Green;
            else
                black_theme_btn.BackColor = Color.Red;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            track_bar_value = trackBar1.Value;
        }

        private void color_buttons_btn_Click(object sender, EventArgs e)
        {
            if (buttons_colored)
            {
                buttons_colored = false;
                color_buttons_btn.BackColor = Color.Red;
            }
            else
            {
                buttons_colored = true;
                color_buttons_btn.BackColor = Color.Green;
            }
            sp_buttons_color.Clear();
            if (buttons_colored)
            {
                Random rnd = new Random();
                for (int i = 0; i < 8; i += 1)
                {
                    sp_buttons_color.Add(Color.FromArgb(Convert.ToInt32(rnd.Next(1, 255)), 
                        Convert.ToInt32(rnd.Next(1, 255)), 
                        Convert.ToInt32(rnd.Next(1, 255))));
                }
            }
        }

        private void black_theme_btn_Click(object sender, EventArgs e)
        {
            if (black_theme_on)
            {
                black_theme_on = false;
                black_theme_btn.BackColor = Color.Red;
            }
            else
            {
                black_theme_on = true;
                black_theme_btn.BackColor = Color.Green;
            }
        }
    }
}
