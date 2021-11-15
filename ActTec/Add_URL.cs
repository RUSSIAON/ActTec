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
    public partial class Add_URL : Form
    {
        public Add_URL()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
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
            Properties.Settings.Default.List_url_sites.Add(Name_site.Text.Trim() + "|" + URL_site.Text + "|" + Time_ch.Value);
            //Properties.Settings.Default.List_url += Name_site.Text.Trim() + "||" + URL_site.Text + "||" + Time_ch.Value + "\n";
            Properties.Settings.Default.Save();
            this.Close();
        }
    }
}
