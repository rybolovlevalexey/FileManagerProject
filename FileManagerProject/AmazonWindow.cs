using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Http;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace FileManagerProject
{
    public partial class AmazonWindow : Form
    {
        // название книги, автора, оценку, цену
        // class="sg-col sg-col-4-of-12 sg-col-4-of-16 sg-col-4-of-20 sg-col-4-of-24"
        // ".//div[@class='a-section']//div[@class='sg-row']//div[@class='sg-col sg-col-4-of-12 sg-col-8-of-16 sg-col-12-of-20 sg-col-12-of-24 s-list-col-right']"
        
        string base_url = "https://www.amazon.com/s?k=python&i=stripbooks-intl-ship&ref=nb_sb_noss";
        List<List<string>> current_result = new List<List<string>>();

        WebClient client = new WebClient();
        List<string> my_URL_books = new List<string>();
        public AmazonWindow()
        {
            InitializeComponent();
            InitForm();
        }
        private void InitForm()
        {
            listView1.Scrollable = true;
            listView1.FullRowSelect = true;

            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/111.0");
            client.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
            client.Headers.Add("Accept-Encoding", "br");
            client.Headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");

            // создание переключателя для выбора кол-ва книжек
            List<int> items_updown = new List<int>();
            for (int i = 20; i > 0; i -= 1)
                items_updown.Add(i);
            domainUpDown1.Items.Clear();
            domainUpDown1.Items.AddRange(items_updown.ToArray());
            domainUpDown1.SelectedIndex = 10;
            domainUpDown1.ReadOnly = true;

            // наполнение выпадающего списка с языками программирования
            List<string> items_combo = new List<string>();
            items_combo.Add("Python");
            items_combo.Add("C++");
            items_combo.Add("Java");
            items_combo.Add("C#");
            items_combo.Add("Kotlin");
            items_combo.Add("HTML");
            items_combo.Add("CSS");
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(items_combo.ToArray());
            comboBox1.SelectedIndex = 0;

            // создание таблицы с шапкой в list view

            listView1.ColumnClick += listView1_ColumnClick;
            listView1.Columns.Add("Номер", 10);
            listView1.Columns.Add("Название", 250);
            listView1.Columns.Add("Автор", 150);
            listView1.Columns.Add("Рэйтинг", 100);
            listView1.Columns.Add("Цена", 100);
            listView1.Columns.Add("Дата", 100);
        }
        private void search_btn_Click(object sender, EventArgs e)
        {
            int item_count = Convert.ToInt32(domainUpDown1.SelectedItem);
            string program_language = comboBox1.SelectedItem.ToString();
            this.TryDownload(program_language, item_count);
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {

        }
        private void TryDownload(string language, int count)
        {
            List<string> listformyline = new List<string>();
            listView1.Items.Clear();
            my_URL_books.Clear();


            string filepath = @"C:\C# Projects from VS\Amazon.txt";
            File.WriteAllText(filepath, string.Empty);
            string address = @$"https://www.amazon.com/s?k={language}&i=stripbooks-intl-ship&ref=nb_sb_noss";
            MessageBox.Show(address);
            int page = 1;
            int current_count = 0;
            string reply = "";

            while (current_count != count)
            {
                File.WriteAllText(filepath, string.Empty);

                client.DownloadFile(address, filepath);
                string[] HTMLine = File.ReadAllLines(filepath, Encoding.Default);

                StreamWriter sw = new StreamWriter(@"C:\C# Projects from VS\Amazon1.txt");
                foreach (string s in HTMLine)
                {
                    if (s.Contains("data-component-type=\"s-search-result\""))
                    {
                        listformyline.Add(s);
                        sw.WriteLine(s);
                    }
                }
                sw.Close();

                reply = client.DownloadString(address);
                foreach (string match in listformyline)
                {
                    string this_author = "Неизвестно";
                    string this_date = "Неизвестно";
                    string this_mark = "Неизвестно";
                    string this_cost = "Неизвестно";

                    Regex r = new Regex("<a class=\"a-link-normal s-no-outline\" href=\"([^>]+)\"");
                    MatchCollection m = r.Matches(match);
                    string t = "https://www.amazon.com" + m[0].Groups[1].ToString();
                    my_URL_books.Add(t);


                    //Find mark
                    Regex regex_marks = new Regex(@"(\d\.\d) out of 5 stars");
                    MatchCollection matches_marks = regex_marks.Matches(match);
                    try { this_mark = matches_marks[0].Groups[1].ToString(); }
                    catch { }

                    //Find cost
                    Regex regex_cost = new Regex(@"(\$\d{1,3}\.\d{1,3})");
                    MatchCollection matches_cost = regex_cost.Matches(match);
                    try { this_cost = matches_cost[0].Groups[1].ToString(); }
                    catch { }

                    //Find date
                    Regex regex_date = new Regex(@"(\w{3} \d{1,2}, \d{4})");
                    MatchCollection match_date = regex_date.Matches(match);
                    try { this_date = match_date[0].Groups[1].ToString(); this_date = this_date.Replace(" ", "").Replace(",", "."); }
                    catch { }

                    //find tittle                  
                    Regex regex_name = new Regex("<span class=\"a-size-medium a-color-base a-text-normal\">([^<]+)</span>");
                    MatchCollection matches_name = regex_name.Matches(match);

                    //find author
                    if (match.Contains("by </span><span"))
                    {
                        Regex regex_author = new Regex("by </span><span class=\"a-size-base\">([^>]+)</span>");
                        MatchCollection matches_author = regex_author.Matches(match);
                        try { this_author = matches_author[0].Groups[1].ToString(); }
                        catch { }
                    }
                    else
                    {
                        Regex regex_author = new Regex("by </span><a class([^>]+)>([^<]+)</a>");
                        MatchCollection matches_author = regex_author.Matches(match);
                        try { this_author = matches_author[0].Groups[2].ToString(); }
                        catch { }
                    }

                    if (current_count < count)
                    {
                        current_count += 1;
                        List<string> line = new List<string>();
                        line.Add(current_count.ToString());
                        line.Add(matches_name[0].Groups[1].ToString());
                        line.Add(this_author);
                        line.Add(this_mark);
                        line.Add(this_cost);
                        line.Add(this_date);
                        ListViewItem new_line = new ListViewItem(new string[] {
                            current_count.ToString(),
                            matches_name[0].Groups[1].ToString(),
                            this_author,
                            this_mark,
                            this_cost,
                            this_date });
                        listView1.Items.Add(new_line);
                        current_result.Add(line);
                    }
                }
                if (current_count < count)
                {
                    Regex regex_page = new Regex("<a href=\"(.+)aria-label=\"Go to page " + Convert.ToString(page + 1));
                    page += 1;
                    MatchCollection matches_page = regex_page.Matches(reply);
                    address = "https://www.amazon.com" + matches_page[0].Groups[1];
                }
            }
        }
    }
}
