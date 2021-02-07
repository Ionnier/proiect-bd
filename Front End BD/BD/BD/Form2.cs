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
    public partial class Form2 : Form
    {
        int id;
        public Form2(int idhere)
        {
            InitializeComponent();
            id = idhere;
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
            DataTable dt = new DataTable();
            dt = ToDataTable(new_list);
            dataGridView2.DataSource = dt;
            Thread.Sleep(1000);
        }

        List<Produse> new_list = new List<Produse>();

        public class Produse
        {
            public int id_produs { get; set; }
            public int cantitate { get; set; }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        public class Comentariu
        {
            public string utilizator { get; set; }
            public string continut { get; set; }
            public string data_publicare { get; set; }
        }

        public class Produs
        {


            public string nume { get; set; }
            public string nume_producator { get; set; }

            public double pret { get; set; }
            public string descriere { get; set; }
            public string categorie { get; set; }
            public int id { get; set; }
        }

        public void populateDGV()
        {
            string html = string.Empty;
            string url = @"http://localhost:3000/api/front";

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
            var result = JsonConvert.DeserializeObject<List<Produs>>(html, settings);

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

        private void button1_Click(object sender, EventArgs e)
        {
            Produse aux = new Produse();
            aux.id_produs = Int32.Parse(textBox1.Text);
            aux.cantitate = Int32.Parse(textBox2.Text);

            int adaugat = 0;
            foreach (var elem in new_list)
            {
                if (elem.id_produs == aux.id_produs)
                {
                    elem.cantitate += aux.cantitate;
                    adaugat = 1;
                    break;
                }


            }
            if (adaugat == 0)
            {
                new_list.Add(aux);


            }
            DataTable dt = new DataTable();
            dt = ToDataTable(new_list);
            dataGridView2.DataSource = dt;
            label13.Text = calculare();
            clear();
        }

        public void populateDGV2()
        {
            try
            {
                string html = string.Empty;
                string url = @"http://localhost:3000/api/frontcomm/" + textBox1.Text.ToString();

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
                dataGridView3.DataSource = dt;

            }
            catch { }

        }
        private void clearcheckbox()
        {

            for (int i = 0; i <= (checkedListBox1.Items.Count - 1); i++)
            {

                checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);


            }

        }

        private string calculare()
        {
            float valoare = 0;
            foreach (var elem in new_list)
            {

                
                int id_produs = elem.id_produs;
                int cantitate = elem.cantitate;


                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Int32.Parse(row.Cells[5].Value.ToString()) == id_produs)
                    {
                        valoare += (float.Parse(row.Cells[2].Value.ToString()) * cantitate);
                    }

                }
            }
            return "Pret total:" + valoare.ToString();
        }

        public class MyListBoxItem
        {
            public string id { get; set; }
            public string nume { get; set; }
        }

        List<MyListBoxItem> result;
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            cleardgv3();
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                clearcheckbox();
                textBox1.Text = row.Cells[5].Value.ToString();
                //string value2 = row.Cells[1].Value.ToString();

                string html = string.Empty;
                string url = @"http://localhost:3000/api/compatibilitateproduse/" + textBox1.Text.ToString();

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

        private void button4_Click(object sender, EventArgs e)
        {

            int id_produs = Int32.Parse(textBox1.Text);
            int i = 0;
            int schimbat = 0;
            foreach (var elem in new_list)
            {

                if (elem.id_produs == id_produs)
                {
                    schimbat = 1;
                    break;
                }
                i++;


            }
            if (schimbat == 1)
            {
                new_list.RemoveAt(i);
            }
            DataTable dt = new DataTable();
            dt = ToDataTable(new_list);
            dataGridView2.DataSource = dt;
            label13.Text = calculare();
            clear();


        }



        private void button5_Click(object sender, EventArgs e)
        {
            Produse aux = new Produse();
            aux.id_produs = Int32.Parse(textBox1.Text);
            aux.cantitate = Int32.Parse(textBox2.Text);

            int adaugat = 0;
            foreach (var elem in new_list)
            {
                if (elem.id_produs == aux.id_produs)
                {
                    elem.cantitate = aux.cantitate;
                    adaugat = 1;
                    break;
                }


            }
            if (adaugat == 0)
            {
                new_list.Add(aux);


            }
            DataTable dt = new DataTable();
            dt = ToDataTable(new_list);
            dataGridView2.DataSource = dt;
            label13.Text = calculare();
            clear();
        }

        private void clear()
        {
            clearcheckbox();
            textBox1.Text = "";
            textBox2.Text = "";

        }
        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            cleardgv3();
            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                textBox1.Text = row.Cells[0].Value.ToString();
                textBox2.Text = row.Cells[1].Value.ToString();
                //string value2 = row.Cells[1].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int counter = 1;
                for (int i = 0; i <= dataGridView2.Rows.Count - 1; i++)
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
                if (counter == 1 && dataGridView2.Rows.Count >= 1)
                {

                    for (int i = 0; i <= dataGridView2.Rows.Count - 1; i++)
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
                if (dataGridView2.Rows.Count == 0)
                {
                    MessageBox.Show("Cosul este gol");
                    return;

                }
                
                if(textBox3.Text=="" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox7.Text == "")
                {
                    MessageBox.Show("Adresa incompleta");
                    return;

                }

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3000/api/comenzi");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"id_utilizator\":\"" + id.ToString() + "\"," +
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
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3000/api/comentarii");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"id_utilizator\":\"" + id.ToString() + "\"," +
                                  "\"id_produs\":\"" + textBox1.Text.ToString() + "\"," +
                                  "\"continut_comentariu\":\"" + textBox12.Text.ToString() + "\"}";
                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
               populateDGV2();
            }
            catch (WebException ex)
            {
                string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                MessageBox.Show(message);
            }
        }

        private void cleardgv3()
        {

            DataTable dt = new DataTable();
            List<Comentariu> new_asd = new List<Comentariu>();
            dt = ToDataTable(new_asd);
            dataGridView3.DataSource = dt;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            cleardgv3();
            
            if (textBox1.Text.ToString() == "")
            {
                MessageBox.Show("Gol");
            } else
            {
                populateDGV2();

            }
        }
    }
}
