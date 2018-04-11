using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using ShowSeen.Resources;
using System.Net;
using System.IO;
using CsQuery;
using System.Globalization;

namespace ShowSeen
{
    public partial class GUI : Form
    {

        public static string source = ".\\Resources\\ShowSeen.accdb";


        public void PopulateListBox()
        {
            // 
            // listBox1 Initialization query
            // 
            string str = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + source;
            string query = "SELECT * FROM Shows ORDER BY Shows.Airs ASC";

            try
            {
                OleDbConnection con = new OleDbConnection(str);
                OleDbCommand cmd = new OleDbCommand(query, con);
                con.Open();
                OleDbDataReader dreader = cmd.ExecuteReader();

                while (dreader.Read())
                {
                    string[] result = { dreader["ShowName"].ToString(),
                                        dreader["Season"].ToString(),
                                        dreader["Episode"].ToString(),
                                        dreader["Airs"].ToString(),
                                        dreader["Day"].ToString(),
                                        dreader["TBAday"].ToString(),
                                        dreader["TBAmonth"].ToString(),
                                        dreader["TBA"].ToString() };
                    result[3] = result[3].Substring(0, result[3].IndexOf(" "));
                    result[5] = result[5] == "True" ? "✓" : " ";
                    result[6] = result[6] == "True" ? "✓" : " ";
                    result[7] = result[7] == "True" ? "✓" : " ";

                    int space1len;
                    if (result[0] == "Homeland" || result[0] == "Teen Wolf")
                    {
                        space1len = 2;
                    }
                    else if (result[0].Length <= 9)
                    {
                        space1len = 3;
                    }
                    else if (result[0].Length <= 19)
                    {
                        space1len = 2;
                    }
                    else
                    {
                        space1len = 1;
                    }

                    int space2len;
                    if (result[1].Length == 1)
                    {
                        space2len = 14;
                    }
                    else
                    {
                        space2len = 12;
                    }

                    int space3len = 2;

                    int space4len;
                    if (result[3].Length == 10)
                    {
                        space4len = 17;
                    }
                    else
                    {
                        space4len = 19;
                    }

                    int space5len = 1;

                    string space1 = String.Concat(Enumerable.Repeat("\t", space1len));
                    string space2 = String.Concat(Enumerable.Repeat(" ", space2len));
                    string space3 = String.Concat(Enumerable.Repeat("\t", space3len));
                    string space4 = String.Concat(Enumerable.Repeat(" ", space4len));
                    string space5 = String.Concat(Enumerable.Repeat("\t", space5len));
                    string space6 = String.Concat(Enumerable.Repeat("\t", space5len));
                    string space7 = String.Concat(Enumerable.Repeat("\t  ", space5len));

                    string listItem = result[0] + space1 +
                                        result[1] + space2 +
                                        result[2] + space3 +
                                        result[3] + space4 +
                                        result[4] + space5 +
                                        result[5] + space6 +
                                        result[6] + space7 +
                                        result[7];

                    listBox1.Items.Add(listItem);
                }

                con.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        }

        public void PopulateDataGridView(string query)
        {
            string str = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + source;
            label10.Text = query;

            try
            {
                OleDbConnection con = new OleDbConnection(str);
                OleDbCommand cmd = new OleDbCommand(query, con);
                con.Open();
                OleDbDataReader dreader = cmd.ExecuteReader();

                DataTable table = new DataTable();
                table.Columns.Add("TV Show name", typeof(string));
                table.Columns.Add("Season", typeof(int));
                table.Columns.Add("Episode", typeof(int));
                table.Columns.Add("Air date", typeof(string));
                table.Columns.Add("Day", typeof(string));
                table.Columns.Add("Day TBA", typeof(char));
                table.Columns.Add("Month TBA", typeof(char));
                table.Columns.Add("TBA", typeof(char));
                table.Columns.Add("ID", typeof(int));

                while (dreader.Read())
                {
                    string id = dreader["ID"].ToString();
                    string showname = dreader["ShowName"].ToString();
                    string season = dreader["Season"].ToString();
                    string episode = dreader["Episode"].ToString();
                    string airs = dreader["Airs"].ToString();
                    string day = dreader["Day"].ToString();
                    string tbaday = dreader["TBAday"].ToString();
                    string tbamonth = dreader["TBAmonth"].ToString();
                    string tba = dreader["TBA"].ToString();


                    airs = airs.Substring(0, airs.IndexOf(" "));
                    tbaday = tbaday == "True" ? "✓" : " ";
                    tbamonth = tbamonth == "True" ? "✓" : " ";
                    tba = tba == "True" ? "✓" : " ";

                    table.Rows.Add(showname,
                                        Convert.ToInt32(season),
                                        Convert.ToInt32(episode),
                                        airs,
                                        day,
                                        Convert.ToChar(tbaday),
                                        Convert.ToChar(tbamonth),
                                        Convert.ToChar(tba),
                                        Convert.ToInt32(id));


                }
                string count = Convert.ToString(table.Rows.Count);
                dataGridView1.DataSource = table;
                label12.Text = count;

                con.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "TV Shows", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public void ResizeColWidth()
        {
            dataGridView1.Columns[0].Width = 150;
            dataGridView1.Columns[1].Width = 50;
            dataGridView1.Columns[2].Width = 50;
            dataGridView1.Columns[3].Width = 70;
            dataGridView1.Columns[4].Width = 40;
            dataGridView1.Columns[5].Width = 75;
            dataGridView1.Columns[6].Width = 90;
            dataGridView1.Columns[7].Width = 70;
            dataGridView1.Columns[8].Visible = false;
        }

        public void executeCMDQuery(string query)
        {
            string str = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + source;

            try
            {
                OleDbConnection con = new OleDbConnection(str);
                OleDbCommand cmd = new OleDbCommand(query, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "TV Shows", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void setChanged()
        {
            //MessageBox.Show(label10.Text, "TV Shows", MessageBoxButtons.OK, MessageBoxIcon.Information);
            bool dataPresent = dataGridView1.Rows.Count != 0;

            if (dataPresent)
            {
                int selectedRow = dataGridView1.CurrentRow != null ? dataGridView1.CurrentRow.Index : 0;
                int numRows = Convert.ToInt32(label12.Text);
                PopulateDataGridView(label10.Text);
                if (Convert.ToInt32(label12.Text) == numRows)
                {
                    if (dataGridView1.CurrentRow != null)
                    {
                        dataGridView1.CurrentCell.Selected = false;
                    }
                    dataGridView1.CurrentCell = dataGridView1.Rows[selectedRow].Cells[0];
                }
            }
            else
            {
                PopulateDataGridView(label10.Text);
            }

        }

        public int getCurrentId()
        {
            int selectedRow = dataGridView1.CurrentRow != null ? dataGridView1.CurrentRow.Index : 0;
            return Convert.ToInt32(dataGridView1.Rows[selectedRow].Cells[8].Value);
        }

        public GUI()
        {
            InitializeComponent();

            //PopulateListBox();
            //PopulateDataGridViewTest();
            PopulateDataGridView("SELECT * FROM Shows ORDER BY Shows.Airs ASC");
            ResizeColWidth();
            comboBox1.SelectedIndex = 0;
        }



        private void button8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int rownr = dataGridView1.CurrentRow.Index;
            int id = Convert.ToInt32(dataGridView1.Rows[rownr].Cells[8].Value);
            int value = Convert.ToInt32(dataGridView1.Rows[rownr].Cells[2].Value) + 1;
            string query = "UPDATE Shows SET Episode = " + value + " WHERE ID = " + id;
            executeCMDQuery(query);
            setChanged();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int rownr = dataGridView1.CurrentRow.Index;
            int id = Convert.ToInt32(dataGridView1.Rows[rownr].Cells[8].Value);
            int value = Convert.ToInt32(dataGridView1.Rows[rownr].Cells[2].Value) - 1;
            string query = "UPDATE Shows SET Episode = " + value + " WHERE ID = " + id;
            executeCMDQuery(query);
            setChanged();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int rownr = dataGridView1.CurrentRow.Index;
            int id = Convert.ToInt32(dataGridView1.Rows[rownr].Cells[8].Value);
            int value = Convert.ToInt32(dataGridView1.Rows[rownr].Cells[1].Value) + 1;
            string query = "UPDATE Shows SET Season = " + value + " WHERE ID = " + id;
            executeCMDQuery(query);
            setChanged();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int rownr = dataGridView1.CurrentRow.Index;
            int id = Convert.ToInt32(dataGridView1.Rows[rownr].Cells[8].Value);
            int value = Convert.ToInt32(dataGridView1.Rows[rownr].Cells[1].Value) - 1;
            string query = "UPDATE Shows SET Season = " + value + " WHERE ID = " + id;
            executeCMDQuery(query);
            setChanged();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int rownr = dataGridView1.CurrentRow.Index;
            int id = Convert.ToInt32(dataGridView1.Rows[rownr].Cells[8].Value);
            string name = Convert.ToString(dataGridView1.Rows[rownr].Cells[0].Value);

            DialogResult result = MessageBox.Show("Do you really want to delete the data of " + name +
                "?\nThis data will be permanently lost!", "TV Shows",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            switch (result)
            {
                case DialogResult.Yes:
                    string query = "DELETE FROM Shows WHERE ID = " + id;
                    executeCMDQuery(query);
                    setChanged();
                    break;
                default:
                    break;
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataGridViewRow r = dataGridView1.CurrentRow;

            RowInfo frm = new RowInfo(this, true);
            frm.Text = "Modify TV Show..";
            frm.textBox1.Text = Convert.ToString(r.Cells[0].Value);
            frm.numericUpDown1.Value = Convert.ToInt32(r.Cells[1].Value);
            frm.numericUpDown2.Value = Convert.ToInt32(r.Cells[2].Value);
            frm.dateTimePicker1.Value = Convert.ToDateTime(r.Cells[3].Value);
            frm.comboBox1.SelectedIndex = frm.comboBox1.FindStringExact(Convert.ToString(r.Cells[4].Value));
            frm.checkedListBox1.SetItemCheckState(0, Convert.ToString(r.Cells[5].Value) == "✓" ? CheckState.Checked : CheckState.Unchecked);
            frm.checkedListBox1.SetItemCheckState(1, Convert.ToString(r.Cells[6].Value) == "✓" ? CheckState.Checked : CheckState.Unchecked);
            frm.checkedListBox1.SetItemCheckState(2, Convert.ToString(r.Cells[7].Value) == "✓" ? CheckState.Checked : CheckState.Unchecked);
            frm.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {

            RowInfo frm = new RowInfo(this, false);
            frm.Text = "Add a new TV Show..";
            frm.comboBox1.SelectedIndex = 0;
            frm.numericUpDown1.Value = 1;
            frm.numericUpDown2.Value = 1;
            frm.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query, q;
            switch (comboBox1.SelectedIndex)
            {
                case 1:
                    query = label10.Text;
                    q = query.Replace("Shows.Airs", "Shows.ShowName");

                    label10.Text = q;
                    break;
                case 0:
                    query = label10.Text;
                    q = query.Replace("Shows.ShowName", "Shows.Airs");

                    label10.Text = q;
                    break;
            }

            setChanged();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "Search...")
            {
                textBox1.ForeColor = Color.Black;
                string value = textBox1.Text;
                string query = "SELECT * FROM Shows WHERE ShowName LIKE '%" + value + "%' ORDER BY Shows.Airs ASC";
                label10.Text = query;
                setChanged();
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
            textBox1.ForeColor = Color.Black;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox1.Text == "Search...")
            {
                textBox1.Text = "Search...";
                textBox1.ForeColor = Color.LightGray;
            }
        }

        public string GetReq(string uri, string season)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri + season);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public Tuple<int,DateTime> parseHTML(string html, string episode)
        {
            CQ dom = html;

            int currEpi = Int32.Parse(episode) + 1;
            int episodeNR = 0;
            string airdate = "";

            var eps = dom[".info"];
            foreach (var e in eps)
            {
                episodeNR = Int32.Parse(e.ChildNodes[1].GetAttribute("content"));
                if (episodeNR == currEpi)
                {
                    airdate = e.ChildNodes[3].InnerHTML;
                    break;
                }
                
            }
            CultureInfo provider = CultureInfo.InvariantCulture;
            string date = airdate.ToString().Trim();
            if (date.Length > 10)
            {
                var result = DateTime.ParseExact(airdate.ToString().Trim(), "d MMM. yyyy", provider);
                return new Tuple<int, DateTime>(episodeNR, result);
            }
            else if (date.Length > 5)
            {
                var result = DateTime.ParseExact(airdate.ToString().Trim(), "MMM. yyyy", provider).AddDays(25);
                return new Tuple<int, DateTime>(episodeNR, result);
            }
            else if (date.Length > 3)
            {
                var result = DateTime.ParseExact(airdate.ToString().Trim(), "yyyy", provider).AddMonths(11).AddDays(25);
                return new Tuple<int, DateTime>(episodeNR, result);
            }
            else
            {
                return new Tuple<int, DateTime>(0, DateTime.MinValue);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string str = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + source;
            string q = "SELECT * FROM Shows ORDER BY Shows.Airs ASC";
            try
            {
                OleDbConnection con = new OleDbConnection(str);
                OleDbCommand cmd = new OleDbCommand(q, con);
                con.Open();
                OleDbDataReader dreader = cmd.ExecuteReader();

                while (dreader.Read())
                {
                    Application.DoEvents();
                    string id = dreader["ID"].ToString();
                    string season = dreader["Season"].ToString();
                    string episode = dreader["Episode"].ToString();
                    string link = dreader["Link"].ToString();

                    if (link.Equals("")) { continue; }

                    // Process one entry in the DB (one tv show)
                    try
                    {
                        var html = GetReq(link, season);
                        var res = parseHTML(html, episode);

                        if (res.Item2.Equals(DateTime.MinValue))
                        {
                            CQ dom = html;
                            var seasons = dom["#bySeason"];
                            if (seasons.Children().Length > Int32.Parse(season))
                            {
                                html = GetReq(link, season + 1);
                                res = parseHTML(html, "0");
                            }
                            if (res.Item2.Equals(DateTime.MinValue))
                            {
                                html = GetReq(link, season + 1);
                                res = parseHTML(html, "-1");
                            }
                        }

                        if (!res.Item2.Equals(DateTime.MinValue))
                        {
                            string query = "UPDATE Shows SET Airs='" + res.Item2.AddDays(1).ToString() + "' WHERE (ID=" + id + ")";
                            executeCMDQuery(query);
                            setChanged();
                        }
                        else
                        {
                            string query = "UPDATE Shows SET Airs='" + DateTime.MaxValue.ToString() + "' WHERE (ID=" + id + ")";
                            executeCMDQuery(query);
                            setChanged();
                        }
                    }
                    catch { continue; }
                    //break;
                }
                con.Close();
                MessageBox.Show("Synchronized with IMDb.", "TV Shows", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "TV Shows", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

    }
}


//MessageBox.Show(label10.Text, "TV Shows", MessageBoxButtons.OK, MessageBoxIcon.Information);