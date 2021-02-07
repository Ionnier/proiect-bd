using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BD
{
    public partial class Form10 : Form
    {
        public Form10()
        {
            InitializeComponent();
            Thread.Sleep(250);
            populateDGV();
            Thread.Sleep(250);
        }

        public class Comentariu
        {
            public int id { get; set; }
            public int id_produs { get; set; }
            public int id_utilizator { get; set; }
            public string continut { get; set; }
            public string data_publicare { get; set; }
        }

        public void populateDGV()
        {
            Thread.Sleep(250);
            string html = string.Empty;
            string url = @"http://localhost:3000/api/comentarii";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var result = JsonConvert.DeserializeObject<List<Comentariu>>(html,settings);
            DataTable dt = new DataTable();
            dt = ToDataTable(result);
            Thread.Sleep(250);
            dataGridView1.DataSource = dt;

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

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

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
              
                //string value2 = row.Cells[1].Value.ToString();

                

                


            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Thread.Sleep(250);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3000/api/comentarii");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"id_utilizator\":\"" + textBox3.Text.ToString() + "\"," +
                                  "\"id_produs\":\"" + textBox2.Text.ToString() + "\"," +
                                  "\"continut_comentariu\":\"" + textBox4.Text.ToString() + "\"}";
                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Thread.Sleep(250);
                button6.PerformClick();
                populateDGV();
            }
            catch (WebException ex)
            {
                string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                MessageBox.Show(message);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Thread.Sleep(250);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3000/api/comentarii/"+textBox1.Text.ToString());
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PUT";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"id_utilizator\":\"" + textBox3.Text.ToString() + "\"," +
                                  "\"id_produs\":\"" + textBox2.Text.ToString() + "\"," +
                                  "\"continut_comentariu\":\"" + textBox4.Text.ToString() + "\"}";
                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Thread.Sleep(500);
                button6.PerformClick();
                populateDGV();
            }
            catch (WebException ex)
            {
                string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                MessageBox.Show(message);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Thread.Sleep(250);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3000/api/comentarii/" + textBox1.Text.ToString());
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "DELETE";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                }
                Thread.Sleep(250);
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                button6.PerformClick();
                populateDGV();
            }
            catch (WebException ex)
            {
                string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                MessageBox.Show(message);
            }
        }

        private void Form10_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
