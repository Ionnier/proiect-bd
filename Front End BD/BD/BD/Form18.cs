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
    public partial class Form18 : Form
    {
        public Form18()
        {
            InitializeComponent();
        }

        public class Comanda_Complet
        {
            public int id_comanda { get; set; }
            public int id_utilizator { get; set; }
            public string persoana_contact { get; set; }
            public string id_judet { get; set; }
            public string nume_judet { get; set; }
            public string oras { get; set; }
            public string strada { get; set; }
            public string numar { get; set; }
            public int cod_postal { get; set; }
            public string bloc { get; set; }
            public string scara { get; set; }
            public int apartament { get; set; }
            public string numar_telefon { get; set; }
            public string data_comanda { get; set; }

        }

        static readonly HttpClient client = new HttpClient();


        public async Task Populate_DGV()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                string url = "http://localhost:3000/api/comenzi_complete";


                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    var result = JsonConvert.DeserializeObject<List<Comanda_Complet>>(responseBody, settings);
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

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            textBox13.Text = "";
            textBox14.Text = "";

        }

        private async void Form18_Load(object sender, EventArgs e)
        {
            await Populate_DGV();
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            await Populate_DGV();
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
                textBox8.Text = row.Cells[7].Value.ToString();
                textBox9.Text = row.Cells[8].Value.ToString();
                textBox10.Text = row.Cells[9].Value.ToString();
                textBox11.Text = row.Cells[10].Value.ToString();
                textBox12.Text = row.Cells[11].Value.ToString();
                textBox13.Text = row.Cells[12].Value.ToString();
                textBox14.Text = row.Cells[13].Value.ToString();

                //string value2 = row.Cells[1].Value.ToString();

            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "http://localhost:3000/api/comenzi_complete";

                string json = "{\"id_utilizator\":\"" + textBox2.Text.ToString() + "\"," +
                                  "\"id_judet\":\"" + textBox4.Text.ToString() + "\"," +
                                  "\"oras\":\"" + textBox6.Text.ToString() + "\"," +
                                  "\"strada\":\"" + textBox7.Text.ToString() + "\"," +
                                  "\"numar\":\"" + textBox8.Text.ToString() + "\"," +
                                  "\"cod_postal\":\"" + textBox9.Text.ToString() + "\"," +
                                  "\"bloc\":\"" + textBox10.Text.ToString() + "\"," +
                                  "\"scara\":\"" + textBox11.Text.ToString() + "\"," +
                                  "\"apartament\":\"" + textBox12.Text.ToString() + "\"," +
                                  "\"numar_telefon\":\"" + textBox13.Text.ToString() + "\"}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {    
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

        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "http://localhost:3000/api/comenzi_complete/"+textBox1.Text.ToString();

                string json = "{\"id_utilizator\":\"" + textBox2.Text.ToString() + "\"," +
                                  "\"id_judet\":\"" + textBox4.Text.ToString() + "\"," +
                                  "\"oras\":\"" + textBox6.Text.ToString() + "\"," +
                                  "\"strada\":\"" + textBox7.Text.ToString() + "\"," +
                                  "\"numar\":\"" + textBox8.Text.ToString() + "\"," +
                                  "\"cod_postal\":\"" + textBox9.Text.ToString() + "\"," +
                                  "\"bloc\":\"" + textBox10.Text.ToString() + "\"," +
                                  "\"scara\":\"" + textBox11.Text.ToString() + "\"," +
                                  "\"apartament\":\"" + textBox12.Text.ToString() + "\"," +
                                  "\"numar_telefon\":\"" + textBox13.Text.ToString() + "\"}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
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
                string url = "http://localhost:3000/api/comenzi_complete/" + textBox1.Text.ToString();
                HttpResponseMessage response = await client.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
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
