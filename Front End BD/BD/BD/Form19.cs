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
    public partial class Form19 : Form
    {
        public Form19()
        {
            InitializeComponent();
        }


        public class Sumar_Comanda
        {
            public int id_comanda { get; set; }
            public string persoana_contact { get; set; }
            public int nr_produse_comandate { get; set; }
            public double valoare_totala { get; set; }
            public string nume_judet { get; set; }
            public string oras{ get; set; }
            public string strada { get; set; }
            public string bloc { get; set; }
            public int cod_postal { get; set; }
            public string numar_telefon { get; set; }
            public string data_comanda { get; set; }

        }

        static readonly HttpClient client = new HttpClient();


        public async Task Populate_DGV()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                string url = "http://localhost:3000/api/sumar_comanda";

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    var result = JsonConvert.DeserializeObject<List<Sumar_Comanda>>(responseBody, settings);
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


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void Form19_Load(object sender, EventArgs e)
        {
            await Populate_DGV();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await Populate_DGV();
        }
    }
}
