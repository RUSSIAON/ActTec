using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace ActTec
{
    public delegate void update_delegate(int id, int status);
    public partial class Main_Form : Form
    {
        string[] status = new string[] { "URL НЕВЕРНЫЙ!", "Связь не установлена", "Связь установлена" };
        Edit_form form;
        BGW worker;
        public Main_Form()
        {
            InitializeComponent();
            update_delegate ud = Update_data;
            form = new Edit_form();
            get_data();
            worker = new BackGroundWorker(ud);
            worker.BackGroundWorker_start();
        }
        void get_data()
        {
            dataGridView1.Rows.Clear();
            int i = 1;
            foreach (string site in Properties.Settings.Default.List_url_sites)
            {
                dataGridView1.Rows.Add(new object[] { i, site.Split('|')[0], site.Split('|')[1], "Обновляется раз в " + site.Split('|')[2] + "сек." });
                i++;
            }

        }
        void Update_data(int id, int status)
        {
            if (id > dataGridView1.Rows.Count) return;
            try // Если список обновляется, а поток уже отправил инфу о том,
                // чтобы сменить состояние, функция пытается обратится к пустому списку.
            {
                if (status == 2)
                {
                    dataGridView1.Rows[id].Cells[3].Style.BackColor = Color.Green;
                }
                else
                {
                    dataGridView1.Rows[id].Cells[3].Style.BackColor = Color.Red;
                }
                dataGridView1.Rows[id].Cells[3].Value = this.status[status];

                int seconds = Convert.ToInt32(Properties.Settings.Default.List_url_sites[id].Split('|')[2]);

                dataGridView1.Rows[id].Cells[4].Value = DateTime.Now.AddSeconds(seconds).ToString("HH:mm:ss");
            }
            catch { }
        }

        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form.ShowDialog();
            get_data();
            worker.reset_timer();
        }

        private void Main_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            worker.BackGroundWorker_stop();
        }

        private void инфоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Проект Валиева Е.Р.\nТелефон для связи +79520380345\ne-mail: valievevgeny@gmail.com", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string data = Properties.Settings.Default.List_url_sites[e.RowIndex].Split('|')[0] + "|";
            data += Properties.Settings.Default.List_url_sites[e.RowIndex].Split('|')[1] + "|";
            data += Properties.Settings.Default.List_url_sites[e.RowIndex].Split('|')[2];
            new Edit_site(data).ShowDialog();
            get_data();
            worker.reset_timer();
        }
    }
}
