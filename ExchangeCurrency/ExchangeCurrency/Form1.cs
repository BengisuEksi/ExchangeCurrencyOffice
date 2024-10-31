using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Data.SqlClient;
namespace ExchangeCurrency
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-HPJO022;Initial Catalog=DbExchangeCurrency;Integrated Security=True");
        private void Form1_Load(object sender, EventArgs e)
        {
            string today = "https://www.tcmb.gov.tr/kurlar/today.xml";
            var xmlfile = new XmlDocument();
            xmlfile.Load(today);

            string dollarbuy = xmlfile.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteBuying").InnerXml;
            dollar_buying_lbl.Text = dollarbuy;

            string dollarsell = xmlfile.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml;
            dollar_selling_lbl.Text = dollarsell;

            string eurobuy = xmlfile.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteBuying").InnerXml;
            euro_buying_lbl.Text = eurobuy;

            string eurosell = xmlfile.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml;
            euro_selling_lbl.Text = eurosell;

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * from Tbl_SafeAmount", conn);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void dollar_buying_bttn_Click(object sender, EventArgs e)
        {
            exchangerate_txtbox.Text = dollar_buying_lbl.Text;
            exchange_box.Text = "Dollar Buying/TRY";
        }

        private void dollar_selling_bttn_Click(object sender, EventArgs e)
        {
            exchangerate_txtbox.Text = dollar_selling_lbl.Text;
            exchange_box.Text = "Dollar Selling/TRY";
        }

        private void euro_buying_bttn_Click(object sender, EventArgs e)
        {
            exchangerate_txtbox.Text = euro_buying_lbl.Text;
            exchange_box.Text = "Euro Buying/TRY";
        }

        private void euro_selling_bttn_Click(object sender, EventArgs e)
        {
            exchangerate_txtbox.Text = euro_selling_lbl.Text;
            exchange_box.Text = "Euro Selling/TRY";
        }

        private void exchangerate_txtbox_TextChanged(object sender, EventArgs e)
        {
            exchangerate_txtbox.Text = exchangerate_txtbox.Text.Replace(".", ",");
        }

        private void exit_bttn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void purchaseExRate_bttn_Click(object sender, EventArgs e)
        {
            double total_purchase;
            double amount = Convert.ToDouble(amount_txtbox.Text);
            double rate = Convert.ToDouble(exchangerate_txtbox.Text);

            total_purchase = amount * rate;
            total_txtbox.Text = total_purchase.ToString();
        }

        private void clear_bttn_Click(object sender, EventArgs e)
        {
            exchange_box.Text = "";
            exchangerate_txtbox.Text = "";
            amount_txtbox.Text = "";
            total_txtbox.Text = "";
            remainingbalance_txtbox.Text = "";
        }

        private void purchaseAmount_bttn_Click(object sender, EventArgs e)
        {
            double rate = Convert.ToDouble(exchangerate_txtbox.Text);
            double amount = Convert.ToDouble(amount_txtbox.Text);
            int total = Convert.ToInt16(amount / rate);
            int remain = Convert.ToInt16(amount % rate);
            if (exchange_box.Text == "Dollar Buying/TRY")
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Insert into Tbl_SafeAmount (DollarBuying,TL) values (@p1,@p2)", conn);
                cmd.Parameters.AddWithValue("@p1", total);
                cmd.Parameters.AddWithValue("@p2", remain);
                total_txtbox.Text = total.ToString();
                remainingbalance_txtbox.Text = remain.ToString();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            if (exchange_box.Text == "Dollar Selling/TRY")
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Insert into Tbl_SafeAmount (DollarSelling,TL) values (@p1,@p2)", conn);
                cmd.Parameters.AddWithValue("@p1", total);
                cmd.Parameters.AddWithValue("@p2", remain);
                total_txtbox.Text = total.ToString();
                remainingbalance_txtbox.Text = remain.ToString();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            if (exchange_box.Text == "Euro Buying/TRY")
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Insert into Tbl_SafeAmount (EuroBuying,TL) values (@p1,@p2)", conn);
                cmd.Parameters.AddWithValue("@p1", total);
                cmd.Parameters.AddWithValue("@p2", remain);
                total_txtbox.Text = total.ToString();
                remainingbalance_txtbox.Text = remain.ToString();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            if (exchange_box.Text == "Euro Selling/TRY")
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Insert into Tbl_SafeAmount (EuroSelling,TL) values (@p1,@p2)", conn);
                cmd.Parameters.AddWithValue("@p1", total);
                cmd.Parameters.AddWithValue("@p2", remain);
                total_txtbox.Text = total.ToString();
                remainingbalance_txtbox.Text = remain.ToString();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

        }

        private void add_safe_bttn_Click(object sender, EventArgs e)
        {
            double tl = Convert.ToDouble(total_txtbox.Text);
            double dollar_buy, dollar_sell, euro_buy, euro_sell;
            dollar_buy = Convert.ToDouble(amount_txtbox.Text);
            dollar_sell = Convert.ToDouble(amount_txtbox.Text);
            euro_buy = Convert.ToDouble(amount_txtbox.Text);
            euro_sell = Convert.ToDouble(amount_txtbox.Text);
            if (exchange_box.Text == "Dollar Buying/TRY")
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Insert into Tbl_SafeAmount (DollarBuying,TL) values (@p1,@p2)",conn);
                cmd.Parameters.AddWithValue("@p1", dollar_buy);
                cmd.Parameters.AddWithValue("@p2", tl);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            if(exchange_box.Text == "Dollar Selling/TRY")
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Insert into Tbl_SafeAmount (DollarSelling,TL) values (@p1,@p2)", conn);
                cmd.Parameters.AddWithValue("@p1", dollar_sell);
                cmd.Parameters.AddWithValue("@p2", tl);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            if( exchange_box.Text == "Euro Buying/TRY")
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Insert into Tbl_SafeAmount (EuroBuying,TL) values (@p1,@p2)", conn);
                cmd.Parameters.AddWithValue("@p1", euro_buy);
                cmd.Parameters.AddWithValue("@p2", tl);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            if(exchange_box.Text == "Euro Selling/TRY")
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Insert into Tbl_SafeAmount (EuroSelling,TL) values (@p1,@p2)", conn);
                cmd.Parameters.AddWithValue("@p1", euro_sell);
                cmd.Parameters.AddWithValue("@p2", tl);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void view_bttn_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * from Tbl_SafeAmount", conn);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void bank_total_Click(object sender, EventArgs e)
        {
            double dollar;
            double euro;
            double tl_pos = 0.0;
            double tl_neg = 0.0;
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select Sum(DollarBuying),Sum(DollarSelling),Sum(EuroBuying),Sum(EuroSelling),Sum(TL) from Tbl_SafeAmount", conn);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    var cellValue = dataGridView1.Rows[i].Cells["DollarBuying"].Value;
                    if (cellValue != null && !string.IsNullOrEmpty(cellValue.ToString()))
                    {
                        tl_neg += Convert.ToDouble(dataGridView1.Rows[i].Cells["TL"].Value);
                    }
                    var cell = dataGridView1.Rows[i].Cells["EuroBuying"].Value;
                    if (cell != null && !string.IsNullOrEmpty(cell.ToString()))
                    {
                        tl_neg += Convert.ToDouble(dataGridView1.Rows[i].Cells["TL"].Value);
                    }
                    var cellpos = dataGridView1.Rows[i].Cells["DollarSelling"].Value;
                    if (cellpos != null && !string.IsNullOrEmpty(cellpos.ToString()))
                    {
                        tl_pos += Convert.ToDouble(dataGridView1.Rows[i].Cells["TL"].Value);
                    }
                    var cell_pos = dataGridView1.Rows[i].Cells["EuroSelling"].Value;
                    if (cell_pos != null && !string.IsNullOrEmpty(cell_pos.ToString()))
                    {
                        tl_pos += Convert.ToDouble(dataGridView1.Rows[i].Cells["TL"].Value);
                    }
                }
                try_amount.Text = (tl_pos - tl_neg).ToString("F3");
                dollar = Convert.ToDouble(dr[0].ToString()) - Convert.ToDouble(dr[1].ToString());
                usd_amount.Text = dollar.ToString("F3");
                euro = Convert.ToDouble(dr[2].ToString()) - Convert.ToDouble(dr[3].ToString());
                eur_amount.Text = euro.ToString("F3");
            }
            conn.Close();
        }
    }
}
