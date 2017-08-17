using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ShowSeen.Resources
{
    public partial class RowInfo : Form
    {
        private GUI mainwindow;
        private bool modify; // true = modify, false = add

        public RowInfo(GUI mainW, bool m)
        {
            mainwindow = mainW;
            modify = m;
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime date = dateTimePicker1.Value;
            string  name = textBox1.Text, 
                    se = Convert.ToString(numericUpDown1.Value),
                    ep = Convert.ToString(numericUpDown2.Value), 
                    airs = date.ToString("dd-MMM-yy"),
                    day = Convert.ToString(comboBox1.Text),
                    tbaday = checkedListBox1.GetItemCheckState(0) == CheckState.Checked ? "TRUE" : "FALSE",
                    tbamonth = checkedListBox1.GetItemCheckState(1) == CheckState.Checked ? "TRUE" : "FALSE",
                    tba = checkedListBox1.GetItemCheckState(2) == CheckState.Checked ? "TRUE" : "FALSE";

            string values;
            if (modify)
            {
                values = "ShowName='" + name + "', Season=" + se + ", Episode=" + ep + ", Airs='" + airs + "', [Day]='" + day + "', TBAday=" + tbaday + ", TBAmonth=" + tbamonth + ", TBA=" + tba;
                string query = "UPDATE Shows SET " + values + " WHERE (ID=" + mainwindow.getCurrentId() + ")";
                mainwindow.executeCMDQuery(query);
            }
            else
            {
                values = "('" + name + "'," + se + "," + ep + ",'" + airs + "','" + day + "'," + tbaday + "," + tbamonth + "," + tba + ")";
                string query = "INSERT INTO Shows (ShowName, Season, Episode, Airs, [Day], TBAday, TBAmonth, TBA) VALUES " + values;
                mainwindow.executeCMDQuery(query);
            }
            mainwindow.setChanged();
            this.Close();
        }

    }
}
