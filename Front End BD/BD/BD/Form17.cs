using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BD
{
    public partial class Form17 : Form
    {
        public Form17()
        {
            InitializeComponent();
        }

        public class Produs_Complet
        {
            public int id_produs { get; set; }
            public string nume { get; set; }
            public string nume_producator { get; set; }
            public int id_producator { get; set; }
            public double pret { get; set; }
            public int cantitate { get; set; }
            public string categorie { get; set; }
            
        }

        static readonly HttpClient client = new HttpClient();


        public async Task Populate_DGV()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                string url = "http://localhost:3000/api/produse_complete";

                
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    var result = JsonConvert.DeserializeObject<List<Produs_Complet>>(responseBody, settings);
                    DataTable dt = new DataTable();
                    dt = ToDataTable(result);
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(responseBody);
                }
            }
            catch (HttpRequestException e)
            {

                string exce = "\nException Caught!";
                exce += ("Message:", e.Message);
                MessageBox.Show(exce);

            }
        }
        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        private async void Form17_Load(object sender, EventArgs e)
        {
            await Populate_DGV();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            await Populate_DGV();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "http://localhost:3000/api/produse_complete";

                string json = "{\"nume_produs\":\"" + textBox2.Text.ToString() + "\"," +
                                  "\"id_producator\":\"" + textBox4.Text.ToString() + "\"," +
                                  "\"pret\":\"" + textBox5.Text.ToString() + "\"," +
                                  "\"cantitate\":\"" + textBox6.Text.ToString() + "\"," +
                                  "\"categorie\":\"" + textBox7.Text.ToString() + "\"}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    button6.PerformClick();
                    button5.PerformClick();
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(responseBody);
                }
            }
            catch (HttpRequestException es)
            {

                string exce = "\nException Caught!";
                exce += ("Message:", es.Message);
                MessageBox.Show(exce);

            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                textBox1.Text = row.Cells[0].Value.ToString();
                textBox2.Text = row.Cells[1].Value.ToString();
                textBox3.Text = row.Cells[2].Value.ToString();
                textBox4.Text = row.Cells[3].Value.ToString();
                textBox5.Text = row.Cells[4].Value.ToString();
                textBox6.Text = row.Cells[5].Value.ToString();
                textBox7.Text = row.Cells[6].Value.ToString();
                
                //string value2 = row.Cells[1].Value.ToString();

            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "http://localhost:3000/api/produse_complete/"+textBox1.Text.ToString();
                string json = "{\"nume_produs\":\"" + textBox2.Text.ToString() + "\"," +
                                  "\"id_producator\":\"" + textBox4.Text.ToString() + "\"," +
                                  "\"pret\":\"" + textBox5.Text.ToString() + "\"," +
                                  "\"cantitate\":\"" + textBox6.Text.ToString() + "\"," +
                                  "\"categorie\":\"" + textBox7.Text.ToString() + "\"}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    button6.PerformClick();
                    button5.PerformClick();
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(responseBody);
                }
            }
            catch (HttpRequestException es)
            {

                string exce = "\nException Caught!";
                exce += ("Message:", es.Message);
                MessageBox.Show(exce);

            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "http://localhost:3000/api/produse_complete/" + textBox1.Text.ToString();
                HttpResponseMessage response = await client.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    button6.PerformClick();
                    button5.PerformClick();
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(responseBody);
                }
            }
            catch (HttpRequestException es)
            {

                string exce = "\nException Caught!";
                exce += ("Message:", es.Message);
                MessageBox.Show(exce);

            }
        }
    }
}
