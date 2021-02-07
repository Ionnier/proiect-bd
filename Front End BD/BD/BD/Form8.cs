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
    public partial class Form8 : Form
    {

        public class MyListBoxItem
        {
            public string id { get; set; }
            public string nume { get; set; }
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

        List<MyListBoxItem> result;

        public Form8()
        {
            InitializeComponent();
            populateDGV();
            string html = string.Empty;
            string url = @"http://localhost:3000/api/platforme";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 2000;
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
            result = JsonConvert.DeserializeObject<List<MyListBoxItem>>(html,settings);
            foreach (MyListBoxItem user in result)
            {
                checkedListBox1.Items.Add(user.nume);
            }
            Thread.Sleep(2000);
        }

        public void populateDGV()
        {
            string html = string.Empty;
            string url = @"http://localhost:3000/api/produse";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 2000;
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
                var result = JsonConvert.DeserializeObject<List<Produs>>(html,settings);
                DataTable dt = new DataTable();
                dt = ToDataTable(result);
                dataGridView1.DataSource = dt;
                Thread.Sleep(200);
            }
            catch { }

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
   
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form8_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = "[";
            foreach (object itemChecked in checkedListBox1.CheckedItems)
            {
                foreach (MyListBoxItem user in result)
                {
                    if (user.nume == (string)itemChecked)
                    {
                        str += user.id.ToString();
                        str += ", ";
                    }
                }
            }
            str=str.TrimEnd(' ');
            str=str.TrimEnd(',');
            str += ']';
           
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                clearcheckbox();
                textBox1.Text = row.Cells[0].Value.ToString();
                textBox2.Text = row.Cells[1].Value.ToString();
                textBox3.Text = row.Cells[2].Value.ToString();
                textBox4.Text = row.Cells[3].Value.ToString();
                textBox5.Text = row.Cells[4].Value.ToString();
                textBox6.Text = row.Cells[5].Value.ToString();
                textBox7.Text = row.Cells[6].Value.ToString();
                //string value2 = row.Cells[1].Value.ToString();

                string html = string.Empty;
                string url = @"http://localhost:3000/api/compatibilitateproduse/"+textBox1.Text.ToString();

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 2000;
                request.AutomaticDecompression = DecompressionMethods.GZip;
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        html = reader.ReadToEnd();
                        reader.Close();
                    }
                    
                    html = html.ToString();
                    html = html.Remove(0, 1);
                    html = html.TrimEnd(']');
                    try
                    {
                        foreach (var elem in html.Split(','))
                        {

                            int x = Int32.Parse(elem);
                            for (int i = 0; i <= (checkedListBox1.Items.Count - 1); i++)
                            {
                                if (result[x - 1].nume == (string)checkedListBox1.Items[i])
                                {
                                    checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                                }

                            }


                        }

                    }
                    catch { }
                }
                catch { }
               

            }
            Thread.Sleep(200);
        }

        private void clearcheckbox()
        {

            for (int i = 0; i <= (checkedListBox1.Items.Count - 1); i++)
            {

                checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
                

            }

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
            clearcheckbox();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                string str = "[";
                foreach (object itemChecked in checkedListBox1.CheckedItems)
                {
                    foreach (MyListBoxItem user in result)
                    {
                        if (user.nume == (string)itemChecked)
                        {
                            str += user.id.ToString();
                            str += ", ";
                        }
                    }
                }
                str = str.TrimEnd(' ');
                str = str.TrimEnd(',');
                str += ']';

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3000/api/produse");
                httpWebRequest.Timeout = 2000;
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"id_producator\":\"" + textBox2.Text.ToString() + "\"," +
                                  "\"nume_produs\":\"" + textBox3.Text.ToString() + "\"," +
                                  "\"pret\":\"" + textBox4.Text.ToString() + "\"," +
                                  "\"cantitate\":\"" + textBox5.Text.ToString() + "\"," +
                                  "\"descriere\":\"" + textBox6.Text.ToString() + "\"," +
                                  "\"categorie\":\"" + textBox7.Text.ToString() + "\"," +
                                  "\"platforme\":" + str + "}";
                    streamWriter.Write(json);
                    streamWriter.Close();
                }
               
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                httpResponse.Close();
                Thread.Sleep(100);

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
                string str = "[";
                foreach (object itemChecked in checkedListBox1.CheckedItems)
                {
                    foreach (MyListBoxItem user in result)
                    {
                        if (user.nume == (string)itemChecked)
                        {
                            str += user.id.ToString();
                            str += ", ";
                        }
                    }
                }
                str = str.TrimEnd(' ');
                str = str.TrimEnd(',');
                str += ']';
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3000/api/produse/" + textBox1.Text.ToString());
                httpWebRequest.Timeout = 2000;
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PUT";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"id_producator\":\"" + textBox2.Text.ToString() + "\"," +
                                  "\"nume_produs\":\"" + textBox3.Text.ToString() + "\"," +
                                  "\"pret\":\"" + textBox4.Text.ToString() + "\"," +
                                  "\"cantitate\":\"" + textBox5.Text.ToString() + "\"," +
                                  "\"descriere\":\"" + textBox6.Text.ToString() + "\"," +
                                  "\"categorie\":\"" + textBox7.Text.ToString() + "\"," +
                                  "\"platforme\":" + str + "}";
                    streamWriter.Write(json);

                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Thread.Sleep(200);

                button6.PerformClick();
                populateDGV();
            }
            catch (WebException ex)
            {
                try
                {
                    string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    MessageBox.Show(message);
                }
                catch { }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3000/api/produse/" + textBox1.Text.ToString());
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "DELETE";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                }
                Thread.Sleep(200);
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

        private void button5_Click(object sender, EventArgs e)
        {
            populateDGV();
        }
    }
}
