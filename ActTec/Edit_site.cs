using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace ActTec
{
    public partial class Edit_site : Form
    {
        string first_data = "";
        public Edit_site(string data_site)
        {
            InitializeComponent();
            Name_site.Text = data_site.Split('|')[0];
            URL_site.Text = data_site.Split('|')[1];
            Time_ch.Value = Convert.ToInt32(data_site.Split('|')[2]);
            first_data = data_site;
        }

        private void edit_site_btn_Click(object sender, EventArgs e)
        {
            if (Name_site.Text.Trim() == "")
            {
                MessageBox.Show("Имя сайта не указано");
                return;
            }
            else
            {
                Regex regex = new Regex(@"http.*\.\w*");
                if (!regex.IsMatch(URL_site.Text.Trim()))
                {
                    MessageBox.Show("URL имеет не верный формат");
                    return;
                }
            }
            for (int i = 0; i < Properties.Settings.Default.List_url_sites.Count; i++)
            {
                if (Properties.Settings.Default.List_url_sites[i] == first_data)
                {
                    Properties.Settings.Default.List_url_sites.RemoveAt(i);
                    Properties.Settings.Default.List_url_sites.Insert(i, Name_site.Text.Trim() + "|" + URL_site.Text + "|" + Time_ch.Value);
                    Properties.Settings.Default.Save();
                    break;
                }

            }
            this.Close();
        }
    }
}
