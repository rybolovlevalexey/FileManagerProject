using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace FileManagerProject
{
    [Serializable]
    public struct PersonSettings
    {
        private bool black_theme_on;  // тёмная тема включена
        private bool should_color_buttons;  // надо покрасить кнопки
        public List<Color> sp_buttons_color;  // список рандомных цветов для заливки кнопок
        public int track_bar_value;  // размер шрифта

        public PersonSettings(bool black, bool color, int track_bar)
        {
            black_theme_on = black;
            should_color_buttons = color;
            track_bar_value = track_bar;
            sp_buttons_color = new List<Color>();
        }

        public bool BlackThemeOn
        {
            get { return black_theme_on; }
            set
            {
                black_theme_on = value;
                if (black_theme_on)
                {
                    if (should_color_buttons)
                        should_color_buttons = false;
                }
            }
        }
        public bool ShouldColorButtons
        {
            get { return should_color_buttons; }
            set
            {
                should_color_buttons = value;
                sp_buttons_color.Clear();
                Random rnd = new Random();
                for (int i = 0; i < 10; i += 1)
                    sp_buttons_color.Add(Color.FromArgb(rnd.Next(1, 255), 
                        rnd.Next(1, 255), rnd.Next(1, 255)));
            }
        }
    }
}
