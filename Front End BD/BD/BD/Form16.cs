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
    public partial class Form16 : Form
    {
        public Form16()
        {
            InitializeComponent();
        }

        public class Producator
        {
            public int id { get; set; }
            public string nume { get; set; }
            public string website { get; set; }
        }


        static readonly HttpClient client = new HttpClient();


        public async Task Populate_DGV()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                string url = "http://localhost:3000/api/producatori";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    var result = JsonConvert.DeserializeObject<List<Producator>>(responseBody, settings);
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

        public class Produs
        {
            public int id { get; set; }
            public int id_producator { get; set; }
            public string nume { get; set; }
            public double pret { get; set; }
            public int cantitate { get; set; }
            public string descriere { get; set; }
            public string categorie { get; set; }
        }

        public class CompProd
        {
            public string nume_produs { get; set; }
            public int id_platforma { get; set; }
            public int id_producator { get; set; }
        }

        public class Comentarii
        {
            
            public int id_comentariu { get; set; }
            public string nume_produs { get; set; }
            public int id_producator { get; set; }
        }

        public class Produse_Comandate
        {

            public int id_comanda { get; set; }
            public int id_produs { get; set; }
            
            public int cantitate { get; set; }
            public int id_producator { get; set; }
        }

        public class Istoric_Preturi
        {

            
            public int id_produs { get; set; }
            public double pret { get; set; }
            public int id_producator { get; set; }
            public string data_inceput { get; set; }
            
        }


        public async Task Populate_DGV2()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                string url = "http://localhost:3000/api/produse?id_producator=" + textBox1.Text.ToString();
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    var result = JsonConvert.DeserializeObject<List<Produs>>(responseBody, settings);
                    DataTable dt = new DataTable();
                    dt = ToDataTable(result);
                    dataGridView2.DataSource = dt;
                }
                else
                {
                    DataTable dt = new DataTable();
                    List<Produs> lcp = new List<Produs>();
                    dt = ToDataTable(lcp);
                    dataGridView2.DataSource = dt;
                }
            }
            catch (HttpRequestException e)
            {

                string exce = "\nException Caught!";
                exce += ("Message:", e.Message);
                MessageBox.Show(exce);

            }
        }

        public async Task Populate_DGV4()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                string url = "http://localhost:3000/api/ondeletecascadecompprod/" + textBox1.Text.ToString();
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    var result = JsonConvert.DeserializeObject<List<CompProd>>(responseBody, settings);
                    DataTable dt = new DataTable();
                    dt = ToDataTable(result);
                    dataGridView4.DataSource = dt;
                }
                else
                {
                    DataTable dt = new DataTable();
                    List<CompProd> lcp = new List<CompProd>();
                    dt = ToDataTable(lcp);
                    dataGridView4.DataSource = dt;
                }
            }
            catch (HttpRequestException e)
            {

                string exce = "\nException Caught!";
                exce += ("Message:", e.Message);
                MessageBox.Show(exce);

            }
        }

        public async Task Populate_DGV5()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                string url = "http://localhost:3000/api/ondeletecascadecomenzi/" + textBox1.Text.ToString();
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    var result = JsonConvert.DeserializeObject<List<Produse_Comandate>>(responseBody, settings);
                    DataTable dt = new DataTable();
                    dt = ToDataTable(result);
                    dataGridView5.DataSource = dt;
                }
                else
                {
                    DataTable dt = new DataTable();
                    List<Produse_Comandate> lcp = new List<Produse_Comandate>();
                    dt = ToDataTable(lcp);
                    dataGridView5.DataSource = dt;
                }
            }
            catch (HttpRequestException e)
            {

                string exce = "\nException Caught!";
                exce += ("Message:", e.Message);
                MessageBox.Show(exce);

            }
        }

        public async Task Populate_DGV6()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                string url = "http://localhost:3000/api/ondeletecascadeip/" + textBox1.Text.ToString();
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    var result = JsonConvert.DeserializeObject<List<Istoric_Preturi>>(responseBody, settings);
                    DataTable dt = new DataTable();
                    dt = ToDataTable(result);
                    dataGridView6.DataSource = dt;
                }
                else
                {
                    DataTable dt = new DataTable();
                    List<Istoric_Preturi> lcp = new List<Istoric_Preturi>();
                    dt = ToDataTable(lcp);
                    dataGridView6.DataSource = dt;
                }
            }
            catch (HttpRequestException e)
            {

                string exce = "\nException Caught!";
                exce += ("Message:", e.Message);
                MessageBox.Show(exce);

            }
        }

        public async Task Populate_DGV3()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                string url = "http://localhost:3000/api/ondeletecascadecomentarii/" + textBox1.Text.ToString();
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    var result = JsonConvert.DeserializeObject<List<Comentarii>>(responseBody, settings);
                    DataTable dt = new DataTable();
                    dt = ToDataTable(result);
                    dataGridView3.DataSource = dt;
                }
                else
                {
                    DataTable dt = new DataTable();
                    List<Comentarii> lcp = new List<Comentarii>();
                    dt = ToDataTable(lcp);
                    dataGridView3.DataSource = dt;
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

        private async void button1_Click(object sender, EventArgs e)
        {
            await Delete();
        }


        public async Task Delete()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                string url = "http://localhost:3000/api/producatori/" + textBox1.Text.ToString();
                HttpResponseMessage response = await client.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    await Populate_DGV();
                }
                else
                {
                    DataTable dt = new DataTable();
                    List<Comentarii> lcp = new List<Comentarii>();
                    dt = ToDataTable(lcp);
                    dataGridView3.DataSource = dt;
                }
            }
            catch (HttpRequestException e)
            {

                string exce = "\nException Caught!";
                exce += ("Message:", e.Message);
                MessageBox.Show(exce);

            }
        }


        private async void Form16_Load(object sender, EventArgs e)
        {
            await Populate_DGV();
        }

        
            private void dataGridView1_SelectionChanged(object sender, EventArgs e)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    textBox1.Text = row.Cells[0].Value.ToString();
                button2.PerformClick();
                    //string value2 = row.Cells[1].Value.ToString();

                }
            }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.ToString() != "") {
                await Populate_DGV2();
                await Populate_DGV3();
                await Populate_DGV4();
                await Populate_DGV5();
                await Populate_DGV6();
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
