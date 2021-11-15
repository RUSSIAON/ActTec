using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ActTec
{
    public partial class Edit_form : Form
    {
        Add_URL AU;
        public Edit_form()
        {
            InitializeComponent();
            AU = new Add_URL();
            update_list();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }



        void update_list()
        {
            checkedListBox1.Items.Clear();
            foreach (string s in Properties.Settings.Default.List_url_sites)
            {
                checkedListBox1.Items.Add(s.Split('|')[0] + "\t" + s.Split('|')[1] + "\t" + s.Split('|')[2] + "");
            }
            checkedListBox1_SelectedValueChanged(null, null);
        }

        void remove_elem()
        {
            if (DialogResult.No == MessageBox.Show("Вы уверенны что хотите удалить выбранные сайт(ы) из списка?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) return;

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i) == true)
                {
                    string site_data = checkedListBox1.Items[i].ToString().Replace('\t', '|');
                    Properties.Settings.Default.List_url_sites.Remove(site_data);
                    Properties.Settings.Default.Save();
                }
            }
            update_list();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AU.ShowDialog();
            update_list();
        }

        private void Edit_form_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void checkedListBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            int count_selected = 0;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i) == true) count_selected++;
            }
            button2.Enabled = (count_selected == 1) ? true : false;
            button3.Enabled = (count_selected >= 1) ? true : false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            remove_elem();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i) == true)
                {
                    string site_data = checkedListBox1.Items[i].ToString().Replace('\t', '|');
                    Edit_site es = new Edit_site(site_data);
                    es.ShowDialog();
                    break;
                }
            }
            update_list();
        }
    }
}
