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
    public partial class Form15 : Form
    {
        public Form15()
        {
            InitializeComponent();
        }

        private async void Form15_Load(object sender, EventArgs e)
        {
            await Populate_DGV();
            await Populate_DGV2();
        }

        public class Group2
        {
            public string nume_platforma { get; set; }
            public int numar_produse { get; set; }
            public double pret { get; set; }
        }

        public class Group1
        {
            public string id_judet { get; set; }
            public int numar { get; set; }
        }


        static readonly HttpClient client = new HttpClient();


        public async Task Populate_DGV()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                string url = "http://localhost:3000/api/group";

                if (textBox1.Text.Length > 0)
                {
                    url += ("/" + textBox1.Text.ToString());
                }

                Console.WriteLine(url);
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    var result = JsonConvert.DeserializeObject<List<Group1>>(responseBody, settings);
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



        public async Task Populate_DGV2()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                string url = "http://localhost:3000/api/group2";

                if (textBox2.Text.Length > 0)
                {
                    url += ("/" + textBox2.Text.ToString());
                }
                url += "?";
                if (textBox3.Text.Length > 0)
                {
                    url += ("avg=" + textBox3.Text.ToString());
                }
                Console.WriteLine(url);
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    var result = JsonConvert.DeserializeObject<List<Group2>>(responseBody, settings);
                    DataTable dt = new DataTable();
                    dt = ToDataTable(result);
                    dataGridView2.DataSource = dt;
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

        private async void button1_Click(object sender, EventArgs e)
        {
            await Populate_DGV();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await Populate_DGV2();
        }
    }
}
