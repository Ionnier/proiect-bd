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
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
            populateDGV();

            List<Produse> new_list = new List<Produse>();
            DataTable dt = new DataTable();
            dt = ToDataTable(new_list);
            dataGridView2.DataSource = dt;
            Thread.Sleep(500);

        }

        public class Produse
        {
            public int id_produs { get; set; }
            public int cantitate { get; set; }
        }

        public class Comanda
        {
            public int id_comanda { get; set; }
            public int id_utilizator { get; set; }
            public string id_judet { get; set; }

           
            public string oras { get; set; }
            public string strada { get; set; }

            public string numar { get; set; }
            public string cod_postal { get; set; }
            public string bloc { get; set; }
            public string scara { get; set; }
            public int apartament { get; set; }
            public string numar_telefon { get; set; }
            public string data_comanda { get; set; }
        }

        public void populateDGV()
        {
            string html = string.Empty;
            string url = @"http://localhost:3000/api/comenzi";

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
            var result = JsonConvert.DeserializeObject<List<Comanda>>(html, settings);

            DataTable dt = new DataTable();
            dt = ToDataTable(result);

            dataGridView1.DataSource = dt;
            Thread.Sleep(500);
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
      
                //string value2 = row.Cells[1].Value.ToString();

                
                string html = string.Empty;
                string url = @"http://localhost:3000/api/produsecomenzi/" + textBox1.Text.ToString();

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }
                html = html.ToString();
                html = html.Remove(0, 1);
                html = html.TrimEnd(']');
                

                List<Produse> new_list = new List<Produse>();

                var array = html.Split(',');
                if (array.Length > 1)
                {
                    for(int i = 0; i < array.Length; i+=2)
                    {
                        Produse aux = new Produse();
                        aux.id_produs = Int32.Parse(array[i]);
                        aux.cantitate = Int32.Parse(array[i+1]);
                        new_list.Add(aux);
                    }
                }

                DataTable dt = new DataTable();
                dt = ToDataTable(new_list);
                dataGridView2.DataSource = dt;
               

            }
            Thread.Sleep(500);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            populateDGV();
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
            textBox11.Text = "";
            textBox12.Text = "";
            textBox10.Text = "";
            List<Produse> new_list = new List<Produse>();
            DataTable dt = new DataTable();
            dt = ToDataTable(new_list);
            dataGridView2.DataSource = dt;


        }

        private void button1_Click(object sender, EventArgs e)
        { 
            int counter = 1;
            for(int i = 0; i < dataGridView2.Rows.Count-1; i++)
            {
                if (dataGridView2.Rows[i].Cells[0].Value.ToString() == "")
                {
                    MessageBox.Show("Unul sau mai multe cell-uri nu este complet");
                    counter = 0;
                    break;
                }
                if (dataGridView2.Rows[i].Cells[1].Value.ToString() == "")
                {
                    MessageBox.Show("Unul sau mai multe cell-uri nu este complet");
                    counter = 0;
                    break;
                }

            }
            if (counter == 1 && dataGridView2.Rows.Count>1)
            {
                string produse = "[";
                for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
                {
                    string aux = "[";
                    aux += dataGridView2.Rows[i].Cells[0].Value.ToString();
                    aux += ",";
                    aux += dataGridView2.Rows[i].Cells[1].Value.ToString();
                    aux += "]";
                    produse += aux;
                }
                produse += "]";
                Console.WriteLine(produse);
            }



        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {

                int counter = 1;
                for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
                {
                    if (dataGridView2.Rows[i].Cells[0].Value.ToString() == "")
                    {
                        MessageBox.Show("Unul sau mai multe cell-uri nu este complet");
                        counter = 0;
                        break;
                    }
                    if (dataGridView2.Rows[i].Cells[1].Value.ToString() == "")
                    {
                        MessageBox.Show("Unul sau mai multe cell-uri nu este complet");
                        counter = 0;
                        break;
                    }

                }
                string produse = "[";
                if (counter == 1 && dataGridView2.Rows.Count > 1)
                {
                    for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
                    {
                        string aux = "[";
                        aux += dataGridView2.Rows[i].Cells[0].Value.ToString();
                        aux += ",";
                        aux += dataGridView2.Rows[i].Cells[1].Value.ToString();
                        aux += "]";
                        aux += ",";
                        produse += aux;
                    }
                    produse = produse.TrimEnd(',');
                    produse += "]";

                    Console.WriteLine(produse);
                }


                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3000/api/comenzi/"+textBox1.Text.ToString());
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PUT";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"id_utilizator\":\"" + textBox2.Text.ToString() + "\"," +
                                  "\"id_judet\":\"" + textBox3.Text.ToString() + "\"," +
                                  "\"oras\":\"" + textBox4.Text.ToString() + "\"," +
                                  "\"strada\":\"" + textBox5.Text.ToString() + "\"," +
                                  "\"numar\":\"" + textBox6.Text.ToString() + "\"," +
                                  "\"cod_postal\":\"" + textBox7.Text.ToString() + "\"," +
                                  "\"bloc\":\"" + textBox8.Text.ToString() + "\"," +
                                  "\"scara\":\"" + textBox9.Text.ToString() + "\"," +
                                  "\"apartament\":\"" + textBox10.Text.ToString() + "\",";

                    if (produse == "[")
                    {

                        json += "\"numar_telefon\":\"" + textBox11.Text.ToString() + "\"}";
                    }
                    else
                    {
                        Console.WriteLine(produse);
                        json += "\"numar_telefon\":\"" + textBox11.Text.ToString() + "\",";
                        json += "\"produse\":" + produse + "}";
                    }
                    Console.WriteLine(json);

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

        private void button2_Click(object sender, EventArgs e)
        {
            try {

                int counter = 1;
                for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
                {
                    if (dataGridView2.Rows[i].Cells[0].Value.ToString() == "")
                    {
                        MessageBox.Show("Unul sau mai multe cell-uri nu este complet");
                        counter = 0;
                        break;
                    }
                    if (dataGridView2.Rows[i].Cells[1].Value.ToString() == "")
                    {
                        MessageBox.Show("Unul sau mai multe cell-uri nu este complet");
                        counter = 0;
                        break;
                    }

                }
                string produse = "[";
                if (counter == 1 && dataGridView2.Rows.Count > 1)
                {
                    for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
                    {
                        string aux = "[";
                        aux += dataGridView2.Rows[i].Cells[0].Value.ToString();
                        aux += ",";
                        aux += dataGridView2.Rows[i].Cells[1].Value.ToString();
                        aux += "]";
                        aux += ","; 
                        produse += aux;
                    }
                    produse = produse.TrimEnd(',');
                    produse += "]";
                    
                    Console.WriteLine(produse);
                }


                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3000/api/comenzi");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"id_utilizator\":\"" + textBox2.Text.ToString() + "\"," +
                                  "\"id_judet\":\"" + textBox3.Text.ToString() + "\"," +
                                  "\"oras\":\"" + textBox4.Text.ToString() + "\"," +
                                  "\"strada\":\"" + textBox5.Text.ToString() + "\"," +
                                  "\"numar\":\"" + textBox6.Text.ToString() + "\"," +
                                  "\"cod_postal\":\"" + textBox7.Text.ToString() + "\"," +
                                  "\"bloc\":\"" + textBox8.Text.ToString() + "\"," +
                                  "\"scara\":\"" + textBox9.Text.ToString() + "\"," +
                                  "\"apartament\":\"" + textBox10.Text.ToString() + "\",";

                    if (produse == "[")
                    {

                        json += "\"numar_telefon\":\"" + textBox11.Text.ToString() + "\"}";
                    } else
                    {
                        Console.WriteLine(produse);
                        json += "\"numar_telefon\":\"" + textBox11.Text.ToString() + "\",";
                        json+= "\"produse\":" + produse + "}";
                    }
                    Console.WriteLine(json);
                                  
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

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3000/api/comenzi/" + textBox1.Text.ToString());
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "DELETE";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

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

        private void Form9_Load(object sender, EventArgs e)
        {

        }
    }
    }

