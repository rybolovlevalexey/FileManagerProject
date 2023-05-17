using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Http;
using HtmlAgilityPack;

namespace FileManagerProject
{
    public partial class AmazonWindow : Form
    {
        // название книги, автора, оценку, цену
        // class="sg-col sg-col-4-of-12 sg-col-4-of-16 sg-col-4-of-20 sg-col-4-of-24"
        // ".//div[@class='a-section']//div[@class='sg-row']//div[@class='sg-col sg-col-4-of-12 sg-col-8-of-16 sg-col-12-of-20 sg-col-12-of-24 s-list-col-right']"
        
        string base_url = "https://www.amazon.com/s?k=python&i=stripbooks-intl-ship&ref=nb_sb_noss";
        public AmazonWindow()
        {
            InitializeComponent();
            InitForm();
        }
        private void InitForm()
        {
            List<int> items_updown = new List<int>();
            for (int i = 20; i > 0; i -= 1)
                items_updown.Add(i);
            domainUpDown1.Items.Clear();
            domainUpDown1.Items.AddRange(items_updown.ToArray());
            domainUpDown1.SelectedIndex = 10;
            domainUpDown1.ReadOnly = true;

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
        }
        private void search_btn_Click(object sender, EventArgs e)
        {
            this.Parsing(base_url);
        }

        private void Parsing(string url)
        {
            try
            {
                HttpClientHandler hdl = new HttpClientHandler { AllowAutoRedirect = false };
                HttpClient client = new HttpClient(hdl);
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36";
                headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                headers["Accept-Language"] = "ru-ru,ru;q=0.8,en-us;q=0.5,en;q=0.3";
                headers["Accept-Encoding"] = "gzip, deflate";
                headers["Connection"] = "keep-alive";
                headers["DNT"] = "1";
                foreach (string key in headers.Keys)
                    client.DefaultRequestHeaders.Add(key, headers[key]);

                var responce = client.GetAsync(url).Result;
                MessageBox.Show(responce.StatusCode.ToString());
                if (responce.IsSuccessStatusCode)
                {
                    MessageBox.Show("here 4");
                    var html = responce.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrEmpty(html))
                    {
                        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                        doc.LoadHtml(html);
                        var tables = doc.DocumentNode.SelectNodes(".//div[@class='a-section']//div[@class='sg-row']");
                        if (tables != null && tables.Count > 0)
                        {
                            Dictionary<string, List<string>> books_info = new Dictionary<string, List<string>>();
                            MessageBox.Show(tables.Count.ToString());
                        }
                        else
                        {
                            MessageBox.Show("No information parsed");
                        }
                    }
                }
                MessageBox.Show("here not good");
            } catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
